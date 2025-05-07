// PlayerNameDisplayManager.cs
using System.Drawing;
using System.Drawing.Text;
using PokedexTracker.Helpers;
using System.Collections.Generic;

namespace PokedexTracker.DisplayManagers
{
    public class PlayerNameDisplayManager
    {
        private readonly Dictionary<string, (string FontFile, float SizePx, Point Loc)> _styles
            = new Dictionary<string, (string, float, Point)>
        {
            { "Red",        ("Gen1+2.ttf", 70f,  new Point(517,167)) },
            { "Blue",       ("Gen1+2.ttf", 70f,  new Point(517,167)) },
            { "Yellow",     ("Gen1+2.ttf", 70f,  new Point(517,167)) },
            { "Gold",       ("Gen1+2.ttf", 8f,  new Point(47,15)) },
            { "Silver",     ("Gen1+2.ttf", 8f,  new Point(47,15)) },
            { "Crystal",    ("Gen1+2.ttf", 8f,  new Point(47,15)) },
            { "Ruby",       ("Pokemon RS.ttf",   16.5f, new Point(60,43)) },
            { "Sapphire",   ("Pokemon RS.ttf",   12f, new Point(52,45)) },
            { "Emerald",    ("Pokemon RS.ttf",   12f, new Point(52,45)) },
            { "Fire Red",   ("Pokemon RS.ttf",   11f, new Point(63,38)) },
            { "Leaf Green", ("Pokemon RS.ttf",   11f, new Point(63,38)) },
            { "Diamond",    ("Pokemon RS.ttf",   12f, new Point(59,47)) },
            { "Pearl",      ("Pokemon RS.ttf",   12f, new Point(59,47)) },
            { "Platinum",   ("Pokemon RS.ttf",   12f, new Point(59,47)) },
            { "Heart Gold", ("Pokemon RS.ttf",   12f, new Point(59,47)) },
            { "Soul Silver",("Pokemon RS.ttf",   12f, new Point(59,47)) }
        };
        private readonly PrivateFontCollection _fonts;
        public PlayerNameDisplayManager()
        {
            _fonts = FontLoader.LoadFontCollection(new[] { "Gen1+2.ttf", "Pokemon RS.ttf" });
        }
        public FontSettings GetFontSettings(string gameName)
        {
            if (!_styles.TryGetValue(gameName, out var s))
                s = ("Gen1+2.ttf", 12f, new Point(50, 50));
            var fam = FontLoader.GetFamilyFromCollection(_fonts, s.FontFile);
            return new FontSettings(fam, s.SizePx, s.Loc);
        }
    }
}