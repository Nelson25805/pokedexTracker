using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using PokedexTracker.DisplayManagers; 
using PokedexTracker.Helpers;

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
        private UILayoutManager _layoutManager;


        // Field to store the selected gender ("Boy" or "Girl"). Default is "Boy".
        private string selectedGender = "Boy";

        /// <summary>
        /// Constructor used when a player name is provided.
        /// </summary>
        public MainForm(string name) : this()
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
        }

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public MainForm()
        {
            // Initialize asset manager, database, etc.
            _assetManager = new AssetManager();
            string databasePath = _assetManager.GetDatabasePath();
            _dbManager = new DatabaseManager($@"Data Source={databasePath}");

            InitializeComponent();

            // Initialize the layout manager for the form.
            _layoutManager = new UILayoutManager(this);

            // Subscribe to panelCards resize to reflow the Pokemon cards.
            panelCards.Resize += panelCards_Resize;

            _gameManager = new GameManager(_dbManager);
            _nameDisplayManager = new PlayerNameDisplayManager();
            _progressDisplayManager = new ProgressDisplayManager();

            // Optionally, lock the form size.
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //this.MaximizeBox = false;
            //this.MinimumSize = this.Size;
            //this.MaximumSize = this.Size;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (comboBoxGames.Items.Count > 0)
            {
                comboBoxGames.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Handles when the user selects a game from the dropdown.
        /// </summary>
        private void comboBoxGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxGames.SelectedItem is string gameName)
            {
                // First, update the shiny checkbox state based on the selected game.
                int silverIndex = comboBoxGames.Items.IndexOf("Silver");
                if (comboBoxGames.SelectedIndex >= silverIndex)
                {
                    chkShiny.Enabled = true;
                    // Optionally, you can leave chkShiny.Checked as-is or set it to true if desired.
                }
                else
                {
                    chkShiny.Enabled = false;
                    chkShiny.Checked = false;  // Ensure shiny is off for non-shiny games.
                }

                // Similarly, update the gender radio buttons.
                int goldIndex = comboBoxGames.Items.IndexOf("Gold");
                if (comboBoxGames.SelectedIndex > goldIndex)
                {
                    rdoBoy.Enabled = true;
                    rdoGirl.Enabled = true;
                    rdoBoy.Checked = (selectedGender == "Boy");
                    rdoGirl.Checked = (selectedGender == "Girl");
                }
                else
                {
                    rdoBoy.Enabled = false;
                    rdoGirl.Enabled = false;
                    rdoBoy.Checked = false;
                    rdoGirl.Checked = false;
                }

                // Now that the controls reflect the selected game,
                // load the Pokémon cards using the correct shiny mode.
                LoadPokemonCards(gameName);

                // Update the player name display.
                _nameDisplayManager.UpdatePlayerNameLabel(gameName, lblPlayerName, playerName);

                // Update progress and trainer card display.
                UpdateProgressAndTrainer(gameName);
            }
        }


        /// <summary>
        /// Loads Pokémon cards depending on whether shiny mode is active.
        /// </summary>
        private void LoadPokemonCards(string gameName)
        {
            // Reset the scroll position before making changes.
            panelCards.AutoScrollPosition = new Point(0, 0);

            // Suspend layout and hide the panel to avoid interim redraws.
            panelCards.SuspendLayout();
            panelCards.Visible = false;

            panelCards.Controls.Clear();

            bool useShiny = chkShiny.Enabled && chkShiny.Checked;
            // Retrieve the data.
            var data = useShiny ? _gameManager.GetShinyPokemonData(gameName)
                                : _gameManager.GetPokemonData(gameName);
            var pokemonData = data.PokemonData;
            int total = data.Total;
            int caught = data.Caught;

            UpdateProgressAndTrainer(gameName, total, caught);
            SetTrainerVisibility();

            // Add each Pokemon card. (Setting a temporary location.)
            foreach (var item in pokemonData)
            {
                string name = item.Name;
                string number = item.Number;
                string spritePath = item.SpritePath;
                bool isCaught = item.IsCaught;

                var card = new PokemonCard(name, number, spritePath, isCaught);
                card.Location = new Point(0, 0); // Temporary; will be reflowed.

                card.Click += (s, e) =>
                {
                    bool newStatus = !card.IsCaught;
                    card.UpdateCaughtStatus(newStatus);

                    if (useShiny)
                        _gameManager.ToggleShinyCaughtStatus(number, gameName, newStatus);
                    else
                        _gameManager.ToggleCaughtStatus(number, gameName, newStatus);

                    var updatedData = useShiny ? _gameManager.GetShinyPokemonData(gameName)
                                               : _gameManager.GetPokemonData(gameName);
                    int updatedTotal = updatedData.Total;
                    int updatedCaught = updatedData.Caught;

                    lblProgress.Visible = false;
                    UpdateProgressAndTrainer(gameName, updatedTotal, updatedCaught);
                };

                panelCards.Controls.Add(card);
            }

            // Reposition cards now that they are all added.
            RepositionPokemonCards();

            // Resume layout and show the panel.
            panelCards.ResumeLayout(true);
            panelCards.Visible = true;
        }


        private void panelCards_Resize(object sender, EventArgs e)
        {
            RepositionPokemonCards();
        }

        private void RepositionPokemonCards()
        {
            // Remove the preservation of the old scroll position:
            // int scrollY = panelCards.VerticalScroll.Value;

            int cardWidth = 130;
            int cardHeight = 160;
            int spacingX = 10;
            int spacingY = 10;
            int count = 0;

            int cardsPerRow = Math.Max(1, (panelCards.ClientSize.Width - spacingX) / cardWidth);
            int xPos = spacingX;
            int yPos = spacingY;

            foreach (Control ctrl in panelCards.Controls)
            {
                ctrl.Location = new Point(xPos, yPos);
                count++;
                if (count % cardsPerRow == 0)
                {
                    xPos = spacingX;
                    yPos += cardHeight;
                }
                else
                {
                    xPos += cardWidth;
                }
            }

            int totalRows = (int)Math.Ceiling((double)panelCards.Controls.Count / cardsPerRow);
            int totalHeight = spacingY + (totalRows * cardHeight);
            panelCards.AutoScrollMinSize = new Size(0, totalHeight);

            // Reset the scroll position to the top:
            panelCards.AutoScrollPosition = new Point(0, 0);
        }





        /// <summary>
        /// Updates both the progress display and the trainer card image.
        /// </summary>
        private void UpdateProgressAndTrainer(string gameName, int total = 0, int caught = 0)
        {
            // If total and caught are not provided, retrieve them.
            if (total == 0 && caught == 0)
            {
                bool useShiny = chkShiny.Enabled && chkShiny.Checked;
                var data = useShiny ? _gameManager.GetShinyPokemonData(gameName)
                                    : _gameManager.GetPokemonData(gameName);
                total = data.Total;
                caught = data.Caught;
            }

            lblProgress.Visible = false;

            UpdateTrainerSprite(caught, total, gameName);

            // Defer progress update until trainer card is updated.
            this.BeginInvoke(new Action(() =>
            {
                string progressText = $"{caught} / {total}";
                bool useShiny = chkShiny.Enabled && chkShiny.Checked;
                _progressDisplayManager.UpdateProgressLabel(gameName, lblProgress, progressText);
                lblProgress.Visible = true;
            }));
        }

        /// <summary>
        /// Simple helper to show the trainer card and progress label.
        /// </summary>
        private void SetTrainerVisibility()
        {
            trainerCard.Visible = true;
            lblProgress.Visible = true;
            lblProgress.BringToFront();
        }

        /// <summary>
        /// Updates the trainer sprite image.
        /// For shiny mode, builds the path with the "shiny" folder; otherwise, uses the original path.
        /// </summary>
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

            // Check if shiny mode is enabled
            bool isShiny = chkShiny.Checked;

            // Use AssetManager to get the path, considering shiny mode
            string badgeImagePath;

            badgeImagePath = _assetManager.GetTrainerBadgePath(currentGameName, badgeCount, selectedGender);


            // Check if the image exists
            if (File.Exists(badgeImagePath))
            {
                trainerCard.Image = Image.FromFile(badgeImagePath);
            }
            else
            {
                MessageBox.Show($"Badge image not found: {badgeImagePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        /// <summary>
        /// Handles the shiny checkbox state change.
        /// When changed (if enabled), reloads the cards and updates the display.
        /// </summary>
        private void chkShiny_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShiny.Enabled)
            {
                if (comboBoxGames.SelectedItem is string gameName)
                {
                    LoadPokemonCards(gameName);
                }
            }
        }

        /// <summary>
        /// Handles the gender radio button state changes.
        /// Updates the stored gender and refreshes the trainer card.
        /// </summary>
        private void rdoGender_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoBoy.Enabled || rdoGirl.Enabled)
            {
                if (rdoBoy.Checked)
                    selectedGender = "Boy";
                else if (rdoGirl.Checked)
                    selectedGender = "Girl";

                if (comboBoxGames.SelectedItem is string gameName)
                {
                    bool useShiny = chkShiny.Checked;

                    var data = useShiny ? _gameManager.GetShinyPokemonData(gameName) : _gameManager.GetPokemonData(gameName);
                    UpdateTrainerSprite(data.Caught, data.Total, gameName);
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
