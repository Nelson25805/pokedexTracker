using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
// Make sure to include the namespace where PokemonCard is defined:
using PokedexTracker;

namespace PokedexTracker.Helpers
{
    public class UILayoutManager
    {
        // Dictionary to hold only PokemonCard controls and their original bounds.
        private Dictionary<Control, Rectangle> _originalBounds = new Dictionary<Control, Rectangle>();

        // The container (form or panel) whose size we are tracking.
        private Control _container;

        // The original size of the container.
        private Size _originalSize;

        public UILayoutManager(Control container)
        {
            _container = container;
            _originalSize = container.ClientSize;
            // Capture only the original bounds for PokemonCard controls.
            CaptureOriginalBounds(_container);
            // Hook up to the resize event.
            _container.Resize += Container_Resize;
        }

        private void CaptureOriginalBounds(Control container)
        {
            foreach (Control ctrl in container.Controls)
            {
                // Only capture bounds for controls that are PokemonCards.
                if (ctrl is PokemonCard)
                {
                    _originalBounds[ctrl] = ctrl.Bounds;
                }

                // If the control contains child controls, search them recursively.
                if (ctrl.Controls.Count > 0)
                {
                    CaptureOriginalBounds(ctrl);
                }
            }
        }

        private void Container_Resize(object sender, EventArgs e)
        {
            // Calculate the scale factor for width and height.
            float xScale = (float)_container.ClientSize.Width / _originalSize.Width;
            float yScale = (float)_container.ClientSize.Height / _originalSize.Height;

            // Resize only the PokemonCard controls.
            foreach (var kvp in _originalBounds)
            {
                Control ctrl = kvp.Key;
                // Although we already filtered for PokemonCard, you can check again if needed.
                if (ctrl is PokemonCard)
                {
                    Rectangle originalRect = kvp.Value;
                    int newX = (int)(originalRect.X * xScale);
                    int newY = (int)(originalRect.Y * yScale);
                    int newWidth = (int)(originalRect.Width * xScale);
                    int newHeight = (int)(originalRect.Height * yScale);
                    ctrl.Bounds = new Rectangle(newX, newY, newWidth, newHeight);
                }
            }
        }

        // Call this method if you add or remove PokemonCards dynamically.
        public void ResetOriginalBounds()
        {
            _originalBounds.Clear();
            _originalSize = _container.ClientSize;
            CaptureOriginalBounds(_container);
        }
    }
}
