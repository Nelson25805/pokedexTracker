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
                            fullSpritePath = Path.GetFullPath(fullSpritePath); // This will normalize the path

                            if (!File.Exists(fullSpritePath))
                            {
                                Console.WriteLine($"Sprite not found: {fullSpritePath}");
                            }

                            // Add the Pokémon data to the result list
                            result.Add((
                                reader["Name"].ToString(),
                                reader["Number"].ToString(),
                                fullSpritePath,  // Now using full path for loading the image
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
    }
}
