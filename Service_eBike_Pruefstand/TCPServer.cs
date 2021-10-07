using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using Common_eBike_Pruefstand;

namespace Service_eBike_Pruefstand
{
    public static class TCPServer
    {
        public const Int32 Default_port = 13000;
        private static IPEndPoint IPEndPoint;
        private static TcpListener server;
        private static TcpClient client;
        private static NetworkStream networkStream;
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        public static event EventHandler<TCPEventArgs> CommandReceived;

        public static void Initialization(Int32 port)
        {
            try
            {
                IPEndPoint = new IPEndPoint(IPAddress.Any, port);
                server = new TcpListener(IPEndPoint);
                server.Start();

                Thread t = new Thread(Run);
                t.IsBackground = true;
                t.Start();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void Initialization()
        {
            Initialization(Default_port);
        }

        private static void Run()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(5000);
                    server.Start();
                    log.Debug("Starts listening for incoming connection requests.");
                    client = server.AcceptTcpClient();
                    log.Info(client.Client.RemoteEndPoint);
                    networkStream = client.GetStream();
                    string receivedCommand;
                    while (true)
                    {
                        receivedCommand = ReceiveCommand();
                        if (string.IsNullOrEmpty(receivedCommand))
                            throw new Exception("It was either disconnected the client or received zero value command");
                        OnCommandReceived(receivedCommand);
                    }
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                }
                finally
                {
                    networkStream.Close();
                    client.Close();
                    server.Stop();
                }
            }
        }

        public static string ReceiveCommand()
        {
            StreamReader streamReader = new StreamReader(networkStream);
            return streamReader.ReadLine();
        }

        public static void OnCommandReceived(string command)
        {
            CommandReceived?.Invoke(null ,new TCPEventArgs(command));
        }

        public static void ReceiveFile()
        {
            throw new NotImplementedException();
        }

        public static void SendCommand(string s)
        {
            StreamWriter streamWriter = new StreamWriter(networkStream);
            //streamWriter.AutoFlush = true;
            try
            {
                streamWriter.WriteLine(s);
                streamWriter.Flush();
            }
            catch(Exception e)
            {
                streamWriter.Close();
                streamWriter.Dispose();
                throw new Exception(e.Message);
            }
        }

        public static void SendFile()
        {
            throw new NotImplementedException();
        }
    }
}
