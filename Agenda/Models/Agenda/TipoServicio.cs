using Agenda.Models.Agenda;
using System.ComponentModel.DataAnnotations;

namespace Agenda.Models.Agenda
{
    public class TipoServicio : BaseEntity
    {
        [Required, MaxLength(90)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Descripcion { get; set; }

        [Range(15, 600)]
        public int DuracionMinutosBase { get; set; } = 60;

        [Range(0, 999999999)]
        public decimal PrecioBase { get; set; } = 0;

        public bool Activo { get; set; } = true;
    }
}
