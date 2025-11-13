using Crud_Nativo.Data;
using Crud_Nativo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crud_Nativo.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Producto> listaProductos = _context.Producto;
            return View(listaProductos);
        }

        public IActionResult Create()
        {
            return View();
        }


        //Create post
        [HttpPost]
        public IActionResult Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Producto.Add(producto);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producto);
        }

        //HttpGet Edit Get
        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var producto = _context.Producto.Find(Id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        //HttpPost Edit Post
        [HttpPost]
        public IActionResult Edit(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Producto.Update(producto);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producto);


        }

        //HttpGet Delete Get
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var producto = _context.Producto.Find(Id);
            if (producto == null)
            {
                return NotFound();
            }
            return View();
        }

        //HttpPost Delete Post
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int? Id)
        {
            var producto = _context.Producto.Find(Id);
            if (producto == null)
            {
                return NotFound();
            }
            _context.Producto.Remove(producto);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }






    }
}
