using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_eBike_Pruefstand;

namespace Service_eBike_Pruefstand
{
    public interface IAnemometer
    {
        #region events
        event EventHandler<AnemometerEventArgs> SpeedChanged;
        #endregion


        #region properties
        float Speed { get; }
        #endregion
    }
}
