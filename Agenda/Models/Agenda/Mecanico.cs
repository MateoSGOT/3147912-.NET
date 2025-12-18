using Agenda.Models.Agenda;
using System.ComponentModel.DataAnnotations;

namespace Agenda.Models.Agenda
{
    public class Mecanico : BaseEntity
    {
        [Required, MaxLength(120)]
        public string NombreCompleto { get; set; } = string.Empty;

        [MaxLength(30)]
        public string? Documento { get; set; }

        [MaxLength(30)]
        public string? Telefono { get; set; }

        public bool Activo { get; set; } = true;
    }
}
