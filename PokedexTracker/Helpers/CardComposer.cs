// CardComposer.cs
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace PokedexTracker.Helpers
{
    public struct FontSettings
    {
        public FontFamily Family;
        public float Size;
        public Point Location;
        public FontSettings(FontFamily f, float s, Point loc)
        {
            Family = f;
            Size = s;
            Location = loc;
        }
    }

    public static class CardComposer
    {
        public static Bitmap ComposeTrainerCard(
            Image baseCard,
            Image badgeImage,            // unused; card already has badge
            FontSettings nameSettings,
            string playerName,
            FontSettings progressSettings,
            string progressText)
        {
            var bmp = new Bitmap(baseCard.Width, baseCard.Height, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bmp))
            {
                // draw the base first
                g.PageUnit = GraphicsUnit.Pixel;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.None;
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.CompositingMode = CompositingMode.SourceOver;
                g.SmoothingMode = SmoothingMode.None;
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

                g.DrawImage(baseCard, Point.Empty);

                // === Draw the player name with pure overwrite (no alpha blend!) ===
                g.CompositingMode = CompositingMode.SourceCopy;
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                g.SmoothingMode = SmoothingMode.None;

                using (var namePath = new GraphicsPath())
                {
                    namePath.AddString(
                        playerName,
                        nameSettings.Family,
                        (int)FontStyle.Regular,
                        nameSettings.Size,
                        new PointF(nameSettings.Location.X, nameSettings.Location.Y),
                        StringFormat.GenericDefault
                    );

                    // overwrite with black
                    g.FillPath(Brushes.Black, namePath);
                }

                // === Now draw progress normally (grayscale AA) ===
                g.CompositingMode = CompositingMode.SourceOver;
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                using (var progPath = new GraphicsPath())
                {
                    progPath.AddString(
                        progressText,
                        progressSettings.Family,
                        (int)FontStyle.Regular,
                        progressSettings.Size,
                        new PointF(progressSettings.Location.X, progressSettings.Location.Y),
                        StringFormat.GenericDefault
                    );

                    using (var outline = new Pen(Color.White, 2) { LineJoin = LineJoin.Round })
                        g.DrawPath(outline, progPath);

                    g.FillPath(Brushes.Black, progPath);
                }
            }
            return bmp;
        }
    }
}
