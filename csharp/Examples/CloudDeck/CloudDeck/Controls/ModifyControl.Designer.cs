namespace CloudDeck.Controls
{
    partial class ModifyControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.objectNameTextBox = new System.Windows.Forms.TextBox();
            this.typeNameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.xTextBox = new System.Windows.Forms.TextBox();
            this.yTextBox = new System.Windows.Forms.TextBox();
            this.zTextBox = new System.Windows.Forms.TextBox();
            this.xAddButton = new System.Windows.Forms.Button();
            this.xSubButton = new System.Windows.Forms.Button();
            this.yAddButton = new System.Windows.Forms.Button();
            this.ySubButton = new System.Windows.Forms.Button();
            this.zAddButton = new System.Windows.Forms.Button();
            this.zSubButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.locationStepTextBox = new System.Windows.Forms.TextBox();
            this.scaleStepTextBox = new System.Windows.Forms.TextBox();
            this.scaleTextBox = new System.Windows.Forms.TextBox();
            this.scaleAddbutton = new System.Windows.Forms.Button();
            this.scaleSubButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.orientationStepTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.yawAddButton = new System.Windows.Forms.Button();
            this.yawSubButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.pitchAddButton = new System.Windows.Forms.Button();
            this.pitchSubButton = new System.Windows.Forms.Button();
            this.rollAddButton = new System.Windows.Forms.Button();
            this.rollSubButton = new System.Windows.Forms.Button();
            this.resetOrientationButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Modify";
            // 
            // objectNameTextBox
            // 
            this.objectNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.objectNameTextBox.Location = new System.Drawing.Point(3, 21);
            this.objectNameTextBox.Name = "objectNameTextBox";
            this.objectNameTextBox.Size = new System.Drawing.Size(232, 20);
            this.objectNameTextBox.TabIndex = 1;
            this.objectNameTextBox.Leave += new System.EventHandler(this.objectNameTextBox_Leave);
            // 
            // typeNameTextBox
            // 
            this.typeNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.typeNameTextBox.Location = new System.Drawing.Point(3, 47);
            this.typeNameTextBox.Name = "typeNameTextBox";
            this.typeNameTextBox.ReadOnly = true;
            this.typeNameTextBox.Size = new System.Drawing.Size(232, 20);
            this.typeNameTextBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Location";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "X";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Y";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Z";
            // 
            // xTextBox
            // 
            this.xTextBox.Location = new System.Drawing.Point(24, 105);
            this.xTextBox.Name = "xTextBox";
            this.xTextBox.Size = new System.Drawing.Size(53, 20);
            this.xTextBox.TabIndex = 7;
            this.xTextBox.Leave += new System.EventHandler(this.xTextBox_Leave);
            // 
            // yTextBox
            // 
            this.yTextBox.Location = new System.Drawing.Point(24, 131);
            this.yTextBox.Name = "yTextBox";
            this.yTextBox.Size = new System.Drawing.Size(53, 20);
            this.yTextBox.TabIndex = 8;
            this.yTextBox.Leave += new System.EventHandler(this.yTextBox_Leave);
            // 
            // zTextBox
            // 
            this.zTextBox.Location = new System.Drawing.Point(24, 157);
            this.zTextBox.Name = "zTextBox";
            this.zTextBox.Size = new System.Drawing.Size(53, 20);
            this.zTextBox.TabIndex = 9;
            this.zTextBox.Leave += new System.EventHandler(this.zTextBox_Leave);
            // 
            // xAddButton
            // 
            this.xAddButton.Location = new System.Drawing.Point(81, 103);
            this.xAddButton.Name = "xAddButton";
            this.xAddButton.Size = new System.Drawing.Size(17, 23);
            this.xAddButton.TabIndex = 10;
            this.xAddButton.Text = "+";
            this.xAddButton.UseVisualStyleBackColor = true;
            this.xAddButton.Click += new System.EventHandler(this.xAddButton_Click);
            // 
            // xSubButton
            // 
            this.xSubButton.Location = new System.Drawing.Point(101, 103);
            this.xSubButton.Name = "xSubButton";
            this.xSubButton.Size = new System.Drawing.Size(17, 23);
            this.xSubButton.TabIndex = 11;
            this.xSubButton.Text = "-";
            this.xSubButton.UseVisualStyleBackColor = true;
            this.xSubButton.Click += new System.EventHandler(this.xSubButton_Click);
            // 
            // yAddButton
            // 
            this.yAddButton.Location = new System.Drawing.Point(81, 129);
            this.yAddButton.Name = "yAddButton";
            this.yAddButton.Size = new System.Drawing.Size(17, 23);
            this.yAddButton.TabIndex = 12;
            this.yAddButton.Text = "+";
            this.yAddButton.UseVisualStyleBackColor = true;
            this.yAddButton.Click += new System.EventHandler(this.yAddButton_Click);
            // 
            // ySubButton
            // 
            this.ySubButton.Location = new System.Drawing.Point(101, 129);
            this.ySubButton.Name = "ySubButton";
            this.ySubButton.Size = new System.Drawing.Size(17, 23);
            this.ySubButton.TabIndex = 13;
            this.ySubButton.Text = "-";
            this.ySubButton.UseVisualStyleBackColor = true;
            this.ySubButton.Click += new System.EventHandler(this.ySubButton_Click);
            // 
            // zAddButton
            // 
            this.zAddButton.Location = new System.Drawing.Point(81, 155);
            this.zAddButton.Name = "zAddButton";
            this.zAddButton.Size = new System.Drawing.Size(17, 23);
            this.zAddButton.TabIndex = 14;
            this.zAddButton.Text = "+";
            this.zAddButton.UseVisualStyleBackColor = true;
            this.zAddButton.Click += new System.EventHandler(this.zAddButton_Click);
            // 
            // zSubButton
            // 
            this.zSubButton.Location = new System.Drawing.Point(101, 155);
            this.zSubButton.Name = "zSubButton";
            this.zSubButton.Size = new System.Drawing.Size(17, 23);
            this.zSubButton.TabIndex = 15;
            this.zSubButton.Text = "-";
            this.zSubButton.UseVisualStyleBackColor = true;
            this.zSubButton.Click += new System.EventHandler(this.zSubButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 187);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Scale";
            // 
            // locationStepTextBox
            // 
            this.locationStepTextBox.Location = new System.Drawing.Point(81, 77);
            this.locationStepTextBox.Name = "locationStepTextBox";
            this.locationStepTextBox.Size = new System.Drawing.Size(37, 20);
            this.locationStepTextBox.TabIndex = 17;
            this.locationStepTextBox.Text = "1";
            // 
            // scaleStepTextBox
            // 
            this.scaleStepTextBox.Location = new System.Drawing.Point(81, 184);
            this.scaleStepTextBox.Name = "scaleStepTextBox";
            this.scaleStepTextBox.Size = new System.Drawing.Size(37, 20);
            this.scaleStepTextBox.TabIndex = 18;
            this.scaleStepTextBox.Text = "10";
            // 
            // scaleTextBox
            // 
            this.scaleTextBox.Location = new System.Drawing.Point(24, 212);
            this.scaleTextBox.Name = "scaleTextBox";
            this.scaleTextBox.Size = new System.Drawing.Size(53, 20);
            this.scaleTextBox.TabIndex = 20;
            this.scaleTextBox.Leave += new System.EventHandler(this.scaleTextBox_Leave);
            // 
            // scaleAddbutton
            // 
            this.scaleAddbutton.Location = new System.Drawing.Point(81, 210);
            this.scaleAddbutton.Name = "scaleAddbutton";
            this.scaleAddbutton.Size = new System.Drawing.Size(17, 23);
            this.scaleAddbutton.TabIndex = 21;
            this.scaleAddbutton.Text = "+";
            this.scaleAddbutton.UseVisualStyleBackColor = true;
            this.scaleAddbutton.Click += new System.EventHandler(this.scaleAddbutton_Click);
            // 
            // scaleSubButton
            // 
            this.scaleSubButton.Location = new System.Drawing.Point(101, 210);
            this.scaleSubButton.Name = "scaleSubButton";
            this.scaleSubButton.Size = new System.Drawing.Size(17, 23);
            this.scaleSubButton.TabIndex = 22;
            this.scaleSubButton.Text = "-";
            this.scaleSubButton.UseVisualStyleBackColor = true;
            this.scaleSubButton.Click += new System.EventHandler(this.scaleSubButton_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(134, 80);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "Orientation";
            // 
            // orientationStepTextBox
            // 
            this.orientationStepTextBox.Location = new System.Drawing.Point(198, 77);
            this.orientationStepTextBox.Name = "orientationStepTextBox";
            this.orientationStepTextBox.Size = new System.Drawing.Size(37, 20);
            this.orientationStepTextBox.TabIndex = 24;
            this.orientationStepTextBox.Text = "45";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(134, 108);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(28, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Yaw";
            // 
            // yawAddButton
            // 
            this.yawAddButton.Location = new System.Drawing.Point(198, 102);
            this.yawAddButton.Name = "yawAddButton";
            this.yawAddButton.Size = new System.Drawing.Size(17, 23);
            this.yawAddButton.TabIndex = 27;
            this.yawAddButton.Text = "+";
            this.yawAddButton.UseVisualStyleBackColor = true;
            this.yawAddButton.Click += new System.EventHandler(this.yawAddButton_Click);
            // 
            // yawSubButton
            // 
            this.yawSubButton.Location = new System.Drawing.Point(218, 102);
            this.yawSubButton.Name = "yawSubButton";
            this.yawSubButton.Size = new System.Drawing.Size(17, 23);
            this.yawSubButton.TabIndex = 28;
            this.yawSubButton.Text = "-";
            this.yawSubButton.UseVisualStyleBackColor = true;
            this.yawSubButton.Click += new System.EventHandler(this.yawSubButton_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(134, 134);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 13);
            this.label9.TabIndex = 29;
            this.label9.Text = "Pitch";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(134, 161);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(25, 13);
            this.label10.TabIndex = 30;
            this.label10.Text = "Roll";
            // 
            // pitchAddButton
            // 
            this.pitchAddButton.Location = new System.Drawing.Point(198, 129);
            this.pitchAddButton.Name = "pitchAddButton";
            this.pitchAddButton.Size = new System.Drawing.Size(17, 23);
            this.pitchAddButton.TabIndex = 31;
            this.pitchAddButton.Text = "+";
            this.pitchAddButton.UseVisualStyleBackColor = true;
            this.pitchAddButton.Click += new System.EventHandler(this.pitchAddButton_Click);
            // 
            // pitchSubButton
            // 
            this.pitchSubButton.Location = new System.Drawing.Point(218, 129);
            this.pitchSubButton.Name = "pitchSubButton";
            this.pitchSubButton.Size = new System.Drawing.Size(17, 23);
            this.pitchSubButton.TabIndex = 32;
            this.pitchSubButton.Text = "-";
            this.pitchSubButton.UseVisualStyleBackColor = true;
            this.pitchSubButton.Click += new System.EventHandler(this.pitchSubButton_Click);
            // 
            // rollAddButton
            // 
            this.rollAddButton.Location = new System.Drawing.Point(198, 156);
            this.rollAddButton.Name = "rollAddButton";
            this.rollAddButton.Size = new System.Drawing.Size(17, 23);
            this.rollAddButton.TabIndex = 33;
            this.rollAddButton.Text = "+";
            this.rollAddButton.UseVisualStyleBackColor = true;
            this.rollAddButton.Click += new System.EventHandler(this.rollAddButton_Click);
            // 
            // rollSubButton
            // 
            this.rollSubButton.Location = new System.Drawing.Point(218, 156);
            this.rollSubButton.Name = "rollSubButton";
            this.rollSubButton.Size = new System.Drawing.Size(17, 23);
            this.rollSubButton.TabIndex = 34;
            this.rollSubButton.Text = "-";
            this.rollSubButton.UseVisualStyleBackColor = true;
            this.rollSubButton.Click += new System.EventHandler(this.rollSubButton_Click);
            // 
            // resetOrientationButton
            // 
            this.resetOrientationButton.Location = new System.Drawing.Point(137, 182);
            this.resetOrientationButton.Name = "resetOrientationButton";
            this.resetOrientationButton.Size = new System.Drawing.Size(98, 23);
            this.resetOrientationButton.TabIndex = 35;
            this.resetOrientationButton.Text = "Reset Orientation";
            this.resetOrientationButton.UseVisualStyleBackColor = true;
            this.resetOrientationButton.Click += new System.EventHandler(this.resetOrientationButton_Click);
            // 
            // ModifyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.resetOrientationButton);
            this.Controls.Add(this.rollSubButton);
            this.Controls.Add(this.rollAddButton);
            this.Controls.Add(this.pitchSubButton);
            this.Controls.Add(this.pitchAddButton);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.yawSubButton);
            this.Controls.Add(this.yawAddButton);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.orientationStepTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.scaleSubButton);
            this.Controls.Add(this.scaleAddbutton);
            this.Controls.Add(this.scaleTextBox);
            this.Controls.Add(this.scaleStepTextBox);
            this.Controls.Add(this.locationStepTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.zSubButton);
            this.Controls.Add(this.zAddButton);
            this.Controls.Add(this.ySubButton);
            this.Controls.Add(this.yAddButton);
            this.Controls.Add(this.xSubButton);
            this.Controls.Add(this.xAddButton);
            this.Controls.Add(this.zTextBox);
            this.Controls.Add(this.yTextBox);
            this.Controls.Add(this.xTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.typeNameTextBox);
            this.Controls.Add(this.objectNameTextBox);
            this.Controls.Add(this.label1);
            this.Name = "ModifyControl";
            this.Size = new System.Drawing.Size(238, 239);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox objectNameTextBox;
        private System.Windows.Forms.TextBox typeNameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox xTextBox;
        private System.Windows.Forms.TextBox yTextBox;
        private System.Windows.Forms.TextBox zTextBox;
        private System.Windows.Forms.Button xAddButton;
        private System.Windows.Forms.Button xSubButton;
        private System.Windows.Forms.Button yAddButton;
        private System.Windows.Forms.Button ySubButton;
        private System.Windows.Forms.Button zAddButton;
        private System.Windows.Forms.Button zSubButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox locationStepTextBox;
        private System.Windows.Forms.TextBox scaleStepTextBox;
        private System.Windows.Forms.TextBox scaleTextBox;
        private System.Windows.Forms.Button scaleAddbutton;
        private System.Windows.Forms.Button scaleSubButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox orientationStepTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button yawAddButton;
        private System.Windows.Forms.Button yawSubButton;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button pitchAddButton;
        private System.Windows.Forms.Button pitchSubButton;
        private System.Windows.Forms.Button rollAddButton;
        private System.Windows.Forms.Button rollSubButton;
        private System.Windows.Forms.Button resetOrientationButton;
    }
}
