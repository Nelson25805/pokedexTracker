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
        private readonly DiplomaTimeDisplayManager _timeMgr = new DiplomaTimeDisplayManager();

        // two version axes:
        private string _mediaVersion = "GB";       // always GB
        private string _printVersion = "Regular";  // or "Printer"
        private readonly bool _missingMew;         // new flag!

        // only Red/Blue get SGB
        private static readonly string[] SgbGames = { "Red", "Blue" };

        // only these get printer toggle
        private static readonly string[] PrintGames = { "Yellow", "Gold", "Silver", "Crystal" };

        // 1) Primary ctor
        public DiplomaForm(string gameName, string playerName)
        {
            InitializeComponent();
            _gameName = gameName;
            _playerName = playerName;
            _assets = new AssetManager();
            _nameMgr = new DiplomaNameDisplayManager();

            // wire media radios
            radioGB.CheckedChanged += MediaVersion_CheckedChanged;
            radioSGB.CheckedChanged += MediaVersion_CheckedChanged;
            // wire print radios
            radioRegular.CheckedChanged += PrintVersion_CheckedChanged;
            radioPrinter.CheckedChanged += PrintVersion_CheckedChanged;

            // time edits
            txtHour.TextChanged += (s, e) => LoadAndComposeDiploma();
            txtMinute.TextChanged += (s, e) => LoadAndComposeDiploma();

            // save/close
            buttonSave.Click += buttonSave_Click;
            buttonClose.Click += buttonClose_Click;
        }

        // 2) Overload that accepts “missingMew”
        public DiplomaForm(string gameName, string playerName, bool missingMew)
            : this(gameName, playerName)
        {
            _missingMew = missingMew;
        }

        private void DiplomaForm_Load(object sender, EventArgs e)
        {
            // MEDIA defaults
            radioGB.Checked = true;
            radioGB.Enabled = true;
            bool sgbOK = SgbGames.Contains(_gameName);
            radioSGB.Enabled = sgbOK;
            radioSGB.Checked = false;

            // PRINT toggles
            bool printOK = PrintGames.Contains(_gameName);
            radioRegular.Enabled = printOK;
            radioPrinter.Enabled = printOK;
            radioRegular.Checked = true;
            radioPrinter.Checked = false;

            // show/hide time boxes
            ToggleHourMinuteControls();

            // initial render
            LoadAndComposeDiploma();
        }

        private void MediaVersion_CheckedChanged(object sender, EventArgs e)
        {
            _mediaVersion = radioSGB.Checked ? "SGB" : "GB";
            LoadAndComposeDiploma();
        }

        private void PrintVersion_CheckedChanged(object sender, EventArgs e)
        {
            _printVersion = radioPrinter.Checked ? "Printer" : "Regular";
            ToggleHourMinuteControls();
            LoadAndComposeDiploma();
        }

        private void ToggleHourMinuteControls()
        {
            bool show = (_printVersion == "Printer") && PrintGames.Contains(_gameName);
            lblHour.Visible = txtHour.Visible = show;
            lblMinute.Visible = txtMinute.Visible = show;
        }

        private void LoadAndComposeDiploma()
        {
            // 1) Determine which “print” variant to load
            string actualPrint = _printVersion;

            // For Yellow + Printer + missingMew → use special “PrinterMissing” file
            if (_gameName == "Yellow"
             && _printVersion == "Printer"
             && _missingMew)
            {
                actualPrint = "PrinterMissingMew";
                // ensure you have: Yellow-GB-PrinterMissing.png
            }

            // 2) Build the path
            string path = _assets.GetDiplomaPath(
                _gameName,
                _mediaVersion,
                actualPrint
            );

            if (!File.Exists(path))
            {
                MessageBox.Show(
                    $"Diploma not found: {path}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error
                );
                Close();
                return;
            }

            // 3) Load & draw
            using (var baseImg = Image.FromFile(path))
            {
                var bmp = new Bitmap(
                    baseImg.Width,
                    baseImg.Height,
                    PixelFormat.Format32bppArgb
                );
                using (var g = Graphics.FromImage(bmp))
                {
                    // draw base
                    g.PageUnit = GraphicsUnit.Pixel;
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.PixelOffsetMode = PixelOffsetMode.None;
                    g.CompositingQuality = CompositingQuality.HighSpeed;
                    g.CompositingMode = CompositingMode.SourceOver;
                    g.SmoothingMode = SmoothingMode.None;
                    g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                    g.DrawImage(baseImg, Point.Empty);

                    // name overwrite
                    var nameFmt = _nameMgr.GetFontSettings(_gameName);
                    g.CompositingMode = CompositingMode.SourceCopy;
                    g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                    g.SmoothingMode = SmoothingMode.None;
                    using (var pathName = new GraphicsPath())
                    {
                        pathName.AddString(
                            _playerName,
                            nameFmt.Family,
                            (int)FontStyle.Regular,
                            nameFmt.Size,
                            new PointF(nameFmt.Location.X, nameFmt.Location.Y),
                            StringFormat.GenericDefault
                        );
                        using (var halo = new Pen(Color.White, 3) { LineJoin = LineJoin.Round })
                            g.DrawPath(halo, pathName);
                        g.FillPath(Brushes.Black, pathName);
                    }

                    // restore & hour/minute...
                    g.CompositingMode = CompositingMode.SourceOver;
                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    if (actualPrint.StartsWith("Printer") && PrintGames.Contains(_gameName))
                    {
                        var twoFs = _timeMgr.GetFontSettings(_gameName);
                        // Hour
                        using (var fH = new Font(
                            twoFs.HourSettings.Family,
                            twoFs.HourSettings.Size,
                            FontStyle.Regular,
                            GraphicsUnit.Pixel))
                            g.DrawString(
                                txtHour.Text.PadLeft(2, '0'),
                                fH,
                                Brushes.Black,
                                twoFs.HourSettings.Location
                            );
                        // Minute
                        using (var fM = new Font(
                            twoFs.MinuteSettings.Family,
                            twoFs.MinuteSettings.Size,
                            FontStyle.Regular,
                            GraphicsUnit.Pixel))
                            g.DrawString(
                                txtMinute.Text.PadLeft(2, '0'),
                                fM,
                                Brushes.Black,
                                twoFs.MinuteSettings.Location
                            );
                    }
                }
                pictureBox1.Image = bmp;
            }
        }

        // no change to SetPrinterDefault()
        public void SetPrinterDefault(bool printer)
        {
            radioPrinter.Checked = printer;
            radioRegular.Checked = !printer;
            ToggleHourMinuteControls();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) return;
            using (var dlg = new SaveFileDialog
            {
                Filter = "PNG Image|*.png",
                FileName = $"{_gameName}_Diploma_{_playerName}.png"
            })
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    pictureBox1.Image.Save(dlg.FileName, ImageFormat.Png);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e) => Close();
    }
}
