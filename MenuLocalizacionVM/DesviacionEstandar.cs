using System;
using System.Threading;
using System.Collections.Generic;

namespace MenuLocalizacionVM
{
    public class DesviacionEstandar
    {
        // Ancho y alto de la consola
        private int _consoleWidth;
        private int _consoleHeight;

        // Colores personalizados
        private readonly ConsoleColor _titleColor = ConsoleColor.Cyan;
        private readonly ConsoleColor _formulaColor = ConsoleColor.Yellow;
        private readonly ConsoleColor _resultColor = ConsoleColor.Green;
        private readonly ConsoleColor _errorColor = ConsoleColor.Red;
        private readonly ConsoleColor _inputColor = ConsoleColor.White;
        private readonly ConsoleColor _calcColor = ConsoleColor.DarkCyan;
        private readonly ConsoleColor _navigationColor = ConsoleColor.Magenta;

        // Lista para almacenar líneas de cálculo para scrolling
        private List<string> _calculationLines = new List<string>();

        public void Ejecutar()
        {
            bool continuar = true;

            while (continuar)
            {
                // Obtener dimensiones actuales de la consola
                _consoleWidth = Console.WindowWidth;
                _consoleHeight = Console.WindowHeight;

                Console.Clear();
                MostrarTitulo("CÁLCULO DE DESVIACIÓN ESTÁNDAR");

                // Solicitar el número de datos
                int n = SolicitarNumeroDatos();
                if (n <= 1) continue;

                // Crear arreglo para almacenar los datos
                double[] datos = new double[n];

                // Solicitar los valores de los datos
                SolicitarDatos(datos);

                // Calcular la desviación estándar
                MostrarCalculo(datos);

                // Menú de opciones
                continuar = MostrarMenu();
            }
        }

        private int SolicitarNumeroDatos()
        {
            int centroX = _consoleWidth / 2 - 15;
            Console.ForegroundColor = _inputColor;
            Console.SetCursorPosition(centroX, 5);
            Console.Write("Ingrese el número de datos (n): ");

            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 1)
            {
                Console.SetCursorPosition(centroX, 7);
                Console.ForegroundColor = _errorColor;
                Console.WriteLine("Debe ingresar un número entero mayor a 1.");
                Console.ForegroundColor = _inputColor;
                Console.SetCursorPosition(centroX, 9);
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return 0;
            }

            return n;
        }

        private void SolicitarDatos(double[] datos)
        {
            int centroX = _consoleWidth / 2 - 15;
            int maxVisibleInputs = _consoleHeight - 12; // Reservar espacio para título y mensajes
            int currentPage = 0;
            int totalPages = (int)Math.Ceiling(datos.Length / (double)maxVisibleInputs);

            for (int i = 0; i < datos.Length; i++)
            {
                // Verificar si necesitamos cambiar de página
                if (i > 0 && i % maxVisibleInputs == 0)
                {
                    currentPage++;
                    Console.ForegroundColor = _navigationColor;
                    Console.SetCursorPosition(centroX, _consoleHeight - 2);
                    Console.WriteLine($"Página {currentPage}/{totalPages} - Presione ENTER para continuar...");
                    Console.ReadKey(true);
                    Console.Clear();
                    MostrarTitulo("CÁLCULO DE DESVIACIÓN ESTÁNDAR");
                    Console.SetCursorPosition(centroX, 7);
                    Console.ForegroundColor = _inputColor;
                    Console.WriteLine($"Continúe ingresando los valores (página {currentPage + 1}/{totalPages}):");
                }

                // Calcular la posición Y relativa a la página actual
                int relativeY = 9 + (i % maxVisibleInputs);

                Console.SetCursorPosition(centroX, relativeY);
                Console.ForegroundColor = _inputColor;
                Console.Write($"Valor {i + 1}: ");

                if (!double.TryParse(Console.ReadLine(), out datos[i]))
                {
                    Console.SetCursorPosition(centroX + 15, relativeY);
                    Console.ForegroundColor = _errorColor;
                    Console.WriteLine("Error: Debe ingresar un número válido.");
                    Console.ForegroundColor = _inputColor;
                    i--; // Volver a pedir el mismo dato
                }
            }
        }

