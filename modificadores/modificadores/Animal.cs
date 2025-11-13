using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modificadores
{
    internal class Animal
    {
        
        protected string especie {  get; set; } //atributo protegido

        //constructor
        public Animal(string especie)
        {
            this.especie = especie;
        }

        protected string MostrarEspecie() // metodo protegido
        {
            return $"La especie del animal es: {especie}.";
        }

        public class Perro : Animal
        {
            public string nombre { get; set; }
            public Perro(string especie) : base(especie)
            {
                this.nombre = nombre;
            }

            public void MostrarInfo()
            {

               Console.WriteLine($"{ MostrarEspecie() } y su nombre es {nombre}");
            }
        }
    }
}
