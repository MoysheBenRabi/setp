namespace CloudDeck
{
    partial class HudForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HudForm));
            this.logoutButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.buildButton = new System.Windows.Forms.Button();
            this.selectionButton = new System.Windows.Forms.Button();
            this.chatButton = new System.Windows.Forms.Button();
            this.urlLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.bubbleInformationLabel = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.modifyControl1 = new CloudDeck.Controls.ModifyControl();
            this.buildControl1 = new CloudDeck.Controls.BuildControl();
            this.targetControl1 = new CloudDeck.Controls.SelectionControl();
            this.chatControl1 = new CloudDeck.Controls.ChatControl();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // logoutButton
            // 
            this.logoutButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.logoutButton.Location = new System.Drawing.Point(0, 0);
            this.logoutButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(84, 28);
            this.logoutButton.TabIndex = 0;
            this.logoutButton.Text = "<< Logout";
            this.logoutButton.UseVisualStyleBackColor = true;
            this.logoutButton.Click += new System.EventHandler(this.logoutButton_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.buildButton);
            this.panel1.Controls.Add(this.selectionButton);
            this.panel1.Controls.Add(this.chatButton);
            this.panel1.Controls.Add(this.logoutButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(5, 565);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1071, 30);
            this.panel1.TabIndex = 1;
            // 
            // button5
            // 
            this.button5.Dock = System.Windows.Forms.DockStyle.Right;
            this.button5.Location = new System.Drawing.Point(649, 0);
            this.button5.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(84, 28);
            this.button5.TabIndex = 5;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Dock = System.Windows.Forms.DockStyle.Right;
            this.button4.Location = new System.Drawing.Point(733, 0);
            this.button4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(84, 28);
            this.button4.TabIndex = 4;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // buildButton
            // 
            this.buildButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.buildButton.Location = new System.Drawing.Point(817, 0);
            this.buildButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buildButton.Name = "buildButton";
            this.buildButton.Size = new System.Drawing.Size(84, 28);
            this.buildButton.TabIndex = 3;
            this.buildButton.Text = "Build";
            this.buildButton.UseVisualStyleBackColor = true;
            this.buildButton.Click += new System.EventHandler(this.buildButton_Click);
            // 
            // selectionButton
            // 
            this.selectionButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.selectionButton.Location = new System.Drawing.Point(901, 0);
            this.selectionButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.selectionButton.Name = "selectionButton";
            this.selectionButton.Size = new System.Drawing.Size(84, 28);
            this.selectionButton.TabIndex = 2;
            this.selectionButton.Text = "Selection";
            this.selectionButton.UseVisualStyleBackColor = true;
            this.selectionButton.Click += new System.EventHandler(this.selectionButton_Click);
            // 
            // chatButton
            // 
            this.chatButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.chatButton.Location = new System.Drawing.Point(985, 0);
            this.chatButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chatButton.Name = "chatButton";
            this.chatButton.Size = new System.Drawing.Size(84, 28);
            this.chatButton.TabIndex = 1;
            this.chatButton.Text = "Chat";
            this.chatButton.UseVisualStyleBackColor = true;
            this.chatButton.Click += new System.EventHandler(this.chatButton_Click);
            // 
            // urlLabel
            // 
            this.urlLabel.AutoSize = true;
            this.urlLabel.BackColor = System.Drawing.Color.Transparent;
            this.urlLabel.Enabled = false;
            this.urlLabel.Font = new System.Drawing.Font("Arial Narrow", 10F);
            this.urlLabel.Location = new System.Drawing.Point(542, 6);
            this.urlLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.urlLabel.Name = "urlLabel";
            this.urlLabel.Size = new System.Drawing.Size(39, 17);
            this.urlLabel.TabIndex = 1;
            this.urlLabel.Text = "mxp://";
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(5, 5);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1071, 30);
            this.panel2.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.Controls.Add(this.urlLabel);
            this.panel4.Controls.Add(this.bubbleInformationLabel);
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(5);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(5);
            this.panel4.Size = new System.Drawing.Size(1028, 28);
            this.panel4.TabIndex = 1;
            // 
            // bubbleInformationLabel
            // 
            this.bubbleInformationLabel.AutoSize = true;
            this.bubbleInformationLabel.Font = new System.Drawing.Font("Arial Narrow", 10F);
            this.bubbleInformationLabel.Location = new System.Drawing.Point(7, 5);
            this.bubbleInformationLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.bubbleInformationLabel.Name = "bubbleInformationLabel";
            this.bubbleInformationLabel.Size = new System.Drawing.Size(53, 17);
            this.bubbleInformationLabel.TabIndex = 0;
            this.bubbleInformationLabel.Text = "Bubble X";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.Controls.Add(this.progressBar1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(1028, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(5);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(5);
            this.panel3.Size = new System.Drawing.Size(41, 28);
            this.panel3.TabIndex = 0;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.progressBar1.Location = new System.Drawing.Point(7, 5);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(29, 18);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 0;
            this.progressBar1.Value = 10;
            // 
            // modifyControl1
            // 
            this.modifyControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.modifyControl1.BackColor = System.Drawing.Color.White;
            this.modifyControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.modifyControl1.Location = new System.Drawing.Point(838, 110);
            this.modifyControl1.Name = "modifyControl1";
            this.modifyControl1.Size = new System.Drawing.Size(238, 239);
            this.modifyControl1.TabIndex = 6;
            this.modifyControl1.Visible = false;
            // 
            // buildControl1
            // 
            this.buildControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buildControl1.BackColor = System.Drawing.Color.White;
            this.buildControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.buildControl1.Location = new System.Drawing.Point(566, 42);
            this.buildControl1.Name = "buildControl1";
            this.buildControl1.Size = new System.Drawing.Size(510, 62);
            this.buildControl1.TabIndex = 5;
            this.buildControl1.Visible = false;
            // 
            // targetControl1
            // 
            this.targetControl1.BackColor = System.Drawing.Color.White;
            this.targetControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.targetControl1.Location = new System.Drawing.Point(5, 42);
            this.targetControl1.Name = "targetControl1";
            this.targetControl1.Size = new System.Drawing.Size(317, 198);
            this.targetControl1.TabIndex = 4;
            this.targetControl1.Visible = false;
            // 
            // chatControl1
            // 
            this.chatControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chatControl1.BackColor = System.Drawing.Color.White;
            this.chatControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chatControl1.Location = new System.Drawing.Point(6, 355);
            this.chatControl1.Name = "chatControl1";
            this.chatControl1.Size = new System.Drawing.Size(419, 204);
            this.chatControl1.TabIndex = 3;
            this.chatControl1.Visible = false;
            // 
            // HudForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGreen;
            this.ClientSize = new System.Drawing.Size(1081, 600);
            this.Controls.Add(this.modifyControl1);
            this.Controls.Add(this.buildControl1);
            this.Controls.Add(this.targetControl1);
            this.Controls.Add(this.chatControl1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "HudForm";
            this.Opacity = 0.7;
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "HudForm";
            this.TransparencyKey = System.Drawing.Color.LightGreen;
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button logoutButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button buildButton;
        private System.Windows.Forms.Button selectionButton;
        private System.Windows.Forms.Button chatButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label bubbleInformationLabel;
        private System.Windows.Forms.Label urlLabel;
        private CloudDeck.Controls.ChatControl chatControl1;
        private CloudDeck.Controls.SelectionControl targetControl1;
        private CloudDeck.Controls.BuildControl buildControl1;
        private CloudDeck.Controls.ModifyControl modifyControl1;
    }
}