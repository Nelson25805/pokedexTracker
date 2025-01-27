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
            InitializeIntroUI();
        }

        private void InitializeIntroUI()
        {
            // Set up intro controls
            var titleLabel = new Label()
            {
                Text = "Welcome to the World of Pokémon!",
                Font = new System.Drawing.Font("Arial", 24, System.Drawing.FontStyle.Bold),
                AutoSize = true,
                Location = new System.Drawing.Point(50, 50),
            };

            var professorLabel = new Label()
            {
                Text = "Hello! I am the Pokémon Professor. What is your name?",
                AutoSize = true,
                Location = new System.Drawing.Point(100, 150),
            };

            var nameTextBox = new TextBox()
            {
                Width = 200,
                Location = new System.Drawing.Point(100, 250),
            };

            var submitButton = new Button()
            {
                Text = "Submit",
                Location = new System.Drawing.Point(100, 300),
            };

            var skipButton = new Button()
            {
                Text = "Skip Intro",
                Location = new System.Drawing.Point(100, 350),
            };

            // Add controls to the form
            this.Controls.Add(titleLabel);
            this.Controls.Add(professorLabel);
            this.Controls.Add(nameTextBox);
            this.Controls.Add(submitButton);
            this.Controls.Add(skipButton);

            // Attach event handler to the "Submit" button
            submitButton.Click += (s, e) =>
            {
                playerName = nameTextBox.Text.Trim();

                if (string.IsNullOrEmpty(playerName))
                {
                    playerName = "Trainer"; // Default name
                    professorLabel.Text = "You haven't entered a name.\nWe'll go with Trainer as your name!";
                }
                else
                {
                    professorLabel.Text = $"Ah, {playerName}, what a great name!\nWelcome to the Pokémon world!";
                }

                // Save the name to settings
                Properties.Settings.Default.playerName = playerName;
                Properties.Settings.Default.Save();

                // Delay and navigate to MainForm
                Timer timer = new Timer();
                timer.Interval = 3000; // Wait for 3 seconds
                timer.Tick += (timerSender, timerArgs) =>
                {
                    timer.Stop();
                    NavigateToMainForm();
                };
                timer.Start();

                // Disable input fields after submission
                nameTextBox.Enabled = false;
                submitButton.Enabled = false;
            };

            // Attach event handler to the "Skip Intro" button
            skipButton.Click += (s, e) =>
            {
                playerName = "Trainer"; // Default name when skipping intro
                Properties.Settings.Default.playerName = playerName;
                Properties.Settings.Default.Save(); // Save name to settings

                // Navigate directly to MainForm
                NavigateToMainForm();
            };
        }

        private void NavigateToMainForm()
        {
            this.Hide();
            MainForm mainForm = new MainForm(playerName); // Pass the name to MainForm
            mainForm.Show();
        }
    }
}
