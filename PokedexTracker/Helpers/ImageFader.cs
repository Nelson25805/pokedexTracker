using System.Drawing;
using System.Drawing.Imaging;

namespace PokedexTracker.Helpers
{
    public static class ImageFader
    {
        public static Image AdjustImageOpacity(Image image, float opacity)
        {
            if (image == null) return null;

            var bmp = new Bitmap(image.Width, image.Height);
            using (var graphics = Graphics.FromImage(bmp))
            {
                var matrix = new ColorMatrix
                {
                    Matrix33 = opacity
                };

                var attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                graphics.DrawImage(image,
                    new Rectangle(0, 0, bmp.Width, bmp.Height),
                    0, 0, image.Width, image.Height,
                    GraphicsUnit.Pixel,
                    attributes);
            }
            return bmp;
        }
    }
}
