using System;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;
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
        public Temperatur()
        {

        }
        #endregion

        #region methodes

        #endregion

    }
}
