using System;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Service_eBike_Pruefstand
{
    public abstract class I2C_Raspberry<T>
    {
        #region members
        protected II2CDevice ADC_Device { get; }
        protected II2CDevice LUE_Device { get; }
        #endregion

        #region constructor & destructor
        public I2C_Raspberry(T addr)
        {
            //typeof(I2C_Address.Address).GetType();
            try
            {
                Pi.Init<BootstrapWiringPi>();
                if(addr.GetType() == ADC_MAX11617.Address.Anemometer.GetType() || addr.GetType() == ADC_MAX11617.Address.Temperatur.GetType() || addr.GetType() == ADC_MAX11617.Address.Gewicht.GetType())
                    ADC_Device = Pi.I2C.AddDevice((byte)Convert.ChangeType(addr, typeof(byte)) & 0x7F); //7-Bit Adress //https://www.codegrepper.com/code-examples/csharp/c%23+cast+generic+type+to+specific+type
                else if(addr.GetType() == I2C_Address.Address.Luefter.GetType())
                    LUE_Device = Pi.I2C.AddDevice((byte)Convert.ChangeType(addr, typeof(byte)) & 0x7F); //7-Bit Adress //https://www.codegrepper.com/code-examples/csharp/c%23+cast+generic+type+to+specific+type
            }
            catch(Exception e)
            {
                throw e;
            }
            
        }
        #endregion
    }

    public class ADC_MAX11617
    {
        private const byte Addr_IMAX11617 = 51;

        public enum Address
        {
            Anemometer = Addr_IMAX11617,
            Gewicht = Addr_IMAX11617,
            Temperatur = Addr_IMAX11617
        }

        public enum Channel
        {
            Anemometer = 6,
            Gewicht,
            Luefter,
            TemperaturDefault = 0,
            Temperatur0 = 0,
            Temperatur1,
            Temperatur2,
            Temperatur3,
            Temperatur4,
            Temperatur5
        }
    }

    public class I2C_Address
    {
        private const byte Luefter = 0x5A;

        public enum Address
        {
            Luefter = I2C_Address.Luefter
        }
    }
}
