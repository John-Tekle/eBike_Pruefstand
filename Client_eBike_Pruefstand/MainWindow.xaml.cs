using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        Connecting_Win startwin;// = new Connecting_Win();
        Einstellung_Win einstellung_Win;
        private CheckBox currentcheckbox;
        #endregion

        public MainWindow()
        {
            //startwin.Show();
            //while (!startwin.ShowActivated) ;
            InitializeComponent();
            //startwin.Close();
            checkBoxes = new List<CheckBox>() { checkBox0, checkBox1, checkBox2, checkBox3, checkBox4, checkBox5 };
            Einstellung_Win.EinstellungChanged += Einstellung_Win_EinstellungChanged;
            einstellung_Win = new Einstellung_Win();

            #region RegistryHelper
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
            #endregion
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

        #region Move WinApp using Mouse
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        #endregion

        #region Log_Ellipse
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(sender == BTemperatur)
            {
                if (ElTemperatur.Visibility == Visibility.Visible)
                    ElTemperatur.Visibility = Visibility.Hidden;
                else if (ElTemperatur.Visibility == Visibility.Hidden)
                    ElTemperatur.Visibility = Visibility.Visible;
            }
            else if(sender == BGewicht)
            {
                if (ElGewicht.Visibility == Visibility.Visible)
                    ElGewicht.Visibility = Visibility.Hidden;
                else if (ElGewicht.Visibility == Visibility.Hidden)
                    ElGewicht.Visibility = Visibility.Visible;

            }
            else if (sender == BAnemometer)
            {
                if (ElAnemometer.Visibility == Visibility.Visible)
                    ElAnemometer.Visibility = Visibility.Hidden;
                else if (ElAnemometer.Visibility == Visibility.Hidden)
                    ElAnemometer.Visibility = Visibility.Visible;

            }
            else if (sender == BLuefter)
            {
                if (ElLuefter.Visibility == Visibility.Visible)
                    ElLuefter.Visibility = Visibility.Hidden;
                else if (ElLuefter.Visibility == Visibility.Hidden)
                    ElLuefter.Visibility = Visibility.Visible;

            }

            e.Handled = true; //Event marked as handled

        }
        #endregion

        #region Ellipse Close & Mini
        private void Windows_Close(object sender, MouseButtonEventArgs e)
        {
            //startwin.Close();
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

        private void Click_Einstellung(object sender, RoutedEventArgs e)
        {
            einstellung_Win.ShowDialog();
        }

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
    }
}
