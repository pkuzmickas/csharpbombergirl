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
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/1playerButtonC.png");
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/1playerButton.png");
            
            //this.Dispose();
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/2playerButtonC.png");

        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/2playerButton.png");
            this.Hide();
            new Form1(this);
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
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

        private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/exitButtonC.png");
        }

        private void pictureBox4_MouseUp(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/exitButton.png");
            Application.Exit();

        }
    }
}
