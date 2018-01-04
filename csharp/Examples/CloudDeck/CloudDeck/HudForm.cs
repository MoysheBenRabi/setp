using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CloudDeck
{
    public partial class HudForm : Form
    {
        public HudForm()
        {
            InitializeComponent();
        }

        public bool ModifyControlVisible
        {
            get
            {
                return modifyControl1.Visible;
            }
            set
            {
                modifyControl1.Visible = value;
            }
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            DeckProgram.CloudView.Disconnect();
        }

        public void SetBubbleInformation(string bubbleInformation)
        {
            bubbleInformationLabel.Text = bubbleInformation;
        }

        public void SetUrlLabel(string bubbleUrl)
        {
            urlLabel.Text = bubbleUrl;
        }

        private void chatButton_Click(object sender, EventArgs e)
        {
            chatControl1.Visible = !chatControl1.Visible;
        }

        private void selectionButton_Click(object sender, EventArgs e)
        {
            targetControl1.Visible = !targetControl1.Visible;
        }

        private void buildButton_Click(object sender, EventArgs e)
        {
            buildControl1.Visible = !buildControl1.Visible;
        }

    }
}
