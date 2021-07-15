using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_eBike_Pruefstand
{
    public class GewichtEventArgs : EventArgs
    {
        #region constructor & destructor
        public GewichtEventArgs(float load)
        {
            Load = load;
        }
        #endregion


        #region properties
        public float Load  { get; }
        #endregion
    }
}
