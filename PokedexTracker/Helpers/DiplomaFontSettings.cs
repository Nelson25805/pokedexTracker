using System.Drawing;

namespace PokedexTracker.Helpers
{
    public struct DiplomaFontSettings
    {
        public FontFamily Family;
        public float Size;
        public Point Location;
        public Color TextColor;

        public DiplomaFontSettings(FontFamily family, float size, Point location, Color textColor)
        {
            Family = family;
            Size = size;
            Location = location;
            TextColor = textColor;
        }
    }
}
