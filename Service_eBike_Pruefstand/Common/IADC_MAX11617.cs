using System;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Service_eBike_Pruefstand
{
    public interface IADC_MAX11617
    {
        #region properties
        //byte Channel { get; set; }  //Compiler Error CS0737
        #endregion

        #region methods
        bool WriteSetupByte();
        bool WriteConfigByte(byte command_);
        bool ReadResult(out byte[] data_, out int length);
        #endregion
    }
}
