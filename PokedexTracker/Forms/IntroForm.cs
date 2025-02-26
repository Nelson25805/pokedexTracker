using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PokedexTracker.Helpers;
using PokedexTracker.Managers;

namespace PokedexTracker.Forms
{
    public partial class IntroForm : Form
    {
        private readonly AssetManager _assetManager;
        private readonly GenerationManager _generationManager;
        private PokemonGeneration _selectedGeneration;
        private SpeechManager _speechManager;
        private string currentText = "";
        private int textCharIndex = 0;
        private string playerName = "Trainer";
        private bool isErrorMessage = false;

        // Variables for fade effect.
        private float imageOpacity = 0f;
        private const float fadeStep = 0.05f;
        private Image originalProfessorImage;

        public IntroForm()
        {
            InitializeComponent();

            // Initialize managers.
            _assetManager = new AssetManager();
            _generationManager = new GenerationManager(_assetManager);

            // Automatically select Generation 1 since it's the only option.
            _selectedGeneration = _generationManager.GetGeneration("Generation 1");

            HideAllContent();

            // Hook up button events.
            submitButton.Click += SubmitButton_Click;
            skipIntroButton.Click += SkipIntroButton_Click;
            advanceButton.Click += AdvanceButton_Click;
        }

        // Hide all content initially.
        private void HideAllContent()
        {
            professorLabel.Visible = false;
            professorPictureBox.Visible = false;
            advanceButton.Visible = false;
            nameTextBox.Visible = false;
            submitButton.Visible = false;
            skipIntroButton.Visible = false;
        }

        // On form load, immediately start the intro.
        private void IntroForm_Load(object sender, EventArgs e)
        {
            StartIntro();
        }

        // Begin the intro sequence.
        private void StartIntro()
        {
            if (_selectedGeneration == null)
            {
                MessageBox.Show("Generation 1 is not available.");
                return;
            }

            ShowIntroContent();

            // Initialize SpeechManager with the single speech option.
            _speechManager = new SpeechManager(_selectedGeneration.Speeches);
            currentText = _speechManager.CurrentSpeech;
            professorLabel.Text = "";
            textCharIndex = 0;
            advanceButton.Visible = false;
            typingTimer.Start();
        }

        // Make the professor and skip button visible.
        private void ShowIntroContent()
        {
            professorLabel.Visible = true;
            professorPictureBox.Visible = true;
            skipIntroButton.Visible = true;
        }

        // Timer tick: display text with a typing effect.
        private void typingTimer_Tick(object sender, EventArgs e)
        {
            if (textCharIndex < currentText.Length)
            {
                professorLabel.Text += currentText[textCharIndex];
                textCharIndex++;
            }
            else
            {
                typingTimer.Stop();
                if (!isErrorMessage)
                {
                    advanceButton.Visible = true;
                    // For example, when the professor asks for the player's name (speech index 3).
                    if (_speechManager.CurrentIndex == 3)
                    {
                        nameTextBox.Visible = true;
                        submitButton.Visible = true;
                        nameTextBox.Focus();
                        advanceButton.Visible = false;
                    }
                }
                else
                {
                    // Reset the error flag so the user can try again.
                    isErrorMessage = false;
                }
            }
        }

        // Advance to the next part of the speech.
        private void AdvanceButton_Click(object sender, EventArgs e)
        {
            _speechManager.Advance();
            if (_speechManager.CurrentIndex < _selectedGeneration.Speeches.Length)
            {
                // Replace any placeholder with the player's name.
                currentText = _selectedGeneration.Speeches[_speechManager.CurrentIndex].Replace("{playerName}", playerName);
                professorLabel.Text = "";
                textCharIndex = 0;
                advanceButton.Visible = false;

                if (_speechManager.CurrentIndex == 1)
                {
                    UpdateProfessorImage();
                }
                if (_speechManager.CurrentIndex == 4)
                {
                    nameTextBox.Visible = false;
                    submitButton.Visible = false;
                    // Retrieve the stored player name.
                    playerName = Properties.Settings.Default.playerName;
                    currentText = currentText.Replace("{playerName}", playerName);
                }

                typingTimer.Start();
            }
            else
            {
                NavigateToMainForm();
            }
        }

        // Validate the player's name and advance if valid.
        private void SubmitButton_Click(object sender, EventArgs e)
        {
            string enteredName = nameTextBox.Text.Trim();

            if (enteredName.Length < 1)
            {
                isErrorMessage = true;
                currentText = "Your name must be at least 1 character long. Please try again.";
                professorLabel.Text = "";
                textCharIndex = 0;
                typingTimer.Start();
                nameTextBox.Focus();
                return;
            }
            else if (enteredName.Length > 7)
            {
                isErrorMessage = true;
                currentText = "Your name cannot be more than 7 characters long. Please try again.";
                professorLabel.Text = "";
                textCharIndex = 0;
                typingTimer.Start();
                nameTextBox.Focus();
                return;
            }

            playerName = enteredName;
            Properties.Settings.Default.playerName = playerName;
            Properties.Settings.Default.Save();
            AdvanceButton_Click(sender, e);
        }

        // Load and fade in the professor image.
        private void UpdateProfessorImage()
        {
            if (_selectedGeneration != null && professorPictureBox.Image == null)
            {
                string imagePath = _selectedGeneration.ProfessorImages.FirstOrDefault();
                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    originalProfessorImage = Image.FromFile(imagePath);
                    professorPictureBox.Image = ImageFader.AdjustImageOpacity(originalProfessorImage, 0f);
                    professorPictureBox.Visible = true;
                    imageOpacity = 0f;
                    fadeTimer.Start();
                }
                else
                {
                    professorPictureBox.Image = null;
                    MessageBox.Show("Professor image not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Skip the intro and navigate to the main form.
        private void SkipIntroButton_Click(object sender, EventArgs e)
        {
            playerName = "Trainer";
            Properties.Settings.Default.playerName = playerName;
            Properties.Settings.Default.Save();
            NavigateToMainForm();
        }

        // Open the main form.
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

        // Fade timer tick: gradually increase the professor image opacity.
        private void fadeTimer_Tick(object sender, EventArgs e)
        {
            if (imageOpacity < 1f)
            {
                imageOpacity += fadeStep;
                professorPictureBox.Image = ImageFader.AdjustImageOpacity(originalProfessorImage, imageOpacity);
            }
            else
            {
                fadeTimer.Stop();
            }
        }
    }
}
