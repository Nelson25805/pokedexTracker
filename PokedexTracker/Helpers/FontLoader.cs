using System;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace PokedexTracker.Helpers
{
    public static class FontLoader
    {
        /// <summary>
        /// Loads a font from a TTF file.
        /// </summary>
        /// <param name="fontFilePath">The full path to the TTF file.</param>
        /// <param name="fontSize">The desired font size.</param>
        /// <param name="fontStyle">The font style (Regular, Bold, etc.).</param>
        /// <returns>A Font object if successful; otherwise, null.</returns>
        public static Font LoadFont(string fontFilePath, float fontSize, FontStyle fontStyle)
        {
            try
            {
                PrivateFontCollection pfc = new PrivateFontCollection();
                pfc.AddFontFile(fontFilePath);
                if (pfc.Families.Length > 0)
                {
                    return new Font(pfc.Families[0], fontSize, fontStyle);
                }
                else
                {
                    throw new Exception("No font families found in the file.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error) as needed.
                System.Diagnostics.Debug.WriteLine("Error loading font: " + ex.Message);
                return null;
            }
        }
    }
}
