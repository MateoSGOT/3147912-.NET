using Agenda.Models.Agenda;
using System.ComponentModel.DataAnnotations;

namespace Agenda.Models.Agenda
{
    public class HorarioTaller : BaseEntity
    {
        // 1 = Lunes ... 7 = Domingo (o usa DayOfWeek si prefieres)
        [Range(1, 7)]
        public int DiaSemana { get; set; }

        // Hora de apertura/cierre (solo hora, sin fecha)
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }

        public bool Activo { get; set; } = true;
    }
}
