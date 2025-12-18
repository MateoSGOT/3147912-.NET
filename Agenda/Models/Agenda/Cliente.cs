using Agenda.Models.Agenda;
using System.ComponentModel.DataAnnotations;

namespace Agenda.Models.Agenda
{   
    public class Cliente : BaseEntity
    {
        [Required, MaxLength(120)]
        public string NombreCompleto { get; set; } = string.Empty;

        [MaxLength(30)]
        public string? Documento { get; set; }

        [MaxLength(30)]
        public string? Telefono { get; set; }

        [MaxLength(120)]
        public string? Email { get; set; }

        [MaxLength(200)]
        public string? Direccion { get; set; }

        public bool Activo { get; set; } = true;
    }
}
