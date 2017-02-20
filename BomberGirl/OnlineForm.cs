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
using System.Net;


namespace BomberGirl
{
    public partial class OnlineForm : Form
    {
        Image refreshImage = Image.FromFile("Sprites/refresh.png");
        Image refreshImageClicked = Image.FromFile("Sprites/refreshC.png");
        delegate void SetTextCallback();
        bool keyPressed = false, admin = false;
        Thread serverThread;
        Thread chatThread;
        Client client;
        Server server;
        Form menu;
        string ip;
        public OnlineForm(Form menu)
        {
            InitializeComponent();
            this.menu = menu;
        }

        private void OnlineForm_Load(object sender, EventArgs e)
        {
            createGamePanel.Hide();


            client = new Client();



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
            if (client.connected)
            {
                panel1.Hide();

                client.send("\r\nPlayer2 has joined the game!");
                label3.Text = "2";
                createGamePanel.Show();
                textBox1.ReadOnly = true;
                textBox1.Text = client.serverName;
                pictureBox5.Enabled = false;
                textBox2.Text = client.chat;
                chatThread = new Thread(loadChat);
                chatThread.Start();
            }
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
            server = new Server(ip);
            serverThread = new Thread(server.listen);
            serverThread.Start();

            chatThread = new Thread(loadChat);
            chatThread.Start();
            admin = true;

        }

        public void loadChat()
        {

            while (true)
            {
                SetText();
            }
        }

        private void SetText()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox2.InvokeRequired)
            {
                try
                {
                    SetTextCallback d = new SetTextCallback(SetText);
                    this.Invoke(d);
                }
                catch (Exception e)
                {
                    Console.WriteLine("session ended");
                }
            }
            else
            {
                if (server != null)
                {

                    textBox2.Text = server.chat;
                }
                else if (client != null)
                {
                    textBox2.Text = client.chat;
                    textBox1.Text = client.serverName;

                }
            }
        }

        private void createGamePanel_Paint(object sender, PaintEventArgs e)
        {



        }

        protected void textBox1_Focus(Object sender, EventArgs e)
        {
            if (textBox1.Text == "Server Name")
                textBox1.Text = "";


        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !keyPressed)
            {
                if (client.connected)
                {
                    client.send("\r\nPlayer2: " + textBox3.Text);

                }
                else if (server != null)
                {
                    server.chatMsg("\r\nPlayer1: " + textBox3.Text);
                }
                textBox3.Text = "";
                keyPressed = true;
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (client != null)
                {
                    textBox2.Text = client.chat;
                }
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
            this.Hide();
            new Form1(menu, Int32.Parse(label3.Text), true);
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
            //serverThread.Suspend();
            //chatThread.Suspend();
            //server.close();
            //if (client.t != null)
            //{
            //    client.t.Suspend();
            //}
            //Application.Exit();
            System.Environment.Exit(0);
        }

        private void pictureBox7_MouseDown(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = refreshImageClicked;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (server != null)
            {
                server.changeServerName(textBox1.Text);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void OnlineForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {


            ip = textBox4.Text;
            
            if (ip=="" || !client.connect(ip))
            {
                MessageBox.Show("Couldn't connect!");

            }
            else
            {
                panel2.Hide();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[2];
            ip = ipAddress.ToString();
            if (ip == "" || !client.connect(ip))
            {
                MessageBox.Show("Couldn't connect!");

            }
            else
            {
                panel2.Hide();
            }
        }

        private void pictureBox7_MouseUp(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = refreshImage;
            if (client.connected)
            {
                this.listBox1.Items.Clear();
                this.listBox1.Items.Add(client.serverName);

            }
        }
    }
}
