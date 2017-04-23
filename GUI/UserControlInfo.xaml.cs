using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Program;
using Program = Program.Program;

namespace GUI
{
    /// <summary>
    /// Interaction logic for UserControlInfo.xaml
    /// </summary>
    public partial class UserControlInfo : UserControl
    {
        public UserControlInfo()
        {
            InitializeComponent();
            UpdateInformation();
        }

        private void UpdateInformation()
        {
            PresentData();
        }

        private void PresentData()
        {
            global::Program.MarketClient marketClient = new global::Program.MarketClient();
            MarketUserData marketUserData = (MarketUserData)marketClient.SendQueryUserRequest();

            try
            {
                // Set all to visible
                this.DataGridCommodities.Visibility = Visibility.Visible;
                this.DataGridRequests.Visibility = Visibility.Visible;
                this.LabelCommodities.Visibility = Visibility.Visible;
                this.LabelRequests.Visibility = Visibility.Visible;
                this.LabelFunds.Visibility = Visibility.Visible;

                // Set the commodities section
                this.DataGridCommodities.ItemsSource = marketUserData.commodities;
                if (DataGridCommodities.Columns.Count > 1)
                {
                    this.DataGridCommodities.Columns[0].Header = "Commodity";
                    this.DataGridCommodities.Columns[1].Header = "Quantity";
                }

                // Set the requests section
                this.LabelRequests.Content = "Requests";
                if (marketUserData.requests.Length > 0)
                {
                    this.DataGridRequests.Visibility = Visibility.Visible;
                    this.DataGridRequests.ItemsSource = marketUserData.requests
                        .Select((x, index) => new {Number = index + 1, Request = x})
                        .ToList();
                }
                else
                {
                    this.DataGridRequests.Visibility = Visibility.Hidden;
                    this.LabelRequests.Content += "\n\n No requests.";
                }

                this.LabelHeader.Content = "Refreshed: " + DateTime.Now.ToLongTimeString();

                // Set the funds section
                this.LabelFunds.Content = "Funds: " + marketUserData.funds + " $";
            }
            catch
            {
                // Set all to hidden
                this.DataGridCommodities.Visibility = Visibility.Hidden;
                this.DataGridRequests.Visibility = Visibility.Hidden;
                this.LabelCommodities.Visibility = Visibility.Hidden;
                this.LabelRequests.Visibility = Visibility.Hidden;
                this.LabelFunds.Visibility = Visibility.Hidden;
                this.LabelHeader.Content = "No connection to server. Refreshed: " + DateTime.Now.ToLongTimeString();
            }
        }

    }
}
