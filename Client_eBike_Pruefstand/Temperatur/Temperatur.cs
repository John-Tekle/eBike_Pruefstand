using System;
using Common_eBike_Pruefstand;

namespace Client_eBike_Pruefstand
{
    class Temperatur
    {
        public static event EventHandler<TemperaturEventArgs> TemperatureChanged;

        public static void Update(object value)
        {
            float f_value = float.Parse(value.ToString());
            OnValueOfTemperaturChanged(f_value);
        }

        private static void OnValueOfTemperaturChanged(float value)
        {
            TemperatureChanged?.Invoke(null, new TemperaturEventArgs(value));
        }
    }
}
