using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_eBike_Pruefstand;
using System.Threading;

namespace Service_eBike_Pruefstand
{
    public class Anemometer : AnalogToDigital ,IAnemometer
    {
        #region members
        protected static new readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        public event EventHandler<AnemometerEventArgs> SpeedChanged;
        #endregion

        #region constructor & destructor
        public Anemometer(ADC_MAX11617.Address addr, ADC_MAX11617.Channel chan): base(addr)
        {
            if ((byte)chan >= 5 && (byte)chan <= 10)
                base.Channel = (byte)chan;
            else
                throw new ArgumentOutOfRangeException($"{nameof(Anemometer)}: value of Channel is out of the range, must be between 5..10. Current value is {(byte)chan}");
        }
        #endregion

        #region properties
        public float Speed { get; set; }
        #endregion

        /*
             * Es ist nur ein Versuchen. Es wurde noch nicht richtig implementiert.
             * */
        #region methodes
        public bool Read(byte chan, out float speed)
        {
            if ((byte)chan >= 5 && (byte)chan <= 10)
            {
                byte[] data = new byte[2];
                int length = 0;
                if (WriteConfigByte(chan))
                {
                    //Thread.Sleep(1);
                    if (!ReadResult(out data, out length))
                    {
                        log.Error("Reading data from MAX11617, Channel to read: " + chan);
                        speed = float.MaxValue;
                        return false;
                    }
                    UInt16 sensorValue = (UInt16)(((data[0] << 8) | data[1]) & 0b0000111111111111);
                    float sensorVoltage = (float)(3.30 / 4095.0) * sensorValue;
                    speed = (float)((sensorVoltage - (float)1.2379) / (float)0.005);
                    speed = (float)Math.Round(sensorVoltage, 2);
                    return true;
                }
                else
                    log.Error("Writing command to MAX11617, Channel to read: " + chan);
                speed = float.MaxValue;
                return false;
            }
            else
                throw new ArgumentOutOfRangeException($"{ nameof(Anemometer) }: value of Channel is out of the range, must be between 5..10.Current value is { (byte)chan }");
        }

        public void OnSpeedChanged(float speed)
        {
            SpeedChanged?.Invoke(this, new AnemometerEventArgs(speed));
        }
        #endregion
    }
}
