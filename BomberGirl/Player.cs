using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberGirl
{
    public class Player
    {
        
        public Image spriteSheet {get;set;}
        private int ID;
        private int Lives = 1;
        
        const int SPRITE_AMOUNT = 8;
        private int Speed = Constants.SPEED;
        public Rectangle currentSprite { get; set; }
        public int spriteNr=0;
        public float posX;
        public float posY;
        public bool moving_left = false;
        public bool moving_right = false;
        public bool moving_up = false;
        public bool moving_down = false;
        public bool justBombed = false;
        public bool takingDamage = false;
        public bool dead = false;
        public bool fliped = false;

        private int placedBombs = 0, maxBombs = 1, xplosionSize = 2;

        public int bombsPlaced
        {
            get { return placedBombs; }
            set { placedBombs = value; }
        }
        public int bombLimit
        {
            get { return maxBombs; }
            set { maxBombs = value; }
        }
        public int explosionSize
        {
            get { return xplosionSize; }
            set { xplosionSize = value; }
        }


        public Player(int ID)
        {
            this.ID = ID;
            spriteSheet = Image.FromFile("Sprites/players.png");
            
        }

        public int id
        {
            get { return ID;}
        }

        public int lives
        {
            get { return Lives; }
            set { Lives = value; }
        }

        public int speed
        {
            get { return Speed; }
            set { Speed = value; }
        }
    }
}
