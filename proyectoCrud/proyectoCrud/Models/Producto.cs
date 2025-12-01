using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace proyectoCrud.Models
{
    public class Producto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }

        // Relación con Ventas
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    }
}