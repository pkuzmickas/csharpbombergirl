using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberGirl
{
    class Player
    {
        
        public Image spriteSheet {get;set;}
        int ID;
        const int SPRITE_AMOUNT = 8;

        public Rectangle currentSprite { get; set; }
        public float posX { get; set; }
        public float posY { get; set; }
        public Player(int ID)
        {
            this.ID = ID;
            spriteSheet = Image.FromFile("Sprites/players.png");
            
        }
    }
}
