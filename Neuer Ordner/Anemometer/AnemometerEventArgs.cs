using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_eBike_Pruefstand
{
    class AnemometerEventArgs : EventArgs
    {
        #region constructor & destructor
        public AnemometerEventArgs(float speed)
        {
            Speed = speed;
        }
        #endregion


        #region properties
        public float Speed { get; }
        #endregion
    }
}
