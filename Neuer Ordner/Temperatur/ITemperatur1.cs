using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_eBike_Pruefstand
{
    interface ITemperatur1
    {
        #region events
        event EventHandler<TemperaturEventArgs> TemperatureChanged;
        #endregion


        #region properties
        float Temperature { get; }
        #endregion
    }
}
