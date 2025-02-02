using System;
using System.IO;
using System.Windows;

namespace PokedexTracker
{
    public class AssetManager
    {
        private readonly string _baseAssetsPath;

        public AssetManager()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            _baseAssetsPath = Path.GetFullPath(Path.Combine(basePath, @"..\..\Assets"));
        }

        public string GetBaseAssetsPath()
        {
            return _baseAssetsPath;
        }

        public string GetTrainerCardPath()
        {
            return Path.Combine(_baseAssetsPath, "TrainerCard");
        }

        public string GetDatabasePath()
        {
            return Path.Combine(_baseAssetsPath, "pokedex.db");
        }

        public string GetSpritePath(string gameName, string spriteName)
        {
            return Path.Combine(_baseAssetsPath, gameName, "Sprites", spriteName);
        }

        public string GetTrainerBadgePath(string gameName, int badgeCount)
        {
            return Path.Combine(GetTrainerCardPath(), gameName, $"Trainer_{badgeCount}.png");
        }

        public string GetProfessorImagePath(int index)
        {
            string professorFolderPath = Path.Combine(_baseAssetsPath, "Professor");
            return Path.Combine(professorFolderPath, $"professor{index + 1}.png");
        }

        // New: Return the path to the Fonts folder.
        public string GetFontsPath()
        {
            return Path.Combine(_baseAssetsPath, "Fonts");
        }

        // New: Return the full path for a specific font file.
        public string GetFontPath(string fontFileName)
        {
            return Path.Combine(GetFontsPath(), fontFileName);
        }
    }
}
