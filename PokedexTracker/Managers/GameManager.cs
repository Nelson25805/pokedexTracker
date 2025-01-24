using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace PokedexTracker
{
    public class GameManager
    {
        private readonly DatabaseManager _dbManager;

        public GameManager(DatabaseManager dbManager)
        {
            _dbManager = dbManager;
        }

        public (List<(string Name, string Number, string SpritePath, bool IsCaught)> PokemonData, int Total, int Caught)
    GetPokemonData(string gameName)
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

                            result.Add((
                                reader["Name"].ToString(),
                                reader["Number"].ToString(),
                                reader["FilePath"].ToString(),
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
