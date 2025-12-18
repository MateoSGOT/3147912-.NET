using Agenda.Models.Agenda;
using System.ComponentModel.DataAnnotations;

namespace Agenda.Models.Agenda
{
    public class Vehiculo : BaseEntity
    {
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        [Required, MaxLength(10)]
        public string Placa { get; set; } = string.Empty;

        [MaxLength(60)]
        public string? Marca { get; set; }

        [MaxLength(60)]
        public string? LineaModelo { get; set; } // Ej: "Mazda 3", "Duster"

        public int? Anio { get; set; }

        [MaxLength(30)]
        public string? Color { get; set; }

        [MaxLength(40)]
        public string? TipoCombustible { get; set; }

        [MaxLength(40)]
        public string? Transmision { get; set; }

        public bool Activo { get; set; } = true;
    }
}
