using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modificadores
{
    internal class Vehiculo
    {
        //atributos privados
        private string marca { get; set; }
        private string modelo { get; set; }
        private int kilomatraje { get; set; }

        public Vehiculo(string marca, string modelo, int kilomatraje)
        {
            this.marca = marca;
            this.modelo = modelo;
            this.kilomatraje = kilomatraje;
        }

        protected void CostoMantenimiento()
        {
           
        }

    }
}
