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

        public string GetDiplomaPath(string gameName, string choice)
        {
            // 1) Determine generation folder:
            string gen;
            if (gameName == "Red" || gameName == "Blue" || gameName == "Yellow")
                gen = "Gen1";
            else if (gameName == "Gold" || gameName == "Silver" || gameName == "Crystal")
                gen = "Gen2";
            else if (gameName == "Ruby" || gameName == "Sapphire" || gameName == "Emerald")
                gen = "Gen3";
            else if (gameName == "Fire Red" || gameName == "Leaf Green" || gameName == "Platinum")
                gen = "Gen3";
            else if (gameName == "Diamond" || gameName == "Pearl" || gameName == "Heart Gold" || gameName == "Soul Silver")
                gen = "Gen4";
            else if (gameName == "Black" || gameName == "White" || gameName == "Black 2" || gameName == "White 2")
                gen = "Gen5";
            else if (gameName == "X" || gameName == "Y")
                gen = "Gen6";
            else
                gen = "GenMisc";

            // 2) Normalize game-folder name:
            string gf;
            if (gameName == "Red" || gameName == "Blue")
                gf = "Red-Blue";
            else if (gameName == "Gold" || gameName == "Silver")
                gf = "Gold-Silver";
            else if (gameName == "Ruby" || gameName == "Sapphire")
                gf = "Ruby-Sapphire";
            else if (gameName == "Fire Red" || gameName == "Leaf Green")
                gf = "FireRed-LeafGreen";
            else if (gameName == "Diamond" || gameName == "Pearl")
                gf = "Diamond-Pearl";
            else if (gameName == "Heart Gold" || gameName == "Soul Silver")
                gf = "HeartGold-SoulSilver";
            else
                gf = gameName.Replace(" ", "");

            // 3) Build filename with exactly one choice token:
            //    e.g. "Red-Blue-GB.png", "Yellow-Printer.png", "Ruby-Regional.png"
            string fileName = $"{gf}-{choice}.png";

            // 4) Assemble full path
            return Path.Combine(
                _baseAssetsPath,
                "Diplomas",
                gen,
                gf,
                fileName
            );
        }
    }
}