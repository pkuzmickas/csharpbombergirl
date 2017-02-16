using System;
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
    public partial class OnlineForm : Form
    {
        public OnlineForm()
        {
            InitializeComponent();
        }

        private void OnlineForm_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("ga");
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/joinGameC.png");
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/joinGame.png");
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/createGameC.png");
        }

        private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/createGame.png");
        }
    }
}
