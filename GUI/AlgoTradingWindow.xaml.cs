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
        private bool _amaWorking;
        private Thread _amaThread;

        public AlgoTradingWindow()
        {
            InitializeComponent();
            InitAma();
            UserControlWelcome userControlWelcome = new UserControlWelcome();
            this.ContentControl.Content = userControlWelcome;
        }

        private void InitAma()
        {
            this.ButtonAma.Background = Brushes.Red;
            this._amaWorking = false;
        }

        private void buttonAMA_Click(object sender, RoutedEventArgs e)
        {
            this._amaWorking = !this._amaWorking;
            if (_amaWorking)
            {
                SetButtonsAvailability(false);
                TurnAMAOn();
            }
            else
            {
                SetButtonsAvailability(true);
                TurnAMAOff();
            }
        }

        private void TurnAMAOn()
        {
            try
            {
                UserControlAMAWorking userControlAmaWorking = new UserControlAMAWorking();
                this.ContentControl.Content = userControlAmaWorking;
                this.ButtonAma.Background = Brushes.Green;
                MessageBoxResult popup =
                    MessageBox.Show("The Auto Agent has started working", "Automatic Market Agent");
                _amaThread = new Thread(new ThreadStart(CallAma));
                _amaThread.Start();
            }
            catch
            {
                return;
            }
        }

        private void TurnAMAOff()
        {
            try
            {
                UserControlWelcome userControlWelcome = new UserControlWelcome();
                this.ContentControl.Content = userControlWelcome;
                this.ButtonAma.Background = Brushes.Red;
                MessageBoxResult popup = MessageBox.Show("The Auto Agent has stopped working",
                    "Automatic Market Agent");
                _amaThread.Abort();
            }
            catch
            {
                return;
            }
        }

        private void CallAma()
        {
            try
            {
                Program.AutoMarketAgent ama = new Program.AutoMarketAgent();
                ama.autoPilot();
            }
            catch
            {
                if (this._amaWorking)
                {
                    MessageBoxResult popup = MessageBox.Show("The Auto Agent has failed", "Automatic Market Agent");
                }
            }
            finally
            {
                this._amaWorking = false;
            }
        }

        private void SetButtonsAvailability(bool setTo)
        {
            foreach (Object obj in this.MenuGrid.Children)
            {
                if (obj is Button)
                    ((Button)obj).IsEnabled = setTo;
            }
            this.ButtonAma.IsEnabled = true;
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
            UserControlBuySell userControlBuySell = new UserControlBuySell();
            this.ContentControl.Content = userControlBuySell;
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

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            UserControlMarketStatus userControlMarketStatus = new UserControlMarketStatus();
            this.ContentControl.Content = userControlMarketStatus;
        }

        private void ButtonGraphs_Click(object sender, RoutedEventArgs e)
        {
            UserControlGraphs userControlGraphs = new UserControlGraphs();
            this.ContentControl.Content = userControlGraphs;
        }
    }
}
