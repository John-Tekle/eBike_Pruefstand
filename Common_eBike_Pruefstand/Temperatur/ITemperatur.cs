using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_eBike_Pruefstand
{
    public interface ITemperatur
    {
        #region events
        event EventHandler<TemperaturEventArgs> TemperatureChanged;
        #endregion


        #region properties
        float TemperatureValue { get; }
        #endregion

        #region Method
        bool Read(byte chan, out float temperatur);
        #endregion
    }
}
