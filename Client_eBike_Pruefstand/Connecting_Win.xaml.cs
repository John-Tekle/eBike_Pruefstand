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
using LoadingControl.Control;

namespace Client_eBike_Pruefstand
{
    /// <summary>
    /// Interaction logic for StartWin.xaml
    /// </summary>
    public partial class Connecting_Win : Window
    {
        public Connecting_Win()
        {
            InitializeComponent();
        }

        private void MoveOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
