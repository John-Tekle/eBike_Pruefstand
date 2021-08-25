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

namespace Client_eBike_Pruefstand
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StartWin startwin = new StartWin();

        public MainWindow()
        {
            startwin.Show();
            InitializeComponent();
            startwin.Close();
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
            startwin.Close();
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

        }
    }
}
