using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_eBike_Pruefstand
{
    public class TemperaturEventArgs
    {
        #region constructor & destructor
        public TemperaturEventArgs(float temperature)
        {
            Temperature = temperature;
        }
        #endregion


        #region properties
        public float Temperature { get; }
        #endregion
    }
}
