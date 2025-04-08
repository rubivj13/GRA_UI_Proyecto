using System;
using System.Threading;

namespace MenuLocalizacionVM
{
    public class SumaFilasColumnas
    {
        private int[,] matriz;
        private int[] sumaFilas;
        private int[] sumaColumnas;
        private bool salir;

        public SumaFilasColumnas()
        {
            matriz = new int[3, 3];
            sumaFilas = new int[3];
            sumaColumnas = new int[3];
            salir = false;
        }

        public void Ejecutar()
        {
            while (!salir)
            {
                Console.Clear();
                DibujarMarco();
                LlenarMatriz();
                CalcularSumas();
                MostrarResultados();
                MostrarMenuFinal();
            }
        }

        private void DibujarMarco()
        {
            // Dibujar marco principal
            int ancho = 60;
            int alto = 20;

            Console.ForegroundColor = ConsoleColor.DarkCyan;

            // Parte superior
            Console.SetCursorPosition(10, 1);
            Console.Write("╔");
            for (int i = 0; i < ancho; i++)
                Console.Write("═");
            Console.Write("╗");

            // Lados
            for (int i = 0; i < alto; i++)
            {
                Console.SetCursorPosition(10, 2 + i);
                Console.Write("║");
                Console.SetCursorPosition(10 + ancho + 1, 2 + i);
                Console.Write("║");
            }

            // Parte inferior
            Console.SetCursorPosition(10, 2 + alto);
            Console.Write("╚");
            for (int i = 0; i < ancho; i++)
                Console.Write("═");
            Console.Write("╝");

            // Título
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(20, 2);
            Console.WriteLine("▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
            Console.SetCursorPosition(20, 3);
            Console.WriteLine("▓      SUMA DE FILAS Y COLUMNAS      ▓");
            Console.SetCursorPosition(20, 4);
            Console.WriteLine("▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
        }

        private void LlenarMatriz()
        {
            // Instrucciones
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(15, 6);
            Console.WriteLine("Por favor, ingrese los valores para la matriz 3x3:");

            // Dibujar tabla de matriz
            DibujarTablaMatriz();

            // Llenar matriz
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Coordenadas para la celda actual
                    int x = 22 + j * 6;
                    int y = 9 + i * 2;

                    Console.SetCursorPosition(x, y);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.CursorVisible = true;

                    bool valorValido = false;
                    while (!valorValido)
                    {
                        string input = Console.ReadLine();
                        if (int.TryParse(input, out int valor))
                        {
                            matriz[i, j] = valor;
                            valorValido = true;
                        }
                        else
                        {
                            Console.SetCursorPosition(x, y);
                            Console.Write("   ");
                            Console.SetCursorPosition(x, y);
                        }
                    }

                    // Mostrar valor ingresado con formato
                    MostrarValorMatriz(i, j);
                    Thread.Sleep(200);
                }
            }
            Console.CursorVisible = false;
        }

        private void DibujarTablaMatriz()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;

            // Borde superior
            Console.SetCursorPosition(20, 8);
            Console.Write("┌─────┬─────┬─────┐");

            // Filas intermedias
            for (int i = 0; i < 3; i++)
            {
                // Celdas
                Console.SetCursorPosition(20, 9 + i * 2);
                Console.Write("│     │     │     │");

                // Separadores horizontales (excepto después de la última fila)
                if (i < 2)
                {
                    Console.SetCursorPosition(20, 10 + i * 2);
                    Console.Write("├─────┼─────┼─────┤");
                }
            }

            // Borde inferior
            Console.SetCursorPosition(20, 14);
            Console.Write("└─────┴─────┴─────┘");
        }

        private void MostrarValorMatriz(int fila, int columna)
        {
            int x = 22 + columna * 6;
            int y = 9 + fila * 2;

            Console.SetCursorPosition(x, y);

            // Colorear el valor según si es positivo, negativo o cero
            if (matriz[fila, columna] > 0)
                Console.ForegroundColor = ConsoleColor.Green;
            else if (matriz[fila, columna] < 0)
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.DarkGray;

            // Formatear para que ocupe 3 espacios
            Console.Write("{0,3}", matriz[fila, columna]);
        }

