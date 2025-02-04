using System.Drawing;
using System.Windows.Forms;
using PokedexTracker.Helpers;
using System.Collections.Generic;

namespace PokedexTracker.DisplayManagers
{
    public class PlayerNameDisplayManager
    {
        private readonly Dictionary<string, PlayerNameStyle> _styles;
        private readonly AssetManager _assetManager;

        public PlayerNameDisplayManager()
        {
            _assetManager = new AssetManager();
            _styles = new Dictionary<string, PlayerNameStyle>
            {
                { "Red", new PlayerNameStyle {
                    Location = new Point(90, 22),
                    FontFileName = "Gen1+2.ttf",
                    FontSize = 7,
                    Size = new Size(300, 50)
                } },
                { "Blue", new PlayerNameStyle {
                    Location = new Point(90, 22),
                    FontFileName = "Gen1+2.ttf",
                    FontSize = 7,
                    Size = new Size(300, 50)
                } },
                { "Yellow", new PlayerNameStyle {
                    Location = new Point(90, 22),
                    FontFileName = "Gen1+2.ttf",
                    FontSize = 7,
                    Size = new Size(300, 50)
                } },
                { "Gold", new PlayerNameStyle {
                    Location = new Point(84, 18),
                    FontFileName = "Gen1+2.ttf",
                    FontSize = 7,
                    Size = new Size(300, 50)
                } },
                { "Silver", new PlayerNameStyle {
                    Location = new Point(84, 18),
                    FontFileName = "Gen1+2.ttf",
                    FontSize = 7,
                    Size = new Size(300, 50)
                } },
                { "Crystal", new PlayerNameStyle {
                    Location = new Point(84, 18),
                    FontFileName = "Gen1+2.ttf",
                    FontSize = 7,
                    Size = new Size(300, 50)
                } },
                // Add more styles for other games as needed.
            };
        }

        /// <summary>
        /// Applies the player name display style based on the game name.
        /// </summary>
        public void UpdatePlayerNameLabel(string gameName, Label playerNameLabel, string playerName)
        {
            if (_styles.TryGetValue(gameName, out PlayerNameStyle style))
            {
                playerNameLabel.Location = style.Location;
                if (style.Size.HasValue)
                {
                    playerNameLabel.Size = style.Size.Value;
                }

                // Build the full font file path using AssetManager.
                string fontPath = _assetManager.GetFontPath(style.FontFileName);
                Font customFont = FontLoader.LoadFont(fontPath, style.FontSize, style.FontStyle);

                // If loading failed, fall back to a default font.
                playerNameLabel.Font = customFont ?? new Font("Arial", style.FontSize, style.FontStyle);
                playerNameLabel.Text = playerName;
                playerNameLabel.Visible = true;
            }
            else
            {
                // Default style if none is defined.
                playerNameLabel.Location = new Point(50, 50);
                playerNameLabel.Font = new Font("Arial", 20, FontStyle.Bold);
                playerNameLabel.Text = playerName;
                playerNameLabel.Visible = true;
            }
        }
    }
}
