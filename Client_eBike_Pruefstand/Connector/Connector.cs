using Common_eBike_Pruefstand;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Client_eBike_Pruefstand
{
    public delegate void EllipseUpdate_Notify(KeyValuePair<string, object> keyValuePair);
    public delegate void ClientStatusUpdate_Notify(string s);
    public static class Connector
    {
        private static KeyValuePair<string, object> key_ValuePair;
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
                                break;
                            case "Value+Luefter":
                                break;
                        }
                    }

                    if (keyValuePair.Key.Substring(0, 4) == "Path")
                    {
                        key_ValuePair = keyValuePair;
                        Thread thread = new(DownloadTask) //Using Thread files can be downloaded parallel 
                        {
                            IsBackground = false
                        };
                        thread.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.Message);
            }
        }
       
        private static void DownloadTask()
        {
            using SecureDownload secureDownload = new(key_ValuePair.Value.ToString());
            secureDownload.DownloadFile();
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
