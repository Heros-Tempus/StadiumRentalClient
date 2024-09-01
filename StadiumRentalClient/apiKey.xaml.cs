using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StadiumRentalClient
{
    /// <summary>
    /// Interaction logic for apiKeyDialog.xaml
    /// </summary>
    public partial class apiKeyDialog : UserControl
    {
        private static string? connectionString;
        public apiKeyDialog()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (txtAPIKey.Text != "")
            {
                connectionString = txtAPIKey.Text;
            }
            //MainWindow.Get_Key();

            //if (MainWindow.Test_Key())
                //Visibility = Visibility.Collapsed;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }
        public static string GetConnectionString()
        {
            if (connectionString != null)
                return connectionString;
            else
                return "null";
        }
    }
}
