using System;
using System.Threading;

namespace MenuLocalizacionVM
{
    public class MultiplicacionMatrices
    {
        private int[,] matrizA;
        private int[,] matrizB;
        private int[,] matrizC; // Resultado de la multiplicación
        private int filasA, columnasA;
        private int filasB, columnasB;
        private const int MAXIMO_DIMENSION = 10; // Tamaño máximo para que no se salga de la pantalla
        private int anchoConsola;
        private int altoConsola;

        public void Ejecutar()
        {
            // Establecer un tamaño mínimo para la consola
            Console.SetWindowSize(Math.Max(Console.WindowWidth, 90), Math.Max(Console.WindowHeight, 30));
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);

            // Guardar dimensiones de la consola
            anchoConsola = Console.WindowWidth;
            altoConsola = Console.WindowHeight;

            bool continuarPrograma = true;

            while (continuarPrograma)
            {
                Console.Clear();
                DibujarEncabezado();

                // Obtener dimensiones de matrices
                ObtenerDimensionesMatrices();

                // Crear matrices
                matrizA = new int[filasA, columnasA];
                matrizB = new int[filasB, columnasB];
                matrizC = new int[filasA, columnasB];

                // Llenar matrices
                LlenarMatriz("A", matrizA, filasA, columnasA);
                LlenarMatriz("B", matrizB, filasB, columnasB);

                // Multiplicar matrices
                MultiplicarMatrices();

                // Mostrar resultado
                MostrarResultado();

                // Opciones finales
                continuarPrograma = MostrarOpcionesFinales();
            }
        }

        private void DibujarEncabezado()
        {
            DibujarMarco();

            int centroX = anchoConsola / 2;
            Console.ForegroundColor = ConsoleColor.Green;
            CentrarTexto("MULTIPLICACIÓN DE MATRICES", 2);
            CentrarTexto("=========================", 3);

            Console.ForegroundColor = ConsoleColor.White;
            CentrarTexto("El tamaño máximo de las matrices es de " + MAXIMO_DIMENSION + "x" + MAXIMO_DIMENSION + " para evitar problemas de visualización.", 5);
        }

        private void DibujarMarco()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;

            // Dibujar bordes horizontales
            for (int i = 0; i < anchoConsola; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("═");
                Console.SetCursorPosition(i, altoConsola - 1);
                Console.Write("═");
            }

            // Dibujar bordes verticales
            for (int i = 1; i < altoConsola - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("║");
                Console.SetCursorPosition(anchoConsola - 1, i);
                Console.Write("║");
            }

