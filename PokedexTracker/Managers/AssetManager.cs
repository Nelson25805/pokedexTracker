// AssetManager.cs
using System;
using System.IO;

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

        public string GetBaseAssetsPath() => _baseAssetsPath;

        public string GetTrainerCardPath() => Path.Combine(_baseAssetsPath, "TrainerCard");
        public string GetDatabasePath() => Path.Combine(_baseAssetsPath, "Database", "pokedex.db");
        public string GetSpritePath(string gameName, string spriteName) =>
            Path.Combine(_baseAssetsPath, gameName, "Sprites", spriteName);

        public string GetTrainerBadgePath(string gameName, int badgeCount, string gender)
        {
            string trainerCardPath = GetTrainerCardPath();
            string folderName;

            if (gameName == "Ruby" || gameName == "Sapphire")
                folderName = "Ruby-Sapphire";
            else if (gameName == "Fire Red" || gameName == "Leaf Green")
                folderName = "FireRed-LeafGreen";
            else if (gameName == "Diamond" || gameName == "Pearl")
                folderName = "Diamond-Pearl";
            else if (gameName == "Heart Gold" || gameName == "Soul Silver")
                folderName = "HeartGold-SoulSilver";
            else
                folderName = gameName;

            string basePath = Path.Combine(trainerCardPath, folderName);
            string[] nonGender = { "Red", "Blue", "Yellow", "Gold", "Silver" };
            if (Array.IndexOf(nonGender, folderName) == -1)
                basePath = Path.Combine(basePath, gender);

            return Path.Combine(basePath, $"Trainer_{badgeCount}.png");
        }

        public string GetProfessorImagePath(int index)
        {
            string path = Path.Combine(_baseAssetsPath, "Professor");
            return Path.Combine(path, $"professor{index + 1}.png");
        }

        public string GetFontsPath() => Path.Combine(_baseAssetsPath, "Fonts");
        public string GetFontPath(string fileName) => Path.Combine(GetFontsPath(), fileName);
        public string GetRadioButtonImagePath(string fileName) =>
            Path.Combine(_baseAssetsPath, "RadioButtons", fileName);

        public string GetPokemonCardBackgroundPath() =>
            Path.Combine(_baseAssetsPath, "PokemonCardBackground", "pokeballBg.png");

        public string GetDiplomaPath(string gameName, string mediaVersion = "GB", string printVersion = "Regular")
        {
            // Base diplomas folder
            string diplomasRoot = Path.Combine(_baseAssetsPath, "Diplomas");

            // Determine folder name
            string folderName;
            if (gameName == "Red" || gameName == "Blue")
                folderName = "Red-Blue";
            else if (gameName == "Gold" || gameName == "Silver")
                folderName = "Gold-Silver";
            else if (gameName == "Ruby" || gameName == "Sapphire")
                folderName = "Ruby-Sapphire";
            else if (gameName == "Fire Red" || gameName == "Leaf Green")
                folderName = "FireRed-LeafGreen";
            else if (gameName == "Diamond" || gameName == "Pearl")
                folderName = "Diamond-Pearl";
            else if (gameName == "Heart Gold" || gameName == "Soul Silver")
                folderName = "HeartGold-SoulSilver";
            else
                folderName = gameName;

            // Compose file name with both versions: e.g., "Yellow-GB-Regular.png"
            string fileName = $"{folderName}-{mediaVersion}-{printVersion}.png";

            return Path.Combine(diplomasRoot, fileName);
        }
    }
}