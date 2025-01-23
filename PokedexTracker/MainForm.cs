using System;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

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
            // Clear existing cards before loading new data
            panelCards.Controls.Clear();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Base query for fetching Pokémon data
                string query = @"
        SELECT Pokemon.Name, Pokemon.Number, Sprites.FilePath 
        FROM Pokemon
        JOIN Pokedex_Status ON Pokedex_Status.Pokemon_Number = Pokemon.Number
        JOIN Sprites ON Sprites.PokemonId = Pokemon.Number
        WHERE Pokedex_Status.Game_Id = (SELECT Id FROM Game WHERE Name = @gameName)";

                // Add conditions for game-specific sprites
                if (gameName == "Red" || gameName == "Blue")
                {
                    query += " AND Sprites.GameName = 'Red/Blue'";
                }
                else
                {
                    query += " AND Sprites.GameName = @gameName";
                }

                query += " ORDER BY Pokemon.Number";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@gameName", gameName);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        int xPos = 10; // Starting position for the first card
                        int yPos = 10; // Starting position for the first card
                        int cardsPerRow = panelCards.Width / 130; // Dynamically calculate cards per row
                        int count = 0;

                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            string number = reader["Number"].ToString();
                            string spritePath = reader["FilePath"].ToString();

                            // Fetch the current caught status of the Pokémon from the database
                            bool isCaught = GetPokemonCaughtStatus(connection, number, gameName);

                            // Create the "cube card" dynamically
                            Panel card = new Panel
                            {
                                Size = new Size(120, 150),
                                Location = new Point(xPos, yPos),
                                BorderStyle = BorderStyle.FixedSingle,
                                BackColor = isCaught ? Color.Green : Color.LightGray // Green if caught
                            };

                            PictureBox sprite = new PictureBox
                            {
                                SizeMode = PictureBoxSizeMode.StretchImage,
                                Size = new Size(100, 100),
                                Location = new Point(10, 10)
                            };

                            // Check if the sprite file exists before displaying
                            if (File.Exists(spritePath))
                            {
                                sprite.ImageLocation = spritePath;
                            }

                            Label label = new Label
                            {
                                Text = $"{name} #{number}",
                                Location = new Point(10, 115),
                                Size = new Size(100, 20),
                                TextAlign = ContentAlignment.MiddleCenter
                            };

                            card.Controls.Add(sprite);
                            card.Controls.Add(label);

                            // Add a click event to the card
                            card.Click += (sender, e) =>
                            {
                                // Reopen the connection to avoid disposed error
                                using (SQLiteConnection newConnection = new SQLiteConnection(connectionString))
                                {
                                    newConnection.Open();
                                    ToggleCaughtStatus(card, number, newConnection, gameName);  // Pass gameName as parameter
                                }
                            };

                            panelCards.Controls.Add(card);

                            // Adjust positions for the next card
                            xPos += 130;
                            count++;

                            if (count % cardsPerRow == 0)
                            {
                                // Move to the next row after enough cards
                                xPos = 10;
                                yPos += 160;
                            }
                        }
                    }
                }
            }
        }



        // Method to get the "caught" status of a Pokémon from the database
        private bool GetPokemonCaughtStatus(SQLiteConnection connection, string pokemonNumber, string gameName)
        {
            string query = @"
                SELECT Is_Caught 
                FROM Pokedex_Status
                WHERE Pokemon_Number = @pokemonNumber
                AND Game_Id = (SELECT Id FROM Game WHERE Name = @gameName)";

            using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@pokemonNumber", pokemonNumber);
                cmd.Parameters.AddWithValue("@gameName", gameName);
                object result = cmd.ExecuteScalar();
                return result != null && Convert.ToInt32(result) == 1;
            }
        }



        // Fix the "disposed" error by ensuring the connection is passed in correctly or reopened
        private void ToggleCaughtStatus(Panel card, string pokemonNumber, SQLiteConnection connection, string gameName)
        {
            // Get the current caught status
            bool currentStatus = card.BackColor == Color.Green;

            // Toggle the caught status (Green -> Uncaught, LightGray -> Caught)
            int newStatus = currentStatus ? 0 : 1;
            card.BackColor = newStatus == 1 ? Color.Green : Color.LightGray;

            // Update the status in the database for the specific game
            string query = @"
                UPDATE Pokedex_Status
                SET Is_Caught = @newStatus
                WHERE Pokemon_Number = @pokemonNumber
                AND Game_Id = (SELECT Id FROM Game WHERE Name = @gameName)";

            using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@newStatus", newStatus);
                cmd.Parameters.AddWithValue("@pokemonNumber", pokemonNumber);
                cmd.Parameters.AddWithValue("@gameName", gameName);  // Add game-specific parameter
                cmd.ExecuteNonQuery();
            }

            // Debug message to confirm database update
            MessageBox.Show("Caught status updated for Pokémon #" + pokemonNumber + " in game " + gameName + ": " + (newStatus == 1 ? "Caught" : "Uncaught"));
        }

    }
}
