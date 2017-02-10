using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberGirl
{
    /*
     * Stores all of the main constants for the game
     */ 
    class Constants
    {
        // Size of each sprite (x and y)
        public const int SPRITE_SIZE = 42;
        // Initial speed of the player
        public const float SPEED = 1.5f;
        // Error for collisions so that the collision box would not be the same as the player sprites for better user experience
        public const int COLLISION_ERROR = 9;
        // The size of the form window
        public const int SCREEN_X = 629;
        public const int SCREEN_Y = 630;
        // The number of columns the grids have
        public const int BOARD_WIDTH = 15;
        // The number of rows the grids have
        public const int BOARD_HEIGHT = 13;
        // The time it takes for the bomb to explode
        public const int BOMB_TIMER_IN_SECONDS = 2;
        // The amount of different powerups
        public const int POWERUPS = 4;
        // The amount of pickups each powerup has
        public const int PICKUPS_EACH = 4;
        // The rate of the animation change in miliseconds
        public const int ANIM_SPEED = 200;
    }
}
