using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_eBike_Pruefstand
{
    public class LuefterEventArgs : EventArgs
    {
        #region constructor & destructor
        public LuefterEventArgs(byte value)
        {
            Value = value;
        }
        #endregion


        #region properties
        public byte Value { get; }
        public string Name { get { return "Luefter"; } }
        #endregion
    }
}
