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
using System.Timers;

namespace BomberGirl
{



    public partial class Form1 : Form
    {
        private const int SPRITE_SIZE = 42;
        private const int SPEED = 1;
        private const int COLLISION_ERROR = 9;
        private const int NUMBER_OF_BOMBS = 10;
        private Graphics gc;
        Player player1;
        Grid grid;
        int[,] Board;

        Image wall = Image.FromFile("Sprites/wall.png");
        Image grass = Image.FromFile("Sprites/grass.png");
        Image box = Image.FromFile("Sprites/box.png");
        Image bombs = Image.FromFile("Sprites/bombs.png");

        public struct Bomb
        {
            public int col, row;
            public System.Timers.Timer timer;
            
        }

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
                int row1 = (int)((player.posY + COLLISION_ERROR) / SPRITE_SIZE - 2);
                int row2 = (int)((player.posY + SPRITE_SIZE - COLLISION_ERROR) / SPRITE_SIZE - 2);
                if ((Board[col, row1] == 0 && Board[col, row2] == 0) || (player.justBombed && ((Board[col, row1] == 3 && Board[col, row2] == 3) || (Board[col, row1] == 3 && Board[col, row2] == 0) || (Board[col, row1] == 0 && Board[col, row2] == 3))))
                {
                    player.posX -= SPEED;
                    col = (int)((player.posX - SPEED) / SPRITE_SIZE);
                    row1 = (int)((player.posY + COLLISION_ERROR) / SPRITE_SIZE - 2);
                    row2 = (int)((player.posY + SPRITE_SIZE - COLLISION_ERROR) / SPRITE_SIZE - 2);
                    if (Board[col, row1] == 0 && Board[col, row2] == 0)
                    {
                        player.justBombed = false;
                    }
                }


            }
            if(player.moving_right)
            {
                int col = (int)((player.posX + SPEED) / SPRITE_SIZE);
                int row1 = (int)((player.posY + COLLISION_ERROR) / SPRITE_SIZE - 2);
                int row2 = (int)((player.posY + SPRITE_SIZE - COLLISION_ERROR) / SPRITE_SIZE - 2);
                if ((Board[col+1, row1] == 0 && Board[col+1, row2] == 0) || (player.justBombed && ((Board[col+1, row1] == 3 && Board[col+1, row2] == 3) || (Board[col+1, row1] == 3 && Board[col+1, row2] == 0) || (Board[col+1, row1] == 0 && Board[col+1, row2] == 3))))
                {
                    player.posX += SPEED;
                    col = (int)((player.posX + SPEED) / SPRITE_SIZE);
                    row1 = (int)((player.posY + COLLISION_ERROR) / SPRITE_SIZE - 2);
                    row2 = (int)((player.posY + SPRITE_SIZE - COLLISION_ERROR) / SPRITE_SIZE - 2);
                    if (Board[col+1, row1] == 0 && Board[col+1, row2] == 0)
                    {
                        player.justBombed = false;
                    }
                }
            }
            if (player.moving_up)
            {
                int col1 = (int)((player.posX + COLLISION_ERROR)  / SPRITE_SIZE);
                int col2 = (int)((player.posX + SPRITE_SIZE - COLLISION_ERROR) / SPRITE_SIZE);
                int row = (int)((player.posY - SPEED) / SPRITE_SIZE - 2);
                if ((Board[col1, row] == 0 && Board[col2, row] == 0) || (player.justBombed && ((Board[col1, row] == 3 && Board[col2, row] == 3) || (Board[col1, row] == 3 && Board[col2, row] == 0) || (Board[col1, row] == 0 && Board[col2, row] == 3))))
                {
                    col1 = (int)((player.posX + COLLISION_ERROR) / SPRITE_SIZE);
                    col2 = (int)((player.posX + SPRITE_SIZE - COLLISION_ERROR) / SPRITE_SIZE);
                    row = (int)((player.posY - SPEED) / SPRITE_SIZE - 2);
                    if (Board[col1, row] == 0 && Board[col2, row] == 0)
                    {
                        player.justBombed = false;
                    }
                    player.posY -= SPEED;
                }
                
            }
            if (player.moving_down)
            {
                int col1 = (int)((player.posX + COLLISION_ERROR) / SPRITE_SIZE);
                int col2 = (int)((player.posX + SPRITE_SIZE - COLLISION_ERROR) / SPRITE_SIZE);
                int row = (int)((player.posY + SPEED) / SPRITE_SIZE - 2);
                if ((Board[col1, row+1] == 0 && Board[col2, row+1] == 0) || (player.justBombed && ((Board[col1, row+1] == 3 && Board[col2, row+1] == 3) || (Board[col1, row+1] == 3 && Board[col2, row+1] == 0) || (Board[col1, row+1] == 0 && Board[col2, row+1] == 3))))
                {
                    player.posY += SPEED;
                    col1 = (int)((player.posX + COLLISION_ERROR) / SPRITE_SIZE);
                    col2 = (int)((player.posX + SPRITE_SIZE - COLLISION_ERROR) / SPRITE_SIZE);
                    row = (int)((player.posY + SPEED) / SPRITE_SIZE - 2);
                    if (Board[col1, row+1] == 0 && Board[col2, row+1] == 0)
                    {
                        player.justBombed = false;
                    }
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
            if (e.KeyCode == Keys.B)
            {
                int col = (int)((player1.posX+SPRITE_SIZE/2)/SPRITE_SIZE); 
                int row = (int)((player1.posY+SPRITE_SIZE/2)/SPRITE_SIZE-2);
                if (Board[col, row] != 3 && player1.bombsPlaced!=player1.bombLimit)
                {
                    Board[col, row] = 3;
                    player1.bombsPlaced++;
                    createBomb(col, row, player1);
                    player1.justBombed = true;
                }

                
            }
        }

        void createBomb(int col, int row, Player player)
        {
            Bomb bomb = new Bomb();
            bomb.col = col;
            bomb.row = row;
            bomb.timer = new System.Timers.Timer(2000);
            bomb.timer.Elapsed += (sender, e) => explode(sender, e, bomb, player);
            bomb.timer.Enabled = true;
        }

        private void explode(object source, ElapsedEventArgs e, Bomb bomb, Player player)
        {
            Console.WriteLine("BOOOM");
            ((System.Timers.Timer)source).Enabled = false;
            player.bombsPlaced--;
            Board[bomb.col, bomb.row] = 0;
            for (int i = bomb.col+1; i < bomb.col+1 + player.explosionSize; i++)
            {
                if (Board[i, bomb.row] == 2)
                {
                    Board[i, bomb.row] = 0;
                    break;
                }
                else if (Board[i, bomb.row] == 1) break;
            }
            for (int i = bomb.col-1; i > bomb.col-1 - player.explosionSize; i--)
            {
                if (Board[i, bomb.row] == 2)
                {
                    Board[i, bomb.row] = 0;
                    break;
                }
                else if (Board[i, bomb.row] == 1) break;
            }
            for (int i = bomb.row-1; i > bomb.row-1 - player.explosionSize; i--)
            {
                if (Board[bomb.col, i] == 2)
                {
                    Board[bomb.col, i] = 0;
                    break;
                }
                else if (Board[bomb.col, i] == 1) break;
            }
            for (int i = bomb.row+1; i < bomb.row+1 + player.explosionSize; i++)
            {
                if (Board[bomb.col, i] == 2)
                {
                    Board[bomb.col, i] = 0;
                    break;
                }
                else if (Board[bomb.col, i] == 1) break;
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
                    else if (Board[i, j] == 0 || Board[i, j] == 3)
                    {

                        gc.DrawImage(grass, i * SPRITE_SIZE, (j + 2) * SPRITE_SIZE, SPRITE_SIZE + 2, SPRITE_SIZE + 2);

                    }
                    else if (Board[i, j] == 2)
                    {

                        gc.DrawImage(box, i * SPRITE_SIZE, (j + 2) * SPRITE_SIZE, SPRITE_SIZE + 2, SPRITE_SIZE + 2);

                    }
                    if (Board[i, j] == 3)
                    {
                        gc.DrawImage(bombs, new Rectangle(i * SPRITE_SIZE, (j + 2) * SPRITE_SIZE, SPRITE_SIZE - 2, SPRITE_SIZE - 2), new Rectangle(0, 0, 16, 16), GraphicsUnit.Pixel);
                    }
                }
            }
            

            gc.DrawImage(player1.spriteSheet, new Rectangle((int)player1.posX, (int)player1.posY, SPRITE_SIZE-5, SPRITE_SIZE-5), new Rectangle(0, 16 * 2 - 1, 15, 15), GraphicsUnit.Pixel);

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
