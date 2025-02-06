using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PokedexTracker.Helpers
{
    public static class FontLoader
    {
        private static PrivateFontCollection _privateFontCollection = new PrivateFontCollection();

        /// <summary>
        /// Loads an embedded font from a resource.
        /// resourceName must be the full resource name (e.g. "PokedexTracker.Fonts.Gen3.ttf").
        /// </summary>
        public static Font LoadEmbeddedFont(string resourceName, float size)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream fontStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (fontStream == null)
                    throw new Exception("Font resource not found: " + resourceName);
                byte[] fontData = new byte[fontStream.Length];
                fontStream.Read(fontData, 0, fontData.Length);

                // Pin the byte array so that its address remains fixed.
                GCHandle handle = GCHandle.Alloc(fontData, GCHandleType.Pinned);
                try
                {
                    IntPtr pFontData = handle.AddrOfPinnedObject();
                    _privateFontCollection.AddMemoryFont(pFontData, fontData.Length);
                }
                finally
                {
                    handle.Free();
                }
            }

            if (_privateFontCollection.Families.Length > 0)
            {
                FontFamily family = _privateFontCollection.Families[_privateFontCollection.Families.Length - 1];
                return new Font(family, size);
            }
            throw new Exception("No font families found in resource: " + resourceName);
        }
    }
}
