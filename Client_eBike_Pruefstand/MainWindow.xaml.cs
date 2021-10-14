using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Win32;

namespace Client_eBike_Pruefstand
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Members
        List<CheckBox> checkBoxes;
        Einstellung_Win einstellung_Win;
        private CheckBox currentcheckbox;
        public Dictionary<string, bool> KeyValuePairsCommmand { get; private set; }
        #endregion

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
            checkBoxes = new List<CheckBox>() { checkBox0, checkBox1, checkBox2, checkBox3, checkBox4, checkBox5 };
            Einstellung_Win.EinstellungChanged += Einstellung_Win_EinstellungChanged;
            einstellung_Win = new Einstellung_Win();
            RegistryHelperConfiguration();
            TCP_Client.ClientStatusUpdate += TCP_Client_ClientStatusUpdate;
            TCP_Client.CommandReceived += TCP_Client_CommandReceived;
            //Expander is disabled until the connection is established
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            { expander.IsEnabled = false; })); //=  BTemperatur.IsEnabled = BGewicht.IsEnabled = BAnemometer.IsEnabled = BLuefter.IsEnabled 
        }
        #endregion

        private void TCP_Client_CommandReceived(object sender, Common_eBike_Pruefstand.TCPEventArgs e)
        {
            KeyValuePairsCommmand = null;
            try
            {
                KeyValuePairsCommmand = JsonSerializer.Deserialize<Dictionary<string, bool>>(e.Command);
                foreach (KeyValuePair<string, bool> res in KeyValuePairsCommmand)
                {
                    if (res.Key.Substring(0, 6) == "Logger")
                    {
                        switch (res.Key.Substring(0, (int)(res.Key.Length - 2)))
                        {
                            case "Logger+Gewicht+ACK": UpdateEllipseState(res, ElGewicht); break;
                            case "Logger+Anemometer+ACK": UpdateEllipseState(res, ElAnemometer); break;
                            case "Logger+Luefter+ACK": UpdateEllipseState(res, ElLuefter); break;
                            case "Logger+Temperature+ACK":
                                //Not allowed changing temperature channel during Logging
                                if (res.Value) Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                                { expander.IsEnabled = false; }));
                                else Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                                { expander.IsEnabled = true; }));
                                UpdateEllipseState(res, ElTemperatur); break;
                            default: break;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TCP_Client_ClientStatusUpdate(string ClientStatusUpdate)
        {
            this.networkStatus.Dispatcher.Invoke(() => { networkStatus.Text = ClientStatusUpdate; });
        }

        private void Einstellung_Win_EinstellungChanged(object sender, EinstellungEventArgs e)
        {
            for (int i = 0; i < 6; i++)
            {
                if (e.TextBoxes[Einstellung_Win.textBoxes0[i]].Text != checkBoxes[i].Name)
                {
                    checkBoxes[i].Content = e.TextBoxes[Einstellung_Win.textBoxes0[i]].Text;
                    if (currentcheckbox != null)
                    {
                        expander.Header = currentcheckbox.Content;
                        RegistryHelper.RegistrySetString("Expander Header", expander.Header);
                        RegistryHelper.RegistrySetString("Last Checked", currentcheckbox.Content);
                    }
                }   
            }
        }

        #region RegistryHelper
        private void RegistryHelperConfiguration()
        {
            for (int i = 0; i < 6; i++)
            {
                if (RegistryHelper.RegistryGetString(Einstellung_Win.textBoxes0[i].Text, string.Empty) != string.Empty)
                    checkBoxes[i].Content = Einstellung_Win.textBoxes1[i].Text =
                        RegistryHelper.RegistryGetString(Einstellung_Win.textBoxes0[i].Text, string.Empty);
                else
                {
                    checkBoxes[i].Content = Einstellung_Win.textBoxes1[i].Text = $"#####{i}";
                    RegistryHelper.RegistrySetString(Einstellung_Win.textBoxes0[i].Text, $"#####{i}");
                }
            }

            if (RegistryHelper.RegistryGetString("Expander Header", string.Empty) != string.Empty)
                expander.Header = RegistryHelper.RegistryGetString("Expander Header", string.Empty);
            else
            {
                RegistryHelper.RegistrySetString("Expander Header", "######");
                expander.Header = RegistryHelper.RegistryGetString("Expander Header", string.Empty);
            }

            if (RegistryHelper.RegistryGetString("IP Address", string.Empty) != string.Empty)
                einstellung_Win.IPAddress = RegistryHelper.RegistryGetString("IP Address", string.Empty);
            else
            {
                einstellung_Win.IPAddress = Einstellung_Win.staticIp;
                RegistryHelper.RegistrySetString("IP Address", Einstellung_Win.staticIp);
            }

            foreach (var checkBox in checkBoxes.Where(_checkBox => _checkBox.Content.ToString() == RegistryHelper.RegistryGetString("Last Checked", "")))
                checkBox.IsChecked = true;
        }
        #endregion

        #region Move WinApp using Mouse
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        #endregion

        #region Log and Ellipses
        private void LogButtonOnClick(object sender, RoutedEventArgs e)
        {
            if(sender == BTemperatur)
                SendEllipseState(ElTemperatur, "Temperature");
            else if(sender == BGewicht)
                SendEllipseState(ElGewicht, "Gewicht");
            else if (sender == BAnemometer)
                SendEllipseState(ElAnemometer, "Anemometer");
            else if (sender == BLuefter)
                SendEllipseState(ElLuefter, "Luefter");
            e.Handled = true; //Event marked as handled
        }
        private void SendEllipseState(Ellipse ellipse, string name)
        {
            if (ellipse.Visibility == Visibility.Visible)
                SendLogCommand(name, false);
            else if (ellipse.Visibility == Visibility.Hidden)
                SendLogCommand(name, true);
        }
        
        private void UpdateEllipseState(KeyValuePair<string, bool> keyValuePair, Ellipse ellipse)
        {
            if (keyValuePair.Value) Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            { ellipse.Visibility = Visibility.Visible; }));
            else Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            { ellipse.Visibility = Visibility.Hidden; }));
        }
        private void SendLogCommand(string name, bool value)
        {
            Dictionary<string, bool> keyValuePairs = new Dictionary<string, bool>
            {
                { $"Logger+{name}: ", value }
            };
            TCP_Client.SendCommand(JsonSerializer.Serialize(keyValuePairs));
        }
        #endregion

        #region Ellipse Close & Mini
        private void Windows_Close(object sender, MouseButtonEventArgs e)
        {
            TCP_Client.Close();
            if(einstellung_Win.ShowActivated)
                einstellung_Win.Close();
            this.Close();
        }

        private void Windows_Minimize(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Windows_CloseMouseEnter(object sender, MouseEventArgs e)
        {
            El_Close.Fill = Brushes.Firebrick;
        }

        private void Windows_CloseMouseLeave(object sender, MouseEventArgs e)
        {
            El_Close.Fill = Brushes.Red;
        }

        private void Windows_CloseMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            El_Close.Fill = Brushes.DarkRed;
        }

        private void Windows_MiniMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            El_Mini.Fill = Brushes.Yellow;
            El_Mini.Opacity = 0.5;
        }

        private void Windows_MiniMouseEnter(object sender, MouseEventArgs e)
        {
            El_Mini.Fill = Brushes.Yellow;
            El_Mini.Opacity = 0.8;
        }

        private void Windows_MiniMouseLeave(object sender, MouseEventArgs e)
        {
            El_Mini.Fill = Brushes.Yellow;
            El_Mini.Opacity = 1;
        }
        #endregion

        #region Einstellung_Win
        private void Click_Einstellung(object sender, RoutedEventArgs e)
        {
            einstellung_Win.ShowDialog();
        }
        #endregion

        #region CheckBox
        /// <summary>
        /// You can only select one at a time
        /// </summary>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            currentcheckbox = (CheckBox)sender;
            expander.IsExpanded = false;
            expander.Header = currentcheckbox.Content;
            foreach (var checkBox in checkBoxes.OfType<CheckBox>().Where(_checkBox => (bool)_checkBox.IsChecked))
                if (!currentcheckbox.Name.Equals(checkBox.Name)) 
                    checkBox.IsChecked = false;
            RegistryHelper.RegistrySetString("Expander Header", expander.Header);
            RegistryHelper.RegistrySetString("Last Checked", currentcheckbox.Content);
        }
        
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            expander.IsExpanded = false;
            foreach (var checkBox in checkBoxes.OfType<CheckBox>().Where(_checkBox => (bool)_checkBox.IsChecked)) return;
            expander.Header = "######";
            RegistryHelper.RegistrySetString("Expander Header", "######");
            RegistryHelper.RegistrySetString("Last Checked", "");
        }
        #endregion 
    }
}
