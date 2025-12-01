using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POO
{
    internal class Estudiante
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }

        //constructor 
        public Estudiante(string nombre, int edad)
        {
            Nombre = nombre;
            Edad = edad;
        }

        //metodo 
        public void verificarEdad()
        {
            if (Edad >= 18)
            {
                Console.WriteLine($"{Nombre} Eres mayor de edad");
            } else
            {
                Console.WriteLine($"{Nombre} Eres menor de edad");
            }
        }
    }
}
