using Agenda.Models.Agenda;
using System.ComponentModel.DataAnnotations;

namespace Agenda.Models.Agenda
{
    public class CheckInCita : BaseEntity
    {
        public int CitaServicioId { get; set; }
        public CitaServicio? CitaServicio { get; set; }

        public DateTime? HoraLlegadaUtc { get; set; }
        public DateTime? HoraInicioTrabajoUtc { get; set; }
        public DateTime? HoraEntregaUtc { get; set; }

        [MaxLength(300)]
        public string? ObservacionesRecepcion { get; set; }
    }
}
