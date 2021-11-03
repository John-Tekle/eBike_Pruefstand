using System;
using System.Windows;
using Renci.SshNet;

namespace Client_eBike_Pruefstand
{
    public static class SSHPortForwarding
    {
        private static SshClient client;
        private static ForwardedPortLocal port;
        private static string localhost = "127.0.0.1";

        public static void StartPortForwarding()
        {
            try
            {
                ConnectionInfo connectionInfo = new ConnectionInfo(TCP_Client.Default_IPAddress,
                                        "pi",
                                        new PrivateKeyAuthenticationMethod("pi",
                                        new PrivateKeyFile("C:\\Users\\JOHN\\.ssh\\piKey")));

                client = new SshClient(connectionInfo);
                client.Connect();

                port = new ForwardedPortLocal(localhost, TCP_Client.Default_port, 
                    TCP_Client.Default_IPAddress, TCP_Client.Default_port);
                client.AddForwardedPort(port);
                port.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static void StopPortForwarding()
        {
            try
            {
                if (port.IsStarted)
                {
                    port.Stop();
                    port.Dispose();
                }
                if (client.IsConnected)
                {
                    client.Disconnect();
                    client.Dispose();
                }
            }
            catch (Exception) { }
            
        }
    }
}
