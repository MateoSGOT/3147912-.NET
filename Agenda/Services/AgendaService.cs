using System.Globalization;
using Agenda.Data;
using Agenda.Models.Agenda;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Agenda.Services
{
    public class AgendaService : IAgendaService
    {
        private readonly AgendaDbContext _db;
        private readonly AgendaSettings _settings;
        private readonly TimeZoneInfo _tz;

        public AgendaService(AgendaDbContext db, IOptions<AgendaSettings> settings)
        {
            _db = db;
            _settings = settings.Value;
            _tz = TimeZoneInfo.FindSystemTimeZoneById(_settings.ZonaHoraria);
        }

        // ============================
        // CREATE
        // ============================
        public async Task<ServiceResult<int>> CrearCitaAsync(CrearCitaRequest req)
        {
            var vehiculo = await _db.Vehiculos.FirstOrDefaultAsync(x => x.Id == req.VehiculoId);
            if (vehiculo is null) return ServiceResult<int>.Fail("NOT_FOUND", "Vehículo no existe.");

            var tipo = await _db.TiposServicio.FirstOrDefaultAsync(x => x.Id == req.TipoServicioId);
            if (tipo is null) return ServiceResult<int>.Fail("NOT_FOUND", "Tipo de servicio no existe.");

            if (req.MecanicoId.HasValue)
            {
                var mecOk = await _db.Mecanicos.AnyAsync(x => x.Id == req.MecanicoId.Value && x.Activo);
                if (!mecOk) return ServiceResult<int>.Fail("NOT_FOUND", "Mecánico no existe o está inactivo.");
            }

            if (req.BahiaId.HasValue)
            {
                var bahOk = await _db.Bahias.AnyAsync(x => x.Id == req.BahiaId.Value && x.Activa);
                if (!bahOk) return ServiceResult<int>.Fail("NOT_FOUND", "Bahía no existe o está inactiva.");
            }

            int durMin = ResolveDuracionMinutos(tipo.DuracionMinutosBase, req.DuracionMinutosOverride);
            var inicioUtc = LocalToUtc(req.InicioLocal);
            var finUtc = inicioUtc.AddMinutes(durMin);

            var bufferMin = Math.Max(0, _settings.MinutosBufferEntreCitas);

            var rule = await ValidateAgendaRulesAsync(
                inicioUtc, finUtc, req.MecanicoId, req.BahiaId, bufferMin, citaIdExcluida: null);

            if (!rule.Ok) return ServiceResult<int>.Fail(rule.Code!, rule.Error!);

            var cita = new CitaServicio
            {
                VehiculoId = req.VehiculoId,
                TipoServicioId = req.TipoServicioId,
                MecanicoId = req.MecanicoId,
                BahiaId = req.BahiaId,

                Inicio = inicioUtc,
                Fin = finUtc,
                DuracionMinutos = durMin,

                Estado = EstadoCita.Reservada,
                Prioridad = req.Prioridad,
                MotivoIngreso = req.MotivoIngreso,
                NotasInternas = req.NotasInternas,
                ValorEstimado = req.ValorEstimado,

                CreadoPor = req.Usuario,
                CreadoEnUtc = DateTime.UtcNow
            };

            _db.CitasServicio.Add(cita);

            _db.CitasLog.Add(new CitaLog
            {
                CitaServicio = cita,
                Accion = "Creada",
                Detalle = BuildDetalleCreacion(req, durMin),
                CreadoPor = req.Usuario,
                CreadoEnUtc = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
            return ServiceResult<int>.Success(cita.Id);
        }

        // ============================
        // UPDATE
        // ============================
        public async Task<ServiceResult> EditarCitaAsync(EditarCitaRequest req)
        {
            var cita = await _db.CitasServicio.FirstOrDefaultAsync(x => x.Id == req.CitaId);
            if (cita is null) return ServiceResult.Fail("NOT_FOUND", "Cita no existe.");

            if (cita.Estado is EstadoCita.Finalizada or EstadoCita.Cancelada or EstadoCita.NoAsistio)
                return ServiceResult.Fail("INVALID_STATE", "No puedes editar una cita finalizada/cancelada/no asistió.");

            if (!req.RowVersion.SequenceEqual(cita.RowVersion ?? Array.Empty<byte>()))
                return ServiceResult.Fail("CONCURRENCY", "La cita fue modificada por otra persona. Recarga e intenta de nuevo.");

            var vehOk = await _db.Vehiculos.AnyAsync(x => x.Id == req.VehiculoId);
            if (!vehOk) return ServiceResult.Fail("NOT_FOUND", "Vehículo no existe.");

            var tipo = await _db.TiposServicio.FirstOrDefaultAsync(x => x.Id == req.TipoServicioId);
            if (tipo is null) return ServiceResult.Fail("NOT_FOUND", "Tipo de servicio no existe.");

            if (req.MecanicoId.HasValue)
            {
                var mecOk = await _db.Mecanicos.AnyAsync(x => x.Id == req.MecanicoId.Value && x.Activo);
                if (!mecOk) return ServiceResult.Fail("NOT_FOUND", "Mecánico no existe o está inactivo.");
            }

            if (req.BahiaId.HasValue)
            {
                var bahOk = await _db.Bahias.AnyAsync(x => x.Id == req.BahiaId.Value && x.Activa);
                if (!bahOk) return ServiceResult.Fail("NOT_FOUND", "Bahía no existe o está inactiva.");
            }

            int durMin = ResolveDuracionMinutos(tipo.DuracionMinutosBase, req.DuracionMinutosOverride);
            var nuevoInicioUtc = LocalToUtc(req.InicioLocal);
            var nuevoFinUtc = nuevoInicioUtc.AddMinutes(durMin);

            var bufferMin = Math.Max(0, _settings.MinutosBufferEntreCitas);

            var rule = await ValidateAgendaRulesAsync(
                nuevoInicioUtc, nuevoFinUtc, req.MecanicoId, req.BahiaId, bufferMin, citaIdExcluida: cita.Id);

            if (!rule.Ok) return rule;

            var antesInicio = cita.Inicio;
            var antesFin = cita.Fin;
            var antesMec = cita.MecanicoId;
            var antesBah = cita.BahiaId;
            var antesTipo = cita.TipoServicioId;

            cita.VehiculoId = req.VehiculoId;
            cita.TipoServicioId = req.TipoServicioId;
            cita.MecanicoId = req.MecanicoId;
            cita.BahiaId = req.BahiaId;

            cita.Inicio = nuevoInicioUtc;
            cita.Fin = nuevoFinUtc;
            cita.DuracionMinutos = durMin;

            cita.Prioridad = req.Prioridad;
            cita.MotivoIngreso = req.MotivoIngreso;
            cita.NotasInternas = req.NotasInternas;
            cita.ValorEstimado = req.ValorEstimado;

            cita.ActualizadoPor = req.Usuario;
            cita.ActualizadoEnUtc = DateTime.UtcNow;

            _db.CitasLog.Add(new CitaLog
            {
                CitaServicioId = cita.Id,
                Accion = "Editada",
                Detalle = BuildDetalleEdicion(antesInicio, antesFin, nuevoInicioUtc, nuevoFinUtc, antesMec, req.MecanicoId, antesBah, req.BahiaId, antesTipo, req.TipoServicioId),
                InicioAntes = antesInicio,
                FinAntes = antesFin,
                InicioDespues = nuevoInicioUtc,
                FinDespues = nuevoFinUtc,
                CreadoPor = req.Usuario,
                CreadoEnUtc = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
            return ServiceResult.Success();
        }

        // ============================
        // RESCHEDULE
        // ============================
        public async Task<ServiceResult> ReprogramarAsync(ReprogramarCitaRequest req)
        {
            var cita = await _db.CitasServicio.FirstOrDefaultAsync(x => x.Id == req.CitaId);
            if (cita is null) return ServiceResult.Fail("NOT_FOUND", "Cita no existe.");

            if (cita.Estado is EstadoCita.Finalizada or EstadoCita.Cancelada or EstadoCita.NoAsistio)
                return ServiceResult.Fail("INVALID_STATE", "No puedes reprogramar una cita finalizada/cancelada/no asistió.");

            if (!req.RowVersion.SequenceEqual(cita.RowVersion ?? Array.Empty<byte>()))
                return ServiceResult.Fail("CONCURRENCY", "La cita fue modificada por otra persona. Recarga e intenta de nuevo.");

            var tipo = await _db.TiposServicio.FirstOrDefaultAsync(x => x.Id == cita.TipoServicioId);
            if (tipo is null) return ServiceResult.Fail("NOT_FOUND", "Tipo de servicio de la cita no existe.");

            if (req.NuevoMecanicoId.HasValue)
            {
                var mecOk = await _db.Mecanicos.AnyAsync(x => x.Id == req.NuevoMecanicoId.Value && x.Activo);
                if (!mecOk) return ServiceResult.Fail("NOT_FOUND", "Mecánico no existe o está inactivo.");
            }

            if (req.NuevaBahiaId.HasValue)
            {
                var bahOk = await _db.Bahias.AnyAsync(x => x.Id == req.NuevaBahiaId.Value && x.Activa);
                if (!bahOk) return ServiceResult.Fail("NOT_FOUND", "Bahía no existe o está inactiva.");
            }

            int durMin = ResolveDuracionMinutos(tipo.DuracionMinutosBase, req.DuracionMinutosOverride);
            var nuevoInicioUtc = LocalToUtc(req.NuevoInicioLocal);
            var nuevoFinUtc = nuevoInicioUtc.AddMinutes(durMin);

            var bufferMin = Math.Max(0, _settings.MinutosBufferEntreCitas);

            var rule = await ValidateAgendaRulesAsync(
                nuevoInicioUtc, nuevoFinUtc,
                req.NuevoMecanicoId ?? cita.MecanicoId,
                req.NuevaBahiaId ?? cita.BahiaId,
                bufferMin,
                citaIdExcluida: cita.Id);

            if (!rule.Ok) return rule;

            var antesInicio = cita.Inicio;
            var antesFin = cita.Fin;
            var antesMec = cita.MecanicoId;
            var antesBah = cita.BahiaId;

            cita.Inicio = nuevoInicioUtc;
            cita.Fin = nuevoFinUtc;
            cita.DuracionMinutos = durMin;

            if (req.NuevoMecanicoId.HasValue) cita.MecanicoId = req.NuevoMecanicoId;
            if (req.NuevaBahiaId.HasValue) cita.BahiaId = req.NuevaBahiaId;

            cita.EsReprogramada = true;
            cita.ActualizadoPor = req.Usuario;
            cita.ActualizadoEnUtc = DateTime.UtcNow;

            _db.CitasLog.Add(new CitaLog
            {
                CitaServicioId = cita.Id,
                Accion = "Reprogramada",
                Detalle = $"Motivo: {req.Motivo}",
                InicioAntes = antesInicio,
                FinAntes = antesFin,
                InicioDespues = nuevoInicioUtc,
                FinDespues = nuevoFinUtc,
                CreadoPor = req.Usuario,
                CreadoEnUtc = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
            return ServiceResult.Success();
        }

        // ============================
        // CHANGE STATUS
        // ============================
        public async Task<ServiceResult> CambiarEstadoAsync(CambiarEstadoRequest req)
        {
            var cita = await _db.CitasServicio.FirstOrDefaultAsync(x => x.Id == req.CitaId);
            if (cita is null) return ServiceResult.Fail("NOT_FOUND", "Cita no existe.");

            if (!req.RowVersion.SequenceEqual(cita.RowVersion ?? Array.Empty<byte>()))
                return ServiceResult.Fail("CONCURRENCY", "La cita fue modificada por otra persona. Recarga e intenta de nuevo.");

            var transOk = EsTransicionValida(cita.Estado, req.NuevoEstado);
            if (!transOk)
                return ServiceResult.Fail("INVALID_TRANSITION", $"Transición inválida: {cita.Estado} → {req.NuevoEstado}");

            var antes = cita.Estado;
            cita.Estado = req.NuevoEstado;
            cita.ActualizadoPor = req.Usuario;
            cita.ActualizadoEnUtc = DateTime.UtcNow;

            _db.CitasLog.Add(new CitaLog
            {
                CitaServicioId = cita.Id,
                Accion = "CambioEstado",
                Detalle = $"Estado: {antes} → {req.NuevoEstado}. {req.Nota}".Trim(),
                CreadoPor = req.Usuario,
                CreadoEnUtc = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
            return ServiceResult.Success();
        }

        // ============================
        // SOFT DELETE
        // ============================
        public async Task<ServiceResult> EliminarLogicoAsync(EliminarLogicoRequest req)
        {
            var cita = await _db.CitasServicio.FirstOrDefaultAsync(x => x.Id == req.CitaId);
            if (cita is null) return ServiceResult.Fail("NOT_FOUND", "Cita no existe.");

            if (!req.RowVersion.SequenceEqual(cita.RowVersion ?? Array.Empty<byte>()))
                return ServiceResult.Fail("CONCURRENCY", "La cita fue modificada por otra persona. Recarga e intenta de nuevo.");

            cita.Eliminado = true;
            cita.EliminadoEnUtc = DateTime.UtcNow;
            cita.EliminadoPor = req.Usuario;

            cita.ActualizadoPor = req.Usuario;
            cita.ActualizadoEnUtc = DateTime.UtcNow;

            _db.CitasLog.Add(new CitaLog
            {
                CitaServicioId = cita.Id,
                Accion = "EliminadaLogica",
                Detalle = $"Motivo: {req.Motivo}",
                CreadoPor = req.Usuario,
                CreadoEnUtc = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
            return ServiceResult.Success();
        }

        // ============================
        // DISPONIBILIDAD
        // ============================
        public async Task<ServiceResult<List<SlotDisponible>>> ObtenerSlotsDisponiblesAsync(
            DateTime diaLocal,
            int? mecanicoId,
            int? bahiaId,
            int tipoServicioId,
            int? duracionOverrideMin)
        {
            var tipo = await _db.TiposServicio.FirstOrDefaultAsync(x => x.Id == tipoServicioId && x.Activo);
            if (tipo is null) return ServiceResult<List<SlotDisponible>>.Fail("NOT_FOUND", "Tipo de servicio no existe o inactivo.");

            if (mecanicoId.HasValue)
            {
                var mecOk = await _db.Mecanicos.AnyAsync(x => x.Id == mecanicoId.Value && x.Activo);
                if (!mecOk) return ServiceResult<List<SlotDisponible>>.Fail("NOT_FOUND", "Mecánico no existe o inactivo.");
            }

            if (bahiaId.HasValue)
            {
                var bahOk = await _db.Bahias.AnyAsync(x => x.Id == bahiaId.Value && x.Activa);
                if (!bahOk) return ServiceResult<List<SlotDisponible>>.Fail("NOT_FOUND", "Bahía no existe o inactiva.");
            }

            int durMin = ResolveDuracionMinutos(tipo.DuracionMinutosBase, duracionOverrideMin);
            var bufferMin = Math.Max(0, _settings.MinutosBufferEntreCitas);

            var (inicioDiaUtc, finDiaUtc, okHorario, errHorario) = await GetRangoHorarioDelDiaUtcAsync(diaLocal.Date);
            if (!okHorario) return ServiceResult<List<SlotDisponible>>.Fail("OUT_OF_WORKING_HOURS", errHorario!);

            var ocupaciones = await GetOcupacionesUtcAsync(inicioDiaUtc, finDiaUtc, mecanicoId, bahiaId);

            var stepMin = 15;
            var slots = new List<SlotDisponible>();

            for (var cursor = inicioDiaUtc; cursor.AddMinutes(durMin) <= finDiaUtc; cursor = cursor.AddMinutes(stepMin))
            {
                var slotInicio = cursor;
                var slotFin = cursor.AddMinutes(durMin);

                var slotInicioConBuffer = slotInicio.AddMinutes(-bufferMin);
                var slotFinConBuffer = slotFin.AddMinutes(bufferMin);

                bool choca = ocupaciones.Any(o => Overlaps(slotInicioConBuffer, slotFinConBuffer, o.InicioUtc, o.FinUtc));
                if (choca) continue;

                slots.Add(new SlotDisponible(UtcToLocal(slotInicio), UtcToLocal(slotFin)));
            }

            return ServiceResult<List<SlotDisponible>>.Success(slots);
        }

        // ==========================================================
        // Reglas avanzadas
        // ==========================================================

        private int ResolveDuracionMinutos(int duracionBase, int? overrideMin)
        {
            var dur = overrideMin.HasValue ? overrideMin.Value : duracionBase;
            if (dur < 15) dur = 15;
            if (dur > 600) dur = 600;
            return dur;
        }

        private DateTime LocalToUtc(DateTime local)
        {
            var unspecified = DateTime.SpecifyKind(local, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(unspecified, _tz);
        }

        private DateTime UtcToLocal(DateTime utc)
        {
            var specified = DateTime.SpecifyKind(utc, DateTimeKind.Utc);
            return TimeZoneInfo.ConvertTimeFromUtc(specified, _tz);
        }

        private bool EsTransicionValida(EstadoCita actual, EstadoCita nuevo)
        {
            return actual switch
            {
                EstadoCita.Reservada => nuevo is EstadoCita.Confirmada or EstadoCita.Cancelada or EstadoCita.NoAsistio,
                EstadoCita.Confirmada => nuevo is EstadoCita.EnProceso or EstadoCita.Cancelada or EstadoCita.NoAsistio,
                EstadoCita.EnProceso => nuevo is EstadoCita.Finalizada or EstadoCita.Cancelada,
                _ => false
            };
        }

        private async Task<ServiceResult> ValidateAgendaRulesAsync(
            DateTime inicioUtc,
            DateTime finUtc,
            int? mecanicoId,
            int? bahiaId,
            int bufferMin,
            int? citaIdExcluida)
        {
            if (finUtc <= inicioUtc)
                return ServiceResult.Fail("INVALID_RANGE", "La hora de fin debe ser mayor que la hora de inicio.");

            var (inicioDiaUtc, finDiaUtc, okHorario, errHorario) =
                await GetRangoHorarioDelDiaUtcAsync(UtcToLocal(inicioUtc).Date);

            if (!okHorario) return ServiceResult.Fail("OUT_OF_WORKING_HOURS", errHorario!);

            if (inicioUtc < inicioDiaUtc || finUtc > finDiaUtc)
                return ServiceResult.Fail("OUT_OF_WORKING_HOURS", "La cita está fuera del horario del taller para ese día.");

            var bloquea = await ExistsBloqueoOverlapAsync(inicioUtc, finUtc, mecanicoId, bahiaId);
            if (bloquea)
                return ServiceResult.Fail("BLOCKED", "Hay un bloqueo en ese horario (taller/bahía/mecánico).");

            var inicioConBuffer = inicioUtc.AddMinutes(-bufferMin);
            var finConBuffer = finUtc.AddMinutes(bufferMin);

            if (mecanicoId.HasValue)
            {
                var choqueMec = await ExistsCitaOverlapAsync(inicioConBuffer, finConBuffer, mecanicoId: mecanicoId.Value, bahiaId: null, citaIdExcluida);
                if (choqueMec && !_settings.PermitirSobreAgendar)
                    return ServiceResult.Fail("CONFLICT", "Choque de agenda: el mecánico ya tiene una cita en ese horario.");
            }

            if (bahiaId.HasValue)
            {
                var choqueBah = await ExistsCitaOverlapAsync(inicioConBuffer, finConBuffer, mecanicoId: null, bahiaId: bahiaId.Value, citaIdExcluida);
                if (choqueBah && !_settings.PermitirSobreAgendar)
                    return ServiceResult.Fail("CONFLICT", "Choque de agenda: la bahía ya está ocupada en ese horario.");
            }

            return ServiceResult.Success();
        }

        private async Task<(DateTime inicioDiaUtc, DateTime finDiaUtc, bool ok, string? error)> GetRangoHorarioDelDiaUtcAsync(DateTime diaLocalDate)
        {
            int diaSemana = diaLocalDate.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)diaLocalDate.DayOfWeek;

            var horario = await _db.HorariosTaller
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.DiaSemana == diaSemana && h.Activo);

            TimeSpan horaInicio;
            TimeSpan horaFin;

            if (horario is null)
            {
                if (!TimeSpan.TryParseExact(_settings.HoraInicioDefecto, @"hh\:mm", CultureInfo.InvariantCulture, out horaInicio) ||
                    !TimeSpan.TryParseExact(_settings.HoraFinDefecto, @"hh\:mm", CultureInfo.InvariantCulture, out horaFin))
                {
                    return (default, default, false, "Configuración de horario por defecto inválida en appsettings.");
                }
            }
            else
            {
                horaInicio = horario.HoraInicio;
                horaFin = horario.HoraFin;
            }

            if (horaFin <= horaInicio) return (default, default, false, "Horario del taller inválido (HoraFin <= HoraInicio).");

            var inicioLocal = diaLocalDate.Date.Add(horaInicio);
            var finLocal = diaLocalDate.Date.Add(horaFin);

            return (LocalToUtc(inicioLocal), LocalToUtc(finLocal), true, null);
        }

        // ============================
        // FIX 1: BLOQUEOS (SQL traducible)
        // ============================
        private async Task<bool> ExistsBloqueoOverlapAsync(DateTime inicioUtc, DateTime finUtc, int? mecanicoId, int? bahiaId)
        {
            return await _db.BloqueosAgenda
                .AsNoTracking()
                .AnyAsync(b =>
                    // overlap traducible:
                    (inicioUtc < b.Fin && b.Inicio < finUtc) &&
                    (
                        (b.MecanicoId == null && b.BahiaId == null) ||
                        (mecanicoId.HasValue && b.MecanicoId == mecanicoId.Value) ||
                        (bahiaId.HasValue && b.BahiaId == bahiaId.Value)
                    )
                );
        }

        // ============================
        // FIX 2: CHOQUES DE CITAS (SQL traducible)
        // ============================
        private async Task<bool> ExistsCitaOverlapAsync(DateTime inicioUtc, DateTime finUtc, int? mecanicoId, int? bahiaId, int? citaIdExcluida)
        {
            var q = _db.CitasServicio.AsNoTracking().Where(c =>
                c.Estado != EstadoCita.Cancelada &&
                c.Estado != EstadoCita.NoAsistio &&
                // overlap traducible:
                (inicioUtc < c.Fin && c.Inicio < finUtc)
            );

            if (citaIdExcluida.HasValue) q = q.Where(c => c.Id != citaIdExcluida.Value);
            if (mecanicoId.HasValue) q = q.Where(c => c.MecanicoId == mecanicoId.Value);
            if (bahiaId.HasValue) q = q.Where(c => c.BahiaId == bahiaId.Value);

            return await q.AnyAsync();
        }

        // ============================
        // Overlaps solo para memoria (slots)
        // ============================
        private static bool Overlaps(DateTime aStart, DateTime aEnd, DateTime bStart, DateTime bEnd)
            => aStart < bEnd && bStart < aEnd;

        // ============================
        // FIX 3: Ocupaciones (SQL traducible)
        // ============================
        private async Task<List<(DateTime InicioUtc, DateTime FinUtc)>> GetOcupacionesUtcAsync(
            DateTime inicioDiaUtc,
            DateTime finDiaUtc,
            int? mecanicoId,
            int? bahiaId)
        {
            var ocup = new List<(DateTime, DateTime)>();

            var citasQ = _db.CitasServicio.AsNoTracking().Where(c =>
                c.Estado != EstadoCita.Cancelada &&
                c.Estado != EstadoCita.NoAsistio &&
                (inicioDiaUtc < c.Fin && c.Inicio < finDiaUtc)
            );

            if (mecanicoId.HasValue) citasQ = citasQ.Where(c => c.MecanicoId == mecanicoId.Value);
            if (bahiaId.HasValue) citasQ = citasQ.Where(c => c.BahiaId == bahiaId.Value);

            var citas = await citasQ.Select(c => new { c.Inicio, c.Fin }).ToListAsync();
            ocup.AddRange(citas.Select(x => (x.Inicio, x.Fin)));

            var bloqQ = _db.BloqueosAgenda.AsNoTracking().Where(b =>
                (inicioDiaUtc < b.Fin && b.Inicio < finDiaUtc) &&
                (
                    (b.MecanicoId == null && b.BahiaId == null) ||
                    (mecanicoId.HasValue && b.MecanicoId == mecanicoId.Value) ||
                    (bahiaId.HasValue && b.BahiaId == bahiaId.Value)
                )
            );

            var bloq = await bloqQ.Select(b => new { b.Inicio, b.Fin }).ToListAsync();
            ocup.AddRange(bloq.Select(x => (x.Inicio, x.Fin)));

            ocup.Sort((x, y) => x.Item1.CompareTo(y.Item1));
            return ocup.Select(x => (x.Item1, x.Item2)).ToList();
        }

        private static string BuildDetalleCreacion(CrearCitaRequest req, int durMin)
        {
            return $"TipoServicioId={req.TipoServicioId}, VehiculoId={req.VehiculoId}, Mec={req.MecanicoId}, Bah={req.BahiaId}, " +
                   $"InicioLocal={req.InicioLocal:yyyy-MM-dd HH:mm}, DurMin={durMin}, Prioridad={req.Prioridad}.";
        }

        private static string BuildDetalleEdicion(
            DateTime antesInicio, DateTime antesFin, DateTime nuevoInicio, DateTime nuevoFin,
            int? antesMec, int? nuevoMec,
            int? antesBah, int? nuevoBah,
            int antesTipo, int nuevoTipo)
        {
            return $"Cambio: Inicio {antesInicio:o}→{nuevoInicio:o}, Fin {antesFin:o}→{nuevoFin:o}, " +
                   $"Mec {antesMec}→{nuevoMec}, Bah {antesBah}→{nuevoBah}, Tipo {antesTipo}→{nuevoTipo}.";
        }
    }
}
