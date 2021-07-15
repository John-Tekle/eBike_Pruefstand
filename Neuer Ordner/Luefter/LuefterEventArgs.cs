using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_eBike_Pruefstand
{
    class LuefterEventArgs
    {
        #region constructor & destructor
        public LuefterEventArgs(int value)
        {
            Value = value;
        }
        #endregion


        #region properties
        public int Value { get; }
        #endregion
    }
}
