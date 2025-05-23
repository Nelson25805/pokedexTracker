﻿using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using PokedexTracker.DisplayManagers;
using PokedexTracker.Managers;
using PokedexTracker.Helpers;
using System.Threading;
using PokedexTracker.Forms;

namespace PokedexTracker
{
    public partial class MainForm : Form
    {
        private readonly DatabaseManager _dbManager;
        private readonly GameManager _gameManager;
        private readonly AssetManager _assetManager;
        private readonly PlayerNameDisplayManager _nameDisplayManager;
        private readonly ProgressDisplayManager _progressDisplayManager;
        private PokemonCardsManager _pokemonCardsManager;

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

            _pokemonCardsManager = new PokemonCardsManager(panelCards, _gameManager, _assetManager);
            _pokemonCardsManager.ProgressUpdated += (total, caught) =>
            {
                if (comboBoxGames.SelectedItem is string gameName)
                    UpdateProgressAndTrainer(gameName, total, caught);
            };
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (comboBoxGames.Items.Count > 0)
            {
                comboBoxGames.SelectedIndex = 0;
            }

            //Finding font names
            //var coll = FontLoader.LoadFontCollection(new[] { "Gen1+2.ttf" });
            //foreach (var f in coll.Families)
            //    MessageBox.Show($"Found loaded font family '{f.Name}'");
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
                int goldIndex = comboBoxGames.Items.IndexOf("Gold");
                chkShiny.Enabled = comboBoxGames.SelectedIndex >= goldIndex;
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
            // 1) Fetch totals if not provided
            if (total == 0 && caught == 0)
            {
                bool useShiny = chkShiny.Enabled && chkShiny.Checked;
                var data = useShiny
                    ? _gameManager.GetShinyPokemonData(gameName)
                    : _gameManager.GetPokemonData(gameName);
                total = data.Total;
                caught = data.Caught;
            }

            // 2) Hide old label and redraw trainer sprite
            lblProgress.Visible = false;
            UpdateTrainerSpriteAsync(caught, total, gameName);

            // 3) Diploma‐button logic
            bool enableDiploma = false;

            if (gameName == "Yellow")
            {
                if (caught == total)
                {
                    // Fully complete (151/151)
                    enableDiploma = true;
                }
                else if (caught == total - 1)
                {
                    // Exactly one missing—check it's Mew (#151)
                    var missing = _gameManager.GetMissingPokemonIds(gameName,
                                        chkShiny.Enabled && chkShiny.Checked);
                    enableDiploma = (missing.Count == 1 && missing[0] == 151);
                }
            }
            else
            {
                // All other games require 100% completion
                enableDiploma = (caught == total);
            }

            diplomaButton.Enabled = enableDiploma;
        }




        /// <summary>
        /// Updates the trainer sprite based on progress and gender.
        /// </summary>
        private async void UpdateTrainerSpriteAsync(int caughtCount, int totalCount, string game)
        {
            if (trainerCard == null || totalCount == 0)
                return;

            int threshold = totalCount / 8;
            int badges = Math.Min(caughtCount / threshold, 8);

            // Always redraw (no early exit)
            lastTrainerState = (badges, selectedGender, game);

            trainerCard.Visible = false;

            // 1) Load the combined card+badge image
            string path = _assetManager.GetTrainerBadgePath(game, badges, selectedGender);
            if (!File.Exists(path))
            {
                MessageBox.Show($"Card not found: {path}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Image cardImage = await Task.Run(() => Image.FromFile(path));

            // 2) Pull font settings & prepare text
            var nameSet = _nameDisplayManager.GetFontSettings(game);
            var progSet = _progressDisplayManager.GetFontSettings(game);
            string progText = $"{caughtCount} / {totalCount}";

            // 3) Compose final bitmap with updated name & progress
            var finalBmp = CardComposer.ComposeTrainerCard(
                baseCard: cardImage,
                badgeImage: null,
                nameSettings: nameSet,
                playerName: playerName,
                progressSettings: progSet,
                progressText: progText
            );

            // 4) Display it
            trainerCard.Image = finalBmp;
            trainerCard.Visible = true;
        }


        /// <summary>
        /// Handles changes to the shiny checkbox state.
        /// </summary>
        private async void chkShiny_CheckedChanged(object sender, EventArgs e)
        {
            // don’t run until the cards manager is in place
            if (_pokemonCardsManager == null || comboBoxGames.SelectedItem == null)
                return;

            if (chkShiny.Enabled && comboBoxGames.SelectedItem is string gameName)
            {
                _gameSelectionCTS?.Cancel();
                _gameSelectionCTS = new CancellationTokenSource();
                var token = _gameSelectionCTS.Token;
                await _pokemonCardsManager.LoadPokemonCardsAsync(gameName, chkShiny.Checked, token);
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

        private void btnChangeName_Click(object sender, EventArgs e)
        {
            using (var dlg = new ChangeNameForm(playerName))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ApplyNewPlayerName(dlg.NewName);
                }
            }
        }

        // Pull out your update logic into one method:
        private void ApplyNewPlayerName(string newName)
        {
            playerName = newName;

            // Persist across sessions
            Properties.Settings.Default.playerName = playerName;
            Properties.Settings.Default.Save();

            // Refresh trainer card in case the name prints elsewhere
            UpdateProgressAndTrainer(comboBoxGames.SelectedItem as string ?? "",
                                     total: 0, caught: 0);
        }

        private void diplomaButton_Click(object sender, EventArgs e)
        {
            if (!(comboBoxGames.SelectedItem is string gameName))
                return;

            bool useShiny = chkShiny.Enabled && chkShiny.Checked;
            var data = useShiny
                ? _gameManager.GetShinyPokemonData(gameName)
                : _gameManager.GetPokemonData(gameName);

            // Only for Yellow: missingMew if caught == total–1 *and* that missing is #151
            bool missingMew = false;
            if (gameName == "Yellow" && data.Caught == data.Total - 1)
            {
                var missing = _gameManager.GetMissingPokemonIds(gameName, useShiny);
                missingMew = (missing.Count == 1 && missing[0] == 151);
            }

            using (var dlg = new DiplomaForm(gameName, playerName, missingMew))
            {
                // Force the “Printer” toggle if they were in printer mode before:
                // (You can still call SetPrinterDefault here if you like.)
                dlg.ShowDialog(this);
            }
        }
    }
}
