using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BomberGirl
{
 /*
     
     * FOR THE MAIN BOARD
    0 - GRASS
    1 - WALL
    2 - BOX
    3 - BOMB
     
 */

/*
     
     * FOR THE POWERUP BOARD
    0 - EMPTY
    1 - EXTRA FLAME PICKUP
    2 - EXTRA BOMB PICKUP
    3 - EXTRA SPEED PICKUP
    4 - EXTRA LIFE PICKUP
     
*/
    class Grid
    {
        private int[,] Board;
        private int[,] PowerUpBoard;
        Random r = new Random();
        int numOfBoxes = 0;
        public Grid()
        {
            Board = new int[Constants.BOARD_WIDTH, Constants.BOARD_HEIGHT];
            PowerUpBoard = new int[Constants.BOARD_WIDTH, Constants.BOARD_HEIGHT];



            int[] powerups = new int[Constants.PICKUPS_EACH * Constants.POWERUPS];
            int n = 0;
            while (n != Constants.PICKUPS_EACH * Constants.POWERUPS)
            {
                int t = r.Next(0, 70);
                bool foundCopy = false;
                for (int i = 0; i < n; i++)
                {
                    if (powerups[i] == t)
                    {
                        foundCopy = true;
                        break;
                    }
                }
                if (!foundCopy)
                {
                    powerups[n] = t;
                    n++;
                }
            }
            int currentPowerup = 1;


            for (int i = 0; i < Constants.BOARD_WIDTH; i++)
            {
                for (int j = 0; j < Constants.BOARD_HEIGHT; j++)
                {
                    PowerUpBoard[i, j] = 0;
                    if ((i == 0 || i == Constants.BOARD_WIDTH - 1 || j == 0 || j == Constants.BOARD_HEIGHT - 1) || i > 1 && i < Constants.BOARD_WIDTH - 2 && j > 1 && j < Constants.BOARD_HEIGHT - 2 && i % 2 == 0 && j % 2 == 0) Board[i, j] = 1;
                    else Board[i, j] = 0;
                    if (Board[i, j] == 0 && ((i > 1 && j > 1 && i < Constants.BOARD_WIDTH - 2 && j < Constants.BOARD_HEIGHT - 2) || (j == 1 && i > 3 && i < Constants.BOARD_WIDTH - 4) || (i == 1 && j > 3 && j < Constants.BOARD_HEIGHT - 4) || (i == Constants.BOARD_WIDTH - 2 && j > 3 && j < Constants.BOARD_HEIGHT - 4) || (j == Constants.BOARD_HEIGHT-2 && i > 3 && i < Constants.BOARD_WIDTH - 4)))
                    {
                        if (r.Next(2) == 0) {
                            Board[i, j] = 2;
                            bool found = false;
                            for (int k = 0; k < n; k++)
                            {
                                if (numOfBoxes == powerups[k])
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (found)
                            {
                                PowerUpBoard[i, j] = currentPowerup;
                                currentPowerup++;
                                if (currentPowerup > Constants.POWERUPS)
                                {
                                    currentPowerup = 1;
                                }
                            }
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
                                bool found = false;
                                for (int k = 0; k < n; k++)
                                {
                                    if (numOfBoxes == powerups[k])
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                                if (found)
                                {
                                    PowerUpBoard[i, j] = currentPowerup;
                                    currentPowerup++;
                                    if (currentPowerup > Constants.POWERUPS)
                                    {
                                        currentPowerup = 1;
                                    }
                                }
                                numOfBoxes++;
                            }
                            //Board[i, j] = 2;

                        }
                    }
                }
            }

        }

        public int[,] getGrid()
        {
            return Board;
        }

        public int[,] getPowerupGrid()
        {
            return PowerUpBoard;
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
