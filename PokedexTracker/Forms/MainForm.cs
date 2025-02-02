using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PokedexTracker.DisplayManagers;  // Contains PlayerNameDisplayManager and ProgressDisplayManager

namespace PokedexTracker
{
    public partial class MainForm : Form
    {
        private readonly DatabaseManager _dbManager;
        private readonly GameManager _gameManager;
        private readonly AssetManager _assetManager;
        private readonly PlayerNameDisplayManager _nameDisplayManager;
        private readonly ProgressDisplayManager _progressDisplayManager;
        private string playerName;

        /// <summary>
        /// Constructor used when a player name is provided.
        /// </summary>
        public MainForm(string name) : this()
        {
            playerName = name;
            // Immediately update the player name display using the currently selected game (if any)
            if (comboBoxGames.SelectedItem is string selectedGame)
            {
                _nameDisplayManager.UpdatePlayerNameLabel(selectedGame, lblPlayerName, playerName);
            }
            // Set lblPlayerName to be drawn on top of trainerCard.
            lblPlayerName.Parent = trainerCard;
            lblPlayerName.BackColor = Color.Transparent;
        }

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public MainForm()
        {
            // Initialize asset manager and related database components.
            _assetManager = new AssetManager();
            string databasePath = _assetManager.GetDatabasePath();
            _dbManager = new DatabaseManager($@"Data Source={databasePath}");

            InitializeComponent();

            _gameManager = new GameManager(_dbManager);
            // Create the player name and progress display manager instances.
            _nameDisplayManager = new PlayerNameDisplayManager();
            _progressDisplayManager = new ProgressDisplayManager();

            // Optionally, lock the form size.
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Pre-select a default game if available.
            if (comboBoxGames.Items.Count > 0)
            {
                comboBoxGames.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Event handler for when the user selects a game from the dropdown.
        /// </summary>
        private void comboBoxGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxGames.SelectedItem is string gameName)
            {
                LoadPokemonCards(gameName);

                // Ensure player name transparency is relative to the trainer card
                lblPlayerName.Parent = trainerCard;
                lblPlayerName.BackColor = Color.Transparent;

                // Ensure progress label transparency is relative to the trainer card
                lblProgress.Parent = trainerCard;
                lblProgress.BackColor = Color.Transparent;

                // Update labels with the new game
                _nameDisplayManager.UpdatePlayerNameLabel(gameName, lblPlayerName, playerName);

                var (pokemonData, total, caught) = _gameManager.GetPokemonData(gameName);
                string progressText = $"{caught} / {total}";
                _progressDisplayManager.UpdateProgressLabel(gameName, lblProgress, progressText);
            }
        }


        private void LoadPokemonCards(string gameName)
        {
            // Clear previous Pokémon cards.
            panelCards.Controls.Clear();

            // Get data from the GameManager.
            var (pokemonData, total, caught) = _gameManager.GetPokemonData(gameName);

            // Update the progress display and trainer sprite.
            string progressText = $"{caught} / {total}";
            _progressDisplayManager.UpdateProgressLabel(gameName, lblProgress, progressText);
            UpdateTrainerSprite(caught, total, gameName);

            // Show the trainer card and progress label.
            SetTrainerVisibility();

            int xPos = 10, yPos = 10, count = 0;
            int cardsPerRow = panelCards.Width / 130;

            // Create and add Pokémon card controls.
            foreach (var (name, number, spritePath, isCaught) in pokemonData)
            {
                var card = new PokemonCard(name, number, spritePath, isCaught);
                card.Location = new Point(xPos, yPos);

                // Toggle caught status when a card is clicked.
                card.Click += (s, e) =>
                {
                    var newStatus = !card.IsCaught;
                    card.UpdateCaughtStatus(newStatus);
                    _gameManager.ToggleCaughtStatus(number, gameName, newStatus);

                    // Refresh progress and trainer sprite.
                    var (updatedData, updatedTotal, updatedCaught) = _gameManager.GetPokemonData(gameName);
                    string updatedProgress = $"{updatedCaught} / {updatedTotal}";
                    _progressDisplayManager.UpdateProgressLabel(gameName, lblProgress, updatedProgress);
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

        private void SetTrainerVisibility()
        {
            trainerCard.Visible = true;
            lblProgress.Visible = true;

            // Make sure the progress label appears above the trainer card
            lblProgress.Parent = trainerCard;
            lblProgress.BackColor = Color.Transparent;
            lblProgress.BringToFront();
        }


        // The original UpdateProgressBar method is now replaced by the call to the ProgressDisplayManager.
        // (You can remove this method if no longer needed.)
        // private void UpdateProgressBar(int caught, int total) { lblProgress.Text = $"{caught} / {total}"; }

        private void UpdateTrainerSprite(int caughtCount, int totalCount, string currentGameName)
        {
            if (trainerCard == null)
            {
                MessageBox.Show("Trainer card PictureBox is not initialized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (totalCount == 0) return;

            int badgeThreshold = totalCount / 8;
            int badgeCount = Math.Min(caughtCount / badgeThreshold, 8);

            string badgeImagePath = _assetManager.GetTrainerBadgePath(currentGameName, badgeCount);
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
            Application.Exit();
        }
    }
}
