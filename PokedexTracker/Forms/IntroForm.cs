using System;
using System.Linq;
using System.Windows.Forms;

namespace PokedexTracker.Forms
{
    public partial class IntroForm : Form
    {
        private readonly AssetManager _assetManager;
        private GenerationManager _generationManager;
        private PokemonGeneration _selectedGeneration;
        private int currentSpeechIndex = 0; // Declare currentSpeechIndex here
        private int len = 0;
        private string currentText = "";
        private string playerName = "Trainer";
        private string[] speechParts;
        private int currentSelectionIndex = 0;
        private Label arrowLabel; // Declare at class level

        private float imageOpacity = 0f; // Current opacity of the image
        private const float fadeStep = 0.05f; // How much to increase the opacity on each tick

        private System.Drawing.Image originalProfessorImage;



        public IntroForm()
        {
            InitializeComponent();

            _assetManager = new AssetManager();
            _generationManager = new GenerationManager(_assetManager); // Pass AssetManager

            HideAllContent();
            InitGenerationMenu();

            submitButton.Click += SubmitButton_Click;
            skipIntroButton.Click += SkipIntroButton_Click;
            advanceButton.Click += AdvanceButton_Click;
        }



        // Helper method to hide all non-menu elements
        private void HideAllContent()
        {
            professorLabel.Visible = false;
            professorPictureBox.Visible = false;
            advanceButton.Visible = false;
            nameTextBox.Visible = false;
            submitButton.Visible = false;
            skipIntroButton.Visible = false;
        }

        private void InitGenerationMenu()
        {
            var generationNames = _generationManager.GetAvailableGenerations();
            int yPosition = 20; // Start y position for menu items

            // Add labels for each generation
            foreach (var generationName in generationNames)
            {
                Label generationLabel = new Label
                {
                    Text = generationName,
                    Location = new System.Drawing.Point(20, yPosition),
                    AutoSize = true,
                    ForeColor = System.Drawing.Color.Black,
                    Tag = yPosition // Store the Y position in the Tag
                };
                generationMenuPanel.Controls.Add(generationLabel);
                yPosition += 30; // Add some spacing between the menu items
            }

            // Create and position the arrow
            arrowLabel = new Label
            {
                Text = "▶", // Arrow symbol
                Location = new System.Drawing.Point(0, 20), // Initial position of arrow
                AutoSize = true,
                ForeColor = System.Drawing.Color.Black
            };
            generationMenuPanel.Controls.Add(arrowLabel);
        }

