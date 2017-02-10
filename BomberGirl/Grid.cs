using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Class to handle the Tile Grid  for interacting and drawing sprites
 */ 

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
        //Variable declaration
        //Board of the game for drawing GRASS WALLS BOX BOMB
        private int[,] Board;
        //Board of the game for drawing powerups
        private int[,] PowerUpBoard;
        
        Random r = new Random();
        //Number of boxes placed
        int numOfBoxes = 0;

        public Grid()
        {
            Board = new int[Constants.BOARD_WIDTH, Constants.BOARD_HEIGHT];
            PowerUpBoard = new int[Constants.BOARD_WIDTH, Constants.BOARD_HEIGHT];
            int[] powerups = new int[Constants.PICKUPS_EACH * Constants.POWERUPS];
            
            //Number of pickups placed
            int n = 0;

            //Generates a random number from 0 to 69 (There are 70 boxes) and places pickups in random locations under the boxes
            while (n != Constants.PICKUPS_EACH * Constants.POWERUPS)
            {
                int t = r.Next(0, 70);
                //Boolean makes sure they are distinct
                bool foundCopy = false;
                for (int i = 0; i < n; i++)
                {
                    if (powerups[i] == t)
                    {
                        foundCopy = true;
                        break;
                    }
                }
                //If it's distinct create a new powerup
                if (!foundCopy)
                {
                    powerups[n] = t;
                    n++;
                }
            }
            int currentPowerup = 1;

            // Initializes the game board's elements, setting 0 to where the grass should be, 1 to where the walls should be, 2 to where the boxes should be
            for (int i = 0; i < Constants.BOARD_WIDTH; i++)
            {
                for (int j = 0; j < Constants.BOARD_HEIGHT; j++)
                {
                    PowerUpBoard[i, j] = 0;
                    if ((i == 0 || i == Constants.BOARD_WIDTH - 1 || j == 0 || j == Constants.BOARD_HEIGHT - 1) || i > 1 && i < Constants.BOARD_WIDTH - 2 && j > 1 && j < Constants.BOARD_HEIGHT - 2 && i % 2 == 0 && j % 2 == 0) Board[i, j] = 1;
                    else Board[i, j] = 0;
                    // Checks whether the row and column should be a grass image etc.
                    if (Board[i, j] == 0 && ((i > 1 && j > 1 && i < Constants.BOARD_WIDTH - 2 && j < Constants.BOARD_HEIGHT - 2) || (j == 1 && i > 3 && i < Constants.BOARD_WIDTH - 4) || (i == 1 && j > 3 && j < Constants.BOARD_HEIGHT - 4) || (i == Constants.BOARD_WIDTH - 2 && j > 3 && j < Constants.BOARD_HEIGHT - 4) || (j == Constants.BOARD_HEIGHT-2 && i > 3 && i < Constants.BOARD_WIDTH - 4)))
                    {
                        // Generates a random number of 0 or 1 and if it is 0 places a box there (so that the boxes would be randomly generated)
                        if (r.Next(2) == 0) {
                            Board[i, j] = 2;
                            bool found = false;
                            // Puts the pickups in random order behind some of the boxes.
                            // Searches if that place already has a pickup
                            for (int k = 0; k < n; k++)
                            {
                                if (numOfBoxes == powerups[k])
                                {
                                    found = true;
                                    break;
                                }
                            }
                            // If a place does not have a pickup places a pickup there
                            if (found)
                            {
                                PowerUpBoard[i, j] = currentPowerup;
                                // Changes the type of the pickup so it would always generate a new one (First bomb then flame then boot etc.)
                                currentPowerup++;
                                if (currentPowerup > Constants.POWERUPS)
                                {
                                    currentPowerup = 1;
                                }
                            }
                            numOfBoxes++;
                        }

                    }
                }
            }
            
            
            // If the number of boxes is under 70 it places the remaining boxes so that the number of boxes is 70
            while (numOfBoxes < 70)
            {
                for (int i = 0; i < Constants.BOARD_WIDTH; i++)
                {
                    for (int j = 0; j < Constants.BOARD_HEIGHT; j++)
                    {
                        if (Board[i, j] == 0 && ((i > 1 && j > 1 && i < Constants.BOARD_WIDTH - 2 && j < Constants.BOARD_HEIGHT - 2) || (j == 1 && i > 3 && i < Constants.BOARD_WIDTH - 4) || (i == 1 && j > 3 && j < Constants.BOARD_HEIGHT - 4) || (i == Constants.BOARD_WIDTH - 2 && j > 3 && j < Constants.BOARD_HEIGHT - 4) || (j == Constants.BOARD_HEIGHT - 2 && i > 3 && i < Constants.BOARD_WIDTH - 4)))
                        {
                            // Checks whether the row and column should be a grass image etc.
                            if (r.Next(2) == 0)
                            {
                                Board[i, j] = 2;
                                bool found = false;
                                // Puts the pickups in random order behind some of the boxes.
                                // Searches if that place already has a pickup
                                for (int k = 0; k < n; k++)
                                {
                                    if (numOfBoxes == powerups[k])
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                                // If a place does not have a pickup places a pickup there
                                if (found)
                                {
                                    PowerUpBoard[i, j] = currentPowerup;
                                    // Changes the type of the pickup so it would always generate a new one (First bomb then flame then boot etc.)
                                    currentPowerup++;
                                    if (currentPowerup > Constants.POWERUPS)
                                    {
                                        currentPowerup = 1;
                                    }
                                }
                                numOfBoxes++;
                            }
                        }
                    }
                }
            }

        }
        // Returns the main game board grid
        public int[,] getGrid()
        {
            return Board;
        }
        // Returns the pickup grid
        public int[,] getPowerupGrid()
        {
            return PowerUpBoard;
        }


        
    }
}
