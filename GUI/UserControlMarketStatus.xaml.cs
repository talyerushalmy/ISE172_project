using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
using LiveCharts;
using LiveCharts.Wpf;
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
            PopulateChart();
            //PopulateGrid();
        }

        private void PopulateChart()
        {
            global::Program.MarketClient marketClient = new global::Program.MarketClient();
            Commodity[] commodities = marketClient.SendQueryAllMarketRequest();
            this.CommodityRateCollection = new SeriesCollection();
            ChartValues<double> askPrices = new ChartValues<double>();
            ChartValues<double> bidPrices = new ChartValues<double>();
            foreach (Commodity commodity in commodities)
            {
                askPrices.Add(commodity.info.ask);
                bidPrices.Add(commodity.info.bid);
            }
            ColumnSeries askColumnSeries = new ColumnSeries();
            ColumnSeries bidColumnSeries = new ColumnSeries();
            askColumnSeries.Title = "Ask";
            bidColumnSeries.Title = "Bid";
            askColumnSeries.Values = askPrices;
            bidColumnSeries.Values = bidPrices;
            askColumnSeries.DataLabels = true;
            bidColumnSeries.DataLabels = true;
            askColumnSeries.LabelPoint = point => point.Y.ToString();
            bidColumnSeries.LabelPoint = point => point.Y.ToString();
            CommodityRateCollection.Add(askColumnSeries);
            CommodityRateCollection.Add(bidColumnSeries);
            CommodityRates = d => d.ToString("N");
            DataContext = this;
        }

        public Func<double, string> CommodityRates { get; set; }

        public SeriesCollection CommodityRateCollection { get; set; }

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
                Commodity[] commodities = marketClient.SendQueryAllMarketRequest();

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

        private void radioButtonChart_Checked(object sender, RoutedEventArgs e)
        {
            if (((RadioButton) sender).Content.Equals("Chart"))
            {
                try
                {
                    PopulateChart();
                    this.dataGridData.Visibility = Visibility.Hidden;
                    this.Chart.Visibility = Visibility.Visible;
                    this.labelHeader.Content = "Market status as of " + DateTime.Now.ToLongTimeString();
                }
                catch
                {
                    this.labelHeader.Content = "Could not fetch data. Refreshed: " + DateTime.Now.ToLongTimeString();
                }
            }
            else
            {
                PopulateGrid();
                this.Chart.Visibility = Visibility.Hidden;
                this.dataGridData.Visibility = Visibility.Visible;
            }
        }
    }
}
