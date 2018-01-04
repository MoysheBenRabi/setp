namespace CloudDeck
{
    partial class LoginForm
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
            this.webLoginUrlTextBox = new System.Windows.Forms.TextBox();
            this.webLoginBrowser = new System.Windows.Forms.WebBrowser();
            this.label1 = new System.Windows.Forms.Label();
            this.userPassphraseTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.userIdentifierTextBox = new System.Windows.Forms.TextBox();
            this.exitButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.bubbleAddressTextBox = new System.Windows.Forms.TextBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.nickTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // webLoginUrlTextBox
            // 
            this.webLoginUrlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.webLoginUrlTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.webLoginUrlTextBox.Location = new System.Drawing.Point(67, 5);
            this.webLoginUrlTextBox.Margin = new System.Windows.Forms.Padding(5);
            this.webLoginUrlTextBox.Name = "webLoginUrlTextBox";
            this.webLoginUrlTextBox.Size = new System.Drawing.Size(786, 20);
            this.webLoginUrlTextBox.TabIndex = 8;
            this.webLoginUrlTextBox.Text = "hub.eu.bubblecloud.org";
            this.webLoginUrlTextBox.Leave += new System.EventHandler(this.webLoginUrlTextBox_Leave);
            this.webLoginUrlTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.webLoginUrlTextBox_KeyUp);
            // 
            // webLoginBrowser
            // 
            this.webLoginBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.webLoginBrowser.Location = new System.Drawing.Point(0, 55);
            this.webLoginBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webLoginBrowser.Name = "webLoginBrowser";
            this.webLoginBrowser.ScrollBarsEnabled = false;
            this.webLoginBrowser.Size = new System.Drawing.Size(859, 574);
            this.webLoginBrowser.TabIndex = 6;
            this.webLoginBrowser.Url = new System.Uri("", System.UriKind.Relative);
            this.webLoginBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webLoginBrowser_DocumentCompleted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "UserName";
            // 
            // userPassphraseTextBox
            // 
            this.userPassphraseTextBox.Location = new System.Drawing.Point(16, 123);
            this.userPassphraseTextBox.Name = "userPassphraseTextBox";
            this.userPassphraseTextBox.PasswordChar = '*';
            this.userPassphraseTextBox.Size = new System.Drawing.Size(209, 20);
            this.userPassphraseTextBox.TabIndex = 3;
            this.userPassphraseTextBox.Text = "Test Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Hub Address";
            // 
            // userIdentifierTextBox
            // 
            this.userIdentifierTextBox.Location = new System.Drawing.Point(16, 84);
            this.userIdentifierTextBox.Name = "userIdentifierTextBox";
            this.userIdentifierTextBox.Size = new System.Drawing.Size(209, 20);
            this.userIdentifierTextBox.TabIndex = 2;
            this.userIdentifierTextBox.Text = "Test User";
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(16, 158);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(101, 23);
            this.exitButton.TabIndex = 5;
            this.exitButton.Text = "<< Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Password";
            // 
            // bubbleAddressTextBox
            // 
            this.bubbleAddressTextBox.Location = new System.Drawing.Point(15, 33);
            this.bubbleAddressTextBox.Name = "bubbleAddressTextBox";
            this.bubbleAddressTextBox.Size = new System.Drawing.Size(760, 20);
            this.bubbleAddressTextBox.TabIndex = 1;
            this.bubbleAddressTextBox.Text = "mxp://127.0.0.1:1253/E7A8F1BC-212B-4b20-9B6C-66C16362DEC3";
            this.bubbleAddressTextBox.TextChanged += new System.EventHandler(this.bubbleAddressTextBox_TextChanged);
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(123, 158);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(102, 23);
            this.loginButton.TabIndex = 4;
            this.loginButton.Text = "Login >>";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(866, 654);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.nickTextBox);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.webLoginBrowser);
            this.tabPage2.Controls.Add(this.webLoginUrlTextBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(858, 628);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Web Login";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // nickTextBox
            // 
            this.nickTextBox.Location = new System.Drawing.Point(67, 29);
            this.nickTextBox.MaxLength = 20;
            this.nickTextBox.Name = "nickTextBox";
            this.nickTextBox.Size = new System.Drawing.Size(241, 20);
            this.nickTextBox.TabIndex = 11;
            this.nickTextBox.Text = "Anonymous";
            this.nickTextBox.TextChanged += new System.EventHandler(this.nickTextBox_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Nick";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Address";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.loginButton);
            this.tabPage1.Controls.Add(this.bubbleAddressTextBox);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.exitButton);
            this.tabPage1.Controls.Add(this.userIdentifierTextBox);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.userPassphraseTextBox);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(882, 652);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Manual Login";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(890, 678);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Name = "LoginForm";
            this.Opacity = 0.8;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoginForm";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.Shown += new System.EventHandler(this.LoginForm_Shown);
            this.VisibleChanged += new System.EventHandler(this.LoginForm_VisibleChanged);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webLoginBrowser;
        private System.Windows.Forms.TextBox webLoginUrlTextBox;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.TextBox bubbleAddressTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.TextBox userIdentifierTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox userPassphraseTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox nickTextBox;

    }
}