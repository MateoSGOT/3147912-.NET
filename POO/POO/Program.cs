using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace POO
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //int[] numeros = new int[3];

            //for (int i = 0; i < 3; i++)
            //{
            //    Console.WriteLine($"Ingrese el nuemero {i + 1}:");
            //    numeros[i] = int.Parse(Console.ReadLine());

            //}
            //Console.WriteLine("\n =============== numeros ingresados =================");
            //foreach (var item in numeros) {
            //    Console.WriteLine(item);
            //}
            //int suma = 0;
            //for (int i = 0; i < 3; i++) {
            //    suma += numeros[i];
            //}

            //Console.WriteLine("\n =============== Resultado de la suma ================");
            //Console.WriteLine($"la suma es: {suma}");

            //List<int> numeros = new List<int>();
            //numeros.Add(10);
            //numeros.Add(20);
            //numeros.Add(30);
            //foreach (int i in numeros)
            //{
            //    Console.WriteLine(i);
            //}

            //acceder a un elemento de la lista
            //int numero1 = numeros[1];
            //Console.WriteLine($"El numero de la lista es: {numero1}");
            //modificar
            //numeros[2] = 50;
            //Console.WriteLine($"Numero modificado {numeros[2]}");

            //insertar
            //numeros.Insert(2, 24);
            //Console.WriteLine($"numero modificado {numeros[2]}");


            //List<int> numeros = new List<int>();

            //    while (true)
            //    {
            //        Console.WriteLine("\n======================== Menú ========================");
            //        Console.WriteLine("1. Agregar un elemento");
            //        Console.WriteLine("2. Mostrar la lista");
            //        Console.WriteLine("3. Actualizar un elemento existente");
            //        Console.WriteLine("4. Eliminar un elemento de la lista");
            //        Console.WriteLine("5. Salir");
            //        Console.Write("Ingrese la opción: ");
            //        string entrada = Console.ReadLine();

            //        if (entrada == "") 
            //        {
            //            Console.WriteLine("Debes ingresar una opción.");
            //            continue;
            //        }

            //        int opcion = int.Parse(entrada);

            //        // Menú
            //        if (opcion == 1)
            //        {
            //            Console.Write("Ingrese un valor para agregar: ");
            //            string valor = Console.ReadLine();

            //            if (valor == "")
            //            {
            //                Console.WriteLine("No puedes dejar el campo vacío.");
            //            }
            //            else
            //            {
            //                int elemento = int.Parse(valor);
            //                numeros.Add(elemento);
            //                Console.WriteLine("Elemento agregado correctamente.");
            //            }
            //        }

            //        else if (opcion == 2)
            //        {
            //            if (numeros.Count == 0)
            //            {
            //                Console.WriteLine("La lista está vacía.");
            //            }
            //            else
            //            {
            //                Console.WriteLine("Lista actual:");
            //                for (int i = 0; i < numeros.Count; i++)
            //                {
            //                    Console.WriteLine($"[{i}] -> {numeros[i]}");
            //                }
            //            }
            //        }

            //        else if (opcion == 3)
            //        {
            //            if (numeros.Count == 0)
            //            {
            //                Console.WriteLine("No hay elementos para actualizar.");
            //            }
            //            else
            //            {
            //                Console.Write("Ingrese el índice del elemento a actualizar: ");
            //                string indiceStr = Console.ReadLine();

            //                if (indiceStr == "")
            //                {
            //                    Console.WriteLine("Campo vacío. Debes ingresar un número.");
            //                }
            //                else
            //                {
            //                    int indice = int.Parse(indiceStr);

            //                    if (indice < 0 || indice >= numeros.Count)
            //                    {
            //                        Console.WriteLine("Índice fuera de rango.");
            //                    }
            //                    else
            //                    {
            //                        Console.Write("Ingrese el nuevo valor: ");
            //                        string nuevoValorStr = Console.ReadLine();

            //                        if (nuevoValorStr == "")
            //                        {
            //                            Console.WriteLine("No puedes dejar el valor vacío.");
            //                        }
            //                        else
            //                        {
            //                            int nuevoValor = int.Parse(nuevoValorStr);
            //                            numeros[indice] = nuevoValor;
            //                            Console.WriteLine("Elemento actualizado correctamente.");
            //                        }
            //                    }
            //                }
            //            }
            //        }

            //        else if (opcion == 4)
            //        {
            //            if (numeros.Count == 0)
            //            {
            //                Console.WriteLine("No hay elementos para eliminar.");
            //            }
            //            else
            //            {
            //                Console.Write("Ingrese el índice del elemento a eliminar: ");
            //                string eliminarStr = Console.ReadLine();

            //                if (eliminarStr == "")
            //                {
            //                    Console.WriteLine("Campo vacío. Debes ingresar un número.");
            //                }
            //                else
            //                {
            //                    int indiceEliminar = int.Parse(eliminarStr);

            //                    if (indiceEliminar < 0 || indiceEliminar >= numeros.Count)
            //                    {
            //                        Console.WriteLine("Índice inválido.");
            //                    }
            //                    else
            //                    {
            //                        numeros.RemoveAt(indiceEliminar);
            //                        Console.WriteLine("Elemento eliminado correctamente.");
            //                    }
            //                }
            //            }
            //        }

            //        else if (opcion == 5)
            //        {
            //            Console.WriteLine("Saliendo del programa...");
            //            break;
            //        }

            //        else
            //        {
            //            Console.WriteLine("Opción no válida. Debe estar entre 1 y 5.");
            //        }
            //    }
            //}


            ////Creamos el objeto auto

            //Auto miAuto = new Auto("Mercedes", "AMG", 2025);
            //Auto miAuto2 = new Auto("BMW", "M3 Turbo", 2020);
            //Auto miAuto3 = new Auto("Porshe", "Taycan", 2025);

            //miAuto.MonstrarInfo();
            //miAuto2.MonstrarInfo();
            //miAuto3.MonstrarInfo();

            ////editar Informacion del auto 
            //miAuto.Año = 2026;
            //miAuto.MonstrarInfo();

            //Console.WriteLine("Ingrese el nombre del estudiante");
            //string nombre = Console.ReadLine();
            //Console.WriteLine("Ingrese la edad del estudiante");
            //int edad = int.Parse(Console.ReadLine());

            //Estudiante e = new Estudiante(nombre, edad);
            //e.verificarEdad();


            //Producto.productoCrud producto = new Producto.productoCrud();
            //bool salir = false; 

            //while(!salir)
            //{
            //    Console.WriteLine("\n seleccione la opcion");
            //    Console.WriteLine("1. Agregar Producto");
            //    Console.WriteLine("2. Mostrar Productos");
            //    Console.WriteLine("3. Actualizar Producto");
            //    Console.WriteLine("4 Eliminar Producto");
            //    Console.WriteLine("5 Salir del programa");
            //    string opcion = Console.ReadLine();

            //    switch (opcion)
            //    {
            //        case "1":
            //            producto.AgregarProducto();
            //            break;
            //        case "2":
            //            producto.mostrarProducto();
            //            break;
            //        case "3":
            //            producto.actualizarProducto();
            //            break;
            //        case "4":   
            //            producto.eliminarProducto();
            //            break;
            //        case "5":
            //            Console.WriteLine("========== Saliendo ==========");
            //            salir = true;
            //            break;
            //        default:
            //            Console.WriteLine("[x] Opcion no valida. intente de nuevo");
            //            break;
            //    }
            //}


            //Persona persona = new Persona();
            //persona.AgregarPersona();

            //bool salir = false;

            //while (!salir)
            //{
            //    Console.WriteLine("=== MENÚ PRINCIPAL ===");
            //    Console.WriteLine("1. Imprimir detalles de la persona");
            //    Console.WriteLine("2. Calcular edad en días");
            //    Console.WriteLine("3. Editar información");
            //    Console.WriteLine("4. Salir");
            //    Console.Write("Seleccione una opción: ");
            //    string opcion = Console.ReadLine();

            //    switch (opcion)
            //    {
            //        case "1":
            //            persona.ImprimirDetalles();
            //            break;
            //        case "2":
            //            persona.calcularEdad();
            //            break;
            //        case "3":
            //            persona.EditarInformacion();
            //            break;
            //        case "4":
            //            salir = true;
            //            Console.WriteLine("\nSaliendo del programa...");
            //            break;
            //        default:
            //            Console.WriteLine("\nOpción inválida, intente nuevamente.\n");
            //            break;
            //    }
            //}


            //Libro.Biblioteca biblioteca = new Libro.Biblioteca();
            //bool salir = false;

            //while (!salir)
            //{
            //    Console.WriteLine("===== MENÚ BIBLIOTECA =====");
            //    Console.WriteLine("1. Agregar libro");
            //    Console.WriteLine("2. Listar libros");
            //    Console.WriteLine("3. Buscar libro");
            //    Console.WriteLine("4. Salir");
            //    Console.Write("Seleccione una opción: ");
            //    string opcion = Console.ReadLine();

            //    switch (opcion)
            //    {
            //        case "1":
            //            biblioteca.AgregarLibro();
            //            break;
            //        case "2":
            //            biblioteca.ListarLibros();
            //            break;
            //        case "3":
            //            biblioteca.BuscarLibro();
            //            break;
            //        case "4":
            //            salir = true;
            //            Console.WriteLine("Saliendo del programa...");
            //            break;
            //        default:
            //            Console.WriteLine("Opción inválida, intente de nuevo.\n");
            //            break;
            //    }
            //}



            // punto 5
            /*
            string[] programas = { "Ingeniería de Sistemas", "Psicología", "Economía", "Comunicación Social", "Administración de Empresas" };
            int[] creditos = { 20, 16, 18, 18, 20 };
            double[] descuentos = { 0.18, 0.12, 0.10, 0.05, 0.15 };
            const double VALOR_CREDITO = 200000;

            int[] conteo = new int[5];
            double totalSinDesc = 0, totalDesc = 0, totalFinal = 0;

            Console.Write("Ingrese cantidad de estudiantes: ");
            int n = int.Parse(Console.ReadLine());

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine("\nSeleccione programa:");
                for (int j = 0; j < programas.Length; j++)
                    Console.WriteLine($"{j + 1}. {programas[j]}");

                int p = int.Parse(Console.ReadLine()) - 1;
                double valor = creditos[p] * VALOR_CREDITO;

                Console.Write("Forma de pago (1: Efectivo / 2: En línea): ");
                int forma = int.Parse(Console.ReadLine());

                double desc = (forma == 1) ? valor * descuentos[p] : 0;
                double total = valor - desc;

                conteo[p]++;
                totalSinDesc += valor;
                totalDesc += desc;
                totalFinal += total;
            }

            Console.WriteLine("\n===== RESULTADOS =====");
            for (int i = 0; i < programas.Length; i++)
                Console.WriteLine($"{programas[i]}: {conteo[i]} estudiantes");
            Console.WriteLine($"Total sin descuento: ${totalSinDesc:N0}");
            Console.WriteLine($"Descuentos: ${totalDesc:N0}");
            Console.WriteLine($"Total neto: ${totalFinal:N0}");
            */

            // punto 6
            /*
            const double SUELDO_BASE = 500000;
            bool salir = false;
            List<string> empleados = new List<string>();
            List<double> ventas = new List<double>();

            while (!salir)
            {
                Console.WriteLine("\n===== COMPUTRONIC =====");
                Console.WriteLine("1. Registrar venta");
                Console.WriteLine("2. Mostrar resumen");
                Console.WriteLine("3. Salir");
                string op = Console.ReadLine();

                switch (op)
                {
                    case "1":
                        Console.Write("Nombre del empleado: ");
                        empleados.Add(Console.ReadLine());
                        Console.Write("Valor total vendido: ");
                        ventas.Add(double.Parse(Console.ReadLine()));
                        break;

                    case "2":
                        for (int i = 0; i < empleados.Count; i++)
                        {
                            double total = ventas[i];
                            double porcentaje = (total < 400000) ? 0.03 :
                                                (total < 800000) ? 0.05 : 0.10;
                            double bonificacion = total * porcentaje;
                            double pago = SUELDO_BASE + bonificacion;

                            Console.WriteLine($"\nEmpleado: {empleados[i]}");
                            Console.WriteLine($"Ventas: ${total:N0}");
                            Console.WriteLine($"Bonificación: ${bonificacion:N0}");
                            Console.WriteLine($"Pago total: ${pago:N0}");
                        }
                        break;

                    case "3":
                        salir = true;
                        break;
                }
            }
            */

            // punto 7
            /*
            Console.Write("Cantidad de conductores: ");
            int n = int.Parse(Console.ReadLine());

            int menores30 = 0, hombres = 0, mujeres = 0, hombres12a30 = 0, fueraBogota = 0;

            for (int i = 0; i < n; i++)
            {
                Console.Write("\nAño de nacimiento: ");
                int anio = int.Parse(Console.ReadLine());
                Console.Write("Sexo (1:F, 2:M): ");
                int sexo = int.Parse(Console.ReadLine());
                Console.Write("Registro del carro (1:Bogotá, 2:Otra ciudad): ");
                int reg = int.Parse(Console.ReadLine());

                int edad = DateTime.Now.Year - anio;

                if (edad < 30) menores30++;
                if (sexo == 1) mujeres++; else hombres++;
                if (sexo == 2 && edad >= 12 && edad <= 30) hombres12a30++;
                if (reg == 2) fueraBogota++;
            }

            Console.WriteLine("\n===== RESULTADOS =====");
            Console.WriteLine($"Menores de 30 años: {(double)menores30 / n * 100:F2}%");
            Console.WriteLine($"Mujeres: {(double)mujeres / n * 100:F2}%");
            Console.WriteLine($"Hombres: {(double)hombres / n * 100:F2}%");
            Console.WriteLine($"Hombres entre 12 y 30: {(double)hombres12a30 / n * 100:F2}%");
            Console.WriteLine($"Carros fuera de Bogotá: {(double)fueraBogota / n * 100:F2}%");
            */

            // punto 8
            /*
            const double BONO = 150000;
            Console.Write("Cantidad de empleados: ");
            int n = int.Parse(Console.ReadLine());

            int elegibles = 0;
            double sumaEdades = 0;
            double totalBonos = 0;
            int[] cumpleMes = new int[12];

            for (int i = 0; i < n; i++)
            {
                Console.Write("\nAño nacimiento: ");
                int anio = int.Parse(Console.ReadLine());
                Console.Write("Mes nacimiento (1-12): ");
                int mes = int.Parse(Console.ReadLine());

                int edad = DateTime.Now.Year - anio;
                sumaEdades += edad;

                if (edad >= 18 && edad < 50)
                {
                    elegibles++;
                    totalBonos += BONO;
                    cumpleMes[mes - 1]++;
                }
            }

            Console.WriteLine($"\nPromedio de edad: {sumaEdades / n:F1}");
            Console.WriteLine($"Empleados elegibles: {elegibles}");
            Console.WriteLine($"Total en bonos: ${totalBonos:N0}");

            string[] meses = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
            for (int i = 0; i < 12; i++)
            {
                if (cumpleMes[i] > 0)
                    Console.WriteLine($"{meses[i]}: {cumpleMes[i]} empleados - ${cumpleMes[i] * BONO:N0}");
            }
            */

            // punto 9
            /*
            int camiones = 0;

            while (camiones < 20)
            {
                Console.Write($"\nCapacidad del camión #{camiones + 1}: ");
                double capacidad = double.Parse(Console.ReadLine());
                double carga = 0;
                bool seguir = true;

                while (seguir)
                {
                    Console.Write("Ingrese tamaño de la saca (3000–9000): ");
                    double saca = double.Parse(Console.ReadLine());

                    if (carga + saca > capacidad)
                    {
                        Console.WriteLine("⚠️ Se excede la capacidad. Despachar camión.\n");
                        seguir = false;
                    }
                    else
                    {
                        carga += saca;
                        Console.WriteLine($"Carga actual: {carga}/{capacidad}");
                        Console.Write("¿Desea cargar otra saca? (s/n): ");
                        if (Console.ReadLine().ToLower() == "n") seguir = false;
                    }
                }

                camiones++;
                Console.Write("¿Continuar con otro camión? (s/n): ");
                if (Console.ReadLine().ToLower() == "n") break;
            }

            Console.WriteLine($"\nCamiones procesados: {camiones}");
            */
        }
    }
}