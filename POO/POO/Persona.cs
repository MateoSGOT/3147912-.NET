using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POO
{
    internal class Persona
    {
        // Objetos
        public string Nombre { get; set; }
        public string Genero { get; set; }
        public int Edad { get; set; }
        public string Telefono { get; set; }

        //contructor
        public Persona()
        {
            Nombre = "";
            Edad = 0;
            Genero = "";
            Telefono = "";
        }

        // Metodo para agregar personas
        public void AgregarPersona()
        {
            Console.WriteLine("=== REGISTRO DE NUEVA PERSONA ===");
            Console.Write("Ingrese el nombre: ");
            Nombre = Console.ReadLine();

            Console.Write("Ingresa la edad: ");
            Edad = int.Parse(Console.ReadLine());

            Console.Write("Ingrsa el genero (F/M)");
            Genero = Console.ReadLine();

            Console.Write("Ingresa el telefono: ");
            Telefono = Console.ReadLine();

            Console.WriteLine("\nPersona registrada correctamente.");
        }


        // Metodo para Imprimir 
        public void ImprimirDetalles()
        {
            Console.WriteLine("\n=== DETALLES DE LA PERSONA ===");
            Console.WriteLine($"Nombre: {Nombre}");
            Console.WriteLine($"Edad: {Edad} años");
            Console.WriteLine($"Género: {Genero}");
            Console.WriteLine($"Teléfono: {Telefono}");
            Console.WriteLine("==============================\n");
        }


        //metodo para editar la informacion

        public void EditarInformacion()
        {
            Console.WriteLine("\n=== EDITAR INFORMACIÓN ===");
            Console.Write("Nuevo nombre: ");
            Nombre = Console.ReadLine();

            Console.Write("Nueva edad: ");
            Edad = int.Parse(Console.ReadLine());

            Console.Write("Nuevo género (F/M): ");
            Genero = Console.ReadLine();

            Console.Write("Nuevo teléfono: ");
            Telefono = Console.ReadLine();

            Console.WriteLine("\nInformación actualizada correctamente.\n");
        }

        //metodo para calcular edad en dias 

        public void calcularEdad()
        {
            int edadEnDias = Edad * 365;
            Console.WriteLine($"\n{Nombre} tiene aproximadamente {edadEnDias} días de vida.\n");
        }



    }
}
