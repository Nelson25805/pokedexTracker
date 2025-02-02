using System.Drawing;

namespace PokedexTracker.DisplayManagers
{
    public class PlayerNameStyle
    {
        public Point Location { get; set; }
        public string FontFileName { get; set; }  // e.g., "MyCustomFont.ttf"
        public float FontSize { get; set; }
        public FontStyle FontStyle { get; set; }
        public Size? Size { get; set; }  // Optional size
    }
}
