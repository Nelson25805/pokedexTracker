using System.Windows;
using System.Threading.Tasks;

namespace PokedexTracker
{
    public partial class MainWindow : Window
    {
        private readonly PokemonApiClient _apiClient;
        private readonly DatabaseHelper _dbHelper;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize dependencies
            _apiClient = new PokemonApiClient();
            _dbHelper = new DatabaseHelper();

            // Handle startup tasks asynchronously
            HandleStartupAsync();
        }

        private async void HandleStartupAsync()
        {
            // Check if the database is empty
            if (_dbHelper.IsDatabaseEmpty())
            {
                MessageBox.Show("No Pokémon found in the database. Fetching data...");

                // Fetch and store Pokémon from the API
                await FetchAndStorePokemon();
            }

            // Load Pokémon into the UI
            LoadPokemonList();
        }

        private void LoadPokemonList()
        {
            var pokemonList = _dbHelper.GetAllPokemon();

            if (pokemonList == null || pokemonList.Count == 0)
            {
                MessageBox.Show("No Pokémon found in the database.");
            }
            else
            {
                PokemonListView.ItemsSource = pokemonList;
            }
        }

        private async Task FetchAndStorePokemon()
        {
            var pokemonList = await _apiClient.GetAllPokemonAsync();

            if (pokemonList != null && pokemonList.Count > 0)
            {
                _dbHelper.SavePokemon(pokemonList);
                MessageBox.Show("Pokémon data fetched and saved!");
            }
            else
            {
                MessageBox.Show("Failed to fetch Pokémon data.");
            }
        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (PokemonListView.SelectedItem != null)
            {
                string selectedPokemon = PokemonListView.SelectedItem.ToString();
                MessageBox.Show($"Viewing details for: {selectedPokemon}");
            }
            else
            {
                MessageBox.Show("Please select a Pokémon.");
            }
        }
    }
}
