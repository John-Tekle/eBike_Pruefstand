using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_eBike_Pruefstand;

namespace Service_eBike_Pruefstand
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
        bool Read(ADC_MAX11617.Channel chan, out float temperatur);
        #endregion
    }
}
