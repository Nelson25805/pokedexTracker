public class PokemonGeneration
{
    public string Name { get; set; }
    public string[] Speeches { get; set; } // Initial dialogues before name submission
    public string[] ProfessorImages { get; set; }

    // Constructor that accepts four parameters
    public PokemonGeneration(string name, string[] speeches, string[] professorImages)
    {
        Name = name;
        Speeches = speeches;
        ProfessorImages = professorImages;
    }
}
