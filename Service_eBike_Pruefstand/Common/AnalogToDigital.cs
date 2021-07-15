using System;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Service_eBike_Pruefstand
{
    public abstract class AnalogToDigital: IMAX11617
    {
        #region properties
        #endregion

        #region constructor & destructor
        public AnalogToDigital()
        {

        }
        #endregion

        #region methodes
        public bool SetupByte(byte setUp)
        {
            throw new NotImplementedException();
        }

        public bool Configuration(byte config)
        {
            throw new NotImplementedException();
        }

        public bool Read_ADCValue()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
