using System.ComponentModel.DataAnnotations;
using Agenda.Models.Agenda;

namespace Agenda.ViewModels.Agenda
{
    public class CambiarEstadoVm
    {
        public int CitaId { get; set; }

        [Display(Name = "Estado actual")]
        public EstadoCita EstadoActual { get; set; }

        [Display(Name = "Nuevo estado")]
        [Required]
        public EstadoCita NuevoEstado { get; set; }

        [Display(Name = "Nota")]
        [MaxLength(250)]
        public string? Nota { get; set; }

        // Concurrencia
        [Required]
        public string RowVersionBase64 { get; set; } = string.Empty;

        // Opciones posibles (según reglas)
        public List<SelectOptionVm> EstadosPermitidos { get; set; } = new();

        // UI info
        public string ResumenCita { get; set; } = string.Empty;
        public string? DomainError { get; set; }
        public string? DomainErrorCode { get; set; }
    }
}
