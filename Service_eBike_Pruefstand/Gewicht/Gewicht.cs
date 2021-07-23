using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_eBike_Pruefstand;
using System.Threading;

namespace Service_eBike_Pruefstand
{
    public class Gewicht: AnalogToDigital, IGewicht
    {
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        public event EventHandler<GewichtEventArgs> LoadChanged;

        

        #region constructor & destructor
        public Gewicht(ADC_MAX11617.Address addr, ADC_MAX11617.Channel chan) : base(addr)
        {
            base.Channel = (byte)chan;
        }
        #endregion

        #region properties
        public float Load { get; set; }
        #endregion

        /*
             * Es ist nur ein Versuchen. Es wurde noch nicht richtig implementiert.
             * */
        #region methodes
        public bool Read(byte chan, out float load)
        {
            byte[] data = new byte[2];
            int length = 0;
            if (WriteConfigByte(chan))
            {
                Thread.Sleep(1);
                if (!ReadResult(out data, out length))
                {
                    log.Error("Reading data from MAX11617, Channel to read: " + chan);
                    load = float.MaxValue;
                    return false;
                }
                UInt16 sensorValue = (UInt16)((data[0] << 8) | data[1]);
                float sensorVoltage = (float)(3.30 / 4095.0) * sensorValue;
                load = (float)((sensorVoltage - (float)1.2379) / (float)0.005);
                load = (float)Math.Round(load, 2);
                return true;
            }
            else
                log.Error("Writing command to MAX11617, Channel to read: " + chan);
            load = float.MaxValue;
            return false;
        }

        public void OnLoadChanged(float load)
        {
            LoadChanged?.Invoke(this, new GewichtEventArgs(load));
        }
        #endregion


    }
}
