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
    public partial class WinScreen : Form
    {
        Form gameForm, menuForm;
        public WinScreen(Form gameForm, Form menuForm, Image winner)
        {
            InitializeComponent();
            this.menuForm = menuForm;
            this.gameForm = gameForm;
            pictureBox1.Image = winner;
            pictureBox1.Refresh();
        }


        private void button1_Click(object sender, EventArgs e)
        {
           
            gameForm.Dispose();
            new Form1(menuForm);
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Menu m = new Menu();
            m.Show();
            gameForm.Dispose();
            this.Dispose();
        }

        private void WinScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            Menu m = new Menu();
            m.Show();
            gameForm.Dispose();
            this.Dispose();
        }
    }
}
