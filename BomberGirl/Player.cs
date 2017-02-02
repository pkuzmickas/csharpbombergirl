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
        int ID;
        const int SPRITE_AMOUNT = 8;

        public Rectangle currentSprite { get; set; }
        public float posX;
        public float posY;
        public bool moving_left = false;
        public bool moving_right = false;
        public bool moving_up = false;
        public bool moving_down = false;


        public Player(int ID)
        {
            this.ID = ID;
            spriteSheet = Image.FromFile("Sprites/players.png");
            
        }
    }
}
