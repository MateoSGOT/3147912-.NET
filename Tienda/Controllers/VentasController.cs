using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tienda.Models;

namespace Tienda.Controllers
{
    [Authorize]
    public class VentasController : Controller
    {
        private readonly TechNovaDbContext _context;

        public VentasController(TechNovaDbContext context)
        {
            _context = context;
        }

        // GET: Ventas
        // Index con filtros por cliente y fecha
        public async Task<IActionResult> Index(string cliente, DateTime? fecha)
        {
            var ventas = _context.Ventas
                .Include(v => v.Cliente)
                .AsQueryable();

            if (!string.IsNullOrEmpty(cliente))
            {
                ventas = ventas.Where(v => v.Cliente.NombreCompleto.Contains(cliente));
            }

            if (fecha.HasValue)
            {
                ventas = ventas.Where(v => v.Fecha.Date == fecha.Value.Date);
            }

            return View(await ventas.ToListAsync());
        }

        // GET: Ventas/Details/5
        // Incluye cliente y detalle de venta con productos
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.DetallesVenta)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(m => m.VentaId == id);

            if (venta == null) return NotFound();

            return View(venta);
        }

        //// GET: Ventas/Create
        //// Muestra combo de clientes y lista de productos
        //public IActionResult Create()
        //{
        //    ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "NombreCompleto");
        //    ViewBag.Productos = _context.Productos.ToList();
        //    return View();
        //}

        //// POST: Ventas/Create
        //// Crea venta + detalles, calcula total y actualiza stock
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(int ClienteId, int[] ProductoId, int[] Cantidad)
        //{
        //    // Quitar validaciones sobre propiedades de navegación
        //    ModelState.Remove("Cliente");
        //    ModelState.Remove("DetallesVenta");

        //    if (ProductoId == null || ProductoId.Length == 0)
        //    {
        //        ModelState.AddModelError("", "Debe agregar al menos un producto a la venta.");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "NombreCompleto", ClienteId);
        //        ViewBag.Productos = _context.Productos.ToList();
        //        return View();
        //    }

        //    using var transaction = await _context.Database.BeginTransactionAsync();

        //    try
        //    {
        //        var venta = new Venta
        //        {
        //            ClienteId = ClienteId,
        //            Fecha = DateTime.Now,
        //            Total = 0
        //        };

        //        _context.Ventas.Add(venta);
        //        await _context.SaveChangesAsync(); // aquí ya tiene VentaId

        //        for (int i = 0; i < ProductoId.Length; i++)
        //        {
        //            var producto = await _context.Productos.FindAsync(ProductoId[i]);
        //            int cantidad = Cantidad[i];

        //            if (producto == null)
        //                throw new Exception("Producto no encontrado.");

        //            if (cantidad <= 0)
        //                throw new Exception("La cantidad debe ser mayor que cero.");

        //            if (producto.StockDisponible < cantidad)
        //                throw new Exception($"No hay stock suficiente para {producto.Nombre}");

        //            var detalle = new DetallesVentum
        //            {
        //                VentaId = venta.VentaId,
        //                ProductoId = producto.ProductoId,
        //                Cantidad = cantidad,
        //                PrecioUnitario = producto.PrecioUnitario,
        //                Subtotal = producto.PrecioUnitario * cantidad
        //            };

        //            venta.Total += detalle.Subtotal;
        //            producto.StockDisponible -= cantidad;

        //            _context.DetallesVenta.Add(detalle);
        //            _context.Productos.Update(producto);
        //        }

        //        _context.Ventas.Update(venta);
        //        await _context.SaveChangesAsync();
        //        await transaction.CommitAsync();

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        ModelState.AddModelError("", ex.Message);

        //        ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "NombreCompleto", ClienteId);
        //        ViewBag.Productos = _context.Productos.ToList();
        //        return View();
        //    }
        //}

        //// GET: Ventas/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null) return NotFound();

        //    var venta = await _context.Ventas.FindAsync(id);
        //    if (venta == null) return NotFound();

        //    ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "NombreCompleto", venta.ClienteId);
        //    return View(venta);
        //}

        //// POST: Ventas/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("VentaId,ClienteId,Fecha,Total")] Venta venta)
        //{
        //    if (id != venta.VentaId) return NotFound();

        //    ModelState.Remove("Cliente");
        //    ModelState.Remove("DetallesVenta");

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(venta);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!VentaExists(venta.VentaId))
        //                return NotFound();
        //            else
        //                throw;
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }

        //    ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "NombreCompleto", venta.ClienteId);
        //    return View(venta);
        //}

        //// GET: Ventas/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null) return NotFound();

        //    var venta = await _context.Ventas
        //        .Include(v => v.Cliente)
        //        .FirstOrDefaultAsync(m => m.VentaId == id);

        //    if (venta == null) return NotFound();

        //    return View(venta);
        //}

        //// POST: Ventas/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var venta = await _context.Ventas.FindAsync(id);
        //    if (venta != null)
        //    {
        //        _context.Ventas.Remove(venta);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool VentaExists(int id)
        //{
        //    return _context.Ventas.Any(e => e.VentaId == id);
        //}
    }
}
