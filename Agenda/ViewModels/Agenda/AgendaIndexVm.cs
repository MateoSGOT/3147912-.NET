using Agenda.Models.Agenda;

namespace Agenda.ViewModels.Agenda
{
    public class AgendaIndexVm
    {
        // Semana actual
        public DateTime SemanaInicioLocal { get; set; } // lunes
        public DateTime SemanaFinLocal { get; set; }    // domingo

        // Filtros
        public int? MecanicoId { get; set; }
        public int? BahiaId { get; set; }
        public EstadoCita? Estado { get; set; }
        public string? Search { get; set; } // placa, cliente, teléfono

        // Opciones filtros
        public List<SelectOptionVm> Mecanicos { get; set; } = new();
        public List<SelectOptionVm> Bahias { get; set; } = new();
        public List<SelectOptionVm> Estados { get; set; } = new();

        // Resultado
        public List<CitaCardVm> Citas { get; set; } = new();

        // Para render calendario semanal (agrupado por día)
        public Dictionary<DateTime, List<CitaCardVm>> CitasPorDia { get; set; } = new();
    }

    public class CitaCardVm
    {
        public int Id { get; set; }

        public DateTime InicioLocal { get; set; }
        public DateTime FinLocal { get; set; }

        public string Placa { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public string TipoServicio { get; set; } = string.Empty;

        public string? Mecanico { get; set; }
        public string? Bahia { get; set; }

        public EstadoCita Estado { get; set; }
        public PrioridadCita Prioridad { get; set; }

        public bool EsReprogramada { get; set; }
    }
}
