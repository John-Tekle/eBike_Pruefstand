using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client_eBike_Pruefstand
{
    /// <summary>
    /// Interaction logic for Einstellung_Win.xaml
    /// </summary>
    public partial class Einstellung_Win : Window
    {
        #region Members
        public string IPAddress
        {
            get
            {
                return setIPAddress.Text;
            }
            set
            {
                setIPAddress.Text = value;
            }
        }
        public const string staticIp = "192.168.0.1";
        public static List<TextBox> textBoxes0 { get; private set; }
        public static List<TextBox> textBoxes1 { get; private set; }
        public static Dictionary<TextBox, TextBox> keyValuePairs { get; private set; }
        public static event EventHandler<EinstellungEventArgs> EinstellungChanged;
        #endregion

        public Einstellung_Win() 
        {
            InitializeComponent(); 
            textBoxes0 = new List<TextBox>() { temperatur0, temperatur1, temperatur2, temperatur3, temperatur4, temperatur5 };
            textBoxes1 = new List<TextBox>() { temperatur_0, temperatur_1, temperatur_2, temperatur_3, temperatur_4, temperatur_5 };
            keyValuePairs = new Dictionary<TextBox, TextBox>();

            for (int i = 0; i < (textBoxes0.Count & textBoxes1.Count); i++)
            {
                textBoxes1[i].Text = RegistryHelper.RegistryGetString(textBoxes0[i].Text, "");
                keyValuePairs.Add(textBoxes0[i], textBoxes1[i]);
            }

        }

        private void EWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < textBoxes0.Count; i++)
                if (textBoxes1[i].Text != RegistryHelper.RegistryGetString(textBoxes0[i].Text, ""))
                    OnEinstellungChanged(keyValuePairs);
            for (int i = 0; i < (textBoxes0.Count & textBoxes1.Count); i++)
                RegistryHelper.RegistrySetString(textBoxes0[i].Text, textBoxes1[i].Text);
            MessageBoxResult dialogResult;
            if (IPAddress != RegistryHelper.RegistryGetString("IP Address", ""))
            {
                dialogResult = MessageBox.Show("Wollen Sie die neue IP-Adresse speichern?", "IP-Adressänderung", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    try
                    {
                        System.Net.IPAddress.Parse(IPAddress);
                        RegistryHelper.RegistrySetString("IP Address", IPAddress);
                    }
                    catch (FormatException formatException) 
                    { 
                        MessageBox.Show(formatException.Message);
                        IPAddress = RegistryHelper.RegistryGetString("IP Address", "");
                        return; 
                    }
                }
                else IPAddress = RegistryHelper.RegistryGetString("IP Address", "");
            }
            this.Hide();
        }

        public void OnEinstellungChanged(Dictionary<TextBox, TextBox> textBoxes)
        {
            EinstellungChanged?.Invoke(this, new EinstellungEventArgs(textBoxes));
        }
    }

    public class EinstellungEventArgs : EventArgs
    {
        #region constructor & destructor
        public EinstellungEventArgs(Dictionary<TextBox, TextBox> textBoxes)
        {
            TextBoxes = textBoxes;
        }
        #endregion


        #region properties
        public Dictionary<TextBox, TextBox> TextBoxes { get; private set; }
        #endregion
    }
}
