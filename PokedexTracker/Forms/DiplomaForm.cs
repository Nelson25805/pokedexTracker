using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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
        private readonly bool _missingMew;
        private readonly AssetManager _assets;
        private readonly DiplomaNameDisplayManager _nameMgr;
        private readonly DiplomaTimeDisplayManager _timeMgr;

        // Our single “choice” radio value:
        // – For Red/Blue:            GB or SGB
        // – For Yellow/Gold/Silver/Crystal:
        //      Regular or Printer (PrinterMissingMew if special)
        // – For all others:          Regional or National
        private string _choice;

        public DiplomaForm(
            string gameName,
            string playerName,
            bool missingMew)
        {
            InitializeComponent();

            _gameName = gameName;
            _playerName = playerName;
            _missingMew = missingMew;
            _assets = new AssetManager();
            _nameMgr = new DiplomaNameDisplayManager();
            _timeMgr = new DiplomaTimeDisplayManager();

            // Wire the two generic option radios:
            radioOption1.CheckedChanged += Option_CheckedChanged;
            radioOption2.CheckedChanged += Option_CheckedChanged;

            // Re‐draw whenever time text changes:
            txtHour.TextChanged += (_, __) => Compose();
            txtMinute.TextChanged += (_, __) => Compose();

            // Only digits in time fields:
            txtHour.KeyPress += NumericOnly_KeyPress;
            txtMinute.KeyPress += NumericOnly_KeyPress;

            // Save & Close:
            buttonSave.Click += buttonSave_Click;
            buttonClose.Click += (_, __) => Close();
        }

        private void DiplomaForm_Load(object sender, EventArgs e)
        {
            bool isGBmode = (_gameName == "Red" || _gameName == "Blue");
            bool isPrintMode = (_gameName == "Yellow" ||
                                _gameName == "Gold" ||
                                _gameName == "Silver" ||
                                _gameName == "Crystal");

            if (isGBmode)
            {
                lblOption.Text = "Choose Version:";
                radioOption1.Text = "GB";
                radioOption2.Text = "SGB";
            }
            else if (isPrintMode)
            {
                lblOption.Text = "Choose Diploma:";
                radioOption1.Text = "Regular";
                radioOption2.Text = "Printer";
            }
            else
            {
                lblOption.Text = "Choose Pokédex:";
                radioOption1.Text = "Regional";
                radioOption2.Text = "National";
            }

            // Default select _first_ radio:
            radioOption1.Checked = true;
            _choice = radioOption1.Text;

            ToggleTime();
            Compose();
        }

        private void Option_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton r = sender as RadioButton;
            if (r == null || !r.Checked) return;

            _choice = r.Text;    // e.g. "SGB", "Printer", "National", etc.
            ToggleTime();
            Compose();
        }

        private void ToggleTime()
        {
            // Show time fields only when “Printer” selected
            bool show = (_choice == "Printer");
            lblHour.Visible =
            txtHour.Visible =
            lblMinute.Visible =
            txtMinute.Visible = show;
        }

        /// <summary>
        /// Builds the correct path + redraws the diploma image.
        /// </summary>
        private void Compose()
        {
            // 1) If Yellow & Printer with missing Mew → special
            string choiceKey = _choice;
            if (_gameName == "Yellow"
                && _choice == "Printer"
                && _missingMew)
            {
                choiceKey = "PrinterMissingMew";
            }

            // 2) Get the full path from AssetManager
            string path = _assets.GetDiplomaPath(_gameName, choiceKey);

            if (!File.Exists(path))
            {
                MessageBox.Show(
                    "Diploma missing:\n" + path,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Close();
                return;
            }

            // 3) Load & compose
            Image baseImg = Image.FromFile(path);
            Bitmap bmp = new Bitmap(
                baseImg.Width,
                baseImg.Height,
                PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);

            // Draw background
            g.PageUnit = GraphicsUnit.Pixel;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.SmoothingMode = SmoothingMode.None;
            g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            g.DrawImage(baseImg, Point.Empty);

            // 1) fetch new settings with color:
            var nf = _nameMgr.GetFontSettings(_gameName);

            // 2) prepare for overwrite:
            g.CompositingMode = CompositingMode.SourceCopy;
            g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            g.SmoothingMode = SmoothingMode.None;

            using (var gp = new GraphicsPath())
            {
                gp.AddString(
                    _playerName,
                    nf.Family,
                    (int)FontStyle.Regular,
                    nf.Size,
                    new PointF(nf.Location.X, nf.Location.Y),
                    StringFormat.GenericDefault
                );

                // optional white halo:
                //using (var halo = new Pen(Color.White, 3) { LineJoin = LineJoin.Round })
                //    g.DrawPath(halo, gp);

                // 3) fill text with our per‐game color:
                using (var brush = new SolidBrush(nf.TextColor))
                    g.FillPath(brush, gp);
            }

            // 4) restore for any further AA drawing
            g.CompositingMode = CompositingMode.SourceOver;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            g.SmoothingMode = SmoothingMode.AntiAlias;


            // If Printer variant, draw time
            if (choiceKey.StartsWith("Printer"))
            {
                var tf = _timeMgr.GetFontSettings(_gameName);

                // Hours, right-aligned
                using (Font fH = new Font(
                           tf.HourSettings.Family,
                           tf.HourSettings.Size,
                           FontStyle.Regular,
                           GraphicsUnit.Pixel))
                {
                    string h = txtHour.Text.PadLeft(2, '0');
                    SizeF sz = g.MeasureString(h, fH);
                    float x = tf.HourSettings.Location.X - sz.Width;
                    float y = tf.HourSettings.Location.Y;
                    g.DrawString(h, fH, Brushes.Black, new PointF(x, y));
                }

                // Minutes, left-aligned
                using (Font fM = new Font(
                           tf.MinuteSettings.Family,
                           tf.MinuteSettings.Size,
                           FontStyle.Regular,
                           GraphicsUnit.Pixel))
                {
                    string m = txtMinute.Text.PadLeft(2, '0');
                    g.DrawString(m, fM, Brushes.Black, tf.MinuteSettings.Location);
                }
            }

            pictureBox1.Image = (Bitmap)bmp.Clone();

            // Clean up
            g.Dispose();
            bmp.Dispose();
            baseImg.Dispose();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
                return;

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "PNG Image|*.png";
            dlg.FileName = $"{_gameName}_Diploma_{_playerName}.png";
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                pictureBox1.Image.Save(
                    dlg.FileName,
                    ImageFormat.Png);
            }
        }

        private void NumericOnly_KeyPress(
            object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
