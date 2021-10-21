using System;
using Common_eBike_Pruefstand;

namespace Client_eBike_Pruefstand
{
    class Gewicht
    {
        public static event EventHandler<GewichtEventArgs> LoadChanged;

        public static void Update(object value)
        {
            float f_value = float.Parse(value.ToString());
            OnValueOfGewichtChanged(f_value);
        }

        private static void OnValueOfGewichtChanged(float value)
        {
            LoadChanged?.Invoke(null, new GewichtEventArgs(value));
        }
    }
}
