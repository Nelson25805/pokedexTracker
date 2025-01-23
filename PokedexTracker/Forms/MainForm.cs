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
            panelCards.Controls.Clear();
            var pokemonData = _gameManager.GetPokemonData(gameName);

            int xPos = 10, yPos = 10, count = 0;
            int cardsPerRow = panelCards.Width / 130;

            foreach (var (name, number, spritePath, isCaught) in pokemonData)
            {
                var card = new PokemonCard(name, number, spritePath, isCaught);
                card.Location = new System.Drawing.Point(xPos, yPos);

                card.Click += (s, e) =>
                {
                    var newStatus = !card.IsCaught;
                    card.UpdateCaughtStatus(newStatus);
                    _gameManager.ToggleCaughtStatus(number, gameName, newStatus);
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
    }
}
