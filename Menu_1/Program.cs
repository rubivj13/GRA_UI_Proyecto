using Menu_1;

internal class Program
{
    private static void Main(string[] args)
    {
        bool salir = false;
        while (!salir)
        {
            Console.Clear();
            Console.SetCursorPosition(50, 6);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Menú Principal");

            Console.SetCursorPosition(40, 9);
            Console.WriteLine("Selecciona una opción del menú\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(40, 10);
            Console.WriteLine("(1) Menú 1. Programas de introducción.");
            Console.SetCursorPosition(40, 11);
            Console.WriteLine("(2) Menú 2. Programas de localización.");
            Console.SetCursorPosition(40, 12);
            Console.WriteLine("(3) Salir");
            Console.SetCursorPosition(40, 13);
            Console.ForegroundColor = ConsoleColor.Blue;

            if (int.TryParse(Console.ReadLine(), out int opcion))
            {
                switch (opcion)
                {
                    case 1:
                        Console.Clear();
                        Menu1 menu1 = new Menu1(); 
                        menu1.MostrarMenu1(); 
                        break;
                    case 2:
                        Console.Clear();
                        Menu2 menu2 = new Menu2();
                        menu2.MostrarMenu2();
                        break;
                    case 3:
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
