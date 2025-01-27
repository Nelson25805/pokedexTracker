using System;
using System.Windows.Forms;

namespace PokedexTracker.Forms
{
    public partial class IntroForm : Form
    {
        private string playerName = "Trainer"; // Default name

        public IntroForm()
        {
            InitializeComponent();

            // Attach event handlers
            submitButton.Click += SubmitButton_Click;
            skipIntroButton.Click += SkipIntroButton_Click;

            // Load the saved name if it exists
            string savedName = Properties.Settings.Default.playerName;
            if (!string.IsNullOrWhiteSpace(savedName))
            {
                playerName = savedName;
                NavigateToMainForm();
            }
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            playerName = nameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(playerName))
            {
                playerName = "Trainer"; // Use default name
                professorLabel.Text = "You haven't entered a name.\nWe'll go with Trainer as your name!";
            }
            else
            {
                professorLabel.Text = $"Ah, {playerName}, what a great name!\nWelcome to the Pokémon world!";
            }

            // Save the name to settings
            Properties.Settings.Default.playerName = playerName;
            Properties.Settings.Default.Save();

            // Delay navigation to MainForm
            Timer timer = new Timer();
            timer.Interval = 3000; // 3-second delay
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                NavigateToMainForm();
            };
            timer.Start();

            // Disable input fields
            nameTextBox.Enabled = false;
            submitButton.Enabled = false;
            skipIntroButton.Enabled = false;
        }

        private void SkipIntroButton_Click(object sender, EventArgs e)
        {
            // Set default name and save
            playerName = "Trainer";
            Properties.Settings.Default.playerName = playerName;
            Properties.Settings.Default.Save();

            // Navigate to MainForm immediately
            NavigateToMainForm();
        }

        private void NavigateToMainForm()
        {
            this.Hide();
            MainForm mainForm = new MainForm(playerName); // Pass player's name to MainForm
            mainForm.Show();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit(); // Ensure full application termination
        }

    }
}
