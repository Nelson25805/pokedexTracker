namespace PokedexTracker.Forms
{
    partial class DiplomaForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiplomaForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.radioSGB = new PokeballRadioButton();
            this.radioGB = new PokeballRadioButton();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(41, 26);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(628, 511);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // radioSGB
            // 
            this.radioSGB.CheckedImage = global::PokedexTracker.Properties.Resources.checkedPokeball;
            this.radioSGB.Location = new System.Drawing.Point(846, 82);
            this.radioSGB.Name = "radioSGB";
            this.radioSGB.Size = new System.Drawing.Size(180, 81);
            this.radioSGB.TabIndex = 7;
            this.radioSGB.TabStop = true;
            this.radioSGB.Text = "SGB";
            this.radioSGB.UncheckedImage = global::PokedexTracker.Properties.Resources.uncheckedPokeball;
            this.radioSGB.UseVisualStyleBackColor = true;
            // 
            // radioGB
            // 
            this.radioGB.CheckedImage = global::PokedexTracker.Properties.Resources.checkedPokeball;
            this.radioGB.Location = new System.Drawing.Point(687, 82);
            this.radioGB.Name = "radioGB";
            this.radioGB.Size = new System.Drawing.Size(180, 81);
            this.radioGB.TabIndex = 6;
            this.radioGB.TabStop = true;
            this.radioGB.Text = "GB";
            this.radioGB.UncheckedImage = global::PokedexTracker.Properties.Resources.uncheckedPokeball;
            this.radioGB.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(681, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(336, 32);
            this.label1.TabIndex = 8;
            this.label1.Text = "Choose Diploma Version:";
            // 
            // DiplomaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1048, 715);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioSGB);
            this.Controls.Add(this.radioGB);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DiplomaForm";
            this.Text = "Diploma";
            this.Load += new System.EventHandler(this.DiplomaForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private PokeballRadioButton radioGB;
        private PokeballRadioButton radioSGB;
        private System.Windows.Forms.Label label1;
    }
}