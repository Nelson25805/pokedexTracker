// DiplomaForm.cs
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
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

        public DiplomaForm(string gameName, string playerName)
        {
            InitializeComponent();
            _gameName = gameName;
            _playerName = playerName;
            _assets = new AssetManager();
            _nameMgr = new DiplomaNameDisplayManager();
        }

        // DiplomaForm.cs (inside the form’s code‐behind)

        private void DiplomaForm_Load(object sender, EventArgs e)
        {
            string path = _assets.GetDiplomaPath(_gameName);
            if (!File.Exists(path))
            {
                MessageBox.Show($"Diploma image not found: {path}");
                Close();
                return;
            }

            // Load and draw in classic using-blocks
            Image baseImg = Image.FromFile(path);
            try
            {
                // Create a new bitmap to compose onto
                var bmp = new Bitmap(baseImg.Width, baseImg.Height, baseImg.PixelFormat);

                using (var g = Graphics.FromImage(bmp))
                {
                    g.PageUnit = GraphicsUnit.Pixel;
                    g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    // draw the blank diploma
                    g.DrawImage(baseImg, Point.Empty);

                    // draw the playerName
                    var fmts = _nameMgr.GetFontSettings(_gameName);
                    using (var font = new Font(fmts.Family, fmts.Size, FontStyle.Regular, GraphicsUnit.Pixel))
                    {
                        g.DrawString(_playerName, font, Brushes.Black, fmts.Location);
                    }
                }

                // Hand the composed image off to the PictureBox
                pictureBox1.Image = bmp;
            }
            finally
            {
                baseImg.Dispose();
            }
        }

    }
}
