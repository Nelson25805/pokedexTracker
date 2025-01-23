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

            Size = new Size(120, 150);
            BorderStyle = BorderStyle.FixedSingle;
            BackColor = isCaught ? Color.Green : Color.LightGray;

            AddSprite();
            AddLabel();
        }

        private void AddSprite()
        {
            var sprite = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(100, 100),
                Location = new Point(10, 10),
                Enabled = false
            };

            if (File.Exists(SpritePath))
            {
                sprite.ImageLocation = SpritePath;
            }

            Controls.Add(sprite);
        }

        private void AddLabel()
        {
            var label = new Label
            {
                Text = $"{PokemonName} #{PokemonNumber}",
                Location = new Point(10, 115),
                Size = new Size(100, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                Enabled = false
            };

            Controls.Add(label);
        }

        public void UpdateCaughtStatus(bool isCaught)
        {
            IsCaught = isCaught;
            BackColor = isCaught ? Color.Green : Color.LightGray;
        }
    }
}
