using System.ComponentModel.DataAnnotations;
using Agenda.Models.Agenda;

namespace Agenda.ViewModels.Agenda
{
    public abstract class CitaFormVm
    {
        [Display(Name = "Vehículo")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecciona un vehículo.")]
        public int VehiculoId { get; set; }

        [Display(Name = "Tipo de servicio")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecciona un tipo de servicio.")]
        public int TipoServicioId { get; set; }

        [Display(Name = "Mecánico (opcional)")]
        public int? MecanicoId { get; set; }

        [Display(Name = "Bahía (opcional)")]
        public int? BahiaId { get; set; }

        [Display(Name = "Inicio")]
        [Required(ErrorMessage = "El inicio es obligatorio.")]
        public string InicioLocal { get; set; } = string.Empty; // datetime-local string (yyyy-MM-ddTHH:mm)

        [Display(Name = "Duración (min)")]
        [Range(15, 600, ErrorMessage = "La duración debe estar entre 15 y 600 minutos.")]
        public int? DuracionMinutosOverride { get; set; }

        [Display(Name = "Prioridad")]
        [Required]
        public PrioridadCita Prioridad { get; set; } = PrioridadCita.Normal;

        [Display(Name = "Motivo de ingreso")]
        [MaxLength(350)]
        public string? MotivoIngreso { get; set; }

        [Display(Name = "Notas internas")]
        [MaxLength(500)]
        public string? NotasInternas { get; set; }

        [Display(Name = "Valor estimado")]
        [Range(0, 999999999)]
        public decimal? ValorEstimado { get; set; }

        // SelectLists (se rellenan en controller)
        public List<SelectOptionVm> Vehiculos { get; set; } = new();
        public List<SelectOptionVm> TiposServicio { get; set; } = new();
        public List<SelectOptionVm> Mecanicos { get; set; } = new();
        public List<SelectOptionVm> Bahias { get; set; } = new();
        public List<SelectOptionVm> Prioridades { get; set; } = new();

        // Para mostrar errores de dominio (choques, bloqueos, etc.)
        public string? DomainError { get; set; }
        public string? DomainErrorCode { get; set; }
    }
}
