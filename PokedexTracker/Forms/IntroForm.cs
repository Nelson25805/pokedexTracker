using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PokedexTracker.Controls;
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

        // UI Elements for fade effect
        private float imageOpacity = 0f;
        private const float fadeStep = 0.05f;
        private Image originalProfessorImage;

        // The refactored generation menu controller
        private GenerationMenu generationMenu;

        public IntroForm()
        {
            InitializeComponent();

            _assetManager = new AssetManager();
            _generationManager = new GenerationManager(_assetManager);

            HideAllContent();
            InitGenerationMenu();

            submitButton.Click += SubmitButton_Click;
            skipIntroButton.Click += SkipIntroButton_Click;
            advanceButton.Click += AdvanceButton_Click;
        }

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
            generationMenu = new GenerationMenu(generationMenuPanel, generationNames);
        }

        // Override keyboard handling to move arrow and confirm selection.
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up)
            {
                generationMenu.MoveUp();
                return true;
            }
            else if (keyData == Keys.Down)
            {
                generationMenu.MoveDown();
                return true;
            }
            else if (keyData == Keys.Enter)
            {
                SubmitSelection();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SubmitSelection()
        {
            string selectedName = generationMenu.GetSelectedGenerationName();
            _selectedGeneration = _generationManager.GetGeneration(selectedName);
            StartIntro();
        }

        private void StartIntro()
        {
            if (_selectedGeneration == null)
            {
                MessageBox.Show("Please select a generation before proceeding.");
                return;
            }

            generationMenuPanel.Visible = false;
            ShowIntroContent();

            // Initialize the SpeechManager with speeches from the selected generation.
            _speechManager = new SpeechManager(_selectedGeneration.Speeches);
            currentText = _speechManager.CurrentSpeech;
            professorLabel.Text = "";
            textCharIndex = 0;
            advanceButton.Visible = false;
            timer1.Start();
        }

        private void ShowIntroContent()
        {
            professorLabel.Visible = true;
            professorPictureBox.Visible = true;
            skipIntroButton.Visible = true;
        }

        // Timer tick for “typing” effect.
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (textCharIndex < currentText.Length)
            {
                professorLabel.Text += currentText[textCharIndex];
                textCharIndex++;
            }
            else
            {
                timer1.Stop();
                advanceButton.Visible = true;
                // For example, if you want to reveal name input on a specific speech index:
                if (_speechManager.CurrentIndex == 3)
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
            _speechManager.Advance();
            if (_speechManager.CurrentIndex < _selectedGeneration.Speeches.Length)
            {
                // Replace any placeholder with the player's name
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
                    // Retrieve player name from settings and update speeches if needed.
                    playerName = Properties.Settings.Default.playerName;
                    currentText = currentText.Replace("{playerName}", playerName);
                }

                timer1.Start();
            }
            else
            {
                NavigateToMainForm();
            }
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            playerName = nameTextBox.Text;
            Properties.Settings.Default.playerName = playerName;
            Properties.Settings.Default.Save();
            AdvanceButton_Click(sender, e);
        }

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

        // Timer tick for fading in the professor image.
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
