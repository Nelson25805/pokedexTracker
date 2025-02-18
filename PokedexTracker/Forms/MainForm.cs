﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        // Holds the full list of Pokemon data from the last query.
        private List<(string Name, string Number, string SpritePath, bool IsCaught)> _allPokemonData;

        // A simple cache keyed by game name.
        private Dictionary<string, (List<(string Name, string Number, string SpritePath, bool IsCaught)> Data, int Total, int Caught)> _pokemonCache
            = new Dictionary<string, (List<(string, string, string, bool)>, int, int)>();

        private int _lastBadgeCount = -1;




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
                LoadPokemonCardsAsync(gameName);

                // Update the player name display.
                _nameDisplayManager.UpdatePlayerNameLabel(gameName, lblPlayerName, playerName);

                // Update progress and trainer card display.
                UpdateProgressAndTrainer(gameName);
            }
        }

        /// <summary>
        /// Loads Pokémon cards depending on whether shiny mode is active.
        /// </summary>
        private async void LoadPokemonCardsAsync(string gameName)
        {
            // Reset scroll position and suspend layout.
            panelCards.AutoScrollPosition = new Point(0, 0);
            panelCards.SuspendLayout();
            panelCards.Visible = false;
            panelCards.Controls.Clear();

            (List<(string Name, string Number, string SpritePath, bool IsCaught)> pokemonData, int total, int caught) dataResult;

            bool useShiny = chkShiny.Enabled && chkShiny.Checked;

            // Check if we have cached data
            if (_pokemonCache.ContainsKey(gameName) && !useShiny)
            {
                dataResult = _pokemonCache[gameName];
            }
            else
            {
                // Run the database query on a background thread.
                dataResult = await Task.Run(() =>
                {
                    return useShiny
                        ? _gameManager.GetShinyPokemonData(gameName)
                        : _gameManager.GetPokemonData(gameName);
                });

                // Cache the data if not shiny (or adjust your caching strategy as needed)
                if (!useShiny)
                {
                    _pokemonCache[gameName] = dataResult;
                }
            }

            // Update progress display.
            UpdateProgressAndTrainer(gameName, dataResult.Item2, dataResult.Item3);
            SetTrainerVisibility();

            // Store full list for search filtering.
            _allPokemonData = dataResult.Item1;


            // Display all cards (or filtered based on searchTextBox).
            DisplayFilteredPokemon(searchTextBox.Text.Trim());

            panelCards.ResumeLayout(true);
            panelCards.Visible = true;
        }

        private void DisplayFilteredPokemon(string filter)
        {
            // Clear existing cards.
            panelCards.Controls.Clear();

            IEnumerable<(string Name, string Number, string SpritePath, bool IsCaught)> filteredList;

            if (string.IsNullOrWhiteSpace(filter))
            {
                // No filtering – display the full list.
                filteredList = _allPokemonData;
            }
            else
            {
                // Convert filter to lowercase for case-insensitive search.
                string lowerFilter = filter.ToLower();
                filteredList = _allPokemonData.Where(p =>
                    p.Name.ToLower().Contains(lowerFilter) ||
                    p.Number.Contains(lowerFilter));
            }

            // Add a PokemonCard for each filtered item.
            foreach (var item in filteredList)
            {
                var card = new PokemonCard(item.Name, item.Number, item.SpritePath, item.IsCaught);

                // (Optional) Attach your click event to toggle caught status:
                card.Click += (s, e) =>
                {
                    bool newStatus = !card.IsCaught;
                    card.UpdateCaughtStatus(newStatus);

                    // Update the corresponding record in _allPokemonData.
                    int index = _allPokemonData.FindIndex(p => p.Number == item.Number);
                    if (index != -1)
                    {
                        _allPokemonData[index] = (item.Name, item.Number, item.SpritePath, newStatus);
                    }

                    if (chkShiny.Enabled && chkShiny.Checked)
                        _gameManager.ToggleShinyCaughtStatus(item.Number, comboBoxGames.SelectedItem.ToString(), newStatus);
                    else
                        _gameManager.ToggleCaughtStatus(item.Number, comboBoxGames.SelectedItem.ToString(), newStatus);

                    // Update the progress display (remains overall)
                    bool useShiny = chkShiny.Enabled && chkShiny.Checked;
                    var updatedData = useShiny ? _gameManager.GetShinyPokemonData(comboBoxGames.SelectedItem.ToString())
                                               : _gameManager.GetPokemonData(comboBoxGames.SelectedItem.ToString());
                    UpdateProgressAndTrainer(comboBoxGames.SelectedItem.ToString(), updatedData.Total, updatedData.Caught);
                };


                panelCards.Controls.Add(card);
            }

            // Reposition the cards according to your layout logic.
            RepositionPokemonCards();
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            // Filter the displayed cards based on the current text.
            DisplayFilteredPokemon(searchTextBox.Text.Trim());
        }




        /// <summary>
        /// Repositions the Pokémon cards within the panel.
        /// </summary>
        private void RepositionPokemonCards()
        {
            // Clear any previous AutoScrollMinSize to avoid accumulation.
            panelCards.AutoScrollMinSize = Size.Empty;

            int cardWidth = 120;
            int cardHeight = 170;
            int paddingX = 10;
            int paddingY = 10;

            // Calculate how many cards per row fit in the current client width.
            int cardsPerRow = Math.Max(1, (panelCards.ClientSize.Width + paddingX) / (cardWidth + paddingX));
            int xPos = paddingX;
            int yPos = paddingY;
            int count = 0;

            // Reposition each card.
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

            // Calculate the total number of rows needed.
            int totalRows = (int)Math.Ceiling((double)panelCards.Controls.Count / cardsPerRow);
            // Compute the height required by the content.
            int computedHeight = paddingY + totalRows * (cardHeight + paddingY);
            // Ensure that the scrollable height is at least as big as the panel's client height.
            int totalHeight = Math.Max(computedHeight, panelCards.ClientSize.Height);

            // Set the scrollable area.
            panelCards.AutoScrollMinSize = new Size(0, totalHeight);

            // Reset the scroll position.
            panelCards.AutoScrollPosition = new Point(0, 0);
            panelCards.Invalidate();
        }


        private void panelCards_Resize(object sender, EventArgs e)
        {
            panelCards.SuspendLayout();
            // Reset the scroll position.
            panelCards.AutoScrollPosition = new Point(0, 0);
            RepositionPokemonCards();
            // Force the first card into view.
            if (panelCards.Controls.Count > 0)
            {
                panelCards.ScrollControlIntoView(panelCards.Controls[0]);
            }
            panelCards.ResumeLayout();
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
                total = data.Item2;
                caught = data.Item3;
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
        private async void UpdateTrainerSpriteAsync(int caughtCount, int totalCount, string currentGameName)
        {
            if (trainerCard == null || totalCount == 0)
                return;

            int badgeThreshold = totalCount / 8;
            // Calculate the new badge count.
            int newBadgeCount = Math.Min(caughtCount / badgeThreshold, 8);

            // If the badge count hasn't changed, don't update the image.
            if (newBadgeCount == _lastBadgeCount)
                return;

            // Badge count changed, so update our stored value.
            _lastBadgeCount = newBadgeCount;

            // Optionally, hide the trainer card briefly to show the update.
            // (This is optional; if hiding causes flicker, you might choose not to hide it.)
            trainerCard.Visible = false;

            // Get the badge image path using the newBadgeCount.
            string badgeImagePath = _assetManager.GetTrainerBadgePath(currentGameName, newBadgeCount, selectedGender);
            if (!File.Exists(badgeImagePath))
            {
                MessageBox.Show($"Badge image not found: {badgeImagePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Load the image asynchronously.
            Image badgeImage = await Task.Run(() => Image.FromFile(badgeImagePath));

            // Update the trainer card image on the UI thread.
            trainerCard.Invoke((MethodInvoker)(() =>
            {
                trainerCard.Image = badgeImage;
                trainerCard.Visible = true;
            }));
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
                    LoadPokemonCardsAsync(gameName);
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
