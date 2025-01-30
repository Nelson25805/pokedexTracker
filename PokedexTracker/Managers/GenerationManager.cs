using System.Collections.Generic;
using PokedexTracker;

public class GenerationManager
{
    private readonly Dictionary<string, PokemonGeneration> _generations;
    private readonly AssetManager _assetManager;

    public GenerationManager(AssetManager assetManager)
    {
        _assetManager = assetManager;

        _generations = new Dictionary<string, PokemonGeneration>
        {
            {
                "Generation 1",
                new PokemonGeneration(
                    "Generation 1",
                    new string[] // Initial dialogues before name submission
                    {
                        "Hello there! Welcome to the world of Pokémon!",
                        "My name is Professor Oak. \nPeople call me the Pokémon Professor.",
                        "This world is inhabited by creatures called Pokémon.",
                        "First, what is your name?",
                        "Right! So your name is, {playerName}!",
                        "My dream is have a record of every Pokémon, \nso I give to you my lifes work.",
                        "The Pokédex! Keep track of every Pokemon you have,\n and complete a living dex of them all.",
                        "Your very own POKEMON legend is about to unfold!\n A world of dreams and adventures with POKEMON\n awaits! Let's go!"
                    },
                    new string[]
                    {
                        _assetManager.GetProfessorImagePath(0) // Professor image path for Gen 1
                    }
                )
            },
            {
                "Generation 2",
                new PokemonGeneration(
                    "Generation 2",
                    new string[] // Initial dialogues before name submission
                    {
                        "Welcome to Johto!",
                        "I’m Professor Elm.",
                        "I study Pokémon and their unique abilities.",
                        "This world is full of mysteries to uncover!"
                    },
                    new string[]
                    {
                        _assetManager.GetProfessorImagePath(1) // Professor image path for Gen 2
                    }
                )
            }
            // Add more generations with post-name submission dialogues if necessary
        };
    }

    public PokemonGeneration GetGeneration(string generationName)
    {
        if (_generations.TryGetValue(generationName, out var generation))
        {
            return generation;
        }

        return null; // Return null if generation is not found
    }

    public IEnumerable<string> GetAvailableGenerations()
    {
        return _generations.Keys;
    }
}

