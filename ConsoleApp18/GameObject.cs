using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp18
{
    internal class GameObject
    {
        public string Namme { get; set; }
        public bool alive = true;
        public int placeY;
        public int placeX;

        public int sizeY;
        public int sizeX;

        public string Figure;


        public char[,] matrix;

        public GameObject(string namme, int placeY, int placeX, string figure)
        {
            this.placeY = placeY;
            this.placeX = placeX;
            Namme = namme;
            Figure = figure;
            FigureToMatrix();
        }

        void FigureToMatrix()
        {
            int countY = 0, countX = 0;
            foreach (var item in Figure)
            {
                if (item == '\n')
                {
                    countY++;
                } else
                {
                    countX++;
                }
            }
            sizeY = countY + 1;
            sizeX = (countX / (countY + 1));

            char[,] temp = new char[sizeY, sizeX];


            countY = 0;
            countX = 0;
            foreach (var item in Figure)
            {
                if (item == '\n')
                {
                    countY++;
                    countX = 0;
                }
                else
                {
                    temp[countY, countX++] = item;
                }
            }
            matrix = temp;
        }
    }
}
