using Agenda.Models.Agenda;

namespace Agenda.ViewModels.Agenda
{
    public class CitaDetalleVm
    {
        public int Id { get; set; }

        public DateTime InicioLocal { get; set; }
        public DateTime FinLocal { get; set; }
        public int DuracionMinutos { get; set; }

        public string Cliente { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;

        public string Placa { get; set; } = string.Empty;
        public string? Marca { get; set; }
        public string? LineaModelo { get; set; }

        public string TipoServicio { get; set; } = string.Empty;

        public string? Mecanico { get; set; }
        public string? Bahia { get; set; }

        public EstadoCita Estado { get; set; }
        public PrioridadCita Prioridad { get; set; }

        public decimal? ValorEstimado { get; set; }
        public string? MotivoIngreso { get; set; }
        public string? NotasInternas { get; set; }

        public bool EsReprogramada { get; set; }

        // Concurrencia para acciones rápidas desde detalle
        public string RowVersionBase64 { get; set; } = string.Empty;

        // Logs
        public List<CitaLogItemVm> Logs { get; set; } = new();

        // Check-in (si lo usas)
        public CheckInVm? CheckIn { get; set; }
    }

    public class CitaLogItemVm
    {
        public DateTime FechaLocal { get; set; }
        public string Accion { get; set; } = string.Empty;
        public string? Detalle { get; set; }
        public string? Usuario { get; set; }
    }

    public class CheckInVm
    {
        public DateTime? HoraLlegadaLocal { get; set; }
        public DateTime? HoraInicioTrabajoLocal { get; set; }
        public DateTime? HoraEntregaLocal { get; set; }
        public string? ObservacionesRecepcion { get; set; }
    }
}
