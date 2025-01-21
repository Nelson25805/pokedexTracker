﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using static PokemonApiClient;
using System.IO;
using System.Threading.Tasks;

namespace PokedexTracker
{
    public class DatabaseHelper
    {
        private const string DatabaseFile = "pokedex.db";
        public string ConnectionString => $"Data Source={DatabaseFile};Version=3;";

        private PokemonApiClient _pokemonApiClient;

        public DatabaseHelper()
        {
            _pokemonApiClient = new PokemonApiClient(); // Initialize the API client
        }

        public async Task InitializeDatabaseAsync()
        {
            // Check if the database file already exists
            if (!File.Exists(DatabaseFile))
            {
                Console.WriteLine("Database not found. Creating a new one...");
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Pokemon (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL UNIQUE,
                        IsCaught INTEGER NOT NULL
                    );";

                    using (var command = new SQLiteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Database created and table initialized.");
                    }
                }

                // Fetch Pokémon and save them
                Console.WriteLine("Fetching Pokémon data from API...");
                var pokemonList = await FetchPokemonDataFromApiAsync();
                if (pokemonList != null && pokemonList.Count > 0)
                {
                    Console.WriteLine($"Fetched {pokemonList.Count} Pokémon. Saving to database...");
                    SavePokemon(pokemonList);
                    Console.WriteLine("Pokémon data saved successfully.");
                }
                else
                {
                    Console.WriteLine("No Pokémon data fetched from API.");
                }
            }
            else
            {
                Console.WriteLine("Database already exists. Skipping creation.");
            }
        }

        // Fetch Pokémon data from the API
        private async Task<List<PokemonApiClient.PokemonBasic>> FetchPokemonDataFromApiAsync()
        {
            try
            {
                return await _pokemonApiClient.GetAllPokemonAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching Pokémon data: {ex.Message}");
                return null;
            }
        }

        public List<string> GetAllPokemon()
        {
            var pokemonList = new List<string>();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                // Ensure results are ordered by the Id column
                string query = "SELECT Name FROM Pokemon ORDER BY Id ASC;";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            pokemonList.Add(name);
                        }
                    }
                }
            }

            return pokemonList;
        }





        // Save Pokémon data to the database
        public void SavePokemon(List<PokemonApiClient.PokemonBasic> pokemonList)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string insertQuery = "INSERT OR IGNORE INTO Pokemon (Name, IsCaught) VALUES (@Name, 0);";

                foreach (var pokemon in pokemonList)
                {
                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", pokemon.Name);

                        try
                        {
                            command.ExecuteNonQuery();
                            Console.WriteLine($"Saved Pokémon: {pokemon.Name}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error saving Pokémon {pokemon.Name}: {ex.Message}");
                        }
                    }
                }
            }
        }

        // Check if the database is empty (no Pokémon saved yet)
        public bool IsDatabaseEmpty()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Pokemon;";
                using (var command = new SQLiteCommand(query, connection))
                {
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    Console.WriteLine($"Database count: {count}");  // Debugging output
                    return count == 0;  // Return true if there are no records in the database
                }
            }
        }


    }

}
