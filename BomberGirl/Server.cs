using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;


namespace BomberGirl
{
    class Server
    {
        delegate void SetTextCallback(string text);
        TcpListener listener;
        TcpClient client;
        NetworkStream ns;
        Thread t = null;
        string serverName = "Server Name";
        public string chat = "Player1 has joined the game!";
        public bool waitingResponse = false;
        public Server()
        {
            

        }
        public void listen()
        {
            listener = new TcpListener(4545);
            listener.Start();
            client = listener.AcceptTcpClient();
            ns = client.GetStream();
            t = new Thread(DoWork);
            t.Start();
        }
        public void DoWork()
        {
            byte[] bytes = new byte[1024];
            send("");
            Console.WriteLine("again");
            while (true)
            {
                int bytesRead = ns.Read(bytes, 0, bytes.Length);
                string textRead = Encoding.ASCII.GetString(bytes, 0, bytesRead);
                Console.WriteLine("\n" + textRead + "gavo");
                if (textRead =="getServers()")
                {
                    send(serverName + "*SVC");
                    Console.WriteLine(serverName);
                }
                else if(textRead != "kappa")
                {
                    chatMsg(textRead);
                    
                }
                waitingResponse = false;
            }
        }
        public void send(string text)
        {
            if (client != null)
            {
                byte[] byteTime = Encoding.ASCII.GetBytes(text);
                ns.Write(byteTime, 0, byteTime.Length);
                waitingResponse = true;
            }
        }
        public void chatMsg(string text)
        {
            chat += text;
            send(chat + "*CHAT");
        }
        public void changeServerName(string s)
        {
            serverName = s;
            send(s + "*SVC");
        }
    }
}
