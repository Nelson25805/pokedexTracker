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
        private readonly DiplomaTimeDisplayManager _timeMgr = new DiplomaTimeDisplayManager();
        private readonly bool _missingMew;

        // Current selections
        private string _mediaVersion = "GB";
        private string _printVersion = "Regular";

        // Which games get which toggles
        private static readonly string[] SgbGames = { "Red", "Blue" };
        private static readonly string[] PrintGames = { "Yellow", "Gold", "Silver", "Crystal" };

        public DiplomaForm(string gameName, string playerName, bool missingMew)
        {
            InitializeComponent();

            _gameName = gameName;
            _playerName = playerName;
            _missingMew = missingMew;
            _assets = new AssetManager();
            _nameMgr = new DiplomaNameDisplayManager();

            // Wire up events
            radioGB.CheckedChanged += MediaVersion_CheckedChanged;
            radioSGB.CheckedChanged += MediaVersion_CheckedChanged;
            radioRegular.CheckedChanged += PrintVersion_CheckedChanged;
            radioPrinter.CheckedChanged += PrintVersion_CheckedChanged;
            txtHour.TextChanged += (_, __) => LoadAndComposeDiploma();
            txtMinute.TextChanged += (_, __) => LoadAndComposeDiploma();
            buttonSave.Click += buttonSave_Click;
            buttonClose.Click += buttonClose_Click;
        }

        private void DiplomaForm_Load(object sender, EventArgs e)
        {
            // 1) Show GB/SGB only for Red & Blue
            bool sgbOK = SgbGames.Contains(_gameName);
            radioGB.Visible = sgbOK;
            radioSGB.Visible = sgbOK;
            labelGameboy.Visible = sgbOK;
            if (sgbOK)
                radioGB.Checked = true;   // default GB

            // 2) Show Regular/Printer only for Yellow, Gold, Silver, Crystal
            bool printOK = PrintGames.Contains(_gameName);
            radioRegular.Visible = printOK;
            radioPrinter.Visible = printOK;
            labelDiploma.Visible = printOK;

            if (printOK)
            {
                radioRegular.Checked = true;
                _printVersion = "Regular";
            }

            ToggleHourMinuteControls();
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
            bool show = _printVersion == "Printer" && PrintGames.Contains(_gameName);
            lblHour.Visible = txtHour.Visible = show;
            lblMinute.Visible = txtMinute.Visible = show;
        }

        private void LoadAndComposeDiploma()
        {
            // If Yellow+Printer+missingMew, use the special “PrinterMissing” file
            var actualPrint = (_gameName == "Yellow"
                               && _printVersion == "Printer"
                               && _missingMew)
                              ? "PrinterMissingMew"
                              : _printVersion;

            // Compose filename: e.g. "Yellow-GB-PrinterMissing.png"
            var path = _assets.GetDiplomaPath(_gameName, _mediaVersion, actualPrint);
            if (!File.Exists(path))
            {
                MessageBox.Show($"Diploma not found: {path}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            using (var baseImg = Image.FromFile(path))
            {
                var bmp = new Bitmap(
                    baseImg.Width,
                    baseImg.Height,
                    PixelFormat.Format32bppArgb
                );

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
                            StringFormat.GenericDefault
                        );
                        g.FillPath(Brushes.Black, gp);
                    }

                    // restore for time / anti‐alias
                    g.CompositingMode = CompositingMode.SourceOver;
                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    // draw hour/minute if using any Printer variant
                    if (actualPrint.StartsWith("Printer") && PrintGames.Contains(_gameName))
                    {
                        var twoFs = _timeMgr.GetFontSettings(_gameName);

                        using (var fH = new Font(
                                   twoFs.HourSettings.Family,
                                   twoFs.HourSettings.Size,
                                   FontStyle.Regular,
                                   GraphicsUnit.Pixel))
                        {
                            g.DrawString(
                                txtHour.Text.PadLeft(2, '0'),
                                fH,
                                Brushes.Black,
                                twoFs.HourSettings.Location
                            );
                        }

                        using (var fM = new Font(
                                   twoFs.MinuteSettings.Family,
                                   twoFs.MinuteSettings.Size,
                                   FontStyle.Regular,
                                   GraphicsUnit.Pixel))
                        {
                            g.DrawString(
                                txtMinute.Text.PadLeft(2, '0'),
                                fM,
                                Brushes.Black,
                                twoFs.MinuteSettings.Location
                            );
                        }
                    }
                }

                pictureBox1.Image = bmp;
            }
        }

        public void SetPrinterDefault(bool printer)
        {
            if (radioPrinter.Visible)
            {
                radioPrinter.Checked = printer;
                radioRegular.Checked = !printer;
                _printVersion = printer ? "Printer" : "Regular";
                ToggleHourMinuteControls();
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
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    pictureBox1.Image.Save(dlg.FileName, ImageFormat.Png);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e) => Close();
    }
}
