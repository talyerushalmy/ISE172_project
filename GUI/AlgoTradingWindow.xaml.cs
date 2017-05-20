using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
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
using AutoMarketAgent = Program.AutoMarketAgent;

namespace GUI
{
    /// <summary>
    /// Interaction logic for AlgoTradingWindow.xaml
    /// </summary>
    public partial class AlgoTradingWindow : Window
    {
        private bool AmaWorking;
        private Thread AmaThread;

        public AlgoTradingWindow()
        {
            InitializeComponent();
            InitAma();
            UserControlWelcome userControlWelcome = new UserControlWelcome();
            this.ContentControl.Content = userControlWelcome;
        }

        private void InitAma()
        {
            AmaThread = new Thread(new ThreadStart(CallAma));
            this.buttonAMA.Background = Brushes.Red;
            this.AmaWorking = false;
        }

        private void CallAma()
        {
            try
            {
                Program.AutoMarketAgent ama = new Program.AutoMarketAgent();
                ama.autoPilot();
                this.AmaWorking = false;
            }
            catch
            {
                if (!this.AmaWorking)
                {
                    MessageBoxResult popup = MessageBox.Show("The Auto Agent has failed", "Automatic Market Agent");
                }
            }
            finally
            {
                ToggleAMA();
            }
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
            this.AmaWorking = !this.AmaWorking;
            ToggleAMA();
        }

        private void ToggleAMA()
        {
            if (AmaWorking)
            {
                AmaThread.Start();
                this.buttonAMA.Background = Brushes.Green;
                MessageBoxResult popup = MessageBox.Show("The Auto Agent has started working", "Automatic Market Agent");
            }
            else
            {
                AmaThread.Abort();
                this.buttonAMA.Background = Brushes.Red;
                MessageBoxResult popup = MessageBox.Show("The Auto Agent has stopped working", "Automatic Market Agent");
            }
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            UserControlMarketStatus userControlMarketStatus = new UserControlMarketStatus();
            this.ContentControl.Content = userControlMarketStatus;
        }
    }
}
