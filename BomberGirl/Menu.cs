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
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace BomberGirl
{
    /*
     * The window for the main menu
     */ 
    public partial class Menu : Form
    {
        // The sound player variable
        System.Media.SoundPlayer sound;
        public static string data = null;
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

        private void pictureBox6_MouseDown(object sender, MouseEventArgs e)
        {
            Thread t = new Thread(StartListening);
            t.Start();
            
        }

        public static void StartListening()
        {
            // Data buffer for incoming data.  
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.  
            // Dns.GetHostName returns the name of the   
            // host running the application.  
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and   
            // listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.  
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.  
                    Socket handler = listener.Accept();
                    data = null;

                    // An incoming connection needs to be processed.  
                    while (true)
                    {
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }

                    // Show the data on the console.  
                    Console.WriteLine("Text received : {0}", data);

                    // Echo the data back to the client.  
                    byte[] msg = Encoding.ASCII.GetBytes(data);

                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        public static void StartClient()
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.  
                Socket sender = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.  
                    byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

                    // Send the data through the socket.  
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.  
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Echoed test = {0}",
                        Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    // Release the socket.  
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            StartClient();
        }
    }
}
