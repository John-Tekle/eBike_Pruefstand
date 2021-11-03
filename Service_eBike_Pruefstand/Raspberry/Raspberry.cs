using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace Service_eBike_Pruefstand
{
    public delegate void Notify(object sender, Dictionary<string, object> keyValuePairs);  // delegate

    public class Raspberry
    {
        #region members
        public Anemometer Anemometer { get; }
        public Gewicht Gewicht { get; }
        public Luefter Luefter { get; }
        public Temperatur Temperatur { get; }

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private static readonly NLog.Logger logTemperatur = NLog.LogManager.GetLogger("Temperatur_Log");

        private float anemometer = (float)99.99;//float.MaxValue;
        private float gewicht = (float)99.99;//float.MaxValue;
        private byte luefter = 99;//float.MaxValue;
        private float temperatur = (float)99.99;//float.MaxValue;

        public Dictionary<string, object> KeyValuePairsCommmand { get; private set; }
        public Dictionary<string, bool> keyValuePairsUpdate = new Dictionary<string, bool>
            {
                { "Temperature" ,false },
                { "Gewicht" ,false },
                { "Anemometer" ,false },
                { "Luefter" ,false },
            };
        private static ADC_MAX11617.Channel tempChannel;

        public event Notify CommandToGUI; // event
        Logger loggerTemperatur;
        Logger loggerGewicht;
        Logger loggerAnemometer;
        Logger loggerLuefter;
        #endregion

        #region constructor & destructor
        public Raspberry()
        {
            try
            {
                tempChannel = ADC_MAX11617.Channel.TemperaturDefault;
                Anemometer = new Anemometer(ADC_MAX11617.Address.Anemometer, ADC_MAX11617.Channel.Anemometer);
                Gewicht = new Gewicht(ADC_MAX11617.Address.Gewicht, ADC_MAX11617.Channel.Gewicht);
                Temperatur = new Temperatur(ADC_MAX11617.Address.Temperatur, ADC_MAX11617.Channel.TemperaturDefault);
                Luefter = new Luefter(I2C_Address.Address.Luefter);
                TCPServer.Initialization();
            }
            catch(Exception e)
            {
                log.Error(e.Message);
                throw e;
            }
            //EventHandler
            Anemometer.SpeedChanged += Anemometer_SpeedChanged;
            Gewicht.LoadChanged += Gewicht_LoadChanged;
            Luefter.ValueChanged += Luefter_ValueChanged;
            Temperatur.TemperatureChanged += Temperatur_TemperatureChanged;

            TCPServer.CommandReceived += TCPServer_CommandReceived;
            TCPServer.NotifyOnAcceptedTcpClient += TCPServer_NotifyOnAcceptedTcpClient;
        }

        private void TCPServer_NotifyOnAcceptedTcpClient()
        {
            for (int i = 0; i < 3; i++) //Send Ellipse state two times to be sure that client has all received 
                foreach (KeyValuePair<string, bool> keyValuePairs in keyValuePairsUpdate)
                    if (keyValuePairs.Key.Equals("Temperature") || keyValuePairs.Value) //Exception for the temperature
                        SendCommand(keyValuePairs.Key, keyValuePairs.Value);
        }
        #endregion

        #region properties
        public float GetAnValue { get { return this.anemometer; } }
        public float GetGeValue { get { return this.gewicht; } }
        public byte   GetLuValue { get { return this.luefter; } }
        public float GetTeValue { get { return this.temperatur; } }
        #endregion


        #region Methodes
        public bool Run()
        {
            bool _return = false;
            try
            {
                if (this.Temperatur.Read(tempChannel, out this.temperatur)) _return = true; else return false;
                if (this.Gewicht.Read(Gewicht.Channel, out this.gewicht)) _return = true; else return false;
                if (this.Anemometer.Read(Anemometer.Channel, out this.anemometer)) _return = true; else return false;
                //if (this.Luefter.Read(out luefter)) _return = true; else return false;

                #region Log every one secound
                {
                    if (Logger.logState[0])
                        loggerTemperatur.Log(GetTeValue.ToString("00.00"));
                    if (Logger.logState[1])
                        loggerGewicht.Log(GetGeValue.ToString("00.00"));
                    if (Logger.logState[2])
                        loggerAnemometer.Log(GetAnValue.ToString("00.00"));
                    if (Logger.logState[3])
                        loggerLuefter.Log(GetLuValue.ToString("00.00"));
                }
                #endregion

                if (_return)
                    UpdateData(GetAnValue, GetGeValue, GetLuValue, GetTeValue);
            }
            catch(Exception e)
            {
                log.Error(e.Message);
                throw e;
            }

            return _return;
        }

        private void UpdateData(float anemometer, float gewicht, byte luefter, float temperatur)
        {
            if (temperatur != Temperatur.TemperatureValue)
            {
                Temperatur.TemperatureValue = temperatur;
                Temperatur.OnTemperatureChanged(temperatur);
            }
            if (gewicht != Gewicht.Load)
            {
                Gewicht.Load = gewicht;
                Gewicht.OnLoadChanged(gewicht);
            }
            if (anemometer != Anemometer.Speed)
            {
                Anemometer.Speed = anemometer;
                Anemometer.OnSpeedChanged(anemometer);
            }
            if (luefter != Luefter.Value)
            {
                Luefter.Value = luefter;
                Luefter.OnValueChanged(luefter);
            }
        }

        private void Temperatur_TemperatureChanged(object sender, Common_eBike_Pruefstand.TemperaturEventArgs e)
        {
            SendCommand(e.Name, e.Temperature);
        }

        private void Anemometer_SpeedChanged(object sender, Common_eBike_Pruefstand.AnemometerEventArgs e)
        {
            SendCommand(e.Name, e.Speed);
        }

        private void Gewicht_LoadChanged(object sender, Common_eBike_Pruefstand.GewichtEventArgs e)
        {
            SendCommand(e.Name, e.Load);
        }

        private void Luefter_ValueChanged(object sender, Common_eBike_Pruefstand.LuefterEventArgs e)
        {
            //SendCommand(e.Name, e.Value);
        }

        private void SendCommand(string name, float value)
        {
            Dictionary<string, float> keyValuePairs = new Dictionary<string, float>
                {
                    { $"Value+{name}", value }
                };
            TCPServer.SendCommand(JsonSerializer.Serialize(keyValuePairs));
        }
        public void SendCommand(string name, bool value)
        {
            Dictionary<string, bool> keyValuePairs = new Dictionary<string, bool>
            {
                { $"Logger+{name}+ACK: ", value }
            };
            TCPServer.SendCommand(JsonSerializer.Serialize(keyValuePairs));
        }
        private void SendCommand(string commandName, string path)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                {
                    { $"Path+{commandName}", path }
                };
            TCPServer.SendCommand(JsonSerializer.Serialize(keyValuePairs));
        }
        private void TCPServer_CommandReceived(object sender, Common_eBike_Pruefstand.TCPEventArgs e)
        {
            KeyValuePairsCommmand = null;
            try
            {
                KeyValuePairsCommmand = JsonSerializer.Deserialize<Dictionary<string, object>>(e.Command);
                foreach (KeyValuePair<string, object> res in KeyValuePairsCommmand)
                {
                    if(res.Key.Substring(0, 6) == "Logger")
                    {
                        OnCommandToGUI(sender, KeyValuePairsCommmand);
                        bool logStateValue = bool.Parse(res.Value.ToString());
                        switch (res.Key.Substring(0, (int)(res.Key.Length - 2)))
                        {
                            case "Logger+Temperature":
                                LogStartStop("Temperatur", ref loggerTemperatur, logStateValue, 0);
                                break;
                            case "Logger+Gewicht":
                                LogStartStop("Gewicht", ref loggerGewicht, logStateValue, 1);
                                break;
                            case "Logger+Anemometer":
                                LogStartStop("Anemometer", ref loggerAnemometer, logStateValue, 2);
                                break;
                            case "Logger+Luefter":
                                LogStartStop("Luefter", ref loggerLuefter, logStateValue, 3);
                                break;
                            default:
                                log.Error($"The given key '{res.Key}' could not processed.");
                                break;
                        }
                    }
                    else if (res.Key.Substring(0, 10) == "Temperatur")
                    {
                        OnCommandToGUI(sender, KeyValuePairsCommmand);
                        switch (res.Key.Substring(0, res.Key.Length))
                        {
                            case "Temperatur0":
                                tempChannel = ADC_MAX11617.Channel.Temperatur0;
                                break;
                            case "Temperatur1":
                                tempChannel = ADC_MAX11617.Channel.Temperatur1;
                                break;
                            case "Temperatur2":
                                tempChannel = ADC_MAX11617.Channel.Temperatur2;
                                break;
                            case "Temperatur3":
                                tempChannel = ADC_MAX11617.Channel.Temperatur3;
                                break;
                            case "Temperatur4":
                                tempChannel = ADC_MAX11617.Channel.Temperatur4;
                                break;
                            case "Temperatur5":
                                tempChannel = ADC_MAX11617.Channel.Temperatur5;
                                break;
                            default:
                                log.Error($"The given key '{res.Key}' could not processed.");
                                break;
                        }
                    }

                }
            }
            catch (Exception ex) 
            { 
                log.Error(ex.Message); 
            }
        }
        //Updating GUI Value
        private void OnCommandToGUI(object sender, Dictionary<string, object> keyValuePairsCommmand)
        {
            CommandToGUI?.Invoke(sender, keyValuePairsCommmand);
        }

        private void LogStartStop(string name, ref Logger logger, bool state, int i)
        {
            if (!(i >= 0 || i <= 3))
            {
                log.Error("The indexer must be greater than 0 and less than 3.");
                return;
            }
            if (state)
            {
                logger = new Logger(name);
                Logger.logState[i] = state;
            }
            else
            {
                Logger.logState[i] = state;
                SendCommand(name, logger.Path);
                logger = null;
            }
        }
        #endregion
    }
}
