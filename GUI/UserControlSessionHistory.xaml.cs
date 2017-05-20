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
    /// Interaction logic for UserControlSessionHistory.xaml
    /// </summary>
    public partial class UserControlSessionHistory : UserControl
    {
        public UserControlSessionHistory()
        {
            InitializeComponent();
            PopulateGrid();
        }

        private void PopulateGrid()
        {
            try
            {
                this.labelHeader.Content = "Session history as of " + DateTime.Now.ToLongTimeString();
                var history = HistoryTable.getHistoryList();
                this.dataGridHistory.ItemsSource = history;
            }
            catch
            {
                this.labelHeader.Content = "Could not fetch history. Refreshed: " + DateTime.Now.ToLongTimeString();
            }
        }
    }
}
