using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MenuLocalizacionVM
{
    internal class ahorcado
    {
        private string palabraSecreta;
        private char[] palabraAdivinada;
        private List<char> letrasIntentadas;
        private int intentosFallidos;
        private const int MAX_INTENTOS = 6;
        private int anchoConsola;
        private int altoConsola;
        private bool interfazDibujada = false;

        public void Jugar()
        {
            bool jugarDeNuevo = true;

            // Establecer tamaño de la consola para mejor visualización
            Console.WindowHeight = 35;
            Console.WindowWidth = 100;
            anchoConsola = Console.WindowWidth;
            altoConsola = Console.WindowHeight;

            while (jugarDeNuevo)
            {
                Inicializar();
                bool juegoTerminado = false;
                interfazDibujada = false;

                while (!juegoTerminado)
                {
                    if (!interfazDibujada)
                    {
                        Console.Clear();
                        DibujarInterfazCompleta();
                        interfazDibujada = true;
                    }
                    else
                    {
                        // Solo actualiza las partes necesarias sin redibujar todo
                        DibujarAhorcado();
                        MostrarPalabraOculta();
                        MostrarLetrasIntentadas();
                        MostrarEstadoJuego();
                    }

                    if (intentosFallidos >= MAX_INTENTOS)
                    {
                        MostrarMensajeFinal(false);
                        juegoTerminado = true;
                    }
                    else if (!palabraAdivinada.Contains('_'))
                    {
                        MostrarMensajeFinal(true);
                        juegoTerminado = true;
                    }
                    else
                    {
                        ProcesarEntrada();
                    }
                }

                jugarDeNuevo = MostrarMenuFinal();
            }
        }

        private void DibujarInterfazCompleta()
        {
            // Dibujar todos los elementos una sola vez
            DibujarMarco();
            DibujarTituloOptimizado();
            DibujarAhorcado();
            MostrarPalabraOculta();
            MostrarLetrasIntentadas();
            MostrarEstadoJuego();
        }

        private void Inicializar()
        {
            Console.Clear();
            DibujarMarco();

            // Animación de título
            DibujarTituloOptimizado();
            Thread.Sleep(300);

            // Caja para la entrada de la palabra con más espacio
            DibujarCaja(anchoConsola / 2 - 30, 12, 60, 6);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(anchoConsola / 2 - 25, 14);
            Console.WriteLine("Introduce la palabra secreta:");

            // Ocultar la palabra mientras se escribe con mejor posicionamiento
            Console.SetCursorPosition(anchoConsola / 2 - 25, 16);
            Console.Write("→ ");

            string palabra = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace && char.IsLetter(key.KeyChar))
                {
                    palabra += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && palabra.Length > 0)
                {
                    palabra = palabra.Substring(0, palabra.Length - 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter || palabra.Length == 0);

            // Animación de carga
            Console.SetCursorPosition(anchoConsola / 2 - 10, 18);
            Console.Write("Preparando juego ");

            for (int i = 0; i < 10; i++)
            {
                Console.Write(".");
                Thread.Sleep(100);
            }

            palabraSecreta = palabra.ToUpper();
            palabraAdivinada = new char[palabraSecreta.Length];

            for (int i = 0; i < palabraAdivinada.Length; i++)
            {
                palabraAdivinada[i] = '_';
            }

            letrasIntentadas = new List<char>();
            intentosFallidos = 0;
        }

        private void DibujarMarco()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;

            // Líneas horizontales con animación
            for (int i = 0; i < anchoConsola; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("═");
                Console.SetCursorPosition(i, altoConsola - 1);
                Console.Write("═");

                if (i % 5 == 0) Thread.Sleep(1); // Pequeña pausa para efecto visual
            }

            // Líneas verticales con animación
            for (int i = 1; i < altoConsola - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("║");
                Console.SetCursorPosition(anchoConsola - 1, i);
                Console.Write("║");

                if (i % 3 == 0) Thread.Sleep(1); // Pequeña pausa para efecto visual
            }

            // Esquinas
            Console.SetCursorPosition(0, 0);
            Console.Write("╔");
            Console.SetCursorPosition(anchoConsola - 1, 0);
            Console.Write("╗");
            Console.SetCursorPosition(0, altoConsola - 1);
            Console.Write("╚");
            Console.SetCursorPosition(anchoConsola - 1, altoConsola - 1);
            Console.Write("╝");
        }

        private void DibujarCaja(int x, int y, int ancho, int alto)
        {
            Console.ForegroundColor = ConsoleColor.Blue;

            // Líneas horizontales
            for (int i = x; i < x + ancho; i++)
            {
                Console.SetCursorPosition(i, y);
                Console.Write("─");
                Console.SetCursorPosition(i, y + alto);
                Console.Write("─");
            }

            // Líneas verticales
            for (int i = y + 1; i < y + alto; i++)
            {
                Console.SetCursorPosition(x, i);
                Console.Write("│");
                Console.SetCursorPosition(x + ancho - 1, i);
                Console.Write("│");
            }

            // Esquinas
            Console.SetCursorPosition(x, y);
            Console.Write("┌");
            Console.SetCursorPosition(x + ancho - 1, y);
            Console.Write("┐");
            Console.SetCursorPosition(x, y + alto);
            Console.Write("└");
            Console.SetCursorPosition(x + ancho - 1, y + alto);
            Console.Write("┘");
        }

        private void DibujarTituloOptimizado()
        {
            // Título más pequeño para evitar solapamientos
            Console.ForegroundColor = ConsoleColor.Magenta;

            string[] titulo = {
                "  █████  ██   ██  ██████  ██████   ██████  █████  ██████   ██████  ",
                " ██   ██ ██   ██ ██    ██ ██   ██ ██      ██   ██ ██   ██ ██    ██ ",
                " ███████ ███████ ██    ██ ██████  ██      ███████ ██   ██ ██    ██ ",
                " ██   ██ ██   ██ ████████ ██   ██ ███████ ██   ██ ███████ ████████ "
            };

            // Dibujar directamente sin animación para optimizar
            for (int i = 0; i < titulo.Length; i++)
            {
                CentrarTexto(titulo[i], i + 2);
            }
        }

        private void DibujarAhorcado()
        {
            int x = 20;
            int y = 14;

            // Limpiar el área antes de redibujar
            for (int i = y - 1; i < y + 12; i++)
            {
                Console.SetCursorPosition(x - 4, i);
                Console.Write(new string(' ', 28));
            }

            // Marco del área del ahorcado con más espacio
            DibujarCaja(x - 5, y - 2, 30, 14);

            Console.ForegroundColor = ConsoleColor.Yellow;

            // Base
            for (int i = 0; i < 20; i++)
            {
                Console.SetCursorPosition(x + i, y + 10);
                Console.Write("=");
            }

            // Poste vertical
            for (int i = 0; i <= 9; i++)
            {
                Console.SetCursorPosition(x + 5, y + i);
                Console.WriteLine("║");
            }

            // Poste horizontal
            for (int i = 0; i < 12; i++)
            {
                Console.SetCursorPosition(x + 5 + i, y);
                Console.Write("═");
            }

            // Cuerda
            Console.SetCursorPosition(x + 17, y);
            Console.WriteLine("╥");
            Console.SetCursorPosition(x + 17, y + 1);
            Console.WriteLine("║");

            // Dibujar al ahorcado según los intentos fallidos
            Console.ForegroundColor = ConsoleColor.Red;

            // Cabeza
            if (intentosFallidos >= 1)
            {
                Console.SetCursorPosition(x + 17, y + 2);
                Console.WriteLine("O");
            }

            // Torso
            if (intentosFallidos >= 2)
            {
                Console.SetCursorPosition(x + 17, y + 3);
                Console.WriteLine("║");
                Console.SetCursorPosition(x + 17, y + 4);
                Console.WriteLine("║");
            }

            // Brazo izquierdo
            if (intentosFallidos >= 3)
            {
                Console.SetCursorPosition(x + 16, y + 3);
                Console.WriteLine("/");
            }

            // Brazo derecho
            if (intentosFallidos >= 4)
            {
                Console.SetCursorPosition(x + 18, y + 3);
                Console.WriteLine("\\");
            }

            // Pierna izquierda
            if (intentosFallidos >= 5)
            {
                Console.SetCursorPosition(x + 16, y + 5);
                Console.WriteLine("/");
            }

            // Pierna derecha
            if (intentosFallidos >= 6)
            {
                Console.SetCursorPosition(x + 18, y + 5);
                Console.WriteLine("\\");
            }
        }

        private void MostrarPalabraOculta()
        {
            int x = anchoConsola / 2 + 15;
            int y = 15;

            // Limpiar el área antes de actualizar
            for (int i = y - 1; i < y + 4; i++)
            {
                Console.SetCursorPosition(x - 9, i);
                Console.Write(new string(' ', 38));
            }

            // Marco para la palabra con más espacio
            DibujarCaja(x - 10, y - 2, 40, 5);

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(x, y);
            Console.Write("Palabra: ");

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < palabraAdivinada.Length; i++)
            {
                sb.Append(palabraAdivinada[i] + " ");
            }

            // Colorear según el estado del juego
            Console.ForegroundColor = palabraAdivinada.Contains('_') ? ConsoleColor.Yellow : ConsoleColor.Green;
            Console.Write(sb.ToString());
        }

        private void MostrarLetrasIntentadas()
        {
            int x = anchoConsola / 2 + 15;
            int y = 21;

            // Limpiar el área antes de actualizar
            for (int i = y - 1; i < y + 4; i++)
            {
                Console.SetCursorPosition(x - 9, i);
                Console.Write(new string(' ', 38));
            }

            // Marco para las letras intentadas con más espacio
            DibujarCaja(x - 10, y - 2, 40, 5);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(x, y);
            Console.Write("Letras usadas: ");

            // Mostrar letras sin animación para optimizar
            int posX = x + 14;
            foreach (char letra in letrasIntentadas)
            {
                // Colorear según si la letra está en la palabra o no
                if (palabraSecreta.Contains(letra))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.SetCursorPosition(posX, y);
                Console.Write(letra);
                posX += 2;
            }
        }

        private void MostrarEstadoJuego()
        {
            int x = anchoConsola / 2;
            int y = 26;

            // Limpiar el área antes de actualizar
            for (int i = y - 1; i < y + 3; i++)
            {
                Console.SetCursorPosition(x - 34, i);
                Console.Write(new string(' ', 68));
            }

            // Marco para el estado del juego con más espacio
            DibujarCaja(x - 35, y - 1, 70, 3);

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(x - 30, y);
            Console.Write($"Intentos fallidos: {intentosFallidos}/{MAX_INTENTOS}");

            string estadoMensaje = "";
            if (intentosFallidos == 0)
            {
                estadoMensaje = "¡Perfecto! Sigue así.";
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (intentosFallidos < 3)
            {
                estadoMensaje = "¡Vas bien! Piensa con cuidado.";
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (intentosFallidos < 5)
            {
                estadoMensaje = "¡Cuidado! Quedan pocos intentos.";
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }
            else
            {
                estadoMensaje = "¡Última oportunidad! Piensa bien.";
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.SetCursorPosition(x + 5, y);
            Console.Write(estadoMensaje);
        }

        private void ProcesarEntrada()
        {
            Console.ForegroundColor = ConsoleColor.White;
            int x = anchoConsola / 2;
            int y = 29;

            // Limpiar el área de entrada antes de actualizar
            for (int i = y - 1; i < y + 3; i++)
            {
                Console.SetCursorPosition(x - 34, i);
                Console.Write(new string(' ', 68));
            }

            // Marco para la entrada con más espacio
            DibujarCaja(x - 35, y - 1, 70, 3);

            Console.SetCursorPosition(x - 15, y);
            Console.Write("Ingresa una letra: ");

            char letra = ObtenerLetra();

            if (!letrasIntentadas.Contains(letra))
            {
                letrasIntentadas.Add(letra);

                if (palabraSecreta.Contains(letra))
                {
                    for (int i = 0; i < palabraSecreta.Length; i++)
                    {
                        if (palabraSecreta[i] == letra)
                        {
                            palabraAdivinada[i] = letra;
                        }
                    }
                    MostrarMensajeFlotante("¡Correcto!", ConsoleColor.Green);
                }
                else
                {
                    intentosFallidos++;
                    MostrarMensajeFlotante("¡Incorrecto!", ConsoleColor.Red);
                }
            }
            else
            {
                MostrarMensajeFlotante("Ya has intentado esa letra", ConsoleColor.Yellow);
            }
        }

        private void MostrarMensajeFlotante(string mensaje, ConsoleColor color)
        {
            int x = anchoConsola / 2 - (mensaje.Length / 2);
            int y = 31;

            // Limpiar el área del mensaje anterior
            Console.SetCursorPosition(x - 5, y);
            Console.Write(new string(' ', mensaje.Length + 10));

            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(mensaje);

            Thread.Sleep(500); // Reducido el tiempo de espera

            // Borrar el mensaje
            Console.SetCursorPosition(x, y);
            Console.Write(new string(' ', mensaje.Length));
        }

        private void MostrarMensajeFinal(bool victoria)
        {
            int x = anchoConsola / 2;
            int y = 31;

            if (victoria)
            {
                Console.ForegroundColor = ConsoleColor.Green;

                // Mostrar mensaje de victoria sin demasiadas animaciones
                Console.Clear();
                DibujarMarco();
                DibujarTituloOptimizado();
                DibujarAhorcado();
                MostrarPalabraOculta();
                MostrarLetrasIntentadas();

                Console.ForegroundColor = ConsoleColor.Green;

                // Mensaje de victoria
                string[] celebracion = {
                    "\\o/ ¡FELICIDADES! \\o/",
                    "¡HAS SALVADO AL PERSONAJE!",
                    $"La palabra era: {palabraSecreta}"
                };

                for (int j = 0; j < celebracion.Length; j++)
                {
                    CentrarTexto(celebracion[j], y + j);
                }

                Thread.Sleep(1000); // Reducido tiempo de espera
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                string mensaje = "¡OH NO! ¡HAS PERDIDO!";
                CentrarTexto(mensaje, y);

                Console.ForegroundColor = ConsoleColor.Yellow;
                string revelacion = $"La palabra era: {palabraSecreta}";
                CentrarTexto(revelacion, y + 1);
                Thread.Sleep(800); // Reducido tiempo de espera
            }
        }

        private char ObtenerLetra()
        {
            char letra;
            ConsoleKeyInfo key = Console.ReadKey(true);

            while (!char.IsLetter(key.KeyChar))
            {
                MostrarMensajeFlotante("Por favor ingresa una letra válida", ConsoleColor.Yellow);
                key = Console.ReadKey(true);
            }

            letra = char.ToUpper(key.KeyChar);
            Console.Write(letra);

            return letra;
        }

        private bool MostrarMenuFinal()
        {
            bool opcionValida = false;
            int opcion = 0;
            int x = anchoConsola / 2;
            int y = 24;

            Console.Clear();
            DibujarMarco();
            DibujarTituloOptimizado();

            // Marco para el menú final más espacioso
            DibujarCaja(x - 30, y - 7, 60, 13);

            Console.ForegroundColor = ConsoleColor.Yellow;
            CentrarTexto("JUEGO TERMINADO", y - 5);

            // Mostrar opciones sin animación para que aparezcan inmediatamente
            Console.ForegroundColor = ConsoleColor.Cyan;
            CentrarTexto("¿Qué deseas hacer?", y - 3);

            string[] opciones = {
                "1. Continuar jugando",
                "2. Menú anterior",
                "3. Salir"
            };

            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < opciones.Length; i++)
            {
                CentrarTexto(opciones[i], y - 1 + i);
            }

            while (!opcionValida)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(x - 3, y + 3);
                Console.Write("Opción:   "); // Espacio para borrar entrada anterior
                Console.SetCursorPosition(x + 5, y + 3);

                if (int.TryParse(Console.ReadLine(), out opcion) && opcion >= 1 && opcion <= 3)
                {
                    opcionValida = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    CentrarTexto("Opción no válida. Intenta de nuevo.", y + 4);
                    Thread.Sleep(800);
                    Console.SetCursorPosition(x - 15, y + 4);
                    Console.Write(new string(' ', 40));
                }
            }

            switch (opcion)
            {
                case 1: // Continuar jugando
                    return true;
                case 2: // Menú anterior
                    return false;
                case 3: // Salir
                    Environment.Exit(0);
                    return false;
                default:
                    return false;
            }
        }

        private void CentrarTexto(string texto, int y, bool saltoLinea = true)
        {
            Console.SetCursorPosition((anchoConsola - texto.Length) / 2, y);
            if (saltoLinea)
                Console.WriteLine(texto);
            else
                Console.Write(texto);
        }
    }
}