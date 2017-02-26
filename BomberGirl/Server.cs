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
    public class Server
    {
        delegate void SetTextCallback(string text);
        TcpListener listener;
        TcpClient client;
        NetworkStream ns;
        public Thread t = null;
        string serverName = "Server Name";
        public string chat = "Player1 has joined the game!";
        public bool waitingResponse = false, placeBomb = false;
        bool firstConnect = false;
        string ip;
        public int numOfPlayers = 1;
        // For gameplay
        public float playerPosX = 0, playerPosY = 0;
        public bool moving_up = false, moving_down = false, moving_right = false, moving_left = false;

        public Server(string ip)
        {
            this.ip = ip;

        }
        public void listen()
        {
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPHostEntry ipHostInfo = Dns.Resolve(ip);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            
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
                
               
                if (textRead =="getServers()")
                {
                    send(serverName + "*SVC");
                }
                else if (textRead == "plantBomb()")
                {
                    placeBomb = true;
                }
                else if (textRead == "addPlayer()")
                {
                    numOfPlayers++;
                    send(numOfPlayers + "*PLAYERNR");
                }
                else if (textRead == "hi" && !firstConnect)
                {
                    send("bye");
                    firstConnect = true;
                }
                else if (textRead.IndexOf('*') != -1 && textRead.Split('*')[1] == "PLAYERPOS")
                {
                    textRead = textRead.Split('*')[0];
                    if (textRead.Split(';')[0] == "down") moving_down = true;
                    else if (textRead.Split(';')[0] == "notdown") moving_down = false;
                    if (textRead.Split(';')[0] == "up") moving_up = true;
                    else if (textRead.Split(';')[0] == "notup") moving_up = false;
                    if (textRead.Split(';')[0] == "right") moving_right = true;
                    else if (textRead.Split(';')[0] == "notright") moving_right = false;
                    if (textRead.Split(';')[0] == "left") moving_left = true;
                    else if (textRead.Split(';')[0] == "notleft") moving_left = false;
                    try
                    {
                        playerPosX = float.Parse(textRead.Split(';')[1]);
                        playerPosY = float.Parse(textRead.Split(';')[2]);
                    } catch(Exception e)
                    {

                    } 


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
