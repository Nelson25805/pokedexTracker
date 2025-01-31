using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PokedexTracker.Controls
{
    public class GenerationMenu
    {
        private readonly Panel _menuPanel;
        private readonly List<Label> _generationLabels = new List<Label>();
        public Label ArrowLabel { get; private set; }
        public int SelectedIndex { get; private set; } = 0;

        public GenerationMenu(Panel menuPanel, IEnumerable<string> generationNames)
        {
            _menuPanel = menuPanel;
            InitializeMenu(generationNames);
        }

        private void InitializeMenu(IEnumerable<string> generationNames)
        {
            int yPosition = 20;
            foreach (var name in generationNames)
            {
                var label = new Label
                {
                    Text = name,
                    Location = new Point(20, yPosition),
                    AutoSize = true,
                    ForeColor = Color.Black,
                    Tag = yPosition  // store Y position for arrow alignment
                };
                _menuPanel.Controls.Add(label);
                _generationLabels.Add(label);
                yPosition += 30;
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

        public void MoveUp()
        {
            if (_generationLabels.Count == 0) return;
            SelectedIndex = (SelectedIndex > 0) ? SelectedIndex - 1 : _generationLabels.Count - 1;
            UpdateArrowPosition();
        }

        public void MoveDown()
        {
            if (_generationLabels.Count == 0) return;
            SelectedIndex = (SelectedIndex < _generationLabels.Count - 1) ? SelectedIndex + 1 : 0;
            UpdateArrowPosition();
        }

        private void UpdateArrowPosition()
        {
            int newY = (int)_generationLabels[SelectedIndex].Tag;
            ArrowLabel.Location = new Point(0, newY);
        }

        public string GetSelectedGenerationName()
        {
            if (_generationLabels.Count == 0) return string.Empty;
            return _generationLabels[SelectedIndex].Text;
        }
    }
}
