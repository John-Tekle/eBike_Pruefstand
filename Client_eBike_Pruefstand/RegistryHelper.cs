using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_eBike_Pruefstand
{
    public class RegistryHelper
    {
        #region members
        private const string RegKey = "Software\\hslu\\Client_eBike_Pruefstand";
        public static bool RegisteryAvailable
        {
            get
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(RegKey, false);
                if (key != null) return true;
                else return false;
            }
            private set { }
        }
        #endregion

        #region registry helper
        public static string RegistryGetString(string name, string defaultValue)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(RegKey, false);
            if (key == null) key = Registry.CurrentUser.CreateSubKey(RegKey);
            string value = (string)key.GetValue(name, defaultValue);
            key.Close();
            return value;
        }

        public static string RegistryGetString(string name, object defaultValue)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(RegKey, false);
            if (key == null) key = Registry.CurrentUser.CreateSubKey(RegKey);
            string value = (string)key.GetValue(name, defaultValue);
            key.Close();
            return value;
        }

        public static void RegistrySetString(string name, string value)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(RegKey, true);
            if (key == null) key = Registry.CurrentUser.CreateSubKey(RegKey);
            key.SetValue(name, value);
            key.Close();
        }

        public static void RegistrySetString(string name, object value)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(RegKey, true);
            if (key == null) key = Registry.CurrentUser.CreateSubKey(RegKey);
            key.SetValue(name, value);
            key.Close();
        }
        #endregion
    }
}
