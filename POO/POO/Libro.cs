using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POO
{
    internal class Libro
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Editorial { get; set; }
        public int AnioPublicacion { get; set; }

        public Libro(string titulo, string autor, string editorial, int anioPublicacion)
        {
            Titulo = titulo;
            Autor = autor;
            Editorial = editorial;
            AnioPublicacion = anioPublicacion;
        }

        public void ImprimirDetalles()
        {
            Console.WriteLine("\n===== INFORMACIÓN DEL LIBRO =====");
            Console.WriteLine($"Título: {Titulo}");
            Console.WriteLine($"Autor: {Autor}");
            Console.WriteLine($"Editorial: {Editorial}");
            Console.WriteLine($"Año de publicación: {AnioPublicacion}");
            Console.WriteLine("=================================\n");
        }

        public class Biblioteca
        {
            private List<Libro> libros = new List<Libro>();

            public void AgregarLibro()
            {
                Console.WriteLine("=== AGREGAR NUEVO LIBRO ===");
                Console.Write("Ingrese el título: ");
                string titulo = Console.ReadLine();

                Console.Write("Ingrese el autor: ");
                string autor = Console.ReadLine();

                Console.Write("Ingrese la editorial: ");
                string editorial = Console.ReadLine();

                Console.Write("Ingrese el año de publicación: ");
                int anio = int.Parse(Console.ReadLine());

                libros.Add(new Libro(titulo, autor, editorial, anio));
                Console.WriteLine("\nLibro agregado correctamente.\n");
            }

            public void ListarLibros()
            {
                Console.WriteLine("\n=== LISTADO DE LIBROS ===");
                if (libros.Count == 0)
                {
                    Console.WriteLine("No hay libros registrados.\n");
                }
                else
                {
                    foreach (var libro in libros)
                    {
                        libro.ImprimirDetalles();
                    }
                }
            }

            public void BuscarLibro()
            {
                Console.Write("Ingrese el título del libro que desea buscar: ");
                string busqueda = Console.ReadLine().Trim().ToLower();

                bool encontrado = false;
                foreach (var libro in libros)
                {
                    if (libro.Titulo.Equals(busqueda, StringComparison.OrdinalIgnoreCase))
                    {
                        libro.ImprimirDetalles();
                        encontrado = true;
                    }
                }

                if (!encontrado)
                {
                    Console.WriteLine("\nNo se encontró un libro con ese título.\n");
                }
            }
        }
    }
}
