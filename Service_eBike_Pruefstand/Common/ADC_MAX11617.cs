using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_eBike_Pruefstand
{
    public class ADC_MAX11617
    {
        private const byte Addr_IMAX11617 = 35;
        
        public enum Address
        {
            Anemometer = Addr_IMAX11617,
            Gewicht = Addr_IMAX11617,
            Luefter = Addr_IMAX11617,
            Temperatur = Addr_IMAX11617
        }

        public enum Channel
        {
            Anemometer = 7,
            Gewicht,
            Luefter,
            Temperatur0 = 0,
            Temperatur1,
            Temperatur2,
            Temperatur3,
            Temperatur4,
            Temperatur5
        }
    }
}
