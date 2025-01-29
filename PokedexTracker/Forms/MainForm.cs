using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PokedexTracker
{
    public partial class MainForm : Form
    {
        private readonly DatabaseManager _dbManager;
        private readonly GameManager _gameManager;
        private readonly AssetManager _assetManager;

        private string playerName;

        public MainForm()
        {
            _assetManager = new AssetManager();

            // Construct the database path using AssetManager
            string databasePath = _assetManager.GetDatabasePath();
            _dbManager = new DatabaseManager($@"Data Source={databasePath}");

            InitializeComponent();
            _gameManager = new GameManager(_dbManager);
        }

        public MainForm(string name)
        {
            _assetManager = new AssetManager();
            playerName = name;

            InitializeComponent();

            Label welcomeLabel = new Label()
            {
                Text = $"Welcome to the Pokémon world, {playerName}!",
                Font = new Font("Arial", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(100, 100)
            };
            this.Controls.Add(welcomeLabel);

            // Construct the database path using AssetManager
            string databasePath = _assetManager.GetDatabasePath();
            _dbManager = new DatabaseManager($@"Data Source={databasePath}");

            _gameManager = new GameManager(_dbManager);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Optionally, initial setup for the form.
            // this.BackColor = ColorTranslator.FromHtml("#ADD8E6");
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
            UpdateTrainerSprite(caught, total, gameName);

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
                    UpdateTrainerSprite(updatedCaught, updatedTotal, gameName);
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

            // Use AssetManager to get the badge image path
            string badgeImagePath = _assetManager.GetTrainerBadgePath(currentGameName, badgeCount);

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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit(); // Ensure full application termination
        }
    }
}