            // Dibujar esquinas
            Console.SetCursorPosition(0, 0);
            Console.Write("╔");
            Console.SetCursorPosition(anchoConsola - 1, 0);
            Console.Write("╗");
            Console.SetCursorPosition(0, altoConsola - 1);
            Console.Write("╚");
            Console.SetCursorPosition(anchoConsola - 1, altoConsola - 1);
            Console.Write("╝");
        }

        private void CentrarTexto(string texto, int fila)
        {
            int posX = (anchoConsola - texto.Length) / 2;
            Console.SetCursorPosition(posX, fila);
            Console.WriteLine(texto);
        }

        private void ObtenerDimensionesMatrices()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            int inicioY = 8;
            int margenX = 5;

            // Dimensiones de la matriz A
            Console.SetCursorPosition(margenX, inicioY);
            Console.Write("Ingrese el número de filas para la Matriz A (máximo " + MAXIMO_DIMENSION + "): ");
            while (!int.TryParse(Console.ReadLine(), out filasA) || filasA <= 0 || filasA > MAXIMO_DIMENSION)
            {
                Console.SetCursorPosition(margenX, inicioY + 1);
                Console.Write("Valor inválido. Ingrese un número entre 1 y " + MAXIMO_DIMENSION + ": ");
                Console.SetCursorPosition(margenX + 55, inicioY + 1);
                Console.Write(new string(' ', 10));
                Console.SetCursorPosition(margenX + 55, inicioY + 1);
            }

            Console.SetCursorPosition(margenX, inicioY + 2);
            Console.Write("Ingrese el número de columnas para la Matriz A (máximo " + MAXIMO_DIMENSION + "): ");
            while (!int.TryParse(Console.ReadLine(), out columnasA) || columnasA <= 0 || columnasA > MAXIMO_DIMENSION)
            {
                Console.SetCursorPosition(margenX, inicioY + 3);
                Console.Write("Valor inválido. Ingrese un número entre 1 y " + MAXIMO_DIMENSION + ": ");
                Console.SetCursorPosition(margenX + 55, inicioY + 3);
                Console.Write(new string(' ', 10));
                Console.SetCursorPosition(margenX + 55, inicioY + 3);
            }

            // Las dimensiones de la matriz B deben ser compatibles
            filasB = columnasA; // Para multiplicación, las columnas de A deben ser iguales a las filas de B

            Console.SetCursorPosition(margenX, inicioY + 4);
            Console.WriteLine("El número de filas de la Matriz B debe ser " + filasB + " para permitir la multiplicación.");

            Console.SetCursorPosition(margenX, inicioY + 5);
            Console.Write("Ingrese el número de columnas para la Matriz B (máximo " + MAXIMO_DIMENSION + "): ");
            while (!int.TryParse(Console.ReadLine(), out columnasB) || columnasB <= 0 || columnasB > MAXIMO_DIMENSION)
            {
                Console.SetCursorPosition(margenX, inicioY + 6);
                Console.Write("Valor inválido. Ingrese un número entre 1 y " + MAXIMO_DIMENSION + ": ");
                Console.SetCursorPosition(margenX + 55, inicioY + 6);
                Console.Write(new string(' ', 10));
                Console.SetCursorPosition(margenX + 55, inicioY + 6);
            }

            Console.Clear();
            DibujarEncabezado();
        }

        private void LlenarMatriz(string nombre, int[,] matriz, int filas, int columnas)
        {
            Console.Clear();
            DibujarMarco();

            Console.ForegroundColor = ConsoleColor.Cyan;
            CentrarTexto("LLENANDO MATRIZ " + nombre, 2);
            CentrarTexto("======================", 3);

            // Calcular espaciado óptimo para cada celda
            int anchoCelda = 6; // Cada celda tendrá un ancho de 6 caracteres
            int anchoTotal = (anchoCelda + 1) * columnas + 1; // +1 para los separadores verticales
            int inicioX = (anchoConsola - anchoTotal) / 2;
            int inicioY = 6;

            // Dibujar la matriz vacía
            DibujarMatrizEsqueleto(nombre, matriz, filas, columnas, inicioX, inicioY, anchoCelda);

            // Llenar la matriz
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    int posY = inicioY + 1 + (i * 2); // +1 para ajustar después del encabezado
                    int posX = inicioX + 1 + (j * (anchoCelda + 1)); // +1 para evitar el borde izquierdo

                    Console.SetCursorPosition(posX, posY);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(new string(' ', anchoCelda - 1)); // Limpiar el espacio para la entrada

                    Console.SetCursorPosition(posX + 1, posY);

                    bool valorValido = false;
                    while (!valorValido)
                    {
                        Console.SetCursorPosition(posX + 1, posY);
                        Console.Write(new string(' ', anchoCelda - 2)); // Limpiar espacio para la entrada
                        Console.SetCursorPosition(posX + 1, posY);
                        string input = Console.ReadLine();

                        if (string.IsNullOrEmpty(input))
                        {
                            matriz[i, j] = 0;
                            valorValido = true;
                        }
                        else if (int.TryParse(input, out int valor))
                        {
                            matriz[i, j] = valor;
                            valorValido = true;
                        }
                        else
                        {
                            MostrarMensaje("Valor inválido. Ingrese un número entero.", ConsoleColor.Red);
                            Thread.Sleep(800);
                            LimpiarMensaje();
                        }
                    }

                    // Redibujar la celda con el valor centrado
                    Console.SetCursorPosition(posX, posY);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(new string(' ', anchoCelda - 1)); // Limpiar el espacio para la entrada
                    Console.SetCursorPosition(posX, posY);

                    // Centrar el número en la celda
                    string valorStr = matriz[i, j].ToString();
                    int espacioInicial = (anchoCelda - valorStr.Length) / 2;
                    Console.SetCursorPosition(posX + espacioInicial, posY);
                    Console.Write(valorStr);
                }
            }

            MostrarMensaje("Matriz completada. Presione ENTER para continuar...", ConsoleColor.Green);
            Console.ReadLine();
            LimpiarMensaje();
        }

        private void DibujarMatrizEsqueleto(string nombre, int[,] matriz, int filas, int columnas, int inicioX, int inicioY, int anchoCelda)
        {
            Console.ForegroundColor = ConsoleColor.White;
            CentrarTexto("Matriz " + nombre + " [" + filas + "x" + columnas + "]:", inicioY - 1);

            // Dibujar línea superior
            Console.SetCursorPosition(inicioX, inicioY);
            Console.Write("┌");
            for (int j = 0; j < columnas; j++)
            {
                Console.Write(new string('─', anchoCelda));
                if (j < columnas - 1)
                    Console.Write("┬");
            }
            Console.Write("┐");

            // Dibujar filas
            for (int i = 0; i < filas; i++)
            {
                // Dibujar línea de contenido
                Console.SetCursorPosition(inicioX, inicioY + 1 + (i * 2));
                Console.Write("│");
                for (int j = 0; j < columnas; j++)
                {
                    Console.Write(new string(' ', anchoCelda));
                    Console.Write("│");
                }

                // Dibujar línea divisoria
                if (i < filas - 1)
                {
                    Console.SetCursorPosition(inicioX, inicioY + 2 + (i * 2));
                    Console.Write("├");
                    for (int j = 0; j < columnas; j++)
                    {
                        Console.Write(new string('─', anchoCelda));
                        if (j < columnas - 1)
                            Console.Write("┼");
                    }
                    Console.Write("┤");
                }
            }

            // Dibujar línea inferior
            Console.SetCursorPosition(inicioX, inicioY + (filas * 2));
            Console.Write("└");
            for (int j = 0; j < columnas; j++)
            {
                Console.Write(new string('─', anchoCelda));
                if (j < columnas - 1)
                    Console.Write("┴");
            }
            Console.Write("┘");
        }

        private void MostrarMensaje(string mensaje, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            CentrarTexto(mensaje, altoConsola - 3);
        }

        private void LimpiarMensaje()
        {
            Console.SetCursorPosition(0, altoConsola - 3);
            Console.Write(new string(' ', anchoConsola));
        }

        private void MultiplicarMatrices()
        {
            Console.Clear();
            DibujarMarco();

            Console.ForegroundColor = ConsoleColor.Green;
            CentrarTexto("MULTIPLICACIÓN DE MATRICES", 2);
            CentrarTexto("=========================", 3);

            Console.ForegroundColor = ConsoleColor.Yellow;
            CentrarTexto("Calculando el producto de las matrices A y B...", 5);

            // Muestra una animación simple para la multiplicación
            CentrarTexto("[                    ]", 7);

            for (int i = 0; i < 20; i++)
            {
                Thread.Sleep(50);
                Console.SetCursorPosition((anchoConsola - 20) / 2 + 1 + i, 7);
                Console.Write("█");
            }

            // Realizar la multiplicación
            for (int i = 0; i < filasA; i++)
            {
                for (int j = 0; j < columnasB; j++)
                {
                    matrizC[i, j] = 0;
                    for (int k = 0; k < columnasA; k++)
                    {
                        matrizC[i, j] += matrizA[i, k] * matrizB[k, j];
                    }
                }
            }

            MostrarMensaje("¡Multiplicación completada! Presione ENTER para ver los resultados.", ConsoleColor.Green);
            Console.ReadLine();
        }

        private void MostrarResultado()
        {
            // Mostrar cada matriz individualmente
            MostrarMatrizIndividual("A", matrizA, filasA, columnasA);

            Console.Clear();
            DibujarMarco();
            Console.ForegroundColor = ConsoleColor.Yellow;
            CentrarTexto("× (multiplicado por)", 2);
            Thread.Sleep(1000);

            MostrarMatrizIndividual("B", matrizB, filasB, columnasB);

            Console.Clear();
            DibujarMarco();
            Console.ForegroundColor = ConsoleColor.Yellow;
            CentrarTexto("= (igual a)", 2);
            Thread.Sleep(1000);

            MostrarMatrizIndividual("C (Resultado)", matrizC, filasA, columnasB);

            // Explicación final
            Console.Clear();
            DibujarMarco();
            Console.ForegroundColor = ConsoleColor.Cyan;

            CentrarTexto("RESUMEN DE LA MULTIPLICACIÓN", 2);
            CentrarTexto("==========================", 3);

            int inicioY = 5;
            CentrarTexto("La multiplicación de una matriz A[" + filasA + "x" + columnasA + "] por una matriz B[" + filasB + "x" + columnasB + "]", inicioY);
            CentrarTexto("da como resultado una matriz C[" + filasA + "x" + columnasB + "]", inicioY + 1);

            MostrarMensaje("Presione ENTER para continuar...", ConsoleColor.Green);
            Console.ReadLine();
        }

        private void MostrarMatrizIndividual(string nombre, int[,] matriz, int filas, int columnas)
        {
            Console.Clear();
            DibujarMarco();

            Console.ForegroundColor = ConsoleColor.Green;
            CentrarTexto("MATRIZ " + nombre, 2);
            CentrarTexto("===============", 3);

            // Usar un tamaño de celda fijo para mostrar las matrices
            int anchoCelda = 6;
            int anchoTotal = (anchoCelda + 1) * columnas + 1;
            int inicioX = (anchoConsola - anchoTotal) / 2;
            int inicioY = 6;

            DibujarMatrizEsqueleto(nombre, matriz, filas, columnas, inicioX, inicioY, anchoCelda);

            // Mostrar valores de la matriz centrados en cada celda
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    int posX = inicioX + 1 + (j * (anchoCelda + 1));
                    int posY = inicioY + 1 + (i * 2);

                    // Centrar el número en la celda
                    string valorStr = matriz[i, j].ToString();
                    int espacioInicial = (anchoCelda - valorStr.Length) / 2;

                    Console.SetCursorPosition(posX + espacioInicial, posY);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(valorStr);
                }
            }

            MostrarMensaje("Presione ENTER para continuar...", ConsoleColor.Green);
            Console.ReadLine();
        }

        private bool MostrarOpcionesFinales()
        {
            Console.Clear();
            DibujarMarco();

            Console.ForegroundColor = ConsoleColor.Green;
            CentrarTexto("OPCIONES FINALES", 6);
            CentrarTexto("===============", 7);

            Console.ForegroundColor = ConsoleColor.Yellow;
            CentrarTexto("¿Qué deseas hacer ahora?", 9);

            int centroX = anchoConsola / 2;
            int inicioMenu = centroX - 15;

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(inicioMenu, 11);
            Console.WriteLine("1. Continuar con multiplicación de matrices");
            Console.SetCursorPosition(inicioMenu, 12);
            Console.WriteLine("2. Volver al menú principal");
            Console.SetCursorPosition(inicioMenu, 13);
            Console.WriteLine("3. Salir del programa");

            Console.SetCursorPosition(inicioMenu, 15);
            Console.Write("Selecciona una opción: ");

            int opcion;
            while (!int.TryParse(Console.ReadLine(), out opcion) || opcion < 1 || opcion > 3)
            {
                Console.SetCursorPosition(inicioMenu, 16);
                Console.Write("Opción inválida. Intente nuevamente: ");
                Console.SetCursorPosition(inicioMenu + 35, 16);
                Console.Write(new string(' ', 5));
                Console.SetCursorPosition(inicioMenu + 35, 16);
            }

            switch (opcion)
            {
                case 1:
                    return true;  // Continuar con el programa actual
                case 2:
                    return false; // Volver al menú principal
                case 3:
                    Environment.Exit(0); // Salir del programa completamente
                    return false;
                default:
                    return false;
            }
        }
    }
}