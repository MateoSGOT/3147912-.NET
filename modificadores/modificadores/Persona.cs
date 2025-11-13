using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace modificadores
{
    internal class Persona
    {
        public string nombre {  get; set; }
        private int edad { get; set; }

        public Persona(string nombre, int edad)
        {
            this.nombre = nombre;
            this.edad = edad;
        }
        //metodo privado 
        private void MostraEdad()
        {
            Console.WriteLine($"la edad de {nombre} es: {edad}");
        }

        //metodo que contiene el privado 
        public void MostrarInformacion()
        {
            MostraEdad();
        }
    }
}
