
using MenuLocalizacionVM;

internal class Program
{
    private static void Main(string[] args)
    {
        bool salir = false;
        while (!salir)
        {
            Console.Clear();
            Console.SetCursorPosition(35, 6);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Menú. Localización de vectores y matrices");

            Console.SetCursorPosition(30, 8);
            Console.WriteLine("Selecciona una opción del menú\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(30, 9);
            Console.WriteLine("(1) Juego del ahorcado para cualquier palabra.");
            Console.SetCursorPosition(30, 10);
            Console.WriteLine("(2) Análisis de una matriz (positivos, negativos y ceros).");
            Console.SetCursorPosition(30, 11);
            Console.WriteLine("(3) Transposición de una matriz.");
            Console.SetCursorPosition(30, 12);
            Console.WriteLine("(4) Identificación de número mayor y menor en una matriz.");
            Console.SetCursorPosition(30, 13);
            Console.WriteLine("(5) Cálculos sobre números pares e impares en una matriz.");
            Console.SetCursorPosition(30, 14);
            Console.WriteLine("(6) Suma de filas y columnas de una matriz 3x3.");
            Console.SetCursorPosition(30, 15);
            Console.WriteLine("(7) Multiplicación de dos matrices.");
            Console.SetCursorPosition(30, 16);
            Console.WriteLine("(8) Cálculo de desviación estándar.");
            Console.SetCursorPosition(30, 17);
            Console.WriteLine("(9) Salir.");
            Console.SetCursorPosition(30, 18);
            Console.ForegroundColor = ConsoleColor.Blue;

            if (int.TryParse(Console.ReadLine(), out int opcion))
            {
                switch (opcion)
                {
                    case 1:
                        Console.Clear();
                        ahorcado juego = new ahorcado();
                        juego.Jugar();
                        break;
                    case 2:
                        Console.Clear();
                        AnalizadorMatriz analizador = new AnalizadorMatriz();
                        analizador.Ejecutar();
                        break;
                    case 3:
                        Console.Clear();
                        TransposicionMatriz transposicion = new TransposicionMatriz();
                        transposicion.Ejecutar();
                        break;
                    case 4:
                        Console.Clear();
                        IdentificadorMayorMenor identificador = new IdentificadorMayorMenor();
                        identificador.Ejecutar();
                        break;
                    case 5:
                        Console.Clear();
                        CalculadoraParesImpares calculadora = new CalculadoraParesImpares();
                        calculadora.Ejecutar();
                        break;
                    case 6:
                        Console.Clear();
                        SumaFilasColumnas sumador = new SumaFilasColumnas();
                        sumador.Ejecutar();
                        break;
                    case 7:
                        Console.Clear();
                        MultiplicacionMatrices multiplicador = new MultiplicacionMatrices();
                        multiplicador.Ejecutar();
                        break;
                    case 8:
                        Console.Clear();
                        DesviacionEstandar calculador = new DesviacionEstandar();
                        calculador.Ejecutar();
                        break;
                    case 9:
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Presiona una tecla para continuar.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
