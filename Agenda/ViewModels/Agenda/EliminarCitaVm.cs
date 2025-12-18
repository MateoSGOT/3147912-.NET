using System.ComponentModel.DataAnnotations;

namespace Agenda.ViewModels.Agenda
{
    public class EliminarCitaVm
    {
        public int CitaId { get; set; }

        [Display(Name = "Motivo de anulación")]
        [Required, MaxLength(200)]
        public string Motivo { get; set; } = string.Empty;

        [Required]
        public string RowVersionBase64 { get; set; } = string.Empty;

        public string ResumenCita { get; set; } = string.Empty;

        public string? DomainError { get; set; }
        public string? DomainErrorCode { get; set; }
    }
}
