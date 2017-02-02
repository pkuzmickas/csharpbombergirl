using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BomberGirl
{



    public partial class Form1 : Form
    {
        private const int SPRITE_SIZE = 42;
        private const int SPEED = 5;
        private Graphics gc;
        Player player1;
        Grid grid;
        int[,] Board;
        Image wall = Image.FromFile("Sprites/wall.png");
        Image grass = Image.FromFile("Sprites/grass.png");
        Image box = Image.FromFile("Sprites/box.png");

       



        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            grid = new Grid();
            gc = this.CreateGraphics();
            this.Show();
            Board = grid.getGrid();
            

            player1 = new Player(3);
            player1.posX = SPRITE_SIZE;
            player1.posY = SPRITE_SIZE * 3;

            draw(gc);
         
        }

        public void updatePlayer(Player player)
        {
           
            
           // Console.WriteLine(col + " " + row);
            if (player.moving_left)
            {
                int col = (int)((player.posX-SPEED) / SPRITE_SIZE);
                int row = (int)(player.posY / SPRITE_SIZE - 2);
                if (Board[col, row] == 0)
                {
                    player.posX -= SPEED;
                }


            }
            if(player.moving_right)
            {
                int col = (int)((player.posX + SPEED) / SPRITE_SIZE);
                int row = (int)(player.posY / SPRITE_SIZE - 2);
                if (Board[col+1, row] == 0)
                {
                    player.posX += SPEED;
                }
            }
            if (player.moving_up)
            {
                int col = (int)(player.posX  / SPRITE_SIZE);
                int row = (int)((player.posY - SPEED) / SPRITE_SIZE - 2);
                if (Board[col, row] == 0)
                {
                    player.posY -= SPEED;
                }
                
            }
            if (player.moving_down)
            {
                int col = (int)(player.posX / SPRITE_SIZE);
                int row = (int)((player.posY + SPEED) / SPRITE_SIZE - 2);
                if (Board[col, row+1] == 0)
                {
                    player.posY += SPEED;
                }
            }
            Invalidate();
        }

     

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
            draw(dc);
            updatePlayer(player1);
            base.OnPaint(e);
        }

        private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                player1.moving_left = true;
                
            }
            if (e.KeyCode == Keys.D)
            {
                player1.moving_right = true;
            }
            if (e.KeyCode == Keys.S)
            {
                player1.moving_down = true;
            }
            if (e.KeyCode == Keys.W)
            {
                player1.moving_up = true;
            }
            if (e.KeyCode == Keys.P)
            {
                float col = (float)(player1.posX / SPRITE_SIZE);
                float row = (float)(player1.posY / SPRITE_SIZE - 2);
                Console.WriteLine(col + " " + row);
            }

        }

        private void draw( Graphics gc)
        {



            for (int i = 0; i < grid.getGridWidth(); i++)
            {
                for (int j = 0; j < grid.getGridHeight(); j++)
                {
                    if (Board[i, j] == 1)
                    {

                        gc.DrawImage(wall, i * SPRITE_SIZE, (j + 2) * SPRITE_SIZE, SPRITE_SIZE + 2, SPRITE_SIZE + 2);

                    }
                    else if (Board[i, j] == 0)
                    {

                        gc.DrawImage(grass, i * SPRITE_SIZE, (j + 2) * SPRITE_SIZE, SPRITE_SIZE + 2, SPRITE_SIZE + 2);

                    }
                    else if (Board[i, j] == 2)
                    {

                        gc.DrawImage(box, i * SPRITE_SIZE, (j + 2) * SPRITE_SIZE, SPRITE_SIZE + 2, SPRITE_SIZE + 2);

                    }
                }
            }
            gc.DrawImage(player1.spriteSheet, new Rectangle((int)player1.posX, (int)player1.posY, SPRITE_SIZE, SPRITE_SIZE), new Rectangle(0, 16 * 2 - 1, 15, 15), GraphicsUnit.Pixel);

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                player1.moving_left = false;


            }
            if (e.KeyCode == Keys.D)
            {
                player1.moving_right = false;
            }
            if (e.KeyCode == Keys.S)
            {
                player1.moving_down = false;
            }
            if (e.KeyCode == Keys.W)
            {
                player1.moving_up = false;
            }
        }
    }
}
