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

            // Increase the card's height to give more room
            Size = new Size(120, 170);
            BorderStyle = BorderStyle.FixedSingle;
            BackColor = isCaught ? Color.Green : Color.LightGray;

            AddNumberLabel();
            AddSprite();
            AddNameLabel();
        }

        private void AddSprite()
        {
            var sprite = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(100, 100),
                // Move the sprite down to avoid overlapping the number label.
                Location = new Point(10, 35), // Changed from (10, 10) or (10, 25) to (10, 35)
                Enabled = false
            };

            if (File.Exists(SpritePath))
            {
                sprite.ImageLocation = SpritePath;
            }

            Controls.Add(sprite);
        }

        private void AddNumberLabel()
        {
            var numberLabel = new Label
            {
                Text = $"#{PokemonNumber}",
                Location = new Point(5, 5), // Top left corner
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.Transparent,
                ForeColor = Color.Black
            };

            Controls.Add(numberLabel);
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
        }

        public void UpdateCaughtStatus(bool isCaught)
        {
            IsCaught = isCaught;
            BackColor = isCaught ? Color.Green : Color.LightGray;
        }
    }
}
