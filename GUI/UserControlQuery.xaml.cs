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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Program;

namespace GUI
{
    /// <summary>
    /// Interaction logic for UserControlQuery.xaml
    /// </summary>
    public partial class UserControlQuery : UserControl
    {
        public UserControlQuery()
        {
            InitializeComponent();
        }

        private void comboBoxType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.comboBoxType.SelectedIndex != -1)
            {
                switch (((ComboBoxItem)this.comboBoxType.SelectedItem).Content.ToString())
                {
                    case "Buy Request":
                    case "Sell Request":
                        this.labelInsert.Content = "Request ID:";
                        break;
                    case "Commodity":
                        this.labelInsert.Content = "Commodity ID:";
                        break;
                    default:
                        this.labelInsert.Content = "Select type";
                        break;
                }
                this.textBoxInput.IsEnabled = true;
                this.buttonSearch.IsEnabled = true;
            }
            else
            {
                this.labelInsert.Content = "Select type";
                this.textBoxInput.IsEnabled = false;
                this.buttonSearch.IsEnabled = false;
            }
        }

        private void BuySell(int input)
        {
            global::Program.MarketClient marketClient = new global::Program.MarketClient();
            try
            {
                MarketItemQuery resp = (MarketItemQuery) marketClient.SendQueryBuySellRequest(input);
                this.textBoxOutput.Text = resp.ToString();
                this.textBoxOutput.Visibility = Visibility.Visible;
            }
            catch
            {
                MessageBoxResult popup = MessageBox.Show("Could not fetch info about trade #" + input, "Confirmation");
            }
        }

        private void Commodity(int input)
        {
            global::Program.MarketClient marketClient = new global::Program.MarketClient();
            try
            {
                MarketCommodityOffer respCommodityOffer = (MarketCommodityOffer)marketClient.SendQueryMarketRequest(input);
                this.textBoxOutput.Text = respCommodityOffer.ToString();
                this.textBoxOutput.Visibility = Visibility.Visible;
            }
            catch
            {
                MessageBoxResult popup = MessageBox.Show("Could not fetch info about commodity #" + input, "Confirmation");
            }
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                global::Program.MarketClient marketClient = new global::Program.MarketClient();
                int input = Convert.ToInt32(this.textBoxInput.Text);
                switch (((ComboBoxItem)this.comboBoxType.SelectedItem).Content.ToString())
                {
                    case "Buy Request":
                    case "Sell Request":
                        BuySell(input);
                        break;
                    case "Commodity":
                        Commodity(input);
                        break;
                    default:
                        MessageBoxResult popup = MessageBox.Show("Please make sure your form is valid", "Confirmation");
                        return;
                }
            }
            catch
            {
                MessageBoxResult popup = MessageBox.Show("Please enter a valid number", "Confirmation");
                this.textBoxInput.Text = "";
            }
        }
    }
}
