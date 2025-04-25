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
            { "Red",         ("Gen1+2.ttf",  53f, new Point(665,350)) },
            { "Blue",        ("Gen1+2.ttf",  53f, new Point(665,350)) },
            { "Yellow",      ("Gen1+2.ttf",  53f, new Point(665,350)) },
            { "Gold",        ("Gen1+2.ttf",  6f, new Point(69,32)) },
            { "Silver",      ("Gen1+2.ttf",  6f, new Point(69,32)) },
            { "Crystal",     ("Gen1+2.ttf",  6f, new Point(69,32)) },
            { "Ruby",        ("Gen3.ttf",   10f, new Point(70,69)) },
            { "Sapphire",    ("Gen3.ttf",   10f, new Point(70,69)) },
            { "Emerald",     ("Gen3.ttf",   10f, new Point(70,69)) },
            { "Fire Red",    ("Gen3.ttf",   11f, new Point(75,65)) },
            { "Leaf Green",  ("Gen3.ttf",   11f, new Point(75,65)) },
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