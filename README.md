# Pokemon Pokedex Tracker

A Windows Forms application developed in C# that provides a comprehensive solution for tracking Pokedex data across multiple Pokémon games. This project allows users to manage both caught and uncaught Pokémon data, using SQLite for efficient, lightweight data storage.

## Features

- **Multi-Game Support**: Manage and track Pokémon data from various Pokémon games.
- **Detailed Pokedex Tracking**: Easily view and update the status of Pokémon (caught/uncaught).
- **SQLite Integration**: Leverages SQLite for fast, reliable local database management.
- **User-Friendly Interface**: Built with Windows Forms for a familiar and intuitive user experience.
- **Extendable Architecture**: Designed with modularity in mind, making it easy to add new features or support additional data types.

## Technologies Used

- **C#**: Primary programming language.
- **Windows Forms**: Framework for building the graphical user interface.
- **SQLite**: Embedded database engine for storing Pokedex data.
- **.NET Framework/Core**: Underlying platform for application development.

## Getting Started

### Prerequisites

- [Visual Studio 2019 or later](https://visualstudio.microsoft.com/)
- [.NET Framework 4.7.2+ or .NET Core 3.1+](https://dotnet.microsoft.com/)
- SQLite libraries (included in the project)

### Installation

1. **Clone the Repository**
   ```bash
   git clone https://github.com/yourusername/PokemonPokedexTracker.git
Open the Project

Launch Visual Studio.
Navigate to File -> Open -> Project/Solution.
Select the .sln file from the cloned repository.
Restore Dependencies

Visual Studio should automatically restore any required NuGet packages.
Alternatively, right-click the solution in the Solution Explorer and select Restore NuGet Packages.
Build and Run

Build the project using Build -> Build Solution or press Ctrl+Shift+B.
Run the application by clicking the Start button or pressing F5.
Usage
Adding Pokémon Data
Click the "Add Pokémon" button to input new entries into your Pokedex.
Viewing the Pokedex
Navigate through the list to see detailed information about each Pokémon, including the game of origin and caught status.
Editing and Removing Entries
Select a Pokémon entry to edit its details or remove it from the database.
Database Management
The application handles all database interactions with SQLite internally, so no manual database configuration is required.
Project Structure
/src: Contains the core application source code including forms, business logic, and data access layers.
/assets: Stores images and other assets used within the application.
/docs: Additional documentation and reference materials.
/tests: Unit tests and test-related files (if applicable).
Contributing
Contributions are welcome! Please follow these steps:

Fork the repository.
Create a new branch (git checkout -b feature/your-feature).
Commit your changes (git commit -m "Add new feature").
Push the branch (git push origin feature/your-feature).
Open a pull request explaining your changes.
License
This project is licensed under the MIT License. See the LICENSE file for more details.

Contact
For any questions, suggestions, or issues, please open an issue in the repository or contact [your-email@example.com].

Acknowledgements
Thanks to the open-source community for invaluable support and inspiration.
This project is a tribute to the enduring legacy of the Pokémon franchise.
css
Copy
Edit

This README provides a clear overview, installation steps, usage instructions, and guidelines for contributions, ensuring a professional presentation for your GitHub repository.
