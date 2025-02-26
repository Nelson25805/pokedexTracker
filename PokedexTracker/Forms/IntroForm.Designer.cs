namespace PokedexTracker.Forms
{
    partial class IntroForm
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
            this.components = new System.ComponentModel.Container();
            this.professorLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.submitButton = new System.Windows.Forms.Button();
            this.skipIntroButton = new System.Windows.Forms.Button();
            this.typingTimer = new System.Windows.Forms.Timer(this.components);
            this.professorPictureBox = new System.Windows.Forms.PictureBox();
            this.advanceButton = new System.Windows.Forms.Button();
            this.fadeTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.professorPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // professorLabel
            // 
            this.professorLabel.AutoSize = true;
            this.professorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.professorLabel.Location = new System.Drawing.Point(341, 388);
            this.professorLabel.Name = "professorLabel";
            this.professorLabel.Size = new System.Drawing.Size(715, 32);
            this.professorLabel.TabIndex = 1;
            this.professorLabel.Text = "Hello! I am the Pokémon Professor. What is your name?";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameTextBox.Location = new System.Drawing.Point(802, 561);
            this.nameTextBox.MaxLength = 7;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(200, 38);
            this.nameTextBox.TabIndex = 2;
            this.nameTextBox.Visible = false;
            // 
            // submitButton
            // 
            this.submitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.submitButton.Location = new System.Drawing.Point(798, 652);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(204, 60);
            this.submitButton.TabIndex = 3;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Visible = false;
            // 
            // skipIntroButton
            // 
            this.skipIntroButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.skipIntroButton.Location = new System.Drawing.Point(1463, 840);
            this.skipIntroButton.Name = "skipIntroButton";
            this.skipIntroButton.Size = new System.Drawing.Size(204, 60);
            this.skipIntroButton.TabIndex = 4;
            this.skipIntroButton.Text = "Skip Intro";
            this.skipIntroButton.UseVisualStyleBackColor = true;
            // 
            // typingTimer
            // 
            this.typingTimer.Interval = 20;
            this.typingTimer.Tick += new System.EventHandler(this.typingTimer_Tick);
            // 
            // professorPictureBox
            // 
            this.professorPictureBox.Location = new System.Drawing.Point(757, 50);
            this.professorPictureBox.Name = "professorPictureBox";
            this.professorPictureBox.Size = new System.Drawing.Size(293, 317);
            this.professorPictureBox.TabIndex = 5;
            this.professorPictureBox.TabStop = false;
            // 
            // advanceButton
            // 
            this.advanceButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.advanceButton.Location = new System.Drawing.Point(1288, 482);
            this.advanceButton.Name = "advanceButton";
            this.advanceButton.Size = new System.Drawing.Size(150, 70);
            this.advanceButton.TabIndex = 6;
            this.advanceButton.Text = "Next";
            this.advanceButton.UseVisualStyleBackColor = true;
            this.advanceButton.Visible = false;
            // 
            // fadeTimer
            // 
            this.fadeTimer.Interval = 200;
            this.fadeTimer.Tick += new System.EventHandler(this.fadeTimer_Tick);
            // 
            // IntroForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1697, 961);
            this.Controls.Add(this.advanceButton);
            this.Controls.Add(this.professorPictureBox);
            this.Controls.Add(this.skipIntroButton);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.professorLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "IntroForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IntroForm";
            this.Load += new System.EventHandler(this.IntroForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.professorPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label professorLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Button skipIntroButton;
        private System.Windows.Forms.Timer typingTimer;
        private System.Windows.Forms.PictureBox professorPictureBox;
        private System.Windows.Forms.Button advanceButton;
        private System.Windows.Forms.Timer fadeTimer;
    }
}