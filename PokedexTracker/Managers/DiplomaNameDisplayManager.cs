using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using PokedexTracker.Helpers;

namespace PokedexTracker.DisplayManagers
{
    public class DiplomaNameDisplayManager
    {
        // Font file, size, position
        private readonly Dictionary<string, (string File, float Size, Point Loc)> _styles
            = new Dictionary<string, (string, float, Point)>
        {
            { "Red",       ("PKMN RBYGSC.ttf",  8f, new Point(78, 31)) },
            { "Blue",      ("PKMN RBYGSC.ttf",  8f, new Point(78, 31)) },
            { "Yellow",    ("PKMN RBYGSC.ttf",  8f, new Point(78, 31)) },
            { "Gold",      ("PKMN RBYGSC.ttf",  8f, new Point(70, 39)) },
            { "Silver",    ("PKMN RBYGSC.ttf",  8f, new Point(70, 39)) },
            { "Crystal",   ("PKMN RBYGSC.ttf",  8f, new Point(70, 39)) },
            { "Ruby",       ("Pokemon RS.ttf",   17f, new Point(99,14)) },
            { "Sapphire",  ("Pokemon RS.ttf",    17f, new Point(70, 39)) },
            { "Emerald",   ("Pokemon RS.ttf",    8f, new Point(70, 39)) },
            { "Fire Red",  ("Pokémon FireRed & LeafGreen Fon.ttf",    10f, new Point(140, 18)) },
            { "Leaf Green",("Pokémon FireRed & LeafGreen Fon.ttf",    17f, new Point(70, 39)) },
            // …add others…
        };

        // Per‐game override colors (only these get special colors; the rest fallback to Black)
        private readonly Dictionary<string, Color> _colorOverrides
            = new Dictionary<string, Color>
        {
            { "Ruby",      Color.FromArgb(255,224,8,8) },
            { "Sapphire",      Color.FromArgb(255,224,8,8) },
            // add any additional games here…
        };

        private readonly PrivateFontCollection _fonts;

        public DiplomaNameDisplayManager()
        {
            // load any fonts you need (same as your FontLoader expects)
            _fonts = FontLoader.LoadFontCollection(new[]
            {
                "PKMN RBYGSC.ttf",
                "Pokemon RS.ttf",
                "Pokémon FireRed & LeafGreen Fon.ttf"
            });
        }

        public DiplomaFontSettings GetFontSettings(string gameName)
        {
            // 1) pick font/size/loc or fallback
            if (!_styles.TryGetValue(gameName, out var _style))
                _style = ("PKMN RBYGSC.ttf", 12f, new Point(50, 100));

            // 2) pick color override or default to Black
            var color = _colorOverrides.TryGetValue(gameName, out var c)
                        ? c
                        : Color.Black;

            // 3) get the loaded FontFamily from your collection
            var family = FontLoader.GetFamilyFromCollection(_fonts, _style.File);

            return new DiplomaFontSettings(
                family,
                _style.Size,
                _style.Loc,
                color
            );
        }
    }
}
