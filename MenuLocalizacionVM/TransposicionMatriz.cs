using System;
using System.Threading;

namespace MenuLocalizacionVM
{
    public class TransposicionMatriz
    {
        private int[,] matriz;
        private int filas;
        private int columnas;
        private const int ESPACIADO_CELDA = 6;  // Espacio horizontal entre celdas
        private const int MARGEN_X = 20;        // Margen izquierdo
        private const int MAX_FILAS = 10;       // Máximo número de filas permitido
        private const int MAX_COLUMNAS = 8;     // Máximo número de columnas permitido

        // Caracteres para los bordes de la matriz
        private const char ESQUINA_SUP_IZQ = '╔';
        private const char ESQUINA_SUP_DER = '╗';
        private const char ESQUINA_INF_IZQ = '╚';
        private const char ESQUINA_INF_DER = '╝';
        private const char LINEA_HORIZONTAL = '═';
        private const char LINEA_VERTICAL = '║';
        private const char UNION_T_ARRIBA = '╦';
        private const char UNION_T_ABAJO = '╩';
        private const char UNION_T_IZQ = '╠';
        private const char UNION_T_DER = '╣';
        private const char CRUCE = '╬';

        public void Ejecutar()
        {
            bool continuar = true;

            while (continuar)
            {
                Console.Clear();
                DibujarEncabezadoPrograma();

                // Solicitar dimensiones de la matriz con información sobre límites
                if (!ObtenerDimensionesMatriz())
                    continue;

                matriz = new int[filas, columnas];

                // Llenar la matriz
                LlenarMatriz();

                // Mostrar resultados
                MostrarResultados();

                // CORRECCIÓN: Calcular posición adecuada que no dependa del tamaño de las matrices
                int posYMensajeFinal = Math.Min(Console.WindowHeight - 3, 20 + filas * 2);
                Console.SetCursorPosition(MARGEN_X, posYMensajeFinal);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Presione ENTER para continuar al menú de opciones...");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadLine();

                // Menú de opciones
                continuar = MostrarMenuOpciones();
            }
        }

        private void DibujarEncabezadoPrograma()
        {
            // Dibujar un marco alrededor del título principal
            int anchoBorde = 50;
            int posX = (Console.WindowWidth - anchoBorde) / 2;
            if (posX < 0) posX = 5;

            Console.ForegroundColor = ConsoleColor.Cyan;

            // Borde superior
            Console.SetCursorPosition(posX, 1);
            Console.Write(ESQUINA_SUP_IZQ);
            for (int i = 0; i < anchoBorde - 2; i++)
                Console.Write(LINEA_HORIZONTAL);
            Console.Write(ESQUINA_SUP_DER);

            // Laterales
            Console.SetCursorPosition(posX, 2);
            Console.Write(LINEA_VERTICAL);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            string titulo = " PROGRAMA DE TRANSPOSICIÓN DE MATRICES ";
            Console.Write(titulo.PadRight(anchoBorde - 2));
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(LINEA_VERTICAL);

            // Borde inferior
            Console.SetCursorPosition(posX, 3);
            Console.Write(ESQUINA_INF_IZQ);
            for (int i = 0; i < anchoBorde - 2; i++)
                Console.Write(LINEA_HORIZONTAL);
            Console.Write(ESQUINA_INF_DER);

            Console.ForegroundColor = ConsoleColor.White;
        }

