using System;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Drawing;

namespace PokedexTracker
{
    public partial class MainForm : Form
    {
        private const string connectionString = @"Data Source=C:\Users\Nelso\OneDrive\Desktop\Pokdex Project\PokedexTracker\PokedexTracker\Assets\pokedex.db";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Optionally, initial setup for the form.
        }

        private void btnRed_Click(object sender, EventArgs e)
        {
            LoadPokemonData("Red");
        }

        private void btnBlue_Click(object sender, EventArgs e)
        {
            LoadPokemonData("Blue");
        }

        private void btnYellow_Click(object sender, EventArgs e)
        {
            LoadPokemonData("Yellow");
        }

        private void LoadPokemonData(string gameName)
        {
            // Clear existing cards
            panelCards.Controls.Clear();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Query for Red/Blue or Yellow
                string query;
                if (gameName == "Red" || gameName == "Blue")
                {
                    query = @"
                SELECT Pokemon.Name, Pokemon.Number, Sprites.FilePath 
                FROM Pokemon
                JOIN Game_Pokemon ON Game_Pokemon.Pokemon_Number = Pokemon.Number
                JOIN Sprites ON Sprites.PokemonId = Pokemon.Number
                WHERE Game_Pokemon.Game_Id = (SELECT Id FROM Game WHERE Name = @gameName)
                  AND Sprites.GameName = 'Red/Blue'
                ORDER BY Pokemon.Number";
                }
                else if (gameName == "Yellow")
                {
                    query = @"
                SELECT Pokemon.Name, Pokemon.Number, Sprites.FilePath 
                FROM Pokemon
                JOIN Game_Pokemon ON Game_Pokemon.Pokemon_Number = Pokemon.Number
                JOIN Sprites ON Sprites.PokemonId = Pokemon.Number
                WHERE Game_Pokemon.Game_Id = (SELECT Id FROM Game WHERE Name = @gameName)
                  AND Sprites.GameName = @gameName
                ORDER BY Pokemon.Number";
                }
                else
                {
                    MessageBox.Show("Invalid game name.");
                    return;
                }

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@gameName", gameName);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        int xPos = 10; // Starting position for the first card
                        int yPos = 10; // Starting position for the first card

                        int count = 0;
                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            string number = reader["Number"].ToString();
                            string spritePath = reader["FilePath"].ToString();

                            // Create the "cube card" dynamically
                            Panel card = new Panel
                            {
                                Size = new Size(120, 150),
                                Location = new Point(xPos, yPos),
                                BorderStyle = BorderStyle.FixedSingle,
                                BackColor = Color.LightGray
                            };

                            PictureBox sprite = new PictureBox
                            {
                                ImageLocation = spritePath,
                                Size = new Size(100, 100),
                                Location = new Point(10, 10),
                                SizeMode = PictureBoxSizeMode.StretchImage
                            };

                            Label label = new Label
                            {
                                Text = $"{name} #{number}",
                                Location = new Point(10, 115),
                                Size = new Size(100, 20),
                                TextAlign = ContentAlignment.MiddleCenter
                            };

                            card.Controls.Add(sprite);
                            card.Controls.Add(label);

                            panelCards.Controls.Add(card);

                            // Adjust positions for the next card
                            xPos += 130;
                            count++;

                            if (count % 5 == 0)
                            {
                                // Move to the next row after 5 cards
                                xPos = 10;
                                yPos += 160;
                            }
                        }
                    }
                }
            }
        }




    }
}
