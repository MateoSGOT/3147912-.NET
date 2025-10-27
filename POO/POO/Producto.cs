using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POO
{
    internal class Producto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Producto(int id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public class productoCrud
        {
            public List<Producto> productos = new List<Producto>();
            public int siguienteId = 1; // inializa en uno 

            public void AgregarProducto()
            {
                Console.WriteLine("Ingrese el nombre del producto: ");
                string nombre = Console.ReadLine();
                Console.WriteLine("Ingrese el precio del producto");
                decimal precio = decimal.Parse(Console.ReadLine());

                Producto nuevoProducto = new Producto(siguienteId++, nombre, precio);
                productos.Add(nuevoProducto);
                Console.WriteLine("================ Producto Agregado  =============");
            }

            public void mostrarProducto()
            {
                Console.WriteLine("================ Lista de productos ============");
                foreach (var producto in productos)
                {
                    Console.WriteLine($"Id: {producto.Id} | Nombre: {producto.Name} | Precio: {producto.Price} ");
                }
            }

            public void actualizarProducto()
            {
                Console.WriteLine("Ingrese el ID del producto de actualizar: ");
                int idActualizar = int.Parse(Console.ReadLine());
                var producto = productos.Find(p => p.Id == idActualizar);
                if (producto != null)
                {
                    Console.WriteLine("Ingrese el nuevo nombre del producto: ");
                    producto.Name = Console.ReadLine();
                    Console.WriteLine("Ingrese el nuevo precio del producto: ");
                    producto.Price = decimal.Parse(Console.ReadLine());
                    Console.WriteLine("======================== Producto actualizado  ================");
                } else
                {
                    Console.WriteLine("[x] producto no encontrado");

                }
            }

            public void eliminarProducto()
            {
                Console.WriteLine("Ingrse el ID del producto de actualizar: ");
                int idEliminar = int.Parse(Console.ReadLine());

                var producto = productos.Find(p => p.Id == idEliminar);
                if (producto != null)
                {
                    productos.Remove(producto);
                    Console.WriteLine("============= Producto Eliminado =============");
                }
                else
                {
                    Console.WriteLine("[x] Producto no encontrado");
                }

            }
        }
    }
}
