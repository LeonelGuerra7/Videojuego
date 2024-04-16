using System;
using System.Diagnostics;
using System.Threading;

class Program
{
    static char[,] tablero = new char[10, 10];
    static Random random = new Random();
    static Stopwatch cronometro = new Stopwatch();

    static void Main(string[] args)
    {
        Console.Title = "Batalla Naval";
        Console.ForegroundColor = ConsoleColor.White;
        MostrarMensajeBienvenida();
        InicializarTablero();
        ColocarBarcos();
        Presentacion();
        Jugar();
    }

    static void MostrarMensajeBienvenida()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(@"
   - -        -    -------    -      |       |            -
  |    \     / \      |      / \     |       |           / \
  |    /    /   \     |     /   \    |       |          /   \
  |   |    /--N--\    A    /--B--\   |   A   |   L     /-----\
  |    \  /       \   |   /       \  |       |        /       \
  |    / /         \  |  /         \ |_____  |_____  /         \
   - -
     ");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.White;
    }

    static void InicializarTablero()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                tablero[i, j] = '~'; // '~' representa el agua en el tablero
            }
        }
    }

    static void ColocarBarcos()
    {
        ColocarBarco(3);
        ColocarBarco(3);
        ColocarBarco(2);
        ColocarBarco(2);
        ColocarTanque();
        ColocarTanque();
        ColocarAvion();
        ColocarAvion();
        ColocarAvion();
    }

    static void ColocarBarco(int longitud)
    {
        bool colocado = false;
        while (!colocado)
        {
            int fila = random.Next(10);
            int columna = random.Next(10);
            int direccion = random.Next(2); // 0: horizontal, 1: vertical

            if (direccion == 0 && columna + longitud <= 10)
            {
                bool libre = true;
                for (int i = columna; i < columna + longitud; i++)
                {
                    if (tablero[fila, i] != '~')
                    {
                        libre = false;
                        break;
                    }
                }

                if (libre)
                {
                    for (int i = columna; i < columna + longitud; i++)
                    {
                        tablero[fila, i] = 'B'; // 'B' representa un barco en el tablero
                    }
                    colocado = true;
                }
            }
            else if (direccion == 1 && fila + longitud <= 10)
            {
                bool libre = true;
                for (int i = fila; i < fila + longitud; i++)
                {
                    if (tablero[i, columna] != '~')
                    {
                        libre = false;
                        break;
                    }
                }

                if (libre)
                {
                    for (int i = fila; i < fila + longitud; i++)
                    {
                        tablero[i, columna] = 'B'; // 'B' representa un barco en el tablero
                    }
                    colocado = true;
                }
            }
        }
    }

    static void ColocarTanque()
    {
        bool colocado = false;
        while (!colocado)
        {
            int fila = random.Next(10);
            int columna = random.Next(10);

            if (tablero[fila, columna] == '~')
            {
                tablero[fila, columna] = 'T'; // 'T' representa un tanque en el tablero
                colocado = true;
            }
        }
    }

    static void ColocarAvion()
    {
        bool colocado = false;
        while (!colocado)
        {
            int fila = random.Next(10);
            int columna = random.Next(10);

            if (tablero[fila, columna] == '~')
            {
                tablero[fila, columna] = 'A'; // 'A' representa un avión en el tablero
                colocado = true;
            }
        }
    }

    static void Presentacion()
    {
        Console.WriteLine("¿Estás listo para hundir los barcos enemigos?");
        Console.WriteLine("Presiona Enter para comenzar...");
        Console.ReadLine();
        Console.Clear();
        cronometro.Start(); // Iniciar el cronómetro al comenzar el juego
    }

    static void Jugar()
    {
        int barcosRestantes = 7; // 7 barcos en total (3 barcos + 2 tanques + 2 aviones)
        int tanquesRestantes = 2; // 2 tanques en total
        int avionesRestantes = 3; // 3 aviones en total

        while (barcosRestantes > 0)
        {
            Console.Clear();
            MostrarTablero();
            Console.WriteLine();
            Console.WriteLine("Ingresa la fila (0-9):");
            int fila = LeerCoordenada();
            Console.WriteLine("Ingresa la columna (0-9):");
            int columna = LeerCoordenada();

            if (fila < 0 || fila > 9 || columna < 0 || columna > 9)
            {
                Console.WriteLine("Coordenadas inválidas. Inténtalo de nuevo.");
                Thread.Sleep(1500);
                continue;
            }

            if (tablero[fila, columna] == 'B' || tablero[fila, columna] == 'T' || tablero[fila, columna] == 'A')
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("¡Le has dado a un barco!");
                Console.ForegroundColor = ConsoleColor.White;
                char tipo = tablero[fila, columna];
                tablero[fila, columna] = 'X'; // 'X' representa un barco impactado en el tablero

                if (tipo == 'B')
                {
                    barcosRestantes--;
                }
                else if (tipo == 'T')
                {
                    tanquesRestantes--;
                }
                else if (tipo == 'A')
                {
                    avionesRestantes--;
                }

                if (barcosRestantes == 0 && tanquesRestantes == 0 && avionesRestantes == 0)
                {
                    Console.WriteLine("¡Has hundido todos los barcos enemigos!");
                    Thread.Sleep(2000);
                }
            }
            else if (tablero[fila, columna] == '~' || tablero[fila, columna] == 'X')
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("¡Agua!");
                Console.ForegroundColor = ConsoleColor.White;
                tablero[fila, columna] = '.'; // Punto indica ubicación ya seleccionada
            }

            Console.WriteLine("Presiona Enter para continuar...");
            Console.ReadLine();
        }

        cronometro.Stop(); // Detener el cronómetro al finalizar el juego

        Console.Clear();
        MostrarTablero();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("¡Felicidades, has ganado la batalla naval!");
        Console.WriteLine($"Tiempo total: {cronometro.Elapsed.Minutes} minutos {cronometro.Elapsed.Seconds} segundos");
        Console.ForegroundColor = ConsoleColor.White;
    }

    static void MostrarTablero()
    {
        Console.WriteLine("    0 1 2 3 4 5 6 7 8 9");
        for (int i = 0; i < 10; i++)
        {
            Console.Write($"{i}   ");
            for (int j = 0; j < 10; j++)
            {
                if (tablero[i, j] == 'B' || tablero[i, j] == 'T' || tablero[i, j] == 'A')
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("~ ");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (tablero[i, j] == 'X')
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("X ");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (tablero[i, j] == '.')
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(". ");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write($"{tablero[i, j]} ");
                }
            }
            Console.WriteLine();
        }
    }

    static int LeerCoordenada()
    {
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int coordenada))
            {
                return coordenada;
            }
            else
            {
                Console.WriteLine("Entrada inválida. Introduce un número entre 0 y 9.");
            }
        }
    }
}

