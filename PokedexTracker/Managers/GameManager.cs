using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace PokedexTracker
{
    public class GameManager
    {
        private readonly DatabaseManager _dbManager;
        private readonly string _assetsPath;

        public GameManager(DatabaseManager dbManager)
        {
            _dbManager = dbManager;
            // Assuming _assetsPath is set to the correct directory path of the Assets folder
            _assetsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Assets");
        }

        /// <summary>
        /// Retrieves Pokémon data from the standard tables.
        /// </summary>
        public (List<(string Name, string Number, string SpritePath, bool IsCaught)> PokemonData, int Total, int Caught) GetPokemonData(string gameName)
        {
            var result = new List<(string, string, string, bool)>();
            int total = 0, caught = 0;

            string query = @"
    SELECT Pokemon.Name, Pokemon.Number, Sprites.FilePath, Pokedex_Status.Is_Caught
    FROM Pokemon
    JOIN Pokedex_Status ON Pokedex_Status.Pokemon_Number = Pokemon.Number
    JOIN Sprites ON Sprites.PokemonId = Pokemon.Number
    WHERE Pokedex_Status.Game_Id = (SELECT Id FROM Game WHERE Name = @gameName)
      AND Sprites.GameName = @spriteGameName
    ORDER BY Pokemon.Number";

            string spriteGameName = BuildSpriteGameName(gameName);

            using (var connection = _dbManager.GetConnection())
            {
                connection.Open();
                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@gameName", gameName);
                    cmd.Parameters.AddWithValue("@spriteGameName", spriteGameName);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bool isCaught = reader["Is_Caught"] != DBNull.Value && Convert.ToInt32(reader["Is_Caught"]) == 1;

                            // Get the relative path from the database
                            string relativeSpritePath = reader["FilePath"].ToString();

                            // Combine _assetsPath with the relative path
                            string fullSpritePath = Path.Combine(_assetsPath, relativeSpritePath.TrimStart(Path.DirectorySeparatorChar));

                            // Normalize the path
                            fullSpritePath = fullSpritePath.Replace("\\", Path.DirectorySeparatorChar.ToString());
                            fullSpritePath = Path.GetFullPath(fullSpritePath);

                            if (!File.Exists(fullSpritePath))
                            {
                                Console.WriteLine($"Sprite not found: {fullSpritePath}");
                            }

                            result.Add((
                                reader["Name"].ToString(),
                                reader["Number"].ToString(),
                                fullSpritePath,
                                isCaught
                            ));

                            total++;
                            if (isCaught) caught++;
                        }
                    }
                }
            }

            return (result, total, caught);
        }

        /// <summary>
        /// Retrieves shiny Pokémon data by querying the Shiny_Pokedex_Status and Shiny_Sprites tables.
        /// </summary>
        public (List<(string Name, string Number, string SpritePath, bool IsCaught)> PokemonData, int Total, int Caught) GetShinyPokemonData(string gameName)
        {
            var result = new List<(string, string, string, bool)>();
            int total = 0, caught = 0;

            string query = @"
    SELECT Pokemon.Name, Pokemon.Number, Shiny_Sprites.FilePath, Shiny_Pokedex_Status.Is_Caught
    FROM Pokemon
    JOIN Shiny_Pokedex_Status ON Shiny_Pokedex_Status.Pokemon_Number = Pokemon.Number
    JOIN Shiny_Sprites ON Shiny_Sprites.PokemonId = Pokemon.Number
    WHERE Shiny_Pokedex_Status.Game_Id = (SELECT Id FROM Game WHERE Name = @gameName)
      AND Shiny_Sprites.GameName = @spriteGameName
    ORDER BY Pokemon.Number";

            // Use the same naming logic as the standard query.
            string spriteGameName = BuildSpriteGameName(gameName);

            using (var connection = _dbManager.GetConnection())
            {
                connection.Open();
                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@gameName", gameName);
                    cmd.Parameters.AddWithValue("@spriteGameName", spriteGameName);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bool isCaught = reader["Is_Caught"] != DBNull.Value && Convert.ToInt32(reader["Is_Caught"]) == 1;

                            string relativeSpritePath = reader["FilePath"].ToString();

                            string fullSpritePath = Path.Combine(_assetsPath, relativeSpritePath.TrimStart(Path.DirectorySeparatorChar));
                            fullSpritePath = fullSpritePath.Replace("\\", Path.DirectorySeparatorChar.ToString());
                            fullSpritePath = Path.GetFullPath(fullSpritePath);

                            if (!File.Exists(fullSpritePath))
                            {
                                Console.WriteLine($"Shiny sprite not found: {fullSpritePath}");
                            }

                            result.Add((
                                reader["Name"].ToString(),
                                reader["Number"].ToString(),
                                fullSpritePath,
                                isCaught
                            ));

                            total++;
                            if (isCaught) caught++;
                        }
                    }
                }
            }

            return (result, total, caught);
        }

        /// <summary>
        /// Builds the sprite game name according to the following rules:
        /// - If gameName is Red or Blue → "Red-Blue"
        /// - If gameName is Ruby or Sapphire → "Ruby-Sapphire"
        /// - If gameName is FireRed or LeafGreen → "FireRed-LeafGreen"
        /// - Otherwise, return gameName.
        /// </summary>
        private string BuildSpriteGameName(string gameName)
        {
            if (gameName == "Red" || gameName == "Blue")
                return "Red-Blue";
            else if (gameName == "Ruby" || gameName == "Sapphire")
                return "Ruby-Sapphire";
            else if (gameName == "Fire Red" || gameName == "Leaf Green")
                return "FireRed-LeafGreen";
            else
                return gameName;
        }

        public void ToggleCaughtStatus(string pokemonNumber, string gameName, bool newStatus)
        {
            string query = @"
                UPDATE Pokedex_Status
                SET Is_Caught = @newStatus
                WHERE Pokemon_Number = @pokemonNumber
                AND Game_Id = (SELECT Id FROM Game WHERE Name = @gameName)";

            var parameters = new[]
            {
                new SQLiteParameter("@newStatus", newStatus ? 1 : 0),
                new SQLiteParameter("@pokemonNumber", pokemonNumber),
                new SQLiteParameter("@gameName", gameName)
            };

            _dbManager.ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Toggles the caught status for shiny data in the Shiny_Pokedex_Status table.
        /// </summary>
        public void ToggleShinyCaughtStatus(string pokemonNumber, string gameName, bool newStatus)
        {
            string query = @"
                UPDATE Shiny_Pokedex_Status
                SET Is_Caught = @newStatus
                WHERE Pokemon_Number = @pokemonNumber
                AND Game_Id = (SELECT Id FROM Game WHERE Name = @gameName)";

            var parameters = new[]
            {
                new SQLiteParameter("@newStatus", newStatus ? 1 : 0),
                new SQLiteParameter("@pokemonNumber", pokemonNumber),
                new SQLiteParameter("@gameName", gameName)
            };

            _dbManager.ExecuteNonQuery(query, parameters);
        }
    }
}