        private void MostrarCalculo(double[] datos)
        {
            // Limpiar lista de líneas para el nuevo cálculo
            _calculationLines.Clear();

            int n = datos.Length;
            int centroX = _consoleWidth / 2 - 25;

            // Calcular la media
            double media = CalcularMedia(datos);

            // Agregar líneas a la lista
            _calculationLines.Add($"Media (x̄) = {media:F4}");
            _calculationLines.Add("");

            // Calcular la suma de los cuadrados de las diferencias
            double sumaCuadrados = 0;
            for (int i = 0; i < n; i++)
            {
                double diferencia = datos[i] - media;
                double cuadrado = diferencia * diferencia;
                sumaCuadrados += cuadrado;

                // Agregar línea al listado
                _calculationLines.Add($"(x{i + 1} - x̄)² = ({datos[i]:F4} - {media:F4})² = {diferencia:F4}² = {cuadrado:F4}");
            }

            // Calcular la desviación estándar
            double desviacionEstandar = Math.Sqrt(sumaCuadrados / (n - 1));

            // Agregar líneas de resultado
            _calculationLines.Add("");
            _calculationLines.Add($"Suma de cuadrados = {sumaCuadrados:F4}");
            _calculationLines.Add($"Suma de cuadrados / (n-1) = {sumaCuadrados:F4} / {n - 1} = {sumaCuadrados / (n - 1):F4}");
            _calculationLines.Add($"Desviación Estándar = √({sumaCuadrados / (n - 1):F4}) = {desviacionEstandar:F4}");
            _calculationLines.Add("");

            // Mostrar los resultados con scroll
            MostrarResultadosConScroll(desviacionEstandar);
        }

