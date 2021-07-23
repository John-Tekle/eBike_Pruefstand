using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Service_eBike_Pruefstand
{
    public partial class eBIKE : Form
    {
        #region members
        private Raspberry PI { get; }
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        String exMessage = string.Empty;
        #endregion

        public eBIKE()
        {
            InitializeComponent();
            try
            {
                PI = new Raspberry();
                this.Shown += new System.EventHandler(this.RunOnShown);
            }
            catch (Exception e)
            {
                this.Shown += new System.EventHandler(this.Ebike_Shown);
                exMessage = e.Message;
                log.Error(e.Message);
            }
        }

        private void RunOnShown(object sender, EventArgs e)
        {
            Cursor.Hide(); //Courser hide on mono doesen't work as expected
            //Start new Thread
            Thread t = new Thread(PI_Run);
            t.IsBackground = true;
            t.Start();
        }

        private void Ebike_Shown(object sender, EventArgs e)
        {
            Invoke(new Action(() => MessagesText.Text = exMessage));
            MessageBox.Show(exMessage);
        }

        private void PI_Run()
        {
            myTimer.Tick += new EventHandler(TimerEventProcessor);
            myTimer.Interval = 5000;
            myTimer.Enabled = true;
            while (true)
            {
                try
                {
                    if(PI.Run())
                    {
                        Invoke(new Action(() => label_Temp.Text = $"{PI.GetTeValue.ToString()}°C"));
                        Invoke(new Action(() => label_Gewicht.Text = $"{PI.GetGeValue.ToString()}Kg"));
                        Invoke(new Action(() => label_Anemo.Text = $"{PI.GetAnValue.ToString()}m/s"));
                        Invoke(new Action(() => label_Luef.Text = $"{PI.GetLuValue.ToString()}%"));
                    }
                    else
                    {
                        Invoke(new Action(() => MessagesText.Text = "Something went wrong!"));
                        myTimer.Start();
                    }
                }
                catch(Exception e)
                {
                    myTimer.Start();
                    Invoke(new Action(() => MessagesText.Text = e.Message));
                }
                finally
                {
                    Thread.Sleep(1000);
                }
                
            }
        }

        private void TimerEventProcessor(object sender, EventArgs e)
        {
            Invoke(new Action(() => MessagesText.Clear()));
            myTimer.Stop();
            myTimer.Enabled = true;
        }
    }
}
