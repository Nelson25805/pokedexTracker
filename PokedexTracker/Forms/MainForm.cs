﻿using System;
using System.Drawing;
using System.IO;
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

            // Set the images for the custom radio buttons.
            SetRadioButtonImages();
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

            _gameManager = new GameManager(_dbManager);
            _nameDisplayManager = new PlayerNameDisplayManager();
            _progressDisplayManager = new ProgressDisplayManager();

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
        /// Loads the images for the custom radio buttons.
        /// </summary>
        private void SetRadioButtonImages()
        {
            // Get the file paths from AssetManager.
            string checkedPath = _assetManager.GetRadioButtonImagePath("checkedPokeball.png");
            string uncheckedPath = _assetManager.GetRadioButtonImagePath("uncheckedPokeball.png");

            // Load images from the file paths.
            Image checkedImage = Image.FromFile(checkedPath);
            Image uncheckedImage = Image.FromFile(uncheckedPath);

            // Assign the images to your custom radio buttons.
            pokeballRadioButtonBoy.CheckedImage = checkedImage;
            pokeballRadioButtonBoy.UncheckedImage = uncheckedImage;
            pokeballRadioButtonBoy.Text = "Boy";

            pokeballRadioButtonGirl.CheckedImage = checkedImage;
            pokeballRadioButtonGirl.UncheckedImage = uncheckedImage;
            pokeballRadioButtonGirl.Text = "Girl";
        }

        /// <summary>
        /// Handles when the user selects a game from the dropdown.
        /// </summary>
        private void comboBoxGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxGames.SelectedItem is string gameName)
            {
                // Update the shiny checkbox state based on the selected game.
                int silverIndex = comboBoxGames.Items.IndexOf("Silver");
                if (comboBoxGames.SelectedIndex >= silverIndex)
                {
                    chkShiny.Enabled = true;
                }
                else
                {
                    chkShiny.Enabled = false;
                    chkShiny.Checked = false;  // Ensure shiny is off for non-shiny games.
                }

                // Update the gender radio buttons based on the selected game.
                int goldIndex = comboBoxGames.Items.IndexOf("Gold");
                if (comboBoxGames.SelectedIndex > goldIndex)
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

                // Load the Pokémon cards (which also updates progress and trainer card display).
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

            // Add each Pokémon card.
            foreach (var item in pokemonData)
            {
                string name = item.Name;
                string number = item.Number;
                string spritePath = item.SpritePath;
                bool isCaught = item.IsCaught;

                var card = new PokemonCard(name, number, spritePath, isCaught);
                card.Location = new Point(0, 0); // Temporary; will be repositioned.

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

            // Reposition cards.
            RepositionPokemonCards();

            // Resume layout and show the panel.
            panelCards.ResumeLayout(true);
            panelCards.Visible = true;
        }

        /// <summary>
        /// Repositions the Pokémon cards within the panel.
        /// </summary>
        private void RepositionPokemonCards()
        {
            int cardWidth = 120;
            int cardHeight = 170;
            int paddingX = 10;
            int paddingY = 10;

            int cardsPerRow = Math.Max(1, (panelCards.ClientSize.Width + paddingX) / (cardWidth + paddingX));
            int xPos = paddingX;
            int yPos = paddingY;
            int count = 0;

            foreach (Control ctrl in panelCards.Controls)
            {
                ctrl.Location = new Point(xPos, yPos);
                count++;

                if (count % cardsPerRow == 0)
                {
                    xPos = paddingX;
                    yPos += cardHeight + paddingY;
                }
                else
                {
                    xPos += cardWidth + paddingX;
                }
            }

            int totalRows = (int)Math.Ceiling((double)panelCards.Controls.Count / cardsPerRow);
            int totalHeight = paddingY + totalRows * (cardHeight + paddingY);
            panelCards.AutoScrollMinSize = new Size(0, totalHeight);
        }

        private void panelCards_Resize(object sender, EventArgs e)
        {
            panelCards.AutoScrollPosition = new Point(0, 0);
            RepositionPokemonCards();
        }

        /// <summary>
        /// Updates both the progress display and the trainer card image.
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
            UpdateTrainerSprite(caught, total, gameName);

            this.BeginInvoke(new Action(() =>
            {
                string progressText = $"{caught} / {total}";
                bool useShiny = chkShiny.Enabled && chkShiny.Checked;
                _progressDisplayManager.UpdateProgressLabel(gameName, lblProgress, progressText);
                lblProgress.Visible = true;
            }));
        }

        /// <summary>
        /// Makes the trainer card and progress label visible.
        /// </summary>
        private void SetTrainerVisibility()
        {
            trainerCard.Visible = true;
            lblProgress.Visible = true;
            lblProgress.BringToFront();
        }

        /// <summary>
        /// Updates the trainer sprite image based on the current progress and gender.
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
            bool isShiny = chkShiny.Checked;

            string badgeImagePath = _assetManager.GetTrainerBadgePath(currentGameName, badgeCount, selectedGender);

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
        /// This event is now attached to the custom PokeballRadioButton controls.
        /// </summary>
        private void rdoGender_CheckedChanged(object sender, EventArgs e)
        {
            // Only proceed if at least one of the custom radio buttons is enabled.
            if (pokeballRadioButtonBoy.Enabled || pokeballRadioButtonGirl.Enabled)
            {
                if (pokeballRadioButtonBoy.Checked)
                    selectedGender = "Boy";
                else if (pokeballRadioButtonGirl.Checked)
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
