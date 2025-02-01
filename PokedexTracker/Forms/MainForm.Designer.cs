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
            ((System.ComponentModel.ISupportInitialize)(this.trainerCard)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxGames
            // 
            this.comboBoxGames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGames.Font = new System.Drawing.Font("PKMN RBYGSC", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxGames.FormattingEnabled = true;
            // You can add items here or do it in code; for example:
            this.comboBoxGames.Items.AddRange(new object[] {
                "Red",
                "Blue",
                "Yellow",
                "Silver",
                "Gold",
                "Crystal"});
            this.comboBoxGames.Location = new System.Drawing.Point(12, 459);
            this.comboBoxGames.Name = "comboBoxGames";
            this.comboBoxGames.Size = new System.Drawing.Size(300, 57);
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1825, 1379);
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
    }
}
