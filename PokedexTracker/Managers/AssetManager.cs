// AssetManager.cs
using System;
using System.Collections.Generic;
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
            // Map each game to its generation folder:
            var genMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                // Gen I
                ["Red"] = "Gen1",
                ["Blue"] = "Gen1",
                ["Yellow"] = "Gen1",
                // Gen II
                ["Gold"] = "Gen2",
                ["Silver"] = "Gen2",
                ["Crystal"] = "Gen2",
                // Gen III
                ["Ruby"] = "Gen3",
                ["Sapphire"] = "Gen3",
                ["Emerald"] = "Gen3",
                ["Fire Red"] = "Gen3",
                ["Leaf Green"] = "Gen3",
                // Gen IV
                ["Diamond"] = "Gen4",
                ["Pearl"] = "Gen4",
                ["Platinum"] = "Gen4",
                ["Heart Gold"] = "Gen4",
                ["Soul Silver"] = "Gen4",
                // … add Gen V+ as needed …
            };

            // Determine gen folder, default to top-level if unknown:
            string genFolder = genMap.TryGetValue(gameName, out var g) ? g : "";

            // Determine game folder (e.g. “Red-Blue”, “Gold-Silver”, etc.)
            string gameFolder;
            if (gameName == "Red" || gameName == "Blue")
                gameFolder = "Red-Blue";
            else if (gameName == "Gold" || gameName == "Silver")
                gameFolder = "Gold-Silver";
            else if (gameName == "Ruby" || gameName == "Sapphire")
                gameFolder = "Ruby-Sapphire";
            else if (gameName == "Fire Red" || gameName == "Leaf Green")
                gameFolder = "FireRed-LeafGreen";
            else if (gameName == "Diamond" || gameName == "Pearl")
                gameFolder = "Diamond-Pearl";
            else if (gameName == "Heart Gold" || gameName == "Soul Silver")
                gameFolder = "HeartGold-SoulSilver";
            else
                gameFolder = gameName;

            // Build the diplomas root:
            string diplomasRoot = Path.Combine(_baseAssetsPath, "Diplomas");

            // If we have a genFolder, look under it:
            string targetDir = string.IsNullOrEmpty(genFolder)
                ? diplomasRoot
                : Path.Combine(diplomasRoot, genFolder);

            // Compose the filename: e.g. “Yellow-GB-Printer.png”
            string fileName = $"{gameFolder}-{mediaVersion}-{printVersion}.png";

            return Path.Combine(targetDir, fileName);
        }
    }
}