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
    class Client
    {
        private const int portNum = 4545;
        public string readText = "";
        TcpClient client;
        NetworkStream ns;
        Thread t = null;
        private const string hostName = "localhost";
        public bool waitingResponse = false;
        public string chat = "ble kaip baisu";

        public Client()
        {

        }
        public bool connect()
        {
            try
            {
                client = new TcpClient(hostName, portNum);
                ns = client.GetStream();
                t = new Thread(DoWork);
                t.Start();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public void DoWork()
        {
            byte[] bytes = new byte[1024];
            while (true)
            {

                int bytesRead = ns.Read(bytes, 0, bytes.Length);
                readText = Encoding.ASCII.GetString(bytes, 0, bytesRead);
                
                if (readText.IndexOf('*')!=-1 && readText.Split('*')[1] == "SVC")
                {
                    readText = readText.Split('*')[0];
                    send("kappa");
                }
                if (readText.IndexOf('*') != -1 && readText.Split('*')[1] == "CHAT")
                {
                    chat = readText;
                    send("kappa");
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
