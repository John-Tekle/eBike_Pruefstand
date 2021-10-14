using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_eBike_Pruefstand;

namespace Service_eBike_Pruefstand
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
