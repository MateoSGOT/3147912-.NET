using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POO
{
    internal class Auto //clase Auto 
    {
        //propiedades
        public string Marca {  get; set; }
        public string Modelo { get; set; }
        public int Año {  get; set; }

        //constructor
        public Auto(string marca, string modelo, int año)
        {
            Marca = marca;
            Modelo = modelo;
            Año = año;
        }

        //Metodo 
        public void MonstrarInfo()
        {
            Console.WriteLine($"Marca: {Marca} | Modelo: {Modelo} | Año: {Año} ");
        }





    }
}
