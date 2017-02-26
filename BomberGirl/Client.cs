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
    public class Client
    {
        private const int portNum = 443;
        public string readText = "";
        TcpClient client;
        NetworkStream ns;
        public Thread t = null;
        private string hostName = "172.19.5.51";
        public bool waitingResponse = false;
        public string chat = "ble kaip baisu";
        public string serverName = "Server Name";
        public int numOfPlayers = 1, myPlayerID = 2;
        // GAMEPLAY
        public bool connected = false, gameStarted = false, placeBomb = false;
        public float playerPosX= 0, playerPosY= 0;
        public bool moving_up = false, moving_down = false, moving_right = false, moving_left = false;

        public Client()
        {

        }
        public bool connect(string ip)
        {
            hostName = ip;
            try
            {
                client = new TcpClient(hostName, portNum);
            
            ns = client.GetStream();
            t = new Thread(DoWork);
            t.Start();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;

        }
        public void DoWork()
        {
            bool startMsg = false;
            byte[] bytes = new byte[1024];
            send("hi");
            while (true)
            {

                int bytesRead = ns.Read(bytes, 0, bytes.Length);
                readText = Encoding.ASCII.GetString(bytes, 0, bytesRead);
                if (!startMsg && readText != "bye")
                {
                    t.Suspend();
                    ns.Close();
                    client.Close();
                    connected = false;
                    return;
                } else if(readText == "bye")
                {
                    connected = true;
                    startMsg = true;
                }
               
                if (readText == "connected")
                {
                    readText = "";
                }
                if (readText.IndexOf('*') != -1 && readText.Split('*')[1] == "SVC")
                {
                    readText = readText.Split('*')[0];
                    serverName = readText;
                    send("kappa");
                }
                if (readText.IndexOf('*') != -1 && readText.Split('*')[1] == "CHAT")
                {
                    readText = readText.Split('*')[0];
                    chat = readText;
                    send("kappa");
                }
                if (readText.IndexOf('*') != -1 && readText.Split('*')[1] == "PLAYERNR")
                {
                    readText = readText.Split('*')[0];
                    numOfPlayers = Int32.Parse(readText);
                    send("kappa");
                }
                if (readText.IndexOf('*') != -1 && readText.Split('*')[1] == "PLAYERPOS")
                {
                    readText = readText.Split('*')[0];
                    if (readText.Split(';')[0] == "down") moving_down = true;
                    else if (readText.Split(';')[0] == "notdown") moving_down = false;
                    if (readText.Split(';')[0] == "up") moving_up = true;
                    else if (readText.Split(';')[0] == "notup") moving_up = false;
                    if (readText.Split(';')[0] == "right") moving_right = true;
                    else if (readText.Split(';')[0] == "notright") moving_right = false;
                    if (readText.Split(';')[0] == "left") moving_left = true;
                    else if (readText.Split(';')[0] == "notleft") moving_left = false;
                    try
                    {
                        playerPosX = float.Parse(readText.Split(';')[1]);
                        playerPosY = float.Parse(readText.Split(';')[2]);
                    } catch(Exception e)
                    {

                    }

                }
                if (readText=="startGame()")
                {
                    gameStarted = true;
                }
                if (readText == "plantBomb()")
                {
                    placeBomb = true;
                }

                waitingResponse = false;
            }
        }
        public void send(string text)
        {

            byte[] byteTime = Encoding.ASCII.GetBytes(text);
            ns.Write(byteTime, 0, byteTime.Length);

            waitingResponse = true;
        }

    }
}
