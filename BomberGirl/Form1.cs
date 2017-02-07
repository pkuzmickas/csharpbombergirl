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
using System.Diagnostics;

namespace BomberGirl
{

    

    public partial class Form1 : Form
    {
        Form lastForm;
        private Graphics gc;
        Player player1, player2;
        Grid grid;
        int[,] Board;
        int[,] PowerupBoard;
        int[,] BombAnimBoard;
        bool[,] ExplosionBoard;
        bool[,] drawingPowerupsBoard;

        Image wall = Image.FromFile("Sprites/wall.png");
        Image grass = Image.FromFile("Sprites/grass.png");
        Image box = Image.FromFile("Sprites/box.png");
        Image bombs = Image.FromFile("Sprites/bombs.png");
        Image explosion = Image.FromFile("Sprites/explosion.png");
        Image extraBomb = Image.FromFile("Sprites/extraBomb.png");
        Image extraFlame = Image.FromFile("Sprites/extraFlame.png");
        Image extraLife = Image.FromFile("Sprites/extraLife.png");
        Image extraSpeed = Image.FromFile("Sprites/extraSpeed.png");


        //Stopwatch deltaTime = new Stopwatch();
        int deltaTime = 1;


        public struct Bomb
        {
            public int col, row;
            public System.Timers.Timer timer;
            
        }

        public Form1(Form lastForm)
        {
            InitializeComponent();
            this.lastForm = lastForm;
            //deltaTime.Start();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            grid = new Grid();
            gc = this.CreateGraphics();
            this.Show();
            Board = grid.getGrid();
            PowerupBoard = grid.getPowerupGrid();
            ExplosionBoard = new bool[Constants.BOARD_WIDTH, Constants.BOARD_HEIGHT];
            drawingPowerupsBoard = new bool[Constants.BOARD_WIDTH, Constants.BOARD_HEIGHT];
            BombAnimBoard = new int[Constants.BOARD_WIDTH, Constants.BOARD_HEIGHT];
            for (int i = 0; i < Constants.BOARD_WIDTH; i++)
            {
                for(int j=0; j < Constants.BOARD_HEIGHT; j++)
                {
                    ExplosionBoard[i, j] = false;
                    drawingPowerupsBoard[i, j] = false;
                    BombAnimBoard[i, j] = 0;
                }
            }

            player1 = new Player(2);
            player1.posX = Constants.SPRITE_SIZE;
            player1.posY = Constants.SPRITE_SIZE * 3;
            System.Timers.Timer timer = new System.Timers.Timer(Constants.ANIM_SPEED);
            timer.Elapsed += (sender, e) => animate(sender, e, player1);
            timer.Enabled = true;

            player2 = new Player(3);
            player2.posX = (Constants.SCREEN_X / Constants.SPRITE_SIZE - 1) * Constants.SPRITE_SIZE;
            player2.posY = (Constants.SCREEN_Y / Constants.SPRITE_SIZE - 2) * Constants.SPRITE_SIZE;
            System.Timers.Timer timer2 = new System.Timers.Timer(Constants.ANIM_SPEED);
            timer2.Elapsed += (sender, e) => animate(sender, e, player2);
            timer2.Enabled = true;
            
            draw(gc);
         
        }

        