        private void CalcularSumas()
        {
            // Calcular sumas de filas
            for (int i = 0; i < 3; i++)
            {
                sumaFilas[i] = 0;
                for (int j = 0; j < 3; j++)
                {
                    sumaFilas[i] += matriz[i, j];
                }
            }

            // Calcular sumas de columnas
            for (int j = 0; j < 3; j++)
            {
                sumaColumnas[j] = 0;
                for (int i = 0; i < 3; i++)
                {
                    sumaColumnas[j] += matriz[i, j];
                }
            }
        }

        private void MostrarResultados()
        {
            // Mostrar resultados de sumas con animación
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(15, 16);
            Console.WriteLine("♦ RESULTADOS ♦");

            // Mostrar sumas de filas
            Console.SetCursorPosition(15, 18);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Suma de filas    → [ ");

            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(300);
                if (sumaFilas[i] > 0)
                    Console.ForegroundColor = ConsoleColor.Green;
                else if (sumaFilas[i] < 0)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.DarkGray;

                Console.Write($"{sumaFilas[i],3}");

                if (i < 2)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(" | ");
                }
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(" ]");

            // Mostrar sumas de columnas
            Console.SetCursorPosition(15, 19);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Suma de columnas → [ ");

            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(300);
                if (sumaColumnas[i] > 0)
                    Console.ForegroundColor = ConsoleColor.Green;
                else if (sumaColumnas[i] < 0)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.DarkGray;

                Console.Write($"{sumaColumnas[i],3}");

                if (i < 2)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(" | ");
                }
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(" ]");

            // Mensaje para continuar
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(15, 21);
            Console.WriteLine("Presiona Enter para continuar...");
            Console.ReadLine();
        }

        private void MostrarMenuFinal()
        {
            bool opcionValida = false;

            while (!opcionValida)
            {
                Console.Clear();

                // Dibujar marco del menú
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.SetCursorPosition(20, 5);
                Console.Write("╔══════════════════════════════╗");
                Console.SetCursorPosition(20, 6);
                Console.Write("║                              ║");
                Console.SetCursorPosition(20, 7);
                Console.Write("║      ¿QUÉ DESEAS HACER?      ║");
                Console.SetCursorPosition(20, 8);
                Console.Write("║                              ║");
                Console.SetCursorPosition(20, 9);
                Console.Write("╚══════════════════════════════╝");

                // Opciones
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(25, 11);
                Console.Write("▌ 1 ▐ Menú anterior");
                Console.SetCursorPosition(25, 13);
                Console.Write("▌ 2 ▐ Continuar (repetir)");
                Console.SetCursorPosition(25, 15);
                Console.Write("▌ 3 ▐ Salir");

                // Solicitar opción
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(25, 18);
                Console.Write("Tu elección: ");
                Console.CursorVisible = true;

                if (int.TryParse(Console.ReadLine(), out int opcion))
                {
                    switch (opcion)
                    {
                        case 1:
                            salir = true;
                            opcionValida = true;
                            break;
                        case 2:
                            // Reiniciar matriz y vectores
                            matriz = new int[3, 3];
                            sumaFilas = new int[3];
                            sumaColumnas = new int[3];
                            opcionValida = true;
                            break;
                        case 3:
                            // Salir del programa completamente
                            salir = true;
                            Environment.Exit(0);
                            break;
                        default:
                            Console.SetCursorPosition(25, 20);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("¡Opción no válida! Presiona Enter...");
                            Console.ReadKey();
                            break;
                    }
                }
                else
                {
                    Console.SetCursorPosition(25, 20);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("¡Entrada inválida! Presiona Enter...");
                    Console.ReadKey();
                }
                Console.CursorVisible = false;
            }
        }
    }
}