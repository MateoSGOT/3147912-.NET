using Agenda.Models.Agenda;
using System.ComponentModel.DataAnnotations;

namespace Agenda.Models.Agenda
{
    public class CitaLog : BaseEntity
    {
        public int CitaServicioId { get; set; }
        public CitaServicio? CitaServicio { get; set; }

        [Required, MaxLength(60)]
        public string Accion { get; set; } = string.Empty; // "Reprogramada", "Confirmada", etc.

        [MaxLength(700)]
        public string? Detalle { get; set; }

        // Opcional: guardar antes/después
        public DateTime? InicioAntes { get; set; }
        public DateTime? FinAntes { get; set; }
        public DateTime? InicioDespues { get; set; }
        public DateTime? FinDespues { get; set; }
    }
}
