using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EstructuraControl
{
    internal class Program
    {
        static void Main(string[] args)
        {
<<<<<<< HEAD
            //float sumaPonderada = 0;

            //// pesos de cada nota
            //float peso1 = 0.20f;
            //float peso2 = 0.30f;
            //float peso3 = 0.50f;

            //float nota1, nota2, nota3;

            //// --- Nota 1 ---
            //Console.WriteLine("Ingrese la nota 1 (20%):");
            //nota1 = float.Parse(Console.ReadLine());

            //if (nota1 < 0 || nota1 > 5)
            //{
            //    Console.WriteLine("Número incorrecto (debe ser entre 0 y 5).");
            //    return;
            //}
            //sumaPonderada += nota1 * peso1;

            //// --- Nota 2 ---
            //Console.WriteLine("Ingrese la nota 2 (30%):");
            //nota2 = float.Parse(Console.ReadLine());

            //if (nota2 < 0 || nota2 > 5)
            //{
            //    Console.WriteLine("Número incorrecto (debe ser entre 0 y 5).");
            //    return;
            //}
            //sumaPonderada += nota2 * peso2;

            //// --- Nota 3 ---
            //Console.WriteLine("Ingrese la nota 3 (50%):");
            //nota3 = float.Parse(Console.ReadLine());

            //if (nota3 < 0 || nota3 > 5)
            //{
            //    Console.WriteLine("Número incorrecto (debe ser entre 0 y 5).");
            //    return;
            //}
            //sumaPonderada += nota3 * peso3;

            ////---Resultado---
            //Console.WriteLine($"\nEl promedio ponderado es: {sumaPonderada:F2}");

            //if (sumaPonderada >= 3.0)
            //    Console.WriteLine("✅ ¡Aprobaste!");
            //else
            //    Console.WriteLine("❌ Reprobaste.");


            /* BANCO DE PROBLEMAS*/


=======
            float sumaPonderada = 0;

            // pesos de cada nota
            float peso1 = 0.20f;
            float peso2 = 0.30f;
            float peso3 = 0.50f;

            float nota1, nota2, nota3;

            // --- Nota 1 ---
            Console.WriteLine("Ingrese la nota 1 (20%):");
            nota1 = float.Parse(Console.ReadLine());

            if (nota1 < 0 || nota1 > 5)
            {
                Console.WriteLine("Número incorrecto (debe ser entre 0 y 5).");
                return;
            }
            sumaPonderada += nota1 * peso1;

            // --- Nota 2 ---
            Console.WriteLine("Ingrese la nota 2 (30%):");
            nota2 = float.Parse(Console.ReadLine());

            if (nota2 < 0 || nota2 > 5)
            {
                Console.WriteLine("Número incorrecto (debe ser entre 0 y 5).");
                return;
            }
            sumaPonderada += nota2 * peso2;

            // --- Nota 3 ---
            Console.WriteLine("Ingrese la nota 3 (50%):");
            nota3 = float.Parse(Console.ReadLine());

            if (nota3 < 0 || nota3 > 5)
            {
                Console.WriteLine("Número incorrecto (debe ser entre 0 y 5).");
                return;
            }
            sumaPonderada += nota3 * peso3;

            // --- Resultado ---
            Console.WriteLine($"\nEl promedio ponderado es: {sumaPonderada:F2}");

            if (sumaPonderada >= 3.0)
                Console.WriteLine("✅ ¡Aprobaste!");
            else
                Console.WriteLine("❌ Reprobaste.");
>>>>>>> 43f98047f883d0936b3718bdfd5a0a6af15330b3
        }
    }
}

