using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_eBike_Pruefstand;

namespace Service_eBike_Pruefstand
{
    public interface ILuefter
    {
        #region events
        event EventHandler<LuefterEventArgs> ValueChanged;
        #endregion


        #region properties
        byte Value { get; }
        #endregion
    }
}
