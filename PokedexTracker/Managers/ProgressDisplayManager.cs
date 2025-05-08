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
            { "Red",         ("PKMN RBYGSC.ttf",  53f, new Point(665,350)) },
            { "Blue",        ("PKMN RBYGSC.ttf",  53f, new Point(665,350)) },
            { "Yellow",      ("PKMN RBYGSC.ttf",  53f, new Point(665,350)) },
            { "Gold",        ("PKMN RBYGSC.ttf",  6f, new Point(69,32)) },
            { "Silver",      ("PKMN RBYGSC.ttf",  6f, new Point(69,32)) },
            { "Crystal",     ("PKMN RBYGSC.ttf",  6f, new Point(69,32)) },
            { "Ruby",        ("Pokemon RS.ttf",   17f, new Point(72,66)) },
            { "Sapphire",    ("Pokemon RS.ttf",   10f, new Point(70,69)) },
            { "Emerald",     ("Pokémon FireRed & LeafGreen Fon.ttf",   10f, new Point(70,69)) },
            { "Fire Red",    ("Pokémon FireRed & LeafGreen Fon.ttf",   11f, new Point(75,65)) },
            { "Leaf Green",  ("Pokémon FireRed & LeafGreen Fon.ttf",   11f, new Point(75,65)) },
            { "Diamond",     ("Pokemon RS.ttf",   11f, new Point(85,70)) },
            { "Pearl",       ("Pokemon RS.ttf",   11f, new Point(85,70)) },
            { "Platinum",    ("Pokemon RS.ttf",   11f, new Point(85,70)) },
            { "Heart Gold",  ("Pokemon RS.ttf",   11f, new Point(85,70)) },
            { "Soul Silver", ("Pokemon RS.ttf",   11f, new Point(85,70)) }
        };
        private readonly PrivateFontCollection _fonts;
        public ProgressDisplayManager()
        {
            _fonts = FontLoader.LoadFontCollection(new[] { "PKMN RBYGSC.ttf", "Pokemon RS.ttf", "Pokémon FireRed & LeafGreen Fon.ttf" });
        }
        public FontSettings GetFontSettings(string gameName)
        {
            if (!_styles.TryGetValue(gameName, out var s))
                s = ("PKMN RBYGSC.ttf", 9f, new Point(100, 50));
            var fam = FontLoader.GetFamilyFromCollection(_fonts, s.FontFile);
            return new FontSettings(fam, s.SizePx, s.Loc);
        }
    }
}