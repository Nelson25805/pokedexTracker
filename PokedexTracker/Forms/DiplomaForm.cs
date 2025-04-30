// DiplomaForm.cs
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PokedexTracker.DisplayManagers;
using PokedexTracker.Helpers;

namespace PokedexTracker.Forms
{
    public partial class DiplomaForm : Form
    {
        private readonly string _gameName;
        private readonly string _playerName;
        private readonly AssetManager _assets;
        private readonly DiplomaNameDisplayManager _nameMgr;

        // two version axes:
        private string _mediaVersion = "GB";      // or "SGB"
        private string _printVersion = "Regular"; // or "Printer"

        // which games get the GB/SGB toggles:
        private static readonly string[] MediaGames =
            { "Red", "Blue", "Yellow", "Gold", "Silver", "Crystal" };

        // which games get the print toggles:
        private static readonly string[] PrintGames =
            { "Yellow", "Gold", "Silver", "Crystal" };

        public DiplomaForm(string gameName, string playerName)
        {
            InitializeComponent();
            _gameName = gameName;
            _playerName = playerName;
            _assets = new AssetManager();
            _nameMgr = new DiplomaNameDisplayManager();

            // wire up media‐version radios (pokeball style)
            radioGB.CheckedChanged += MediaVersion_CheckedChanged;
            radioSGB.CheckedChanged += MediaVersion_CheckedChanged;

            // wire up print‐version radios
            radioRegular.CheckedChanged += PrintVersion_CheckedChanged;
            radioPrinter.CheckedChanged += PrintVersion_CheckedChanged;
        }

        private void DiplomaForm_Load(object sender, EventArgs e)
        {
            // enable/disable media toggles
            bool mediaOK = MediaGames.Contains(_gameName);
            radioGB.Enabled = mediaOK;
            radioSGB.Enabled = mediaOK;

            // default media selection
            radioGB.Checked = true;
            radioSGB.Checked = false;

            // enable/disable print toggles
            bool printOK = PrintGames.Contains(_gameName);
            radioRegular.Enabled = printOK;
            radioPrinter.Enabled = printOK;

            // default print selection
            radioRegular.Checked = true;
            radioPrinter.Checked = false;

            LoadAndComposeDiploma();
        }

        private void MediaVersion_CheckedChanged(object sender, EventArgs e)
        {
            if (radioGB.Checked) _mediaVersion = "GB";
            if (radioSGB.Checked) _mediaVersion = "SGB";
            LoadAndComposeDiploma();
        }

        private void PrintVersion_CheckedChanged(object sender, EventArgs e)
        {
            if (radioRegular.Checked) _printVersion = "Regular";
            if (radioPrinter.Checked) _printVersion = "Printer";
            LoadAndComposeDiploma();
        }

        private void LoadAndComposeDiploma()
        {
            // build path using both axes:
            // assumes filenames like "Yellow-GB-Regular.png" or "Yellow-GB-Printer.png"
            string path = _assets.GetDiplomaPath(_gameName, _mediaVersion, _printVersion);
            if (!File.Exists(path))
            {
                MessageBox.Show($"Diploma image not found: {path}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            // load & draw exactly as before (with vector name overwrite)
            Image baseImg = Image.FromFile(path);
            try
            {
                var bmp = new Bitmap(baseImg.Width, baseImg.Height, PixelFormat.Format32bppArgb);
                using (var g = Graphics.FromImage(bmp))
                {
                    // base draw
                    g.PageUnit = GraphicsUnit.Pixel;
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.PixelOffsetMode = PixelOffsetMode.None;
                    g.CompositingQuality = CompositingQuality.HighSpeed;
                    g.CompositingMode = CompositingMode.SourceOver;
                    g.SmoothingMode = SmoothingMode.None;
                    g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                    g.DrawImage(baseImg, Point.Empty);

                    // name draw (no AA)
                    var fmts = _nameMgr.GetFontSettings(_gameName);
                    g.CompositingMode = CompositingMode.SourceCopy;
                    g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                    g.SmoothingMode = SmoothingMode.None;

                    using (var namePath = new GraphicsPath())
                    {
                        namePath.AddString(
                            _playerName,
                            fmts.Family,
                            (int)FontStyle.Regular,
                            fmts.Size,
                            new PointF(fmts.Location.X, fmts.Location.Y),
                            StringFormat.GenericDefault
                        );
                        // optional halo
                        using (var halo = new Pen(Color.White, 3) { LineJoin = LineJoin.Round })
                            g.DrawPath(halo, namePath);

                        g.FillPath(Brushes.Black, namePath);
                    }

                    // restore
                    g.CompositingMode = CompositingMode.SourceOver;
                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                }
                pictureBox1.Image = bmp;
            }
            finally
            {
                baseImg.Dispose();
            }
        }


        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
                return;

            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter = "PNG Image|*.png|JPEG Image|*.jpg;*.jpeg";
                dlg.FileName = $"{_gameName}_Diploma_{_playerName}.png";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    // choose format by extension
                    var ext = Path.GetExtension(dlg.FileName).ToLowerInvariant();
                    var img = pictureBox1.Image;
                    if (ext == ".jpg" || ext == ".jpeg")
                        img.Save(dlg.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    else
                        img.Save(dlg.FileName, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
