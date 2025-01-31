namespace PokedexTracker.Managers
{
    public class SpeechManager
    {
        private readonly string[] _speechParts;
        private int _currentSpeechIndex;

        public SpeechManager(string[] speechParts)
        {
            _speechParts = speechParts;
            _currentSpeechIndex = 0;
        }

        public string CurrentSpeech => _speechParts[_currentSpeechIndex];

        public bool HasMoreSpeeches => _currentSpeechIndex < _speechParts.Length - 1;

        public void Advance()
        {
            if (_currentSpeechIndex < _speechParts.Length - 1)
            {
                _currentSpeechIndex++;
            }
            else
            {
                // Move index out of bounds so we can detect the end.
                _currentSpeechIndex = _speechParts.Length;
            }
        }


        public int CurrentIndex => _currentSpeechIndex;
    }
}
