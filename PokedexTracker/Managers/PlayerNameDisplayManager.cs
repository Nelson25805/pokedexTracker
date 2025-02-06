using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PokedexTracker.Helpers;

namespace PokedexTracker.DisplayManagers
{
    public class PlayerNameDisplayManager
    {
        // Dictionary mapping game names to a tuple with font file name, size, style, location, and optional size.
        private readonly Dictionary<string, (string FontFileName, float FontSize, Point Location)> _fontStyles;
        private readonly AssetManager _assetManager;

        public PlayerNameDisplayManager()
        {
            _assetManager = new AssetManager();
            _fontStyles = new Dictionary<string, (string, float, Point)>
            {
                // For Gen1/2 games
                { "Red",       ("Gen1+2.ttf", 9f,  new Point(90, 20)) },
                { "Blue",      ("Gen1+2.ttf", 9f,  new Point(90, 20)) },
                { "Yellow",    ("Gen1+2.ttf", 9f,  new Point(90, 20)) },
                { "Gold",      ("Gen1+2.ttf", 9f,  new Point(84, 16)) },
                { "Silver",    ("Gen1+2.ttf", 9f,  new Point(84, 16)) },
                { "Crystal",   ("Gen1+2.ttf", 9f,  new Point(84, 16)) },
                // For Gen3 games
                { "Ruby",      ("Gen3.ttf",   12f, new Point(59, 47)) },
                { "Sapphire",  ("Gen3.ttf",   12f, new Point(59, 47)) },
                { "Emerald",   ("Gen3.ttf",   12f, new Point(59, 47)) },
                { "Fire Red",  ("Gen3.ttf",   11f, new Point(70, 40)) },
                { "Leaf Green",("Gen3.ttf",   11f, new Point(70, 40)) }
                // Add more styles for additional games as needed.
            };
        }

        /// <summary>
        /// Updates the player name label using a fixed base font loaded from an embedded resource.
        /// </summary>
        public void UpdatePlayerNameLabel(string gameName, Label playerNameLabel, string playerName)
        {
            // Get the style from the dictionary; if not found, use a default style.
            if (!_fontStyles.TryGetValue(gameName, out var style))
            {
                style = ("Gen1+2.ttf", 20f, new Point(50, 50));
            }

            // Set the label's location (and size if provided).
            playerNameLabel.Location = style.Location;

            // Build the resource name.  (Make sure the font file is added as an Embedded Resource in the "Fonts" folder.)
            string resourceName = $"PokedexTracker.Assets.Fonts.{style.FontFileName}";

            Font customFont = null;
            try
            {
                customFont = FontLoader.LoadEmbeddedFont(resourceName, style.FontSize);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading font '{resourceName}': {ex.Message}", "Font Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Always create a new font instance from the fixed settings.
            playerNameLabel.Font = customFont ?? new Font("Arial", style.FontSize);
            playerNameLabel.Text = playerName;
            playerNameLabel.Visible = true;
        }
    }
}
