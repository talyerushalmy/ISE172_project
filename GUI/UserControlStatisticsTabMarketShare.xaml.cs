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
using LiveCharts;
using LiveCharts.Wpf;
using Program;

namespace GUI
{
    /// <summary>
    /// Interaction logic for UserControlStatisticsTabMarketShare.xaml
    /// </summary>
    public partial class UserControlStatisticsTabMarketShare : UserControl
    {
        public UserControlStatisticsTabMarketShare()
        {
            InitializeComponent();
            this.MarketShareCollection = new SeriesCollection();
            PopulateChart(DatabaseSocket.GetMarketShareOfLastHour());
        }

        private void PopulateChart(int[,] data)
        {
            this.MarketShareCollection.Clear();
            for (int i = 0; i < data.GetLength(0); i++)
            {
                ChartValues<int> value = new ChartValues<int>();
                value.Add(data[i, 1]);
                PieSeries pieSeries = new PieSeries();
                pieSeries.Title = "" + data[i, 0];
                pieSeries.Values = value;
                pieSeries.DataLabels = true;
                pieSeries.LabelPoint = (point) =>
                {
                    return point.Y != 0 ? string.Format("{0} ({1:P})", point.Y, point.Participation) : "";
                };
                MarketShareCollection.Add(pieSeries);
                DataContext = this;
            }
        }

        public SeriesCollection MarketShareCollection { get; set; }

        private void comboBoxRange_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.comboBoxRange.SelectedIndex != -1)
                {
                    switch (((ComboBoxItem)this.comboBoxRange.SelectedItem).Content.ToString())
                    {
                        case "Between...":
                            this.buttonSearch.Visibility = Visibility.Visible;
                            this.comboBoxInputNumber.Visibility = Visibility.Hidden;
                            this.DatePickerFrom.Visibility = Visibility.Visible;
                            this.DatePickerTo.Visibility = Visibility.Visible;
                            break;
                        case "Last ... trades":
                            this.buttonSearch.Visibility = Visibility.Hidden;
                            this.comboBoxInputNumber.Visibility = Visibility.Visible;
                            this.DatePickerFrom.Visibility = Visibility.Hidden;
                            this.DatePickerTo.Visibility = Visibility.Hidden;
                            PopulateChart(DatabaseSocket.getMarketShare(5000));
                            break;
                        case "Last week":
                            this.buttonSearch.Visibility = Visibility.Hidden;
                            this.comboBoxInputNumber.Visibility = Visibility.Hidden;
                            this.DatePickerFrom.Visibility = Visibility.Hidden;
                            this.DatePickerTo.Visibility = Visibility.Hidden;
                            PopulateChart(DatabaseSocket.GetMarketShareOfLastWeek());
                            break;
                        case "Last day":
                            this.buttonSearch.Visibility = Visibility.Hidden;
                            this.comboBoxInputNumber.Visibility = Visibility.Hidden;
                            this.DatePickerFrom.Visibility = Visibility.Hidden;
                            this.DatePickerTo.Visibility = Visibility.Hidden;
                            PopulateChart(DatabaseSocket.GetMarketShareOfLastDay());
                            break;
                        case "Last hour":
                        default:
                            this.buttonSearch.Visibility = Visibility.Hidden;
                            this.comboBoxInputNumber.Visibility = Visibility.Hidden;
                            this.DatePickerFrom.Visibility = Visibility.Hidden;
                            this.DatePickerTo.Visibility = Visibility.Hidden;
                            PopulateChart(DatabaseSocket.GetMarketShareOfLastHour());
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime? from = this.DatePickerFrom.SelectedDate;
                DateTime? to = this.DatePickerTo.SelectedDate;
                if (!from.HasValue || !to.HasValue)
                    throw new ArgumentException();
                if (from.Value.CompareTo(to.Value) < 0)
                {
                    PopulateChart(DatabaseSocket.getMarketShareBetweenDates(from.Value, to.Value));
                }
                else
                    throw new ArgumentException();
            }
            catch
            {
                MessageBox.Show("Please pick dates where the left one is earlier than the right one.", "Illegal Input!");
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.comboBoxInputNumber.SelectedIndex != -1)
                {
                    int n = Convert.ToInt32(((ComboBoxItem)this.comboBoxInputNumber.SelectedItem).Content.ToString());
                    PopulateChart(DatabaseSocket.getMarketShare(n));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
