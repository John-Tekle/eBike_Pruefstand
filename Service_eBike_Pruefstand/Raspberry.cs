using System;
using System.Text.Json;
using System.Collections.Generic;

namespace Service_eBike_Pruefstand
{
    public delegate void Notify(object sender, Dictionary<string, bool> keyValuePairs);  // delegate

    public class Raspberry
    {
        #region members
        public Anemometer Anemometer { get; }
        public Gewicht Gewicht { get; }
        public Luefter Luefter { get; }
        public Temperatur Temperatur { get; }

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        private float anemometer = (float)99.99;//float.MaxValue;
        private float gewicht = (float)99.99;//float.MaxValue;
        private byte luefter = 99;//float.MaxValue;
        private float temperatur = (float)99.99;//float.MaxValue;

        public Dictionary<string, bool> KeyValuePairsCommmand { get; private set; }
        public Dictionary<string, bool> keyValuePairsUpdate = new Dictionary<string, bool>
            {
                { "Temperature" ,false },
                { "Gewicht" ,false },
                { "Anemometer" ,false },
                { "Luefter" ,false },
            };

    public event Notify CommandToGUI; // event
        #endregion

        #region constructor & destructor
        public Raspberry()
        {
            try
            {
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
            foreach(KeyValuePair<string, bool> keyValuePairs in keyValuePairsUpdate)
            {
                SendCommand(keyValuePairs.Key, keyValuePairs.Value);
            }
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
                if (this.Temperatur.Read(ADC_MAX11617.Channel.TemperaturDefault, out this.temperatur)) _return = true; else return false;
                if (this.Gewicht.Read(Gewicht.Channel, out this.gewicht)) _return = true; else return false;
                if (this.Anemometer.Read(Anemometer.Channel, out this.anemometer)) _return = true; else return false;
                if (this.Luefter.Read(out luefter)) _return = true; else return false;

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

        private void UpdateData(float anemometer, float gewicht, byte luefter, float temperature)
        {
            if (temperature != Temperatur.TemperatureValue)
            {
                Temperatur.TemperatureValue = temperatur;
                Temperatur.OnTemperatureChanged(temperature);
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
            SendCommandToClient(e.Name, e.Temperature);
        }

        private void Anemometer_SpeedChanged(object sender, Common_eBike_Pruefstand.AnemometerEventArgs e)
        {
            SendCommandToClient(e.Name, e.Speed);
        }

        private void Gewicht_LoadChanged(object sender, Common_eBike_Pruefstand.GewichtEventArgs e)
        {
            SendCommandToClient(e.Name, e.Load);
        }

        private void Luefter_ValueChanged(object sender, Common_eBike_Pruefstand.LuefterEventArgs e)
        {
            SendCommandToClient(e.Name, e.Value);
        }

        private void SendCommandToClient(string name, float value)
        {
            Dictionary<string, float> keyValuePairs = new Dictionary<string, float>
            {
                { name, value }
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
        private void TCPServer_CommandReceived(object sender, Common_eBike_Pruefstand.TCPEventArgs e)
        {
            KeyValuePairsCommmand = null;
            try
            {
                KeyValuePairsCommmand = JsonSerializer.Deserialize<Dictionary<string, bool>>(e.Command);
                foreach (KeyValuePair<string, bool> res in KeyValuePairsCommmand)
                {
                    if(res.Key.Substring(0, 6) == "Logger")
                    {
                        switch (res.Key.Substring(0, (int)(res.Key.Length - 2)))
                        {
                            case "Logger+Temperature":
                                OnCommandToGUI(sender, KeyValuePairsCommmand);
                                break;
                            case "Logger+Gewicht":
                                OnCommandToGUI(sender, KeyValuePairsCommmand);
                                break;
                            case "Logger+Anemometer":
                                OnCommandToGUI(sender, KeyValuePairsCommmand);
                                break;
                            case "Logger+Luefter":
                                OnCommandToGUI(sender, KeyValuePairsCommmand);
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
        private void OnCommandToGUI(object sender, Dictionary<string, bool> keyValuePairsCommmand)
        {
            CommandToGUI?.Invoke(sender, keyValuePairsCommmand);
        }
        #endregion
    }
}
