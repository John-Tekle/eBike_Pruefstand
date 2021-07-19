using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_eBike_Pruefstand;

namespace Service_eBike_Pruefstand
{
    public class Anemometer : AnalogToDigital ,IAnemometer
    {
        #region members
        public event EventHandler<AnemometerEventArgs> SpeedChanged;
        #endregion

        #region properties
        public float Speed { get; protected set; }
        #endregion

        #region constructor & destructor
        public Anemometer(ADC_MAX11617.Address addr, ADC_MAX11617.Channel chan): base(addr)
        {
            this.Channel = (byte)chan;
        }
        #endregion
    }
}
