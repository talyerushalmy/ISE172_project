using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace GUI
{
    /// <summary>
    /// Interaction logic for AlgoTradingWindow.xaml
    /// </summary>
    public partial class AlgoTradingWindow : Window
    {
        public AlgoTradingWindow()
        {
            InitializeComponent();
        }

        private void buttonInformation_Click(object sender, RoutedEventArgs e)
        {
            UserControlInfo userControlInfo = new UserControlInfo();
            this.ContentControl.Content = userControlInfo;
        }

        public void ExitProgram()
        {
            Application.Current.Shutdown();
        }

        private void AlgoTradingWindow_OnClosing(object sender, CancelEventArgs e)
        {
            ExitProgram();
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you want to exit?", "Exit Algo-Trading", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
                ExitProgram();
        }
    }
}
