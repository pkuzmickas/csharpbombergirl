using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberGirl
{

    /*
     * Stores the information about each player
     */ 

    public class Player
    {
        //Variables (most of them self explanatory due to proper naming)

        //Stores the player spritesheet
        public Image spriteSheet {get;set;}
        //The ID of the player in the spritesheet (0-3)
        private int ID;
        private int Lives = 2;
        
        const int SPRITE_AMOUNT = 8;
        private float Speed = Constants.SPEED;
        //Sprite number in the row of the spritesheet
        public int spriteNr=0;
        public int lastSpriteNr = 0;
        public int bootsCollected=0;
        public float posX;
        public float posY;
        public bool moving_left = false;
        public bool moving_right = false;
        public bool moving_up = false;
        public bool moving_down = false;
        public bool justBombed = false;
        public bool takingDamage = false;
        public bool dead = false;

        private int placedBombs = 0, maxBombs = 1, xplosionSize = 2;

        //Returns or sets the amount of bombs placed by the player at the time
        public int bombsPlaced
        {
            get { return placedBombs; }
            set { placedBombs = value; }
        }
        //Returns or sets the limit of the bombs able to be placed by the player at the time
        public int bombLimit
        {
            get { return maxBombs; }
            set { maxBombs = value; }
        }
        //Returns or sets the explosion size in tiles
        public int explosionSize
        {
            get { return xplosionSize; }
            set { xplosionSize = value; }
        }

        //Player constructor requiring the ID of the player when creating
        public Player(int ID)
        {
            this.ID = ID;
            //Loads the spritesheet from the Sprites folder
            spriteSheet = Image.FromFile("Sprites/players.png");
            
        }
        //Returns the ID of the player
        public int id
        {
            get { return ID;}
        }
        //Returns the lives of the player
        public int lives
        {
            get { return Lives; }
            set { Lives = value; }
        }
        //Returns the speed of the player
        public float speed
        {
            get { return Speed; }
            set { Speed = value; }
        }
    }
}
