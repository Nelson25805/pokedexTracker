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
            { "Red",        ("Gen1+2.ttf", 9f,  new Point(90,20)) },
            { "Blue",       ("Gen1+2.ttf", 9f,  new Point(90,20)) },
            { "Yellow",     ("Gen1+2.ttf", 9f,  new Point(90,20)) },
            { "Gold",       ("Gen1+2.ttf", 9f,  new Point(84,16)) },
            { "Silver",     ("Gen1+2.ttf", 8f,  new Point(53,15)) }, //nice one
            { "Crystal",    ("Gen1+2.ttf", 9f,  new Point(84,16)) },
            { "Ruby",       ("Gen3.ttf",   12f, new Point(59,47)) },
            { "Sapphire",   ("Gen3.ttf",   12f, new Point(59,47)) },
            { "Emerald",    ("Gen3.ttf",   12f, new Point(59,47)) },
            { "Fire Red",   ("Gen3.ttf",   11f, new Point(70,40)) },
            { "Leaf Green", ("Gen3.ttf",   11f, new Point(70,40)) },
            { "Diamond",    ("Gen3.ttf",   12f, new Point(59,47)) },
            { "Pearl",      ("Gen3.ttf",   12f, new Point(59,47)) },
            { "Platinum",   ("Gen3.ttf",   12f, new Point(59,47)) },
            { "Heart Gold", ("Gen3.ttf",   12f, new Point(59,47)) },
            { "Soul Silver",("Gen3.ttf",   12f, new Point(59,47)) }
        };
        private readonly PrivateFontCollection _fonts;
        public PlayerNameDisplayManager()
        {
            _fonts = FontLoader.LoadFontCollection(new[] { "Gen1+2.ttf", "Gen3.ttf" });
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