using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PokedexTracker.Helpers
{
    public class UILayoutManager
    {
        // Dictionary to hold each control and its original bounds.
        private Dictionary<Control, Rectangle> _originalBounds = new Dictionary<Control, Rectangle>();
        // Dictionary to hold the original fonts for controls (e.g. Labels) that you want scaled.
        private Dictionary<Control, Font> _originalFonts = new Dictionary<Control, Font>();

        // The container (form or panel) whose size we are tracking.
        private Control _container;

        // The original size of the container.
        private Size _originalSize;

        public UILayoutManager(Control container)
        {
            _container = container;
            _originalSize = container.ClientSize;
            // Capture the original bounds (and fonts) for all child controls.
            CaptureOriginalBounds(_container);
            // Hook up to the resize event.
            _container.Resize += Container_Resize;
        }

        private void CaptureOriginalBounds(Control container)
        {
            foreach (Control ctrl in container.Controls)
            {
                // Save the original bounds.
                _originalBounds[ctrl] = ctrl.Bounds;

                // If the control is a label, store its original font.
                if (ctrl is Label)
                {
                    _originalFonts[ctrl] = ctrl.Font;
                }

                // Recursively capture child controls.
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

            // Resize each control based on its original bounds.
            foreach (var kvp in _originalBounds)
            {
                Control ctrl = kvp.Key;
                Rectangle originalRect = kvp.Value;
                int newX = (int)(originalRect.X * xScale);
                int newY = (int)(originalRect.Y * yScale);
                int newWidth = (int)(originalRect.Width * xScale);
                int newHeight = (int)(originalRect.Height * yScale);
                ctrl.Bounds = new Rectangle(newX, newY, newWidth, newHeight);

                // If the control is a label and we stored an original font, update its font size.
                if (ctrl is Label && _originalFonts.ContainsKey(ctrl))
                {
                    Font originalFont = _originalFonts[ctrl];
                    // Use the smaller scale factor to avoid disproportionate stretching.
                    float scale = Math.Min(xScale, yScale);
                    float newFontSize = originalFont.Size * scale;
                    // (Optional) Prevent the font from becoming too small.
                    if (newFontSize < 1)
                        newFontSize = 1;
                    ctrl.Font = new Font(originalFont.FontFamily, newFontSize, originalFont.Style);
                }
            }
        }

        // Call this method if you add or remove controls dynamically and need to update the captured layout.
        public void ResetOriginalBounds()
        {
            _originalBounds.Clear();
            _originalFonts.Clear();
            _originalSize = _container.ClientSize;
            CaptureOriginalBounds(_container);
        }
    }
}
