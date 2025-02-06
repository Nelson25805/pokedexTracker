using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PokedexTracker.Helpers;

namespace PokedexTracker.DisplayManagers
{
    public class ProgressDisplayManager
    {
        private readonly Dictionary<string, (string FontFileName, float FontSize, Point Location)> _fontStyles;

        public ProgressDisplayManager()
        {
            _fontStyles = new Dictionary<string, (string, float, Point)>
            {
                { "Red", ("Gen1+2.ttf", 9f, new Point(120, 43)) },
                { "Blue", ("Gen1+2.ttf", 9f, new Point(120, 43)) },
                { "Yellow", ("Gen1+2.ttf", 9f, new Point(120, 43)) },
                { "Gold", ("Gen1+2.ttf", 9f, new Point(125, 36)) },
                { "Silver", ("Gen1+2.ttf", 9f, new Point(125, 36)) },
                { "Crystal", ("Gen1+2.ttf", 9f, new Point(125, 36)) },
                { "Ruby", ("Gen3.ttf", 12f, new Point(77, 73)) },
                { "Sapphire", ("Gen3.ttf", 12f, new Point(77, 73)) },
                { "Emerald", ("Gen3.ttf", 12f, new Point(77, 73)) },
                { "Fire Red", ("Gen3.ttf", 11f, new Point(85, 70)) },
                { "Leaf Green", ("Gen3.ttf", 11f, new Point(85, 70)) }
            };
        }

        public void UpdateProgressLabel(string gameName, Label progressLabel, string progressText)
        {
            if (!_fontStyles.TryGetValue(gameName, out var fontSettings))
            {
                // Default font settings if the game is not found in the dictionary
                fontSettings = ("Gen1+2.ttf", 14f, new Point(100, 50));
            }

            string resourceName = $"PokedexTracker.Assets.Fonts.{fontSettings.FontFileName}";
            Font customFont = null;

            try
            {
                customFont = FontLoader.LoadEmbeddedFont(resourceName, fontSettings.FontSize);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading font '{resourceName}': {ex.Message}", "Font Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            progressLabel.Font = customFont ?? new Font("Arial", fontSettings.FontSize);
            progressLabel.Text = progressText;
            progressLabel.Location = fontSettings.Location;
            progressLabel.Visible = true;
        }
    }
}
