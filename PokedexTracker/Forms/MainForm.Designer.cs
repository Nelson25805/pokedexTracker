namespace PokedexTracker
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Remove these game button declarations if no longer needed:
        // private System.Windows.Forms.Button btnRed;
        // private System.Windows.Forms.Button btnBlue;
        // private System.Windows.Forms.Button btnYellow;
        // private System.Windows.Forms.Button btnSilver;
        // private System.Windows.Forms.Button btnGold;
        // private System.Windows.Forms.Button btnCrystal;

        // Instead, add a ComboBox for game selection:
        private System.Windows.Forms.ComboBox comboBoxGames;

        private System.Windows.Forms.Panel panelCards;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.PictureBox trainerCard;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.rdoGirl = new System.Windows.Forms.RadioButton();
            this.rdoBoy = new System.Windows.Forms.RadioButton();
            this.chkShiny = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trainerCard)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxGames
            // 
            this.comboBoxGames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGames.Font = new System.Drawing.Font("PKMN RBYGSC", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            "FireRed",
            "LeadGreen",
            "Diamond",
            "Pearl",
            "Platinum",
            "HeartGold",
            "SoulSilver",
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
            this.comboBoxGames.Location = new System.Drawing.Point(12, 459);
            this.comboBoxGames.Name = "comboBoxGames";
            this.comboBoxGames.Size = new System.Drawing.Size(487, 38);
            this.comboBoxGames.TabIndex = 0;
            this.comboBoxGames.SelectedIndexChanged += new System.EventHandler(this.comboBoxGames_SelectedIndexChanged);
            // 
            // panelCards
            // 
            this.panelCards.AutoScroll = true;
            this.panelCards.Location = new System.Drawing.Point(716, 12);
            this.panelCards.Name = "panelCards";
            this.panelCards.Size = new System.Drawing.Size(1108, 1329);
            this.panelCards.TabIndex = 1;
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
            // rdoGirl
            // 
            this.rdoGirl.AutoSize = true;
            this.rdoGirl.Location = new System.Drawing.Point(134, 560);
            this.rdoGirl.Name = "rdoGirl";
            this.rdoGirl.Size = new System.Drawing.Size(96, 36);
            this.rdoGirl.TabIndex = 4;
            this.rdoGirl.TabStop = true;
            this.rdoGirl.Text = "Girl";
            this.rdoGirl.UseVisualStyleBackColor = true;
            this.rdoGirl.CheckedChanged += new System.EventHandler(this.rdoGender_CheckedChanged);
            // 
            // rdoBoy
            // 
            this.rdoBoy.AutoSize = true;
            this.rdoBoy.Location = new System.Drawing.Point(12, 560);
            this.rdoBoy.Name = "rdoBoy";
            this.rdoBoy.Size = new System.Drawing.Size(100, 36);
            this.rdoBoy.TabIndex = 5;
            this.rdoBoy.Text = "Boy";
            this.rdoBoy.UseVisualStyleBackColor = true;
            this.rdoBoy.CheckedChanged += new System.EventHandler(this.rdoGender_CheckedChanged);
            // 
            // chkShiny
            // 
            this.chkShiny.AutoSize = true;
            this.chkShiny.Location = new System.Drawing.Point(22, 654);
            this.chkShiny.Name = "chkShiny";
            this.chkShiny.Size = new System.Drawing.Size(124, 36);
            this.chkShiny.TabIndex = 6;
            this.chkShiny.Text = "Shiny";
            this.chkShiny.UseVisualStyleBackColor = true;
            this.chkShiny.CheckedChanged += new System.EventHandler(this.chkShiny_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1825, 1379);
            this.Controls.Add(this.chkShiny);
            this.Controls.Add(this.rdoBoy);
            this.Controls.Add(this.rdoGirl);
            this.Controls.Add(this.lblPlayerName);
            this.Controls.Add(this.trainerCard);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.panelCards);
            this.Controls.Add(this.comboBoxGames);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.trainerCard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPlayerName;
        private System.Windows.Forms.RadioButton rdoGirl;
        private System.Windows.Forms.RadioButton rdoBoy;
        private System.Windows.Forms.CheckBox chkShiny;
    }
}
