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
            this.titleLabel = new System.Windows.Forms.Label();
            this.professorLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.submitButton = new System.Windows.Forms.Button();
            this.skipIntroButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(374, 176);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(457, 32);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Welcome to the World of Pokémon!";
            // 
            // professorLabel
            // 
            this.professorLabel.AutoSize = true;
            this.professorLabel.Location = new System.Drawing.Point(447, 329);
            this.professorLabel.Name = "professorLabel";
            this.professorLabel.Size = new System.Drawing.Size(715, 32);
            this.professorLabel.TabIndex = 1;
            this.professorLabel.Text = "Hello! I am the Pokémon Professor. What is your name?";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(539, 455);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(200, 38);
            this.nameTextBox.TabIndex = 2;
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(663, 613);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(204, 60);
            this.submitButton.TabIndex = 3;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            // 
            // skipIntroButton
            // 
            this.skipIntroButton.Location = new System.Drawing.Point(912, 613);
            this.skipIntroButton.Name = "skipIntroButton";
            this.skipIntroButton.Size = new System.Drawing.Size(204, 60);
            this.skipIntroButton.TabIndex = 4;
            this.skipIntroButton.Text = "Skip Intro";
            this.skipIntroButton.UseVisualStyleBackColor = true;
            // 
            // IntroForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1697, 961);
            this.Controls.Add(this.skipIntroButton);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.professorLabel);
            this.Controls.Add(this.titleLabel);
            this.Name = "IntroForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IntroForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label professorLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Button skipIntroButton;
    }
}