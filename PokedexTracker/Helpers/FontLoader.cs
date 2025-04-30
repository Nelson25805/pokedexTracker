// FontLoader.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PokedexTracker.Helpers
{
    public static class FontLoader
    {
        public static Font LoadEmbeddedFont(string resourceName, float size)
        {
            var coll = LoadFontCollection(new[] { GetFileName(resourceName) });
            var family = coll.Families[0];
            return new Font(family, size, FontStyle.Regular, GraphicsUnit.Pixel);
        }

        public static PrivateFontCollection LoadFontCollection(IEnumerable<string> fontFiles)
        {
            var collection = new PrivateFontCollection();
            var asm = Assembly.GetExecutingAssembly();
            foreach (var file in fontFiles)
            {
                string res = $"PokedexTracker.Assets.Fonts.{file}";
                using (var stream = asm.GetManifestResourceStream(res))
                {
                    if (stream == null)
                        throw new InvalidOperationException($"Font resource not found: {res}");
                    byte[] data = new byte[stream.Length];
                    stream.Read(data, 0, data.Length);
                    var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                    try { collection.AddMemoryFont(handle.AddrOfPinnedObject(), data.Length); }
                    finally { handle.Free(); }
                }
            }
            return collection;
        }

        public static FontFamily GetFamilyFromCollection(
            PrivateFontCollection coll,
            string fileName)
        {
            var baseName = Path.GetFileNameWithoutExtension(fileName);
            foreach (var fam in coll.Families)
                if (fam.Name.Equals(baseName, StringComparison.OrdinalIgnoreCase))
                    return fam;
            return coll.Families.Length > 0 ? coll.Families[0]
                   : throw new InvalidOperationException("No font families available");
        }

        public static FontFamily LoadEmbeddedFontFamily(string fontFileName)
        {
            // fontFileName should be like "Gen1+2.ttf"
            var coll = LoadFontCollection(new[] { fontFileName });
            if (coll.Families.Length == 0)
                throw new InvalidOperationException($"No font families found for {fontFileName}");
            return coll.Families[0];
        }

        private static string GetFileName(string resourceName)
        {
            int idx = resourceName.LastIndexOf('.');
            return (idx >= 0 && idx + 1 < resourceName.Length)
                 ? resourceName.Substring(idx + 1)
                 : resourceName;
        }
    }
}