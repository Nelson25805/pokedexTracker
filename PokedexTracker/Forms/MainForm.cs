using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PokedexTracker.DisplayManagers;
using PokedexTracker.Managers;
using PokedexTracker.Helpers;
using System.Threading;

namespace PokedexTracker
{
    public partial class MainForm : Form
    {
        private readonly DatabaseManager _dbManager;
        private readonly GameManager _gameManager;
        private readonly AssetManager _assetManager;
        private readonly PlayerNameDisplayManager _nameDisplayManager;
        private readonly ProgressDisplayManager _progressDisplayManager;
        private PokemonCardsManager _pokemonCardsManager; // Will be instantiated in the constructor.

        private CancellationTokenSource _gameSelectionCTS;


        private string playerName;
        private (int BadgeCount, string Gender, string GameName) lastTrainerState = (-1, "Boy", string.Empty);
        private string selectedGender = "Boy";

        /// <summary>
        /// Constructor used when a player name is provided.
        /// </summary>
        public MainForm(string name) : this() // Ensures the parameterless constructor runs.
        {
            playerName = name;
            if (comboBoxGames.SelectedItem is string selectedGame)
            {
                _nameDisplayManager.UpdatePlayerNameLabel(selectedGame, lblPlayerName, playerName);
            }
            // Ensure labels are drawn on top of the trainer card.
            lblPlayerName.Parent = trainerCard;
            lblPlayerName.BackColor = Color.Transparent;
            lblProgress.Parent = trainerCard;
            lblProgress.BackColor = Color.Transparent;

            // Set the custom radio button images.
            SetRadioButtonImages();
        }

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public MainForm()
        {
            // Initialize asset manager and database.
            _assetManager = new AssetManager();
            string databasePath = _assetManager.GetDatabasePath();
            _dbManager = new DatabaseManager($@"Data Source={databasePath}");

            // Initialize UI components.
            InitializeComponent();

            // Instantiate managers after InitializeComponent so that designer controls (like panelCards) are not null.
            _gameManager = new GameManager(_dbManager);
            _nameDisplayManager = new PlayerNameDisplayManager();
            _progressDisplayManager = new ProgressDisplayManager();

            // Create the PokemonCardsManager using a valid reference to panelCards.
            _pokemonCardsManager = new PokemonCardsManager(panelCards, _gameManager, _assetManager);
            // Subscribe to progress updates so we can update the trainer card.
            _pokemonCardsManager.ProgressUpdated += (total, caught) =>
            {
                if (comboBoxGames.SelectedItem is string gameName)
                {
                    UpdateProgressAndTrainer(gameName, total, caught);
                }
            };

            // Set the custom radio button images.
            SetRadioButtonImages();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (comboBoxGames.Items.Count > 0)
            {
                comboBoxGames.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Loads images for custom radio buttons.
        /// </summary>
        private void SetRadioButtonImages()
        {
            string checkedPath = _assetManager.GetRadioButtonImagePath("checkedPokeball.png");
            string uncheckedPath = _assetManager.GetRadioButtonImagePath("uncheckedPokeball.png");

            Image checkedImage = Image.FromFile(checkedPath);
            Image uncheckedImage = Image.FromFile(uncheckedPath);

            pokeballRadioButtonBoy.CheckedImage = checkedImage;
            pokeballRadioButtonBoy.UncheckedImage = uncheckedImage;
            pokeballRadioButtonBoy.Text = "Boy";

            pokeballRadioButtonGirl.CheckedImage = checkedImage;
            pokeballRadioButtonGirl.UncheckedImage = uncheckedImage;
            pokeballRadioButtonGirl.Text = "Girl";
        }

        /// <summary>
        /// Handles game selection changes.
        /// </summary>
        private async void comboBoxGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxGames.SelectedItem is string gameName)
            {
                // Cancel any previous loading task.
                _gameSelectionCTS?.Cancel();
                _gameSelectionCTS?.Dispose();
                _gameSelectionCTS = new CancellationTokenSource();
                var token = _gameSelectionCTS.Token;

                // Enable/disable shiny and gender options as before.
                int silverIndex = comboBoxGames.Items.IndexOf("Silver");
                chkShiny.Enabled = comboBoxGames.SelectedIndex >= silverIndex;
                if (!chkShiny.Enabled)
                    chkShiny.Checked = false;

                int crystalIndex = comboBoxGames.Items.IndexOf("Crystal");
                if (comboBoxGames.SelectedIndex >= crystalIndex)
                {
                    pokeballRadioButtonBoy.Enabled = true;
                    pokeballRadioButtonGirl.Enabled = true;
                    pokeballRadioButtonBoy.Checked = (selectedGender == "Boy");
                    pokeballRadioButtonGirl.Checked = (selectedGender == "Girl");
                }
                else
                {
                    pokeballRadioButtonBoy.Enabled = false;
                    pokeballRadioButtonGirl.Enabled = false;
                    pokeballRadioButtonBoy.Checked = false;
                    pokeballRadioButtonGirl.Checked = false;
                }

                bool useShiny = chkShiny.Enabled && chkShiny.Checked;
                try
                {
                    // Pass the token to LoadPokemonCardsAsync.
                    await _pokemonCardsManager.LoadPokemonCardsAsync(gameName, useShiny, token);

                    // Update UI elements after successful load.
                    _nameDisplayManager.UpdatePlayerNameLabel(gameName, lblPlayerName, playerName);
                    UpdateProgressAndTrainer(gameName);
                }
                catch (OperationCanceledException)
                {
                    // Loading was canceled because a new selection occurred.
                }
            }
        }


