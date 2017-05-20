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
    /// Interaction logic for UserControlMarketStatus.xaml
    /// </summary>
    public partial class UserControlMarketStatus : UserControl
    {
        public UserControlMarketStatus()
        {
            InitializeComponent();
            GenerateColumns();
            PopulateGrid();
        }

        private void GenerateColumns()
        {
            DataGridTextColumn c1 = new DataGridTextColumn();
            c1.Header = "Commodity ID";
            c1.Binding = new Binding("id");
            c1.Width = 90;
            this.dataGridData.Columns.Add(c1);
            DataGridTextColumn c2 = new DataGridTextColumn();
            c2.Header = "Ask";
            c2.Binding = new Binding("ask");
            c2.Width = 90;
            this.dataGridData.Columns.Add(c2);
            DataGridTextColumn c3 = new DataGridTextColumn();
            c3.Header = "Bid";
            c3.Binding = new Binding("bid");
            c3.Width = 90;
            this.dataGridData.Columns.Add(c3);
        }

        private class GridItem
        {
            public int id { get; set; }
            public int ask { get; set; }
            public int bid { get; set; }
        }

        private void PopulateGrid()
        {
            try
            {
                global::Program.MarketClient marketClient = new global::Program.MarketClient();
                Commodity[] commodities = marketClient.sendQueryAllMarketRequest();

                this.dataGridData.Items.Clear();
                foreach (var comm in commodities)
                {
                    this.dataGridData.Items.Add(new GridItem()
                    {
                        id = comm.id,
                        ask = comm.info.ask,
                        bid = comm.info.bid
                    });
                }

                this.labelHeader.Content = "Market status as of " + DateTime.Now.ToLongTimeString();
                this.dataGridData.Visibility = Visibility.Visible;
            }
            catch
            {
                this.labelHeader.Content = "Could not fetch data. Refreshed: " + DateTime.Now.ToLongTimeString();
                this.dataGridData.Visibility = Visibility.Hidden;
            }
        }
    }
}
