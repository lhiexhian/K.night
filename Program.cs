using System;
using K.night;
using static System.Console;
using static K.night.Class;

class Program
{
    public static void Main()
    {
        CreateMap();
        while (true)
        {
            PrintMap();
            var key = ReadKey(true).Key;
            DetectCollision(key);
        }
    }
}