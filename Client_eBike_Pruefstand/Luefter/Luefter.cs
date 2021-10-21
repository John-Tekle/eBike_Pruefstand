using System;
using Common_eBike_Pruefstand;

namespace Client_eBike_Pruefstand
{
    class Luefter
    {
        public static event EventHandler<LuefterEventArgs> ValueChanged;

        public static void Update(object value)
        {
            byte b_value = byte.Parse(value.ToString());
            OnAnemometerUpdate(b_value);
        }

        private static void OnAnemometerUpdate(byte value)
        {
            ValueChanged?.Invoke(null, new LuefterEventArgs(value));
        }

    }
}
