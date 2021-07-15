using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_eBike_Pruefstand
{
    public interface ILuefter
    {
        #region events
        event EventHandler<LuefterEventArgs> ValueChanged;
        #endregion


        #region properties
        float Value { get; }
        #endregion
    }
}
