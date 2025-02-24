using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu_1
{
    internal class Menu1
    {
        public void MostrarMenu1()
        {
            bool salir = false;

            while (!salir)
            {
                Console.SetCursorPosition(40, 6);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Menú 1. Programas de introducción.");

                Console.SetCursorPosition(40, 9);
                Console.WriteLine("Selecciona una opción del menú \n");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(40, 10);
                Console.WriteLine("(1) Generar rectángulos con *");
                Console.SetCursorPosition(40, 11);
                Console.WriteLine("(2) Dibujar barras con *");
                Console.SetCursorPosition(40, 12);
                Console.WriteLine("(3) Dibujar espiral con *");
                Console.SetCursorPosition(40, 13);
                Console.WriteLine("(4) Regresar al Menú Principal");
                Console.SetCursorPosition(40, 14);
                Console.WriteLine("(5) Salir");
                Console.SetCursorPosition(40, 15);
                Console.ForegroundColor = ConsoleColor.Blue;
                var opcion = int.Parse(Console.ReadLine());

                switch (opcion)
                {
                    case 1:
                        Console.Clear();
                        DibujarRectangulos();
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
                        return; // Regresar al Menú Principal
                    case 5:
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida, por favor intenta de nuevo.");
                        break;
                }
            }
        }

        private static void DibujarRectangulos()
        {
            Console.ForegroundColor = ConsoleColor.Green;


            int anchoConsola = Console.WindowWidth;
            int altoConsola = Console.WindowHeight;


            string mensajeTitulo = "Dibujar rectángulos";


            int posX = (anchoConsola - mensajeTitulo.Length) / 2;
            int posY = altoConsola / 2 - 10;


            Console.SetCursorPosition(posX, posY);
            Console.WriteLine(mensajeTitulo + "\n");


            (ConsoleColor color, int ancho, int alto)[] rectangulos =
            {
                (ConsoleColor.Green, 3, 1),
                (ConsoleColor.Yellow, 15, 5),
                (ConsoleColor.Red, 25, 9),
                (ConsoleColor.Blue, 35, 13),
                (ConsoleColor.Cyan, 45, 17)
            };


            int centroX = anchoConsola / 2;
            int centroY = altoConsola / 2;


            foreach (var (color, ancho, alto) in rectangulos) // Corregido
            {
                Console.ForegroundColor = color;

     
                for (int x = -ancho / 2; x <= ancho / 2; x++)
                {
                    Console.SetCursorPosition(centroX + x, centroY - alto / 2);
                    Console.Write("*");
                    Thread.Sleep(10); 
                }

                for (int y = -alto / 2 + 1; y <= alto / 2; y++)
                {
                    Console.SetCursorPosition(centroX + ancho / 2, centroY + y);
                    Console.Write("*");
                    Thread.Sleep(10); 
                }

                for (int x = ancho / 2 - 1; x >= -ancho / 2; x--)
                {
                    Console.SetCursorPosition(centroX + x, centroY + alto / 2);
                    Console.Write("*");
                    Thread.Sleep(10); 
                }

                for (int y = alto / 2 - 1; y >= -alto / 2 + 1; y--)
                {
                    Console.SetCursorPosition(centroX - ancho / 2, centroY + y);
                    Console.Write("*");
                    Thread.Sleep(10); 
                }


                Thread.Sleep(500); // Retraso entre rectángulos
            }

            Console.SetCursorPosition(45, 27);
            Console.WriteLine("Listo!!! Presiona espacio para continuar");
            Console.ReadKey();
            Console.ResetColor();
            Console.Clear();
        }

        private static void DibujarGrafica()
        {
            int x = 100;
            int y = 23;

            bool arriba = false;
            bool abajo = true;
            bool neutro = true;

            int avanzar = 0;

            Console.SetCursorPosition(55, 2);
            Console.WriteLine("Dibujar Gráfica");

            for (int i = 0; i < 258; i++)
            {
                if (avanzar < 6 && !arriba && abajo && neutro)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.SetCursorPosition(x--, y);
                    if (avanzar == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }
                    if (i == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    Console.WriteLine("*");
                    avanzar++;

                    if (avanzar == 6)
                    {
                        abajo = false;
                        neutro = false;
                        arriba = true;
                        avanzar = 0;
                    }
                    System.Threading.Thread.Sleep(40);
                }
                else if (avanzar < 15 && arriba && !abajo && !neutro)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.SetCursorPosition(x, y--);
                    Console.WriteLine("*");
                    avanzar++;
                    if (avanzar == 15)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        neutro = true;
                        avanzar = 0;
                    }
                    System.Threading.Thread.Sleep(40);
                }
                else if (avanzar < 6 && arriba && !abajo && neutro)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.SetCursorPosition(x--, y);
                    if (avanzar == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }
                    Console.WriteLine("*");
                    avanzar++;
                    if (avanzar == 6)
                    {
                        abajo = true;
                        neutro = false;
                        arriba = false;
                        avanzar = 0;
                    }
                    System.Threading.Thread.Sleep(40);
                }
                else if (avanzar < 15 && !arriba && abajo && !neutro) // Segmento vertical hacia abajo (color azul oscuro)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.SetCursorPosition(x, y++);
                    Console.WriteLine("*");
                    avanzar++;
                    if (avanzar == 15)
                    {
                        neutro = true;
                        avanzar = 0;
                    }
                    System.Threading.Thread.Sleep(40);
                }
            }

            Console.SetCursorPosition(45, 27);
            Console.WriteLine("Listo!!! Presiona espacio para continuar");
            Console.ReadKey();
            Console.Clear();
        }


        private static void DibujarEspiral()
        {
            int x = 60;
            int y = 13;
            int horizontal = 5;
            int vertical = 2;
            int pasos = 0;
            bool derecha = false;
            bool izquierda = true;
            bool arriba = false;
            bool abajo = false;
            bool neutro = true;


            ConsoleColor[] colores = { ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.Cyan };

            for (int i = 0; i < 461; i++)
            {
                if (izquierda && pasos < horizontal && neutro)
                {
                    Console.ForegroundColor = colores[i % colores.Length];
                    Console.SetCursorPosition(x--, y);
                    Console.WriteLine("*");
                    pasos++;
                    if (pasos == horizontal)
                    {
                        pasos = 0;
                        horizontal += 5;
                        arriba = true;
                        izquierda = false;
                        neutro = false;
                    }
                    System.Threading.Thread.Sleep(40);
                }
                else if (arriba && pasos < vertical && !neutro)
                {
                    Console.ForegroundColor = colores[i % colores.Length];
                    Console.SetCursorPosition(x, y--);
                    Console.WriteLine("*");
                    pasos++;
                    if (pasos == vertical)
                    {
                        pasos = 0;
                        vertical += 2;
                        derecha = true;
                        arriba = false;
                        neutro = true;
                    }
                    System.Threading.Thread.Sleep(40);
                }
                else if (derecha && pasos < horizontal && neutro)
                {
                    Console.ForegroundColor = colores[i % colores.Length];
                    Console.SetCursorPosition(x++, y);
                    Console.WriteLine("*");
                    pasos++;
                    if (pasos == horizontal)
                    {
                        pasos = 0;
                        horizontal += 5;
                        abajo = true;
                        derecha = false;
                        neutro = false;
                    }
                    System.Threading.Thread.Sleep(40);
                }
                else if (abajo && pasos < vertical && !neutro)
                {
                    Console.ForegroundColor = colores[i % colores.Length];
                    Console.SetCursorPosition(x, y++);
                    Console.WriteLine("*");
                    pasos++;
                    if (pasos == vertical)
                    {
                        pasos = 0;
                        vertical += 2;
                        izquierda = true;
                        abajo = false;
                        neutro = true;
                    }
                    System.Threading.Thread.Sleep(40);
                }
            }

            Console.SetCursorPosition(45, 27);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Listo!!! Presiona una tecla para continuar.");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }
    }
}
