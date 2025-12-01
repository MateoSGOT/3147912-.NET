using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace proyectoCrud.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Relación con Ventas
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    }
}