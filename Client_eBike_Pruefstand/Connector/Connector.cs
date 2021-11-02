using Common_eBike_Pruefstand;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Client_eBike_Pruefstand
{
    public delegate void EllipseUpdate_Notify(KeyValuePair<string, object> keyValuePair);
    public delegate void ClientStatusUpdate_Notify(string s);
    public static class Connector
    {
        //private static bool keyValuePair;
        public static event EllipseUpdate_Notify EllipseUpdater; // event
        public static event ClientStatusUpdate_Notify ClientStatusUpdater; // event
        public static Dictionary<string, object> KeyValuePairsCommmand { get; private set; }

        public static void Initialization()
        {
            TCP_Client.CommandReceived += TCP_Client_CommandReceived;
        }

        private static void TCP_Client_CommandReceived(object sender, TCPEventArgs e)
        {
            KeyValuePairsCommmand = null;
            try
            {
                KeyValuePairsCommmand = JsonSerializer.Deserialize<Dictionary<string, object>>(e.Command);
                foreach (KeyValuePair<string, object> keyValuePair in KeyValuePairsCommmand)
                {
                    if (keyValuePair.Key.Substring(0, 6) == "Logger")
                    {
                        EllipseUpdater?.Invoke(keyValuePair);
                    }

                    if (keyValuePair.Key.Substring(0, 5) == "Value")
                    {
                        switch (keyValuePair.Key)
                        {
                            case "Value+Temperatur":
                                Temperatur.Update(keyValuePair.Value);
                                break;
                            case "Value+Gewicht":
                                Gewicht.Update(keyValuePair.Value);
                                break;
                            case "Value+Anemometer":
                                Anemometer.Update(keyValuePair.Value);
                                //using (Process myProcess = new Process())
                                //{
                                //    myProcess.StartInfo.FileName = "scp.exe";
                                //    myProcess.StartInfo.Arguments = "pi-eth:/home/pi/Documents/1GB.bin c:\\temp";
                                //    myProcess.StartInfo.UseShellExecute = false;
                                //    myProcess.StartInfo.RedirectStandardOutput = false;
                                //    myProcess.Start();
                                //}
                                //ProcessStartInfo psi = new ProcessStartInfo
                                //{
                                //    FileName = @"C:\\Windows\\System32\\OpenSSH\\scp.exe",
                                //    WindowStyle = ProcessWindowStyle.Hidden,
                                //    UseShellExecute = true,
                                //    CreateNoWindow = true,
                                //   // WorkingDirectory = @"C:\\Windows\\System32\\OpenSSH\\scp.exe",
                                //    Arguments = "/c pi-eth:/home/pi/Documents/1GB.bin c:\\temp"
                                //};
                                //_ = Process.Start(psi);
                                break;
                            case "Value+Luefter":
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.Message);
            }
        }

        public static void SendLogCommand(string name, bool value)
        {
            Dictionary<string, bool> keyValuePairs = new Dictionary<string, bool>
            {
                { $"Logger+{name}: ", value }
            };
            TCP_Client.SendCommand(JsonSerializer.Serialize(keyValuePairs));
        }

        public static void SendHeaderCommand(string name, string value)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
            {
                { name, value }
            };
            TCP_Client.SendCommand(JsonSerializer.Serialize(keyValuePairs));
        }

        public static void OnClientStatusUpdate(string ClientStatus)
        {
            ClientStatusUpdater?.Invoke(ClientStatus);
        }
    }
}
