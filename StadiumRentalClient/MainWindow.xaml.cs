using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MongoDB.Driver;

namespace StadiumRentalClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string? connectionString = "I ain't putting the connection string into a repo";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            ConnectionStringPopup.IsOpen = true;
        }
        public static void Get_Key()
        {
            connectionString = ConnectionStringDialog.GetConnectionString();
        }
        public static bool Test_Key()
        {
            MongoClient dbClient;
            try
            {
                dbClient = new MongoClient(connectionString);
            }
            catch (MongoConfigurationException ex)
            {
                return false;
            }
            catch (MongoAuthenticationException ex)
            {
                return false;
            }

            return true;
        }
    }
}