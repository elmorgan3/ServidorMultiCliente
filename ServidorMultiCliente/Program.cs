using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace multitaskserver
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener serverSocket = new TcpListener(IPAddress.Any, 5555);
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;
            serverSocket.Start();
            Console.WriteLine(" >> " + "Server iniciat");
            counter = 0;
            try
            {
                while (true)
                {
                    counter += 1;
                    clientSocket = serverSocket.AcceptTcpClient();

                    Console.WriteLine(" >> " + "Client No:" + Convert.ToString(counter) + " connectat");
                    gestioClient client = new gestioClient();
                    client.startClient(clientSocket, Convert.ToString(counter));

                }
            }
            finally
            {
                serverSocket.Stop();
            }
            Console.WriteLine(" >> " + "exit");
            Console.ReadLine();
        }
    }
    //Class gestioClient que gestiona cada client per separat
    public class gestioClient
    {
        TcpClient clientSocket;
        string clNo;
        public void startClient(TcpClient inClientSocket, string clineNo)
        {
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            Task MTCl = new Task(doChat);
            MTCl.Start();
        }
        private void doChat()
        {
            using (NetworkStream n = clientSocket.GetStream())
            {
                string msg = new BinaryReader(n).ReadString();
                BinaryWriter w = new BinaryWriter(n);
                Console.WriteLine(">> Client {0} diu: {1}", this.clNo, msg);
                w.Write("echo " + msg);
                w.Flush();
            }
        }
    }
}