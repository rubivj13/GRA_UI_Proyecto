using System;

namespace MenuLocalizacionVM
{
    public class IdentificadorMayorMenor
    {
        private int[,] matriz;
        private int filas;
        private int columnas;
        private int mayor;
        private int menor;
        private int filaMayor, columnaMayor;
        private int filaMenor, columnaMenor;
        private const int MAX_FILAS = 8;
        private const int MAX_COLUMNAS = 8;

        public void Ejecutar()
        {
            bool continuar = true;

            while (continuar)
            {
                Console.Clear();
                DibujarMarco(2, 1, 76, 24);

                Console.ForegroundColor = ConsoleColor.Cyan;
                CentrarTexto("═══ IDENTIFICACIÓN DE NÚMERO MAYOR Y MENOR EN UNA MATRIZ ═══", 3);
                Console.ForegroundColor = ConsoleColor.White;

                // Mostrar advertencia sobre tamaño máximo
                Console.ForegroundColor = ConsoleColor.Yellow;
                CentrarTexto($"(Tamaño máximo recomendado: {MAX_FILAS}x{MAX_COLUMNAS})", 4);
                Console.ForegroundColor = ConsoleColor.White;

                // Solicitar tamaño de matriz
                ObtenerTamano();

                // Crear matriz
                matriz = new int[filas, columnas];

                // Llenar matriz
                LlenarMatriz();

                // Encontrar mayor y menor
                EncontrarMayorMenor();

                // Mostrar resultados
                MostrarResultados();

                // Menú de opciones (en pantalla limpia)
                continuar = MostrarMenu();
            }
        }

        private void DibujarMarco(int x, int y, int ancho, int alto)
        {
            // Esquinas
            Console.SetCursorPosition(x, y);
            Console.Write("╔");
            Console.SetCursorPosition(x + ancho - 1, y);
            Console.Write("╗");
            Console.SetCursorPosition(x, y + alto - 1);
            Console.Write("╚");
            Console.SetCursorPosition(x + ancho - 1, y + alto - 1);
            Console.Write("╝");

            // Bordes horizontales
            for (int i = x + 1; i < x + ancho - 1; i++)
            {
                Console.SetCursorPosition(i, y);
                Console.Write("═");
                Console.SetCursorPosition(i, y + alto - 1);
                Console.Write("═");
            }

            // Bordes verticales
            for (int i = y + 1; i < y + alto - 1; i++)
            {
                Console.SetCursorPosition(x, i);
                Console.Write("║");
                Console.SetCursorPosition(x + ancho - 1, i);
                Console.Write("║");
            }
        }

        private void CentrarTexto(string texto, int y)
        {
            int x = (80 - texto.Length) / 2;
            Console.SetCursorPosition(x, y);
            Console.WriteLine(texto);
        }

        private void ObtenerTamano()
        {
            Console.SetCursorPosition(10, 6);
            Console.Write($"► Ingrese el número de filas (1-{MAX_FILAS} recomendado): ");
            while (!int.TryParse(Console.ReadLine(), out filas) || filas <= 0)
            {
                Console.SetCursorPosition(10, 6);
                Console.Write($"► Valor inválido. Ingrese el número de filas (1-{MAX_FILAS} recomendado): ");
            }

            // Advertir si el tamaño excede lo recomendado
            if (filas > MAX_FILAS)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(10, 8);
                Console.WriteLine("ADVERTENCIA: El número de filas excede lo recomendado.");
                Console.SetCursorPosition(10, 9);
                Console.WriteLine("Es posible que algunos elementos no se muestren correctamente.");
                Console.SetCursorPosition(10, 10);
                Console.WriteLine("Presione Enter para continuar o ESC para volver a ingresar el tamaño.");

                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    DibujarMarco(2, 1, 76, 24);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    CentrarTexto("═══ IDENTIFICACIÓN DE NÚMERO MAYOR Y MENOR EN UNA MATRIZ ═══", 3);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    CentrarTexto($"(Tamaño máximo recomendado: {MAX_FILAS}x{MAX_COLUMNAS})", 4);
                    Console.ForegroundColor = ConsoleColor.White;
                    ObtenerTamano();
                    return;
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                DibujarMarco(2, 1, 76, 24);
                Console.ForegroundColor = ConsoleColor.Cyan;
                CentrarTexto("═══ IDENTIFICACIÓN DE NÚMERO MAYOR Y MENOR EN UNA MATRIZ ═══", 3);
                Console.ForegroundColor = ConsoleColor.Yellow;
                CentrarTexto($"(Tamaño máximo recomendado: {MAX_FILAS}x{MAX_COLUMNAS})", 4);
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(10, 6);
                Console.Write($"► Ingrese el número de filas (1-{MAX_FILAS} recomendado): {filas}");
            }

