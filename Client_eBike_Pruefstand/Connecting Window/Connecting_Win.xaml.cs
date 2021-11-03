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
        private Thread thread;
        private static bool[] state;

        public Connecting_Win()
        {
            InitializeComponent();
            try
            {
                MainWindow = new MainWindow();
                this.Activated += new EventHandler(this.RunOnShown); //Occurs when a window becomes the foreground window
                TCP_Client.Initialization(MainWindow.einstellung_Win.IPAddress);
                //MainWindow.ShowActivated = false;
                state = new bool[2];
                Dispatcher.Invoke(() => { MainWindow.ShowActivated = false; });
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }

        private void RunOnShown(object sender, EventArgs e)
        {
            thread = new Thread(RUN)
            {
                IsBackground = false
            };
            thread.Start();
        }

        //[Obsolete]
        private void RUN()
        {
            bool stopThread = true;
            string exceptionMessage = string.Empty;
            while (stopThread)
            {
                Thread.Sleep(5000);
                try
                {
                    SSHPortForwarding.StartPortForwarding();
                    TCP_Client.Connect();
                }
                catch (Exception ex)
                {
                    exceptionMessage = ex.Message;
                }
                finally
                {
                    this.Dispatcher.Invoke(() => { state[0] = MainWindow.ShowActivated; });
                    if (!state[0])
                    {
                        if(exceptionMessage != string.Empty)
                            MessageBox.Show(exceptionMessage);
                        this.Dispatcher.Invoke(() => { this.Close(); });
                        MainWindow.Dispatcher.Invoke(() => { MainWindow.Show(); });
                        MainWindow.Dispatcher.Invoke(() => { MainWindow.ShowActivated = true; });
                    }
                    if (TCP_Client.Connected)
                    {
                        stopThread = false;
                    }
                        this.Dispatcher.Invoke(() => { state[1] = this.ShowActivated; });
                        if (!state[1])
                        this.Dispatcher.Invoke(() => { this.Close(); });
                }
            }
            thread.Abort();
            
        }

        private void MoveOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
