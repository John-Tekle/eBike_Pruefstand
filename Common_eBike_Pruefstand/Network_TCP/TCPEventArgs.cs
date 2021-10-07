using System;

namespace Common_eBike_Pruefstand
{
    public class TCPEventArgs
    {
        #region constructor & destructor
        public TCPEventArgs(string command)
        {
            Command = command;
        }
        #endregion

        #region properties
        public string Command { get; }
        #endregion
    }
}