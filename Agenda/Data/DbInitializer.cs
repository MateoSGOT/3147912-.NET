using Agenda.Models.Agenda;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AgendaDbContext db)
        {
            Console.WriteLine("🌱 SEED INICIADO");

            var now = DateTime.UtcNow;
            var user = "SEED";

            // =========================
            // BAHÍAS
            // =========================
            if (!await db.Bahias.AnyAsync())
            {
                db.Bahias.AddRange(
                    new Bahia { Nombre = "Bahía 1", Descripcion = "Mecánica general", Activa = true, CreadoEnUtc = now, CreadoPor = user },
                    new Bahia { Nombre = "Bahía 2", Descripcion = "Mecánica general", Activa = true, CreadoEnUtc = now, CreadoPor = user },
                    new Bahia { Nombre = "Elevador", Descripcion = "Servicios rápidos", Activa = true, CreadoEnUtc = now, CreadoPor = user },
                    new Bahia { Nombre = "Diagnóstico", Descripcion = "Escáner y electricidad", Activa = true, CreadoEnUtc = now, CreadoPor = user }
                );
                await db.SaveChangesAsync();
                Console.WriteLine("✔ Bahías creadas");
            }

            // =========================
            // MECÁNICOS
            // =========================
            if (!await db.Mecanicos.AnyAsync())
            {
                db.Mecanicos.AddRange(
                    new Mecanico { NombreCompleto = "Carlos Ruiz", Activo = true, CreadoEnUtc = now, CreadoPor = user },
                    new Mecanico { NombreCompleto = "Julián Pérez", Activo = true, CreadoEnUtc = now, CreadoPor = user },
                    new Mecanico { NombreCompleto = "Mateo Gómez", Activo = true, CreadoEnUtc = now, CreadoPor = user }
                );
                await db.SaveChangesAsync();
                Console.WriteLine("✔ Mecánicos creados");
            }

            // =========================
            // TIPOS DE SERVICIO
            // =========================
            if (!await db.TiposServicio.AnyAsync())
            {
                db.TiposServicio.AddRange(
                    new TipoServicio { Nombre = "Cambio de aceite", DuracionMinutosBase = 60, PrecioBase = 120000, Activo = true, CreadoEnUtc = now, CreadoPor = user },
                    new TipoServicio { Nombre = "Alineación y balanceo", DuracionMinutosBase = 90, PrecioBase = 180000, Activo = true, CreadoEnUtc = now, CreadoPor = user },
                    new TipoServicio { Nombre = "Diagnóstico escáner", DuracionMinutosBase = 45, PrecioBase = 80000, Activo = true, CreadoEnUtc = now, CreadoPor = user },
                    new TipoServicio { Nombre = "Revisión general", DuracionMinutosBase = 120, PrecioBase = 150000, Activo = true, CreadoEnUtc = now, CreadoPor = user }
                );
                await db.SaveChangesAsync();
                Console.WriteLine("✔ Tipos de servicio creados");
            }

            // =========================
            // HORARIO DEL TALLER (LUN–SÁB)
            // =========================
            if (!await db.HorariosTaller.AnyAsync())
            {
                for (int dia = 1; dia <= 6; dia++)
                {
                    db.HorariosTaller.Add(new HorarioTaller
                    {
                        DiaSemana = dia, // 1=Lunes ... 6=Sábado
                        HoraInicio = new TimeSpan(8, 0, 0),
                        HoraFin = new TimeSpan(18, 0, 0),
                        Activo = true,
                        CreadoEnUtc = now,
                        CreadoPor = user
                    });
                }
                await db.SaveChangesAsync();
                Console.WriteLine("✔ Horarios creados");
            }

            // =========================
            // CLIENTES
            // =========================
            if (!await db.Clientes.AnyAsync())
            {
                db.Clientes.AddRange(
                    new Cliente
                    {
                        NombreCompleto = "Andrés Martínez",
                        Documento = "CC1030123456",
                        Telefono = "3001112233",
                        Email = "andres@mail.com",
                        Direccion = "Cra 10 #20-30",
                        Activo = true,
                        CreadoEnUtc = now,
                        CreadoPor = user
                    },
                    new Cliente
                    {
                        NombreCompleto = "Laura Rodríguez",
                        Documento = "CC1140567890",
                        Telefono = "3014445566",
                        Email = "laura@mail.com",
                        Direccion = "Cl 5 #12-50",
                        Activo = true,
                        CreadoEnUtc = now,
                        CreadoPor = user
                    }
                );
                await db.SaveChangesAsync();
                Console.WriteLine("✔ Clientes creados");
            }

            // =========================
            // VEHÍCULOS (CRÍTICO PARA EL CREATE)
            // =========================
            if (!await db.Vehiculos.AnyAsync())
            {
                var clientes = await db.Clientes
                    .OrderBy(x => x.Id)
                    .Take(2)
                    .ToListAsync();

                if (clientes.Count == 0)
                {
                    Console.WriteLine("❌ No hay clientes para crear vehículos");
                }
                else
                {
                    db.Vehiculos.Add(new Vehiculo
                    {
                        ClienteId = clientes[0].Id,
                        Placa = "GRY123",
                        Marca = "Chevrolet",
                        LineaModelo = "Sail 2018",
                        Activo = true,
                        CreadoEnUtc = now,
                        CreadoPor = user
                    });

                    if (clientes.Count > 1)
                    {
                        db.Vehiculos.Add(new Vehiculo
                        {
                            ClienteId = clientes[1].Id,
                            Placa = "ABC456",
                            Marca = "Mazda",
                            LineaModelo = "3 2020",
                            Activo = true,
                            CreadoEnUtc = now,
                            CreadoPor = user
                        });
                    }

                    await db.SaveChangesAsync();
                    Console.WriteLine("✔ Vehículos creados");
                }
            }

            Console.WriteLine("🌱 SEED FINALIZADO");
        }
    }
}
