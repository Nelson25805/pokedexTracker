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

        public string GetBaseAssetsPath()
        {
            return _baseAssetsPath;
        }

        /// <summary>
        /// Returns the path to the TrainerCard folder.
        /// </summary>
        public string GetTrainerCardPath()
        {
            return Path.Combine(_baseAssetsPath, "TrainerCard");
        }

        /// <summary>
        /// Returns the full path to the SQLite database.
        /// </summary>
        public string GetDatabasePath()
        {
            return Path.Combine(_baseAssetsPath, "pokedex.db");
        }

        /// <summary>
        /// Returns the full path to a sprite file given a game and sprite name.
        /// </summary>
        public string GetSpritePath(string gameName, string spriteName)
        {
            return Path.Combine(_baseAssetsPath, gameName, "Sprites", spriteName);
        }

        /// <summary>
        /// Returns the full path to the trainer badge image.
        /// 
        /// Special logic:
        /// - For Ruby or Sapphire, the folder name is "Ruby-Sapphire".
        /// - For Fire Red or Leaf Green, the folder name is "FireRed-LeafGreen".
        /// - Otherwise, folderName defaults to gameName.
        /// 
        /// Then, if the game is not in the non-gender list ({"Red", "Blue", "Yellow", "Gold", "Silver"}),
        /// a gender folder (e.g. "Boy" or "Girl") is appended.
        /// </summary>
        public string GetTrainerBadgePath(string gameName, int badgeCount, string gender)
        {
            string trainerCardPath = GetTrainerCardPath();
            string folderName;

            // Use special folder names for certain games.
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

            // Build the base path.
            string basePath = Path.Combine(trainerCardPath, folderName);

            // List of games that do NOT use a gender folder.
            string[] nonGenderFolders = new string[] { "Red", "Blue", "Yellow", "Gold", "Silver" };

            // If the folderName is NOT in the nonGender list, then append the gender folder.
            if (Array.IndexOf(nonGenderFolders, folderName) == -1)
            {
                basePath = Path.Combine(basePath, gender);
            }

            return Path.Combine(basePath, $"Trainer_{badgeCount}.png");
        }

        /// <summary>
        /// Returns the path to the Professor images folder.
        /// </summary>
        public string GetProfessorImagePath(int index)
        {
            string professorFolderPath = Path.Combine(_baseAssetsPath, "Professor");
            return Path.Combine(professorFolderPath, $"professor{index + 1}.png");
        }

        /// <summary>
        /// Returns the path to the Fonts folder.
        /// </summary>
        public string GetFontsPath()
        {
            return Path.Combine(_baseAssetsPath, "Fonts");
        }

        /// <summary>
        /// Returns the full path for a specific font file.
        /// </summary>
        public string GetFontPath(string fontFileName)
        {
            return Path.Combine(GetFontsPath(), fontFileName);
        }

        /// <summary>
        /// Returns the full path for the custom Radio Button images.
        /// </summary>
        public string GetRadioButtonImagePath(string imageFileName)
        {
            return Path.Combine(_baseAssetsPath,"RadioButtons", imageFileName);
        }

        /// <summary>
        /// Returns the full path for the Pokemon card background image.
        /// </summary>
        public string GetPokemonCardBackgroundPath()
        {
            return Path.Combine(_baseAssetsPath, "PokemonCardBackground", "pokeballBg.png");
        }
    }
}
