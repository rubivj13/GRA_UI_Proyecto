using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu_1
{
    internal class Menu1
    {
        public void MostrarMenu()
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.SetCursorPosition(50, 6);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Menú 1 - Tema 2");

                Console.SetCursorPosition(40, 9);
                Console.WriteLine("Selecciona una opción del menú\n");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(40, 10);
                Console.WriteLine("(1)  Generar rectángulos con *");
                Console.SetCursorPosition(40, 11);
                Console.WriteLine("(2) Dibujar  barras con *");
                Console.SetCursorPosition(40, 12);
                Console.WriteLine("(3) Dibujar espiral con *");
                Console.SetCursorPosition(40, 13);
                Console.WriteLine("(4) Salir");
                Console.SetCursorPosition(40, 14);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Opción: ");

                if (int.TryParse(Console.ReadLine(), out int opcion))
                {
                    switch (opcion)
                    {
                        case 1:
                            
                            break;
                        case 2:
                            Console.Clear();
                            DibujarGrafica();
                            break;
                        case 3:
                            Console.Clear();
                            DibujarEspiral();
                            break;
                        case 4:
                            salir = true;
                            break;
                        default:
                            Console.WriteLine("Opción no válida, por favor intenta de nuevo.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Entrada no válida. Introduce un número.");
                }

                Console.WriteLine("\nPresiona una tecla para continuar...");
                Console.ReadKey();
            }
        }

        private void DibujarGrafica()
        {
            Console.Clear();
            Console.SetCursorPosition(55, 2);
            Console.WriteLine("Dibujar Gráfica");

            // Implementación de la gráfica aquí...

            Console.SetCursorPosition(45, 27);
            Console.WriteLine("Listo!!! Presiona espacio para continuar");
            Console.ReadKey();
            Console.Clear();
        }

        private void DibujarEspiral()
        {
            Console.Clear();
            Console.SetCursorPosition(55, 2);
            Console.WriteLine("Dibujar Espiral");

            // Implementación de la espiral aquí...

            Console.SetCursorPosition(45, 27);
            Console.WriteLine("Listo!!! Presiona una tecla para continuar.");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
