using System;
using Common_eBike_Pruefstand;

namespace Service_eBike_Pruefstand
{
    public class Temperatur: AnalogToDigital, ITemperatur
    {
        public event EventHandler<TemperaturEventArgs> TemperatureChanged;

        #region properties
        public float Temperature { get; protected set; }
        #endregion

        #region constructor & destructor
        public Temperatur(ADC_MAX11617.Address addr, ADC_MAX11617.Channel chan) : base(addr)
        {
            this.Channel = (byte)chan;
        }
        #endregion

        #region methodes
        public bool Read(byte chan, out float temperatur)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
