namespace PokedexTracker
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnRed = new System.Windows.Forms.Button();
            this.btnBlue = new System.Windows.Forms.Button();
            this.btnYellow = new System.Windows.Forms.Button();
            this.panelCards = new System.Windows.Forms.Panel();
            this.lblProgress = new System.Windows.Forms.Label();
            this.trainerCard = new System.Windows.Forms.PictureBox();
            this.btnSilver = new System.Windows.Forms.Button();
            this.btnGold = new System.Windows.Forms.Button();
            this.btnCrystal = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trainerCard)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRed
            // 
            this.btnRed.Font = new System.Drawing.Font("PKMN RBYGSC", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRed.Location = new System.Drawing.Point(12, 459);
            this.btnRed.Name = "btnRed";
            this.btnRed.Size = new System.Drawing.Size(130, 57);
            this.btnRed.TabIndex = 0;
            this.btnRed.Tag = "Red";
            this.btnRed.Text = "Red";
            this.btnRed.UseVisualStyleBackColor = true;
            this.btnRed.Click += new System.EventHandler(this.GameSwitchButton_Click);
            // 
            // btnBlue
            // 
            this.btnBlue.Font = new System.Drawing.Font("PKMN RBYGSC", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBlue.Location = new System.Drawing.Point(167, 459);
            this.btnBlue.Name = "btnBlue";
            this.btnBlue.Size = new System.Drawing.Size(138, 57);
            this.btnBlue.TabIndex = 1;
            this.btnBlue.Tag = "Blue";
            this.btnBlue.Text = "Blue";
            this.btnBlue.UseVisualStyleBackColor = true;
            this.btnBlue.Click += new System.EventHandler(this.GameSwitchButton_Click);
            // 
            // btnYellow
            // 
            this.btnYellow.Font = new System.Drawing.Font("PKMN RBYGSC", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYellow.Location = new System.Drawing.Point(330, 459);
            this.btnYellow.Name = "btnYellow";
            this.btnYellow.Size = new System.Drawing.Size(171, 57);
            this.btnYellow.TabIndex = 2;
            this.btnYellow.Tag = "Yellow";
            this.btnYellow.Text = "Yellow";
            this.btnYellow.UseVisualStyleBackColor = true;
            this.btnYellow.Click += new System.EventHandler(this.GameSwitchButton_Click);
            // 
            // panelCards
            // 
            this.panelCards.AutoScroll = true;
            this.panelCards.Location = new System.Drawing.Point(716, 12);
            this.panelCards.Name = "panelCards";
            this.panelCards.Size = new System.Drawing.Size(1108, 1329);
            this.panelCards.TabIndex = 4;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.BackColor = System.Drawing.SystemColors.Window;
            this.lblProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.Location = new System.Drawing.Point(341, 118);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(73, 32);
            this.lblProgress.TabIndex = 6;
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
            this.trainerCard.TabIndex = 8;
            this.trainerCard.TabStop = false;
            this.trainerCard.Visible = false;
            // 
            // btnSilver
            // 
            this.btnSilver.Font = new System.Drawing.Font("PKMN RBYGSC", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSilver.Location = new System.Drawing.Point(12, 540);
            this.btnSilver.Name = "btnSilver";
            this.btnSilver.Size = new System.Drawing.Size(170, 50);
            this.btnSilver.TabIndex = 9;
            this.btnSilver.Tag = "Silver";
            this.btnSilver.Text = "Silver";
            this.btnSilver.UseVisualStyleBackColor = true;
            this.btnSilver.Click += new System.EventHandler(this.GameSwitchButton_Click);
            // 
            // btnGold
            // 
            this.btnGold.Font = new System.Drawing.Font("PKMN RBYGSC", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGold.Location = new System.Drawing.Point(211, 540);
            this.btnGold.Name = "btnGold";
            this.btnGold.Size = new System.Drawing.Size(144, 50);
            this.btnGold.TabIndex = 10;
            this.btnGold.Tag = "Gold";
            this.btnGold.Text = "Gold";
            this.btnGold.UseVisualStyleBackColor = true;
            this.btnGold.Click += new System.EventHandler(this.GameSwitchButton_Click);
            // 
            // btnCrystal
            // 
            this.btnCrystal.Font = new System.Drawing.Font("PKMN RBYGSC", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCrystal.Location = new System.Drawing.Point(361, 540);
            this.btnCrystal.Name = "btnCrystal";
            this.btnCrystal.Size = new System.Drawing.Size(205, 50);
            this.btnCrystal.TabIndex = 11;
            this.btnCrystal.Tag = "Crystal";
            this.btnCrystal.Text = "Crystal";
            this.btnCrystal.UseVisualStyleBackColor = true;
            this.btnCrystal.Click += new System.EventHandler(this.GameSwitchButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1825, 1379);
            this.Controls.Add(this.btnCrystal);
            this.Controls.Add(this.btnGold);
            this.Controls.Add(this.btnSilver);
            this.Controls.Add(this.trainerCard);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.panelCards);
            this.Controls.Add(this.btnYellow);
            this.Controls.Add(this.btnBlue);
            this.Controls.Add(this.btnRed);
            this.Name = "MainForm";
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.trainerCard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRed;
        private System.Windows.Forms.Button btnBlue;
        private System.Windows.Forms.Button btnYellow;
        private System.Windows.Forms.Panel panelCards;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.PictureBox trainerCard;
        private System.Windows.Forms.Button btnSilver;
        private System.Windows.Forms.Button btnGold;
        private System.Windows.Forms.Button btnCrystal;
    }
}