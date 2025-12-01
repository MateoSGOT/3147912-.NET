using System.ComponentModel.DataAnnotations;

namespace proyectoCrud.Models
{
    public class Venta
    {
        public int VentaId { get; set; }
        public int ClienteId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;

        // Relaciones
        public Cliente? Cliente { get; set; }
        public Producto? Producto { get; set; }
    }
}