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
            this.lblOption = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.txtHour = new System.Windows.Forms.TextBox();
            this.txtMinute = new System.Windows.Forms.TextBox();
            this.lblHour = new System.Windows.Forms.Label();
            this.lblMinute = new System.Windows.Forms.Label();
            this.radioOption2 = new PokeballRadioButton();
            this.radioOption1 = new PokeballRadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(41, 26);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(628, 648);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lblOption
            // 
            this.lblOption.AutoSize = true;
            this.lblOption.Location = new System.Drawing.Point(686, 43);
            this.lblOption.Name = "lblOption";
            this.lblOption.Size = new System.Drawing.Size(354, 32);
            this.lblOption.TabIndex = 8;
            this.lblOption.Text = "Choose Gameboy Version:";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(755, 374);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(211, 102);
            this.buttonSave.TabIndex = 9;
            this.buttonSave.Text = "Save Diploma";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(755, 502);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(211, 102);
            this.buttonClose.TabIndex = 10;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // txtHour
            // 
            this.txtHour.Location = new System.Drawing.Point(704, 281);
            this.txtHour.Name = "txtHour";
            this.txtHour.Size = new System.Drawing.Size(150, 38);
            this.txtHour.TabIndex = 14;
            this.txtHour.Text = "00";
            this.txtHour.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtMinute
            // 
            this.txtMinute.Location = new System.Drawing.Point(891, 281);
            this.txtMinute.MaxLength = 2;
            this.txtMinute.Name = "txtMinute";
            this.txtMinute.Size = new System.Drawing.Size(100, 38);
            this.txtMinute.TabIndex = 15;
            this.txtMinute.Text = "00";
            // 
            // lblHour
            // 
            this.lblHour.AutoSize = true;
            this.lblHour.Location = new System.Drawing.Point(698, 238);
            this.lblHour.Name = "lblHour";
            this.lblHour.Size = new System.Drawing.Size(97, 32);
            this.lblHour.TabIndex = 16;
            this.lblHour.Text = "Hours:";
            // 
            // lblMinute
            // 
            this.lblMinute.AutoSize = true;
            this.lblMinute.Location = new System.Drawing.Point(885, 238);
            this.lblMinute.Name = "lblMinute";
            this.lblMinute.Size = new System.Drawing.Size(122, 32);
            this.lblMinute.TabIndex = 17;
            this.lblMinute.Text = "Minutes:";
            // 
            // radioOption2
            // 
            this.radioOption2.CheckedImage = global::PokedexTracker.Properties.Resources.checkedPokeball;
            this.radioOption2.Location = new System.Drawing.Point(675, 144);
            this.radioOption2.Name = "radioOption2";
            this.radioOption2.Size = new System.Drawing.Size(253, 81);
            this.radioOption2.TabIndex = 7;
            this.radioOption2.Text = "SGB";
            this.radioOption2.UncheckedImage = global::PokedexTracker.Properties.Resources.uncheckedPokeball;
            this.radioOption2.UseVisualStyleBackColor = true;
            // 
            // radioOption1
            // 
            this.radioOption1.CheckedImage = global::PokedexTracker.Properties.Resources.checkedPokeball;
            this.radioOption1.Location = new System.Drawing.Point(675, 69);
            this.radioOption1.Name = "radioOption1";
            this.radioOption1.Size = new System.Drawing.Size(253, 81);
            this.radioOption1.TabIndex = 6;
            this.radioOption1.Text = "GB";
            this.radioOption1.UncheckedImage = global::PokedexTracker.Properties.Resources.uncheckedPokeball;
            this.radioOption1.UseVisualStyleBackColor = true;
            // 
            // DiplomaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1076, 701);
            this.Controls.Add(this.lblMinute);
            this.Controls.Add(this.lblHour);
            this.Controls.Add(this.txtMinute);
            this.Controls.Add(this.txtHour);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.lblOption);
            this.Controls.Add(this.radioOption2);
            this.Controls.Add(this.radioOption1);
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
        private PokeballRadioButton radioOption1;
        private PokeballRadioButton radioOption2;
        private System.Windows.Forms.Label lblOption;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TextBox txtHour;
        private System.Windows.Forms.TextBox txtMinute;
        private System.Windows.Forms.Label lblHour;
        private System.Windows.Forms.Label lblMinute;
    }
}