        // Key press handling for moving the arrow up and down
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down)
            {
                MoveArrow(keyData);
                return true;
            }
            else if (keyData == Keys.Enter) // User confirms selection
            {
                SubmitSelection();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SubmitSelection()
        {
            var generationLabels = generationMenuPanel.Controls.OfType<Label>()
                                      .Where(lbl => lbl != arrowLabel) // Exclude the arrow itself
                                      .ToList();

            if (generationLabels.Count == 0) return; // Prevent errors

            // Get the selected generation's name
            string selectedGeneration = generationLabels[currentSelectionIndex].Text;
            _selectedGeneration = _generationManager.GetGeneration(selectedGeneration);

            // Hide the menu and start the intro
            StartIntro();
        }



        private void MoveArrow(Keys direction)
        {
            var generationLabels = generationMenuPanel.Controls.OfType<Label>()
                                      .Where(lbl => lbl != arrowLabel) // Exclude the arrow itself
                                      .ToList();

            if (generationLabels.Count == 0) return; // Prevent errors if no labels exist

            // Move the selection index up or down
            if (direction == Keys.Up)
            {
                currentSelectionIndex = (currentSelectionIndex > 0) ? currentSelectionIndex - 1 : generationLabels.Count - 1;
            }
            else if (direction == Keys.Down)
            {
                currentSelectionIndex = (currentSelectionIndex < generationLabels.Count - 1) ? currentSelectionIndex + 1 : 0;
            }

            // Move the arrow to the selected label's position
            arrowLabel.Location = new System.Drawing.Point(0, (int)generationLabels[currentSelectionIndex].Tag);
        }


        private void SubmitButton_Click(object sender, EventArgs e)
        {
            // Set the player's name and save it
            playerName = nameTextBox.Text;
            Properties.Settings.Default.playerName = playerName;
            Properties.Settings.Default.Save();
            AdvanceButton_Click(sender, e);
        }

        private void StartIntro()
        {
            if (_selectedGeneration == null)
            {
                MessageBox.Show("Please select a generation before proceeding.");
                return; // Do NOT proceed until the user selects a generation
            }

            // Hide the selection menu
            generationMenuPanel.Visible = false;

            // Show the actual intro elements
            ShowIntroContent();

            speechParts = _selectedGeneration.Speeches; // Get the extended speech parts
            currentSpeechIndex = 0; // Reset speech index
            currentText = speechParts[currentSpeechIndex];
            professorLabel.Text = "";
            len = 0; // Reset text length
            advanceButton.Visible = false; // Ensure it's hidden before speech starts
            timer1.Start();
        }



        // Helper method to show all intro elements
        private void ShowIntroContent()
        {
            professorLabel.Visible = true;
            professorPictureBox.Visible = true;
            skipIntroButton.Visible = true;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (len < currentText.Length)
            {
                professorLabel.Text += currentText.ElementAt(len);
                len++;
            }
            else
            {
                timer1.Stop(); // Stop the timer once the text is fully shown
                advanceButton.Visible = true; // Show the "Advance" button when speech finishes
                if (currentSpeechIndex == 3)
                {
                    nameTextBox.Visible = true;
                    submitButton.Visible = true;
                    nameTextBox.Focus();
                    advanceButton.Visible = false;
                }
            }
        }


        private void AdvanceButton_Click(object sender, EventArgs e)
        {
            currentSpeechIndex++;

            if (currentSpeechIndex < speechParts.Length)
            {
                // Replace {playerName} before displaying
                currentText = speechParts[currentSpeechIndex].Replace("{playerName}", playerName);
                professorLabel.Text = "";
                len = 0;
                advanceButton.Visible = false;

                if (currentSpeechIndex == 1)
                {
                    UpdateProfessorImage();
                }
                if (currentSpeechIndex == 4)
                {
                    nameTextBox.Visible = false;
                    submitButton.Visible = false;
                    // Retrieve player name from settings
                    playerName = Properties.Settings.Default.playerName;

                    // Replace "{playerName}" dynamically in the speech
                    speechParts = _selectedGeneration.Speeches.Select(s => s.Replace("{playerName}", playerName)).ToArray();
                }

                timer1.Start();
            }
            else
            {
                NavigateToMainForm();
            }
        }



        private void UpdateProfessorImage()
        {
            if (_selectedGeneration != null && professorPictureBox.Image == null)
            {
                string imagePath = _selectedGeneration.ProfessorImages.FirstOrDefault(); // Use the first image only

                if (!string.IsNullOrEmpty(imagePath) && System.IO.File.Exists(imagePath))
                {
                    originalProfessorImage = System.Drawing.Image.FromFile(imagePath); // Store original image
                    professorPictureBox.Image = AdjustImageOpacity((System.Drawing.Bitmap)originalProfessorImage, 0f);
                    professorPictureBox.Visible = true;
                    imageOpacity = 0f; // Reset opacity
                    fadeTimer.Start(); // Start fade effect
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Professor image not found.");
                    professorPictureBox.Image = null;
                    MessageBox.Show($"Professor image not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void SkipIntroButton_Click(object sender, EventArgs e)
        {
            playerName = "Trainer";
            Properties.Settings.Default.playerName = playerName;
            Properties.Settings.Default.Save();
            NavigateToMainForm();
        }

        private void NavigateToMainForm()
        {
            this.Hide();
            MainForm mainForm = new MainForm(playerName);
            mainForm.Show();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }

        private void fadeTimer_Tick(object sender, EventArgs e)
        {
            if (imageOpacity < 1f)
            {
                imageOpacity += fadeStep;  // Increase opacity
                professorPictureBox.Image = AdjustImageOpacity((System.Drawing.Bitmap)originalProfessorImage, imageOpacity);
                professorPictureBox.Refresh(); // Force UI update
            }
            else
            {
                fadeTimer.Stop();
                System.Diagnostics.Debug.WriteLine("Fade complete. Stopping timer.");
            }
        }

        private System.Drawing.Image AdjustImageOpacity(System.Drawing.Image image, float opacity)
        {
            if (image == null) return null;

            var bmp = new System.Drawing.Bitmap(image.Width, image.Height);
            using (var graphics = System.Drawing.Graphics.FromImage(bmp))
            {
                var matrix = new System.Drawing.Imaging.ColorMatrix
                {
                    Matrix33 = opacity
                };

                var attributes = new System.Drawing.Imaging.ImageAttributes();
                attributes.SetColorMatrix(matrix, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);

                graphics.DrawImage(image, new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, System.Drawing.GraphicsUnit.Pixel, attributes);
            }
            return bmp;
        }
    }
}
