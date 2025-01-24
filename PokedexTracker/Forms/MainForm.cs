using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PokedexTracker
{
    public partial class MainForm : Form
    {
        private readonly DatabaseManager _dbManager;
        private readonly GameManager _gameManager;

        public MainForm()
        {
            InitializeComponent();
            _dbManager = new DatabaseManager(@"Data Source=C:\Users\Nelso\OneDrive\Desktop\Pokdex Project\PokedexTracker\PokedexTracker\Assets\pokedex.db");
            _gameManager = new GameManager(_dbManager);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Optionally, initial setup for the form.
        }

        private void btnRed_Click(object sender, EventArgs e) => LoadPokemonCards("Red");
        private void btnBlue_Click(object sender, EventArgs e) => LoadPokemonCards("Blue");
        private void btnYellow_Click(object sender, EventArgs e) => LoadPokemonCards("Yellow");

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
                card.Location = new System.Drawing.Point(xPos, yPos);

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
            // Hide all trainers and progress initially
            trainerCard.Visible = false;
            lblProgress.Visible = false;

            // Set visibility based on the selected game
            switch (gameName)
            {
                case "Red":
                    trainerCard.Visible = true;
                    lblProgress.Visible = true;
                    lblProgress.BringToFront();
                    break;
                case "Blue":
                    trainerCard.Visible = true;
                    lblProgress.Visible = true;
                    lblProgress.BringToFront();
                    break;
                case "Yellow":
                    trainerCard.Visible = true;
                    lblProgress.Visible = true;
                    lblProgress.BringToFront();
                    break;
                default:
                    break;
            }
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

            // Build the path to the correct image
            string projectPath = @"C:\Users\Nelso\OneDrive\Desktop\Pokdex Project\PokedexTracker\PokedexTracker\Assets\TrainerCard";
            string badgeImagePath = $@"{projectPath}\{currentGameName}\Trainer_{badgeCount}.png";

            // Check if the file exists before updating the image
            if (System.IO.File.Exists(badgeImagePath))
            {
                trainerCard.Image = System.Drawing.Image.FromFile(badgeImagePath);
            }
            else
            {
                MessageBox.Show($"Badge image not found: {badgeImagePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}