        /// <summary>
        /// Filters the displayed Pokémon cards based on search input.
        /// </summary>
        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_pokemonCardsManager != null)
            {
                _pokemonCardsManager.DisplayFilteredPokemon(searchTextBox.Text.Trim());
            }
        }


        /// <summary>
        /// Repositions cards when the panel is resized.
        /// </summary>
        private void panelCards_Resize(object sender, EventArgs e)
        {
            panelCards.SuspendLayout();
            panelCards.AutoScrollPosition = new Point(0, 0);
            if (_pokemonCardsManager != null)
            {
                _pokemonCardsManager.RepositionPokemonCards();
                if (panelCards.Controls.Count > 0)
                {
                    panelCards.ScrollControlIntoView(panelCards.Controls[0]);
                }
            }
            panelCards.ResumeLayout();
        }


        /// <summary>
        /// Updates both the progress display and the trainer card.
        /// </summary>
        private void UpdateProgressAndTrainer(string gameName, int total = 0, int caught = 0)
        {
            if (total == 0 && caught == 0)
            {
                bool useShiny = chkShiny.Enabled && chkShiny.Checked;
                var data = useShiny ? _gameManager.GetShinyPokemonData(gameName)
                                    : _gameManager.GetPokemonData(gameName);
                total = data.Total;
                caught = data.Caught;
            }

            lblProgress.Visible = false;
            UpdateTrainerSpriteAsync(caught, total, gameName);

            this.BeginInvoke(new Action(() =>
            {
                string progressText = $"{caught} / {total}";
                bool useShiny = chkShiny.Enabled && chkShiny.Checked;
                _progressDisplayManager.UpdateProgressLabel(gameName, lblProgress, progressText);
                lblProgress.Visible = true;
            }));
        }

        /// <summary>
        /// Updates the trainer sprite based on progress and gender.
        /// </summary>
        private async void UpdateTrainerSpriteAsync(int caughtCount, int totalCount, string currentGameName)
        {
            if (trainerCard == null || totalCount == 0)
                return;

            int badgeThreshold = totalCount / 8;
            int newBadgeCount = Math.Min(caughtCount / badgeThreshold, 8);

            // Avoid unnecessary updates.
            if (newBadgeCount == lastTrainerState.BadgeCount &&
                selectedGender == lastTrainerState.Gender &&
                currentGameName == lastTrainerState.GameName)
            {
                return;
            }

            lastTrainerState = (newBadgeCount, selectedGender, currentGameName);

            trainerCard.Visible = false;

            string badgeImagePath = _assetManager.GetTrainerBadgePath(currentGameName, newBadgeCount, selectedGender);
            if (!File.Exists(badgeImagePath))
            {
                MessageBox.Show($"Badge image not found: {badgeImagePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Image badgeImage = await Task.Run(() => Image.FromFile(badgeImagePath));

            trainerCard.Invoke((MethodInvoker)(() =>
            {
                trainerCard.Image = badgeImage;
                trainerCard.Visible = true;
            }));
        }

        /// <summary>
        /// Handles changes to the shiny checkbox state.
        /// </summary>
        private void chkShiny_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShiny.Enabled && comboBoxGames.SelectedItem is string gameName)
            {
                _gameSelectionCTS?.Cancel();
                _gameSelectionCTS = new CancellationTokenSource();
                var token = _gameSelectionCTS.Token;

                _ = _pokemonCardsManager.LoadPokemonCardsAsync(gameName, chkShiny.Checked, token);
            }
        }


        /// <summary>
        /// Handles gender radio button state changes.
        /// </summary>
        private void rdoGender_CheckedChanged(object sender, EventArgs e)
        {
            if (pokeballRadioButtonBoy.Enabled || pokeballRadioButtonGirl.Enabled)
            {
                selectedGender = pokeballRadioButtonBoy.Checked ? "Boy" : "Girl";

                if (comboBoxGames.SelectedItem is string gameName)
                {
                    bool useShiny = chkShiny.Checked;
                    var data = useShiny ? _gameManager.GetShinyPokemonData(gameName) : _gameManager.GetPokemonData(gameName);
                    UpdateTrainerSpriteAsync(data.Caught, data.Total, gameName);
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }
    }
}
