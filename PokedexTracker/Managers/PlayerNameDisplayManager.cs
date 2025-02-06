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
                // For Gen1/2 games, use one font; for Gen3 games, use another.
                { "Red", new PlayerNameStyle {
                    Location = new Point(90, 20),
                    FontFileName = "Gen1+2.ttf",
                    FontSize = 9,
                } },
                { "Blue", new PlayerNameStyle {
                    Location = new Point(90, 20),
                    FontFileName = "Gen1+2.ttf",
                    FontSize = 9,
                } },
                { "Yellow", new PlayerNameStyle {
                    Location = new Point(90, 20),
                    FontFileName = "Gen1+2.ttf",
                    FontSize = 9,
                } },
                { "Gold", new PlayerNameStyle {
                    Location = new Point(84, 16),
                    FontFileName = "Gen1+2.ttf",
                    FontSize = 9,
                } },
                { "Silver", new PlayerNameStyle {
                    Location = new Point(84, 16),
                    FontFileName = "Gen1+2.ttf",
                    FontSize = 9,
                } },
                { "Crystal", new PlayerNameStyle {
                    Location = new Point(84, 16),
                    FontFileName = "Gen1+2.ttf",
                    FontSize = 9,
                } },
                { "Ruby", new PlayerNameStyle {
                    Location = new Point(59, 47),
                    FontFileName = "Gen3.ttf",
                    FontSize = 12,
                } },
                { "Sapphire", new PlayerNameStyle {
                    Location = new Point(59, 47),
                    FontFileName = "Gen3.ttf",
                    FontSize = 12,
                } },
                { "Emerald", new PlayerNameStyle {
                    Location = new Point(59, 47),
                    FontFileName = "Gen3.ttf",
                    FontSize = 12,
                } },
                { "Fire Red", new PlayerNameStyle {
                    Location = new Point(70, 40),
                    FontFileName = "Gen3.ttf",
                    FontSize = 11,
                } },
                { "Leaf Green", new PlayerNameStyle {
                    Location = new Point(70, 40),
                    FontFileName = "Gen3.ttf",
                    FontSize = 11,
                } },
                // Add more styles as needed.
            };
        }

        /// <summary>
        /// Updates the player name label using a fixed base font loaded from an embedded resource.
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

                // Build the resource name. 
                // (Assuming the font file is in the "Fonts" folder and the namespace is PokedexTracker)
                string resourceName = $"PokedexTracker.Assets.Fonts.{style.FontFileName}";

                // Always create a new font from the fixed base settings.
                Font customFont = null;
                try
                {
                    customFont = FontLoader.LoadEmbeddedFont(resourceName, style.FontSize);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Error loading font '{resourceName}': {ex.Message}", "Font Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
