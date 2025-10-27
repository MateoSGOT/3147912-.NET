using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio
{
    internal class Program
    {
        static void Main(string[] args)
        {

            ///* == Ejercicio 1 */
            //Console.WriteLine("== Colilla de Pago == ");
            //Console.WriteLine("Ingresa el salario");
            //double salario = double.Parse(Console.ReadLine());
            //Console.WriteLine("Ingresa el ahorro");
            //double ahorro = double.Parse(Console.ReadLine());

            //Salud
            //double salud = salario * 0.125;
            //double pension = salario * 0.16;
            //double totalDeducciones = salud + pension + ahorro;
            //double totalRecibir = salario - totalDeducciones;


            //Console.WriteLine("\n========= COLILLA DE PAGO =========");
            //Console.WriteLine($"Salario del empleado:           {salario:C2}");
            //Console.WriteLine($"Ahorro mensual programado:      {ahorro:C2}");
            //Console.WriteLine($"Descuento por salud (12.5%):    {salud:C2}");
            //Console.WriteLine($"Descuento por pensión (16%):    {pension:C2}");
            //Console.WriteLine("---------------------------------------");
            //Console.WriteLine($"Total de deducciones:           {totalDeducciones:C2}");
            //Console.WriteLine($"TOTAL A RECIBIR:                {totalRecibir:C2}");
            //Console.WriteLine("=======================================");

            /* Ejercicio 2 */

            //Console.Write("Ingrese el valor total de la matrícula: ");
            //decimal matricula = decimal.Parse(Console.ReadLine());

            //decimal primeraCuota = matricula * 0.40m;
            //decimal segundaCuota = matricula * 0.25m;
            //decimal terceraCuota = matricula * 0.20m;
            //decimal cuartaCuota = matricula * 0.15m;



            //Console.WriteLine("Cuotas para el pago de la matricula");
            //Console.WriteLine($"Primera cuota (40%): {primeraCuota:C2}");
            //Console.WriteLine($"Segunda cuota (25%): {segundaCuota:C2}");
            //Console.WriteLine($"Tercera cuota (20%): {terceraCuota:C2}");
            //Console.WriteLine($"Cuarta cuota (15%): {cuartaCuota:C2}");



            /* Ejercicio 3 

            Console.WriteLine("===== Calcula tu edad ==========");

            Console.WriteLine("Ingresa tu año de nacimiento");
            int dateBirthday = int.Parse(Console.ReadLine());
            // nombre
            Console.WriteLine("Ingresa tu nombre");
            string Name = Console.ReadLine();
            // Direccion
            Console.WriteLine("Ingresa tu direccion");
            string adress = Console.ReadLine();

            int edad = DateTime.Now.Year - dateBirthday;


            Console.WriteLine("\n ============== Informacion Personal ===================");
            Console.WriteLine($"Tu nombre es: {Name}");
            Console.WriteLine($"Tu direccion es: {adress}");
            Console.WriteLine($"Tu edad es: {edad} Años");
            Console.WriteLine("==========================================================");
 
            */

            /*Ejercicio 4 */

            Console.WriteLine("========== Duración en llenarse el balde de agua ==========");

            Console.WriteLine("Ingresa la cantidad de litros para el balde #1 ");
            int balde1 = int.Parse(Console.ReadLine()); // litros
            Console.WriteLine("Ingresa la cantidad de litros para el balde #2 ");
            int balde2 = int.Parse(Console.ReadLine()); // litros
            Console.WriteLine("Ingresa la cantidad de litros para el balde #3 ");
            int balde3 = int.Parse(Console.ReadLine()); // litros

            int minutosPorLitro = 90; // cada litro tarda 90 minutos

            // Cálculo de duración total
            double duracion1 = balde1 * minutosPorLitro;
            double duracion2 = balde2 * minutosPorLitro;
            double duracion3 = balde3 * minutosPorLitro;

            Console.WriteLine("\n================ Duración ================");
            Console.WriteLine($"Balde1: {balde1} litros");
            Console.WriteLine($"Balde2: {balde2} litros");
            Console.WriteLine($"Balde3: {balde3} litros");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine($"El balde #1 tarda {duracion1} minutos");
            Console.WriteLine($"El balde #2 tarda {duracion2} minutos");
            Console.WriteLine($"El balde #3 tarda {duracion3} minutos");
            Console.WriteLine("==========================================");

           

            /*Ejercicio 5*/











        }
    }
}
