using System;
using System.Threading;

namespace MenuLocalizacionVM
{
    public class CalculadoraParesImpares
    {
        private int[,] matriz;
        private int filas;
        private int columnas;
        private readonly int maxFilas = 10;     // Reducido a 10 para mejor visualización
        private readonly int maxColumnas = 10;  // Reducido a 10 para mejor visualización
        private readonly string nombreMatriz = "MatrizNumeros"; // Nombre por defecto fijo

        public void Ejecutar()
        {
            bool continuar = true;
            while (continuar)
            {
                ObtenerDatosDeLaMatriz();
                LlenarMatriz();
                MostrarResultados();
                continuar = MostrarMenuFinal();
            }
        }

        private void ObtenerDatosDeLaMatriz()
        {
            Console.Clear();
            MostrarTitulo("CALCULADORA DE PARES E IMPARES EN MATRIZ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(5, 4);
            Console.WriteLine($"Tamaño máximo de la matriz: {maxFilas}x{maxColumnas} (para evitar que se salga de la pantalla)");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(5, 6);
            Console.WriteLine($"Nombre de la matriz: {nombreMatriz}");

            filas = PedirNumeroValido("Ingrese el número de filas", 1, maxFilas, 8);
            columnas = PedirNumeroValido("Ingrese el número de columnas", 1, maxColumnas, 10);

            matriz = new int[filas, columnas];
        }

        private void MostrarTitulo(string titulo)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            int posX = (Console.WindowWidth - titulo.Length) / 2;
            Console.SetCursorPosition(Math.Max(0, posX), 2);
            Console.WriteLine(titulo);
        }

        private int PedirNumeroValido(string mensaje, int min, int max, int posY)
        {
            int numero;
            bool valido = false;

            do
            {
                Console.SetCursorPosition(5, posY);
                Console.Write(new string(' ', Console.WindowWidth - 6));  // Limpiar la línea
                Console.SetCursorPosition(5, posY);
                Console.Write($"{mensaje} ({min}-{max}): ");

                if (int.TryParse(Console.ReadLine(), out numero) && numero >= min && numero <= max)
                {
                    valido = true;
                }
                else
                {
                    Console.SetCursorPosition(5, posY + 1);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: Ingrese un número entre {min} y {max}");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Thread.Sleep(1000);
                    Console.SetCursorPosition(5, posY + 1);
                    Console.Write(new string(' ', Console.WindowWidth - 6));  // Limpiar mensaje de error
                }
            } while (!valido);

            return numero;
        }

        private void LlenarMatriz()
        {
            Console.Clear();
            MostrarTitulo($"LLENANDO LA MATRIZ '{nombreMatriz}' ({filas}x{columnas})");

            DibujarMatrizConColores();

            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    bool valorValido = false;
                    while (!valorValido)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.SetCursorPosition(10 + j * 5, 6 + i * 2);
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.Write("    ");  // Espacio para el número
                        Console.BackgroundColor = ConsoleColor.Black;

                        Console.SetCursorPosition(10 + j * 5, 6 + i * 2);
                        string input = Console.ReadLine();

                        if (int.TryParse(input, out int valor))
                        {
                            matriz[i, j] = valor;
                            valorValido = true;
                        }
                        else
                        {
                            MostrarMensajeError("Por favor, ingrese un número entero válido.", filas);
                        }
                    }

