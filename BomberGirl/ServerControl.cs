using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace BomberGirl
{
    class ServerControl
    {
        public static string serverData = "", clientData = "", serverName = "", chat="Player1 joined the game!\r\n";
        public static Socket sender;
        public static Socket handler;
        public static byte[] bytes;
        public static bool clientConnected = false, listening = false;
        public static int playerNr=1;

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
                listening = true;
                // Start listening for connections.  
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.  
                    handler = listener.Accept();
                    serverData = null;

                    // An incoming connection needs to be processed.  
                    while (true)
                    {
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        serverData += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (serverData.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }
                    byte[] msg = null;
                    // Show the data on the console.  
                    if (serverData == "GetList<EOF>")
                    {
                        
                        msg = Encoding.ASCII.GetBytes(serverName);
                    }
                    else if (serverData == "GetChat<EOF>")
                    {

                        msg = Encoding.ASCII.GetBytes(chat);
                    }
                    else
                    {
                        chat += serverData + "\n";
                    }
                    // Echo the data back to the client.  


                    handler.Send(msg);
                    
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
            bytes = new byte[1024];

            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.  
                sender = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());
                    clientConnected = true;


                    // Receive the response from the remote device.  

                    // Release the socket.  


                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
        }

        public static void closeClient()
        {
            clientConnected = false;
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
        public static void sendMsgClient(String text)
        {
            
            try
            {
                // Encode the data string into a byte array.  
                byte[] msg = Encoding.ASCII.GetBytes(text + "<EOF>");

                // Send the data through the socket.  
                sender.Send(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
        }
        public static String receiveMsgClient()
        {
            try
            {
                int bytesRec = sender.Receive(bytes);
                clientData = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
            return clientData;
        }
        public static void closeServer()
        {
            listening = false;
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
    }
    
}
