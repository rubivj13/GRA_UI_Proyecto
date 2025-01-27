internal class Program
{
    private static void Main(string[] args)
    {
        bool salir = false;
        while (!salir)
        {
            Console.SetCursorPosition(50, 6);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Menú 1 - Tema 2");

            Console.SetCursorPosition(40, 9);
            Console.WriteLine("Selecciona una opción del menú\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(40, 10);
            Console.WriteLine("(1) Dibujar grafica con *");
            Console.SetCursorPosition(40, 11);
            Console.WriteLine("(2) Dibujar espiral con *");
            Console.SetCursorPosition(40, 12);
            Console.WriteLine("(3) Salir");
            Console.SetCursorPosition(40, 13);
            Console.ForegroundColor = ConsoleColor.Blue;
            var opcion = int.Parse(Console.ReadLine());
            switch (opcion)
            {
                case 1:
                    Console.Clear();
                    DibujarGrafica();
                    break;
                case 2:
                    Console.Clear();
                    DibujarEspiral();
                    break;
                case 3:
                    salir = true;
                    break;
                default:
                    Console.WriteLine("Opción no válida, por favor intenta de nuevo.");
                    break;
            }
        }

    }
    private static void DibujarGrafica()
    {
        int x = 100; 
        int y = 23; 

        bool arriba = false;
        bool abajo = true;
        bool neutro = true;

        int avanzar = 0; 

        Console.SetCursorPosition(55, 2);
        Console.WriteLine("Dibujar Gráfica");

        for (int i = 0; i < 250; i++)
        {
            if (avanzar < 6 && !arriba && abajo && neutro) 
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(x--, y);
                if (avanzar == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                if (i == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                Console.WriteLine("*");
                avanzar++;
                
                if (avanzar == 6)
                {
                    abajo = false;
                    neutro = false;
                    arriba = true;
                    avanzar = 0;
                }
                System.Threading.Thread.Sleep(40);
            }
            else if (avanzar < 15 && arriba && !abajo && !neutro) // Segmento vertical hacia arriba (color azul oscuro)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.SetCursorPosition(x, y--);
                Console.WriteLine("*");
                avanzar++;
                if (avanzar == 15)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    neutro = true;
                    avanzar = 0;
                }
                System.Threading.Thread.Sleep(40);
            }
            else if (avanzar < 6 && arriba && !abajo && neutro) // Segmento horizontal hacia la izquierda (color cian)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(x--, y);
                if (avanzar == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                Console.WriteLine("*");
                avanzar++;
                if (avanzar == 6)
                {
                    abajo = true;
                    neutro = false;
                    arriba = false;
                    avanzar = 0;
                }
                System.Threading.Thread.Sleep(40);
            }
            else if (avanzar < 15 && !arriba && abajo && !neutro) // Segmento vertical hacia abajo (color azul oscuro)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.SetCursorPosition(x, y++);
                Console.WriteLine("*");
                avanzar++;
                if (avanzar == 15)
                {
                    neutro = true;
                    avanzar = 0;
                }
                System.Threading.Thread.Sleep(40);
            }
        }

        Console.SetCursorPosition(45, 27);
        Console.WriteLine("Listo!!! Presiona espacio para continuar");
        Console.ReadKey();
        Console.Clear();
    }


    private static void DibujarEspiral()
    {
        int posX = 55;
        int posY = 13;
        Console.SetCursorPosition(posX, posY);
        int contador = 0;
        string direccion = "izquierda";
        int horizontal = 4;
        int vertical = 2;
        int colorActual = 1;

        for (int i = 1; i < 452; i++)
        {
            if (colorActual == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                colorActual = 2;
            }
            else if (colorActual == 2)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                colorActual = 3;
            }
            else if (colorActual == 3)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                colorActual = 4;
            }
            else if (colorActual == 4)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                colorActual = 5;
            }
            else if (colorActual == 5)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                colorActual = 1;
            }
            if (direccion == "izquierda" && contador < horizontal)
            {
                Console.WriteLine("*");
                posX--;
                Console.SetCursorPosition(posX, posY);
                contador++;
                System.Threading.Thread.Sleep(30);
                if (contador == horizontal)
                {
                    horizontal += 5;
                    direccion = "arriba";
                    contador = 0;
                }
            }
            else if (direccion == "arriba" && contador < vertical)
            {
                Console.WriteLine("*");
                posY--;
                Console.SetCursorPosition(posX, posY);
                contador++;
                System.Threading.Thread.Sleep(30);
                if (contador == vertical)
                {
                    vertical += 2;
                    direccion = "derecha";
                    contador = 0;
                }
            }
            else if (direccion == "derecha" && contador < horizontal)
            {
                Console.WriteLine("*");
                posX++;
                Console.SetCursorPosition(posX, posY);
                contador++;
                System.Threading.Thread.Sleep(30);
                if (contador == horizontal)
                {
                    horizontal += 5;
                    direccion = "abajo";
                    contador = 0;
                }
            }
            else if (direccion == "abajo" && contador < vertical)
            {
                Console.WriteLine("*");
                posY++;
                Console.SetCursorPosition(posX, posY);
                contador++;
                System.Threading.Thread.Sleep(30);
                if (contador == vertical)
                {
                    direccion = "izquierda";
                    vertical += 2;
                    contador = 0;
                }
            }
        }
        Console.SetCursorPosition(35, 27);
        Console.WriteLine("Presiona espacio para continuar");
        Console.ReadKey();
        Console.Clear();
    }

}