        private bool ObtenerDimensionesMatriz()
        {
            DibujarCuadroEntrada("DIMENSIONES DE LA MATRIZ", 10, 5);

            // Mostrar límites de tamaño
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(MARGEN_X + 5, 7);
            Console.WriteLine($"► Tamaño máximo permitido: {MAX_FILAS} filas x {MAX_COLUMNAS} columnas ◄");
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(MARGEN_X + 5, 9);
            Console.Write("Ingrese el número de filas: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (!int.TryParse(Console.ReadLine(), out filas) || filas <= 0)
            {
                MostrarError("El número de filas debe ser un valor positivo.");
                return false;
            }

            // Validar que el número de filas no sea demasiado grande
            if (filas > MAX_FILAS)
            {
                MostrarError($"El número de filas no debe ser mayor a {MAX_FILAS} para una correcta visualización.");
                return false;
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(MARGEN_X + 5, 10);
            Console.Write("Ingrese el número de columnas: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (!int.TryParse(Console.ReadLine(), out columnas) || columnas <= 0)
            {
                MostrarError("El número de columnas debe ser un valor positivo.");
                return false;
            }

            // Validar que el número de columnas no sea demasiado grande
            if (columnas > MAX_COLUMNAS)
            {
                MostrarError($"El número de columnas no debe ser mayor a {MAX_COLUMNAS} para una correcta visualización.");
                return false;
            }

            Console.ForegroundColor = ConsoleColor.White;
            return true;
        }

        private void DibujarCuadroEntrada(string titulo, int alto, int posY)
        {
            int ancho = 60;
            int posX = MARGEN_X;

            Console.ForegroundColor = ConsoleColor.Cyan;

            // Borde superior con título
            Console.SetCursorPosition(posX, posY);
            Console.Write(ESQUINA_SUP_IZQ);

            // Espacio antes del título
            int espacioAntes = (ancho - titulo.Length - 2) / 2;
            for (int i = 0; i < espacioAntes; i++)
                Console.Write(LINEA_HORIZONTAL);

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" " + titulo + " ");

            Console.ForegroundColor = ConsoleColor.Cyan;
            // Espacio después del título
            int espacioDespues = ancho - titulo.Length - 2 - espacioAntes;
            for (int i = 0; i < espacioDespues - 2; i++)
                Console.Write(LINEA_HORIZONTAL);

            Console.Write(ESQUINA_SUP_DER);

            // Laterales
            for (int i = 1; i < alto - 1; i++)
            {
                Console.SetCursorPosition(posX, posY + i);
                Console.Write(LINEA_VERTICAL);
                Console.SetCursorPosition(posX + ancho - 1, posY + i);
                Console.Write(LINEA_VERTICAL);
            }

            // Borde inferior
            Console.SetCursorPosition(posX, posY + alto - 1);
            Console.Write(ESQUINA_INF_IZQ);
            for (int i = 0; i < ancho - 2; i++)
                Console.Write(LINEA_HORIZONTAL);
            Console.Write(ESQUINA_INF_DER);

            Console.ForegroundColor = ConsoleColor.White;
        }

        private void LlenarMatriz()
        {
            Console.Clear();
            DibujarEncabezadoPrograma();

            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    // Dibujar la matriz en cada iteración con diseño mejorado
                    DibujarMatrizParcial(i, j);

                    // Posicionamiento preciso del cursor dentro de la celda
                    int posX = MARGEN_X + 5 + j * ESPACIADO_CELDA + 1; // +1 para posicionar después del borde vertical
                    int posY = 10 + i; // Posición exacta de la fila

                    // Primero limpiamos la celda (espacio para 3 dígitos)
                    Console.SetCursorPosition(posX, posY);
                    Console.Write("   ");

                    // Luego posicionamos el cursor al inicio de la celda
                    Console.SetCursorPosition(posX, posY);

                    // Leer el valor
                    bool valorValido = false;
                    while (!valorValido)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        string input = Console.ReadLine();
                        Console.ForegroundColor = ConsoleColor.White;

                        if (int.TryParse(input, out int valor))
                        {
                            matriz[i, j] = valor;
                            valorValido = true;
                        }
                        else
                        {
                            // Redibujamos la matriz para mantener la consistencia visual
                            DibujarMatrizParcial(i, j);

                            // Mensaje de error
                            Console.SetCursorPosition(MARGEN_X + 5, 12 + filas + 2);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Por favor ingrese un número entero válido.");
                            Console.ForegroundColor = ConsoleColor.White;

                            // Limpiar celda y reposicionar cursor
                            Console.SetCursorPosition(posX, posY);
                            Console.Write("   ");
                            Console.SetCursorPosition(posX, posY);
                        }
                    }
                }
            }
        }

