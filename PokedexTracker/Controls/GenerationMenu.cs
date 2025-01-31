using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PokedexTracker.Controls
{
    public class GenerationMenu
    {
        private readonly Panel _menuPanel;
        private readonly List<Label> _generationLabels = new List<Label>();

        /// <summary>
        /// The currently selected index.
        /// </summary>
        public int SelectedIndex { get; private set; } = 0;

        public Label ArrowLabel { get; private set; }

        /// <summary>
        /// Raised when a generation is clicked.
        /// </summary>
        public event EventHandler GenerationClicked;

        public GenerationMenu(Panel menuPanel, IEnumerable<string> generationNames)
        {
            _menuPanel = menuPanel;
            InitializeMenu(generationNames);
        }

        private void InitializeMenu(IEnumerable<string> generationNames)
        {
            int yPosition = 20;
            int index = 0; // keep track of label index

            foreach (var name in generationNames)
            {
                var label = new Label
                {
                    Text = name,
                    Location = new Point(20, yPosition),
                    AutoSize = true,
                    ForeColor = Color.Black,
                    Tag = index  // store the index in the Tag
                };

                // Attach mouse events:
                label.MouseEnter += GenerationLabel_MouseEnter;
                label.Click += GenerationLabel_Click;

                _menuPanel.Controls.Add(label);
                _generationLabels.Add(label);
                yPosition += 30;
                index++;
            }

            ArrowLabel = new Label
            {
                Text = "▶",
                Location = new Point(0, 20),
                AutoSize = true,
                ForeColor = Color.Black
            };
            _menuPanel.Controls.Add(ArrowLabel);
        }

        /// <summary>
        /// When the user hovers over a label, update the selection.
        /// </summary>
        private void GenerationLabel_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Label hoveredLabel)
            {
                int hoveredIndex = (int)hoveredLabel.Tag;
                SelectedIndex = hoveredIndex;
                UpdateArrowPosition();
            }
        }

        /// <summary>
        /// When the user clicks on a label, fire the GenerationClicked event.
        /// </summary>
        private void GenerationLabel_Click(object sender, EventArgs e)
        {
            // We already update the selected index on hover,
            // so clicking will simply confirm the current selection.
            GenerationClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Moves the arrow upward using the keyboard.
        /// </summary>
        public void MoveUp()
        {
            if (_generationLabels.Count == 0) return;
            SelectedIndex = (SelectedIndex > 0) ? SelectedIndex - 1 : _generationLabels.Count - 1;
            UpdateArrowPosition();
        }

        /// <summary>
        /// Moves the arrow downward using the keyboard.
        /// </summary>
        public void MoveDown()
        {
            if (_generationLabels.Count == 0) return;
            SelectedIndex = (SelectedIndex < _generationLabels.Count - 1) ? SelectedIndex + 1 : 0;
            UpdateArrowPosition();
        }

        /// <summary>
        /// Updates the arrow position to align with the selected label.
        /// </summary>
        private void UpdateArrowPosition()
        {
            if (_generationLabels.Count == 0) return;
            // Update the arrow's Y position based on the currently selected label.
            int newY = _generationLabels[SelectedIndex].Location.Y;
            ArrowLabel.Location = new Point(0, newY);
        }

        /// <summary>
        /// Returns the generation name of the currently selected label.
        /// </summary>
        public string GetSelectedGenerationName()
        {
            if (_generationLabels.Count == 0) return string.Empty;
            return _generationLabels[SelectedIndex].Text;
        }
    }
}
