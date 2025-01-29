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
                        "Welcome to the world of Pokémon!",
                        "My name is Professor Oak.",
                        "People call me the Pokémon Professor.",
                        "This world is inhabited by creatures called Pokémon."
                    },
                    new string[]
                    {
                        _assetManager.GetProfessorImagePath(0) // Professor image path for Gen 1
                    },
                    new string[] // New array for post-name submission dialogue
                    {
                        "Ah, so your name is {playerName}!",
                        "I hope you're ready for your journey, {playerName}!",
                        "In this world, there are creatures called Pokémon. Get ready to catch them all!",
                        "I’ll help you get started. Let's make sure you're prepared!"
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
                    },
                    new string[] // New array for post-name submission dialogue
                    {
                        "So, {playerName}, are you ready to begin your adventure?",
                        "In Johto, new Pokémon await you. Your journey is about to begin!",
                        "Remember, you can catch Pokémon in the wild. I’ll teach you how!",
                        "Good luck, {playerName}! Go and make new friends!"
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

