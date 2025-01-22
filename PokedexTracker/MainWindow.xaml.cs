using System.Windows;
using System.Threading.Tasks;
using System.Linq;
using System;

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
            MessageBox.Show("Fetching the latest Pokémon data...");

            // Fetch and store Pokémon from the API, even if the database has some data
            await FetchAndStorePokemon();

            // Load Pokémon into the UI
            LoadPokemonList();
        }




        private void LoadPokemonList()
        {
            // Fetch the updated list of Pokémon from the database
            var pokemonList = _dbHelper.GetAllPokemon();

            if (pokemonList == null || pokemonList.Count == 0)
            {
                MessageBox.Show("No Pokémon found in the database.");
            }
            else
            {
                // Display the Pokémon list in the UI
                PokemonListView.ItemsSource = pokemonList;
            }
        }



        private async Task FetchAndStorePokemon()
        {
            try
            {
                // Check if the database already has Pokémon data
                int pokemonCount = _dbHelper.GetPokemonCount();

                // If fewer than the expected number of Pokémon, fetch more from the API
                if (pokemonCount < 1000)  // Assuming 1000 or more Pokémon should be in the database
                {
                    var pokemonList = await _apiClient.GetAllPokemonAsync();

                    if (pokemonList != null && pokemonList.Count > 0)
                    {
                        // Save only new Pokémon that are not already in the database
                        _dbHelper.SavePokemon(pokemonList);
                        MessageBox.Show("New Pokémon data fetched and saved!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to fetch Pokémon data.");
                    }
                }
                else
                {
                    MessageBox.Show("Database already contains the latest Pokémon.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching and saving Pokémon: {ex.Message}");
            }

            // Reload the Pokémon list in the UI after fetching and saving
            LoadPokemonList();
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
