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
using System.Net;

namespace BomberGirl
{
    class Server
    {
        delegate void SetTextCallback(string text);
        TcpListener listener;
        TcpClient client;
        NetworkStream ns;
        public Thread t = null;
        string serverName = "Server Name";
        public string chat = "Player1 has joined the game!";
        public bool waitingResponse = false;
        bool firstConnect = false;
        public Server()
        {
            

        }
        public void listen()
        {
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[2];
            Console.WriteLine(ipAddress.ToString());
            listener = new TcpListener(ipAddress, 443);
            
            listener.Start();
            client = listener.AcceptTcpClient();
            ns = client.GetStream();
            
            t = new Thread(DoWork);
            t.Start();
            
        }
        public void DoWork()
        {
            byte[] bytes = new byte[1024];
            
            
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
                else if (textRead == "hi" && !firstConnect)
                {
                    send("bye");
                    firstConnect = true;
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
        public void close()
        {
            if(ns!=null)
            ns.Close();
            if(client!=null)
            client.Close();
            if (t != null) t.Suspend();
            //listener.EndAcceptTcpClient();
        }
    }
}
