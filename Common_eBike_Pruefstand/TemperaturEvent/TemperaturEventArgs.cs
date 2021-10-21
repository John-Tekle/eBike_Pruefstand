using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_eBike_Pruefstand
{
    public class TemperaturEventArgs : EventArgs
    {
        #region constructor & destructor
        public TemperaturEventArgs(float temperature)
        {
            Temperature = temperature;
        }
        #endregion


        #region properties
        public float Temperature { get; private set; }
        public string Name { get { return "Temperatur"; } }
        #endregion
    }
}
