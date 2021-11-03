using System;
using System.IO;
using System.Windows;
using Renci.SshNet;

namespace Client_eBike_Pruefstand
{
    class SecureDownload: IDisposable
    {
        private SftpClient client;
        private ConnectionInfo connectionInfo;
        private bool disposedValue;

        public string Path { get; private set; }
        public SecureDownload(string path)
        {
            Path = path;
            try
            {
                connectionInfo = new ConnectionInfo(TCP_Client.Default_IPAddress,"pi",
                                        new PrivateKeyAuthenticationMethod("pi",
                                        new PrivateKeyFile("C:\\Users\\JOHN\\.ssh\\piKey")));
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }
        public void DownloadFile()
        {
            try
            {
                client = new SftpClient(connectionInfo);
                client.Connect();
                string path = @$"c:\Pi-Logging\{Path.TrimStart("/home/pi/Documents/".ToCharArray())}";
                path = path.Replace('/','\\');
                using FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                client.DownloadFile(Path, fileStream);
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
            finally { client.Disconnect(); }

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    client.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SecureDownload()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
