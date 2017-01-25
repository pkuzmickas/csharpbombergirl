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

            Thread gameThread = new Thread(run);
            gameThread.Start();
         
        }

        public void run()
        {
            while (true)
            {
                Application.DoEvents();
                //Invalidate();
                //draw();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
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
            dc.DrawImage(player1.spriteSheet, new Rectangle((int)player1.posX, (int)player1.posY, SPRITE_SIZE, SPRITE_SIZE), new Rectangle(0, 16 * 2 - 1, 15, 15), GraphicsUnit.Pixel);

            base.OnPaint(e);
        }

        private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                player1.posX -= 5;
            }
            if (e.KeyCode == Keys.D)
            {
                player1.posX += 5;
            }
            if (e.KeyCode == Keys.S)
            {
                player1.posY += 5;
            }
            if (e.KeyCode == Keys.W)
            {
                player1.posY -= 5;
            }
            //draw();
            Invalidate();
        }

        private void draw()
        {
            


            gc.Clear(Color.White);
            
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
    }
}
