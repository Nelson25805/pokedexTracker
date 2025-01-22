using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Windows;

public class PokemonApiClient
{
    private readonly HttpClient _httpClient;

    public PokemonApiClient()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri("https://pokeapi.co/api/v2/") };
    }

    // Fetch individual Pokémon details
    public async Task<Pokemon> GetPokemonAsync(string name)
    {
        var response = await _httpClient.GetStringAsync($"pokemon/{name}");
        return JsonConvert.DeserializeObject<Pokemon>(response);
    }

    // Fetch the first 151 Pokémon
    public async Task<List<PokemonBasic>> GetAllPokemonAsync()
    {
        var allPokemon = new List<PokemonBasic>();
        int offset = 0;
        int limit = 100;  // Number of Pokémon to fetch per request

        while (true)
        {
            var response = await _httpClient.GetStringAsync($"pokemon?limit={limit}&offset={offset}");
            var result = JsonConvert.DeserializeObject<PokemonResponse>(response);

            // Add a MessageBox to show how many Pokémon were fetched
            MessageBox.Show($"Fetched {result.Results.Count} Pokémon (Offset: {offset})");

            if (result.Results.Count == 0)
            {
                // No more Pokémon to fetch, exit the loop
                break;
            }

            allPokemon.AddRange(result.Results);

            // Move the offset forward to fetch the next set of Pokémon
            offset += limit;
        }

        return allPokemon;
    }



    // Class to hold the response structure
    public class PokemonResponse
    {
        [JsonProperty("results")]
        public List<PokemonBasic> Results { get; set; }
    }

    // Basic Pokémon info that you'll use to store in the database
    public class PokemonBasic
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    // Full Pokémon info with details like types
    public class Pokemon
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<PokemonType> Types { get; set; }
    }

    // Type information for the Pokémon
    public class PokemonType
    {
        public TypeInfo Type { get; set; }
    }

    // Type info (e.g., "fire", "water")
    public class TypeInfo
    {
        public string Name { get; set; }
    }
}
