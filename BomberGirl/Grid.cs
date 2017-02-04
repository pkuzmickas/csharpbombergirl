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
    3 - BOMB
     
 */
    class Grid
    {
        private int[,] Board;
        
        Random r = new Random();
        int numOfBoxes = 0;
        public Grid()
        {
            Board = new int[Constants.BOARD_WIDTH, Constants.BOARD_HEIGHT];
            for (int i = 0; i < Constants.BOARD_WIDTH; i++)
            {
                for (int j = 0; j < Constants.BOARD_HEIGHT; j++)
                {
                    if ((i == 0 || i == Constants.BOARD_WIDTH - 1 || j == 0 || j == Constants.BOARD_HEIGHT - 1) || i > 1 && i < Constants.BOARD_WIDTH - 2 && j > 1 && j < Constants.BOARD_HEIGHT - 2 && i % 2 == 0 && j % 2 == 0) Board[i, j] = 1;
                    else Board[i, j] = 0;
                    if (Board[i, j] == 0 && ((i > 1 && j > 1 && i < Constants.BOARD_WIDTH - 2 && j < Constants.BOARD_HEIGHT - 2) || (j == 1 && i > 3 && i < Constants.BOARD_WIDTH - 4) || (i == 1 && j > 3 && j < Constants.BOARD_HEIGHT - 4) || (i == Constants.BOARD_WIDTH - 2 && j > 3 && j < Constants.BOARD_HEIGHT - 4) || (j == Constants.BOARD_HEIGHT-2 && i > 3 && i < Constants.BOARD_WIDTH - 4)))
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
                for (int i = 0; i < Constants.BOARD_WIDTH; i++)
                {
                    for (int j = 0; j < Constants.BOARD_HEIGHT; j++)
                    {
                        if (Board[i, j] == 0 && ((i > 1 && j > 1 && i < Constants.BOARD_WIDTH - 2 && j < Constants.BOARD_HEIGHT - 2) || (j == 1 && i > 3 && i < Constants.BOARD_WIDTH - 4) || (i == 1 && j > 3 && j < Constants.BOARD_HEIGHT - 4) || (i == Constants.BOARD_WIDTH - 2 && j > 3 && j < Constants.BOARD_HEIGHT - 4) || (j == Constants.BOARD_HEIGHT - 2 && i > 3 && i < Constants.BOARD_WIDTH - 4)))
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
            for (int i = 0; i < Constants.BOARD_HEIGHT; i++)
            {
                for (int j = 0; j < Constants.BOARD_WIDTH; j++)
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
            return Constants.BOARD_WIDTH;
        }
        public int getGridHeight()
        {
            return Constants.BOARD_HEIGHT;
        }

        
    }
}
