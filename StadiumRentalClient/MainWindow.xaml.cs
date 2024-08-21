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
            catch (Exception ex)
            {
                //message box: invalid connection string
                return false;
            }

            return true;
        }
    }
    public class Pokemon
    {
        public int Dex_num {  get; set; }
        public string Species {  get; set; }
        public string Type_1 { get; set; }
        public string Type_2 { get; set; }
        public Dictionary<string, string> Moves { get; set; }
        public Dictionary<string, int>? Stats { get; set; }
        private static List<string> Stat_Names = new List<string> { "hp", "atk", "spec", "def", "speed" };
        public Pokemon()
        {
            Dex_num = -1;
            Species = "";
            Type_1 = "";
            Type_2 = "";
            Moves = new Dictionary<string, string>();
            Stats = Stat_Names.ToDictionary(k=>k, k=>0);
        }
        public Pokemon(int dex_num, string species, string type_1, string type_2, Dictionary<string, string> moves)
        {
            Dex_num = dex_num;
            Species = species;
            Type_1 = type_1;
            Type_2 = type_2;
            Moves = moves;
        }
        public Pokemon(int dex_num, string species, string type_1, string type_2, Dictionary<string, string> moves, Dictionary<string, int>? stats) : this(dex_num, species, type_1, type_2, moves)
        {
            Stats = stats;
        }
    }

}