using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

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
        var response = await _httpClient.GetStringAsync("pokemon?limit=151&offset=0");
        var result = JsonConvert.DeserializeObject<PokemonResponse>(response);
        return result.Results;
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
