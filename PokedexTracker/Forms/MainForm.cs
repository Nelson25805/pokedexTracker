using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace PokedexTracker
{
    public partial class MainForm : Form
    {
        private readonly DatabaseManager _dbManager;
        private readonly GameManager _gameManager;
        private readonly string _assetsPath;
        private readonly string _trainerCardPath;

        private string playerName;

        public MainForm()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            _assetsPath = Path.GetFullPath(Path.Combine(basePath, @"..\..\Assets"));

            _trainerCardPath = Path.Combine(_assetsPath, "TrainerCard");

            // Construct the database path
            string databasePath = Path.Combine(_assetsPath, "pokedex.db");
            _dbManager = new DatabaseManager($@"Data Source={databasePath}");

            InitializeComponent();
            _gameManager = new GameManager(_dbManager);
        }

        public MainForm(string name)
        {
            InitializeComponent();
            playerName = name;
            Label welcomeLabel = new Label()
            {
                Text = $"Welcome to the Pokémon world, {playerName}!",
                Font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold),
                AutoSize = true,
                Location = new System.Drawing.Point(100, 100)
            };
            this.Controls.Add(welcomeLabel);

            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            _assetsPath = Path.GetFullPath(Path.Combine(basePath, @"..\..\Assets"));

            _trainerCardPath = Path.Combine(_assetsPath, "TrainerCard");

            // Construct the database path
            string databasePath = Path.Combine(_assetsPath, "pokedex.db");
            _dbManager = new DatabaseManager($@"Data Source={databasePath}");

            _gameManager = new GameManager(_dbManager);

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Optionally, initial setup for the form.
            //this.BackColor = ColorTranslator.FromHtml("#ADD8E6");
        }

        private void GameSwitchButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                string gameName = button.Tag as string; // Use the Tag property to store game names
                if (!string.IsNullOrEmpty(gameName))
                {
                    LoadPokemonCards(gameName);
                }
            }
        }


        private void LoadPokemonCards(string gameName)
        {
            // Clear previous Pokémon cards
            panelCards.Controls.Clear();

            // Get data from the GameManager
            var (pokemonData, total, caught) = _gameManager.GetPokemonData(gameName);

            // Update progress bar and trainer sprite
            UpdateProgressBar(caught, total);
            UpdateTrainerSprite(caught, total, gameName); // Pass gameName here

            // Set visibility of the trainer card and progress label based on the game selected
            SetTrainerVisibility(gameName);

            int xPos = 10, yPos = 10, count = 0;
            int cardsPerRow = panelCards.Width / 130;

            // Add Pokémon cards to the panel
            foreach (var (name, number, spritePath, isCaught) in pokemonData)
            {
                var card = new PokemonCard(name, number, spritePath, isCaught);
                card.Location = new Point(xPos, yPos);

                card.Click += (s, e) =>
                {
                    var newStatus = !card.IsCaught;
                    card.UpdateCaughtStatus(newStatus);
                    _gameManager.ToggleCaughtStatus(number, gameName, newStatus);

                    // Recalculate progress and update UI
                    var (updatedData, updatedTotal, updatedCaught) = _gameManager.GetPokemonData(gameName);
                    UpdateProgressBar(updatedCaught, updatedTotal);
                    UpdateTrainerSprite(updatedCaught, updatedTotal, gameName); // Pass gameName here
                };

                panelCards.Controls.Add(card);

                xPos += 130;
                count++;
                if (count % cardsPerRow == 0)
                {
                    xPos = 10;
                    yPos += 160;
                }
            }
        }


        private void SetTrainerVisibility(string gameName)
        {
            // Set trainer card and progress label visibility for all games
            trainerCard.Visible = true;
            lblProgress.Visible = true;
            lblProgress.BringToFront();
        }

        private void UpdateProgressBar(int caught, int total)
        {
            lblProgress.Text = $"{caught} / {total}";  // Optional label showing caught/total
        }

        private void UpdateTrainerSprite(int caughtCount, int totalCount, string currentGameName)
        {
            if (trainerCard == null)
            {
                MessageBox.Show("Trainer card PictureBox is not initialized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (totalCount == 0) return; // Avoid division by zero

            int badgeThreshold = totalCount / 8; // Each badge represents 1/8 of total Pokémon
            int badgeCount = Math.Min(caughtCount / badgeThreshold, 8); // Cap badge count at 8

            // Use the class-level _trainerCardPath field here
            string badgeImagePath = Path.Combine(_trainerCardPath, currentGameName, $"Trainer_{badgeCount}.png");

            // Check if the file exists before updating the image
            if (File.Exists(badgeImagePath))
            {
                trainerCard.Image = Image.FromFile(badgeImagePath);
            }
            else
            {
                MessageBox.Show($"Badge image not found: {badgeImagePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
