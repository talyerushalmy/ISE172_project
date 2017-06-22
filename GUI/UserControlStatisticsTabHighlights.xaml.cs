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
    /// Interaction logic for UserControlStatisticsTabHighlights.xaml
    /// </summary>
    public partial class UserControlStatisticsTabHighlights : UserControl
    {
        public UserControlStatisticsTabHighlights()
        {
            InitializeComponent();
            PopulateData("HOUR");
        }

        private void PopulateData(string TIME_WINDOW)
        {
            try
            {
                List<String> biggestPrice = DatabaseSocket.GetHighlightsBiggestPrice(TIME_WINDOW);
                List<String> mostSoldComm = DatabaseSocket.GetHighlightsMostSoldComm(TIME_WINDOW);
                List<String> leastSoldComm = DatabaseSocket.GetHighlightsLeastSoldComm(TIME_WINDOW);
                List<String> bestSale = DatabaseSocket.GetHighlightsBestSale(TIME_WINDOW);
                List<String> worstSale = DatabaseSocket.GetHighlightsWorstSale(TIME_WINDOW);

                this.labelBiggestPrice.Content = "Biggest price: " + biggestPrice[0] + " of commodity " + biggestPrice[1] + " at " + biggestPrice[2];
                this.labelMostSoldComm.Content = "Most sold commodity: " + mostSoldComm[0] + " with " + mostSoldComm[1] + " sold";
                this.labelLeastSoldComm.Content = "Least sold commodity: " + leastSoldComm[0] + " with " + leastSoldComm[1] + " sold";
                this.labelBestSale.Content = "Best sale: " + bestSale[0] + "$ (Amount: " + bestSale[1] + ", Price: " + bestSale[2] + "), of commodity " + bestSale[3];
                this.labelWorstSale.Content = "Worst sale: " + worstSale[0] + "$ (Amount: " + worstSale[1] + ", Price: " + worstSale[2] + "), of commodity " + worstSale[3];
            }
            catch
            {
                
            }
        }

        private void comboBoxTimeWindow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.comboBoxTimeWindow.SelectedIndex != -1)
                {
                    switch (((ComboBoxItem)this.comboBoxTimeWindow.SelectedItem).Content.ToString())
                    {
                        case "Last day":
                            PopulateData("DAY");
                            break;
                        case "Last week":
                            PopulateData("WEEK");
                            break;
                        case "Always":
                            PopulateData("ALWAYS");
                            break;
                        case "Last hour":
                        default:
                            PopulateData("HOUR");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
