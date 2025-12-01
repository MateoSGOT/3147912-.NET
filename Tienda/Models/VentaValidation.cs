using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Tienda.Models
{
    [ModelMetadataType(typeof(VentaMetadata))]
    public partial class Venta
    {
    }

    public class VentaMetadata
    {
        [Required(ErrorMessage = "Debe seleccionar un cliente")]
        public int ClienteId { get; set; }
    }
}
