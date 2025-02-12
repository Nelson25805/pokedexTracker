using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class PokeballRadioButton : RadioButton
{
    // Properties to hold the images for checked and unchecked states.
    public Image CheckedImage { get; set; }
    public Image UncheckedImage { get; set; }

    public PokeballRadioButton()
    {
        // Enable custom painting.
        this.SetStyle(ControlStyles.UserPaint, true);
        this.DoubleBuffered = true;
        this.AutoSize = false;
    }

    protected override void OnPaint(PaintEventArgs e)
    {

        // Debug the control size.
        System.Diagnostics.Debug.WriteLine($"Control size: {this.Width} x {this.Height}");

        // Define the drawing area based on Padding.
        Rectangle clientRect = new Rectangle(
            this.Padding.Left,
            this.Padding.Top,
            this.Width - this.Padding.Horizontal,
            this.Height - this.Padding.Vertical);

        // Clear the background.
        e.Graphics.Clear(this.BackColor);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        // Use a fixed spacing between elements.
        int pad = 5;  // you can adjust this if needed

        // Define the desired image size.
        int imageSize = 24;

        // Compute the rectangle for the image.
        // Draw the image with a left margin of pad and vertically centered within clientRect.
        Rectangle imageRect = new Rectangle(
            clientRect.Left + pad,
            clientRect.Top + (clientRect.Height - imageSize) / 2,
            imageSize,
            imageSize);

        // Draw the image (or fallback ellipse) based on the Checked state.
        if (this.Checked)
        {
            if (CheckedImage != null)
            {
                e.Graphics.DrawImage(CheckedImage, imageRect);
            }
            else
            {
                using (Pen pen = new Pen(this.ForeColor, 2))
                {
                    e.Graphics.DrawEllipse(pen, imageRect);
                    using (SolidBrush brush = new SolidBrush(this.ForeColor))
                    {
                        e.Graphics.FillEllipse(brush, imageRect);
                    }
                }
            }
        }
        else
        {
            if (UncheckedImage != null)
            {
                e.Graphics.DrawImage(UncheckedImage, imageRect);
            }
            else
            {
                using (Pen pen = new Pen(this.ForeColor, 2))
                {
                    e.Graphics.DrawEllipse(pen, imageRect);
                }
            }
        }

        // Compute the rectangle for the text.
        // Start drawing the text after the image plus a small gap.
        int textX = imageRect.Right + pad;
        int textWidth = clientRect.Right - textX - pad;  // Ensure we leave a bit of right padding
        Rectangle textRect = new Rectangle(textX, clientRect.Top, textWidth, clientRect.Height);

        // Prepare the StringFormat for left alignment and vertical centering.
        StringFormat sf = new StringFormat
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Center,
            Trimming = StringTrimming.EllipsisCharacter,
            FormatFlags = StringFormatFlags.NoWrap
        };

        // Draw the text.
        using (SolidBrush textBrush = new SolidBrush(this.ForeColor))
        {
            e.Graphics.DrawString(this.Text, this.Font, textBrush, textRect, sf);
        }
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        base.OnEnabledChanged(e);
        // Change appearance when disabled (for example, update the ForeColor)
        if (!this.Enabled)
        {
            this.ForeColor = Color.Gray; // Or any style you like
        }
        else
        {
            this.ForeColor = Color.Black;
        }
        Invalidate(); // Redraw the control to reflect changes
    }



}
