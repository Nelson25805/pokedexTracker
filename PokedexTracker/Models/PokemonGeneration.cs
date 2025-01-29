public class PokemonGeneration
{
    public string Name { get; set; }
    public string[] Speeches { get; set; } // Initial dialogues before name submission
    public string[] ProfessorImages { get; set; }
    public string[] PostNameSubmissionSpeeches { get; set; } // Post-name submission dialogues

    // Constructor that accepts four parameters
    public PokemonGeneration(string name, string[] speeches, string[] professorImages, string[] postNameSubmissionSpeeches)
    {
        Name = name;
        Speeches = speeches;
        ProfessorImages = professorImages;
        PostNameSubmissionSpeeches = postNameSubmissionSpeeches;
    }
}
