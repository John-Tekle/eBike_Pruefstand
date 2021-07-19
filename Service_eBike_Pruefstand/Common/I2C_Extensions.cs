using System.Runtime.InteropServices;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;


namespace Service_eBike_Pruefstand
{
    public static class Extensions
    {
        internal const string WiringPiLibrary = "libwiringPi.so.2.52";

        public static int Read(this II2CDevice dev, byte[] data, int length)
        {
            return WiringPiI2CReadEx(dev.FileDescriptor, data, length);
        }

        public static int Write(this II2CDevice dev, byte[] data, int length)
        {
            return WiringPiI2CWriteEx(dev.FileDescriptor, data, length);
        }

        [DllImport(WiringPiLibrary, EntryPoint = "read", SetLastError = true)]
        public static extern int WiringPiI2CReadEx(int fd, byte[] data, int length);

        [DllImport(WiringPiLibrary, EntryPoint = "write", SetLastError = true)]
        public static extern int WiringPiI2CWriteEx(int fd, byte[] data, int length);

    }
}
