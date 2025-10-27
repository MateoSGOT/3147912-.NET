using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POO
{
    internal class Accidentes
    {
        public class Conductor
        {
            public int AnioNacimiento { get; set; }
            public int Sexo { get; set; }          // 1 = Femenino, 2 = Masculino
            public int RegistroCarro { get; set; } // 1 = Bogotá, 2 = Otras ciudades
            public int Edad { get; private set; }

            public void CalcularEdad()
            {
                int anioActual = DateTime.Now.Year;
                Edad = anioActual - AnioNacimiento;
            }
        }

        public class OficinaSeguros
        {
            private List<Conductor> conductores = new List<Conductor>();

            public void RegistrarConductor()
            {
                Console.Write("\nIngrese el año de nacimiento: ");
                int anio = int.Parse(Console.ReadLine());

                Console.Write("Ingrese el sexo (1: Femenino, 2: Masculino): ");
                int sexo = int.Parse(Console.ReadLine());

                Console.Write("Registro del carro (1: Bogotá, 2: Otras ciudades): ");
                int registro = int.Parse(Console.ReadLine());

                Conductor c = new Conductor
                {
                    AnioNacimiento = anio,
                    Sexo = sexo,
                    RegistroCarro = registro
                };

                c.CalcularEdad();
                conductores.Add(c);

                Console.WriteLine("\nConductor registrado correctamente.\n");
            }

            public void MostrarResultados()
            {
                if (conductores.Count == 0)
                {
                    Console.WriteLine("\nNo hay conductores registrados.\n");
                    return;
                }

                int menores30 = 0;
                int masculinos = 0;
                int femeninos = 0;
                int masculinos12a30 = 0;
                int fueraBogota = 0;

                foreach (var c in conductores)
                {
                    if (c.Edad < 30)
                        menores30++;

                    if (c.Sexo == 1)
                        femeninos++;
                    else if (c.Sexo == 2)
                        masculinos++;

                    if (c.Sexo == 2 && c.Edad >= 12 && c.Edad <= 30)
                        masculinos12a30++;

                    if (c.RegistroCarro == 2)
                        fueraBogota++;
                }

                double total = conductores.Count;

                double porcMenores30 = (menores30 / total) * 100;
                double porcHombres = (masculinos / total) * 100;
                double porcMujeres = (femeninos / total) * 100;
                double porcMasculinos12a30 = (masculinos12a30 / total) * 100;
                double porcFueraBogota = (fueraBogota / total) * 100;

                Console.WriteLine("\n===== RESULTADOS ESTADÍSTICOS =====");
                Console.WriteLine($"Total de conductores registrados: {conductores.Count}");
                Console.WriteLine($"Conductores menores de 30 años: {porcMenores30:F2}%");
                Console.WriteLine($"Conductores masculinos: {porcHombres:F2}%");
                Console.WriteLine($"Conductores femeninos: {porcMujeres:F2}%");
                Console.WriteLine($"Hombres entre 12 y 30 años: {porcMasculinos12a30:F2}%");
                Console.WriteLine($"Conductores con carro fuera de Bogotá: {porcFueraBogota:F2}%");
                Console.WriteLine("====================================\n");
            }
        }
    }
}
