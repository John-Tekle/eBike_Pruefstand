using System;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;
using Common_eBike_Pruefstand;

namespace Service_eBike_Pruefstand
{
    public abstract class AnalogToDigital: IADC_MAX11617
    {
        #region members
        private II2CDevice ADC_Device;
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        #endregion

        #region constructor & destructor
        public AnalogToDigital(ADC_MAX11617.Address addr)
        {
            Pi.Init<BootstrapWiringPi>();
            ADC_Device = Pi.I2C.AddDevice((byte)addr & 0x7F); //7-Bit Adress
        }
        #endregion

        #region properties
        protected byte Channel { get; set; } = 0;
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
            command[1] = command_;
            command[1] = (byte)(command[1] & 0b01111111); // Register bit. 1 = configuration byte
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