        private void DibujarMatrizParcial(int filaActual, int columnaActual)
        {
            Console.Clear();
            DibujarEncabezadoPrograma();

            DibujarCuadroEntrada("LLENADO DE LA MATRIZ", 3 + filas + 3, 6);

            int posInicialX = MARGEN_X + 5;
            int posInicialY = 10;

            // Dibujar índices de columna con colores
            Console.ForegroundColor = ConsoleColor.Green;
            for (int j = 0; j < columnas; j++)
            {
                Console.SetCursorPosition(posInicialX + j * ESPACIADO_CELDA + 2, posInicialY - 1);
                Console.Write($"{j + 1}");
            }

            // Dibujar la matriz con diseño avanzado
            DibujarMarcoMatriz(posInicialX, posInicialY, filas, columnas);

            // Dibujar los valores ya ingresados
            for (int i = 0; i < filas; i++)
            {
                // Índice de fila
                Console.SetCursorPosition(posInicialX - 2, posInicialY + i);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{i + 1}");
                Console.ForegroundColor = ConsoleColor.White;

                for (int j = 0; j < columnas; j++)
                {
                    // Saltamos la celda actual que estamos por llenar
                    if (i == filaActual && j == columnaActual)
                        continue;

                    // Solo mostramos celdas que ya han sido llenadas
                    if (i < filaActual || (i == filaActual && j < columnaActual))
                    {
                        Console.SetCursorPosition(posInicialX + j * ESPACIADO_CELDA + 1, posInicialY + i);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($"{matriz[i, j],3}");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }

            // Resaltar visualmente la celda actual
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(posInicialX + columnaActual * ESPACIADO_CELDA + 1, posInicialY + filaActual);
            Console.Write("   ");
            Console.BackgroundColor = ConsoleColor.Black;

            // Indicar la posición actual con estilo
            Console.SetCursorPosition(posInicialX, posInicialY + filas + 2);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"► ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Ingrese el valor para la posición [");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{filaActual + 1},{columnaActual + 1}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"]: ");
        }

        private void DibujarMarcoMatriz(int posX, int posY, int filas, int columnas)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            // Esquina superior izquierda
            Console.SetCursorPosition(posX, posY - 1);
            Console.Write(ESQUINA_SUP_IZQ);

            // Línea superior con uniones T arriba
            for (int j = 0; j < columnas; j++)
            {
                // Líneas horizontales para cada celda
                for (int k = 0; k < ESPACIADO_CELDA - 1; k++)
                    Console.Write(LINEA_HORIZONTAL);

                // Unión T arriba, excepto para la última columna
                if (j < columnas - 1)
                    Console.Write(UNION_T_ARRIBA);
                else
                    Console.Write(ESQUINA_SUP_DER);
            }

            // Líneas verticales y horizontales internas
            for (int i = 0; i < filas; i++)
            {
                Console.SetCursorPosition(posX, posY + i);
                Console.Write(LINEA_VERTICAL);

                for (int j = 0; j < columnas; j++)
                {
                    // Espacio para el valor (se llena en otra parte)
                    Console.Write(new string(' ', ESPACIADO_CELDA - 1));

                    // Línea vertical, excepto para la última columna
                    Console.Write(LINEA_VERTICAL);
                }

                // Si no es la última fila, dibujar líneas horizontales con cruces
                if (i < filas - 1)
                {
                    Console.SetCursorPosition(posX, posY + i + 1);
                    Console.Write(UNION_T_IZQ);

                    for (int j = 0; j < columnas; j++)
                    {
                        // Líneas horizontales para cada celda
                        for (int k = 0; k < ESPACIADO_CELDA - 1; k++)
                            Console.Write(LINEA_HORIZONTAL);

                        // Cruce o unión T derecha
                        if (j < columnas - 1)
                            Console.Write(CRUCE);
                        else
                            Console.Write(UNION_T_DER);
                    }
                }
            }

            // Línea inferior
            Console.SetCursorPosition(posX, posY + filas);
            Console.Write(ESQUINA_INF_IZQ);

