// DiplomaNameDisplayManager.cs
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using PokedexTracker.Helpers;

namespace PokedexTracker.DisplayManagers
{
    public class DiplomaNameDisplayManager
    {
        private readonly Dictionary<string, (string FontFile, float SizePx, Point Loc)> _styles
            = new Dictionary<string, (string, float, Point)>
        {
            { "Red",        ("Gen1+2.ttf",  8f, new Point(78, 31)) },
            { "Blue",       ("Gen1+2.ttf",  8f, new Point(78, 31)) },
            { "Yellow",     ("Gen1+2.ttf",  8f, new Point(78, 31)) },
            { "Gold",       ("Gen1+2.ttf",  12f, new Point(75, 110)) },
            { "Silver",     ("Gen1+2.ttf",  12f, new Point(75, 110)) },
            { "Crystal",    ("Gen1+2.ttf",  12f, new Point(75, 110)) },
            // … add others …
        };

        private readonly PrivateFontCollection _fonts;

        public DiplomaNameDisplayManager()
        {
            // load all fonts once
            _fonts = FontLoader.LoadFontCollection(new[] { "Gen1+2.ttf", "Gen3.ttf" });
        }

        public FontSettings GetFontSettings(string gameName)
        {
            if (!_styles.TryGetValue(gameName, out var s))
                s = ("Gen1+2.ttf", 12f, new Point(50, 100));

            var family = FontLoader.GetFamilyFromCollection(_fonts, s.FontFile);
            return new FontSettings(family, s.SizePx, s.Loc);
        }
    }
}