                    // Redibujar la matriz después de cada entrada
                    DibujarMatrizConColores();
                }
            }
        }

        private void MostrarMensajeError(string mensaje, int alturaMatriz)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(5, 6 + alturaMatriz * 2 + 2);
            Console.WriteLine(mensaje);
            Thread.Sleep(1000);
            Console.SetCursorPosition(5, 6 + alturaMatriz * 2 + 2);
            Console.Write(new string(' ', mensaje.Length + 5));  // Limpiar mensaje de error
        }

        private void DibujarMatrizConColores()
        {
            // Dibujar encabezado de columnas
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int j = 0; j < columnas; j++)
            {
                Console.SetCursorPosition(10 + j * 5, 5);
                Console.Write($"[{j}]");
            }

            // Dibujar números de fila y contenido
            for (int i = 0; i < filas; i++)
            {
                Console.SetCursorPosition(5, 6 + i * 2);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"[{i}]");

                for (int j = 0; j < columnas; j++)
                {
                    Console.SetCursorPosition(10 + j * 5, 6 + i * 2);

                    // Colorear según par o impar
                    if (matriz[i, j] % 2 == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;  // Números pares en verde
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;  // Números impares en magenta
                    }

                    // Si la celda ya tiene un valor, mostrarlo
                    if (matriz[i, j] != 0 || (i == 0 && j == 0))  // Para la posición 0,0 mostramos el valor incluso si es cero
                    {
                        Console.Write($"{matriz[i, j],4}");
                    }
                    else
                    {
                        Console.Write("    ");  // Espacios en blanco para celdas vacías
                    }
                }
            }

            // Agregar leyenda de colores
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(5, 6 + filas * 2 + 1);
            Console.WriteLine("Números: ");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(15, 6 + filas * 2 + 1);
            Console.Write("Pares ");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(25, 6 + filas * 2 + 1);
            Console.Write("Impares");

            // Restaurar color por defecto
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void MostrarResultados()
        {
            int sumaPares = 0;
            int sumaImpares = 0;
            int cantidadPares = 0;
            int cantidadImpares = 0;

            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    if (matriz[i, j] % 2 == 0)
                    {
                        sumaPares += matriz[i, j];
                        cantidadPares++;
                    }
                    else
                    {
                        sumaImpares += matriz[i, j];
                        cantidadImpares++;
                    }
                }
            }

            double promedioPares = cantidadPares > 0 ? (double)sumaPares / cantidadPares : 0;
            double promedioImpares = cantidadImpares > 0 ? (double)sumaImpares / cantidadImpares : 0;

            Console.Clear();
            MostrarTitulo($"RESULTADOS DE LA MATRIZ '{nombreMatriz}' ({filas}x{columnas})");

            // Verificar si la matriz cabe en la pantalla para mostrarla
            if (6 + filas * 2 + 10 < Console.WindowHeight)
            {
                // Mostrar la matriz con colores
                DibujarMatrizConColores();
                MostrarEstadisticas(sumaPares, sumaImpares, cantidadPares, cantidadImpares, promedioPares, promedioImpares, 6 + filas * 2 + 3);
            }
            else
            {
                // Solo mostrar estadísticas si la matriz es muy grande
                MostrarEstadisticas(sumaPares, sumaImpares, cantidadPares, cantidadImpares, promedioPares, promedioImpares, 6);
            }
        }

        private void MostrarEstadisticas(int sumaPares, int sumaImpares, int cantidadPares, int cantidadImpares, double promedioPares, double promedioImpares, int baseY)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(5, baseY);
            Console.WriteLine("RESULTADOS DEL ANÁLISIS:");

            // Verificar espacio disponible y ajustar la visualización
            int lineasDisponibles = Console.WindowHeight - baseY - 2;

            if (lineasDisponibles >= 6)
            {
                // Estadísticas de pares
                Console.SetCursorPosition(5, baseY + 2);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("NÚMEROS PARES:");
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(5, baseY + 3);
                Console.WriteLine($"Suma: {sumaPares}");
                Console.SetCursorPosition(5, baseY + 4);
                Console.WriteLine($"Cantidad: {cantidadPares}");
                Console.SetCursorPosition(5, baseY + 5);
                Console.WriteLine($"Promedio: {promedioPares:F2}");

                // Estadísticas de impares
                Console.SetCursorPosition(40, baseY + 2);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("NÚMEROS IMPARES:");
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(40, baseY + 3);
                Console.WriteLine($"Suma: {sumaImpares}");
                Console.SetCursorPosition(40, baseY + 4);
                Console.WriteLine($"Cantidad: {cantidadImpares}");
                Console.SetCursorPosition(40, baseY + 5);
                Console.WriteLine($"Promedio: {promedioImpares:F2}");
            }
            else if (lineasDisponibles >= 3)
            {
                // Formato condensado
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(5, baseY + 2);
                Console.Write("PARES: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Suma={sumaPares}, Cant={cantidadPares}, Prom={promedioPares:F2}");

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition(5, baseY + 3);
                Console.Write("IMPARES: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Suma={sumaImpares}, Cant={cantidadImpares}, Prom={promedioImpares:F2}");
            }
            else
            {
                // Formato mínimo
                Console.SetCursorPosition(5, baseY + 2);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("P: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{sumaPares}/{cantidadPares}={promedioPares:F2} | ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("I: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{sumaImpares}/{cantidadImpares}={promedioImpares:F2}");
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(5, baseY + lineasDisponibles);
            Console.WriteLine("Presione ENTER para continuar...");
            Console.ReadLine();
        }

        private bool MostrarMenuFinal()
        {
            Console.Clear();
            MostrarTitulo("MENÚ DE OPCIONES");

            int centroX = Console.WindowWidth / 2 - 15;
            int baseY = 6;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(centroX, baseY);
            Console.WriteLine("¿Qué desea hacer ahora?");

            Console.SetCursorPosition(centroX, baseY + 2);
            Console.WriteLine("(1) Menú anterior");
            Console.SetCursorPosition(centroX, baseY + 3);
            Console.WriteLine("(2) Continuar con este programa");
            Console.SetCursorPosition(centroX, baseY + 4);
            Console.WriteLine("(3) Salir");

            Console.SetCursorPosition(centroX, baseY + 6);
            Console.Write("Seleccione una opción: ");

            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int opcion))
                {
                    switch (opcion)
                    {
                        case 1:
                            return false;  // Volver al menú principal
                        case 2:
                            return true;   // Continuar con este programa
                        case 3:
                            Environment.Exit(0);  // Salir completamente
                            return false;
                        default:
                            Console.SetCursorPosition(centroX, baseY + 7);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Opción no válida. Intente de nuevo.");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.SetCursorPosition(centroX, baseY + 6);
                            Console.Write("Seleccione una opción: ");
                            Console.Write(new string(' ', 10));
                            Console.SetCursorPosition(centroX + 21, baseY + 6);
                            break;
                    }
                }
                else
                {
                    Console.SetCursorPosition(centroX, baseY + 7);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Entrada inválida. Intente de nuevo.");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.SetCursorPosition(centroX, baseY + 6);
                    Console.Write("Seleccione una opción: ");
                    Console.Write(new string(' ', 10));
                    Console.SetCursorPosition(centroX + 21, baseY + 6);
                }
            }
        }
    }
}