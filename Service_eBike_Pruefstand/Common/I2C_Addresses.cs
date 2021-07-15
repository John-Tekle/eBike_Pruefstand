using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_eBike_Pruefstand
{
    public enum I2C_Addr
    {
        Anemometer = 65,
        Gewicht = 85,
        Luefter = 78,
        Temperatur = 0x5A
    }
}
