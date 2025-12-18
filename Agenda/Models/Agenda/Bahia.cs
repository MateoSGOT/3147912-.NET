using Agenda.Models.Agenda;
using System.ComponentModel.DataAnnotations;

namespace Agenda.Models.Agenda
{
    public class Bahia : BaseEntity
    {
        [Required, MaxLength(50)]
        public string Nombre { get; set; } = string.Empty; // "Bahía 1", "Elevador"

        [MaxLength(150)]
        public string? Descripcion { get; set; }

        public bool Activa { get; set; } = true;
    }
}
