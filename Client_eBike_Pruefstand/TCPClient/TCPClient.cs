using System;
using System.Windows;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Common_eBike_Pruefstand;

namespace Client_eBike_Pruefstand
{
    public static class TCP_Client
    {
        private static IPAddress IPAddress;
        public const string Default_IPAddress = "192.168.61.27";
        public const Int32 Default_port = 13000;
        private static IPEndPoint IPEndPoint;
        private static TcpClient client;
        private static NetworkStream networkStream;
        public static event EventHandler<TCPEventArgs> CommandReceived;

        public static void Initialization(string ipaddress, Int32 port)
        {
            try
            {
                IPAddress = IPAddress.Parse(ipaddress);
                IPEndPoint = new IPEndPoint(IPAddress, port);
                client = new TcpClient();

                Thread t = new Thread(Run);
                t.IsBackground = true;
                t.Start();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private static void Run()
        {
            while (true)
            {
                Thread.Sleep(4500);
                try
                {
                    Connect();
                    while (true)
                    {
                        ReceiveCommand();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public static void Initialization()
        {
            Initialization(Default_IPAddress,Default_port);
        }
        
        public static void Connect()
        {
            if (!client.Connected)
            {
                try
                {
                    client.Connect(IPEndPoint);
                    networkStream = client.GetStream();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        public static void Close()
        {
            networkStream.Close();
            networkStream.Dispose();
            client.Close();
            client.Dispose();
        }

        public static string ReceiveCommand()
        {
            try
            {
                StreamReader streamReader = new StreamReader(networkStream);
                return streamReader.ReadLine() ;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void OnCommandReceived(string command)
        {
            CommandReceived?.Invoke(null, new TCPEventArgs(command));
        }

        public static void ReceiveFile()
        {
            throw new NotImplementedException();
        }

        public static void SendCommand(string s)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(networkStream);
                streamWriter.AutoFlush = true;
                streamWriter.WriteLine(s);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }

        public static void SendFile()
        {
            throw new NotImplementedException();
        }
    }
}
