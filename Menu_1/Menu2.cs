using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Menu_1
{
    internal class Menu2
    {
        public void MostrarMenu2()
        {
            bool salir = false;

            while (!salir)
            {
                Console.SetCursorPosition(40, 6);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Menú 2. Programas de localización.");

                Console.SetCursorPosition(10, 9);
                Console.WriteLine("Selecciona una opción del menú \n");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(10, 10);
                Console.WriteLine("(1) Mostrar la tabla de senos del 0 al 90.");
                Console.SetCursorPosition(10, 11);
                Console.WriteLine("(2) Mostrar la tabla de cosenos del 0 al 90.");
                Console.SetCursorPosition(10, 12);
                Console.WriteLine("(3) Calcular hipotenusa y ángulos de un triángulo rectángulo a partir de los catetos.");
                Console.SetCursorPosition(10, 13);
                Console.WriteLine("(4) Calcular pendiente, ángulo de inclinación y punto medio de una recta dados dos puntos.");
                Console.SetCursorPosition(10, 14);
                Console.WriteLine("(5) Calcular coordenadas, altura, velocidad y distancia máxima de un proyectil en intervalos de tiempo.");
                Console.SetCursorPosition(10, 15);
                Console.WriteLine("(6) Regresar al Menú Principal");
                Console.SetCursorPosition(10, 16);
                Console.WriteLine("(7) Salir.");
                Console.SetCursorPosition(10, 17);
                Console.ForegroundColor = ConsoleColor.Blue;
                var opcion = int.Parse(Console.ReadLine());

                switch (opcion)
                {
                    case 1:
                        Console.Clear();
                        TablaSenos();
                        break;
                    case 2:
                        Console.Clear();
                        TablaCosenos();
                        break;
                    case 3:
                        Console.Clear();
                        calcularTrianguloRectangulo();
                        break;
                    case 4:
                        Console.Clear();
                        calcularRecta();
                        break;
                    case 5:
                        Console.Clear();
                        calcularTrayectoriaProyectil();
                        break;
                    case 6:
                        return;
                    case 7:
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida, por favor intenta de nuevo.");
                        break;
                }
            }
        }

        private static void TablaSenos()
        {
            double Factorial(int n)
            {
                double fact = 1;
                for (int i = 2; i <= n; i++)
                    fact *= i;
                return fact;
            }

            double GradosARadianes(double grados)
            {
                return grados * (Math.PI) / 180.0; // Convertimos grados a radianes con mayor precisión
            }

            double Seno(double x)
            {
                x = GradosARadianes(x);
                double sum = 0;
                for (int i = 0; i < 10; i++)
                {
                    double term = Math.Pow(-1, i) * Math.Pow(x, 2 * i + 1) / Factorial(2 * i + 1);
                    sum += term;
                }
                return sum;
            }

            int columnas = 7;  
            int filas = 13;    
            int espacioX = 15; 
            int espacioY = 1;  

            Console.ForegroundColor = ConsoleColor.Red;
            string titulo = "Tabla del Seno de 0° a 90°";
            int anchoConsola = Console.WindowWidth;
            int altoConsola = Console.WindowHeight;
            int posicionTituloX = (anchoConsola - titulo.Length) / 2;
            int posicionTituloY = 2;
            Console.SetCursorPosition(posicionTituloX, posicionTituloY);
            Console.WriteLine(titulo);

            int inicioY = 5; // Ajustamos la posición de inicio

            int angulo = 0; // Iniciamos en 0°

            
            for (int j = 0; j < columnas; j++)
            {
                for (int i = 0; i < filas; i++)
                {
                    if (angulo > 90) break; // No imprimir más allá de 90°

                    double seno = Seno(angulo);

                    int posY = inicioY + (i * espacioY);
                    int posX = (anchoConsola - (columnas * espacioX)) / 2 + (j * espacioX);

                    Console.SetCursorPosition(posX, posY);
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write($"{angulo,2}°: {seno:F2}");

                    Thread.Sleep(50); 

                    angulo += 1; 
                }

                Thread.Sleep(100); 
            }

            Console.SetCursorPosition(40, 25);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Listo!!! Presiona espacio para continuar");
            Console.ReadKey();
            Console.Clear();
        }

        private static void TablaCosenos()
        {

            double CalcularPi()
            {
                // Aproximación de pi utilizando la serie de Leibniz
                double pi = 0;
                for (int i = 0; i < 1000; i++)
                {
                    pi += (4 * Math.Pow(-1, i)) / (2 * i + 1);
                }
                return pi;
            }

            double GradosARadianes(double grados)
            {
                double pi = CalcularPi();
                return grados * (pi / 180.0); // Convertimos grados a radianes sin Math.PI
            }

            double Coseno(double x)
            {
                x = GradosARadianes(x); // Convertimos grados a radianes
                double sum = 1; // El primer término de la serie de Taylor es 1
                double term = 1; // Este es el valor del término actual en la serie
                int i = 2;

                // Calculamos los primeros 10 términos de la serie de Taylor para el coseno
                while (Math.Abs(term) > 1e-10) // Lo calculamos hasta que el término sea muy pequeño
                {
                    term *= -x * x / ((i - 1) * i); // Calculamos el siguiente término
                    sum += term; // Sumamos el término a la suma
                    i += 2;
                }

                return sum;
            }

            int columnas = 7;
            int filas = 13;
            int espacioX = 15;
            int espacioY = 1;

            Console.ForegroundColor = ConsoleColor.Red;
            string titulo = "Tabla del Coseno de 0° a 90°";
            int anchoConsola = Console.WindowWidth;
            int posicionTituloX = (anchoConsola - titulo.Length) / 2;
            Console.SetCursorPosition(posicionTituloX, 2);
            Console.WriteLine(titulo);

            int inicioY = 5;
            int angulo = 0;

            for (int j = 0; j < columnas; j++)
            {
                for (int i = 0; i < filas; i++)
                {
                    if (angulo > 90) break;

                    double coseno = Coseno(angulo);

                    int posY = inicioY + (i * espacioY);
                    int posX = (anchoConsola - (columnas * espacioX)) / 2 + (j * espacioX);

                    Console.SetCursorPosition(posX, posY);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"{angulo,2}°: {coseno:F2}");

                    Thread.Sleep(50);

                    angulo += 1;
                }

                Thread.Sleep(100);
            }

            Console.SetCursorPosition(40, 25);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Listo!!! Presiona espacio para continuar");
            Console.ReadKey();
            Console.Clear();
        }

        private static void calcularTrianguloRectangulo()
        {
        }

        private static void calcularRecta()
        {
        }

        private static void calcularTrayectoriaProyectil()
        {
        }
    }
}
