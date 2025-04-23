// ProgressDisplayManager.cs
using System.Drawing;
using System.Drawing.Text;
using PokedexTracker.Helpers;
using System.Collections.Generic;

namespace PokedexTracker.DisplayManagers
{
    public class ProgressDisplayManager
    {
        private readonly Dictionary<string, (string FontFile, float SizePx, Point Loc)> _styles
            = new Dictionary<string, (string, float, Point)>
        {
            { "Red",         ("Gen1+2.ttf",  9f, new Point(120,43)) },
            { "Blue",        ("Gen1+2.ttf",  9f, new Point(120,43)) },
            { "Yellow",      ("Gen1+2.ttf",  9f, new Point(120,43)) },
            { "Gold",        ("Gen1+2.ttf",  9f, new Point(125,36)) },
            { "Silver",      ("Gen1+2.ttf",  6f, new Point(73,33)) },
            { "Crystal",     ("Gen1+2.ttf",  9f, new Point(125,36)) },
            { "Ruby",        ("Gen3.ttf",   12f, new Point(77,73)) },
            { "Sapphire",    ("Gen3.ttf",   12f, new Point(77,73)) },
            { "Emerald",     ("Gen3.ttf",   12f, new Point(77,73)) },
            { "Fire Red",    ("Gen3.ttf",   11f, new Point(85,70)) },
            { "Leaf Green",  ("Gen3.ttf",   11f, new Point(85,70)) },
            { "Diamond",     ("Gen3.ttf",   11f, new Point(85,70)) },
            { "Pearl",       ("Gen3.ttf",   11f, new Point(85,70)) },
            { "Platinum",    ("Gen3.ttf",   11f, new Point(85,70)) },
            { "Heart Gold",  ("Gen3.ttf",   11f, new Point(85,70)) },
            { "Soul Silver", ("Gen3.ttf",   11f, new Point(85,70)) }
        };
        private readonly PrivateFontCollection _fonts;
        public ProgressDisplayManager()
        {
            _fonts = FontLoader.LoadFontCollection(new[] { "Gen1+2.ttf", "Gen3.ttf" });
        }
        public FontSettings GetFontSettings(string gameName)
        {
            if (!_styles.TryGetValue(gameName, out var s))
                s = ("Gen1+2.ttf", 9f, new Point(100, 50));
            var fam = FontLoader.GetFamilyFromCollection(_fonts, s.FontFile);
            return new FontSettings(fam, s.SizePx, s.Loc);
        }
    }
}