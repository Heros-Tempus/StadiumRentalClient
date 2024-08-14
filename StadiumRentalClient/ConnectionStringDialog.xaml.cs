using MongoDB.Driver.Core.Configuration;
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

namespace StadiumRentalClient
{
    /// <summary>
    /// Interaction logic for ConnectionStringDialog.xaml
    /// </summary>
    public partial class ConnectionStringDialog : UserControl
    {
        private static string? connectionString;
        public ConnectionStringDialog()
        {
            InitializeComponent();
        }

        private void Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Connection_String_Textbox.Text != "")
            {
                connectionString = Connection_String_Textbox.Text;
            }
            MainWindow.Get_Key();

            if (MainWindow.Test_Key())
                Visibility = Visibility.Collapsed;
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
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
