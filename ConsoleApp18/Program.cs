using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ConsoleApp18
{
    internal class Program
    {
        static byte[] box = { 219, 219, 219, 010, 219, 219, 219 };
        static byte[] box1 = { 219 };
        static byte[] player = { 219, 195 };
        static byte[] missle = { 196 };

        static void Main(string[] args)
        {

            int Height = 20;
            int Width = 40;

            Encoding encoding = Encoding.GetEncoding(437);

            char[,] matrix = new char[Height, Width];

            GameObject Player = new GameObject("Player" ,2, 3, encoding.GetString(player));
            GameObject Box = new GameObject("Box" ,5, 8, encoding.GetString(box));
            GameObject Alien1 = new GameObject("Alien", 4, 30, encoding.GetString(box1));
            GameObject Alien2 = new GameObject("Alien", 5, 30, encoding.GetString(box1));
            GameObject Missle1 = new GameObject("Missle", 4, 6, encoding.GetString(missle));
            GameObject Missle2 = new GameObject("Missle", 5, 6, encoding.GetString(missle));
            List<GameObject> listGameObjects = new List<GameObject>();
            listGameObjects.Add(Player);
            listGameObjects.Add(Missle1);
            listGameObjects.Add(Missle2);
            listGameObjects.Add(Alien1);
            listGameObjects.Add(Alien2);

            var rand = new Random();

            var timer = new System.Timers.Timer(1000);
            timer.Elapsed += (x, e)=> listGameObjects.Add(new GameObject("Alien", rand.Next(Height), Width, encoding.GetString(box1)));
            timer.AutoReset = true;
            timer.Enabled = true;

            Thread render = new Thread(()=> {
                Console.CursorVisible = false;
                    do
                    {
                        Console.SetCursorPosition(0, 0);
                        foreach (var missle in listGameObjects)
                        {
                            if (missle.Namme == "Missle")
                            {
                                foreach (var boxMini in listGameObjects)
                                {
                                    if (boxMini.Namme == "Alien")
                                    {
                                        if (missle.placeY == boxMini.placeY && missle.placeX == boxMini.placeX)
                                        {
                                            boxMini.alive = false;
                                            missle.alive = false;
                                        }
                                    }
                                }
                                missle.placeX++;
                            }
                            if (missle.Namme == "Alien" && missle.placeX > 0) missle.placeX--;

                        }
                        List<GameObject> temp = listGameObjects;
                        for (int i = 0; i < temp.Count; i++)
                        {
                            if (temp[i].alive == false) listGameObjects.Remove(listGameObjects[i]);
                        }

                        CalcMatrix(matrix, Width, Height, listGameObjects);
                        RenderMatrix(matrix, Width, Height, ' ');
                        Thread.Sleep(100);
                        Console.WriteLine(Console.KeyAvailable);
                    } while (true);
                }
            );

            render.Start();

            Console.CursorVisible = false;
            ConsoleKeyInfo key = Console.ReadKey(true);

            ConsoleKeyInfo key2 = new ConsoleKeyInfo('P', ConsoleKey.P, false, false, false);
            do
            {
                if (key.Key.Equals(ConsoleKey.D))
                {
                    if (Player.placeX + 2 < Width)
                    {
                        Player.placeX++;
                    }                    
                } 
                else if (key.Key.Equals(ConsoleKey.A))
                {
                    if (Player.placeX != 0)
                    {
                        Player.placeX--;
                    }                    
                }
                else if (key.Key.Equals(ConsoleKey.W))
                {
                    if (Player.placeY !=0)
                    {
                        Player.placeY--;
                    }                    
                }
                else if (key.Key.Equals(ConsoleKey.S))
                {
                    if (Player.placeY + 1 < Height)
                    {
                        Player.placeY++;
                    }                    
                }
                else if (key.Key.Equals(ConsoleKey.Spacebar))
                {
                    listGameObjects.Add(new GameObject("Missle", Player.placeY, Player.placeX+2, "-"));
                }
                else if (key.Key.Equals(ConsoleKey.P))
                {
                    render.Suspend();
                }
                else if (key.Key.Equals(ConsoleKey.O))
                {
                    render.Resume();
                }
                else if (key.Key.Equals(ConsoleKey.Q))
                {
                    render.Abort();
                    Console.SetCursorPosition(5, 5);
                    Console.Write("   END GAME   ");
                    break;
                }
                key = Console.ReadKey(true);

                Thread.Sleep(100);

            } while (true);

            Console.ReadKey();
        }
        static void CalcMatrix(char[,] matrix, int matrixWidth, int matrixHeight, List<GameObject> gameObjects)
        {
            for (int matrixY = 0; matrixY < matrixHeight; matrixY++)
            {
                for (int matrixX = 0; matrixX < matrixWidth; matrixX++)
                {
                        matrix[matrixY, matrixX] = ' ';
                }
            }

            //foreach (var obj in gameObjects)
            for (int x = 0; x < gameObjects.Count; x++)
            {
                int a = 0;
                for (int matrixY = 0; matrixY < matrixHeight; matrixY++)
                {
                    int b = 0;
                    for (int matrixX = 0; matrixX < matrixWidth; matrixX++)
                    {
                        if (matrixY == gameObjects[x].placeY + a && matrixX == gameObjects[x].placeX + b && b < gameObjects[x].sizeX && a < gameObjects[x].sizeY)
                        {
                            matrix[matrixY, matrixX] = gameObjects[x].matrix[a, b++];
                        }                     
                    }
                    if (matrixY == gameObjects[x].placeY + a) a++;
                }
            }
        }

        static void RenderMatrix(char[,] matrix, int matrixWidth, int matrixHeight, char init)
        {
            string render = "";
            for (int matrixY = 0; matrixY < matrixHeight; matrixY++)
            {
                for (int matrixX = 0; matrixX < matrixWidth; matrixX++)
                {
                    if (matrix[matrixY, matrixX] != ' ')
                    {
                        render += matrix[matrixY, matrixX];
                    }
                    else
                    {
                        render += init;
                    }
                }
                render += '\n';
            }
            Console.Write(render);
        }

        static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            
        }

    }
}
