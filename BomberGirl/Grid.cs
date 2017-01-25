using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberGirl
{
    /*
    0 - GRASS
    1 - WALL
    2 - BOX
     
 */
    class Grid
    {
        private int[,] Board;
        private const int BOARD_WIDTH = 15;
        private const int BOARD_HEIGHT = 13;
        Random r = new Random();
        int numOfBoxes = 0;
        public Grid()
        {
            Board = new int[BOARD_WIDTH, BOARD_HEIGHT];
            for (int i = 0; i < BOARD_WIDTH; i++)
            {
                for (int j = 0; j < BOARD_HEIGHT; j++)
                {
                    if ((i == 0 || i == BOARD_WIDTH - 1 || j == 0 || j == BOARD_HEIGHT - 1) || i > 1 && i < BOARD_WIDTH - 2 && j > 1 && j < BOARD_HEIGHT - 2 && i % 2 == 0 && j % 2 == 0) Board[i, j] = 1;
                    else Board[i, j] = 0;
                    if (Board[i, j] == 0 && ((i > 1 && j > 1 && i < BOARD_WIDTH - 2 && j < BOARD_HEIGHT - 2) || (j == 1 && i > 3 && i < BOARD_WIDTH - 4) || (i == 1 && j > 3 && j < BOARD_HEIGHT - 4) || (i == BOARD_WIDTH - 2 && j > 3 && j < BOARD_HEIGHT - 4) || (j == BOARD_HEIGHT-2 && i > 3 && i < BOARD_WIDTH - 4)))
                    {
                        if (r.Next(2) == 0) {
                            Board[i, j] = 2;
                            numOfBoxes++;
                        }
                        //Board[i, j] = 2;

                    }
                }
            }
            while (numOfBoxes < 70)
            {
                for (int i = 0; i < BOARD_WIDTH; i++)
                {
                    for (int j = 0; j < BOARD_HEIGHT; j++)
                    {
                        if (Board[i, j] == 0 && ((i > 1 && j > 1 && i < BOARD_WIDTH - 2 && j < BOARD_HEIGHT - 2) || (j == 1 && i > 3 && i < BOARD_WIDTH - 4) || (i == 1 && j > 3 && j < BOARD_HEIGHT - 4) || (i == BOARD_WIDTH - 2 && j > 3 && j < BOARD_HEIGHT - 4) || (j == BOARD_HEIGHT - 2 && i > 3 && i < BOARD_WIDTH - 4)))
                        {
                            if (r.Next(2) == 0)
                            {
                                Board[i, j] = 2;
                                numOfBoxes++;
                            }
                            //Board[i, j] = 2;

                        }
                    }
                }
            }
            for (int i = 0; i < BOARD_HEIGHT; i++)
            {
                for (int j = 0; j < BOARD_WIDTH; j++)
                {
                    Console.Write(Board[j, i] + " ");

                }
                Console.WriteLine();
            }

        }

        public int[,] getGrid()
        {
            return Board;
        }

        public int getGridWidth()
        {
            return BOARD_WIDTH;
        }
        public int getGridHeight()
        {
            return BOARD_HEIGHT;
        }

        
    }
}
