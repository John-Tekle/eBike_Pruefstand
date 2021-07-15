using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_eBike_Pruefstand;

namespace Service_eBike_Pruefstand
{
    public class Luefter: AnalogToDigital, ILuefter
    {
        public event EventHandler<LuefterEventArgs> ValueChanged;

        #region properties
        public float Value { get; protected set; }
        #endregion

        #region Constructor and Deconstructor
        public Luefter(I2C_Addr addr)
        {

        }
        #endregion
    }
}
