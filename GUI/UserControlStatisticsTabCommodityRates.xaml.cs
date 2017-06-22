using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for UserControlStatisticsTabCommodityRates.xaml
    /// </summary>
    public partial class UserControlStatisticsTabCommodityRates : UserControl
    {
        private int _commodityToCheck;

        public UserControlStatisticsTabCommodityRates()
        {
            InitializeComponent();
            this._commodityToCheck = 0;
            this.CommodityRatesCollection = new SeriesCollection();
            PopulateChart(DatabaseSocket.getPriceOfCommByLastHour(_commodityToCheck));
        }

        private void PopulateChart(Transaction[] data)
        {
            this.CommodityRatesCollection.Clear();
            YFormatter = value => value.ToString("C");
            ChartValues<double> prices = new ChartValues<double>();
            Labels = new String[data.GetLength(0)];
            int i = 0;
            foreach (Transaction transaction in data)
            {
                prices.Add(transaction.getPrice());
                Labels[0] = transaction.getTimestamp().ToString();
                i++;
            }
            LineSeries lineSeries = new LineSeries {Title = "Commodity " + this._commodityToCheck, Values = prices};
            this.CommodityRatesCollection.Add(lineSeries);
            DataContext = this;
        }

        public SeriesCollection CommodityRatesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

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
                            PopulateChart(DatabaseSocket.getPriceOfCommByLastNTrades(this._commodityToCheck, 5000));
                            break;
                        case "Last day":
                            this.buttonSearch.Visibility = Visibility.Hidden;
                            this.comboBoxInputNumber.Visibility = Visibility.Hidden;
                            this.DatePickerFrom.Visibility = Visibility.Hidden;
                            this.DatePickerTo.Visibility = Visibility.Hidden;
                            PopulateChart(DatabaseSocket.getPriceOfCommByLastDay(this._commodityToCheck));
                            break;
                        case "Last hour":
                        default:
                            this.buttonSearch.Visibility = Visibility.Hidden;
                            this.comboBoxInputNumber.Visibility = Visibility.Hidden;
                            this.DatePickerFrom.Visibility = Visibility.Hidden;
                            this.DatePickerTo.Visibility = Visibility.Hidden;
                            PopulateChart(DatabaseSocket.getPriceOfCommByLastHour(this._commodityToCheck));
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
                    PopulateChart(DatabaseSocket.getPriceOfCommBetweenDates(this._commodityToCheck, from.Value, to.Value));
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
                    PopulateChart(DatabaseSocket.getPriceOfCommByLastNTrades(this._commodityToCheck, n));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void comboBoxCommID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.comboBoxCommID.SelectedIndex != -1)
                {
                    this._commodityToCheck =
                        Convert.ToInt32(((ComboBoxItem)this.comboBoxCommID.SelectedItem).Content.ToString());
                    comboBoxRange_SelectionChanged(this.comboBoxRange, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
