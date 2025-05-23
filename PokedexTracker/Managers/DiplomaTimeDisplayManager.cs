﻿using System.Collections.Generic;
using System.Drawing;
using PokedexTracker.Helpers;

namespace PokedexTracker.DisplayManagers
{
    public struct TwoFontSettings
    {
        public FontSettings HourSettings;
        public FontSettings MinuteSettings;
        public TwoFontSettings(FontSettings h, FontSettings m)
        {
            HourSettings = h;
            MinuteSettings = m;
        }
    }

    public class DiplomaTimeDisplayManager
    {
        // Map game → (hourFontFile, hourSize, hourLoc, minuteFontFile, minuteSize, minuteLoc)
        private readonly Dictionary<string, (string FontFile, float Size, Point HourLoc, Point MinLoc)> _timeStyles
            = new Dictionary<string, (string, float, Point, Point)>()
        {
            { "Yellow", ("PKMN RBYGSC.ttf", 8f, new Point(122,263), new Point(127,263)) },
            { "Gold",   ("PKMN RBYGSC.ttf", 8f, new Point(130,263), new Point(135,263)) },
            { "Silver", ("PKMN RBYGSC.ttf", 8f, new Point(130,263), new Point(135,263)) },
            { "Crystal",("PKMN RBYGSC.ttf", 8f, new Point(130,263), new Point(135,263)) }
        };

        public TwoFontSettings GetFontSettings(string gameName)
        {
            if (!_timeStyles.TryGetValue(gameName, out var s))
            {
                // fallback if needed
                s = ("PKMN RBYGSC.ttf", 8f, new Point(0, 0), new Point(0, 0));
            }

            // Load the embedded font family once from resource
            FontFamily fam = FontLoader.LoadEmbeddedFontFamily(s.FontFile);

            var hourFs = new FontSettings(fam, s.Size, s.HourLoc);
            var minFs = new FontSettings(fam, s.Size, s.MinLoc);
            return new TwoFontSettings(hourFs, minFs);
        }
    }
}
