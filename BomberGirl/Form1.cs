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
        //Global Variables (most of them self explanatory due to good naming practice)

        //Menu form
        Form lastForm;
        //The graphics handler
        private Graphics gc;
        Player player1, player2, player3, player4;
        Grid grid;
        int[,] Board;
        int[,] PowerupBoard;
        //The board for storing the location of the bombs for animating them
        int[,] BombAnimBoard;
        //The board to store the location of the powerups and their visibility (true if visible)
        bool[,] drawingPowerupsBoard;
        public int numOfPlayers;
        //Players alive
        int playersStanding;
       
        //Amount of powerups labels at the top of the screen below the player images
        Label[] player1Labels = new Label[4];
        Label[] player2Labels = new Label[4];
        Label[] player3Labels = new Label[4];
        Label[] player4Labels = new Label[4];

        //Loading of the images for the game
        Image wall = Image.FromFile("Sprites/wall2.png");
        Image grass = Image.FromFile("Sprites/grass2.png");
        Image box = Image.FromFile("Sprites/box2.png");
        Image bombs = Image.FromFile("Sprites/bombs.png");
        Image explosion = Image.FromFile("Sprites/explosion.png");
        Image extraBomb = Image.FromFile("Sprites/extraBomb2.png");
        Image extraFlame = Image.FromFile("Sprites/extraFlame2.png");
        Image extraLife = Image.FromFile("Sprites/extraLife2.png");
        Image extraSpeed = Image.FromFile("Sprites/extraSpeed2.png");
        Image player1score = Image.FromFile("Sprites/player1score.png");
        Image player2score = Image.FromFile("Sprites/player2score.png");
        Image player3score = Image.FromFile("Sprites/player3score.png");
        Image player4score = Image.FromFile("Sprites/player4score.png");
        Image explosions = Image.FromFile("Sprites/explosions.png");
        
        bool gameOver = false;
        //Keeps a record of all the bombs placed on the screen to handle the explosions properly
        LinkedList<Bomb> bombsPlaced = new LinkedList<Bomb>();

        //Structure to keep the information about the bomb (timer of explosion, position, explosion positions and animations)
        public struct Bomb
        {
            public int col, row;
            public System.Timers.Timer timer;
            public int[,] ExplosionBoard;
        }

        //The constructor which requires the menu to be given as a form and the number of players specified
        public Form1(Form lastForm, int numOfPlayers)
        {
            InitializeComponent();
            // Initializes the variables accordingly
            this.numOfPlayers = numOfPlayers;
            playersStanding = numOfPlayers;
            this.ShowInTaskbar = true;
            this.lastForm = lastForm;
            //Makes the window not resizable
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            grid = new Grid();
            //Initializes the graphics handler
            gc = this.CreateGraphics();
            //Makes the window visible
            this.Show();
            Board = grid.getGrid();
            PowerupBoard = grid.getPowerupGrid();
            //Initializes the grids, setting the initial values to 0 or false
            drawingPowerupsBoard = new bool[Constants.BOARD_WIDTH, Constants.BOARD_HEIGHT];
            BombAnimBoard = new int[Constants.BOARD_WIDTH, Constants.BOARD_HEIGHT];
            for (int i = 0; i < Constants.BOARD_WIDTH; i++)
            {
                for (int j = 0; j < Constants.BOARD_HEIGHT; j++)
                {
                    drawingPowerupsBoard[i, j] = false;
                    BombAnimBoard[i, j] = 0;
                }
            }
            //Creates the players accordingly to the amount specified
            if (numOfPlayers >= 2)
            {
                // Initializes the Player class with the ID, positions
                player1 = new Player(0);
                player1.posX = Constants.SPRITE_SIZE;
                player1.posY = Constants.SPRITE_SIZE * 3;
                // Starts animating the Player with a timer
                System.Timers.Timer timer = new System.Timers.Timer(Constants.ANIM_SPEED);
                // A lambda expression to forward the handler function to the 'animate' function, specifying the Player class as an extra argument
                timer.Elapsed += (sender, e) => animate(sender, e, player1);
                // Begins the timer
                timer.Enabled = true;
                // Initializes the Player class with the ID, positions
                player2 = new Player(1);
                player2.posX = (Constants.SCREEN_X / Constants.SPRITE_SIZE - 1) * Constants.SPRITE_SIZE;
                player2.posY = (Constants.SCREEN_Y / Constants.SPRITE_SIZE - 2) * Constants.SPRITE_SIZE;
                // Starts animating the Player with a timer
                System.Timers.Timer timer2 = new System.Timers.Timer(Constants.ANIM_SPEED);
                // A lambda expression to forward the handler function to the 'animate' function, specifying the Player class as an extra argument
                timer2.Elapsed += (sender, e) => animate(sender, e, player2);
                // Begins the timer
                timer2.Enabled = true;
                // Sets the images at the top to the according player images
                pictureBox4.Image = player1score;
                pictureBox3.Image = player2score;
            }
            if (numOfPlayers >= 3)
            {
                // Initializes the Player class with the ID, positions
                player3 = new Player(2);
                player3.posX = Constants.SPRITE_SIZE* (Constants.BOARD_WIDTH-2);
                player3.posY = Constants.SPRITE_SIZE * 3;
                // Starts animating the Player with a timer
                System.Timers.Timer timer = new System.Timers.Timer(Constants.ANIM_SPEED);
                // A lambda expression to forward the handler function to the 'animate' function, specifying the Player class as an extra argument
                timer.Elapsed += (sender, e) => animate(sender, e, player3);
                // Begins the timer
                timer.Enabled = true;


                
               
            }
            if (numOfPlayers == 4)
            {
                // Initializes the Player class with the ID, positions
                player4 = new Player(3);
                player4.posX = Constants.SPRITE_SIZE;
                player4.posY = Constants.SPRITE_SIZE * (Constants.BOARD_HEIGHT);
                // Starts animating the Player with a timer
                System.Timers.Timer timer = new System.Timers.Timer(Constants.ANIM_SPEED);
                // A lambda expression to forward the handler function to the 'animate' function, specifying the Player class as an extra argument
                timer.Elapsed += (sender, e) => animate(sender, e, player4);
                // Begins the timer
                timer.Enabled = true;


                
            }
            // Sets the images at the top to the according player images
            pictureBox5.Image = player3score;
            pictureBox6.Image = player4score;

            // The location of each of the player's labels at the top below their images(for pickups)
            int p1LabelLoc = 12;
            int p2LabelLoc = 115;
            int p3LabelLoc = 417;
            int p4LabelLoc = 527;
            //Draws the labels with specific spaces between them
            for (int i = 0; i < 4; i++)
            {
                if (numOfPlayers >= 2)
                {
                    player1Labels[i] = new System.Windows.Forms.Label();
                    player1Labels[i].BackColor = System.Drawing.Color.Gray;
                    player1Labels[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
                    player1Labels[i].Location = new System.Drawing.Point(p1LabelLoc, 85);
                    player1Labels[i].Size = new System.Drawing.Size(18, 20);
                    if (i == 1) player1Labels[i].Text = "1";
                    else player1Labels[i].Text = "0";
                    Controls.Add(player1Labels[i]);
                    Controls.SetChildIndex(player1Labels[i], 3);
                    player1Labels[i].Refresh();

                    player2Labels[i] = new System.Windows.Forms.Label();
                    player2Labels[i].BackColor = System.Drawing.Color.Gray;
                    player2Labels[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
                    player2Labels[i].Location = new System.Drawing.Point(p2LabelLoc, 85);
                    player2Labels[i].Size = new System.Drawing.Size(18, 20);
                    if (i == 1) player2Labels[i].Text = "1";
                    else player2Labels[i].Text = "0";
                    Controls.Add(player2Labels[i]);
                    Controls.SetChildIndex(player2Labels[i], 3);
                    // Redraws the label
                    player2Labels[i].Refresh();
                    // Changes the distance of the new label
                    p1LabelLoc += 26;
                    p2LabelLoc += 26;
                }
                if (numOfPlayers >= 3)
                {
                    player3Labels[i] = new System.Windows.Forms.Label();
                    player3Labels[i].BackColor = System.Drawing.Color.Gray;
                    player3Labels[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
                    player3Labels[i].Location = new System.Drawing.Point(p3LabelLoc, 85);
                    player3Labels[i].Size = new System.Drawing.Size(18, 20);
                    if (i == 1) player3Labels[i].Text = "1";
                    else player3Labels[i].Text = "0";
                    Controls.Add(player3Labels[i]);
                    Controls.SetChildIndex(player3Labels[i], 3);
                    // Redraws the label
                    player3Labels[i].Refresh();
                    // Changes the distance of the new label
                    p3LabelLoc += 26;
                }
                if (numOfPlayers == 4)
                {
                    player4Labels[i] = new System.Windows.Forms.Label();
                    player4Labels[i].BackColor = System.Drawing.Color.Gray;
                    player4Labels[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
                    player4Labels[i].Location = new System.Drawing.Point(p4LabelLoc, 85);
                    player4Labels[i].Size = new System.Drawing.Size(18, 20);
                    if (i == 1) player4Labels[i].Text = "1";
                    else player4Labels[i].Text = "0";
                    Controls.Add(player4Labels[i]);
                    Controls.SetChildIndex(player4Labels[i], 3);
                    player4Labels[i].Refresh();
                    // Changes the distance of the new label
                    p4LabelLoc += 26;
                }
                
               
                
            }

        }

        // Function for updating the player's properties (position etc.)
        public void updatePlayer(Player player)
        {
            // If moving left subtract the speed to the X position of the player 
            if (player.moving_left)
            {
                // Gets the full collision box with both sides of the sprite
                int col = (int)((player.posX - (float)player.speed) / Constants.SPRITE_SIZE);
                int row1 = (int)((player.posY + Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE - 2);
                int row2 = (int)((player.posY + Constants.SPRITE_SIZE - Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE - 2);
                // Checks the collision box whether the player is colliding with anything, if not continues with the movement, if does - nothing happens
                if ((Board[col, row1] == 0 && Board[col, row2] == 0) || (player.justBombed && ((Board[col, row1] == 3 && Board[col, row2] == 3) || (Board[col, row1] == 3 && Board[col, row2] == 0) || (Board[col, row1] == 0 && Board[col, row2] == 3))))
                {
                    player.posX -= (float)player.speed;
                    col = (int)((player.posX - (float)player.speed) / Constants.SPRITE_SIZE);
                    row1 = (int)((player.posY + Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE - 2);
                    row2 = (int)((player.posY + Constants.SPRITE_SIZE - Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE - 2);
                    // Checks whether the player has stepped off the bomb he just placed, if yes then the collisions with the bomb become valid (the player can't go through the bomb)
                    if (Board[col, row1] == 0 && Board[col, row2] == 0)
                    {
                        player.justBombed = false;
                    }
                }


            }
            // If moving right add the speed to the X position of the player 
            if (player.moving_right)
            {
                // Gets the full collision box with both sides of the sprite
                int col = (int)((player.posX + (float)player.speed) / Constants.SPRITE_SIZE);
                int row1 = (int)((player.posY + Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE - 2);
                int row2 = (int)((player.posY + Constants.SPRITE_SIZE - Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE - 2);
                // Checks the collision box whether the player is colliding with anything, if not continues with the movement, if does - nothing happens
                if ((Board[col + 1, row1] == 0 && Board[col + 1, row2] == 0) || (player.justBombed && ((Board[col + 1, row1] == 3 && Board[col + 1, row2] == 3) || (Board[col + 1, row1] == 3 && Board[col + 1, row2] == 0) || (Board[col + 1, row1] == 0 && Board[col + 1, row2] == 3))))
                {
                    player.posX += (float)player.speed;
                    col = (int)((player.posX + (float)player.speed) / Constants.SPRITE_SIZE);
                    row1 = (int)((player.posY + Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE - 2);
                    row2 = (int)((player.posY + Constants.SPRITE_SIZE - Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE - 2);
                    // Checks whether the player has stepped off the bomb he just placed, if yes then the collisions with the bomb become valid (the player can't go through the bomb)
                    if (Board[col + 1, row1] == 0 && Board[col + 1, row2] == 0)
                    {
                        player.justBombed = false;
                    }
                }
            }
            // If moving up subtract the speed to the Y position of the player 
            if (player.moving_up)
            {
                // Gets the full collision box with both sides of the sprite
                int col1 = (int)((player.posX + Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE);
                int col2 = (int)((player.posX + Constants.SPRITE_SIZE - Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE);
                int row = (int)((player.posY - (float)player.speed) / Constants.SPRITE_SIZE - 2);
                // Checks the collision box whether the player is colliding with anything, if not continues with the movement, if does - nothing happens
                if ((Board[col1, row] == 0 && Board[col2, row] == 0) || (player.justBombed && ((Board[col1, row] == 3 && Board[col2, row] == 3) || (Board[col1, row] == 3 && Board[col2, row] == 0) || (Board[col1, row] == 0 && Board[col2, row] == 3))))
                {
                    col1 = (int)((player.posX + Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE);
                    col2 = (int)((player.posX + Constants.SPRITE_SIZE - Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE);
                    row = (int)((player.posY - (float)player.speed) / Constants.SPRITE_SIZE - 2);
                    // Checks whether the player has stepped off the bomb he just placed, if yes then the collisions with the bomb become valid (the player can't go through the bomb)
                    if (Board[col1, row] == 0 && Board[col2, row] == 0)
                    {
                        player.justBombed = false;
                    }
                    player.posY -= (float)player.speed;
                }

            }
            // If moving down add the speed to the Y position of the player 
            if (player.moving_down)
            {
                // Gets the full collision box with both sides of the sprite
                int col1 = (int)((player.posX + Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE);
                int col2 = (int)((player.posX + Constants.SPRITE_SIZE - Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE);
                int row = (int)((player.posY + (float)player.speed) / Constants.SPRITE_SIZE - 2);
                // Checks the collision box whether the player is colliding with anything, if not continues with the movement, if does - nothing happens
                if ((Board[col1, row + 1] == 0 && Board[col2, row + 1] == 0) || (player.justBombed && ((Board[col1, row + 1] == 3 && Board[col2, row + 1] == 3) || (Board[col1, row + 1] == 3 && Board[col2, row + 1] == 0) || (Board[col1, row + 1] == 0 && Board[col2, row + 1] == 3))))
                {
                    player.posY += (float)player.speed;
                    col1 = (int)((player.posX + Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE);
                    col2 = (int)((player.posX + Constants.SPRITE_SIZE - Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE);
                    row = (int)((player.posY + (float)player.speed) / Constants.SPRITE_SIZE - 2);
                    // Checks whether the player has stepped off the bomb he just placed, if yes then the collisions with the bomb become valid (the player can't go through the bomb)
                    if (Board[col1, row + 1] == 0 && Board[col2, row + 1] == 0)
                    {
                        player.justBombed = false;
                    }
                }
            }

            // If the player is not taking damage already, checks the full collision box of the player and sees if it collides with an explosion
            if (!player.takingDamage)
            {
                int col1 = (int)((player.posX + Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE);
                int col2 = (int)((player.posX + Constants.SPRITE_SIZE - Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE);
                int row1 = (int)((player.posY + Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE - 2);
                int row2 = (int)((player.posY + Constants.SPRITE_SIZE - Constants.COLLISION_ERROR) / Constants.SPRITE_SIZE - 2);
                foreach (Bomb b in bombsPlaced)
                {
                    // If the player collides with an explosion decreases their lives
                    if (b.ExplosionBoard[col1, row1] != 0 || b.ExplosionBoard[col2, row2] != 0)
                    {
                        player.lives--;
                        player.takingDamage = true;
                        // If the player lives reach 0, kills the player
                        if (player.lives == 0)
                        {
                            player.dead = true;
                            playersStanding--;
                            // If there is only 1 player left standing, declares him the winner
                            if (playersStanding == 1)
                            {
                                Image winner;
                                if (!player1.dead)
                                {
                                    winner = player1score;
                                }
                                else if (!player2.dead)
                                {
                                    winner = player2score;
                                }
                                else if (numOfPlayers>=3 && !player3.dead)
                                {
                                    winner = player3score;
                                }
                                else
                                {
                                    winner = player4score;
                                    
                                }
                                //Creates the winner screen and plays the winning music
                                WinScreen winScreen = new WinScreen(this, lastForm, winner);
                                winScreen.Show();
                                gameOver = true;
                                System.Media.SoundPlayer sound = new System.Media.SoundPlayer(Properties.Resources.winSound);
                                sound.Play();
                            }

                        }
                        else
                        {
                            System.Timers.Timer timer = new System.Timers.Timer(2000);
                            timer.Elapsed += (sender, e) => takingDamageHandler(sender, e, player);
                            timer.Enabled = true;
                           

                        }
                    }
                }
            }
            // Gets the player position column and row on the main board
            int c = (int)(player.posX + Constants.SPRITE_SIZE / 2) / Constants.SPRITE_SIZE;
            int r = (int)(player.posY + Constants.SPRITE_SIZE / 2) / Constants.SPRITE_SIZE - 2;
            if (drawingPowerupsBoard[c, r])
            {
                //Checks whether the player has stepped on a pickup and if they did acts accordingly
                drawingPowerupsBoard[c, r] = false;
                switch (PowerupBoard[c, r])
                {
                    case 1: //flame pickup
                        {
                            //increases the explosion size
                            player.explosionSize++;
                            break;
                        }
                    case 2: //bomblimit pickup
                        {
                            //increases the bomb limit
                            player.bombLimit++;
                            break;
                        }
                    case 3: //speed pickup
                        {
                            //increases the player speed
                            player.speed += 0.25f;
                            player.bootsCollected++;
                            break;
                        }
                    case 4: //life pickup
                        {
                            //increases the amount of lives the player has
                            player.lives++;
                            break;
                        }
                }

            }
            //Changes the labels of the players according to how many pickups they have collected of each type
            if (player.id == 0)
            {
                player1Labels[0].Text = "" + (player.bootsCollected);
                player1Labels[1].Text = "" + (player.lives);
                player1Labels[2].Text = "" + (player.bombLimit - player.bombsPlaced);
                player1Labels[3].Text = "" + (player.explosionSize - 2);
            }
            if (player.id == 1)
            {
                player2Labels[0].Text = "" + (player.bootsCollected);
                player2Labels[1].Text = "" + (player.lives);
                player2Labels[2].Text = "" + (player.bombLimit - player.bombsPlaced);
                player2Labels[3].Text = "" + (player.explosionSize - 2);
            }
            if (player.id == 2)
            {
                player3Labels[0].Text = "" + (player.bootsCollected);
                player3Labels[1].Text = "" + (player.lives);
                player3Labels[2].Text = "" + (player.bombLimit - player.bombsPlaced);
                player3Labels[3].Text = "" + (player.explosionSize - 2);
            }
            if (player.id == 3)
            {
                player4Labels[0].Text = "" + (player.bootsCollected);
                player4Labels[1].Text = "" + (player.lives);
                player4Labels[2].Text = "" + (player.bombLimit - player.bombsPlaced);
                player4Labels[3].Text = "" + (player.explosionSize - 2);

            }


            // Refreshes the screen to be drawn correctly
            Invalidate();
        }

        // The event handler to stop the player from taking damage for a specific time
        private void takingDamageHandler(object source, EventArgs e, Player player)
        {
            ((System.Timers.Timer)source).Enabled = false;
            player.takingDamage = false;
        }


        // Main game loop (is called every frame)
        protected override void OnPaint(PaintEventArgs e)
        {
            // Gets the graphics
            Graphics dc = e.Graphics;
            // Draws everything on the screen
            draw(dc);
            // If the game is not over updates the players
            if (!gameOver)
            {
                updatePlayer(player1);
                updatePlayer(player2);
                if (numOfPlayers >= 3)
                {
                    updatePlayer(player3);
                }
                if (numOfPlayers == 4)
                {
                    updatePlayer(player4);
                }
            }
            // Part of the OnPaint function (calls the parent function)
            base.OnPaint(e);
        }
        // Keyboard key press handler
        private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            // If the player is not dead records it's movement and bomb placement
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
                if (e.KeyCode == Keys.V)
                {
                    placeBomb(player1);
                }
            }
            // If the player is not dead records it's movement and bomb placement
            if (!player2.dead)
            {
                if (e.KeyCode == Keys.J)
                {
                    player2.moving_left = true;

                }
                else if (e.KeyCode == Keys.L)
                {
                    player2.moving_right = true;
                }
                if (e.KeyCode == Keys.K)
                {
                    player2.moving_down = true;
                }
                if (e.KeyCode == Keys.I)
                {
                    player2.moving_up = true;
                }
                if (e.KeyCode == Keys.OemQuestion)
                {
                    placeBomb(player2);
                }
            }
            // If the player is not dead records it's movement and bomb placement
            if (numOfPlayers>=3 && !player3.dead)
            {
                if (e.KeyCode == Keys.Left)
                {
                    player3.moving_left = true;

                }
                else if (e.KeyCode == Keys.Right)
                {
                    player3.moving_right = true;
                }
                if (e.KeyCode == Keys.Down)
                {
                    player3.moving_down = true;
                }
                if (e.KeyCode == Keys.Up)
                {
                    player3.moving_up = true;
                }
                if (e.KeyCode == Keys.NumPad0)
                {
                    placeBomb(player3);
                }
            }
            // If the player is not dead records it's movement and bomb placement
            if (numOfPlayers == 4 && !player4.dead)
            {
                if (e.KeyCode == Keys.NumPad4)
                {
                    player4.moving_left = true;

                }
                else if (e.KeyCode == Keys.NumPad6)
                {
                    player4.moving_right = true;
                }
                if (e.KeyCode == Keys.NumPad5)
                {
                    player4.moving_down = true;
                }
                if (e.KeyCode == Keys.NumPad8)
                {
                    player4.moving_up = true;
                }
                if (e.KeyCode == Keys.Add)
                {
                    placeBomb(player4);
                }
            }
            // Was used for testing (prints the grid)
            /*if (e.KeyCode == Keys.P)
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
            }*/
        }
        // 
        // Function to find the player's position on the grid and place a bomb there
        void placeBomb(Player player)
        {
            int col = (int)((player.posX + Constants.SPRITE_SIZE / 2) / Constants.SPRITE_SIZE);
            int row = (int)((player.posY + Constants.SPRITE_SIZE / 2) / Constants.SPRITE_SIZE - 2);
            // If there is no bomb at that place and the player has not exceeded his bomb limit places a bomb there, increases the counter
            if (Board[col, row] != 3 && player.bombsPlaced != player.bombLimit)
            {
                Board[col, row] = 3;
                player.bombsPlaced++;
                createBomb(col, row, player);
                player.justBombed = true;
            }
        }
        // Function to create the bomb with the proper explosions associated with it
        void createBomb(int col, int row, Player player)
        {
            Bomb bomb = new Bomb();
            // Creates a new array for recording the correct explosion animations and positions
            bomb.ExplosionBoard = new int[Constants.BOARD_WIDTH, Constants.BOARD_HEIGHT];
            for (int i = 0; i < Constants.BOARD_WIDTH; i++)
            {
                for (int j = 0; j < Constants.BOARD_HEIGHT; j++)
                {
                    bomb.ExplosionBoard[i, j] = 0;
                }
            }
            bomb.col = col;
            bomb.row = row;
            // The timer for the bomb to explode
            bomb.timer = new System.Timers.Timer(Constants.BOMB_TIMER_IN_SECONDS * 1000);
            bomb.timer.Elapsed += (sender, e) => explode(sender, e, bomb, player);
            bomb.timer.Enabled = true;
            // Adds the bomb to the bombPlaced list
            bombsPlaced.AddLast(bomb);
            // The timer for the bomb to stop exploding
            System.Timers.Timer timer = new System.Timers.Timer(Constants.BOMB_TIMER_IN_SECONDS * 1000 + 1000);
            timer.Elapsed += (sender, e) => explosionEnd(sender, e, bomb, player);
            timer.Enabled = true;
            // The timer for the bomb to animate
            System.Timers.Timer timer2 = new System.Timers.Timer(Constants.BOMB_TIMER_IN_SECONDS * 1000 / 6);
            timer2.Elapsed += (sender, e) => animate(sender, e, bomb);
            timer2.Enabled = true;

        }
        // The event handler for the bomb to explode
        private void explode(object source, ElapsedEventArgs e, Bomb bomb, Player player)
        {
            
            player.bombsPlaced--;
            // Starts the sound of the bomb exploding
            System.Media.SoundPlayer sound = new System.Media.SoundPlayer("explosionSound.wav");
            sound.Play();
           // Disables the timer
            ((System.Timers.Timer)source).Enabled = false;
            Board[bomb.col, bomb.row] = 0;
            // Checks the position of the explosion and gives a proper animation to it from the spritesheet (1-7)
            for (int i = bomb.col; i < bomb.col + 1 + player.explosionSize; i++)
            {
                if (Board[i, bomb.row] != 1)
                {
                    if (bomb.col == i) bomb.ExplosionBoard[i, bomb.row] = 1;
                    else if (Board[i + 1, bomb.row] != 1)
                    {
                        bomb.ExplosionBoard[i, bomb.row] = 2;
                    }
                    else
                    {
                        bomb.ExplosionBoard[i, bomb.row] = 3;
                    }
                    if (bomb.col + player.explosionSize == i)
                    {
                        bomb.ExplosionBoard[i, bomb.row] = 3;
                    }
                }
                if (Board[i, bomb.row] == 2)
                {
                    bomb.ExplosionBoard[i, bomb.row] = 3;
                    break;
                }
                else if (Board[i, bomb.row] == 1) break;
            }
            for (int i = bomb.col; i > bomb.col - 1 - player.explosionSize; i--)
            {
                if (Board[i, bomb.row] != 1)
                {
                    if (bomb.col == i) bomb.ExplosionBoard[i, bomb.row] = 1;
                    else if (Board[i - 1, bomb.row] != 1)
                    {
                        bomb.ExplosionBoard[i, bomb.row] = 2;
                    }
                    else
                    {
                        bomb.ExplosionBoard[i, bomb.row] = 5;
                    }
                    if (bomb.col - player.explosionSize == i)
                    {
                        bomb.ExplosionBoard[i, bomb.row] = 5;
                    }
                }
                if (Board[i, bomb.row] == 2)
                {
                    bomb.ExplosionBoard[i, bomb.row] = 5;
                    break;
                }
                else if (Board[i, bomb.row] == 1) break;
            }
            for (int i = bomb.row; i > bomb.row - 1 - player.explosionSize; i--)
            {
                if (Board[bomb.col, i] != 1)
                {
                    if (bomb.row == i) bomb.ExplosionBoard[bomb.col, i] = 1;
                    else if (Board[bomb.col, i - 1] != 1)
                    {
                        bomb.ExplosionBoard[bomb.col, i] = 7;
                    }
                    else
                    {
                        bomb.ExplosionBoard[bomb.col, i] = 6;
                    }
                    if (bomb.row - player.explosionSize == i)
                    {
                        bomb.ExplosionBoard[bomb.col, i] = 6;
                    }
                }
                if (Board[bomb.col, i] == 2)
                {
                    bomb.ExplosionBoard[bomb.col, i] = 6;
                    break;
                }
                else if (Board[bomb.col, i] == 1) break;
            }
            for (int i = bomb.row; i < bomb.row + 1 + player.explosionSize; i++)
            {
                if (Board[bomb.col, i] != 1)
                {
                    if (bomb.row == i) bomb.ExplosionBoard[bomb.col, i] = 1;
                    else if (Board[bomb.col, i + 1] != 1)
                    {
                        bomb.ExplosionBoard[bomb.col, i] = 7;
                    }
                    else
                    {
                        bomb.ExplosionBoard[bomb.col, i] = 4;
                    }
                    if (bomb.row + player.explosionSize == i)
                    {
                        bomb.ExplosionBoard[bomb.col, i] = 4;
                    }
                }
                if (Board[bomb.col, i] == 2)
                {
                    bomb.ExplosionBoard[bomb.col, i] = 4;
                    break;
                }
                else if (Board[bomb.col, i] == 1) break;
            }



        }

        // The event handler for the bomb to stop exploding
        private void explosionEnd(object source, ElapsedEventArgs e, Bomb bomb, Player player)
        {
            //Stops the timer
            ((System.Timers.Timer)source).Enabled = false;

            // Checks whether a position has a bomb exploding there and if it does, disables the animation and sprites at that position
            for (int i = bomb.col; i < bomb.col + 1 + player.explosionSize; i++)
            {
                if (Board[i, bomb.row] != 1) bomb.ExplosionBoard[i, bomb.row] = 0;
                //If finds a box destroys it revealing all the powerups it has
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
            // Checks whether a position has a bomb exploding there and if it does, disables the animation and sprites at that position
            for (int i = bomb.col; i > bomb.col - 1 - player.explosionSize; i--)
            {
                if (Board[i, bomb.row] != 1) bomb.ExplosionBoard[i, bomb.row] = 0;
                //If finds a box destroys it revealing all the powerups it has
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
            // Checks whether a position has a bomb exploding there and if it does, disables the animation and sprites at that position
            for (int i = bomb.row; i > bomb.row - 1 - player.explosionSize; i--)
            {
                if (Board[bomb.col, i] != 1) bomb.ExplosionBoard[bomb.col, i] = 0;
                //If finds a box destroys it revealing all the powerups it has
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
            // Checks whether a position has a bomb exploding there and if it does, disables the animation and sprites at that position
            for (int i = bomb.row; i < bomb.row + 1 + player.explosionSize; i++)
            {
                if (Board[bomb.col, i] != 1) bomb.ExplosionBoard[bomb.col, i] = 0;
                //If finds a box destroys it revealing all the powerups it has
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

        private void draw(Graphics gc)
        {
            // Redraws all of the picture boxes
            pictureBox1.Refresh();
            pictureBox2.Refresh();
            pictureBox3.Refresh();
            pictureBox4.Refresh();
            pictureBox5.Refresh();
            pictureBox6.Refresh();
            // Redraws all of the player pickup labels
            if (numOfPlayers >= 2)
            {
                player1Labels[1].Refresh();
                player1Labels[2].Refresh();
                player1Labels[3].Refresh();
                player1Labels[0].Refresh();
                player2Labels[1].Refresh();
                player2Labels[2].Refresh();
                player2Labels[3].Refresh();
                player2Labels[0].Refresh();
            }
            if (numOfPlayers >= 3)
            {
                player3Labels[1].Refresh();
                player3Labels[2].Refresh();
                player3Labels[3].Refresh();
                player3Labels[0].Refresh();
            }
            if (numOfPlayers == 4)
            {
                player4Labels[1].Refresh();
                player4Labels[2].Refresh();
                player4Labels[3].Refresh();
                player4Labels[0].Refresh();
            }
            // Goes through the entire grid and draws the elements on it
            for (int i = 0; i < Constants.BOARD_WIDTH; i++)
            {
                for (int j = 0; j < Constants.BOARD_HEIGHT; j++)
                {
                    // If the element is 1 draws the wall sprite
                    if (Board[i, j] == 1)
                    {

                        gc.DrawImage(wall, i * Constants.SPRITE_SIZE, (j + 2) * Constants.SPRITE_SIZE, Constants.SPRITE_SIZE, Constants.SPRITE_SIZE);

                    }
                    // If the element is 0/3 draws the grass sprite
                    else if (Board[i, j] == 0 || Board[i, j] == 3)
                    {

                        gc.DrawImage(grass, i * Constants.SPRITE_SIZE, (j + 2) * Constants.SPRITE_SIZE, Constants.SPRITE_SIZE + 2, Constants.SPRITE_SIZE + 2);

                    }
                    // If the element is 2 draws the box sprite
                    else if (Board[i, j] == 2)
                    {

                        gc.DrawImage(box, i * Constants.SPRITE_SIZE, (j + 2) * Constants.SPRITE_SIZE, Constants.SPRITE_SIZE, Constants.SPRITE_SIZE);

                    }
                    // If the element is true draws a powerup
                    if (drawingPowerupsBoard[i, j])
                    {
                        switch (PowerupBoard[i, j])
                        {
                            //If it equals 1 draws the extra flame powerup
                            case 1:
                                {
                                    gc.DrawImage(extraFlame, i * Constants.SPRITE_SIZE, (j + 2) * Constants.SPRITE_SIZE, Constants.SPRITE_SIZE, Constants.SPRITE_SIZE);
                                    break;
                                }
                            //If it equals 1 draws the extra bomb powerup
                            case 2:
                                {
                                    gc.DrawImage(extraBomb, i * Constants.SPRITE_SIZE, (j + 2) * Constants.SPRITE_SIZE, Constants.SPRITE_SIZE, Constants.SPRITE_SIZE);
                                    break;
                                }
                            //If it equals 1 draws the extra speed powerup
                            case 3:
                                {
                                    gc.DrawImage(extraSpeed, i * Constants.SPRITE_SIZE, (j + 2) * Constants.SPRITE_SIZE, Constants.SPRITE_SIZE, Constants.SPRITE_SIZE);
                                    break;
                                }
                            //If it equals 1 draws the extra life powerup
                            case 4:
                                {
                                    gc.DrawImage(extraLife, i * Constants.SPRITE_SIZE, (j + 2) * Constants.SPRITE_SIZE, Constants.SPRITE_SIZE, Constants.SPRITE_SIZE);
                                    break;
                                }
                        }
                    }
                    //If it equals 3 draws the bomb sprite
                    if (Board[i, j] == 3)
                    {

                        gc.DrawImage(bombs, new Rectangle(i * Constants.SPRITE_SIZE, (j + 2) * Constants.SPRITE_SIZE, Constants.SPRITE_SIZE - 2, Constants.SPRITE_SIZE - 2), new Rectangle(BombAnimBoard[i, j] * Constants.SPRITE_SIZE, 0, Constants.SPRITE_SIZE, Constants.SPRITE_SIZE), GraphicsUnit.Pixel);
                    }
                    

                }
            }

            
            // If the player is not dead, draws the current player sprite from the spritesheet
            if (!player1.dead)
            {

                gc.DrawImage(player1.spriteSheet, new Rectangle((int)player1.posX, (int)player1.posY, Constants.SPRITE_SIZE, Constants.SPRITE_SIZE), new Rectangle(player1.spriteNr * Constants.SPRITE_SIZE, Constants.SPRITE_SIZE * player1.id, Constants.SPRITE_SIZE, Constants.SPRITE_SIZE), GraphicsUnit.Pixel);

            }
            // If the player is not dead, draws the current player sprite from the spritesheet
            if (!player2.dead)
            {

                gc.DrawImage(player2.spriteSheet, new Rectangle((int)player2.posX, (int)player2.posY, Constants.SPRITE_SIZE, Constants.SPRITE_SIZE), new Rectangle(player2.spriteNr * Constants.SPRITE_SIZE, Constants.SPRITE_SIZE * player2.id, Constants.SPRITE_SIZE, Constants.SPRITE_SIZE), GraphicsUnit.Pixel);
            }
            if (numOfPlayers >= 3)
            {
                // If the player is not dead, draws the current player sprite from the spritesheet
                if (!player3.dead)
                {

                    gc.DrawImage(player3.spriteSheet, new Rectangle((int)player3.posX, (int)player3.posY, Constants.SPRITE_SIZE, Constants.SPRITE_SIZE), new Rectangle(player3.spriteNr * Constants.SPRITE_SIZE, Constants.SPRITE_SIZE * player3.id, Constants.SPRITE_SIZE, Constants.SPRITE_SIZE), GraphicsUnit.Pixel);

                }
            }
            if (numOfPlayers == 4)
            {
                // If the player is not dead, draws the current player sprite from the spritesheet
                if (!player4.dead)
                {

                    gc.DrawImage(player4.spriteSheet, new Rectangle((int)player4.posX, (int)player4.posY, Constants.SPRITE_SIZE, Constants.SPRITE_SIZE), new Rectangle(player4.spriteNr * Constants.SPRITE_SIZE, Constants.SPRITE_SIZE * player4.id, Constants.SPRITE_SIZE, Constants.SPRITE_SIZE), GraphicsUnit.Pixel);
                }
            }
            // Checks through all the bombs in the list and draws their explosions accordingly
            foreach (Bomb b in bombsPlaced)
            {
                for (int i = 0; i < Constants.BOARD_WIDTH; i++)
                {
                    for (int j = 0; j < Constants.BOARD_HEIGHT; j++)
                    {
                        if (b.ExplosionBoard[i, j] != 0)
                        {
                            gc.DrawImage(explosions, i * Constants.SPRITE_SIZE, (j + 2) * Constants.SPRITE_SIZE, new Rectangle(0, Constants.SPRITE_SIZE * (b.ExplosionBoard[i, j] - 1), Constants.SPRITE_SIZE, Constants.SPRITE_SIZE), GraphicsUnit.Pixel);
                        }
                    }
                }
            }


        }
        // Handler to animate the bombs
        private void animate(object sender, EventArgs e, Bomb bomb)
        {
            BombAnimBoard[bomb.col, bomb.row]++;

            if (BombAnimBoard[bomb.col, bomb.row] == 6)
            {
                BombAnimBoard[bomb.col, bomb.row] = 0;
                ((System.Timers.Timer)sender).Enabled = false;
            }
        }
        // Handler to animate the players
        private void animate(object sender, EventArgs e, Player player)
        {
            // If the player is taking damage make it blink from visible to invisible
            if (player.takingDamage && player.spriteNr != -1)
            {
                player.lastSpriteNr = player.spriteNr;
                player.spriteNr = -1;
            }
            else
            {
                // Change the current sprite of the player according to the last sprite the player had and the direction it's moving
                if (player.moving_up)
                {
                    if ((player.spriteNr > 0 && player.spriteNr < 8) || player.spriteNr == 9) player.spriteNr = 0;
                    else if (player.spriteNr == 0)
                    {
                        player.spriteNr = 8;
                    }
                    else if (player.spriteNr == 8)
                    {
                        player.spriteNr = 9;
                    }
                    else
                    {
                        player.spriteNr = 0;
                    }
                }
                // Change the current sprite of the player according to the last sprite the player had and the direction it's moving
                else if (player.moving_down)
                {
                    if (player.spriteNr > 0 && player.spriteNr < 3)
                    {
                        player.spriteNr++;
                    }
                    else player.spriteNr = 1;
                }
                // Change the current sprite of the player according to the last sprite the player had and the direction it's moving
                else if (player.moving_right)
                {
                    if (player.spriteNr > 3 && player.spriteNr < 7)
                    {
                        player.spriteNr++;
                    }
                    else
                    {
                        player.spriteNr = 4;
                    }
                }
                // Change the current sprite of the player according to the last sprite the player had and the direction it's moving
                else if (player.moving_left)
                {

                    if (player.spriteNr >= 10 && player.spriteNr < 13)
                    {
                        player.spriteNr++;
                    }
                    else player.spriteNr = 10;

                }
                else if (player.takingDamage)
                {
                    player.spriteNr = player.lastSpriteNr;
                }
            }
            if (!player.takingDamage && player.spriteNr == -1)
            {
                player.spriteNr = 0;
            }

        }
        // If the user closes the form the application should exit
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        // Handlers for pressing the main menu button
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            // Makes it looked like it's pressed
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/mainmenuC.png");
            ((PictureBox)sender).Refresh();
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            // Makes it looked like it's not pressed anymore
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/mainmenu.png");
            ((PictureBox)sender).Refresh();

            lastForm.Show();
            this.Dispose();
        }


        // Event handler for the keyboard button being let go
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            // If the player is not dead stop it's movement in the current direction
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
            // If the player is not dead stop it's movement in the current direction
            if (!player2.dead)
            {
                if (e.KeyCode == Keys.J)
                {
                    player2.moving_left = false;

                }
                else if (e.KeyCode == Keys.L)
                {
                    player2.moving_right = false;
                }
                if (e.KeyCode == Keys.K)
                {
                    player2.moving_down = false;
                }
                if (e.KeyCode == Keys.I)
                {
                    player2.moving_up = false;
                }

            }
            // If the player is not dead stop it's movement in the current direction
            if (numOfPlayers >= 3 && !player3.dead)
            {
                if (e.KeyCode == Keys.Left)
                {
                    player3.moving_left = false;

                }
                else if (e.KeyCode == Keys.Right)
                {
                    player3.moving_right = false;
                }
                if (e.KeyCode == Keys.Down)
                {
                    player3.moving_down = false;
                }
                if (e.KeyCode == Keys.Up)
                {
                    player3.moving_up = false;
                }

            }
            // If the player is not dead stop it's movement in the current direction
            if (numOfPlayers == 4 && !player4.dead)
            {
                if (e.KeyCode == Keys.NumPad4)
                {
                    player4.moving_left = false;

                }
                else if (e.KeyCode == Keys.NumPad6)
                {
                    player4.moving_right = false;
                }
                if (e.KeyCode == Keys.NumPad5)
                {
                    player4.moving_down = false;
                }
                if (e.KeyCode == Keys.NumPad8)
                {
                    player4.moving_up = false;
                }

            }

        }
    }
}