            for (int j = 0; j < columnas; j++)
            {
                // Líneas horizontales para cada celda
                for (int k = 0; k < ESPACIADO_CELDA - 1; k++)
                    Console.Write(LINEA_HORIZONTAL);

                // Unión T abajo o esquina inferior derecha
                if (j < columnas - 1)
                    Console.Write(UNION_T_ABAJO);
                else
                    Console.Write(ESQUINA_INF_DER);
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        private void MostrarResultados()
        {
            Console.Clear();
            DibujarEncabezadoPrograma();

            int posYOriginal = 8;
            int altoMatrizOriginal = filas + 4; // +4 para incluir título y espacios adicionales
            int posInicialX = MARGEN_X + 5;

            // Realizar transposición
            int[,] matrizTranspuesta = TransponerMatriz();

            // Título para sección de resultados
            Console.SetCursorPosition(MARGEN_X, 6);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("┌─────────────────────────────────────────────────┐");
            Console.SetCursorPosition(MARGEN_X, 7);
            Console.WriteLine("│              RESULTADO DE OPERACIÓN             │");
            Console.SetCursorPosition(MARGEN_X, 8);
            Console.WriteLine("└─────────────────────────────────────────────────┘");
            Console.ForegroundColor = ConsoleColor.White;

            // Mostrar la matriz original con título decorado
            Console.SetCursorPosition(posInicialX - 5, posYOriginal + 2);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("◄ MATRIZ ORIGINAL ►");
            Console.ForegroundColor = ConsoleColor.White;

            DibujarMatrizCompleta(matriz, posInicialX, posYOriginal + 4);

            // CORRECCIÓN: Adaptar la ubicación de la matriz transpuesta según el tamaño de pantalla
            int posYTranspuesta;
            int posXTranspuesta = posInicialX;

            // Si la matriz es pequeña, mostrar lado a lado
            if (filas <= 6 && columnas <= 6 && Console.WindowWidth >= 100)
            {
                posYTranspuesta = posYOriginal + 4;
                posXTranspuesta = MARGEN_X + 5 + (columnas * ESPACIADO_CELDA) + 15; // A la derecha de la original

                // Flecha horizontal
                int posYFlecha = posYOriginal + altoMatrizOriginal / 2;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(MARGEN_X + 5 + (columnas * ESPACIADO_CELDA) + 5, posYFlecha);
                Console.WriteLine("►►►");
                Console.SetCursorPosition(MARGEN_X + 5 + (columnas * ESPACIADO_CELDA) + 5, posYFlecha + 1);
                Console.WriteLine("TRANSPONER");
            }
            else // Si es grande, mostrar una debajo de otra
            {
                posYTranspuesta = posYOriginal + altoMatrizOriginal + 4;

                // Flecha vertical
                int posXFlecha = posInicialX + Math.Min((columnas * ESPACIADO_CELDA) / 2, 30);
                int posYFlecha = posYOriginal + altoMatrizOriginal + 1;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(posXFlecha, posYFlecha);
                Console.WriteLine("    ▼    ");
                Console.SetCursorPosition(posXFlecha, posYFlecha + 1);
                Console.WriteLine(" TRANSPONER ");
                Console.SetCursorPosition(posXFlecha, posYFlecha + 2);
                Console.WriteLine("    ▼    ");
            }

            // Mostrar la matriz transpuesta con título decorado
            Console.SetCursorPosition(posXTranspuesta - 5, posYTranspuesta);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("◄ MATRIZ TRANSPUESTA ►");
            Console.ForegroundColor = ConsoleColor.White;

            DibujarMatrizCompleta(matrizTranspuesta, posXTranspuesta, posYTranspuesta + 2);

            Console.ForegroundColor = ConsoleColor.White;
        }

        private void DibujarMatrizCompleta(int[,] mat, int posX, int posY)
        {
            int filasM = mat.GetLength(0);
            int columnasM = mat.GetLength(1);

            // Dibujar índices de columna
            Console.ForegroundColor = ConsoleColor.Green;
            for (int j = 0; j < columnasM; j++)
            {
                Console.SetCursorPosition(posX + j * ESPACIADO_CELDA + 2, posY - 1);
                Console.Write($"{j + 1}");
            }
            Console.ForegroundColor = ConsoleColor.White;

            // Dibujar el marco de la matriz
            DibujarMarcoMatriz(posX, posY, filasM, columnasM);

            // Dibujar los valores
            for (int i = 0; i < filasM; i++)
            {
                // Índice de fila
                Console.SetCursorPosition(posX - 2, posY + i);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{i + 1}");
                Console.ForegroundColor = ConsoleColor.White;

                for (int j = 0; j < columnasM; j++)
                {
                    Console.SetCursorPosition(posX + j * ESPACIADO_CELDA + 1, posY + i);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"{mat[i, j],3}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        private int[,] TransponerMatriz()
        {
            int[,] transpuesta = new int[columnas, filas];

            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    transpuesta[j, i] = matriz[i, j];
                }
            }

            return transpuesta;
        }

        private bool MostrarMenuOpciones()
        {
            Console.Clear();
            DibujarEncabezadoPrograma();

            // Calcular posición para el menú en una nueva pantalla
            DibujarCuadroEntrada("MENÚ DE OPCIONES", 8, 6);

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(MARGEN_X + 5, 8);
            Console.Write("1. ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Continuar (nueva matriz)");

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(MARGEN_X + 5, 9);
            Console.Write("2. ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Regresar al menú principal");

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(MARGEN_X + 5, 10);
            Console.Write("3. ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Salir");

            Console.SetCursorPosition(MARGEN_X + 5, 12);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Seleccione una opción: ");
            Console.ForegroundColor = ConsoleColor.Yellow;

            switch (Console.ReadKey().KeyChar)
            {
                case '1':
                    return true;  // Continuar con el programa actual (nueva matriz)
                case '2':
                    return false; // Regresar al menú principal
                case '3':
                    DibujarDespedida();
                    Environment.Exit(0); // Salir completamente
                    return false;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(MARGEN_X + 5, 14);
                    Console.WriteLine("Opción no válida. Presione cualquier tecla para intentar de nuevo.");
                    Console.ReadKey();
                    return MostrarMenuOpciones(); // Recursión para volver a mostrar opciones
            }
        }

        private void DibujarDespedida()
        {
            Console.Clear();
            // Calcular el centro de la pantalla correctamente
            int mitadPantalla = Math.Max(20, Console.WindowWidth / 2 - 15);
            int alturaCentral = Math.Min(10, Console.WindowHeight / 2);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(mitadPantalla, alturaCentral - 2);
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.SetCursorPosition(mitadPantalla, alturaCentral - 1);
            Console.WriteLine("║                                  ║");
            Console.SetCursorPosition(mitadPantalla, alturaCentral);
            Console.WriteLine("║     GRACIAS POR UTILIZAR EL      ║");
            Console.SetCursorPosition(mitadPantalla, alturaCentral + 1);
            Console.WriteLine("║   PROGRAMA DE TRANSPOSICIÓN DE   ║");
            Console.SetCursorPosition(mitadPantalla, alturaCentral + 2);
            Console.WriteLine("║            MATRICES              ║");
            Console.SetCursorPosition(mitadPantalla, alturaCentral + 3);
            Console.WriteLine("║                                  ║");
            Console.SetCursorPosition(mitadPantalla, alturaCentral + 4);
            Console.WriteLine("╚══════════════════════════════════╝");

            Thread.Sleep(1500);
        }

        private void MostrarError(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(MARGEN_X, 15);
            Console.WriteLine("╔═════════════════════════════════════════════════╗");
            Console.SetCursorPosition(MARGEN_X, 16);
            Console.WriteLine("║                     ERROR                       ║");
            Console.SetCursorPosition(MARGEN_X, 17);
            Console.Write("║ ");
            Console.Write(mensaje.PadRight(47));
            Console.WriteLine(" ║");
            Console.SetCursorPosition(MARGEN_X, 18);
            Console.WriteLine("╚═════════════════════════════════════════════════╝");

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(MARGEN_X, 20);
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}