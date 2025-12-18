using System.ComponentModel.DataAnnotations;

namespace Agenda.ViewModels.Agenda
{
    public class ReprogramarVm
    {
        public int CitaId { get; set; }

        [Display(Name = "Nuevo inicio")]
        [Required(ErrorMessage = "El nuevo inicio es obligatorio.")]
        public string NuevoInicioLocal { get; set; } = string.Empty; // datetime-local string

        [Display(Name = "Nuevo mecánico (opcional)")]
        public int? NuevoMecanicoId { get; set; }

        [Display(Name = "Nueva bahía (opcional)")]
        public int? NuevaBahiaId { get; set; }

        [Display(Name = "Duración (min)")]
        [Range(15, 600)]
        public int? DuracionMinutosOverride { get; set; }

        [Display(Name = "Motivo de reprogramación")]
        [Required, MaxLength(200)]
        public string Motivo { get; set; } = string.Empty;

        // Concurrencia
        [Required]
        public string RowVersionBase64 { get; set; } = string.Empty;

        // SelectLists
        public List<SelectOptionVm> Mecanicos { get; set; } = new();
        public List<SelectOptionVm> Bahias { get; set; } = new();

        // Para UI info
        public string ResumenCita { get; set; } = string.Empty; // "Placa - Servicio - Fecha"
        public string? DomainError { get; set; }
        public string? DomainErrorCode { get; set; }
    }
}
