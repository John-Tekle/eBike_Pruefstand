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
using System.Windows.Shapes;
using System.Windows.Threading;
using LoadingControl.Control;

namespace Client_eBike_Pruefstand
{
    /// <summary>
    /// Interaction logic for StartWin.xaml
    /// </summary>
    public partial class Connecting_Win : Window
    {
        private static MainWindow MainWindow;
        private DispatcherTimer dispatcherTimer;
        private static Thread t;

        public Connecting_Win()
        {
            InitializeComponent();
            try
            {
                MainWindow = new MainWindow();
                dispatcherTimer = new DispatcherTimer();
                this.Activated += new EventHandler(this.RunOnShown);
                TCP_Client.Initialization();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }

        private void RunOnShown(object sender, EventArgs e)
        {
            t = new Thread(RUN_MainWindow);
            t.Start();
        }

        private void RUN_MainWindow()
        {
            dispatcherTimer.Tick += new EventHandler(dispatcherdispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start();
        }

        private void dispatcherdispatcherTimer_Tick(object sender, EventArgs e)
        {
            //TCP_Client.Connect();
            MainWindow.Show();
            while (!MainWindow.ShowActivated) ;
            this.Close();
        }

        private void MoveOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
