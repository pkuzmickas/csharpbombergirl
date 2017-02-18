using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace BomberGirl
{
    public partial class OnlineForm : Form
    {
        
        bool keyPressed = false, admin = false;
        bool connection;
        Thread serverThread;
        Client client;
        Server server;
        
        public OnlineForm()
        {
            InitializeComponent();
            
        }

        private void OnlineForm_Load(object sender, EventArgs e)
        {
            createGamePanel.Hide();

            
            client = new Client();
            connection = client.connect();
            if(connection)
            {
                
                client.send("getServers()");
                
            }
           
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/joinGameC.png");
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/joinGame.png");
            panel1.Hide();

            createGamePanel.Show();
            textBox1.ReadOnly = true;
            textBox1.Text = client.readText;
            pictureBox5.Enabled = false;
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/createGameC.png");
        }

        private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/createGame.png");
            
            panel1.Hide();
          
            createGamePanel.Show();
            server = new Server();
            serverThread = new Thread(server.listen);
            serverThread.Start();
            admin = true;
            
        }

        

        private void createGamePanel_Paint(object sender, PaintEventArgs e)
        {

            if (connection)
            {

                

            }

        }

        protected void textBox1_Focus(Object sender, EventArgs e)
        {
            if(textBox1.Text == "Server Name")
            textBox1.Text = "";

           
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !keyPressed)
            {
                if (client.waitingResponse) server.send("kappa");
                client.send("\r\nPlayer2: " + textBox3.Text);
                textBox3.Text = "";
                keyPressed = true;
                e.Handled = true;
                e.SuppressKeyPress = true;
                textBox2.Text = client.chat;
                
            }
        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && keyPressed)
            {
                keyPressed = false;
            }
            
        }

        private void pictureBox5_MouseDown(object sender, MouseEventArgs e) //start game
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/startGameC.png");
        }

        private void pictureBox5_MouseUp(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/startGame.png");
        }

        private void pictureBox4_MouseDown(object sender, MouseEventArgs e) //leave game
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/leaveGameC.png");
        }

        private void OnlineForm_Paint(object sender, PaintEventArgs e)
        {

           


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void pictureBox4_MouseUp(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/leaveGame.png");
        }

        private void pictureBox7_MouseDown(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/refreshC.png");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(server!=null)
            server.changeServerName(textBox1.Text);
            Console.WriteLine("textChanged");
        }

        private void pictureBox7_MouseUp(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = Image.FromFile("Sprites/refresh.png");
            if (connection)
            {

                this.listBox1.Items.Clear();
                this.listBox1.Items.AddRange(new object[] { client.readText });

            }
        }
    }
}
