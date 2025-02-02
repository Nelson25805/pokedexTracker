using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PokedexTracker.Helpers;  // Ensure FontLoader is in Helpers.

namespace PokedexTracker.DisplayManagers
{
    public class ProgressDisplayManager
    {
        private readonly AssetManager _assetManager;
        private readonly Dictionary<string, ProgressDisplayStyle> _styles;

        public ProgressDisplayManager()
        {
            _assetManager = new AssetManager();
            // Initialize styles for each game.
            // (Update the values as needed for your layout and custom font files.)
            _styles = new Dictionary<string, ProgressDisplayStyle>
            {
                { "Red", new ProgressDisplayStyle
                    {
                        Location = new Point(120, 45),
                        FontFileName = "PKMN RBYGSC.ttf",
                        FontSize = 7,
                        BackColor = Color.Transparent
                    }
                },
                { "Blue", new ProgressDisplayStyle
                    {
                        Location = new Point(120, 45),
                        FontFileName = "PKMN RBYGSC.ttf",
                        FontSize = 7,
                        BackColor = Color.Transparent
                    }
                },
                { "Yellow", new ProgressDisplayStyle
                    {
                        Location = new Point(120, 45),
                        FontFileName = "PKMN RBYGSC.ttf",
                        FontSize = 7,
                        BackColor = Color.Transparent
                    }
                },
                { "Gold", new ProgressDisplayStyle
                    {
                        Location = new Point(123, 39),
                        FontFileName = "PKMN RBYGSC.ttf",
                        //FontSize = 7,
                        FontSize = 6.7F,
                        BackColor = Color.Transparent
                    }
                },
                { "Silver", new ProgressDisplayStyle
                    {
                        Location = new Point(123, 39),
                        FontFileName = "PKMN RBYGSC.ttf",
                        //FontSize = 7,
                        FontSize = 6.7F,
                        BackColor = Color.Transparent
                    }
                },
                { "Crystal", new ProgressDisplayStyle
                    {
                        Location = new Point(123, 39),
                        FontFileName = "PKMN RBYGSC.ttf",
                        //FontSize = 7,
                        FontSize = 6.7F,
                        BackColor = Color.Transparent
                    }
                },
                // Add additional styles for other games as needed.
            };
        }

        /// <summary>
        /// Applies the style for the given game to the progress label.
        /// </summary>
        /// <param name="gameName">Name of the game (used as key).</param>
        /// <param name="progressLabel">The Label control to update.</param>
        /// <param name="progressText">The text to display (e.g., "5 / 151").</param>
        public void UpdateProgressLabel(string gameName, Label progressLabel, string progressText)
        {
            if (_styles.TryGetValue(gameName, out ProgressDisplayStyle style))
            {
                progressLabel.Location = style.Location;
                if (style.Size.HasValue)
                {
                    progressLabel.Size = style.Size.Value;
                }

                // Load the custom font using the AssetManager and FontLoader.
                string fontPath = _assetManager.GetFontPath(style.FontFileName);
                Font customFont = FontLoader.LoadFont(fontPath, style.FontSize, style.FontStyle);

                // If the custom font fails, fall back to a default font.
                progressLabel.Font = customFont ?? new Font("Arial", style.FontSize, style.FontStyle);
                progressLabel.ForeColor = style.ForeColor;
                progressLabel.BackColor = style.BackColor;
                progressLabel.Text = progressText;
                progressLabel.Visible = true;
            }
            else
            {
                // Use a default style if no style is defined for the game.
                progressLabel.Location = new Point(300, 150);
                progressLabel.Size = new Size(150, 40);
                progressLabel.Font = new Font("Arial", 14, FontStyle.Bold);
                progressLabel.ForeColor = Color.White;
                progressLabel.BackColor = Color.Transparent;
                progressLabel.Text = progressText;
                progressLabel.Visible = true;
            }
        }
    }
}
