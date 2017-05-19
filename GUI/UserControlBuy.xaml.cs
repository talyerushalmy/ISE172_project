using System;
using System.CodeDom;
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

namespace GUI
{
    /// <summary>
    /// Interaction logic for UserControlBuy.xaml
    /// </summary>
    public partial class UserControlBuy : UserControl
    {
        public UserControlBuy()
        {
            InitializeComponent();
        }

        private void buttonBuy_Click(object sender, RoutedEventArgs e)
        {
            int commodityId;
            int quantity;
            int price;

            // Harvest data and make sure it's legal
            try
            {
                commodityId = this.comboBoxCommodity.SelectedIndex;
                if (commodityId == -1)
                {
                    MessageBoxResult popup = MessageBox.Show("Please select the commodity ID", "Confirmation");
                    return;
                }
            }
            catch
            {
                MessageBoxResult popup = MessageBox.Show("Please select the commodity ID", "Confirmation");
                return;
            }

            try
            {
                quantity = Convert.ToInt32(this.textBoxQuantity.Text);
                if (quantity <= 0)
                {
                    MessageBoxResult popup = MessageBox.Show("Please enter a positive quantity number", "Confirmation");
                    return;
                }
            }
            catch
            {
                MessageBoxResult popup = MessageBox.Show("Please enter a valid quantity number", "Confirmation");
                return;
            }

            try
            {
                price = Convert.ToInt32(this.textBoxPrice.Text);
                if (price <= 0)
                {
                    MessageBoxResult popup = MessageBox.Show("Please enter a positive price number", "Confirmation");
                    return;
                }
            }
            catch
            {
                MessageBoxResult popup = MessageBox.Show("Please enter a valid price number", "Confirmation");
                return;
            }

            // If we got here, all the data is okay and we can send the request
            global::Program.MarketClient marketClient = new global::Program.MarketClient();
            int id = marketClient.SendBuyRequest(price, commodityId, quantity);

            if (id == -1)
            {
                MessageBoxResult popup = MessageBox.Show("An error occured while processing the request", "Confirmation");
            }
            else
            {
                MessageBoxResult popup = MessageBox.Show("Request sent! ID: " + id, "Confirmation");
            }

            this.comboBoxCommodity.SelectedIndex = -1;
            this.textBoxQuantity.Text = "";
            this.textBoxPrice.Text = "";
        }
    }
}
