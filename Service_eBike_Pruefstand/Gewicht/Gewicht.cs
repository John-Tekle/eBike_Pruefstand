using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_eBike_Pruefstand;

namespace Service_eBike_Pruefstand
{
    public class Gewicht: IGewicht
    {
        public event EventHandler<GewichtEventArgs> LoadChanged;

        #region properties
        public float Load { get; protected set; }
        #endregion

        #region constructor & destructor
        public Gewicht(I2C_Addr addr)
        {

        }
        #endregion

        
    }
}
