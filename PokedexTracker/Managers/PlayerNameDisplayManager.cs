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
            { "Red",        ("PKMN RBYGSC.ttf", 70f,  new Point(517,167)) },
            { "Blue",       ("PKMN RBYGSC.ttf", 70f,  new Point(517,167)) },
            { "Yellow",     ("PKMN RBYGSC.ttf", 70f,  new Point(517,167)) },
            { "Gold",       ("PKMN RBYGSC.ttf", 8f,  new Point(47,15)) },
            { "Silver",     ("PKMN RBYGSC.ttf", 8f,  new Point(47,15)) },
            { "Crystal",    ("PKMN RBYGSC.ttf", 8f,  new Point(47,15)) },
            { "Ruby",       ("Pokemon RS.ttf",   17f, new Point(60,41)) },
            { "Sapphire",   ("Pokemon RS.ttf",   12f, new Point(52,45)) },
            { "Emerald",    ("Pokémon FireRed & LeafGreen Fon.ttf",   12f, new Point(52,45)) },
            { "Fire Red",  ("Pokémon FireRed & LeafGreen Fon.ttf",    12f, new Point(70, 39)) },
            { "Leaf Green",("Pokémon FireRed & LeafGreen Fon.ttf",    17f, new Point(70, 39)) },
            { "Diamond",    ("Pokemon RS.ttf",   12f, new Point(59,47)) },
            { "Pearl",      ("Pokemon RS.ttf",   12f, new Point(59,47)) },
            { "Platinum",   ("Pokemon RS.ttf",   12f, new Point(59,47)) },
            { "Heart Gold", ("Pokemon RS.ttf",   12f, new Point(59,47)) },
            { "Soul Silver",("Pokemon RS.ttf",   12f, new Point(59,47)) }
        };
        private readonly PrivateFontCollection _fonts;
        public PlayerNameDisplayManager()
        {
            _fonts = FontLoader.LoadFontCollection(new[] { "PKMN RBYGSC.ttf", "Pokemon RS.ttf", "Pokémon FireRed & LeafGreen Fon.ttf" });
        }
        public FontSettings GetFontSettings(string gameName)
        {
            if (!_styles.TryGetValue(gameName, out var s))
                s = ("PKMN RBYGSC.ttf", 12f, new Point(50, 50));
            var fam = FontLoader.GetFamilyFromCollection(_fonts, s.FontFile);
            return new FontSettings(fam, s.SizePx, s.Loc);
        }
    }
}