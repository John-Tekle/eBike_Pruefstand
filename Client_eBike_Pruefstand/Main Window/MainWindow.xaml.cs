using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Client_eBike_Pruefstand
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Members
        List<CheckBox> checkBoxes;
        public Einstellung_Win einstellung_Win;
        private CheckBox currentcheckbox;
        #endregion

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
            this.ShowActivated = false; //The default value is true, so it must be false at the beginning.
            checkBoxes = new List<CheckBox>() { checkBox0, checkBox1, checkBox2, checkBox3, checkBox4, checkBox5 };
            Einstellung_Win.EinstellungChanged += Einstellung_Win_EinstellungChanged;
            einstellung_Win = new Einstellung_Win();
            RegistryHelperConfiguration();
            Connector.ClientStatusUpdater += Connector_ClientStatusUpdater;
            Connector.Initialization();
            Connector.EllipseUpdater += Connector_EllipseUpdater;
            //Expander is disabled until the connection is established
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            { expander.IsEnabled = false; }));
            Temperatur.TemperatureChanged += Temperatur_TemperatureChanged;
            Gewicht.LoadChanged += Gewicht_LoadChanged;
            Anemometer.SpeedChanged += Anemometer_SpeedChanged;
        }
        #endregion
        private void Temperatur_TemperatureChanged(object sender, Common_eBike_Pruefstand.TemperaturEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            { BTemperatur.Content = $"{e.Temperature:0.00}°C"; }));
        }
        private void Gewicht_LoadChanged(object sender, Common_eBike_Pruefstand.GewichtEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            { BGewicht.Content = $"{e.Load:0.00}Kg"; }));
        }
        private void Anemometer_SpeedChanged(object sender, Common_eBike_Pruefstand.AnemometerEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            { BAnemometer.Content = $"{e.Speed:0.00}m/s"; }));
        }
        private void Connector_ClientStatusUpdater(string clientStatusUpdate)
        {
            this.networkStatus.Dispatcher.Invoke(() => { networkStatus.Text = clientStatusUpdate; });
        }
        private void Connector_EllipseUpdater(KeyValuePair<string, object> res)
        {
            bool keyValuePair = bool.Parse(res.Value.ToString());
            switch (res.Key.Substring(0, (int)(res.Key.Length - 2)))
            {
                case "Logger+Gewicht+ACK": UpdateEllipseState(keyValuePair,ElGewicht); break;
                case "Logger+Anemometer+ACK": UpdateEllipseState(keyValuePair, ElAnemometer); break;
                case "Logger+Luefter+ACK": UpdateEllipseState(keyValuePair, ElLuefter); break;
                case "Logger+Temperature+ACK":
                    //Not allowed changing temperature channel during Logging
                    if (keyValuePair) Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    { expander.IsEnabled = false; }));
                    else Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    { expander.IsEnabled = true; }));
                    UpdateEllipseState(keyValuePair, ElTemperatur); break;
                default: break;
            }
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
                        UpdateHeaderServerSide("Temperatur", expander.Header.ToString());
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
                einstellung_Win.IPAddress = TCP_Client.Default_IPAddress;
                RegistryHelper.RegistrySetString("IP Address", TCP_Client.Default_IPAddress);
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
                Connector.SendLogCommand(name, false);
            else if (ellipse.Visibility == Visibility.Hidden)
                Connector.SendLogCommand(name, true);
        }
        
        private void UpdateEllipseState(bool keyValuePair, Ellipse ellipse)
        {
            if (keyValuePair) Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            { ellipse.Visibility = Visibility.Visible; }));
            else Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            { ellipse.Visibility = Visibility.Hidden; }));
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
            foreach (CheckBox checkBox in checkBoxes.OfType<CheckBox>().Where(_checkBox => (bool)_checkBox.IsChecked))
                if (!currentcheckbox.Name.Equals(checkBox.Name))
                    checkBox.IsChecked = false;
            RegistryHelper.RegistrySetString("Expander Header", expander.Header);
            RegistryHelper.RegistrySetString("Last Checked", currentcheckbox.Content);
            //Updating temperature header for server side
            if (this.ShowActivated) //Make sure that GUI is visible
                for(int i = 0; i < Einstellung_Win.textBoxes1.Count; i++)
                    if(Einstellung_Win.textBoxes1[i].Text.Equals(currentcheckbox.Content.ToString()))
                        UpdateHeaderServerSide(Einstellung_Win.textBoxes0[i].Text, Einstellung_Win.textBoxes1[i].Text);
        }
        
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            expander.IsExpanded = false;
            foreach (var checkBox in checkBoxes.OfType<CheckBox>().Where(_checkBox => (bool)_checkBox.IsChecked)) return;
            expander.Header = "######";
            RegistryHelper.RegistrySetString("Expander Header", "######");
            RegistryHelper.RegistrySetString("Last Checked", "");
            if (this.IsActive)
                UpdateHeaderServerSide("Temperatur", expander.Header.ToString());
        }

        private void UpdateHeaderServerSide(string name, string vale)
        {
            Connector.SendHeaderCommand(name, vale);
        }
        #endregion 
    }
}
