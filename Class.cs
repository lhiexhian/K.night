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
        public static int type;
        public static int width = 200;
        public static int height = 200;
        public static int playerX = width / 2;
        public static int playerY = height / 2;
        public static int viewWidth = 20;
        public static int viewHeight = 5;
        public static int[,] map = new int[height, width];
        public static int offsetX = viewWidth / 2;
        public static int offsetY = viewHeight / 2;
        public static string tile;

        public static void CreateMap()
        {
            Random rand = new Random();
            // Initial random fill (map is [row=y, col=x] => map[y,x])
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    map[y, x] = (rand.Next(0, 100) < 60) ? 0 : 1;
                }
            }
            GenerateLandscape();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                    {
                        map[y, x] = 99; // make borders walls
                    }
                    if (x == 102 && y == 100)
                    {
                        map[y, x] = 3;
                    }
                }
            }

            // place player after map is created
            ShowPlayer();
        }

        public static string[] TileSets(int value)
        {
            switch (value)
            {
                case 0:
                    return new[]
                    {
                        "  ,    ",
                        "   .   ",
                        "       ",
                        ".,     ",
                        "    .. ",
                        "       ",
                        " , ,   "
                    };
                case 1:
                    return new[]
                    {
                        "   A   ",
                        "  / \\  ",
                        " /___\\ ",
                        " /   \\ ",
                        "/_____\\",
                        "  |H|  ",
                        "  1.L  "
                    };
                case 2:
                    return new[]
                    {

                        "       ",
                        "       ",
                        "  ///  ",
                        "( O-O )",
                        " [>-<] ",
                        "  [ ]  ",
                        "  /|\\  "
                    };
                case 3:
                    return new[]
                    {
                        " _____ ",
                        "//////\\",
                        "|     |",
                        "| [+] |",
                        "|  _  |",
                        "| /.\\ |",
                        "| | | |"
                    };
                case 99:
                    return new[]
                    {
                        "|][|][|",
                        "|[|][]|",
                        "|][|][|",
                        "|[|][]|",
                        "|][|][|",
                        "|[|][]|",
                        "|][|][|",
                    };
                default:
                    return new[]
                    {
                        "       ",
                        "       ",
                        "       ",
                        "       ",
                        "       ",
                        "       ",
                        "       "
                    };
            }
        }

        public static void GenerateLandscape()
        {
            for (int iter = 0; iter < 4; iter++)
            {
                int[,] newMap = new int[height, width];
                // count neighbors
                for (int y = 1; y < height - 1; y++)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        int neighbor = 0;
                        for (int ny = -1; ny <= 1; ny++)
                        {
                            for (int nx = -1; nx <= 1; nx++)
                            {
                                if (ny == 0 && nx == 0)
                                    continue;
                                int checkY = y + ny;
                                int checkX = x + nx;
                                if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                                {
                                    // map is [row=y, col=x]
                                    if (map[checkY, checkX] == 1) neighbor++;
                                }
                                else
                                {
                                    neighbor++; // Treat out-of-bounds as land
                                }
                            }
                        }

                        if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                        {
                            newMap[y, x] = 99; // make borders walls
                        }
                        if (neighbor > 4)
                            newMap[y, x] = 1;
                        else if (neighbor < 4)
                            newMap[y, x] = 0;
                        else
                            newMap[y, x] = map[y, x];

                    }
                }
                map = newMap;
            }
        }

        public static void ShowPlayer()
        {
            if (playerY >= 0 && playerY < height && playerX >= 0 && playerX < width)
                map[playerY, playerX] = 2;
        }

        public static void PrintMap()
        {
            SetCursorPosition(0, 0);
            int magnification = 7;
            for (int y = -offsetY; y <= offsetY; y++)
            {
                for (int row = 0; row < magnification; row++)
                {
                    for (int x = -offsetX; x <= offsetX; x++)
                    {
                        int mapX = playerX + x; // column
                        int mapY = playerY + y; // row
                        if (mapX >= 0 && mapX < width && mapY >= 0 && mapY < height)
                        {
                            var tileLines = TileSets(map[mapY, mapX]);

                            Write(tileLines[row]);
                        }
                        else
                        {
                            Write(new string(' ', magnification));
                        }
                    }
                    WriteLine();
                }
            }
            WriteLine($"Player Position: X={playerX}, Y={playerY}   ");
        }

        public static void DetectCollision(ConsoleKey key)
        {
            int newX = playerX;
            int newY = playerY;
            switch (key)
            {
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

            // check bounds and collision using [row, col] => map[newY, newX]
            if (newX >= 0 && newX < width && newY >= 0 && newY < height && map[newY, newX] == 0)
            {
                // update player position
                if (playerY >= 0 && playerY < height && playerX >= 0 && playerX < width)
                    map[playerY, playerX] = 0;
                playerX = newX;
                playerY = newY;
                ShowPlayer();
            }
        }
    }
}
