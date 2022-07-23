namespace Client
{
    partial class FormStart
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
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
        private void InitializeComponent() {
            this.userNameLabel = new System.Windows.Forms.Label();
            this.signinButton = new System.Windows.Forms.Button();
            this.userNameTextBox = new System.Windows.Forms.TextBox();
            this.exitButton = new System.Windows.Forms.Button();
            this.listPlayer = new System.Windows.Forms.ListView();
            this.panelChallenge = new System.Windows.Forms.Panel();
            this.signoutButton = new System.Windows.Forms.Button();
            this.buttonReload = new System.Windows.Forms.Button();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.signupButton = new System.Windows.Forms.Button();
            this.namePlayerLabel = new System.Windows.Forms.Label();
            this.resultButton = new System.Windows.Forms.Button();
            this.listLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // userNameLabel
            // 
            this.userNameLabel.AutoSize = true;
            this.userNameLabel.Location = new System.Drawing.Point(294, 36);
            this.userNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.userNameLabel.Name = "userNameLabel";
            this.userNameLabel.Size = new System.Drawing.Size(73, 17);
            this.userNameLabel.TabIndex = 0;
            this.userNameLabel.Text = "Username";
            // 
            // signinButton
            // 
            this.signinButton.Location = new System.Drawing.Point(321, 136);
            this.signinButton.Margin = new System.Windows.Forms.Padding(4);
            this.signinButton.Name = "signinButton";
            this.signinButton.Size = new System.Drawing.Size(100, 28);
            this.signinButton.TabIndex = 1;
            this.signinButton.Text = "Sign In";
            this.signinButton.UseVisualStyleBackColor = true;
            this.signinButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // userNameTextBox
            // 
            this.userNameTextBox.Location = new System.Drawing.Point(372, 33);
            this.userNameTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.userNameTextBox.Name = "userNameTextBox";
            this.userNameTextBox.Size = new System.Drawing.Size(132, 22);
            this.userNameTextBox.TabIndex = 2;
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(352, 366);
            this.exitButton.Margin = new System.Windows.Forms.Padding(4);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(171, 34);
            this.exitButton.TabIndex = 3;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // listPlayer
            // 
            this.listPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listPlayer.Location = new System.Drawing.Point(13, 36);
            this.listPlayer.Margin = new System.Windows.Forms.Padding(4);
            this.listPlayer.Name = "listPlayer";
            this.listPlayer.Size = new System.Drawing.Size(250, 313);
            this.listPlayer.TabIndex = 4;
            this.listPlayer.UseCompatibleStateImageBehavior = false;
            // 
            // panelChallenge
            // 
            this.panelChallenge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panelChallenge.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panelChallenge.Location = new System.Drawing.Point(352, 172);
            this.panelChallenge.Margin = new System.Windows.Forms.Padding(4);
            this.panelChallenge.Name = "panelChallenge";
            this.panelChallenge.Size = new System.Drawing.Size(171, 177);
            this.panelChallenge.TabIndex = 5;
            // 
            // signoutButton
            // 
            this.signoutButton.Location = new System.Drawing.Point(321, 100);
            this.signoutButton.Margin = new System.Windows.Forms.Padding(4);
            this.signoutButton.Name = "signoutButton";
            this.signoutButton.Size = new System.Drawing.Size(100, 28);
            this.signoutButton.TabIndex = 6;
            this.signoutButton.Text = "Sign Out";
            this.signoutButton.UseVisualStyleBackColor = true;
            this.signoutButton.Click += new System.EventHandler(this.logoutButton_Click);
            // 
            // buttonReload
            // 
            this.buttonReload.Location = new System.Drawing.Point(12, 366);
            this.buttonReload.Name = "buttonReload";
            this.buttonReload.Size = new System.Drawing.Size(251, 34);
            this.buttonReload.TabIndex = 7;
            this.buttonReload.Text = "Reload";
            this.buttonReload.UseVisualStyleBackColor = true;
            this.buttonReload.Click += new System.EventHandler(this.buttonReload_Click);
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(372, 71);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(132, 22);
            this.passwordTextBox.TabIndex = 8;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(294, 74);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(69, 17);
            this.passwordLabel.TabIndex = 10;
            this.passwordLabel.Text = "Password";
            // 
            // signupButton
            // 
            this.signupButton.Location = new System.Drawing.Point(453, 136);
            this.signupButton.Margin = new System.Windows.Forms.Padding(4);
            this.signupButton.Name = "signupButton";
            this.signupButton.Size = new System.Drawing.Size(100, 28);
            this.signupButton.TabIndex = 11;
            this.signupButton.Text = "Sign Up";
            this.signupButton.UseVisualStyleBackColor = true;
            this.signupButton.Click += new System.EventHandler(this.signupButton_Click);
            // 
            // namePlayerLabel
            // 
            this.namePlayerLabel.AutoSize = true;
            this.namePlayerLabel.Location = new System.Drawing.Point(315, 36);
            this.namePlayerLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.namePlayerLabel.Name = "namePlayerLabel";
            this.namePlayerLabel.Size = new System.Drawing.Size(48, 17);
            this.namePlayerLabel.TabIndex = 12;
            this.namePlayerLabel.Text = "Player";
            // 
            // resultButton
            // 
            this.resultButton.Location = new System.Drawing.Point(453, 100);
            this.resultButton.Margin = new System.Windows.Forms.Padding(4);
            this.resultButton.Name = "resultButton";
            this.resultButton.Size = new System.Drawing.Size(100, 28);
            this.resultButton.TabIndex = 13;
            this.resultButton.Text = "Result";
            this.resultButton.UseVisualStyleBackColor = true;
            this.resultButton.Click += new System.EventHandler(this.resultButton_Click);
            // 
            // listLabel
            // 
            this.listLabel.AutoSize = true;
            this.listLabel.Location = new System.Drawing.Point(81, 15);
            this.listLabel.Name = "listLabel";
            this.listLabel.Size = new System.Drawing.Size(91, 17);
            this.listLabel.TabIndex = 14;
            this.listLabel.Text = "Player is free";
            // 
            // FormStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 415);
            this.Controls.Add(this.listLabel);
            this.Controls.Add(this.resultButton);
            this.Controls.Add(this.namePlayerLabel);
            this.Controls.Add(this.signupButton);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.buttonReload);
            this.Controls.Add(this.signoutButton);
            this.Controls.Add(this.panelChallenge);
            this.Controls.Add(this.listPlayer);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.userNameTextBox);
            this.Controls.Add(this.signinButton);
            this.Controls.Add(this.userNameLabel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormStart";
            this.Text = "Application";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label userNameLabel;
        private System.Windows.Forms.Button signinButton;
        private System.Windows.Forms.TextBox userNameTextBox;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.ListView listPlayer;
        private System.Windows.Forms.Panel panelChallenge;
        private System.Windows.Forms.Button signoutButton;
        private System.Windows.Forms.Button buttonReload;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Button signupButton;
        private System.Windows.Forms.Label namePlayerLabel;
        private System.Windows.Forms.Button resultButton;
        private System.Windows.Forms.Label listLabel;
    }
}