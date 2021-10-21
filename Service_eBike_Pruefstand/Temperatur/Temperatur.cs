using System;
using System.Threading;
using System.Windows.Forms;
using Common_eBike_Pruefstand;

namespace Service_eBike_Pruefstand
{
    public class Temperatur: AnalogToDigital, ITemperatur
    {

        #region members
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        public event EventHandler<TemperaturEventArgs> TemperatureChanged;
        #endregion

        #region constructor & destructor
        public Temperatur(ADC_MAX11617.Address addr, ADC_MAX11617.Channel chan) : base(addr)
        {
            if ((byte)chan >= 0 && (byte)chan <= 5)
                base.Channel = (byte)chan;
            else
                throw new ArgumentOutOfRangeException($"{ nameof(Temperatur) }: value of Channel is out of the range, must be between 5..10.Current value is { (byte)chan }");
        }
        #endregion

        #region properties
        public float TemperatureValue { get; set; }
        public new byte Channel 
        { 
            get { return base.Channel; }
            set { base.Channel = value; }
        }
        #endregion

        #region methodes
        public bool Read(ADC_MAX11617.Channel _chan, out float temperatur)
        {
            byte chan = (byte)_chan;
            byte[] data = new byte[2];
            int length = 0;

            if (chan >= 0 && chan <= 5)
            {
                if (WriteConfigByte(chan))
                {
                    //Thread.Sleep(1);
                    if (!ReadResult(out data, out length))
                    {
                        log.Error("Reading data from MAX11617, Channel to read: " + chan);
                        temperatur = float.MaxValue;
                        return false;
                    }
                    UInt16 sensorValue = (UInt16)(((data[0] << 8) | data[1]) & 0b0000111111111111);
                    double sensorVoltage = (double)(3.2864 / 4095.0) * sensorValue;
                    temperatur = (float)((sensorVoltage - 1.2437) / 0.005);
                    if (chan != 0) temperatur -= (float)2.0; //Offset for channels 1..5
                    temperatur = (float)Math.Round(temperatur, 2);
                    return true;
                }
                else
                    log.Error("Writing command to MAX11617, Channel to read: " + chan);

                temperatur = float.MaxValue;
                return false;
            }
            else
                throw new ArgumentOutOfRangeException($"{ nameof(Temperatur) }: value of Channel is out of the range, must be between 5..10.Current value is { (byte)chan }");
                
        }

        public void OnTemperatureChanged(float temperature)
        {
            TemperatureChanged?.Invoke(this, new TemperaturEventArgs(temperature));
        }
        #endregion

    }
}
