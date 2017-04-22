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
            updateInformation();
        }

        private async void updateInformation()
        {
            global::Program.MarketClient marketClient = new global::Program.MarketClient();
            while (true)
            {
                MarketUserData marketUserData = (MarketUserData)marketClient.SendQueryUserRequest();
                await Task.Delay(2000);
                this.textBoxInfo.Text = marketUserData.ToString();
            }
        }

    }
}
