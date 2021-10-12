using System;
using System.Windows;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Common_eBike_Pruefstand;

namespace Client_eBike_Pruefstand
{
    public delegate void Notify(string s);  // delegate
    public static class TCP_Client
    {
        private static IPAddress IPAddress;
        public const string Default_IPAddress = "192.168.61.27";
        public const Int32 Default_port = 13000;
        private static IPEndPoint IPEndPoint;
        private static TcpClient client;
        private static NetworkStream networkStream;
        public static event EventHandler<TCPEventArgs> CommandReceived;
        public static event Notify ClientStatusUpdate; // event
        private static string ClientConnected = "Connected";
        private static string ClientDisconnected = "Disconnected";

        /// <returns>
        /// true if the System.Net.Sockets.TcpClient.Client socket was connected to a remote resource as of the most recent operation; otherwise, false.
        /// </returns> 
        public static bool Connected => client.Connected;

        public static void Initialization(string ipaddress, Int32 port)
        {
            try
            {
                IPAddress = IPAddress.Parse(ipaddress);
                IPEndPoint = new IPEndPoint(IPAddress, port);
                client = new TcpClient();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private static void Run()
        {
            Thread.Sleep(4500);
            bool stop_thread = true;
            while (stop_thread)
            {
                string receivedCommand;
                try
                {
                    receivedCommand = ReceiveCommand();
                    if (string.IsNullOrEmpty(receivedCommand))
                        throw new Exception("It was either disconnected the server or received zero value command");
                    OnCommandReceived(receivedCommand);
                }
                catch (Exception e)
                {
                    if (client.Connected)
                    {
                        _ = MessageBox.Show(e.Message);
                    }
                    //Trying to find out if server is disconnected
                    ClientStatusUpdate?.Invoke(ClientDisconnected);
                    Close();
                    stop_thread = false;
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

                    Thread t = new Thread(Run)
                    {
                        IsBackground = true
                    };
                    t.Start();
                    ClientStatusUpdate?.Invoke(ClientConnected);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        public static void Close()
        {
            if(client.Connected)
            {
                networkStream.Close();
                networkStream.Dispose();
                client.Close();
                client.Dispose();
            }
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
