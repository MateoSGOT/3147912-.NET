using Agenda.Data;
using Agenda.Models.Agenda;
using Agenda.Services;
using Agenda.ViewModels.Agenda;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Controllers
{
    public class AgendaController : Controller
    {
        private readonly AgendaDbContext _db;
        private readonly IAgendaService _agendaService;

        public AgendaController(AgendaDbContext db, IAgendaService agendaService)
        {
            _db = db;
            _agendaService = agendaService;
        }

        // =====================================================
        // INDEX – AGENDA SEMANAL + FILTROS
        // =====================================================
        public async Task<IActionResult> Index(
            DateTime? weekStart,
            int? mecanicoId,
            int? bahiaId,
            EstadoCita? estado,
            string? search)
        {
            var inicioSemana = GetMonday((weekStart ?? DateTime.Today).Date);
            var finSemana = inicioSemana.AddDays(7);

            var vm = new AgendaIndexVm
            {
                SemanaInicioLocal = inicioSemana,
                SemanaFinLocal = finSemana.AddDays(-1),
                MecanicoId = mecanicoId,
                BahiaId = bahiaId,
                Estado = estado,
                Search = search
            };

            await FillAgendaFiltersAsync(vm);

            IQueryable<CitaServicio> query = _db.CitasServicio
                .AsNoTracking()
                .Include(c => c.Vehiculo)!.ThenInclude(v => v!.Cliente)
                .Include(c => c.TipoServicio)
                .Include(c => c.Mecanico)
                .Include(c => c.Bahia)
                .Where(c => c.Inicio >= inicioSemana && c.Inicio < finSemana);

            if (mecanicoId.HasValue) query = query.Where(c => c.MecanicoId == mecanicoId.Value);
            if (bahiaId.HasValue) query = query.Where(c => c.BahiaId == bahiaId.Value);
            if (estado.HasValue) query = query.Where(c => c.Estado == estado.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(c =>
                    c.Vehiculo!.Placa.Contains(s) ||
                    c.Vehiculo.Cliente!.NombreCompleto.Contains(s) ||
                    (c.Vehiculo.Cliente.Telefono != null && c.Vehiculo.Cliente.Telefono.Contains(s))
                );
            }

            var citas = await query.OrderBy(c => c.Inicio).ToListAsync();

            vm.Citas = citas.Select(c => new CitaCardVm
            {
                Id = c.Id,
                InicioLocal = c.Inicio,
                FinLocal = c.Fin,
                Placa = c.Vehiculo!.Placa,
                Cliente = c.Vehiculo.Cliente!.NombreCompleto,
                TipoServicio = c.TipoServicio!.Nombre,
                Mecanico = c.Mecanico?.NombreCompleto,
                Bahia = c.Bahia?.Nombre,
                Estado = c.Estado,
                Prioridad = c.Prioridad,
                EsReprogramada = c.EsReprogramada
            }).ToList();

            vm.CitasPorDia = vm.Citas
                .GroupBy(x => x.InicioLocal.Date)
                .ToDictionary(g => g.Key, g => g.ToList());

            return View(vm);
        }

        // =====================================================
        // DETAILS – DETALLE + LOGS + CHECK-IN
        // =====================================================
        public async Task<IActionResult> Details(int id)
        {
            var cita = await _db.CitasServicio
                .AsNoTracking()
                .Include(c => c.Vehiculo)!.ThenInclude(v => v!.Cliente)
                .Include(c => c.TipoServicio)
                .Include(c => c.Mecanico)
                .Include(c => c.Bahia)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cita is null) return NotFound();

            var logs = await _db.CitasLog
                .AsNoTracking()
                .Where(l => l.CitaServicioId == id)
                .OrderByDescending(l => l.CreadoEnUtc)
                .ToListAsync();

            var check = await _db.CheckInsCita
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CitaServicioId == id);

            var vm = new CitaDetalleVm
            {
                Id = cita.Id,
                InicioLocal = cita.Inicio,
                FinLocal = cita.Fin,
                DuracionMinutos = cita.DuracionMinutos,

                Cliente = cita.Vehiculo!.Cliente!.NombreCompleto,
                Telefono = cita.Vehiculo.Cliente.Telefono ?? "",
                Placa = cita.Vehiculo.Placa,
                Marca = cita.Vehiculo.Marca,
                LineaModelo = cita.Vehiculo.LineaModelo,

                TipoServicio = cita.TipoServicio!.Nombre,
                Mecanico = cita.Mecanico?.NombreCompleto,
                Bahia = cita.Bahia?.Nombre,

                Estado = cita.Estado,
                Prioridad = cita.Prioridad,

                ValorEstimado = cita.ValorEstimado,
                MotivoIngreso = cita.MotivoIngreso,
                NotasInternas = cita.NotasInternas,

                EsReprogramada = cita.EsReprogramada,
                RowVersionBase64 = RowVersionHelper.ToBase64(cita.RowVersion),

                Logs = logs.Select(l => new CitaLogItemVm
                {
                    FechaLocal = l.CreadoEnUtc.ToLocalTime(),
                    Accion = l.Accion,
                    Detalle = l.Detalle,
                    Usuario = l.CreadoPor
                }).ToList(),

                CheckIn = check == null ? null : new CheckInVm
                {
                    HoraLlegadaLocal = check.HoraLlegadaUtc?.ToLocalTime(),
                    HoraInicioTrabajoLocal = check.HoraInicioTrabajoUtc?.ToLocalTime(),
                    HoraEntregaLocal = check.HoraEntregaUtc?.ToLocalTime(),
                    ObservacionesRecepcion = check.ObservacionesRecepcion
                }
            };

            return View(vm);
        }

        // =====================================================
        // CREATE – GET
        // =====================================================
        [HttpGet]
        public async Task<IActionResult> Create(DateTime? inicio)
        {
            var vm = new CitaCreateVm
            {
                InicioLocal = (inicio ?? DateTime.Today.AddHours(8))
                    .ToString(AgendaConstants.DateTimeLocalFormat)
            };

            await FillCitaFormSelectsAsync(vm);
            return View(vm);
        }

        // =====================================================
        // CREATE – POST (ServiceResult<int>)
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CitaCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                await FillCitaFormSelectsAsync(vm);
                return View(vm);
            }

            var inicioLocal = ControllerHelpers.ParseDateTimeLocal(vm.InicioLocal);

            var result = await _agendaService.CrearCitaAsync(
                new CrearCitaRequest(
                    vm.VehiculoId,
                    vm.TipoServicioId,
                    vm.MecanicoId,
                    vm.BahiaId,
                    inicioLocal,
                    vm.DuracionMinutosOverride,
                    vm.Prioridad,
                    vm.MotivoIngreso,
                    vm.NotasInternas,
                    vm.ValorEstimado,
                    User.Identity?.Name ?? "SYSTEM"
                )
            );

            if (!result.Ok)
            {
                vm.DomainError = result.Error;
                vm.DomainErrorCode = result.Code;
                ModelState.AddModelError("", result.Error ?? "No se pudo crear la cita.");
                await FillCitaFormSelectsAsync(vm);
                return View(vm);
            }

            TempData["Success"] = "Cita creada correctamente.";
            return RedirectToAction(nameof(Details), new { id = result.Data });
        }

        // =====================================================
        // EDIT – GET
        // =====================================================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cita = await _db.CitasServicio
                .AsNoTracking()
                .Include(c => c.Vehiculo)!.ThenInclude(v => v!.Cliente)
                .Include(c => c.TipoServicio)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cita is null) return NotFound();

            var vm = new CitaEditVm
            {
                Id = cita.Id,
                VehiculoId = cita.VehiculoId,
                TipoServicioId = cita.TipoServicioId,
                MecanicoId = cita.MecanicoId,
                BahiaId = cita.BahiaId,
                InicioLocal = cita.Inicio.ToString(AgendaConstants.DateTimeLocalFormat),
                DuracionMinutosOverride = cita.DuracionMinutos,
                Prioridad = cita.Prioridad,
                MotivoIngreso = cita.MotivoIngreso,
                NotasInternas = cita.NotasInternas,
                ValorEstimado = cita.ValorEstimado,
                EstadoActual = cita.Estado,
                RowVersionBase64 = RowVersionHelper.ToBase64(cita.RowVersion),

                PlacaActual = cita.Vehiculo!.Placa,
                ClienteActual = cita.Vehiculo.Cliente!.NombreCompleto,
                TipoServicioActual = cita.TipoServicio!.Nombre
            };

            await FillCitaFormSelectsAsync(vm);
            return View(vm);
        }

        // =====================================================
        // EDIT – POST (ServiceResult)
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CitaEditVm vm)
        {
            if (!ModelState.IsValid)
            {
                await FillCitaFormSelectsAsync(vm);
                return View(vm);
            }

            var inicioLocal = ControllerHelpers.ParseDateTimeLocal(vm.InicioLocal);

            var result = await _agendaService.EditarCitaAsync(
                new EditarCitaRequest(
                    vm.Id,
                    vm.VehiculoId,
                    vm.TipoServicioId,
                    vm.MecanicoId,
                    vm.BahiaId,
                    inicioLocal,
                    vm.DuracionMinutosOverride,
                    vm.Prioridad,
                    vm.MotivoIngreso,
                    vm.NotasInternas,
                    vm.ValorEstimado,
                    RowVersionHelper.FromBase64(vm.RowVersionBase64),
                    User.Identity?.Name ?? "SYSTEM"
                )
            );

            if (!result.Ok)
            {
                vm.DomainError = result.Error;
                vm.DomainErrorCode = result.Code;
                ModelState.AddModelError("", result.Error ?? "No se pudo guardar.");
                await FillCitaFormSelectsAsync(vm);
                return View(vm);
            }

            TempData["Success"] = "Cita actualizada.";
            return RedirectToAction(nameof(Details), new { id = vm.Id });
        }

        // =====================================================
        // REPROGRAMAR – GET
        // =====================================================
        [HttpGet]
        public async Task<IActionResult> Reprogramar(int id)
        {
            var cita = await _db.CitasServicio
                .AsNoTracking()
                .Include(c => c.Vehiculo)
                .Include(c => c.TipoServicio)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cita is null) return NotFound();

            var vm = new ReprogramarVm
            {
                CitaId = cita.Id,
                NuevoInicioLocal = cita.Inicio.ToString(AgendaConstants.DateTimeLocalFormat),
                NuevoMecanicoId = cita.MecanicoId,
                NuevaBahiaId = cita.BahiaId,
                DuracionMinutosOverride = cita.DuracionMinutos,
                RowVersionBase64 = RowVersionHelper.ToBase64(cita.RowVersion),
                ResumenCita = $"{cita.Vehiculo!.Placa} - {cita.TipoServicio!.Nombre} - {cita.Inicio:yyyy-MM-dd HH:mm}"
            };

            await FillReprogramarSelectsAsync(vm);
            return View(vm);
        }

        // =====================================================
        // REPROGRAMAR – POST (ServiceResult)
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reprogramar(ReprogramarVm vm)
        {
            if (!ModelState.IsValid)
            {
                await FillReprogramarSelectsAsync(vm);
                return View(vm);
            }

            var nuevoInicioLocal = ControllerHelpers.ParseDateTimeLocal(vm.NuevoInicioLocal);

            var result = await _agendaService.ReprogramarAsync(
                new ReprogramarCitaRequest(
                    vm.CitaId,
                    nuevoInicioLocal,
                    vm.NuevoMecanicoId,
                    vm.NuevaBahiaId,
                    vm.DuracionMinutosOverride,
                    vm.Motivo,
                    RowVersionHelper.FromBase64(vm.RowVersionBase64),
                    User.Identity?.Name ?? "SYSTEM"
                )
            );

            if (!result.Ok)
            {
                vm.DomainError = result.Error;
                vm.DomainErrorCode = result.Code;
                ModelState.AddModelError("", result.Error ?? "No se pudo reprogramar.");
                await FillReprogramarSelectsAsync(vm);
                return View(vm);
            }

            TempData["Success"] = "Cita reprogramada.";
            return RedirectToAction(nameof(Details), new { id = vm.CitaId });
        }

        // =====================================================
        // CAMBIAR ESTADO – GET
        // =====================================================
        [HttpGet]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var cita = await _db.CitasServicio
                .AsNoTracking()
                .Include(c => c.Vehiculo)
                .Include(c => c.TipoServicio)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cita is null) return NotFound();

            var vm = new CambiarEstadoVm
            {
                CitaId = cita.Id,
                EstadoActual = cita.Estado,
                NuevoEstado = cita.Estado,
                RowVersionBase64 = RowVersionHelper.ToBase64(cita.RowVersion),
                ResumenCita = $"{cita.Vehiculo!.Placa} - {cita.TipoServicio!.Nombre} - {cita.Inicio:yyyy-MM-dd HH:mm}",
                EstadosPermitidos = GetEstadosPermitidos(cita.Estado)
            };

            return View(vm);
        }

        // =====================================================
        // CAMBIAR ESTADO – POST (ServiceResult)
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(CambiarEstadoVm vm)
        {
            vm.EstadosPermitidos = GetEstadosPermitidos(vm.EstadoActual);

            if (!ModelState.IsValid)
                return View(vm);

            if (!vm.EstadosPermitidos.Any(x => x.Id == (int)vm.NuevoEstado))
            {
                ModelState.AddModelError("", "Transición de estado no permitida.");
                return View(vm);
            }

            var result = await _agendaService.CambiarEstadoAsync(
                new CambiarEstadoRequest(
                    vm.CitaId,
                    vm.NuevoEstado,
                    vm.Nota,
                    RowVersionHelper.FromBase64(vm.RowVersionBase64),
                    User.Identity?.Name ?? "SYSTEM"
                )
            );

            if (!result.Ok)
            {
                vm.DomainError = result.Error;
                vm.DomainErrorCode = result.Code;
                ModelState.AddModelError("", result.Error ?? "No se pudo cambiar el estado.");
                return View(vm);
            }

            TempData["Success"] = "Estado actualizado.";
            return RedirectToAction(nameof(Details), new { id = vm.CitaId });
        }

        // =====================================================
        // ELIMINAR (SOFT DELETE) – GET
        // =====================================================
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var cita = await _db.CitasServicio
                .AsNoTracking()
                .Include(c => c.Vehiculo)
                .Include(c => c.TipoServicio)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cita is null) return NotFound();

            var vm = new EliminarCitaVm
            {
                CitaId = cita.Id,
                RowVersionBase64 = RowVersionHelper.ToBase64(cita.RowVersion),
                ResumenCita = $"{cita.Vehiculo!.Placa} - {cita.TipoServicio!.Nombre} - {cita.Inicio:yyyy-MM-dd HH:mm}"
            };

            return View(vm);
        }

        // =====================================================
        // ELIMINAR (SOFT DELETE) – POST (ServiceResult)
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(EliminarCitaVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _agendaService.EliminarLogicoAsync(
                new EliminarLogicoRequest(
                    vm.CitaId,
                    vm.Motivo,
                    RowVersionHelper.FromBase64(vm.RowVersionBase64),
                    User.Identity?.Name ?? "SYSTEM"
                )
            );

            if (!result.Ok)
            {
                vm.DomainError = result.Error;
                vm.DomainErrorCode = result.Code;
                ModelState.AddModelError("", result.Error ?? "No se pudo anular la cita.");
                return View(vm);
            }

            TempData["Success"] = "Cita anulada.";
            return RedirectToAction(nameof(Index));
        }

        // =====================================================
        // ELIMINADAS – LISTADO + RESTAURAR
        // =====================================================
        public async Task<IActionResult> Eliminadas(DateTime? from, DateTime? to, string? search)
        {
            var f = (from ?? DateTime.Today.AddDays(-30)).Date;
            var t = (to ?? DateTime.Today).Date.AddDays(1);

            IQueryable<CitaServicio> q = _db.CitasServicio
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Include(c => c.Vehiculo)!.ThenInclude(v => v!.Cliente)
                .Include(c => c.TipoServicio)
                .Where(c => c.Eliminado && c.EliminadoEnUtc != null && c.EliminadoEnUtc >= f && c.EliminadoEnUtc < t);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                q = q.Where(c => c.Vehiculo!.Placa.Contains(s) || c.Vehiculo.Cliente!.NombreCompleto.Contains(s));
            }

            var list = await q.OrderByDescending(c => c.EliminadoEnUtc).Take(400).ToListAsync();

            var vm = new AgendaIndexVm
            {
                SemanaInicioLocal = f,
                SemanaFinLocal = t.AddDays(-1),
                Search = search,
                Citas = list.Select(c => new CitaCardVm
                {
                    Id = c.Id,
                    InicioLocal = c.Inicio,
                    FinLocal = c.Fin,
                    Placa = c.Vehiculo!.Placa,
                    Cliente = c.Vehiculo.Cliente!.NombreCompleto,
                    TipoServicio = c.TipoServicio!.Nombre,
                    Estado = c.Estado,
                    Prioridad = c.Prioridad,
                    EsReprogramada = c.EsReprogramada
                }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restaurar(int id)
        {
            var cita = await _db.CitasServicio.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Id == id);
            if (cita is null) return NotFound();

            cita.Eliminado = false;
            cita.EliminadoEnUtc = null;
            cita.EliminadoPor = null;
            cita.ActualizadoEnUtc = DateTime.UtcNow;
            cita.ActualizadoPor = User.Identity?.Name ?? "SYSTEM";

            _db.CitasLog.Add(new CitaLog
            {
                CitaServicioId = cita.Id,
                Accion = "Restaurada",
                Detalle = "Se restauró una cita previamente anulada.",
                CreadoPor = User.Identity?.Name ?? "SYSTEM",
                CreadoEnUtc = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();

            TempData["Success"] = "Cita restaurada.";
            return RedirectToAction(nameof(Details), new { id });
        }

        // =====================================================
        // DISPONIBILIDAD – GET/POST + JSON
        // =====================================================
        [HttpGet]
        public async Task<IActionResult> Disponibilidad()
        {
            var vm = new DisponibilidadVm
            {
                DiaLocal = DateTime.Today.ToString(AgendaConstants.DateFormat)
            };

            await FillDisponibilidadSelectsAsync(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Disponibilidad(DisponibilidadVm vm)
        {
            if (!ModelState.IsValid)
            {
                await FillDisponibilidadSelectsAsync(vm);
                return View(vm);
            }

            var dia = ControllerHelpers.ParseDateLocal(vm.DiaLocal);

            var res = await _agendaService.ObtenerSlotsDisponiblesAsync(
                dia,
                vm.MecanicoId,
                vm.BahiaId,
                vm.TipoServicioId,
                vm.DuracionMinutosOverride
            );

            if (!res.Ok)
            {
                vm.DomainError = res.Error;
                vm.DomainErrorCode = res.Code;
                await FillDisponibilidadSelectsAsync(vm);
                return View(vm);
            }

            vm.Slots = res.Data!
                .Select(s => new SlotVm
                {
                    InicioLocal = s.InicioLocal.ToString("yyyy-MM-dd HH:mm"),
                    FinLocal = s.FinLocal.ToString("yyyy-MM-dd HH:mm")
                }).ToList();

            await FillDisponibilidadSelectsAsync(vm);
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> DisponibilidadJson(
            string diaLocal,
            int tipoServicioId,
            int? mecanicoId,
            int? bahiaId,
            int? duracionMinutosOverride)
        {
            var dia = ControllerHelpers.ParseDateLocal(diaLocal);

            var res = await _agendaService.ObtenerSlotsDisponiblesAsync(
                dia,
                mecanicoId,
                bahiaId,
                tipoServicioId,
                duracionMinutosOverride
            );

            if (!res.Ok)
                return BadRequest(new { ok = false, code = res.Code, error = res.Error });

            return Ok(new
            {
                ok = true,
                slots = res.Data!.Select(x => new
                {
                    inicio = x.InicioLocal.ToString("yyyy-MM-ddTHH:mm"),
                    fin = x.FinLocal.ToString("yyyy-MM-ddTHH:mm")
                })
            });
        }

        // =====================================================
        // HELPERS
        // =====================================================
        private static DateTime GetMonday(DateTime date)
        {
            var diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-diff).Date;
        }

        private async Task FillAgendaFiltersAsync(AgendaIndexVm vm)
        {
            vm.Mecanicos = await _db.Mecanicos.AsNoTracking()
                .Where(x => x.Activo)
                .OrderBy(x => x.NombreCompleto)
                .Select(x => new SelectOptionVm { Id = x.Id, Text = x.NombreCompleto })
                .ToListAsync();

            vm.Bahias = await _db.Bahias.AsNoTracking()
                .Where(x => x.Activa)
                .OrderBy(x => x.Nombre)
                .Select(x => new SelectOptionVm { Id = x.Id, Text = x.Nombre })
                .ToListAsync();

            vm.Estados = Enum.GetValues(typeof(EstadoCita))
                .Cast<EstadoCita>()
                .Select(e => new SelectOptionVm { Id = (int)e, Text = e.ToString() })
                .ToList();
        }

        private async Task FillCitaFormSelectsAsync(CitaFormVm vm)
        {
            vm.Vehiculos = await _db.Vehiculos.AsNoTracking()
                .Include(v => v.Cliente)
                .Where(v => v.Activo)
                .OrderBy(v => v.Placa)
                .Select(v => new SelectOptionVm
                {
                    Id = v.Id,
                    Text = $"{v.Placa} - {v.Cliente!.NombreCompleto}"
                })
                .ToListAsync();

            vm.TiposServicio = await _db.TiposServicio.AsNoTracking()
                .Where(t => t.Activo)
                .OrderBy(t => t.Nombre)
                .Select(t => new SelectOptionVm
                {
                    Id = t.Id,
                    Text = $"{t.Nombre} ({t.DuracionMinutosBase} min)"
                })
                .ToListAsync();

            vm.Mecanicos = await _db.Mecanicos.AsNoTracking()
                .Where(m => m.Activo)
                .OrderBy(m => m.NombreCompleto)
                .Select(m => new SelectOptionVm { Id = m.Id, Text = m.NombreCompleto })
                .ToListAsync();

            vm.Bahias = await _db.Bahias.AsNoTracking()
                .Where(b => b.Activa)
                .OrderBy(b => b.Nombre)
                .Select(b => new SelectOptionVm { Id = b.Id, Text = b.Nombre })
                .ToListAsync();

            vm.Prioridades = Enum.GetValues(typeof(PrioridadCita))
                .Cast<PrioridadCita>()
                .Select(p => new SelectOptionVm { Id = (int)p, Text = p.ToString() })
                .ToList();
        }

        private async Task FillReprogramarSelectsAsync(ReprogramarVm vm)
        {
            vm.Mecanicos = await _db.Mecanicos.AsNoTracking()
                .Where(m => m.Activo)
                .OrderBy(m => m.NombreCompleto)
                .Select(m => new SelectOptionVm { Id = m.Id, Text = m.NombreCompleto })
                .ToListAsync();

            vm.Bahias = await _db.Bahias.AsNoTracking()
                .Where(b => b.Activa)
                .OrderBy(b => b.Nombre)
                .Select(b => new SelectOptionVm { Id = b.Id, Text = b.Nombre })
                .ToListAsync();
        }

        private async Task FillDisponibilidadSelectsAsync(DisponibilidadVm vm)
        {
            vm.TiposServicio = await _db.TiposServicio.AsNoTracking()
                .Where(x => x.Activo)
                .OrderBy(x => x.Nombre)
                .Select(x => new SelectOptionVm
                {
                    Id = x.Id,
                    Text = $"{x.Nombre} ({x.DuracionMinutosBase} min)"
                }).ToListAsync();

            vm.Mecanicos = await _db.Mecanicos.AsNoTracking()
                .Where(x => x.Activo)
                .OrderBy(x => x.NombreCompleto)
                .Select(x => new SelectOptionVm { Id = x.Id, Text = x.NombreCompleto })
                .ToListAsync();

            vm.Bahias = await _db.Bahias.AsNoTracking()
                .Where(x => x.Activa)
                .OrderBy(x => x.Nombre)
                .Select(x => new SelectOptionVm { Id = x.Id, Text = x.Nombre })
                .ToListAsync();
        }

        private static List<SelectOptionVm> GetEstadosPermitidos(EstadoCita actual)
        {
            IEnumerable<EstadoCita> allowed = actual switch
            {
                EstadoCita.Reservada => new[] { EstadoCita.Confirmada, EstadoCita.Cancelada, EstadoCita.NoAsistio },
                EstadoCita.Confirmada => new[] { EstadoCita.EnProceso, EstadoCita.Cancelada, EstadoCita.NoAsistio },
                EstadoCita.EnProceso => new[] { EstadoCita.Finalizada, EstadoCita.Cancelada },
                _ => Array.Empty<EstadoCita>()
            };

            return allowed.Select(e => new SelectOptionVm { Id = (int)e, Text = e.ToString() }).ToList();
        }
    }
}
