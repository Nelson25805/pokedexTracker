// DiplomaForm.cs
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        // which version to show ("GB" or "SGB")
        private string _version = "GB";

        // only these games get two versions
        private static readonly string[] MultiVersionGames =
            { "Red", "Blue", "Yellow", "Gold", "Silver", "Crystal" };

        public DiplomaForm(string gameName, string playerName)
        {
            InitializeComponent();
            _gameName = gameName;
            _playerName = playerName;
            _assets = new AssetManager();
            _nameMgr = new DiplomaNameDisplayManager();

            // wire up the radio‐toggle events
            radioGB.CheckedChanged += VersionRadio_CheckedChanged;
            radioSGB.CheckedChanged += VersionRadio_CheckedChanged;
        }

        private void DiplomaForm_Load(object sender, EventArgs e)
        {
            // enable toggles only for our multi‐version games
            bool multi = MultiVersionGames.Contains(_gameName);
            radioGB.Enabled = multi;
            radioSGB.Enabled = multi;

            // default to GB
            radioGB.Checked = true;
            radioSGB.Checked = false;

            LoadAndComposeDiploma();
        }

        private void VersionRadio_CheckedChanged(object sender, EventArgs e)
        {
            // flip version key and reload diploma
            if (radioGB.Checked) _version = "GB";
            if (radioSGB.Checked) _version = "SGB";
            LoadAndComposeDiploma();
        }

        private void LoadAndComposeDiploma()
        {
            // build path: uses your updated GetDiplomaPath(game, version)
            string path = _assets.GetDiplomaPath(_gameName, _version);
            if (!File.Exists(path))
            {
                MessageBox.Show($"Diploma image not found: {path}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            // classic using‐blocks to load & compose
            Image baseImg = Image.FromFile(path);
            try
            {
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

                // hand it to the UI
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
