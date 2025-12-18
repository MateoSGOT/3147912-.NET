using Agenda.Models.Agenda;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Agenda.Data
{
    public class AgendaDbContext : DbContext
    {
        public AgendaDbContext(DbContextOptions<AgendaDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Vehiculo> Vehiculos => Set<Vehiculo>();
        public DbSet<Mecanico> Mecanicos => Set<Mecanico>();
        public DbSet<Bahia> Bahias => Set<Bahia>();
        public DbSet<TipoServicio> TiposServicio => Set<TipoServicio>();

        public DbSet<HorarioTaller> HorariosTaller => Set<HorarioTaller>();
        public DbSet<BloqueoAgenda> BloqueosAgenda => Set<BloqueoAgenda>();

        public DbSet<CitaServicio> CitasServicio => Set<CitaServicio>();
        public DbSet<CitaLog> CitasLog => Set<CitaLog>();
        public DbSet<CheckInCita> CheckInsCita => Set<CheckInCita>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===============================
            // Soft Delete Global (BaseEntity)
            // ===============================
            // Aplica el filtro a cada entidad que hereda BaseEntity (Eliminado = false)
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(BuildIsNotDeletedFilter(entityType.ClrType));
                }
            }

            // ===============================
            // CLIENTE
            // ===============================
            modelBuilder.Entity<Cliente>(e =>
            {
                e.Property(x => x.NombreCompleto).IsRequired().HasMaxLength(120);
                e.HasIndex(x => x.NombreCompleto);
                e.HasIndex(x => x.Telefono);
            });

            // ===============================
            // VEHICULO
            // ===============================
            modelBuilder.Entity<Vehiculo>(e =>
            {
                e.Property(x => x.Placa).IsRequired().HasMaxLength(10);

                // Placa única (muy importante)
                e.HasIndex(x => x.Placa).IsUnique();

                e.HasOne(x => x.Cliente)
                    .WithMany()
                    .HasForeignKey(x => x.ClienteId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ===============================
            // MECANICO y BAHIA
            // ===============================
            modelBuilder.Entity<Mecanico>(e =>
            {
                e.Property(x => x.NombreCompleto).IsRequired().HasMaxLength(120);
                e.HasIndex(x => x.NombreCompleto);
            });

            modelBuilder.Entity<Bahia>(e =>
            {
                e.Property(x => x.Nombre).IsRequired().HasMaxLength(50);
                e.HasIndex(x => x.Nombre).IsUnique();
            });

            // ===============================
            // TIPO SERVICIO
            // ===============================
            modelBuilder.Entity<TipoServicio>(e =>
            {
                e.Property(x => x.Nombre).IsRequired().HasMaxLength(90);
                e.HasIndex(x => x.Nombre).IsUnique();
            });

            // ===============================
            // HORARIO TALLER
            // ===============================
            modelBuilder.Entity<HorarioTaller>(e =>
            {
                // Un registro por día de la semana (opcional, pero recomendado)
                e.HasIndex(x => x.DiaSemana).IsUnique();

                // Validaciones no se pueden hacer como CHECK fácilmente desde EF,
                // pero estas propiedades quedan claras:
                // HoraInicio < HoraFin se valida en servicio/controlador luego.
            });

            // ===============================
            // BLOQUEOS AGENDA
            // ===============================
            modelBuilder.Entity<BloqueoAgenda>(e =>
            {
                e.Property(x => x.Motivo).IsRequired().HasMaxLength(120);

                e.HasIndex(x => new { x.Inicio, x.Fin });
                e.HasIndex(x => new { x.MecanicoId, x.Inicio, x.Fin });
                e.HasIndex(x => new { x.BahiaId, x.Inicio, x.Fin });

                e.HasOne(x => x.Mecanico)
                    .WithMany()
                    .HasForeignKey(x => x.MecanicoId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Bahia)
                    .WithMany()
                    .HasForeignKey(x => x.BahiaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ===============================
            // CITA SERVICIO (núcleo)
            // ===============================
            modelBuilder.Entity<CitaServicio>(e =>
            {
                // Guardar enums como INT
                e.Property(x => x.Estado).HasConversion<int>();
                e.Property(x => x.Prioridad).HasConversion<int>();

                // Índices para agenda (búsqueda y choques)
                e.HasIndex(x => x.Inicio);
                e.HasIndex(x => x.Fin);
                e.HasIndex(x => x.Estado);

                // Índices clave para validación de choques
                e.HasIndex(x => new { x.MecanicoId, x.Inicio, x.Fin });
                e.HasIndex(x => new { x.BahiaId, x.Inicio, x.Fin });

                e.HasOne(x => x.Vehiculo)
                    .WithMany()
                    .HasForeignKey(x => x.VehiculoId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.TipoServicio)
                    .WithMany()
                    .HasForeignKey(x => x.TipoServicioId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Mecanico)
                    .WithMany()
                    .HasForeignKey(x => x.MecanicoId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Bahia)
                    .WithMany()
                    .HasForeignKey(x => x.BahiaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ===============================
            // LOGS (historial)
            // ===============================
            modelBuilder.Entity<CitaLog>(e =>
            {
                e.Property(x => x.Accion).IsRequired().HasMaxLength(60);

                e.HasIndex(x => new { x.CitaServicioId, x.CreadoEnUtc });

                e.HasOne(x => x.CitaServicio)
                    .WithMany()
                    .HasForeignKey(x => x.CitaServicioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===============================
            // CHECK-IN (opcional pro)
            // ===============================
            modelBuilder.Entity<CheckInCita>(e =>
            {
                e.HasIndex(x => x.CitaServicioId).IsUnique();

                e.HasOne(x => x.CitaServicio)
                    .WithMany()
                    .HasForeignKey(x => x.CitaServicioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        // Helper para QueryFilter dinámico (Eliminado = false)
        private static LambdaExpression BuildIsNotDeletedFilter(Type entityType)
        {
            // x => !((BaseEntity)x).Eliminado
            var param = Expression.Parameter(entityType, "x");
            var prop = Expression.Property(Expression.Convert(param, typeof(BaseEntity)), nameof(BaseEntity.Eliminado));
            var body = Expression.Equal(prop, Expression.Constant(false));
            return Expression.Lambda(body, param);
        }
    }
}
