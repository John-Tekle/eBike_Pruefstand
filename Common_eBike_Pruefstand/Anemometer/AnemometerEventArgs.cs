using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_eBike_Pruefstand
{
    public class AnemometerEventArgs : EventArgs
    {
        #region constructor & destructor
        public AnemometerEventArgs(float speed)
        {
            Speed = speed;
        }
        #endregion


        #region properties
        public float Speed { get; }
        public string Name { get { return "Anemometer"; } }
        #endregion
    }
}
