using System.ComponentModel.DataAnnotations;

namespace Crud_Nativo.Models
{
    public class Producto
    {
        [Key] public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public DateTime fechaCreacion { get; set; }



    }
}
