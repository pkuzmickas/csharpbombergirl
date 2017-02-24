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
        public bool connected = false, gameStarted = false, placeBomb = false;
        public float playerPosX= 0, playerPosY= 0;
        public Client()
        {

        }
        public bool connect(string ip)
        {
            hostName = ip;
            Console.WriteLine("Connecting " + hostName);
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
                Console.WriteLine(readText + " clientas cia ");
                readText = Encoding.ASCII.GetString(bytes, 0, bytesRead);
                if (!startMsg && readText != "bye")
                {
                    t.Suspend();
                    ns.Close();
                    client.Close();
                    connected = false;
                    Console.WriteLine(readText + " clientas cia1 ");
                    return;
                } else if(readText == "bye")
                {
                    connected = true;
                    startMsg = true;
                    Console.WriteLine(readText + " clientas cia 2");
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
                    Console.WriteLine(readText + "SexYNX");
                    send("kappa");
                }
                if (readText.IndexOf('*') != -1 && readText.Split('*')[1] == "PLAYERPOS")
                {
                    readText = readText.Split('*')[0];
                    playerPosX = float.Parse(readText.Split(';')[0]);
                    playerPosY = float.Parse(readText.Split(';')[1]);

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
