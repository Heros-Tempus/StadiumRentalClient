using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.IO;
using MongoDB.Driver.Core.Configuration;

namespace StadiumRentalClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Pokemon> dex = new List<Pokemon>();
        Party team = new Party();
        string connectionString;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {

            string ConnectionFilePath = Microsoft.VisualBasic.FileSystem.CurDir() + "\\ConnectionString";
            string PlayerName = Microsoft.VisualBasic.FileSystem.CurDir() + "\\PlayerName";
            if (File.Exists(ConnectionFilePath))
            {
                connectionString = File.ReadAllText(ConnectionFilePath);
                MongoClient dbClient;
                try
                {
                    dbClient = new MongoClient(connectionString);
                    Load_Dex();
                }
                catch (Exception ex)
                {
                    Close();
                }
            }
            else
            {
                Close();
            }

            if (File.Exists(PlayerName)) 
            {
                Player_Name.Text = File.ReadAllText(PlayerName);
                Player_Name.IsEnabled = false;
            }
                //check for locally saved connection string and player name
                //if connection string is not valid then
                //ConnectionStringPopup.IsOpen = true;
                //else lock player name text box begin standard logic

            }

        private void Load_Party()
        {
            //search database for party with matching player name
            //if party is found then set all party slots to the relevent mons
        }
        private void Load_Dex()
        {
            //finds all pokemon in database and populates local list of pokemon objects
            //sorts list based on dex number
            //populates all combo boxes with available choices

            MongoClient dbClient = new MongoClient(connectionString);
            var db = dbClient.GetDatabase("Mons");
            var collection = db.GetCollection<BsonDocument>("LVL30s");
            var mons = collection.Find(new BsonDocument()).ToList();

            foreach (var m in mons) 
            { 
                string name = m.GetElement("Name").ToString().Split("=")[1];
                string cup = m.GetElement("C-Up").ToString().Split("=")[1];
                string cdown;
                string cleft;
                string cright;
                try
                {
                    cdown = m.GetElement("C-Down").ToString().Split("=")[1];
                }
                catch (Exception ex) 
                {
                    cdown = "";
                }
                try
                {
                    cleft = m.GetElement("C-Left").ToString().Split("=")[1];
                }
                catch
                {
                    cleft = "";
                }
                try
                {
                    cright = m.GetElement("C-Right").ToString().Split("=")[1];
                }
                catch
                {
                    cright = "";
                }
                Dictionary<string, string> moves = new Dictionary<string, string>() 
                {
                    {"C-Up", cup},
                    {"C-Down", cdown},
                    {"C-Left", cleft},
                    {"C-Right", cright}
                };
                Pokemon mon = new Pokemon(name, moves);
                dex.Add(mon);
            }
            foreach (var mon in dex)
            {
                CB_Slot1.Items.Add(mon);
            }
        }

        private void CB_Slot1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CB_Slot1.SelectedIndex != -1)
            {
                var mon1 = CB_Slot1.SelectedItem as Pokemon;
                Species1.Text = mon1.Species;
                string moves = "C-Up: ";
                moves += mon1.Moves["C-Up"];
                moves += "C-Down: ";
                moves += mon1.Moves["C-Down"];
                moves += "C-Left: ";
                moves += mon1.Moves["C-Left"];
                moves += "C-Right: ";
                moves += mon1.Moves["C-Right"];

                Moves1.Text = moves;
                team.Slot_1 = mon1;
            }
        }

        private void Save_Party_Click(object sender, RoutedEventArgs e)
        {
            //yell at them for not entering a unique name
            if (Player_Name.Text == string.Empty)
            {

            }
            else
            {
                string PlayerName = Microsoft.VisualBasic.FileSystem.CurDir() + "\\PlayerName";
                File.WriteAllText(PlayerName, Player_Name.Text);
                Player_Name.IsEnabled = false;

                //upload to db
                MongoClient dbClient = new MongoClient(connectionString);
                var db = dbClient.GetDatabase("Tournament");
                var collection = db.GetCollection<BsonDocument>("Parties");
                var document = new BsonDocument 
                {
                    { "Party Name", Player_Name.Text },
                    { "Slot 1", team.Slot_1.Species },
                    { "Slot 2", team.Slot_2.Species },
                    { "Slot 3", team.Slot_3.Species },
                    { "Slot 4", team.Slot_4.Species },
                    { "Slot 5", team.Slot_5.Species },
                    { "Slot 6", team.Slot_6.Species }
                };
                var upsert = collection.ReplaceOne(filter: new BsonDocument("Party Name", Player_Name.Text),
                    options: new ReplaceOptions { IsUpsert = true },
                    replacement: document);
            }

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
        public Pokemon(string species, Dictionary<string, string> moves)
        {
            Species = species;
            Type_1 = "";
            Type_2 = "";
            Moves = moves;

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
        public override string ToString()
        {
            return Species;
        }
    }

    public class Party
    {
        public string Name { get; set; }
        public Pokemon Slot_1 { get; set; }
        public Pokemon Slot_2 { get; set; }
        public Pokemon Slot_3 { get; set; }
        public Pokemon Slot_4 { get; set; }
        public Pokemon Slot_5 { get; set; }
        public Pokemon Slot_6 { get; set; }
        public Dictionary<Pokemon, bool>? Battle_Set { get; set; }

        public Party()
        {
            Name = string.Empty;
            Slot_1 = new Pokemon();
            Slot_2 = new Pokemon();
            Slot_3 = new Pokemon();
            Slot_4 = new Pokemon();
            Slot_5 = new Pokemon();
            Slot_6 = new Pokemon();
            Battle_Set = new Dictionary<Pokemon, bool>();
        }
        public Party(string name, Pokemon slot_1, Pokemon slot_2, Pokemon slot_3, Pokemon slot_4, Pokemon slot_5, Pokemon slot_6)
        {
            Name = name;
            Slot_1 = slot_1;
            Slot_2 = slot_2;
            Slot_3 = slot_3;
            Slot_4 = slot_4;
            Slot_5 = slot_5;
            Slot_6 = slot_6;
            Battle_Set = new Dictionary<Pokemon, bool>();
        }
        public Party(string name, Pokemon slot_1, Pokemon slot_2, Pokemon slot_3, Pokemon slot_4, Pokemon slot_5, Pokemon slot_6, Dictionary<Pokemon, bool> battle_Set) : this(name, slot_1, slot_2, slot_3, slot_4, slot_5, slot_6)
        {
            Battle_Set = battle_Set;
        }
    }

}