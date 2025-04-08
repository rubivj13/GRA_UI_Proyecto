using System;
using System.Threading;

namespace MenuLocalizacionVM
{
    public class AnalizadorMatriz
    {
        private int[,] matriz;
        private int filas;
        private int columnas;
        private int positivos;
        private int negativos;
        private int ceros;
        private const int ANCHO_CELDA = 6; // Ancho fijo para cada celda

        public void Ejecutar()
        {
            bool salirPrograma = false;

            while (!salirPrograma)
            {
                Console.Clear();
                LeerDimensionesMatriz();
                CrearMatriz();
                LlenarMatriz();
                ContarElementos();
                MostrarMatrizConColores(true); // Animación al mostrar resultados
                MostrarResultados();

                int opcion = MostrarMenuOpciones();
                switch (opcion)
                {
                    case 1: // Menú anterior
                        return;
                    case 2: // Continuar
                        ResetearContadores();
                        break;
                    case 3: // Salir
                        salirPrograma = true;
                        Environment.Exit(0);
                        break;
                }
            }
        }

        private void LeerDimensionesMatriz()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(25, 3);
            Console.WriteLine("ANÁLISIS DE UNA MATRIZ (Positivos, Negativos y Ceros)");

            Console.SetCursorPosition(25, 5);
            Console.WriteLine("╔════════════════════════════════════════════════╗");
            Console.SetCursorPosition(25, 6);
            Console.WriteLine("║            DIMENSIONES DE LA MATRIZ            ║");
            Console.SetCursorPosition(25, 7);
            Console.WriteLine("╚════════════════════════════════════════════════╝");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(25, 9);
            Console.Write("Número de filas (m): ");
            while (!int.TryParse(Console.ReadLine(), out filas) || filas <= 0 || filas > 10)
            {
                Console.SetCursorPosition(25, 10);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Por favor, ingrese un número entero positivo (máximo 10).");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(25, 9);
                Console.Write("Número de filas (m): ");
                Console.Write(new string(' ', 20));
                Console.SetCursorPosition(45, 9);
            }

            Console.SetCursorPosition(25, 11);
            Console.Write("Número de columnas (n): ");
            while (!int.TryParse(Console.ReadLine(), out columnas) || columnas <= 0 || columnas > 10)
            {
                Console.SetCursorPosition(25, 12);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Por favor, ingrese un número entero positivo (máximo 10).");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(25, 11);
                Console.Write("Número de columnas (n): ");
                Console.Write(new string(' ', 20));
                Console.SetCursorPosition(48, 11);
            }
        }

        private void CrearMatriz()
        {
            matriz = new int[filas, columnas];
        }

        private void LlenarMatriz()
        {
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    Console.Clear();
                    MostrarTitulo("INGRESO DE ELEMENTOS");

                    // Mostrar la posición actual
                    Console.SetCursorPosition(25, 8);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Ingresando elemento en posición [{i},{j}]");

                    // Dibujar la matriz con animación
                    DibujarMatriz(i, j);

                    // Calcular posición para entrada de valores
                    int posEntradaY = GetMatrizHeight() + 13;

                    // Solicitar el valor
                    Console.SetCursorPosition(25, posEntradaY);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"Valor para posición [{i},{j}]: ");