            Console.SetCursorPosition(10, 7);
            Console.Write($"► Ingrese el número de columnas (1-{MAX_COLUMNAS} recomendado): ");
            while (!int.TryParse(Console.ReadLine(), out columnas) || columnas <= 0)
            {
                Console.SetCursorPosition(10, 7);
                Console.Write($"► Valor inválido. Ingrese el número de columnas (1-{MAX_COLUMNAS} recomendado): ");
            }

            // Advertir si el tamaño excede lo recomendado
            if (columnas > MAX_COLUMNAS)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(10, 8);
                Console.WriteLine("ADVERTENCIA: El número de columnas excede lo recomendado.");
                Console.SetCursorPosition(10, 9);
                Console.WriteLine("Es posible que algunos elementos no se muestren correctamente.");
                Console.SetCursorPosition(10, 10);
                Console.WriteLine("Presione Enter para continuar o ESC para volver a ingresar el tamaño.");

                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    DibujarMarco(2, 1, 76, 24);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    CentrarTexto("═══ IDENTIFICACIÓN DE NÚMERO MAYOR Y MENOR EN UNA MATRIZ ═══", 3);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    CentrarTexto($"(Tamaño máximo recomendado: {MAX_FILAS}x{MAX_COLUMNAS})", 4);
                    Console.ForegroundColor = ConsoleColor.White;
                    ObtenerTamano();
                    return;
                }
            }
        }

        private void LlenarMatriz()
        {
            Console.Clear();
            DibujarMarco(2, 1, 76, 24);

            Console.ForegroundColor = ConsoleColor.Yellow;
            CentrarTexto("═══ LLENANDO LA MATRIZ ═══", 3);
            Console.ForegroundColor = ConsoleColor.White;

            // Mostrar guía de coordenadas para la matriz
            for (int j = 0; j < columnas; j++)
            {
                if (15 + j * 6 < 75) // Verificar que esté dentro del área visible
                {
                    Console.SetCursorPosition(15 + j * 6, 5);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write($"[C{j + 1}]");
                }
            }

            for (int i = 0; i < filas; i++)
            {
                if (6 + i < 23) // Verificar que esté dentro del área visible
                {
                    Console.SetCursorPosition(10, 6 + i);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write($"[F{i + 1}]");
                }
            }

            Console.ForegroundColor = ConsoleColor.White;

            // Dibujar la matriz inicial
            DibujarMatrizVacia();

            // Llenar la matriz con valores ingresados por el usuario
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    int posX = 15 + j * 6;
                    int posY = 6 + i;

                    // Solo procesar si está dentro del área visible
                    if (posX < 75 && posY < 23)
                    {
                        Console.SetCursorPosition(posX, posY);
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.Write("    ");
                        Console.SetCursorPosition(posX, posY);

                        int valor;
                        bool valorValido = false;
                        while (!valorValido)
                        {
                            string entrada = Console.ReadLine();
                            if (int.TryParse(entrada, out valor))
                            {
                                matriz[i, j] = valor;
                                valorValido = true;
                            }
                            else
                            {
                                Console.SetCursorPosition(posX, posY);
                                Console.Write("    ");
                                Console.SetCursorPosition(posX, posY);
                            }
                        }

                        Console.BackgroundColor = ConsoleColor.Black;

                        // Actualizar solo la celda actual
                        Console.SetCursorPosition(posX, posY);
                        Console.Write($"{matriz[i, j],4}");
                    }
                    else
                    {
                        // Si está fuera del área visible, solo asignar un valor predeterminado
                        matriz[i, j] = 0;
                    }
                }
            }

            // Mensaje para continuar
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(10, Math.Min(filas + 8, 22));
            Console.WriteLine("Presione ENTER para continuar...");
            Console.ReadLine();
        }

        private void DibujarMatrizVacia()
        {
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    int posX = 15 + j * 6;
                    int posY = 6 + i;
                    if (posX < 75 && posY < 23) // Verificar que esté dentro del área visible
                    {
                        Console.SetCursorPosition(posX, posY);
                        Console.Write("    ");
                    }
                }
            }
        }

        private void EncontrarMayorMenor()
        {
            // Inicializar con el primer elemento
            mayor = matriz[0, 0];
            menor = matriz[0, 0];
            filaMayor = 0;
            columnaMayor = 0;
            filaMenor = 0;
            columnaMenor = 0;

            // Buscar mayor y menor
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    if (matriz[i, j] > mayor)
                    {
                        mayor = matriz[i, j];
                        filaMayor = i;
                        columnaMayor = j;
                    }

                    if (matriz[i, j] < menor)
                    {
                        menor = matriz[i, j];
                        filaMenor = i;
                        columnaMenor = j;
                    }
                }
            }
        }

        private void MostrarResultados()
        {
            Console.Clear();
            DibujarMarco(2, 1, 76, 24);

            Console.ForegroundColor = ConsoleColor.Cyan;
            CentrarTexto("═══ RESULTADOS ═══", 3);
            Console.ForegroundColor = ConsoleColor.White;

            // Definir área visible máxima para la matriz
            int filasVisibles = Math.Min(filas, 15);
            int columnasVisibles = Math.Min(columnas, 10);

            // Mostrar coordenadas
            for (int j = 0; j < columnasVisibles; j++)
            {
                Console.SetCursorPosition(15 + j * 6, 5);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write($"[C{j + 1}]");
            }

            for (int i = 0; i < filasVisibles; i++)
            {
                Console.SetCursorPosition(10, 6 + i);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write($"[F{i + 1}]");
            }

            // Mostrar matriz con colores especiales para mayor y menor
            for (int i = 0; i < filasVisibles; i++)
            {
                for (int j = 0; j < columnasVisibles; j++)
                {
                    int posX = 15 + j * 6;
                    int posY = 6 + i;

                    if (posX < 75 && posY < 20) // Verificar que esté dentro del área visible
                    {
                        Console.SetCursorPosition(posX, posY);

                        if (i == filaMayor && j == columnaMayor)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if (i == filaMenor && j == columnaMenor)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;
                        }

                        Console.Write($"{matriz[i, j],4}");
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                }
            }

            // Indicar si hay elementos no visibles
            if (filas > filasVisibles || columnas > columnasVisibles)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(10, filasVisibles + 7);
                Console.WriteLine("Nota: No todos los elementos pueden mostrarse debido al tamaño de la matriz.");
            }

            // Mostrar información del mayor y menor
            int infoY = Math.Min(filasVisibles + 9, 20);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(10, infoY);
            Console.WriteLine("╔═════════════════════════════════════════════════════════════╗");
            Console.SetCursorPosition(10, infoY + 1);
            Console.WriteLine("║                      INFORMACIÓN                            ║");
            Console.SetCursorPosition(10, infoY + 2);
            Console.WriteLine("╠═════════════════════════════════════════════════════════════╣");

            Console.SetCursorPosition(10, infoY + 3);
            Console.Write("║ ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"Número mayor: {mayor,4} │ Posición: Fila {filaMayor + 1}, Columna {columnaMayor + 1}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(70, infoY + 3);
            Console.WriteLine("║");

            Console.SetCursorPosition(10, infoY + 4);
            Console.Write("║ ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"Número menor: {menor,4} │ Posición: Fila {filaMenor + 1}, Columna {columnaMenor + 1}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(70, infoY + 4);
            Console.WriteLine("║");

            Console.SetCursorPosition(10, infoY + 5);
            Console.WriteLine("╚═════════════════════════════════════════════════════════════╝");

            // Mensaje para continuar
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(10, Math.Min(infoY + 7, 22));
            Console.WriteLine("Presione ENTER para continuar...");
            Console.ReadLine();
        }

        private bool MostrarMenu()
        {
            Console.Clear();
            DibujarMarco(2, 1, 76, 20);

            Console.ForegroundColor = ConsoleColor.Yellow;
            CentrarTexto("═══ OPCIONES ═══", 3);

            DibujarMarco(20, 5, 40, 12);

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(30, 7);
            Console.WriteLine("1. Menú anterior");

            Console.SetCursorPosition(30, 9);
            Console.WriteLine("2. Continuar (Nueva matriz)");

            Console.SetCursorPosition(30, 11);
            Console.WriteLine("3. Salir");

            Console.SetCursorPosition(25, 14);
            Console.Write("Seleccione una opción: ");

            int opcion;
            while (!int.TryParse(Console.ReadLine(), out opcion) || opcion < 1 || opcion > 3)
            {
                Console.SetCursorPosition(25, 14);
                Console.Write("Opción inválida. Seleccione una opción: ");
            }

            switch (opcion)
            {
                case 1:
                    return false; // Regresar al menú principal
                case 2:
                    return true; // Continuar con este programa
                case 3:
                    Environment.Exit(0); // Salir del programa completo
                    return false;
                default:
                    return false;
            }
        }
    }
}