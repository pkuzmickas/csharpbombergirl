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
    /*
     * Controls the Winner screen form
     */ 
    public partial class WinScreen : Form
    {
        //Variables to store the previous forms
        Form1 gameForm;
        Form menuForm;

        //Constructor which requires the game form and the menu form and the winner image to be given
        public WinScreen(Form1 gameForm, Form menuForm, Image winner)
        {
            InitializeComponent();
            // Saves the forms, image and refreshes the picture box so it would be drawn properly
            this.menuForm = menuForm;
            this.gameForm = gameForm;
            pictureBox1.Image = winner;
            pictureBox1.Refresh();
        }

        //Event handler for the 'Play Again' button
        private void button1_Click(object sender, EventArgs e)
        {
           //Disposes of the game, win form and creates it from scratch
            gameForm.Dispose();
            new Form1(menuForm, gameForm.numOfPlayers);
            this.Dispose();
        }
        //Event handler for the 'Main Menu' button
        private void button2_Click(object sender, EventArgs e)
        {
            //Disposes of the game, win form and makes the menu visible
            Menu m = new Menu();
            m.Show();
            gameForm.Dispose();
            this.Dispose();
        }
        //Event handler for the 'Exit' X button
        private void WinScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Disposes of the game, win form and makes the menu visible
            Menu m = new Menu();
            m.Show();
            gameForm.Dispose();
            this.Dispose();
        }
    }
}
