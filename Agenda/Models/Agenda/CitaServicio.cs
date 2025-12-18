using Agenda.Models.Agenda;
using System.ComponentModel.DataAnnotations;

namespace Agenda.Models.Agenda
{
    public class CitaServicio : BaseEntity
    {
        public int VehiculoId { get; set; }
        public Vehiculo? Vehiculo { get; set; }

        public int TipoServicioId { get; set; }
        public TipoServicio? TipoServicio { get; set; }

        public int? MecanicoId { get; set; }
        public Mecanico? Mecanico { get; set; }

        public int? BahiaId { get; set; }
        public Bahia? Bahia { get; set; }

        [Required]
        public DateTime Inicio { get; set; }

        [Required]
        public DateTime Fin { get; set; }

        // Guarda cuánto dura esa cita (si difiere del base)
        [Range(15, 600)]
        public int DuracionMinutos { get; set; } = 60;

        public EstadoCita Estado { get; set; } = EstadoCita.Reservada;
        public PrioridadCita Prioridad { get; set; } = PrioridadCita.Normal;

        [MaxLength(350)]
        public string? MotivoIngreso { get; set; }

        [MaxLength(500)]
        public string? NotasInternas { get; set; }

        // Ej: si se deja anticipo o cotización inicial
        [Range(0, 999999999)]
        public decimal? ValorEstimado { get; set; }

        public bool EsReprogramada { get; set; } = false;
    }
}
