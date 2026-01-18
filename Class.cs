using System;
using static System.Console;
namespace K.night
{
    public static class Class
    {

        // create a widthxheight map starting
        // only 10x10 is visible at a time
        // the 0,0 coordinate is at the center of the map
        // so we need to offset the coordinates
        // by half the width and height
        // to get the correct coordinates
        // control the map
        // directions to move
        // updates the map accordingly each time the player moves
        public static int playerX = 0;
        public static int playerY = 0;
        public static int width = 1000;
        public static int height = 1000;
        public static string[,] map = new string[width, height];

        public static void Start()
        {
            Random rand = new Random();
            // Initial random fill
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    map[x, y] = (rand.Next(0, 100) < 70) ? " " : "+";
                }
            }

            for (int iter = 0; iter < 5; iter++)
            {
                string[,] newmap = new string[height, width];
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int landNeighbors = 0;
                        for (int ny = -1; ny <= 1; ny++)
                        {
                            for (int nx = -1; nx <= 1; nx++)
                            {
                                if (nx == 0 && ny == 0) continue;
                                int checkX = x + nx;
                                int checkY = y + ny;
                                if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                                {
                                    if (map[checkX, checkY] == "+") landNeighbors++;
                                }
                                else
                                {
                                    landNeighbors++; // Treat out-of-bounds as land
                                }
                            }
                        }
                        if (landNeighbors > 3)
                            newmap[x, y] = "+";
                        else if (landNeighbors < 4)
                            newmap[x, y] = " ";
                        else
                            newmap[x, y] = map[x, y];

                    }
                }
                map = newmap;
            }
            map[height/2, width/2] = "@"; // place player at center
        }
        
        public static void PrintMap()
        {
            SetCursorPosition(0, 0);
            int offsetX = width / 2;
            int offsetY = height / 2;
            for (int y = -5; y <= 20; y++)
            {
                for (int x = -5; x <= 50; x++)
                {
                    int mapX = playerX + x + offsetX;
                    int mapY = playerY + y + offsetY;
                    if (mapX >= 0 && mapX < width && mapY >= 0 && mapY < height)
                    {
                        Write(map[mapX, mapY]);
                    }
                    else
                    {
                        Write(" ");
                    }
                }
                WriteLine();
            }
        }

        public static void MovePlayer(ConsoleKeyInfo direction)
        {
            int newX = playerX;
            int newY = playerY;
            switch (direction.Key)
            {
                //directional buttons to move
                case ConsoleKey.UpArrow:
                    newY -= 1; // up
                    break;
                case ConsoleKey.DownArrow:
                    newY += 1; // down
                    break;
                case ConsoleKey.LeftArrow:
                    newX -= 1; // left
                    break;
                case ConsoleKey.RightArrow:
                    newX += 1; // right
                    break;
                default:
                    return; // invalid input
            }
            int offsetX = width / 2;
            int offsetY = height / 2;
            int mapX = newX + offsetX;
            int mapY = newY + offsetY;
            if (mapX >= 0 && mapX < width && mapY >= 0 && mapY < height && map[mapX, mapY] != "+")
            {
                // update player position
                map[playerX + offsetX, playerY + offsetY] = " ";
                playerX = newX;
                playerY = newY;
                map[playerX + offsetX, playerY + offsetY] = "@";
            }
        }

    }
}
