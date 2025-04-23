namespace PokedexTracker
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ComboBox comboBoxGames;
        private System.Windows.Forms.Panel panelCards;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.PictureBox trainerCard;
        private System.Windows.Forms.Label lblPlayerName;
        private System.Windows.Forms.CheckBox chkShiny;
        // Only the custom PokeballRadioButton controls are used.
        private PokeballRadioButton pokeballRadioButtonBoy;
        private PokeballRadioButton pokeballRadioButtonGirl;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.comboBoxGames = new System.Windows.Forms.ComboBox();
            this.panelCards = new System.Windows.Forms.Panel();
            this.lblProgress = new System.Windows.Forms.Label();
            this.trainerCard = new System.Windows.Forms.PictureBox();
            this.lblPlayerName = new System.Windows.Forms.Label();
            this.chkShiny = new System.Windows.Forms.CheckBox();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.diplomaButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.btnChangeName = new System.Windows.Forms.Button();
            this.pokeballRadioButtonGirl = new PokeballRadioButton();
            this.pokeballRadioButtonBoy = new PokeballRadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.trainerCard)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxGames
            // 
            this.comboBoxGames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGames.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxGames.FormattingEnabled = true;
            this.comboBoxGames.Items.AddRange(new object[] {
            "Red",
            "Blue",
            "Yellow",
            "Silver",
            "Gold",
            "Crystal",
            "Ruby",
            "Sapphire",
            "Emerald",
            "Fire Red",
            "Leaf Green",
            "Diamond",
            "Pearl",
            "Platinum",
            "Heart Gold",
            "Soul Silver",
            "Black",
            "White",
            "Black 2",
            "White 2",
            "X",
            "Y",
            "Omega Ruby",
            "Alpha Sapphire",
            "Sun",
            "Moon",
            "Ultra Sun",
            "Ultra Moon",
            "Let\'s Go Pikachu!",
            "Let\'s Go Eevee!",
            "Sword",
            "Shield",
            "Brilliant Diamond",
            "Shining Pearl",
            "Legends Arceus",
            "Scarlet",
            "Violet",
            "Legends: Z-A"});
            this.comboBoxGames.Location = new System.Drawing.Point(19, 510);
            this.comboBoxGames.Name = "comboBoxGames";
            this.comboBoxGames.Size = new System.Drawing.Size(487, 39);
            this.comboBoxGames.TabIndex = 0;
            this.comboBoxGames.SelectedIndexChanged += new System.EventHandler(this.comboBoxGames_SelectedIndexChanged);
            // 
            // panelCards
            // 
            this.panelCards.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCards.AutoScroll = true;
            this.panelCards.Location = new System.Drawing.Point(716, 8);
            this.panelCards.Name = "panelCards";
            this.panelCards.Size = new System.Drawing.Size(1154, 1363);
            this.panelCards.TabIndex = 1;
            this.panelCards.Resize += new System.EventHandler(this.panelCards_Resize);
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.BackColor = System.Drawing.SystemColors.Window;
            this.lblProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.Location = new System.Drawing.Point(341, 118);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(73, 32);
            this.lblProgress.TabIndex = 2;
            this.lblProgress.Text = "0 / 0";
            this.lblProgress.Visible = false;
            // 
            // trainerCard
            // 
            this.trainerCard.InitialImage = ((System.Drawing.Image)(resources.GetObject("trainerCard.InitialImage")));
            this.trainerCard.Location = new System.Drawing.Point(12, 12);
            this.trainerCard.Name = "trainerCard";
            this.trainerCard.Size = new System.Drawing.Size(698, 417);
            this.trainerCard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.trainerCard.TabIndex = 3;
            this.trainerCard.TabStop = false;
            this.trainerCard.Visible = false;
            // 
            // lblPlayerName
            // 
            this.lblPlayerName.AutoSize = true;
            this.lblPlayerName.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayerName.Location = new System.Drawing.Point(276, 57);
            this.lblPlayerName.Name = "lblPlayerName";
            this.lblPlayerName.Size = new System.Drawing.Size(0, 32);
            this.lblPlayerName.TabIndex = 0;
            // 
            // chkShiny
            // 
            this.chkShiny.AutoSize = true;
            this.chkShiny.Enabled = false;
            this.chkShiny.Location = new System.Drawing.Point(19, 721);
            this.chkShiny.Name = "chkShiny";
            this.chkShiny.Size = new System.Drawing.Size(124, 36);
            this.chkShiny.TabIndex = 4;
            this.chkShiny.Text = "Shiny";
            this.chkShiny.UseVisualStyleBackColor = true;
            this.chkShiny.CheckedChanged += new System.EventHandler(this.chkShiny_CheckedChanged);
            // 
            // searchTextBox
            // 
            this.searchTextBox.Location = new System.Drawing.Point(19, 859);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(382, 38);
            this.searchTextBox.TabIndex = 7;
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 586);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 32);
            this.label1.TabIndex = 8;
            this.label1.Text = "Chose Gender:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 463);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(308, 32);
            this.label2.TabIndex = 9;
            this.label2.Text = "Choose Game Version:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 803);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(239, 32);
            this.label3.TabIndex = 10;
            this.label3.Text = "Search Pokemon:";
            // 
            // diplomaButton
            // 
            this.diplomaButton.Enabled = false;
            this.diplomaButton.Location = new System.Drawing.Point(19, 942);
            this.diplomaButton.Name = "diplomaButton";
            this.diplomaButton.Size = new System.Drawing.Size(322, 61);
            this.diplomaButton.TabIndex = 11;
            this.diplomaButton.Text = "Diploma";
            this.diplomaButton.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1876, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // btnChangeName
            // 
            this.btnChangeName.Location = new System.Drawing.Point(19, 1033);
            this.btnChangeName.Name = "btnChangeName";
            this.btnChangeName.Size = new System.Drawing.Size(322, 61);
            this.btnChangeName.TabIndex = 13;
            this.btnChangeName.Text = "Change Name";
            this.btnChangeName.UseVisualStyleBackColor = true;
            this.btnChangeName.Click += new System.EventHandler(this.btnChangeName_Click);
            // 
            // pokeballRadioButtonGirl
            // 
            this.pokeballRadioButtonGirl.CheckedImage = null;
            this.pokeballRadioButtonGirl.Location = new System.Drawing.Point(203, 632);
            this.pokeballRadioButtonGirl.Name = "pokeballRadioButtonGirl";
            this.pokeballRadioButtonGirl.Size = new System.Drawing.Size(175, 50);
            this.pokeballRadioButtonGirl.TabIndex = 6;
            this.pokeballRadioButtonGirl.TabStop = true;
            this.pokeballRadioButtonGirl.Text = "Girl";
            this.pokeballRadioButtonGirl.UncheckedImage = null;
            this.pokeballRadioButtonGirl.UseVisualStyleBackColor = true;
            this.pokeballRadioButtonGirl.CheckedChanged += new System.EventHandler(this.rdoGender_CheckedChanged);
            // 
            // pokeballRadioButtonBoy
            // 
            this.pokeballRadioButtonBoy.CheckedImage = null;
            this.pokeballRadioButtonBoy.Location = new System.Drawing.Point(3, 632);
            this.pokeballRadioButtonBoy.Name = "pokeballRadioButtonBoy";
            this.pokeballRadioButtonBoy.Size = new System.Drawing.Size(180, 50);
            this.pokeballRadioButtonBoy.TabIndex = 5;
            this.pokeballRadioButtonBoy.TabStop = true;
            this.pokeballRadioButtonBoy.Text = "Boy";
            this.pokeballRadioButtonBoy.UncheckedImage = null;
            this.pokeballRadioButtonBoy.UseVisualStyleBackColor = true;
            this.pokeballRadioButtonBoy.CheckedChanged += new System.EventHandler(this.rdoGender_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1876, 1379);
            this.Controls.Add(this.btnChangeName);
            this.Controls.Add(this.diplomaButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.pokeballRadioButtonGirl);
            this.Controls.Add(this.pokeballRadioButtonBoy);
            this.Controls.Add(this.chkShiny);
            this.Controls.Add(this.lblPlayerName);
            this.Controls.Add(this.trainerCard);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.panelCards);
            this.Controls.Add(this.comboBoxGames);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pokédex Tracker";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trainerCard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button diplomaButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Button btnChangeName;
    }
}
