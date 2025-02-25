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
        public static void calcularTrianguloRectangulo()
        {
            Thread.Sleep(200);
            int altura, baseTriangulo;

            // Validar la entrada para la altura
            while (true)
            {
                Console.Clear();
                Console.SetCursorPosition(5, 2);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Ingrese la altura (cateto opuesto): ");
                string inputAltura = Console.ReadLine();

                if (int.TryParse(inputAltura, out altura) && altura > 0)
                    break;

                Console.SetCursorPosition(5, 4);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Ingrese un número entero positivo.");
                Thread.Sleep(1000);
            }

            // Validar la entrada para la base
            while (true)
            {
                Console.Clear();
                Console.SetCursorPosition(5, 2);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Ingrese la altura (cateto opuesto): " + altura); // Mostrar la altura ingresada

                Console.SetCursorPosition(5, 3);
                Console.Write("Ingrese la base (cateto adyacente): ");
                string inputBase = Console.ReadLine();

                if (int.TryParse(inputBase, out baseTriangulo) && baseTriangulo > 0)
                    break;

                Console.SetCursorPosition(5, 5);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Ingrese un número entero positivo.");
                Thread.Sleep(1000);
            }

            // Limpiar pantalla antes de continuar
            Console.Clear();

            // Código original sin modificaciones
            Console.SetCursorPosition(5, 2);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Ingrese la altura (cateto opuesto): " + altura);

            Console.SetCursorPosition(5, 3);
            Console.Write("Ingrese la base (cateto adyacente): " + baseTriangulo);

            // Definir tamaño fijo del triángulo en la consola
            int alturaFija = 10;
            int baseFija = 20;

            // Escalar los valores ingresados para ajustarlos al tamaño fijo
            double factorAltura = (double)alturaFija / altura;
            double factorBase = (double)baseFija / baseTriangulo;
            double factorEscala = Math.Min(factorAltura, factorBase); // Se elige el menor para mantener proporción

            int alturaEscalada = (int)(altura * factorEscala);
            int baseEscalada = (int)(baseTriangulo * factorEscala);

            // Posición inicial del triángulo en la consola
            int startX = 20;
            int startY = 5;

            Console.ForegroundColor = ConsoleColor.Blue;

            // 1. Dibujar el lado izquierdo (vertical)
            for (int i = 0; i <= alturaEscalada; i++)
            {
                Console.SetCursorPosition(startX, startY + i);
                Console.Write("*");
                Thread.Sleep(50);
            }

            // 2. Dibujar la base (horizontal)
            for (int i = 0; i <= baseEscalada * 2; i++)
            {
                Console.SetCursorPosition(startX + i, startY + alturaEscalada);
                Console.Write("*");
                Thread.Sleep(50);
            }

            // 3. Dibujar la hipotenusa (diagonal)
            for (int i = 1; i <= alturaEscalada; i++)
            {
                Console.SetCursorPosition(startX + (i * baseEscalada * 2 / alturaEscalada), startY + i);
                Console.Write("*");
                Thread.Sleep(50);
            }

            // 4. Mostrar los datos dentro de la figura
            Console.SetCursorPosition(startX - 4, startY + alturaEscalada / 2);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{altura}");

            Console.SetCursorPosition(startX + baseEscalada, startY + alturaEscalada + 1);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{baseTriangulo}");

            // Cálculo de la hipotenusa y ángulos
            double hipotenusa = Math.Sqrt(Math.Pow(altura, 2) + Math.Pow(baseTriangulo, 2));
            double angulo1Rad = Math.Atan((double)altura / baseTriangulo);
            double angulo2Rad = Math.PI / 2 - angulo1Rad;
            double angulo1 = angulo1Rad * (180 / Math.PI);
            double angulo2 = angulo2Rad * (180 / Math.PI);

            // Mostrar datos en pantalla
            Console.SetCursorPosition(60, 10);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Hipotenusa: {hipotenusa:F2}");

            Console.SetCursorPosition(60, 12);
            Console.WriteLine($"Ángulo 1: {angulo1:F2}°");

            Console.SetCursorPosition(60, 14);
            Console.WriteLine($"Ángulo 2: {angulo2:F2}°");

            // Pausa antes de redibujar
            Console.SetCursorPosition(60, 17);
            Console.Write("Presiona una tecla para ver todos los datos...");
            Console.ReadKey();

            // 5. Redibujar figura con los datos
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            for (int i = 0; i <= alturaEscalada; i++)
            {
                Console.SetCursorPosition(startX, startY + i);
                Console.Write("*");
            }

            for (int i = 0; i <= baseEscalada * 2; i++)
            {
                Console.SetCursorPosition(startX + i, startY + alturaEscalada);
                Console.Write("*");
            }

            // Dibujar la hipotenusa
            for (int i = 1; i <= alturaEscalada; i++)
            {
                Console.SetCursorPosition(startX + (i * baseEscalada * 2 / alturaEscalada), startY + i);
                Console.Write("*");
            }

            // Mostrar datos dentro del triángulo
            Console.SetCursorPosition(startX - 4, startY + alturaEscalada / 2);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{altura}");

            Console.SetCursorPosition(startX + baseEscalada, startY + alturaEscalada + 1);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{baseTriangulo}");

            Console.SetCursorPosition(startX + baseEscalada, startY + alturaEscalada / 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{hipotenusa:F2}");

            Console.SetCursorPosition(startX + baseEscalada - 2, startY + alturaEscalada - 1);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{angulo1:F1}°");

            Console.SetCursorPosition(startX + 2, startY + 1);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{angulo2:F1}°");

            // Mostrar valores en la parte derecha
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(60, 10);
            Console.WriteLine($"Hipotenusa: {hipotenusa:F2}");

            Console.SetCursorPosition(60, 12);
            Console.WriteLine($"Ángulo 1: {angulo1:F2}°");

            Console.SetCursorPosition(60, 14);
            Console.WriteLine($"Ángulo 2: {angulo2:F2}°");

            // Mensaje de finalización
            Console.SetCursorPosition(40, 25);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Listo!!! Presiona espacio para continuar");

            Console.ReadKey();
            Console.Clear();
        }



        private static void calcularRecta()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;

            // Solicitar los puntos al usuario con validación
            Console.WriteLine("=== Cálculo de la recta ===\n");
            double x1 = SolicitarCoordenada("Ingrese la coordenada x1: ");
            double y1 = SolicitarCoordenada("Ingrese la coordenada y1: ");
            double x2;

            // Validar que x2 no sea igual a x1 para evitar división por cero
            while (true)
            {
                x2 = SolicitarCoordenada("Ingrese la coordenada x2: ");
                if (x2 != x1)
                    break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: x2 no puede ser igual a x1. La recta sería vertical y la pendiente indefinida.");
                Console.ResetColor();
            }

            double y2 = SolicitarCoordenada("Ingrese la coordenada y2: ");

            // Calcular pendiente (m)
            double pendiente = (y2 - y1) / (x2 - x1);
            // Calcular ángulo de inclinación en grados
            double angulo = Math.Atan(pendiente) * (180 / Math.PI);
            // Calcular punto medio
            double xm = (x1 + x2) / 2;
            double ym = (y1 + y2) / 2;

            // Mostrar resultados
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nResultados:");
            Console.WriteLine($"Pendiente (m): {pendiente:F2}");
            Console.WriteLine($"Ángulo de inclinación: {angulo:F2}°");
            Console.WriteLine($"Punto medio: ({xm:F2}, {ym:F2})\n");

            // Dibujar el sistema de coordenadas y la recta
            Console.WriteLine("\nDibujando la recta, espere...");
            Thread.Sleep(1500); // Pequeña pausa para dar tiempo a leer los resultados
            Console.Clear(); // Limpiar pantalla antes de dibujar la recta
            DibujarPlano(pendiente, x1, y1, x2, y2, xm, ym);

            // Mensaje final
            Console.SetCursorPosition(5, 25);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Presiona cualquier tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
        }

        private static double SolicitarCoordenada(string mensaje)
        {
            double valor;
            Console.Write(mensaje);
            while (!double.TryParse(Console.ReadLine(), out valor))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Entrada no válida. Inténtelo de nuevo: ");
                Console.ResetColor();
            }
            return valor;
        }

        private static void DibujarPlano(double pendiente, double x1, double y1, double x2, double y2, double xm, double ym)
        {
            int ancho = 60; // Más ancho para evitar sobreescribir texto
            int alto = 25;
            int origenX = 20; // Mover más a la derecha
            int origenY = 15; // Mover más abajo

            Console.ForegroundColor = ConsoleColor.Gray;

            // Dibujar ejes
            for (int i = 0; i < ancho; i++)
            {
                Console.SetCursorPosition(i + 10, origenY); // Mueve el eje X más a la derecha
                Console.Write("-");
            }
            for (int i = 0; i < alto; i++)
            {
                Console.SetCursorPosition(origenX, i + 5); // Mueve el eje Y más abajo
                Console.Write("|");
            }

            // Dibujar la recta
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int i = -ancho / 2; i <= ancho / 2; i++)
            {
                int y = (int)(pendiente * i + (y1 - pendiente * x1));
                int posX = origenX + i + 10; // Ajuste para mover más a la derecha
                int posY = origenY - y + 5;  // Ajuste para mover más abajo

                if (posX >= 0 && posX < ancho + 10 && posY >= 0 && posY < alto + 5)
                {
                    Console.SetCursorPosition(posX, posY);
                    Console.Write("*");
                    Thread.Sleep(5);
                }
            }

            // Dibujar los puntos dados
            DibujarPunto(origenX + (int)x1 + 10, origenY - (int)y1 + 5, "X1", ConsoleColor.Red);
            DibujarPunto(origenX + (int)x2 + 10, origenY - (int)y2 + 5, "X2", ConsoleColor.Red);
            DibujarPunto(origenX + (int)xm + 10, origenY - (int)ym + 5, "M", ConsoleColor.Yellow);
        }

        private static void DibujarPunto(int x, int y, string etiqueta, ConsoleColor color)
        {
            if (x >= 0 && x < 70 && y >= 0 && y < 30) // Asegurar que el punto está dentro de los límites
            {
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = color;
                Console.Write(etiqueta);
            }
        }

        private static void calcularTrayectoriaProyectil()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== Cálculo de la trayectoria de un proyectil ===\n");

            // Solicitar y validar la velocidad inicial
            double velocidadInicial;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Ingrese la velocidad inicial (m/s): ");
                if (double.TryParse(Console.ReadLine(), out velocidadInicial) && velocidadInicial > 0)
                    break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Error: Ingrese un número válido mayor que 0.");
            }

            // Solicitar y validar el ángulo de lanzamiento
            double anguloGrados;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Ingrese el ángulo de lanzamiento (grados, entre 0 y 90): ");
                if (double.TryParse(Console.ReadLine(), out anguloGrados) && anguloGrados > 0 && anguloGrados <= 90)
                    break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Error: Ingrese un ángulo válido entre 0 y 90 grados.");
            }

            // Convertir ángulo a radianes
            double anguloRadianes = anguloGrados * (Math.PI / 180);

            // Componentes de velocidad
            double vx = velocidadInicial * Math.Cos(anguloRadianes); // Velocidad en X
            double vy = velocidadInicial * Math.Sin(anguloRadianes); // Velocidad en Y

            double g = 9.81; // Gravedad (m/s²)
            double tiempoVuelo = (2 * vy) / g; // Tiempo total de vuelo
            double alturaMaxima = (vy * vy) / (2 * g); // Altura máxima
            double distanciaMaxima = vx * tiempoVuelo; // Distancia total

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nTiempo total de vuelo: {tiempoVuelo:F2} s");
            Console.WriteLine($"Altura máxima alcanzada: {alturaMaxima:F2} m");
            Console.WriteLine($"Distancia máxima alcanzada: {distanciaMaxima:F2} m\n");

            // Dibujar la trayectoria en la consola
            DibujarTrayectoria(velocidadInicial, anguloRadianes, tiempoVuelo, vx, vy, g, distanciaMaxima, alturaMaxima);

            Console.SetCursorPosition(5, 25);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Presiona cualquier tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
        }

        private static void DibujarTrayectoria(double v0, double angulo, double tiempoVuelo, double vx, double vy, double g, double distanciaMaxima, double alturaMaxima)
        {
            int ancho = 60;
            int alto = 20;
            char[,] grafico = new char[alto, ancho];

            // Inicializar el plano vacío
            for (int i = 0; i < alto; i++)
            {
                for (int j = 0; j < ancho; j++)
                {
                    grafico[i, j] = ' ';
                }
            }

            // Simulación de la trayectoria
            double t = 0;
            double intervalo = 0.1; // Intervalo de tiempo

            while (t <= tiempoVuelo)
            {
                double x = vx * t;
                double y = (vy * t) - (0.5 * g * t * t);

                int posX = (int)(x / distanciaMaxima * (ancho - 1));
                int posY = alto - 1 - (int)(y / alturaMaxima * (alto - 1));

                if (posX >= 0 && posX < ancho && posY >= 0 && posY < alto)
                {
                    grafico[posY, posX] = '*';
                }

                t += intervalo;
            }

            // Dibujar los ejes
            for (int i = 0; i < alto; i++)
            {
                grafico[i, 0] = '|'; // Eje Y
            }
            for (int j = 0; j < ancho; j++)
            {
                grafico[alto - 1, j] = '-'; // Eje X
            }

            // Mostrar el gráfico en consola
            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 0; i < alto; i++)
            {
                for (int j = 0; j < ancho; j++)
                {
                    Console.Write(grafico[i, j]);
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLos * representa la trayectoria del proyectil.");
        }


    }
}
