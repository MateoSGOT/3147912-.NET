using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Tienda.Models
{
    [ModelMetadataType(typeof(ProductoMetadata))]
    public partial class Producto
    {
    }

    public class ProductoMetadata
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Nombre del producto")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(200)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        [Display(Name = "Precio unitario")]
        public decimal PrecioUnitario { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        [Display(Name = "Stock disponible")]
        public int StockDisponible { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(50)]
        [Display(Name = "Código")]
        public string Codigo { get; set; }
    }
}
