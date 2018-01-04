namespace CloudDeck.Controls
{
    partial class BuildControl
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
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            this.insertObjectButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.loadTypesButton = new System.Windows.Forms.Button();
            this.deleteObjectButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Build";
            // 
            // typeComboBox
            // 
            this.typeComboBox.FormattingEnabled = true;
            this.typeComboBox.Location = new System.Drawing.Point(43, 33);
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.Size = new System.Drawing.Size(171, 21);
            this.typeComboBox.TabIndex = 2;
            // 
            // insertObjectButton
            // 
            this.insertObjectButton.Location = new System.Drawing.Point(220, 32);
            this.insertObjectButton.Name = "insertObjectButton";
            this.insertObjectButton.Size = new System.Drawing.Size(96, 23);
            this.insertObjectButton.TabIndex = 3;
            this.insertObjectButton.Text = "Insert Object";
            this.insertObjectButton.UseVisualStyleBackColor = true;
            this.insertObjectButton.Click += new System.EventHandler(this.insertObjectButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Type";
            // 
            // loadTypesButton
            // 
            this.loadTypesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loadTypesButton.Location = new System.Drawing.Point(360, 4);
            this.loadTypesButton.Name = "loadTypesButton";
            this.loadTypesButton.Size = new System.Drawing.Size(54, 23);
            this.loadTypesButton.TabIndex = 5;
            this.loadTypesButton.Text = "Refresh";
            this.loadTypesButton.UseVisualStyleBackColor = true;
            this.loadTypesButton.Click += new System.EventHandler(this.loadTypesButton_Click);
            // 
            // deleteObjectButton
            // 
            this.deleteObjectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteObjectButton.Enabled = false;
            this.deleteObjectButton.Location = new System.Drawing.Point(318, 32);
            this.deleteObjectButton.Name = "deleteObjectButton";
            this.deleteObjectButton.Size = new System.Drawing.Size(96, 23);
            this.deleteObjectButton.TabIndex = 6;
            this.deleteObjectButton.Text = "Delete Object";
            this.deleteObjectButton.UseVisualStyleBackColor = true;
            this.deleteObjectButton.Click += new System.EventHandler(this.deleteObjectButton_Click);
            // 
            // BuildControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.deleteObjectButton);
            this.Controls.Add(this.loadTypesButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.insertObjectButton);
            this.Controls.Add(this.typeComboBox);
            this.Controls.Add(this.label1);
            this.Name = "BuildControl";
            this.Size = new System.Drawing.Size(417, 60);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox typeComboBox;
        private System.Windows.Forms.Button insertObjectButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button loadTypesButton;
        private System.Windows.Forms.Button deleteObjectButton;
    }
}