        private void MostrarResultadosConScroll(double resultado)
        {
            int currentLine = 0;
            int centroX = _consoleWidth / 2 - 25;
            // Reservamos espacio para título, fórmula, barra de navegación y resultado
            int linesPerPage = _consoleHeight - 25;

            bool continuar = true;

            while (continuar)
            {
                Console.Clear();
                MostrarTitulo("CÁLCULO DE DESVIACIÓN ESTÁNDAR");

                // Dibujar la fórmula mejorada
                DibujarFormulaMejorada(centroX, 5);

                // Mostrar las líneas visibles actualmente
                Console.ForegroundColor = _calcColor;
                for (int i = 0; i < linesPerPage && (currentLine + i) < _calculationLines.Count; i++)
                {
                    Console.SetCursorPosition(centroX, 14 + i);
                    Console.WriteLine(_calculationLines[currentLine + i]);
                }

                // Dibujar una línea separadora
                Console.ForegroundColor = _formulaColor;
                Console.SetCursorPosition(centroX, _consoleHeight - 10);
                Console.WriteLine(new string('─', 50));

                // Siempre mostrar el resultado final en una posición fija
                MostrarResultado(resultado, _consoleHeight - 8);

                // Dibujar otra línea separadora para la navegación
                Console.ForegroundColor = _formulaColor;
                Console.SetCursorPosition(centroX, _consoleHeight - 4);
                Console.WriteLine(new string('─', 50));

                // Área de navegación con instrucciones claras
                Console.ForegroundColor = _navigationColor;
                Console.SetCursorPosition(centroX, _consoleHeight - 3);
                int lineasVisibles = Math.Min(linesPerPage, _calculationLines.Count - currentLine);
                Console.WriteLine($"Mostrando líneas {currentLine + 1} a {currentLine + lineasVisibles} de {_calculationLines.Count}");

                Console.SetCursorPosition(centroX, _consoleHeight - 2);
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                Console.WriteLine("Use ↑ - ↓ para navegar por los cálculos (línea por línea)");
                Console.SetCursorPosition(centroX, _consoleHeight - 1);
                Console.WriteLine("Presione ENTER cuando termine de revisar los resultados");

                // Leer tecla
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentLine > 0)
                            currentLine--;
                        break;

                    case ConsoleKey.DownArrow:
                        if (currentLine < _calculationLines.Count - 1 && currentLine + linesPerPage < _calculationLines.Count)
                            currentLine++;
                        break;

                    case ConsoleKey.PageUp:
                        currentLine = Math.Max(0, currentLine - linesPerPage);
                        break;

                    case ConsoleKey.PageDown:
                        if (currentLine + linesPerPage < _calculationLines.Count)
                            currentLine = Math.Min(_calculationLines.Count - 1, currentLine + linesPerPage);
                        break;

                    case ConsoleKey.Home:
                        currentLine = 0;
                        break;

                    case ConsoleKey.End:
                        currentLine = Math.Max(0, _calculationLines.Count - linesPerPage);
                        break;

                    case ConsoleKey.Enter:
                    case ConsoleKey.Escape:
                        continuar = false;
                        break;
                }
            }
        }

        private double CalcularMedia(double[] datos)
        {
            double suma = 0;
            foreach (double dato in datos)
            {
                suma += dato;
            }
            return suma / datos.Length;
        }

        private bool MostrarMenu()
        {
            int centroX = _consoleWidth / 2 - 15;

            Console.Clear();
            Console.ForegroundColor = _titleColor;
            Console.SetCursorPosition(centroX, 10);
            Console.WriteLine("¿Qué desea hacer?");

            // Dibujar cuadro para opciones del menú
            Console.ForegroundColor = _titleColor;
            Console.SetCursorPosition(centroX - 2, 11);
            Console.WriteLine("┌" + new string('─', 40) + "┐");

            // Opciones del menú
            Console.ForegroundColor = _inputColor;
            Console.SetCursorPosition(centroX, 12);
            Console.WriteLine("1. Regresar al menú anterior");
            Console.SetCursorPosition(centroX, 13);
            Console.WriteLine("2. Calcular otra desviación estándar");
            Console.SetCursorPosition(centroX, 14);
            Console.WriteLine("3. Salir del programa");

            // Cerrar cuadro
            Console.ForegroundColor = _titleColor;
            Console.SetCursorPosition(centroX - 2, 15);
            Console.WriteLine("└" + new string('─', 40) + "┘");

            // Prompt
            Console.ForegroundColor = _inputColor;
            Console.SetCursorPosition(centroX, 17);
            Console.Write("Seleccione una opción (1-3): ");

            ConsoleKeyInfo tecla = Console.ReadKey();
            Console.WriteLine();

            switch (tecla.KeyChar)
            {
                case '1':
                    return false; // Volver al menú principal
                case '2':
                    return true; // Volver a ejecutar este programa
                case '3':
                    Environment.Exit(0); // Salir del programa completamente
                    return false;
                default:
                    Console.SetCursorPosition(centroX, 19);
                    Console.ForegroundColor = _errorColor;
                    Console.WriteLine("Opción no válida. Debe seleccionar 1, 2 o 3.");
                    Console.ForegroundColor = _inputColor;
                    Console.SetCursorPosition(centroX, 21);
                    Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                    return true;
            }
        }

        private void MostrarTitulo(string titulo)
        {
            int centroX = _consoleWidth / 2 - titulo.Length / 2;
            Console.ForegroundColor = _titleColor;
            Console.SetCursorPosition(centroX, 2);
            Console.WriteLine("╔" + new string('═', titulo.Length + 2) + "╗");
            Console.SetCursorPosition(centroX, 3);
            Console.WriteLine("║ " + titulo + " ║");
            Console.SetCursorPosition(centroX, 4);
            Console.WriteLine("╚" + new string('═', titulo.Length + 2) + "╝");
            Console.ForegroundColor = _inputColor;
        }

        private void MostrarResultado(double resultado, int linea)
        {
            string textoResultado = $"  Resultado: Desviación Estándar = {resultado:F4}  ";
            int centroX = _consoleWidth / 2 - textoResultado.Length / 2;

            Console.ForegroundColor = _resultColor;
            // Dibujar caja superior
            Console.SetCursorPosition(centroX, linea);
            Console.WriteLine("╔" + new string('═', textoResultado.Length) + "╗");

            // Dibujar contenido
            Console.SetCursorPosition(centroX, linea + 1);
            Console.WriteLine("║" + textoResultado + "║");

            // Dibujar caja inferior
            Console.SetCursorPosition(centroX, linea + 2);
            Console.WriteLine("╚" + new string('═', textoResultado.Length) + "╝");

            Console.ForegroundColor = _inputColor;
        }

        private void DibujarFormulaMejorada(int x, int y)
        {
            Console.ForegroundColor = _formulaColor;

            // Desviación Estándar =
            Console.SetCursorPosition(x, y);
            Console.WriteLine("Desviación Estándar =");

            // Símbolo de raíz cuadrada mejorado
            Console.SetCursorPosition(x + 22, y);
            Console.Write("╱");

            // Línea horizontal superior
            Console.SetCursorPosition(x + 24, y);
            Console.WriteLine("─────────────────────────");

            // Parte superior de la fórmula
            Console.SetCursorPosition(x + 24, y + 1);
            Console.WriteLine("   n");
            Console.SetCursorPosition(x + 24, y + 2);
            Console.WriteLine("  ∑   (xᵢ - x̄)²");
            Console.SetCursorPosition(x + 24, y + 3);
            Console.WriteLine(" i=1");

            // Línea horizontal para la fracción
            Console.SetCursorPosition(x + 24, y + 4);
            Console.WriteLine("─────────────────────────");

            // Parte inferior de la fórmula
            Console.SetCursorPosition(x + 24, y + 5);
            Console.WriteLine("        n - 1");

            // Completa el símbolo de raíz
            Console.SetCursorPosition(x + 22, y + 1);
            Console.WriteLine("│");
            Console.SetCursorPosition(x + 22, y + 2);
            Console.WriteLine("│");
            Console.SetCursorPosition(x + 22, y + 3);
            Console.WriteLine("│");
            Console.SetCursorPosition(x + 22, y + 4);
            Console.WriteLine("│");
            Console.SetCursorPosition(x + 22, y + 5);
            Console.WriteLine("│");
            Console.SetCursorPosition(x + 20, y + 6);
            Console.WriteLine("╲╱");

            Console.ForegroundColor = _inputColor;
        }
    }
}