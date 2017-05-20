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
            UserControlWelcome userControlWelcome = new UserControlWelcome();
            this.ContentControl.Content = userControlWelcome;
        }

        public void ExitProgram()
        {
            Application.Current.Shutdown();
        }

        private void AlgoTradingWindow_OnClosing(object sender, CancelEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you want to exit?", "Exit Algo-Trading", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
                ExitProgram();
            else
                e.Cancel = true;
        }

        private void buttonInformation_Click(object sender, RoutedEventArgs e)
        {
            UserControlInfo userControlInfo = new UserControlInfo();
            this.ContentControl.Content = userControlInfo;
        }

        private void buttonBuy_Click(object sender, RoutedEventArgs e)
        {
            UserControlBuy userControlBuy = new UserControlBuy();
            this.ContentControl.Content = userControlBuy;
        }

        private void buttonSell_Click(object sender, RoutedEventArgs e)
        {
            UserControlSell userControlSell = new UserControlSell();
            this.ContentControl.Content = userControlSell;
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            UserControlQuery userControlQuery = new UserControlQuery();
            this.ContentControl.Content = userControlQuery;
        }

        private void buttonSessionHistory_Click(object sender, RoutedEventArgs e)
        {
            UserControlSessionHistory userControlSessionHistory = new UserControlSessionHistory();
            this.ContentControl.Content = userControlSessionHistory;
        }

        private void buttonActiveRequests_Click(object sender, RoutedEventArgs e)
        {
            UserControlActiveRequests userControlActiveRequests = new UserControlActiveRequests();
            this.ContentControl.Content = userControlActiveRequests;
        }

        private void buttonAMA_Click(object sender, RoutedEventArgs e)
        {
            UserControlAMA userControlAma = new UserControlAMA();
            this.ContentControl.Content = userControlAma;
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            UserControlMarketStatus userControlMarketStatus = new UserControlMarketStatus();
            this.ContentControl.Content = userControlMarketStatus;
        }
    }
}
