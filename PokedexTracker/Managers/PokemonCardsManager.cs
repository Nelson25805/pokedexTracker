using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PokedexTracker.Helpers;

namespace PokedexTracker.Managers
{
    public class PokemonCardsManager
    {
        private readonly Panel _panel;
        private readonly GameManager _gameManager;
        private readonly AssetManager _assetManager;
        private List<(string Name, string Number, string SpritePath, bool IsCaught)> _allPokemonData;
        private readonly Dictionary<string, (List<(string Name, string Number, string SpritePath, bool IsCaught)> Data, int Total, int Caught)> _pokemonCache
            = new Dictionary<string, (List<(string, string, string, bool)>, int, int)>();
        private bool _useShiny;

        // Store the current game name so click events can use it.
        private string _currentGameName;

        // Event to let the MainForm update the progress display.
        public event Action<int, int> ProgressUpdated;

        public PokemonCardsManager(Panel panel, GameManager gameManager, AssetManager assetManager)
        {
            _panel = panel;
            _gameManager = gameManager;
            _assetManager = assetManager;
        }

        /// <summary>
        /// Loads the Pokémon cards from the database (or cache) and displays them.
        /// </summary>
        public async Task LoadPokemonCardsAsync(string gameName, bool useShiny, CancellationToken token)
        {
            _currentGameName = gameName;
            _useShiny = useShiny;

            // Reset scroll and layout.
            _panel.AutoScrollPosition = new Point(0, 0);
            _panel.SuspendLayout();
            _panel.Visible = false;
            _panel.Controls.Clear();

            // Check for cancellation before starting.
            token.ThrowIfCancellationRequested();

            (List<(string Name, string Number, string SpritePath, bool IsCaught)> PokemonData, int Total, int Caught) dataResult;

            if (_pokemonCache.ContainsKey(gameName) && !useShiny)
            {
                var cached = _pokemonCache[gameName];
                dataResult = (cached.Data, cached.Total, cached.Caught);
            }
            else
            {
                dataResult = await Task.Run(() =>
                {
                    token.ThrowIfCancellationRequested();
                    return useShiny
                        ? _gameManager.GetShinyPokemonData(gameName)
                        : _gameManager.GetPokemonData(gameName);
                }, token);

                if (!useShiny)
                {
                    _pokemonCache[gameName] = (dataResult.PokemonData, dataResult.Total, dataResult.Caught);
                }
            }

            // Check for cancellation before updating the UI.
            token.ThrowIfCancellationRequested();

            _allPokemonData = dataResult.PokemonData;

            // Notify the MainForm about the new progress data.
            ProgressUpdated?.Invoke(dataResult.Total, dataResult.Caught);

            // Display cards with no filter initially.
            DisplayFilteredPokemon(string.Empty);

            _panel.ResumeLayout(true);
            _panel.Visible = true;
        }


        /// <summary>
        /// Filters the full Pokémon list and adds cards to the panel.
        /// </summary>
        public void DisplayFilteredPokemon(string filter)
        {
            _panel.Controls.Clear();

            IEnumerable<(string Name, string Number, string SpritePath, bool IsCaught)> filteredList;

            if (string.IsNullOrWhiteSpace(filter))
            {
                filteredList = _allPokemonData;
            }
            else
            {
                string lowerFilter = filter.ToLower();
                filteredList = _allPokemonData.Where(p =>
                    p.Name.ToLower().Contains(lowerFilter) ||
                    p.Number.Contains(lowerFilter));
            }

            foreach (var item in filteredList)
            {
                var card = new PokemonCard(item.Name, item.Number, item.SpritePath, item.IsCaught);

                // Attach click event to toggle caught status.
                card.Click += (s, e) =>
                {
                    bool newStatus = !card.IsCaught;
                    card.UpdateCaughtStatus(newStatus);

                    // Update in-memory list.
                    int index = _allPokemonData.FindIndex(p => p.Number == item.Number);
                    if (index != -1)
                    {
                        _allPokemonData[index] = (item.Name, item.Number, item.SpritePath, newStatus);
                    }

                    if (_useShiny)
                    {
                        _gameManager.ToggleShinyCaughtStatus(item.Number, _currentGameName, newStatus);
                        var data = _gameManager.GetShinyPokemonData(_currentGameName);
                        ProgressUpdated?.Invoke(data.Total, data.Caught);
                    }
                    else
                    {
                        _gameManager.ToggleCaughtStatus(item.Number, _currentGameName, newStatus);
                        var data = _gameManager.GetPokemonData(_currentGameName);
                        ProgressUpdated?.Invoke(data.Total, data.Caught);
                    }

                };

                _panel.Controls.Add(card);
            }

            // Reposition cards after adding them.
            RepositionPokemonCards();
        }

        /// <summary>
        /// Arranges the Pokémon cards within the panel.
        /// </summary>
        public void RepositionPokemonCards()
        {
            _panel.AutoScrollMinSize = Size.Empty;

            int cardWidth = 120;
            int cardHeight = 170;
            int paddingX = 10;
            int paddingY = 10;

            int cardsPerRow = Math.Max(1, (_panel.ClientSize.Width + paddingX) / (cardWidth + paddingX));
            int xPos = paddingX;
            int yPos = paddingY;
            int count = 0;

            foreach (Control ctrl in _panel.Controls)
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

            int totalRows = (int)Math.Ceiling((double)_panel.Controls.Count / cardsPerRow);
            int computedHeight = paddingY + totalRows * (cardHeight + paddingY);
            int totalHeight = Math.Max(computedHeight, _panel.ClientSize.Height);

            _panel.AutoScrollMinSize = new Size(0, totalHeight);
            _panel.AutoScrollPosition = new Point(0, 0);
            _panel.Invalidate();
        }
    }
}
