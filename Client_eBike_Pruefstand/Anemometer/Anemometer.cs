using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_eBike_Pruefstand;

namespace Client_eBike_Pruefstand
{
    class Anemometer
    {
        public static event EventHandler<AnemometerEventArgs> SpeedChanged;

        public static void Update(object value)
        {
            float f_value = float.Parse(value.ToString());
            OnValueOfAnemometerChanged(f_value);
        }

        private static void OnValueOfAnemometerChanged(float value)
        {
            SpeedChanged?.Invoke(null, new AnemometerEventArgs(value));
        }

    }
}
