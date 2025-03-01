using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PokedexTracker
{
    public class PokemonCard : Panel
    {
        public string PokemonName { get; set; }
        public string PokemonNumber { get; set; }
        public string SpritePath { get; set; }
        public bool IsCaught { get; set; }

        public PokemonCard(string name, string number, string spritePath, bool isCaught)
        {
            PokemonName = name;
            PokemonNumber = number;
            SpritePath = spritePath;
            IsCaught = isCaught;

            // Set size and border.
            Size = new Size(120, 170);
            BorderStyle = BorderStyle.FixedSingle;
            BackColor = isCaught ? Color.Green : Color.LightGray;

            // Set the pokeball background using the panel's BackgroundImage.
            SetPokeballBackground();

            // Add the sprite and labels.
            AddSprite();
            AddNumberLabel();
            AddNameLabel();
        }

        private void SetPokeballBackground()
        {
            AssetManager assetManager = new AssetManager();
            // Assuming the pokeball background image is stored as "pokeballBg.png" in the Assets folder.
            string pokeballBgPath = assetManager.GetPokemonCardBackgroundPath();

            if (File.Exists(pokeballBgPath))
            {
                // Load the image and assign it as the panel's background.
                BackgroundImage = Image.FromFile(pokeballBgPath);
                BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                // If the image file isn't found, set a fallback background color.
                BackColor = Color.LightBlue;
            }
        }

        private void AddSprite()
        {
            var sprite = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(100, 100),
                // Position the sprite so it appears over the background.
                Location = new Point(10, 35),
                BackColor = Color.Transparent,
                Enabled = false
            };

            if (File.Exists(SpritePath))
            {
                sprite.ImageLocation = SpritePath;
            }

            Controls.Add(sprite);
            sprite.BringToFront();
        }

        private void AddNumberLabel()
        {
            var numberLabel = new Label
            {
                Text = $"#{PokemonNumber}",
                Location = new Point(5, 5),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.Transparent,
                ForeColor = Color.Black
            };

            Controls.Add(numberLabel);
            numberLabel.BringToFront();
        }

        private void AddNameLabel()
        {
            var nameLabel = new Label
            {
                Text = PokemonName,
                Size = new Size(120, 20),
                // Position the name label at the bottom of the card.
                Location = new Point(0, Height - 25),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.Transparent,
                ForeColor = Color.Black
            };

            Controls.Add(nameLabel);
            nameLabel.BringToFront();
        }

        public void UpdateCaughtStatus(bool isCaught)
        {
            IsCaught = isCaught;
            BackColor = isCaught ? Color.Green : Color.LightGray;
        }
    }
}
