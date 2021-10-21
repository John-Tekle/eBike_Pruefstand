using System;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;

namespace Service_eBike_Pruefstand
{
    public partial class eBIKE : Form
    {
        #region members
        private Raspberry PI { get; }
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private static readonly System.Windows.Forms.Timer clearMessageTimer = new System.Windows.Forms.Timer();
        private static readonly System.Windows.Forms.Timer headerLogTimer = new System.Windows.Forms.Timer();
        private static bool[] logState = new bool[4];
        private static bool logStateColor = true;
        private string exMessage = string.Empty;
        #endregion

        public eBIKE()
        {
            InitializeComponent();
            try
            {
                PI = new Raspberry();
                PI.CommandToGUI += PI_CommandToGUI; ;
                this.Shown += new System.EventHandler(this.RunOnShown);
            }
            catch (Exception e)
            {
                this.Shown += new System.EventHandler(this.Ebike_Shown);
                exMessage = e.Message;
                log.Error(e.Message);
            }
            
            //Blink every 1s
            headerLogTimer.Tick += new EventHandler(HeaderLogTimerBlink);
            headerLogTimer.Interval = 1000;
            headerLogTimer.Enabled = true;
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
            clearMessageTimer.Tick += new EventHandler(TimerEventProcessor);
            clearMessageTimer.Interval = 5000;
            clearMessageTimer.Enabled = true;
            while (true)
            {
                try
                {
                    if(PI.Run())
                    {
                        Invoke(new Action(() => label_Temp.Text = $"{PI.GetTeValue.ToString("0.00")}°C"));
                        Invoke(new Action(() => label_Gewicht.Text = $"{PI.GetGeValue.ToString("0.00")}Kg"));
                        Invoke(new Action(() => label_Anemo.Text = $"{PI.GetAnValue.ToString("0.00")}m/s"));
                        //Invoke(new Action(() => label_Luef.Text = $"{PI.GetLuValue.ToString()}%"));
                    }
                    else
                    {
                        Invoke(new Action(() => MessagesText.Text = "Something went wrong!"));
                        clearMessageTimer.Start();
                    }
                }
                catch(Exception e)
                {
                    clearMessageTimer.Start();
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
            clearMessageTimer.Stop();
            clearMessageTimer.Enabled = true;
        }

        private void PI_CommandToGUI(object sender, Dictionary<string, object> keyValuePairs)
        {
            foreach (KeyValuePair<string, object> _keyValuePairs in keyValuePairs)
            {
                if (_keyValuePairs.Key.Substring(0, 6) == "Logger")
                {
                    switch (_keyValuePairs.Key)
                    {
                        case "Logger+Temperature: ":
                            UpdateCommandToGUI(0, "Temperature", _keyValuePairs, Temperatur);
                            break;
                        case "Logger+Gewicht: ":
                            UpdateCommandToGUI(1, "Gewicht", _keyValuePairs, Gewicht);
                            break;
                        case "Logger+Anemometer: ":
                            UpdateCommandToGUI(2, "Anemometer", _keyValuePairs, Anemometer);
                            break;
                        case "Logger+Luefter: ":
                            UpdateCommandToGUI(3, "Luefter", _keyValuePairs, Lufter);
                            break;
                        default:
                            break;
                    }
                    headerLogTimer.Start();
                }
                else if (_keyValuePairs.Key.Substring(0, 10) == "Temperatur")
                {
                    _ = Invoke(new Action(() => Temperatur.Text = $"Temperatur: {_keyValuePairs.Value}"));
                }
            }
        }

        private void UpdateCommandToGUI(int i, string name, KeyValuePair<string, object> _keyValuePairs, GroupBox groupBox)
        {
            bool logStateValue = bool.Parse(_keyValuePairs.Value.ToString());

            if (logStateValue)
            {
                logState[i] = true;
                PI.SendCommand(name, true);
            }
            else if (!logStateValue)
            {
                logState[i] = false;
                _ = Invoke(new Action(() => groupBox.ForeColor = System.Drawing.Color.White));
                PI.SendCommand(name, false);
            }

            try
            {
                PI.keyValuePairsUpdate[name] = logStateValue;
                //keyValuePairs.Key.Substring(7, (int)(keyValuePairs.Key.Length - 2))
            }
            catch (Exception e)
            {
                log.Error("e" + e.Message);
            }
            
        }

        private void HeaderLogTimerBlink(object sender, EventArgs e)
        {
            headerLogTimer.Stop();
            if (logStateColor)
            {
                UpdateHeaderColor(System.Drawing.Color.Red);
                logStateColor = false;
            }
            else if(!logStateColor)
            {
                UpdateHeaderColor(System.Drawing.Color.White);
                logStateColor = true;
            }
            headerLogTimer.Enabled = true;
            headerLogTimer.Start();
        }

        private void UpdateHeaderColor(System.Drawing.Color color)
        {
            if (logState[0])
                _ = Invoke(new Action(() => Temperatur.ForeColor = color));
            if (logState[1])
                _ = Invoke(new Action(() => Gewicht.ForeColor = color));
            if (logState[2])
                _ = Invoke(new Action(() => Anemometer.ForeColor = color));
            if (logState[3])
                _ = Invoke(new Action(() => Lufter.ForeColor = color));
        }

    }
}