                    bool entradaValida = false;
                    while (!entradaValida)
                    {
                        string entrada = Console.ReadLine();
                        if (int.TryParse(entrada, out int valor))
                        {
                            matriz[i, j] = valor;
                            entradaValida = true;
                        }
                        else
                        {
                            Console.SetCursorPosition(25, posEntradaY + 1);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Por favor, ingrese un número entero válido.");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.SetCursorPosition(25, posEntradaY);
                            Console.Write($"Valor para posición [{i},{j}]: ");
                            Console.Write(new string(' ', 20)); // Limpiar entrada anterior
                            Console.SetCursorPosition(25 + $"Valor para posición [{i},{j}]: ".Length, posEntradaY);
                        }
                    }
                }
            }
        }

        private int GetMatrizHeight()
        {
            return filas * 2 + 1; // Altura total de la matriz dibujada
        }

        private void MostrarTitulo(string titulo)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(25, 3);
            Console.WriteLine("ANÁLISIS DE UNA MATRIZ (Positivos, Negativos y Ceros)");

            Console.SetCursorPosition(25, 5);
            Console.WriteLine($"╔════════════════════════════════════════════════╗");
            Console.SetCursorPosition(25, 6);
            Console.WriteLine($"║{titulo.PadLeft(titulo.Length + (42 - titulo.Length) / 2).PadRight(42)}║");
            Console.SetCursorPosition(25, 7);
            Console.WriteLine($"╚════════════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        private void DibujarMatriz(int filaActual = -1, int columnaActual = -1, bool animacion = false)
        {
            int posY = 10;
            int posX = 25;

            // Dibujar el marco superior
            Console.SetCursorPosition(posX, posY);
            Console.Write("┌");
            for (int j = 0; j < columnas; j++)
            {
                Console.Write(new string('─', ANCHO_CELDA));
                if (j < columnas - 1) Console.Write("┬");
                if (animacion) Thread.Sleep(15);
            }
            Console.Write("┐");
            posY++;

            // Dibujar las filas con valores
            for (int i = 0; i < filas; i++)
            {
                Console.SetCursorPosition(posX, posY);
                Console.Write("│");

                for (int j = 0; j < columnas; j++)
                {
                    string valorDisplay;

                    if (i == filaActual && j == columnaActual)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        valorDisplay = " ? ";
                        Console.Write(valorDisplay.PadLeft((ANCHO_CELDA - valorDisplay.Length) / 2 + valorDisplay.Length).PadRight(ANCHO_CELDA));
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else if (i < filaActual || (i == filaActual && j < columnaActual))
                    {
                        valorDisplay = matriz[i, j].ToString();

                        if (matriz[i, j] == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write(valorDisplay.PadLeft((ANCHO_CELDA - valorDisplay.Length) / 2 + valorDisplay.Length).PadRight(ANCHO_CELDA));
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                        else
                        {
                            Console.Write(valorDisplay.PadLeft((ANCHO_CELDA - valorDisplay.Length) / 2 + valorDisplay.Length).PadRight(ANCHO_CELDA));
                        }
                    }
                    else
                    {
                        Console.Write(new string(' ', ANCHO_CELDA));
                    }

                    Console.Write("│");
                    if (animacion) Thread.Sleep(15);
                }

                posY++;

                // Dibujar líneas horizontales entre filas
                if (i < filas - 1)
                {
                    Console.SetCursorPosition(posX, posY);
                    Console.Write("├");
                    for (int j = 0; j < columnas; j++)
                    {
                        Console.Write(new string('─', ANCHO_CELDA));
                        if (j < columnas - 1) Console.Write("┼");
                        if (animacion) Thread.Sleep(15);
                    }
                    Console.Write("┤");
                    posY++;
                }
            }

            // Dibujar el marco inferior
            Console.SetCursorPosition(posX, posY);
            Console.Write("└");
            for (int j = 0; j < columnas; j++)
            {
                Console.Write(new string('─', ANCHO_CELDA));
                if (j < columnas - 1) Console.Write("┴");
                if (animacion) Thread.Sleep(15);
            }
            Console.Write("┘");
        }

        private void ContarElementos()
        {
            positivos = 0;
            negativos = 0;
            ceros = 0;

            foreach (int elemento in matriz)
            {
                if (elemento > 0)
                    positivos++;
                else if (elemento < 0)
                    negativos++;
                else
                    ceros++;
            }
        }

        private void MostrarMatrizConColores(bool animacion = false)
        {
            Console.Clear();
            MostrarTitulo("RESULTADOS DEL ANÁLISIS");

            // Dibujar matriz completa con animación
            int posY = 10;
            int posX = 25;

            // Dibujar el marco superior
            Console.SetCursorPosition(posX, posY);
            Console.Write("┌");
            for (int j = 0; j < columnas; j++)
            {
                Console.Write(new string('─', ANCHO_CELDA));
                if (j < columnas - 1) Console.Write("┬");
                if (animacion) Thread.Sleep(30);
            }
            Console.Write("┐");
            posY++;

            // Dibujar las filas con valores
            for (int i = 0; i < filas; i++)
            {
                Console.SetCursorPosition(posX, posY);
                Console.Write("│");

                for (int j = 0; j < columnas; j++)
                {
                    string valorDisplay = matriz[i, j].ToString();

                    if (matriz[i, j] == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write(valorDisplay.PadLeft((ANCHO_CELDA - valorDisplay.Length) / 2 + valorDisplay.Length).PadRight(ANCHO_CELDA));
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else if (matriz[i, j] > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(valorDisplay.PadLeft((ANCHO_CELDA - valorDisplay.Length) / 2 + valorDisplay.Length).PadRight(ANCHO_CELDA));
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(valorDisplay.PadLeft((ANCHO_CELDA - valorDisplay.Length) / 2 + valorDisplay.Length).PadRight(ANCHO_CELDA));
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }

                    Console.Write("│");
                    if (animacion) Thread.Sleep(30);
                }

                posY++;

                // Dibujar líneas horizontales entre filas
                if (i < filas - 1)
                {
                    Console.SetCursorPosition(posX, posY);
                    Console.Write("├");
                    for (int j = 0; j < columnas; j++)
                    {
                        Console.Write(new string('─', ANCHO_CELDA));
                        if (j < columnas - 1) Console.Write("┼");
                        if (animacion) Thread.Sleep(30);
                    }
                    Console.Write("┤");
                    posY++;
                }
            }

            // Dibujar el marco inferior
            Console.SetCursorPosition(posX, posY);
            Console.Write("└");
            for (int j = 0; j < columnas; j++)
            {
                Console.Write(new string('─', ANCHO_CELDA));
                if (j < columnas - 1) Console.Write("┴");
                if (animacion) Thread.Sleep(30);
            }
            Console.Write("┘");
        }

        private void MostrarResultados()
        {
            int posY = 10 + GetMatrizHeight() + 1;
            Console.SetCursorPosition(25, posY);
            Console.WriteLine("═════════════════════════════════════════════════");
            Console.SetCursorPosition(25, posY + 1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("RESULTADOS DEL ANÁLISIS:");

            Console.SetCursorPosition(25, posY + 3);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"• Números positivos: {positivos}");

            Console.SetCursorPosition(25, posY + 4);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"• Números negativos: {negativos}");

            Console.SetCursorPosition(25, posY + 5);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"• Ceros: {ceros}");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(25, posY + 7);
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }

        private int MostrarMenuOpciones()
        {
            Console.Clear();
            MostrarTitulo("OPCIONES");

            int posY = 10;
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.SetCursorPosition(25, posY);
            Console.WriteLine("╔════════════════════════════════════════════════╗");
            Console.SetCursorPosition(25, posY + 1);
            Console.WriteLine("║              SELECCIONE UNA OPCIÓN             ║");
            Console.SetCursorPosition(25, posY + 2);
            Console.WriteLine("╚════════════════════════════════════════════════╝");

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(30, posY + 4);
            Console.WriteLine("1. Menú anterior");
            Console.SetCursorPosition(30, posY + 5);
            Console.WriteLine("2. Continuar (analizar otra matriz)");
            Console.SetCursorPosition(30, posY + 6);
            Console.WriteLine("3. Salir");

            Console.SetCursorPosition(25, posY + 8);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Su elección → ");

            int opcion;
            while (!int.TryParse(Console.ReadLine(), out opcion) || opcion < 1 || opcion > 3)
            {
                Console.SetCursorPosition(25, posY + 9);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Opción inválida. Intente de nuevo.");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(25, posY + 8);
                Console.Write("Su elección → ");
                Console.Write(new string(' ', 10));
                Console.SetCursorPosition(40, posY + 8);
            }

            return opcion;
        }

        private void ResetearContadores()
        {
            positivos = 0;
            negativos = 0;
            ceros = 0;
        }
    }
}