using Agenda.Models.Agenda;
using System.ComponentModel.DataAnnotations;

namespace Agenda.Models.Agenda
{
    public class BloqueoAgenda : BaseEntity
    {
        [Required]
        public DateTime Inicio { get; set; }

        [Required]
        public DateTime Fin { get; set; }

        // opcional: bloquear por mecánico o bahía específicos
        public int? MecanicoId { get; set; }
        public Mecanico? Mecanico { get; set; }

        public int? BahiaId { get; set; }
        public Bahia? Bahia { get; set; }

        [Required, MaxLength(120)]
        public string Motivo { get; set; } = string.Empty;
    }
}