        public void updatePlayer(Player player)
        {

           


            if (player.moving_left)
            {
                int col = (int)((player.posX-(float)player.speed*deltaTime/*.ElapsedMilliseconds/1000*/) / Constants.SPRITE_SIZE);
                int row1 = (int)((player.posY + Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE - 2);
                int row2 = (int)((player.posY + Constants.SPRITE_SIZE - Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE - 2);
                if ((Board[col, row1] == 0 && Board[col, row2] == 0) || (player.justBombed && ((Board[col, row1] == 3 && Board[col, row2] == 3) || (Board[col, row1] == 3 && Board[col, row2] == 0) || (Board[col, row1] == 0 && Board[col, row2] == 3))))
                {
                    player.posX -= (float)player.speed*deltaTime/*.ElapsedMilliseconds/1000*/;
                    col = (int)((player.posX - (float)player.speed*deltaTime/*.ElapsedMilliseconds/1000*/) / Constants.SPRITE_SIZE);
                    row1 = (int)((player.posY + Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE - 2);
                    row2 = (int)((player.posY + Constants.SPRITE_SIZE - Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE - 2);
                    if (Board[col, row1] == 0 && Board[col, row2] == 0)
                    {
                        player.justBombed = false;
                    }
                }


            }
            if(player.moving_right)
            {
                int col = (int)((player.posX + (float)player.speed*deltaTime/*.ElapsedMilliseconds/1000*/) / Constants.SPRITE_SIZE);
                int row1 = (int)((player.posY + Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE - 2);
                int row2 = (int)((player.posY + Constants.SPRITE_SIZE - Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE - 2);
                if ((Board[col+1, row1] == 0 && Board[col+1, row2] == 0) || (player.justBombed && ((Board[col+1, row1] == 3 && Board[col+1, row2] == 3) || (Board[col+1, row1] == 3 && Board[col+1, row2] == 0) || (Board[col+1, row1] == 0 && Board[col+1, row2] == 3))))
                {
                    player.posX += (float)player.speed*deltaTime/*.ElapsedMilliseconds/1000*/;
                    col = (int)((player.posX + (float)player.speed*deltaTime/*.ElapsedMilliseconds/1000*/) / Constants.SPRITE_SIZE);
                    row1 = (int)((player.posY + Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE - 2);
                    row2 = (int)((player.posY + Constants.SPRITE_SIZE - Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE - 2);
                    if (Board[col+1, row1] == 0 && Board[col+1, row2] == 0)
                    {
                        player.justBombed = false;
                    }
                }
            }
            if (player.moving_up)
            {
                int col1 = (int)((player.posX + Constants.COLLISION_ERROR)  / Constants.SPRITE_SIZE);
                int col2 = (int)((player.posX + Constants.SPRITE_SIZE - Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE);
                int row = (int)((player.posY - (float)player.speed*deltaTime/*.ElapsedMilliseconds/1000*/) / Constants.SPRITE_SIZE - 2);
                if ((Board[col1, row] == 0 && Board[col2, row] == 0) || (player.justBombed && ((Board[col1, row] == 3 && Board[col2, row] == 3) || (Board[col1, row] == 3 && Board[col2, row] == 0) || (Board[col1, row] == 0 && Board[col2, row] == 3))))
                {
                    col1 = (int)((player.posX + Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE);
                    col2 = (int)((player.posX + Constants.SPRITE_SIZE - Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE);
                    row = (int)((player.posY - (float)player.speed*deltaTime/*.ElapsedMilliseconds/1000*/) / Constants.SPRITE_SIZE - 2);
                    if (Board[col1, row] == 0 && Board[col2, row] == 0)
                    {
                        player.justBombed = false;
                    }
                    player.posY -= (float)player.speed*deltaTime/*.ElapsedMilliseconds/1000*/;
                }
                
            }
            if (player.moving_down)
            {
                int col1 = (int)((player.posX + Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE);
                int col2 = (int)((player.posX + Constants.SPRITE_SIZE - Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE);
                int row = (int)((player.posY + (float)player.speed*deltaTime/*.ElapsedMilliseconds/1000*/) / Constants.SPRITE_SIZE - 2);
                if ((Board[col1, row+1] == 0 && Board[col2, row+1] == 0) || (player.justBombed && ((Board[col1, row+1] == 3 && Board[col2, row+1] == 3) || (Board[col1, row+1] == 3 && Board[col2, row+1] == 0) || (Board[col1, row+1] == 0 && Board[col2, row+1] == 3))))
                {
                    player.posY += (float)player.speed*deltaTime/*.ElapsedMilliseconds/1000*/;
                    col1 = (int)((player.posX + Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE);
                    col2 = (int)((player.posX + Constants.SPRITE_SIZE - Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE);
                    row = (int)((player.posY + (float)player.speed*deltaTime/*.ElapsedMilliseconds/1000*/) / Constants.SPRITE_SIZE - 2);
                    if (Board[col1, row+1] == 0 && Board[col2, row+1] == 0)
                    {
                        player.justBombed = false;
                    }
                }
            }

            if (!player.takingDamage)
            {
                int col1 = (int)((player.posX + Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE);
                int col2 = (int)((player.posX + Constants.SPRITE_SIZE - Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE);
                int row1 = (int)((player.posY + Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE - 2);
                int row2 = (int)((player.posY + Constants.SPRITE_SIZE - Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE - 2);
                if (ExplosionBoard[col1, row1] || ExplosionBoard[col2, row2])
                {
                    player.lives--;
                    player.takingDamage = true;
                    if (player.lives == 0)
                    {
                        Console.WriteLine("Game OVER");
                        player.dead = true;

                    }
                    else
                    {
                        Console.WriteLine(player.lives);
                        
                        Console.WriteLine("OUCH!");
                        System.Timers.Timer timer = new System.Timers.Timer(2000);
                        timer.Elapsed += (sender, e) => takingDamageHandler(sender, e, player);
                        timer.Enabled = true;
                    }
                }
            }
            int c = (int)(player.posX + Constants.SPRITE_SIZE/2) / Constants.SPRITE_SIZE;
            int r = (int)(player.posY + Constants.SPRITE_SIZE/2) / Constants.SPRITE_SIZE - 2;
            if (drawingPowerupsBoard[c, r])
            {
                drawingPowerupsBoard[c, r] = false;
                switch(PowerupBoard[c,r])
                {
                    case 1: //flame
                        {
                            player.explosionSize++;
                            break;
                        }
                    case 2: //bomblimit
                        {
                            player.bombLimit++;
                            break;
                        }
                    case 3: //speed
                        {
                            player.speed++;
                            break;
                        }
                    case 4: //life
                        {
                            player.lives++;
                            break;
                        }
                }
            }
            Invalidate();
        }

        private void takingDamageHandler(object source, EventArgs e, Player player)
        {
            ((System.Timers.Timer)source).Enabled = false;
            player.takingDamage = false;
        }

     

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
            //deltaTime.Stop();
            draw(dc);
            updatePlayer(player1);
            updatePlayer(player2);
            //deltaTime.Reset();
            //deltaTime.Start();
            
            base.OnPaint(e);
        }

        private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (!player1.dead)
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
                if (e.KeyCode == Keys.B)
                {
                    placeBomb(player1);
                }
            }

            if (!player2.dead)
            {
                if (e.KeyCode == Keys.Left)
                {
                    player2.moving_left = true;

                }
                if (e.KeyCode == Keys.Right)
                {
                    player2.moving_right = true;
                }
                if (e.KeyCode == Keys.Down)
                {
                    player2.moving_down = true;
                }
                if (e.KeyCode == Keys.Up)
                {
                    player2.moving_up = true;
                }
                if (e.KeyCode == Keys.NumPad0)
                {
                    placeBomb(player2);
                }
            }

            if (e.KeyCode == Keys.P)
            {
                for (int i = 0; i < Constants.BOARD_HEIGHT; i++)
                {
                    for (int j = 0; j < Constants.BOARD_WIDTH; j++)
                    {
                        Console.Write(Board[j, i] + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                Console.WriteLine();
                for (int i = 0; i < Constants.BOARD_HEIGHT; i++)
                {
                    for (int j = 0; j < Constants.BOARD_WIDTH; j++)
                    {
                        Console.Write(PowerupBoard[j, i] + " ");
                    }
                    Console.WriteLine();
                }
            }
        }

        void placeBomb(Player player)
        {
            int col = (int)((player.posX + Constants.SPRITE_SIZE / 2) / Constants.SPRITE_SIZE);
            int row = (int)((player.posY + Constants.SPRITE_SIZE / 2) / Constants.SPRITE_SIZE - 2);
            if (Board[col, row] != 3 && player.bombsPlaced != player.bombLimit)
            {
                Board[col, row] = 3;
                player.bombsPlaced++;
                createBomb(col, row, player);
                player.justBombed = true;
            }
        }

        void createBomb(int col, int row, Player player)
        {
            Bomb bomb = new Bomb();
            bomb.col = col;
            bomb.row = row;
            bomb.timer = new System.Timers.Timer(Constants.BOMB_TIMER_IN_SECONDS * 1000);
            bomb.timer.Elapsed += (sender, e) => explode(sender, e, bomb, player);
            bomb.timer.Enabled = true;

            System.Timers.Timer timer = new System.Timers.Timer(Constants.BOMB_TIMER_IN_SECONDS * 1000 + 1000);
            timer.Elapsed += (sender, e) => explosionEnd(sender, e, bomb, player);
            timer.Enabled = true;

            System.Timers.Timer timer2 = new System.Timers.Timer(Constants.BOMB_TIMER_IN_SECONDS * 1000 / 6);
            timer2.Elapsed += (sender, e) => animate(sender, e, bomb);
            timer2.Enabled = true;

        }

        private void explode(object source, ElapsedEventArgs e, Bomb bomb, Player player)
        {
            player.bombsPlaced--;
            ((System.Timers.Timer)source).Enabled = false;
            Board[bomb.col, bomb.row] = 0;
            for (int i = bomb.col; i < bomb.col + 1 + player.explosionSize; i++)
            {
                if (Board[i, bomb.row] != 1) ExplosionBoard[i, bomb.row] = true;
                if (Board[i, bomb.row] == 2)
                {
                    break;
                }
                else if (Board[i, bomb.row] == 1) break;
            }
            for (int i = bomb.col; i > bomb.col - 1 - player.explosionSize; i--)
            {
                if (Board[i, bomb.row] != 1) ExplosionBoard[i, bomb.row] = true;
                if (Board[i, bomb.row] == 2)
                {
                    break;
                }
                else if (Board[i, bomb.row] == 1) break;
            }
            for (int i = bomb.row; i > bomb.row - 1 - player.explosionSize; i--)
            {
                if (Board[bomb.col, i] != 1) ExplosionBoard[bomb.col, i] = true;
                if (Board[bomb.col, i] == 2)
                {
                    break;
                }
                else if (Board[bomb.col, i] == 1) break;
            }
            for (int i = bomb.row; i < bomb.row + 1 + player.explosionSize; i++)
            {
                if (Board[bomb.col, i] != 1) ExplosionBoard[bomb.col, i] = true;
                if (Board[bomb.col, i] == 2)
                {
                    break;
                }
                else if (Board[bomb.col, i] == 1) break;
            }

            

        }

        private void explosionEnd(object source, ElapsedEventArgs e, Bomb bomb, Player player)
        {

            ((System.Timers.Timer)source).Enabled = false;

            for (int i = bomb.col; i < bomb.col + 1 + player.explosionSize; i++)
            {
                if(Board[i,bomb.row]!=1) ExplosionBoard[i, bomb.row] = false;
                if (Board[i, bomb.row] == 2)
                {
                    Board[i, bomb.row] = 0;
                    if (PowerupBoard[i, bomb.row] > 0)
                    {
                        drawingPowerupsBoard[i, bomb.row] = true;
                    }
                    break;
                }
                else if (Board[i, bomb.row] == 1) break;
            }
            for (int i = bomb.col; i > bomb.col - 1 - player.explosionSize; i--)
            {
                if (Board[i, bomb.row] != 1) ExplosionBoard[i, bomb.row] = false;
                if (Board[i, bomb.row] == 2)
                {
                    Board[i, bomb.row] = 0;
                    if (PowerupBoard[i, bomb.row] > 0)
                    {
                        drawingPowerupsBoard[i, bomb.row] = true;
                    }
                    break;
                }
                else if (Board[i, bomb.row] == 1) break;
            }
            for (int i = bomb.row; i > bomb.row - 1 - player.explosionSize; i--)
            {
                if (Board[bomb.col, i] != 1) ExplosionBoard[bomb.col, i] = false;

                if (Board[bomb.col, i] == 2)
                {
                    Board[bomb.col, i] = 0;
                    if (PowerupBoard[bomb.col, i] > 0)
                    {
                        drawingPowerupsBoard[bomb.col, i] = true;
                    }
                    break;
                }
                else if (Board[bomb.col, i] == 1) break;
            }
            for (int i = bomb.row; i < bomb.row + 1 + player.explosionSize; i++)
            {
                if (Board[bomb.col, i] != 1) ExplosionBoard[bomb.col, i] = false;

                if (Board[bomb.col, i] == 2)
                {
                    Board[bomb.col, i] = 0;
                    if (PowerupBoard[bomb.col, i] > 0)
                    {
                        drawingPowerupsBoard[bomb.col, i] = true;
                    }
                    break;
                }
                else if (Board[bomb.col, i] == 1) break;
            }
        }

        private void draw( Graphics gc)
        {
            
            pictureBox1.Refresh();
            pictureBox2.Refresh();

            for (int i = 0; i < grid.getGridWidth(); i++)
            {
                for (int j = 0; j < grid.getGridHeight(); j++)
                {
                    if (Board[i, j] == 1)
                    {

                        gc.DrawImage(wall, i * Constants.SPRITE_SIZE, (j + 2) * Constants.SPRITE_SIZE, Constants.SPRITE_SIZE + 2, Constants.SPRITE_SIZE + 2);

                    }
                    else if (Board[i, j] == 0 || Board[i, j] == 3)
                    {

                        gc.DrawImage(grass, i * Constants.SPRITE_SIZE, (j + 2) * Constants.SPRITE_SIZE, Constants.SPRITE_SIZE + 2, Constants.SPRITE_SIZE + 2);

                    }
                    else if (Board[i, j] == 2)
                    {

                        gc.DrawImage(box, i * Constants.SPRITE_SIZE, (j + 2) * Constants.SPRITE_SIZE, Constants.SPRITE_SIZE + 2, Constants.SPRITE_SIZE + 2);

                    }
                    
                    if (drawingPowerupsBoard[i, j])
                    {
                        switch (PowerupBoard[i, j])
                        {
                            case 1:
                                {
                                    gc.DrawImage(extraFlame, i * Constants.SPRITE_SIZE, (j + 2) * Constants.SPRITE_SIZE, Constants.SPRITE_SIZE + 2, Constants.SPRITE_SIZE + 2);
                                    break;
                                }
                            case 2:
                                {
                                    gc.DrawImage(extraBomb, i * Constants.SPRITE_SIZE, (j + 2) * Constants.SPRITE_SIZE, Constants.SPRITE_SIZE + 2, Constants.SPRITE_SIZE + 2);
                                    break;
                                }
                            case 3:
                                {
                                    gc.DrawImage(extraSpeed, i * Constants.SPRITE_SIZE, (j + 2) * Constants.SPRITE_SIZE, Constants.SPRITE_SIZE + 2, Constants.SPRITE_SIZE + 2);
                                    break;
                                }
                            case 4:
                                {
                                    gc.DrawImage(extraLife, i * Constants.SPRITE_SIZE, (j + 2) * Constants.SPRITE_SIZE, Constants.SPRITE_SIZE + 2, Constants.SPRITE_SIZE + 2);
                                    break;
                                }
                        }
                    }
                    if (Board[i, j] == 3)
                    {
                        Console.WriteLine(i + " " + j);
                        Console.WriteLine(BombAnimBoard[i, j]);
                        gc.DrawImage(bombs, new Rectangle(i * Constants.SPRITE_SIZE, (j + 2) * Constants.SPRITE_SIZE, Constants.SPRITE_SIZE - 2, Constants.SPRITE_SIZE - 2), new Rectangle(BombAnimBoard[i,j]*16, 0, 16, 16), GraphicsUnit.Pixel);
                    }
                    if (ExplosionBoard[i, j] == true)
                    {
                        gc.DrawImage(explosion, i * Constants.SPRITE_SIZE, (j + 2) * Constants.SPRITE_SIZE, Constants.SPRITE_SIZE + 2, Constants.SPRITE_SIZE + 2);
                    }
                    
                }
            }

            if (!player1.dead)
            {
                
                gc.DrawImage(player1.spriteSheet, new Rectangle((int)player1.posX, (int)player1.posY, Constants.SPRITE_SIZE - 5, Constants.SPRITE_SIZE - 5), new Rectangle(player1.spriteNr * 16, 16 * player1.id , 15, 15), GraphicsUnit.Pixel);
            }
            if (!player2.dead)
            {
                gc.DrawImage(player2.spriteSheet, new Rectangle((int)player2.posX, (int)player2.posY, Constants.SPRITE_SIZE - 5, Constants.SPRITE_SIZE - 5), new Rectangle(player2.spriteNr * 16, 16 * player2.id , 15, 15), GraphicsUnit.Pixel);
            }
        }
        private void animate(object sender, EventArgs e, Bomb bomb)
        {
            BombAnimBoard[bomb.col,bomb.row]++;

            if (BombAnimBoard[bomb.col, bomb.row] == 6)
            {
                BombAnimBoard[bomb.col, bomb.row] = 0;
                ((System.Timers.Timer)sender).Enabled = false;
            }
        }
        private void animate(object sender, EventArgs e, Player player)
        {
            if (player.moving_up) {
                if (player.fliped)
                {
                    player.fliped = false;
                    try
                    {
                        player.spriteSheet.RotateFlip(RotateFlipType.Rotate180FlipY);
                    } catch(Exception)
                    {
                        Console.WriteLine("Flipping error");
                    }
                }
                if ((player.spriteNr > 0 && player.spriteNr < 8) || player.spriteNr == 9) player.spriteNr = 0;
                else if(player.spriteNr==0)
                {
                    player.spriteNr = 8;
                }
                else if(player.spriteNr==8)
                {
                    player.spriteNr = 9;
                }
            }
            else if(player.moving_down)
            {
                if (player.fliped)
                {
                    player.fliped = false;
                    try
                    {
                        player.spriteSheet.RotateFlip(RotateFlipType.Rotate180FlipY);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Flipping error");
                    }
                }
                if (player.spriteNr > 0 && player.spriteNr < 3)
                {
                    player.spriteNr++;
                }
                else player.spriteNr = 1;
            }
            else if(player.moving_right)
            {
                if(player.fliped)
                {
                    player.fliped = false;
                    try
                    {
                        player.spriteSheet.RotateFlip(RotateFlipType.Rotate180FlipY);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Flipping error");
                    }
                }
                if(player.spriteNr >3 && player.spriteNr<7)
                {
                    player.spriteNr++;
                }
                else
                {
                    player.spriteNr = 4;
                }
            }
            else if (player.moving_left)
            {
                if (!player.fliped)
                {
                    player.fliped = true;
                    try
                    {
                        player.spriteSheet.RotateFlip(RotateFlipType.Rotate180FlipY);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Flipping error");
                    }
                }


                if (player.spriteNr > 1 && player.spriteNr < 5)
                {
                    player.spriteNr++;
                }
                else player.spriteNr = 2;

            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/mainmenuC.png");
            ((PictureBox)sender).Refresh();
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/mainmenu.png");
            ((PictureBox)sender).Refresh();
            lastForm.Show();
            this.Dispose();
        }
        
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (!player1.dead)
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
            if (!player2.dead)
            {
                if (e.KeyCode == Keys.Left)
                {
                    player2.moving_left = false;
                }
                if (e.KeyCode == Keys.Right)
                {
                    player2.moving_right = false;
                }
                if (e.KeyCode == Keys.Down)
                {
                    player2.moving_down = false;
                }
                if (e.KeyCode == Keys.Up)
                {
                    player2.moving_up = false;
                }
            }

        }
    }
}
