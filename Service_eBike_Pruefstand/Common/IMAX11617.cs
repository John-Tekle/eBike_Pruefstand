using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_eBike_Pruefstand
{
    public interface IMAX11617
    {
        #region properties
        //bool 
        #endregion

        #region methods
        bool SetupByte(byte setUp);
        bool Configuration(byte config);
        bool Read_ADCValue();
        #endregion
    }
}
