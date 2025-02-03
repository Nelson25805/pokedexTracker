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

            // For certain games like Red/Blue, the sprite path may need special handling.
            string spriteGameName = (gameName == "Red" || gameName == "Blue") ? "Red/Blue" : gameName;

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

                            // Combine _assetsPath with the relative path from the database
                            string fullSpritePath = Path.Combine(_assetsPath, relativeSpritePath.TrimStart(Path.DirectorySeparatorChar));

                            // Normalize the path by replacing any double slashes with the correct separator
                            fullSpritePath = fullSpritePath.Replace("\\", Path.DirectorySeparatorChar.ToString());

                            // Ensure the final path is correctly formed by trimming redundant directory separators
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
        /// Retrieves the shiny data by querying the Shiny_Pokedex_Status and Shiny_Sprites tables.
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

            // Use the same logic for spriteGameName as before.
            string spriteGameName = (gameName == "Red" || gameName == "Blue") ? "Red/Blue" : gameName;

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
