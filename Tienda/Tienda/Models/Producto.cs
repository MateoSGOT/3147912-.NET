using System;
using System.Collections.Generic;

namespace Tienda.Models;

public partial class Producto
{
    public int ProductoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public decimal PrecioUnitario { get; set; }

    public int StockDisponible { get; set; }

    public string Codigo { get; set; } = null!;

    public virtual ICollection<DetallesVentum> DetallesVenta { get; set; } = new List<DetallesVentum>();
}
