using System;
using Common_eBike_Pruefstand;

namespace Service_eBike_Pruefstand
{
    public class Luefter : I2C_Raspberry<I2C_Address.Address> ,ILuefter
    {
        #region members
        public event EventHandler<LuefterEventArgs> ValueChanged;
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructor and Deconstructor
        public Luefter(I2C_Address.Address addr) : base(addr) { }
        #endregion

        #region properties
        public byte Value { get; set; }
        #endregion

        #region method
        public bool Read(out byte data)
        {
            /*
             * Es ist nur ein Versuchen. Es wurde noch nicht richtig implementiert.
             * */
            ushort temp_ = LUE_Device.ReadAddressWord(0x07);
            float temp = (float)((temp_ / 50.0) - 273.15);
            data = (byte)temp;
            return true;
        }

        public void OnValueChanged(byte value)
        {
            ValueChanged?.Invoke(this, new LuefterEventArgs(value));
        }
        #endregion

    }
}
