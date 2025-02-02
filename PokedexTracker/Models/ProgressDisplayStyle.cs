using System;
using System.Drawing;

namespace PokedexTracker.DisplayManagers
{
    public class ProgressDisplayStyle
    {
        public Point Location { get; set; }
        public Size? Size { get; set; }  // Optional size override.
        public string FontFileName { get; set; }  // E.g., "RedProgressFont.ttf"
        public float FontSize { get; set; }
        public FontStyle FontStyle { get; set; }
        public Color ForeColor { get; set; }
        public Color BackColor { get; set; }  // Use Color.Transparent for no background.
    }
}
