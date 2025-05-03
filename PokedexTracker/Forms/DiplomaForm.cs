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
        private readonly string _gameName, _playerName;
        private readonly bool _missingMew;
        private readonly AssetManager _assets;
        private readonly DiplomaNameDisplayManager _nameMgr;
        private readonly DiplomaTimeDisplayManager _timeMgr;

        // state
        private string _mediaVersion = "GB";
        private string _printVersion = "Regular";

        // sets
        private static readonly string[] SgbGames = { "Red", "Blue" };
        private static readonly string[] PrintGames = { "Yellow", "Gold", "Silver", "Crystal" };
        private static readonly string[] NationalPokedex = {"Ruby", "Sapphire", 
            "Emerald", "Diamond", "Pearl", "Platinum", "HeartGold", "SoulSilver", "Black",
        "White", "Black 2", "White 2", "X", "Y", "Omega Ruby", "Alpha Sapphire", "Brilliant Diamond", "Shinning Pearl"};
        private static readonly string[] KantoPokedex = {"Fire Red", "Leaf Green", "Let's Go Pikachu", "Let's Go Eevee" };
        private static readonly string[] JohtoPokedex = { "Heart Gold", "Soul Silver"};
        private static readonly string[] HoennPokedex = { "Ruby", "Sapphire", "Emerald", "Omega Ruby", "Alpha Sapphire" };
        private static readonly string[] SinnohPokedex = { "Diamond", "Pearl", "Platinum", "Brilliant Diamond", "Shinning Pearl" };
        private static readonly string[] UnovaPokedex = { "Black", "White", "Black 2", "White 2" };
        private static readonly string[] KalosPokedex = { "X", "Y" };
        //Add other games later on

        public DiplomaForm(string gameName, string playerName, bool missingMew)
        {
            InitializeComponent();

            _gameName = gameName;
            _playerName = playerName;
            _missingMew = missingMew;
            _assets = new AssetManager();
            _nameMgr = new DiplomaNameDisplayManager();
            _timeMgr = new DiplomaTimeDisplayManager();

            // wire our two generic radios
            radioOption1.CheckedChanged += Option_CheckedChanged;
            radioOption2.CheckedChanged += Option_CheckedChanged;

            // time inputs
            txtHour.TextChanged += (_, __) => LoadAndComposeDiploma();
            txtMinute.TextChanged += (_, __) => LoadAndComposeDiploma();

            // Only allow digits in them
            txtHour.KeyPress += NumericOnly_KeyPress;
            txtMinute.KeyPress += NumericOnly_KeyPress;
        }

        private void DiplomaForm_Load(object sender, EventArgs e)
        {
            bool isSgbMode = SgbGames.Contains(_gameName);
            bool isPrintMode = PrintGames.Contains(_gameName);

            if (isSgbMode)
            {
                // GameBoy version selection
                lblOption.Text = "Choose Gameboy Version:";
                radioOption1.Text = "GB";
                radioOption2.Text = "SGB";
                radioOption1.Checked = true;     // default
                radioOption2.Checked = false;

                lblOption.Visible =
                radioOption1.Visible =
                radioOption2.Visible = true;
            }
            else if (isPrintMode)
            {
                // Diploma version selection
                lblOption.Text = "Choose Diploma Version:";
                radioOption1.Text = "Regular";
                radioOption2.Text = "Printer";
                radioOption1.Checked = true;     // default
                radioOption2.Checked = false;

                lblOption.Visible =
                radioOption1.Visible =
                radioOption2.Visible = true;
            }
            else
            {
                // hide both
                lblOption.Visible =
                radioOption1.Visible =
                radioOption2.Visible = false;
            }

            ToggleHourMinuteControls();
            LoadAndComposeDiploma();
        }

        private void Option_CheckedChanged(object sender, EventArgs e)
        {
            // only react on the checked one
            var r = sender as RadioButton;
            if (r == null || !r.Checked) return;

            // if we're in GameBoy mode...
            if (SgbGames.Contains(_gameName))
            {
                _mediaVersion = (r.Text == "SGB") ? "SGB" : "GB";
            }
            // else if Diploma mode...
            else if (PrintGames.Contains(_gameName))
            {
                _printVersion = (r.Text == "Printer") ? "Printer" : "Regular";
            }

            ToggleHourMinuteControls();
            LoadAndComposeDiploma();
        }

        private void ToggleHourMinuteControls()
        {
            bool showTime = (_printVersion == "Printer")
                            && PrintGames.Contains(_gameName);
            lblHour.Visible =
            txtHour.Visible =
            lblMinute.Visible =
            txtMinute.Visible = showTime;
        }

        private void LoadAndComposeDiploma()
        {
            // special missing-Mew override for Yellow+Printer
            string actualPrint = (_gameName == "Yellow"
                                  && _printVersion == "Printer"
                                  && _missingMew)
                                 ? "PrinterMissingMew"
                                 : _printVersion;

            // build path
            string path = _assets.GetDiplomaPath(
                _gameName,
                _mediaVersion,
                actualPrint);

            if (!File.Exists(path))
            {
                MessageBox.Show($"Diploma not found: {path}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            using (var baseImg = Image.FromFile(path))
            using (var bmp = new Bitmap(
                                  baseImg.Width,
                                  baseImg.Height,
                                  PixelFormat.Format32bppArgb))
            using (var g = Graphics.FromImage(bmp))
            {
                // draw background
                g.PageUnit = GraphicsUnit.Pixel;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.None;
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.CompositingMode = CompositingMode.SourceOver;
                g.SmoothingMode = SmoothingMode.None;
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                g.DrawImage(baseImg, Point.Empty);

                // draw player name
                var nameFmt = _nameMgr.GetFontSettings(_gameName);
                g.CompositingMode = CompositingMode.SourceCopy;
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                g.SmoothingMode = SmoothingMode.None;
                using (var gp = new GraphicsPath())
                {
                    gp.AddString(
                        _playerName,
                        nameFmt.Family,
                        (int)FontStyle.Regular,
                        nameFmt.Size,
                        new PointF(nameFmt.Location.X, nameFmt.Location.Y),
                        StringFormat.GenericDefault);
                    g.FillPath(Brushes.Black, gp);
                }

                // restore for timestamp
                g.CompositingMode = CompositingMode.SourceOver;
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // draw hours/minutes if Printer
                if ((actualPrint == "Printer" || actualPrint == "PrinterMissingMew")
                    && PrintGames.Contains(_gameName))
                {
                    var fs = _timeMgr.GetFontSettings(_gameName);

                    // --- HOURS: measure and right-align ---
                    using (var fH = new Font(
                        fs.HourSettings.Family,
                        fs.HourSettings.Size,
                        FontStyle.Regular,
                        GraphicsUnit.Pixel))
                    {
                        string h = txtHour.Text.PadLeft(2, '0');
                        // measure width in pixels
                        SizeF sz = g.MeasureString(h, fH);
                        // compute X so text right edge == fs.HourSettings.Location.X
                        float x = fs.HourSettings.Location.X - sz.Width;
                        float y = fs.HourSettings.Location.Y;
                        g.DrawString(h, fH, Brushes.Black, new PointF(x, y));
                    }

                    // --- MINUTES: unchanged (left-align) ---
                    using (var fM = new Font(
                        fs.MinuteSettings.Family,
                        fs.MinuteSettings.Size,
                        FontStyle.Regular,
                        GraphicsUnit.Pixel))
                    {
                        string m = txtMinute.Text.PadLeft(2, '0');
                        g.DrawString(
                            m,
                            fM,
                            Brushes.Black,
                            fs.MinuteSettings.Location);
                    }
                }

                // clone out of the using so it isn’t disposed
                pictureBox1.Image = (Bitmap)bmp.Clone();
            }
        }


        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) return;
            using (var dlg = new SaveFileDialog
            {
                Filter = "PNG Image|*.png",
                FileName = $"{_gameName}_Diploma_{_playerName}.png"
            })
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    pictureBox1.Image.Save(dlg.FileName, ImageFormat.Png);
        }

        private void buttonClose_Click(object sender, EventArgs e) => Close();

        private void NumericOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control keys (backspace, delete etc.) and digits only
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }
    }
}
