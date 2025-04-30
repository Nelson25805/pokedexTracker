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
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtHour = new System.Windows.Forms.TextBox();
            this.txtMinute = new System.Windows.Forms.TextBox();
            this.lblHour = new System.Windows.Forms.Label();
            this.lblMinute = new System.Windows.Forms.Label();
            this.radioPrinter = new PokeballRadioButton();
            this.radioRegular = new PokeballRadioButton();
            this.radioSGB = new PokeballRadioButton();
            this.radioGB = new PokeballRadioButton();
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(686, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(354, 32);
            this.label1.TabIndex = 8;
            this.label1.Text = "Choose Gameboy Version:";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(706, 591);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(158, 83);
            this.buttonSave.TabIndex = 9;
            this.buttonSave.Text = "Save Diploma";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(891, 591);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(158, 83);
            this.buttonClose.TabIndex = 10;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(686, 153);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(336, 32);
            this.label2.TabIndex = 13;
            this.label2.Text = "Choose Diploma Version:";
            // 
            // txtHour
            // 
            this.txtHour.Location = new System.Drawing.Point(681, 384);
            this.txtHour.Name = "txtHour";
            this.txtHour.Size = new System.Drawing.Size(150, 38);
            this.txtHour.TabIndex = 14;
            this.txtHour.Text = "00";
            // 
            // txtMinute
            // 
            this.txtMinute.Location = new System.Drawing.Point(868, 384);
            this.txtMinute.MaxLength = 2;
            this.txtMinute.Name = "txtMinute";
            this.txtMinute.Size = new System.Drawing.Size(100, 38);
            this.txtMinute.TabIndex = 15;
            this.txtMinute.Text = "00";
            // 
            // lblHour
            // 
            this.lblHour.AutoSize = true;
            this.lblHour.Location = new System.Drawing.Point(675, 341);
            this.lblHour.Name = "lblHour";
            this.lblHour.Size = new System.Drawing.Size(97, 32);
            this.lblHour.TabIndex = 16;
            this.lblHour.Text = "Hours:";
            // 
            // lblMinute
            // 
            this.lblMinute.AutoSize = true;
            this.lblMinute.Location = new System.Drawing.Point(862, 341);
            this.lblMinute.Name = "lblMinute";
            this.lblMinute.Size = new System.Drawing.Size(122, 32);
            this.lblMinute.TabIndex = 17;
            this.lblMinute.Text = "Minutes:";
            // 
            // radioPrinter
            // 
            this.radioPrinter.CheckedImage = global::PokedexTracker.Properties.Resources.checkedPokeball;
            this.radioPrinter.Location = new System.Drawing.Point(675, 257);
            this.radioPrinter.Name = "radioPrinter";
            this.radioPrinter.Size = new System.Drawing.Size(241, 81);
            this.radioPrinter.TabIndex = 12;
            this.radioPrinter.Text = "Printer";
            this.radioPrinter.UncheckedImage = global::PokedexTracker.Properties.Resources.uncheckedPokeball;
            this.radioPrinter.UseVisualStyleBackColor = true;
            // 
            // radioRegular
            // 
            this.radioRegular.CheckedImage = global::PokedexTracker.Properties.Resources.checkedPokeball;
            this.radioRegular.Location = new System.Drawing.Point(675, 179);
            this.radioRegular.Name = "radioRegular";
            this.radioRegular.Size = new System.Drawing.Size(241, 81);
            this.radioRegular.TabIndex = 11;
            this.radioRegular.Text = "Regular";
            this.radioRegular.UncheckedImage = global::PokedexTracker.Properties.Resources.uncheckedPokeball;
            this.radioRegular.UseVisualStyleBackColor = true;
            // 
            // radioSGB
            // 
            this.radioSGB.CheckedImage = global::PokedexTracker.Properties.Resources.checkedPokeball;
            this.radioSGB.Location = new System.Drawing.Point(825, 69);
            this.radioSGB.Name = "radioSGB";
            this.radioSGB.Size = new System.Drawing.Size(180, 81);
            this.radioSGB.TabIndex = 7;
            this.radioSGB.Text = "SGB";
            this.radioSGB.UncheckedImage = global::PokedexTracker.Properties.Resources.uncheckedPokeball;
            this.radioSGB.UseVisualStyleBackColor = true;
            // 
            // radioGB
            // 
            this.radioGB.CheckedImage = global::PokedexTracker.Properties.Resources.checkedPokeball;
            this.radioGB.Location = new System.Drawing.Point(675, 69);
            this.radioGB.Name = "radioGB";
            this.radioGB.Size = new System.Drawing.Size(180, 81);
            this.radioGB.TabIndex = 6;
            this.radioGB.Text = "GB";
            this.radioGB.UncheckedImage = global::PokedexTracker.Properties.Resources.uncheckedPokeball;
            this.radioGB.UseVisualStyleBackColor = true;
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
            this.Controls.Add(this.label2);
            this.Controls.Add(this.radioPrinter);
            this.Controls.Add(this.radioRegular);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSave);
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
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label label2;
        private PokeballRadioButton radioPrinter;
        private PokeballRadioButton radioRegular;
        private System.Windows.Forms.TextBox txtHour;
        private System.Windows.Forms.TextBox txtMinute;
        private System.Windows.Forms.Label lblHour;
        private System.Windows.Forms.Label lblMinute;
    }
}