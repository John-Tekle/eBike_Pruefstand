using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_eBike_Pruefstand
{
    interface IAnemometer
    {
        #region events
        event EventHandler<AnemometerEventArgs> SpeedChanged;
        #endregion


        #region properties
        float Speed { get; }
        #endregion
    }
}
