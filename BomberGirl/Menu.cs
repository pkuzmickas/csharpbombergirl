using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BomberGirl
{
    /*
     * The window for the main menu
     */ 
    public partial class Menu : Form
    {
        // The sound player variable
        System.Media.SoundPlayer sound;
        // Main constructor which initializes the window and plays the background music
        public Menu()
        {
            InitializeComponent();
            //Sets it not resizable
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            sound = new System.Media.SoundPlayer(Properties.Resources.Vicious);
            sound.Play();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }
       
        // Event handlers for pressing on the '2 Player' button
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            // Changes the image to look like its clicked
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/2playerButtonC.png");

        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            // Changes the image to look like its not clicked
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/2playerButton.png");
            //Hides the menu, stops the music and starts the game
            this.Hide();
            sound.Stop();
            new Form1(this, 2);
        }
        // Event handlers for pressing on the 'help' button
        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            // Changes the image to look like its clicked
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/helpButtonC.png");

        }

        private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/helpButton.png");
            
            StreamReader file = File.OpenText("help.txt");
            string text = "";
            string line = null;
            while ((line = file.ReadLine()) != null)
            {
                text += line;
                text += "\n";
            }
            file.Close();
            MessageBox.Show(text, "", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        // Event handlers for pressing on the 'exit' button
        private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
        {
            // Changes the image to look like its clicked
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/exitButtonC.png");
        }

        private void pictureBox4_MouseUp(object sender, MouseEventArgs e)
        {
            // Changes the image to look like its not clicked
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/exitButton.png");
            Application.Exit();

        }
        // Event handlers for pressing on the '3 player' button
        private void pictureBox1_MouseDown_1(object sender, MouseEventArgs e)
        {
            // Changes the image to look like its clicked
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/3playerC.png");
        }
        private void pictureBox1_MouseUp_1(object sender, MouseEventArgs e)
        {
            // Changes the image to look like its not clicked
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/3player.png");
            //Hides the menu, stops the music and starts the game
            this.Hide();
            sound.Stop();
            new Form1(this, 3);
        }
        // Event handlers for pressing on the '4player' button
        private void pictureBox1_MouseUp_2(object sender, MouseEventArgs e)
        {
            // Changes the image to look like its not clicked
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/4player.png");
            //Hides the menu, stops the music and starts the game
            this.Hide();
            sound.Stop();
            new Form1(this, 4);
        }
        private void pictureBox1_MouseDown_2(object sender, MouseEventArgs e)
        {
            // Changes the image to look like its clicked
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/4playerC.png");
        }
        // Event handler for when pressing on the about 'author' button
        private void authorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Loads the author.txt file
            StreamReader file = File.OpenText("author.txt");
            string text = "";
            string line = null;
            // Reads all the lines of the file and adds them to the line string then to the text string, reads into the line string again until the file is finished
            while ((line = file.ReadLine()) != null)
            {
                text += line;
                text += "\n";
            }
            file.Close();
            // Shows a MessageBox with the 'text' string
            MessageBox.Show(text, "", MessageBoxButtons.OK);
        }
        // Event handler for when pressing on the about 'credits' button
        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Loads the credits.txt file
            StreamReader file = File.OpenText("credits.txt");
            string text = "";
            string line = null;
            // Reads all the lines of the file and adds them to the line string then to the text string, reads into the line string again until the file is finished
            while ((line = file.ReadLine()) != null)
            {
                text += line;
                text += "\n";
            }
            file.Close();
            // Shows a MessageBox with the 'text' string
            MessageBox.Show(text, "", MessageBoxButtons.OK);
        }
    }
}
