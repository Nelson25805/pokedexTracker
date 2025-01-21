using System.Threading.Tasks;
using System.Windows;

namespace PokedexTracker
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize database
            var dbHelper = new DatabaseHelper();
            await dbHelper.InitializeDatabaseAsync();

            // (Optional) Dependency injection or service setup can go here
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            // Perform cleanup tasks if needed
        }
    }
}
