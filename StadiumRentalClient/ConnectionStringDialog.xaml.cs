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
using MongoDB.Bson;
using MongoDB.Driver;

namespace StadiumRentalClient
{
    /// <summary>
    /// Interaction logic for ConnectionStringDialog.xaml
    /// </summary>
    public partial class ConnectionStringDialog : UserControl
    {
        public ConnectionStringDialog()
        {
            InitializeComponent();
        }

        private void Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Connection_String_Textbox.Text != "")
            {
                MongoClient dbClient;
                try
                {
                    //dbClient = new MongoClient(MainWindow.connectionString);
                    //MainWindow.connectionString = Connection_String_Textbox.Text;
                }
                catch (Exception ex)
                {

                }
                IsEnabled = false;
                Visibility = Visibility.Collapsed;
            }
        }

    }
}
