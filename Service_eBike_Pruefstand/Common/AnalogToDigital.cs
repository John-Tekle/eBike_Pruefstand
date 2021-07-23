using System;
using Common_eBike_Pruefstand;

namespace Service_eBike_Pruefstand
{
    public abstract class AnalogToDigital: I2C_Raspberry<ADC_MAX11617.Address>, IADC_MAX11617
    {
        #region members
        //private II2CDevice ADC_Device;
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        #endregion

        #region constructor & destructor
        public AnalogToDigital(ADC_MAX11617.Address addr): base(addr)
        {

        }
        #endregion

        #region properties
        public byte Channel { get; protected set; } = 0;
        #endregion

        #region methodes
        public bool WriteSetupByte()
        {
            byte[] command = new byte[1] { 0x82 }; //Look page 14 Table 1. Setup Byte Format
            command[1] = (byte)(command[1] & 0b11111111); // Register bit. 1 = setup byte
            int result = ADC_Device.Write(command, command.Length);
            if (result != 1)
            {
                log.Error("Error write command to MAX11617, ErrorCode:" + result);
                return false;
            }
            return true;
        }

        public bool WriteConfigByte(byte command_)
        {
            byte[] command = new byte[1];
            command[0] = command_;
            command[0] = (byte)(command[0] & 0b01111111); // Register bit. 1 = configuration byte
            int result = ADC_Device.Write(command, command.Length);
            if (result != 1)
            {
                log.Error("Error write command to MAX11617, ErrorCode:" + result);
                return false;
            }
            return true;
        }

        public bool ReadResult(out byte[] data_, out int length)
        {
            byte[] data = new byte[2];
            int result = ADC_Device.Read(data, 2);
            if (result != 2)
            {
                log.Error("Error reading command to MAX11617, ErrorCode:" + result);
                data_ = data;
                length = result;
                return false;
            }
            data_ = data;
            length = result;
            return true;
        }
        #endregion
    }
}
