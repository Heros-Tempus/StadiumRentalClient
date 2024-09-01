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
using System.Collections.Immutable;

namespace StadiumRentalClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Pokemon> dex = new List<Pokemon>();
        Party team = new Party();
        string Proposed_Input = String.Empty;
        string? connectionString;

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
                catch
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
        }

        private void Load_Dex()
        {

            MongoClient dbClient = new MongoClient(connectionString);
            var db = dbClient.GetDatabase("Mons");
            var collection = db.GetCollection<BsonDocument>("Stadium2");
            var mons = collection.Find(new BsonDocument()).ToList();

            foreach (var m in mons) 
            { 
                string name = m.GetElement("Species").ToString().Split("=")[1];
                string gender = m.GetElement("Gender").ToString().Split("=")[1];
                string type = m.GetElement("Type").ToString().Split("=")[1];
                Dictionary<string, int> stats = new Dictionary<string, int>() {
                    { "HP", Int32.Parse(m.GetElement("HP").ToString().Split("=")[1]) },
                    { "Atk", Int32.Parse(m.GetElement("Atk").ToString().Split("=")[1]) },
                    { "Def", Int32.Parse(m.GetElement("Def").ToString().Split("=")[1]) },
                    { "Speed", Int32.Parse(m.GetElement("Speed").ToString().Split("=")[1]) },
                    { "SAtk", Int32.Parse(m.GetElement("Spec Atk").ToString().Split("=")[1]) },
                    { "SDef", Int32.Parse(m.GetElement("Spec Def").ToString().Split("=")[1]) }
                };
                string cup = m.GetElement("C-Up").ToString().Split("=")[1];
                string cdown;
                string cleft;
                string cright;
                try
                {
                    cdown = m.GetElement("C-Down").ToString().Split("=")[1];
                }
                catch
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
                //Pokemon mon = new Pokemon(name, moves);
                Pokemon mon = new Pokemon(name, gender, type, moves, stats);
                dex.Add(mon);
            }
            foreach (var mon in dex)
            {
                CB_Slot1.Items.Add(mon);
                CB_Slot2.Items.Add(mon);
                CB_Slot3.Items.Add(mon);
                CB_Slot4.Items.Add(mon);
                CB_Slot5.Items.Add(mon);
                CB_Slot6.Items.Add(mon);
            }
        }

        private void CB_Slot1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CB_Slot1.SelectedIndex != -1)
            {
                var mon = CB_Slot1.SelectedItem as Pokemon;
                switch (mon.Gender)
                {
                    case "M": 
                        Species1.Text = mon.Species + "   (He/Him)";
                        break;
                    case "F": 
                        Species1.Text = mon.Species + "   (She/Her)";
                        break;
                    case "NB":
                        Species1.Text = mon.Species + "   (They/Them)";
                        break;
                }
                Type1.Text = mon.Type;
                Stats1.Text = mon.Stats.Aggregate(new StringBuilder(),
                    (sb,kvp) => sb.AppendFormat("{0}{1}={2}", 
                    sb.Length > 0 ? ", " : "", kvp.Key, kvp.Value), 
                    sb => sb.ToString());
                string moves = "cUp:";
                moves += mon.Moves["C-Up"] + " ";
                moves += "cDown:";
                moves += mon.Moves["C-Down"] + " ";
                moves += "cLeft:";
                moves += mon.Moves["C-Left"] + " ";
                moves += "cRight:";
                moves += mon.Moves["C-Right"] + " ";
                Moves1.Text = moves;
                team.Slot_1 = mon;
            }
        }
        private void CB_Slot2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CB_Slot2.SelectedIndex != -1)
            {
                var mon = CB_Slot2.SelectedItem as Pokemon;
                switch (mon.Gender)
                {
                    case "M":
                        Species2.Text = mon.Species + "   (He/Him)";
                        break;
                    case "F":
                        Species2.Text = mon.Species + "   (She/Her)";
                        break;
                    case "NB":
                        Species2.Text = mon.Species + "   (They/Them)";
                        break;
                }
                Type2.Text = mon.Type;
                Stats2.Text = mon.Stats.Aggregate(new StringBuilder(),
                    (sb, kvp) => sb.AppendFormat("{0}{1}={2}",
                    sb.Length > 0 ? ", " : "", kvp.Key, kvp.Value),
                    sb => sb.ToString());
                string moves = "cUp:";
                moves += mon.Moves["C-Up"] + " ";
                moves += "cDown:";
                moves += mon.Moves["C-Down"] + " ";
                moves += "cLeft:";
                moves += mon.Moves["C-Left"] + " ";
                moves += "cRight:";
                moves += mon.Moves["C-Right"] + " ";
                Moves2.Text = moves;
                team.Slot_2 = mon;
            }

        }
        private void CB_Slot3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CB_Slot3.SelectedIndex != -1)
            {
                var mon = CB_Slot3.SelectedItem as Pokemon;
                switch (mon.Gender)
                {
                    case "M":
                        Species3.Text = mon.Species + "   (He/Him)";
                        break;
                    case "F":
                        Species3.Text = mon.Species + "   (She/Her)";
                        break;
                    case "NB":
                        Species3.Text = mon.Species + "   (They/Them)";
                        break;
                }
                Type3.Text = mon.Type;
                Stats3.Text = mon.Stats.Aggregate(new StringBuilder(),
                    (sb, kvp) => sb.AppendFormat("{0}{1}={2}",
                    sb.Length > 0 ? ", " : "", kvp.Key, kvp.Value),
                    sb => sb.ToString());
                string moves = "cUp:";
                moves += mon.Moves["C-Up"] + " ";
                moves += "cDown:";
                moves += mon.Moves["C-Down"] + " ";
                moves += "cLeft:";
                moves += mon.Moves["C-Left"] + " ";
                moves += "cRight:";
                moves += mon.Moves["C-Right"] + " ";
                Moves3.Text = moves;
                team.Slot_3 = mon;
            }

        }
        private void CB_Slot4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CB_Slot4.SelectedIndex != -1)
            {
                var mon = CB_Slot4.SelectedItem as Pokemon;
                switch (mon.Gender)
                {
                    case "M":
                        Species4.Text = mon.Species + "   (He/Him)";
                        break;
                    case "F":
                        Species4.Text = mon.Species + "   (She/Her)";
                        break;
                    case "NB":
                        Species4.Text = mon.Species + "   (They/Them)";
                        break;
                }
                Type4.Text = mon.Type;
                Stats4.Text = mon.Stats.Aggregate(new StringBuilder(),
                    (sb, kvp) => sb.AppendFormat("{0}{1}={2}",
                    sb.Length > 0 ? ", " : "", kvp.Key, kvp.Value),
                    sb => sb.ToString());
                string moves = "cUp:";
                moves += mon.Moves["C-Up"] + " ";
                moves += "cDown:";
                moves += mon.Moves["C-Down"] + " ";
                moves += "cLeft:";
                moves += mon.Moves["C-Left"] + " ";
                moves += "cRight:";
                moves += mon.Moves["C-Right"] + " ";
                Moves4.Text = moves;
                team.Slot_4 = mon;
            }
        }
        private void CB_Slot5_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CB_Slot5.SelectedIndex != -1)
            {
                var mon = CB_Slot5.SelectedItem as Pokemon;
                switch (mon.Gender)
                {
                    case "M":
                        Species5.Text = mon.Species + "   (He/Him)";
                        break;
                    case "F":
                        Species5.Text = mon.Species + "   (She/Her)";
                        break;
                    case "NB":
                        Species5.Text = mon.Species + "   (They/Them)";
                        break;
                }
                Type5.Text = mon.Type;
                Stats5.Text = mon.Stats.Aggregate(new StringBuilder(),
                    (sb, kvp) => sb.AppendFormat("{0}{1}={2}",
                    sb.Length > 0 ? ", " : "", kvp.Key, kvp.Value),
                    sb => sb.ToString());
                string moves = "cUp:";
                moves += mon.Moves["C-Up"] + " ";
                moves += "cDown:";
                moves += mon.Moves["C-Down"] + " ";
                moves += "cLeft:";
                moves += mon.Moves["C-Left"] + " ";
                moves += "cRight:";
                moves += mon.Moves["C-Right"] + " ";
                Moves5.Text = moves;
                team.Slot_5 = mon;
            }
        }
        private void CB_Slot6_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CB_Slot6.SelectedIndex != -1)
            {
                var mon = CB_Slot6.SelectedItem as Pokemon;
                switch (mon.Gender)
                {
                    case "M":
                        Species6.Text = mon.Species + "   (He/Him)";
                        break;
                    case "F":
                        Species6.Text = mon.Species + "   (She/Her)";
                        break;
                    case "NB":
                        Species6.Text = mon.Species + "   (They/Them)";
                        break;
                }
                Type6.Text = mon.Type;
                Stats6.Text = mon.Stats.Aggregate(new StringBuilder(),
                    (sb, kvp) => sb.AppendFormat("{0}{1}={2}",
                    sb.Length > 0 ? ", " : "", kvp.Key, kvp.Value),
                    sb => sb.ToString());
                string moves = "cUp:";
                moves += mon.Moves["C-Up"] + " ";
                moves += "cDown:";
                moves += mon.Moves["C-Down"] + " ";
                moves += "cLeft:";
                moves += mon.Moves["C-Left"] + " ";
                moves += "cRight:";
                moves += mon.Moves["C-Right"] + " ";
                Moves6.Text = moves;
                team.Slot_6 = mon;
            }
        }

        private void Save_Party_Click(object sender, RoutedEventArgs e)
        {
            if (Player_Name.Text == string.Empty)
            {
                MessageBox.Show("You need to enter your name in the textbox", "Error 0");
            }
            else if (CB_Slot1.SelectedIndex == -1 || CB_Slot2.SelectedIndex == -1 || CB_Slot3.SelectedIndex == -1 || CB_Slot4.SelectedIndex == -1 || CB_Slot5.SelectedIndex == -1 || CB_Slot6.SelectedIndex == -1)
            {
                MessageBox.Show("You need to fill out your party", "Error 1");
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
                if (team.Battle_Set_Validate())
                {
                    var document = new BsonDocument 
                    {
                        { "Party Name", Player_Name.Text },
                        { "Slot 1", team.Slot_1.Species },
                        { "Slot 2", team.Slot_2.Species },
                        { "Slot 3", team.Slot_3.Species },
                        { "Slot 4", team.Slot_4.Species },
                        { "Slot 5", team.Slot_5.Species },
                        { "Slot 6", team.Slot_6.Species },
                        { "Battle Set", new BsonArray { 
                            new BsonDocument { { "First Pick", team.Battle_Set[0].Species } },
                            new BsonDocument{ { "Second Pick", team.Battle_Set[1].Species } },
                            new BsonDocument{ { "Third Pick", team.Battle_Set[2].Species } } 
                        } }
                    };
                    var upsert = collection.ReplaceOne(filter: new BsonDocument("Party Name", Player_Name.Text),
                        options: new ReplaceOptions { IsUpsert = true },
                        replacement: document);
                }
                else
                {
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

                        CB_Battleset_Slot1.Items.Add(team.Slot_1);
                        CB_Battleset_Slot1.Items.Add(team.Slot_2);
                        CB_Battleset_Slot1.Items.Add(team.Slot_3);
                        CB_Battleset_Slot1.Items.Add(team.Slot_4);
                        CB_Battleset_Slot1.Items.Add(team.Slot_5);
                        CB_Battleset_Slot1.Items.Add(team.Slot_6);

                        CB_Battleset_Slot2.Items.Add(team.Slot_1);
                        CB_Battleset_Slot2.Items.Add(team.Slot_2);
                        CB_Battleset_Slot2.Items.Add(team.Slot_3);
                        CB_Battleset_Slot2.Items.Add(team.Slot_4);
                        CB_Battleset_Slot2.Items.Add(team.Slot_5);
                        CB_Battleset_Slot2.Items.Add(team.Slot_6);

                        CB_Battleset_Slot3.Items.Add(team.Slot_1);
                        CB_Battleset_Slot3.Items.Add(team.Slot_2);
                        CB_Battleset_Slot3.Items.Add(team.Slot_3);
                        CB_Battleset_Slot3.Items.Add(team.Slot_4);
                        CB_Battleset_Slot3.Items.Add(team.Slot_5);
                        CB_Battleset_Slot3.Items.Add(team.Slot_6);
                }
            }

        }

        private void Load_Party_Click(object sender, RoutedEventArgs e)
        {
            if (Player_Name.Text == string.Empty)
            {
                MessageBox.Show("The database cannot be queried without a name to search for\nPut yours in the text box", "Error 2");
            }
            else
            {
                try
                {
                    MongoClient dbClient = new MongoClient(connectionString);
                    var db = dbClient.GetDatabase("Tournament");
                    var collection = db.GetCollection<BsonDocument>("Parties");
                    var load = collection.Find(new BsonDocument("Party Name", Player_Name.Text)).FirstOrDefault();
                    var party = load.ToDictionary();
                    Player_Name.Text = party["Party Name"].ToString();
                    CB_Slot1.SelectedItem = dex.FirstOrDefault(x => x.Species == party["Slot 1"].ToString());
                    CB_Slot2.SelectedItem = dex.FirstOrDefault(x => x.Species == party["Slot 2"].ToString());
                    CB_Slot3.SelectedItem = dex.FirstOrDefault(x => x.Species == party["Slot 3"].ToString());
                    CB_Slot4.SelectedItem = dex.FirstOrDefault(x => x.Species == party["Slot 4"].ToString());
                    CB_Slot5.SelectedItem = dex.FirstOrDefault(x => x.Species == party["Slot 5"].ToString());
                    CB_Slot6.SelectedItem = dex.FirstOrDefault(x => x.Species == party["Slot 6"].ToString());

                    team.Slot_1 = CB_Slot1.SelectedItem as Pokemon;
                    team.Slot_2 = CB_Slot2.SelectedItem as Pokemon;
                    team.Slot_3 = CB_Slot3.SelectedItem as Pokemon;
                    team.Slot_4 = CB_Slot4.SelectedItem as Pokemon;
                    team.Slot_5 = CB_Slot5.SelectedItem as Pokemon;
                    team.Slot_6 = CB_Slot6.SelectedItem as Pokemon;

                    CB_Battleset_Slot1.Items.Add(team.Slot_1);
                    CB_Battleset_Slot1.Items.Add(team.Slot_2);
                    CB_Battleset_Slot1.Items.Add(team.Slot_3);
                    CB_Battleset_Slot1.Items.Add(team.Slot_4);
                    CB_Battleset_Slot1.Items.Add(team.Slot_5);
                    CB_Battleset_Slot1.Items.Add(team.Slot_6);

                    CB_Battleset_Slot2.Items.Add(team.Slot_1);
                    CB_Battleset_Slot2.Items.Add(team.Slot_2);
                    CB_Battleset_Slot2.Items.Add(team.Slot_3);
                    CB_Battleset_Slot2.Items.Add(team.Slot_4);
                    CB_Battleset_Slot2.Items.Add(team.Slot_5);
                    CB_Battleset_Slot2.Items.Add(team.Slot_6);

                    CB_Battleset_Slot3.Items.Add(team.Slot_1);
                    CB_Battleset_Slot3.Items.Add(team.Slot_2);
                    CB_Battleset_Slot3.Items.Add(team.Slot_3);
                    CB_Battleset_Slot3.Items.Add(team.Slot_4);
                    CB_Battleset_Slot3.Items.Add(team.Slot_5);
                    CB_Battleset_Slot3.Items.Add(team.Slot_6);
                }
                catch
                {
                    MessageBox.Show("Party was not found in the database.\nAre you sure you saved before loading?", "Error 3");
                }
            }
        }


        private void CB_Battleset_Slot1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //set Battle_Set[0] to the selected mon
            do
            {
                if (CB_Battleset_Slot1.SelectedIndex == CB_Battleset_Slot2.SelectedIndex && e.RemovedItems.Count == 0)
                {
                    CB_Battleset_Slot2.SelectedIndex = -1;
                    CB_Battleset_Slot2.Items.Refresh();
                    team.Battle_Set[0] = CB_Battleset_Slot1.SelectedItem as Pokemon;
                    team.Battle_Set[1] = new Pokemon();
                    break;
                }
                else if (CB_Battleset_Slot1.SelectedIndex == CB_Battleset_Slot3.SelectedIndex && e.RemovedItems.Count == 0)
                {
                    CB_Battleset_Slot3.SelectedIndex = -1;
                    CB_Battleset_Slot3.Items.Refresh();
                    team.Battle_Set[0] = CB_Battleset_Slot1.SelectedItem as Pokemon;
                    team.Battle_Set[2] = new Pokemon();
                }
            }
            while (false);
            if (CB_Battleset_Slot1.SelectedIndex != -1)
                team.Battle_Set[0] = CB_Battleset_Slot1.SelectedItem as Pokemon;
        }

        private void CB_Battleset_Slot2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //set Battle_Set[1] to the selected mon
            do
            {
                if (CB_Battleset_Slot2.SelectedIndex == CB_Battleset_Slot1.SelectedIndex && e.RemovedItems.Count == 0)
                {
                    CB_Battleset_Slot1.SelectedIndex = -1;
                    CB_Battleset_Slot1.Items.Refresh();
                    team.Battle_Set[1] = CB_Battleset_Slot2.SelectedItem as Pokemon;
                    team.Battle_Set[0] = new Pokemon();
                    break;
                }
                else if (CB_Battleset_Slot2.SelectedIndex == CB_Battleset_Slot3.SelectedIndex && e.RemovedItems.Count == 0)
                {
                    CB_Battleset_Slot3.SelectedIndex = -1;
                    CB_Battleset_Slot3.Items.Refresh();
                    team.Battle_Set[1] = CB_Battleset_Slot2.SelectedItem as Pokemon;
                    team.Battle_Set[2] = new Pokemon();
                }
            }
            while (false);
            if (CB_Battleset_Slot2.SelectedIndex != -1)
                team.Battle_Set[1] = CB_Battleset_Slot2.SelectedItem as Pokemon;
        }

        private void CB_Battleset_Slot3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //set Battle_Set[2] to the selected mon
            do
            {
                if (CB_Battleset_Slot3.SelectedIndex == CB_Battleset_Slot1.SelectedIndex && e.RemovedItems.Count == 0)
                {
                    CB_Battleset_Slot1.SelectedIndex = -1;
                    CB_Battleset_Slot1.Items.Refresh();
                    team.Battle_Set[2] = CB_Battleset_Slot3.SelectedItem as Pokemon;
                    team.Battle_Set[0] = new Pokemon();
                    break;
                }
                else if (CB_Battleset_Slot3.SelectedIndex == CB_Battleset_Slot2.SelectedIndex && e.RemovedItems.Count == 0)
                {
                    CB_Battleset_Slot2.SelectedIndex = -1;
                    CB_Battleset_Slot2.Items.Refresh();
                    team.Battle_Set[2] = CB_Battleset_Slot3.SelectedItem as Pokemon;
                    team.Battle_Set[1] = new Pokemon();
                }
            }
            while (false);

            if (CB_Battleset_Slot3.SelectedIndex != -1)
                team.Battle_Set[2] = CB_Battleset_Slot3.SelectedItem as Pokemon;
        }

        private void Upload_Battle_Set_Click(object sender, RoutedEventArgs e)
        {
            //if Battle_Set is valid then upsert battleset to the party collection
            if (team.Battle_Set_Validate())
            {
                Slot_1.Text = team.Battle_Set[0].Species;
                Slot_2.Text = team.Battle_Set[1].Species;
                Slot_3.Text = team.Battle_Set[2].Species;

                Slot_1_C_Up.Content = team.Battle_Set[0].Moves["C-Up"];
                Slot_1_C_Right.Content = team.Battle_Set[0].Moves["C-Right"];
                Slot_1_C_Down.Content = team.Battle_Set[0].Moves["C-Down"];
                Slot_1_C_Left.Content = team.Battle_Set[0].Moves["C-Left"];

                Slot_2_C_Up.Content = team.Battle_Set[1].Moves["C-Up"];
                Slot_2_C_Right.Content = team.Battle_Set[1].Moves["C-Right"];
                Slot_2_C_Down.Content = team.Battle_Set[1].Moves["C-Down"];
                Slot_2_C_Left.Content = team.Battle_Set[1].Moves["C-Left"];

                Slot_3_C_Up.Content = team.Battle_Set[2].Moves["C-Up"];
                Slot_3_C_Right.Content = team.Battle_Set[2].Moves["C-Right"];
                Slot_3_C_Down.Content = team.Battle_Set[2].Moves["C-Down"];
                Slot_3_C_Left.Content = team.Battle_Set[2].Moves["C-Left"];

                MongoClient dbClient = new MongoClient(connectionString);
                var db = dbClient.GetDatabase("Tournament");
                var collection = db.GetCollection<BsonDocument>("Parties"); var document = new BsonDocument
                    {
                        { "Party Name", Player_Name.Text },
                        { "Slot 1", team.Slot_1.Species },
                        { "Slot 2", team.Slot_2.Species },
                        { "Slot 3", team.Slot_3.Species },
                        { "Slot 4", team.Slot_4.Species },
                        { "Slot 5", team.Slot_5.Species },
                        { "Slot 6", team.Slot_6.Species },
                        { "Battle Set", new BsonArray {
                            new BsonDocument { { "First Pick", team.Battle_Set[0].Species } },
                            new BsonDocument{ { "Second Pick", team.Battle_Set[1].Species } },
                            new BsonDocument{ { "Third Pick", team.Battle_Set[2].Species } }
                        } }
                    };
                var upsert = collection.ReplaceOne(filter: new BsonDocument("Party Name", Player_Name.Text),
                    options: new ReplaceOptions { IsUpsert = true },
                    replacement: document);
            }
        }

        private void Update_From_Database_Click(object sender, RoutedEventArgs e)
        {
            MongoClient dbClient = new MongoClient(connectionString);
            var db = dbClient.GetDatabase("Tournament");
            var collection = db.GetCollection<BsonDocument>("Parties");
            var load = collection.Find(new BsonDocument("Party Name", Player_Name.Text)).FirstOrDefault();
            var party = load.ToDictionary();
            Player_Name.Text = party["Party Name"].ToString();
            CB_Slot1.SelectedItem = dex.FirstOrDefault(x => x.Species == party["Slot 1"].ToString());
            CB_Slot2.SelectedItem = dex.FirstOrDefault(x => x.Species == party["Slot 2"].ToString());
            CB_Slot3.SelectedItem = dex.FirstOrDefault(x => x.Species == party["Slot 3"].ToString());
            CB_Slot4.SelectedItem = dex.FirstOrDefault(x => x.Species == party["Slot 4"].ToString());
            CB_Slot5.SelectedItem = dex.FirstOrDefault(x => x.Species == party["Slot 5"].ToString());
            CB_Slot6.SelectedItem = dex.FirstOrDefault(x => x.Species == party["Slot 6"].ToString());

            team.Slot_1 = CB_Slot1.SelectedItem as Pokemon;
            team.Slot_2 = CB_Slot2.SelectedItem as Pokemon;
            team.Slot_3 = CB_Slot3.SelectedItem as Pokemon;
            team.Slot_4 = CB_Slot4.SelectedItem as Pokemon;
            team.Slot_5 = CB_Slot5.SelectedItem as Pokemon;
            team.Slot_6 = CB_Slot6.SelectedItem as Pokemon;

            CB_Battleset_Slot1.Items.Add(team.Slot_1);
            CB_Battleset_Slot1.Items.Add(team.Slot_2);
            CB_Battleset_Slot1.Items.Add(team.Slot_3);
            CB_Battleset_Slot1.Items.Add(team.Slot_4);
            CB_Battleset_Slot1.Items.Add(team.Slot_5);
            CB_Battleset_Slot1.Items.Add(team.Slot_6);

            CB_Battleset_Slot2.Items.Add(team.Slot_1);
            CB_Battleset_Slot2.Items.Add(team.Slot_2);
            CB_Battleset_Slot2.Items.Add(team.Slot_3);
            CB_Battleset_Slot2.Items.Add(team.Slot_4);
            CB_Battleset_Slot2.Items.Add(team.Slot_5);
            CB_Battleset_Slot2.Items.Add(team.Slot_6);

            CB_Battleset_Slot3.Items.Add(team.Slot_1);
            CB_Battleset_Slot3.Items.Add(team.Slot_2);
            CB_Battleset_Slot3.Items.Add(team.Slot_3);
            CB_Battleset_Slot3.Items.Add(team.Slot_4);
            CB_Battleset_Slot3.Items.Add(team.Slot_5);
            CB_Battleset_Slot3.Items.Add(team.Slot_6);

        }

        private void Slot_1_Swap_Click(object sender, RoutedEventArgs e)
        {
            if (team.Battle_Set_Validate())
            {
                Reset_Input_Buttons_Color();
                Slot_1_Swap.Background = new SolidColorBrush(Colors.Green);
                Proposed_Input = "Swap : C-Left";
            }
        }


        private void Slot_1_C_Up_Click(object sender, RoutedEventArgs e)
        {
            if (team.Battle_Set_Validate())
            {
                Reset_Input_Buttons_Color();
                Slot_1_C_Up.Background = new SolidColorBrush(Colors.Green);
                Proposed_Input = "C-Up";
            }
        }

        private void Slot_1_C_Right_Click(object sender, RoutedEventArgs e)
        {
            if (team.Battle_Set_Validate())
            {
                Reset_Input_Buttons_Color();
                Slot_1_C_Right.Background = new SolidColorBrush(Colors.Green);
                Proposed_Input = "C-Right";
            }
        }

        private void Slot_1_C_Left_Click(object sender, RoutedEventArgs e)
        {
            if (team.Battle_Set_Validate())
            {
                Reset_Input_Buttons_Color();
                Slot_1_C_Left.Background = new SolidColorBrush(Colors.Green);
                Proposed_Input = "C-Left";
            }
        }

        private void Slot_1_C_Down_Click(object sender, RoutedEventArgs e)
        {
            if (team.Battle_Set_Validate())
            {
                Reset_Input_Buttons_Color();
                Slot_1_C_Down.Background = new SolidColorBrush(Colors.Green);
                Proposed_Input = "C-Down";
            }
        }
        private void Commit_Input_Click(object sender, RoutedEventArgs e)
        {
            if (Proposed_Input != String.Empty)
            {
                MongoClient dbClient = new MongoClient(connectionString);
                var db = dbClient.GetDatabase("Tournament");
                var collection = db.GetCollection<BsonDocument>("Inputs"); var document = new BsonDocument
                    {
                        { "Party Name", Player_Name.Text },
                        { "Input", Proposed_Input }
                    };
                var upsert = collection.ReplaceOne(filter: new BsonDocument("Party Name", Player_Name.Text),
                    options: new ReplaceOptions { IsUpsert = true },
                    replacement: document);
            }
            Reset_Input_Buttons_Color();
            Proposed_Input = String.Empty;
            MessageBox.Show("Sent input to database");
        }

        private void Reset_Input_Buttons_Color()
        {
            Slot_1_Swap.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFDDDDDD");
            Slot_1_C_Up.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFDDDDDD");
            Slot_1_C_Right.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFDDDDDD");
            Slot_1_C_Down.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFDDDDDD");
            Slot_1_C_Left.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFDDDDDD");

            Slot_2_Swap.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFDDDDDD");
            Slot_2_C_Up.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFDDDDDD");
            Slot_2_C_Right.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFDDDDDD");
            Slot_2_C_Down.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFDDDDDD");
            Slot_2_C_Left.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFDDDDDD");

            Slot_3_Swap.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFDDDDDD");
            Slot_3_C_Up.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFDDDDDD");
            Slot_3_C_Right.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFDDDDDD");
            Slot_3_C_Down.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFDDDDDD");
            Slot_3_C_Left.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFDDDDDD");
        }

        private void Slot_2_Swap_Click(object sender, RoutedEventArgs e)
        {
            if (team.Battle_Set_Validate())
            {
                Reset_Input_Buttons_Color();
                Slot_2_Swap.Background = new SolidColorBrush(Colors.Green);
                Proposed_Input = "Swap : C-Up";
            }
        }

        private void Slot_3_Swap_Click(object sender, RoutedEventArgs e)
        {
            if (team.Battle_Set_Validate())
            {
                Reset_Input_Buttons_Color();
                Slot_3_Swap.Background = new SolidColorBrush(Colors.Green);
                Proposed_Input = "Swap : C-Right";
            }
        }

        private void Slot_2_C_Up_Click(object sender, RoutedEventArgs e)
        {
            if (team.Battle_Set_Validate())
            {
                Reset_Input_Buttons_Color();
                Slot_2_C_Up.Background = new SolidColorBrush(Colors.Green);
                Proposed_Input = "C-Up";
            }
        }

        private void Slot_2_C_Right_Click(object sender, RoutedEventArgs e)
        {
            if (team.Battle_Set_Validate())
            {
                Reset_Input_Buttons_Color();
                Slot_2_C_Right.Background = new SolidColorBrush(Colors.Green);
                Proposed_Input = "C-Right";
            }
        }

        private void Slot_2_C_Down_Click(object sender, RoutedEventArgs e)
        {
            if (team.Battle_Set_Validate())
            {
                Reset_Input_Buttons_Color();
                Slot_2_C_Down.Background = new SolidColorBrush(Colors.Green);
                Proposed_Input = "C-Down";
            }
        }

        private void Slot_2_C_Left_Click(object sender, RoutedEventArgs e)
        {
            if (team.Battle_Set_Validate())
            {
                Reset_Input_Buttons_Color();
                Slot_2_C_Left.Background = new SolidColorBrush(Colors.Green);
                Proposed_Input = "C-Left";
            }
        }

        private void Slot_3_C_Up_Click(object sender, RoutedEventArgs e)
        {
            if (team.Battle_Set_Validate())
            {
                Reset_Input_Buttons_Color();
                Slot_3_C_Up.Background = new SolidColorBrush(Colors.Green);
                Proposed_Input = "C-Up";
            }
        }

        private void Slot_3_C_Right_Click(object sender, RoutedEventArgs e)
        {
            if (team.Battle_Set_Validate())
            {
                Reset_Input_Buttons_Color();
                Slot_3_C_Right.Background = new SolidColorBrush(Colors.Green);
                Proposed_Input = "C-Right";
            }
        }

        private void Slot_3_C_Down_Click(object sender, RoutedEventArgs e)
        {
            if (team.Battle_Set_Validate())
            {
                Reset_Input_Buttons_Color();
                Slot_3_C_Down.Background = new SolidColorBrush(Colors.Green);
                Proposed_Input = "C-Down";
            }
        }

        private void Slot_3_C_Left_Click(object sender, RoutedEventArgs e)
        {
            if (team.Battle_Set_Validate())
            {
                Reset_Input_Buttons_Color();
                Slot_3_C_Left.Background = new SolidColorBrush(Colors.Green);
                Proposed_Input = "C-Left";
            }
        }
    }
    public class Pokemon
    {
        public string Species {  get; set; }
        public string Gender { get; set; }
        public string Type { get; set; }
        public Dictionary<string, string> Moves { get; set; }
        
        public Dictionary<string, int>? Stats { get; set; }
        private static List<string> Stat_Names = new List<string> { "hp", "atk", "spec", "def", "speed" };
        public Pokemon()
        {
            Species = "";
            Gender = "NB";
            Type = "";
            Moves = new Dictionary<string, string>();
            Stats = Stat_Names.ToDictionary(k=>k, k=>0);
        }
        public Pokemon(string species, string gender, Dictionary<string, string> moves)
        {
            Species = species;
            Gender = gender;
            Type = "";
            Moves = moves;

        }
        public Pokemon(string species, string gender, string type, Dictionary<string, string> moves)
        {
            Species = species;
            Gender = gender;
            Type = type;
            Moves = moves;
        }
        public Pokemon(string species, string gender, string type, Dictionary<string, string> moves, Dictionary<string, int>? stats) : this(species, gender, type, moves)
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
        public Pokemon[] Battle_Set { get; set; }

        public Party()
        {
            Name = string.Empty;
            Slot_1 = new Pokemon();
            Slot_2 = new Pokemon();
            Slot_3 = new Pokemon();
            Slot_4 = new Pokemon();
            Slot_5 = new Pokemon();
            Slot_6 = new Pokemon();
            Battle_Set = new Pokemon[3];
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
            Battle_Set = new Pokemon[3];
        }
        public Party(string name, Pokemon slot_1, Pokemon slot_2, Pokemon slot_3, Pokemon slot_4, Pokemon slot_5, Pokemon slot_6, Pokemon[] battle_Set) : this(name, slot_1, slot_2, slot_3, slot_4, slot_5, slot_6)
        {
            Battle_Set = battle_Set;
        }
        public bool Party_Validate()
        {
            if (Slot_1.Species != "" && Slot_2.Species != "" && Slot_3.Species != "" && Slot_4.Species != "" && Slot_5.Species != "" && Slot_6.Species != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Battle_Set_Validate()
        {
            try 
            {
                if (Battle_Set[0].Species != "" && Battle_Set[1].Species != "" && Battle_Set[2].Species != "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }

}