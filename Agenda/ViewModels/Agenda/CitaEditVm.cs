using Agenda.Models.Agenda;
using System.ComponentModel.DataAnnotations;

namespace Agenda.ViewModels.Agenda
{
    public class CitaEditVm : CitaFormVm
    {
        public int Id { get; set; }

        [Display(Name = "Estado actual")]
        public EstadoCita EstadoActual { get; set; }

        // Concurrencia
        [Required]
        public string RowVersionBase64 { get; set; } = string.Empty;

        // Datos informativos para UI
        public string PlacaActual { get; set; } = string.Empty;
        public string ClienteActual { get; set; } = string.Empty;
        public string TipoServicioActual { get; set; } = string.Empty;
    }
}
