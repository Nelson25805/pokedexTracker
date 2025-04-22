using System;
using System.Windows.Forms;

namespace PokedexTracker.Forms
{
    public partial class ChangeNameForm : Form
    {
        public string NewName => txtNewName.Text.Trim();

        public ChangeNameForm(string currentName)
        {
            InitializeComponent();

            // Prepopulate with the existing name
            txtNewName.Text = currentName;
            txtNewName.SelectAll();
            txtNewName.Focus();

            // Wire up OK/Cancel
            btnOk.Click += BtnOk_Click;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewName.Text))
            {
                MessageBox.Show(
                    "Name cannot be empty.",
                    "Invalid Name",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                txtNewName.Focus();
                return;
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}
