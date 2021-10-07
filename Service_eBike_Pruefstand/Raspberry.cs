using System;
using System.Collections.Generic;

namespace Service_eBike_Pruefstand
{
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
                TCPServer.CommandReceived += TCPServer_CommandReceived;
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

        }

        private void TCPServer_CommandReceived(object sender, Common_eBike_Pruefstand.TCPEventArgs e)
        {
            log.Log(NLog.LogLevel.Trace,e.Command);
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
                if (this.Temperatur.Read(Temperatur.Channel, out this.temperatur)) _return = true; else return false;
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
            throw new NotImplementedException("Temperatur");
        }

        private void Anemometer_SpeedChanged(object sender, Common_eBike_Pruefstand.AnemometerEventArgs e)
        {
            throw new NotImplementedException("Anemometer");
        }

        private void Gewicht_LoadChanged(object sender, Common_eBike_Pruefstand.GewichtEventArgs e)
        {
            throw new NotImplementedException("Gewicht");
        }

        private void Luefter_ValueChanged(object sender, Common_eBike_Pruefstand.LuefterEventArgs e)
        {
            throw new NotImplementedException("Luefter");
        }
        #endregion
    }
}
