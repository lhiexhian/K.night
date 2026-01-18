using System;
using K.night;
using static System.Console;

class Program
{
    public static void Main()
    {
        Class.Start();
        while (true)
        {
            Class.PrintMap();
            WriteLine("Use WASD to move, Q to quit:");
            ConsoleKeyInfo key;
            key = ReadKey(true);
            Class.MovePlayer(key);
        }
    }
}