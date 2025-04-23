// CardComposer.cs
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace PokedexTracker.Helpers
{
    public struct FontSettings
    {
        public FontFamily Family;
        public float Size;
        public Point Location;
        public FontSettings(FontFamily f, float s, Point loc)
        { Family = f; Size = s; Location = loc; }
    }

    public static class CardComposer
    {
        public static Bitmap ComposeTrainerCard(
            Image baseCard,
            Image badgeImage,
            FontSettings nameSettings,
            string playerName,
            FontSettings progressSettings,
            string progressText)
        {
            var bmp = new Bitmap(baseCard.Width, baseCard.Height, baseCard.PixelFormat);
            using (var g = Graphics.FromImage(bmp))
            {
                g.PageUnit = GraphicsUnit.Pixel;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // draw card+badge
                g.DrawImage(baseCard, Point.Empty);

                // draw name
                using (var f = new Font(nameSettings.Family,
                                         nameSettings.Size,
                                         FontStyle.Regular,
                                         GraphicsUnit.Pixel))
                {
                    g.DrawString(playerName, f, Brushes.Black, nameSettings.Location);
                }

                // draw progress WITH 1px outline
                using (var font = new Font(progressSettings.Family,
                                           progressSettings.Size,
                                           FontStyle.Regular,
                                           GraphicsUnit.Pixel))
                using (var path = new GraphicsPath())
                {
                    // 1) build the text path
                    path.AddString(
                        progressText,
                        progressSettings.Family,
                        (int)FontStyle.Regular,
                        progressSettings.Size,               // emSize == pixel height
                        new PointF(progressSettings.Location.X, progressSettings.Location.Y),
                        StringFormat.GenericDefault
                    );

                    // 2) outline (white halo)
                    using (var pen = new Pen(Color.White, 2) { LineJoin = LineJoin.Round })
                    {
                        g.DrawPath(pen, path);
                    }

                    // 3) fill (black text)
                    g.FillPath(Brushes.Black, path);
                }
            }
            return bmp;
        }
    }
}
