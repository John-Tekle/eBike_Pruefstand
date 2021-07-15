using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_eBike_Pruefstand
{
    public interface IGewicht
    {
        #region events
        event EventHandler<GewichtEventArgs> LoadChanged;
        #endregion


        #region properties
        float Load { get; }
        #endregion
    }
}
