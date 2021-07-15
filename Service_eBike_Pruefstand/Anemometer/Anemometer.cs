using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_eBike_Pruefstand;

namespace Service_eBike_Pruefstand
{
    public class Anemometer : IAnemometer
    {
        #region members
        public event EventHandler<AnemometerEventArgs> SpeedChanged;
        #endregion

        #region properties
        public float Speed { get; protected set; }
        #endregion

        #region constructor & destructor
        public Anemometer(I2C_Addr addr)
        {

        }
        #endregion
    }
}
