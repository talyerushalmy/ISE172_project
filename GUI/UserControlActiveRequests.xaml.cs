using System;
using System.Collections.Generic;
using System.Data;
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
using System.Xml;
using Program;

namespace GUI
{
    /// <summary>
    /// Interaction logic for UserControlActiveRequests.xaml
    /// </summary>
    public partial class UserControlActiveRequests : UserControl
    {
        public UserControlActiveRequests()
        {
            InitializeComponent();
            GenerateColumns();
            PopulateGrid();
        }

        private void GenerateColumns()
        {
            DataGridTextColumn c1 = new DataGridTextColumn();
            c1.Header = "#";
            c1.Binding = new Binding("id");
            c1.Width = 50;
            this.dataGridData.Columns.Add(c1);
            DataGridTextColumn c2 = new DataGridTextColumn();
            c2.Header = "Type";
            c2.Binding = new Binding("type");
            c2.Width = 50;
            this.dataGridData.Columns.Add(c2);
            DataGridTextColumn c3 = new DataGridTextColumn();
            c3.Header = "Price";
            c3.Binding = new Binding("price");
            c3.Width = 50;
            this.dataGridData.Columns.Add(c3);
            DataGridTextColumn c4 = new DataGridTextColumn();
            c4.Header = "Amount";
            c4.Binding = new Binding("amount");
            c4.Width = 60;
            this.dataGridData.Columns.Add(c4);
            DataGridTextColumn c5 = new DataGridTextColumn();
            c5.Header = "Item";
            c5.Binding = new Binding("commodity");
            c5.Width = 40;
            this.dataGridData.Columns.Add(c5);
        }

        private class GridItem
        {
            public int id { get; set; }
            public string type { get; set; }
            public int price { get; set; }
            public int amount { get; set; }
            public int commodity { get; set; }
        }

        private void PopulateGrid()
        {
            try
            {
                global::Program.MarketClient marketClient = new global::Program.MarketClient();
                QueryUserRequest[] resp = marketClient.sendQueryUserRequestsRequest();

                this.dataGridData.Items.Clear();
                foreach (var curr in resp)
                {
                    this.dataGridData.Items.Add(new GridItem()
                    {
                        id = curr.id,
                        type = curr.request.type,
                        price = curr.request.price,
                        amount = curr.request.amount,
                        commodity = curr.request.commodity
                    });
                }

                this.labelHeader.Content = "Active requests as of " + DateTime.Now.ToLongTimeString();
                this.dataGridData.Visibility = Visibility.Visible;
            }
            catch
            {
                this.labelHeader.Content = "Could not fetch data. Refreshed: " + DateTime.Now.ToLongTimeString();
                this.dataGridData.Visibility = Visibility.Hidden;
            }
            finally
            {
                
            }
        }

        private void CancelRequest(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DataGridRow row = GetParent<DataGridRow>((Button) sender);
                    DataGridCell RowColumn = this.dataGridData.Columns[1].GetCellContent(row)?.Parent as DataGridCell;
                    string CellValue = ((TextBlock) RowColumn.Content).Text;
                    int id = Convert.ToInt32(CellValue);

                    // cancel request #id
                    global::Program.MarketClient marketClient = new global::Program.MarketClient();
                    marketClient.SendCancelBuySellRequest(id);
                    // MessageBoxResult popup = MessageBox.Show("Successfuly cancelled request #" + id, "Success");
                }
            }
            catch
            {
                // MessageBoxResult popup = MessageBox.Show("Could not cancel the request", "Error");
            }
            finally
            {
                PopulateGrid();
            }
        }

        private TargetType GetParent<TargetType>(DependencyObject o) where TargetType : DependencyObject
        {
            if (o == null || o is TargetType) return (TargetType)o;
            return GetParent<TargetType>(VisualTreeHelper.GetParent(o));
        }
    }
}
