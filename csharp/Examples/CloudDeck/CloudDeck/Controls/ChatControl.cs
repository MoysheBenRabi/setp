using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MXP.Cloud;
using CloudDeck.Model;

namespace CloudDeck.Controls
{
    public partial class ChatControl : UserControl
    {
        public ChatControl()
        {
            InitializeComponent();
            if (DeckProgram.DeckScene != null)
            {
                DeckProgram.DeckScene.ChatMessage += OnChatMessage;
            }
        }

        private void inputTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DeckProgram.DeckEngine.SendChatMessage(inputTextBox.Text);
                inputTextBox.Text = "";
            }
        }

        public void OnChatMessage(DeckObject sourceObject, string message)
        {
            textBox1.Text += sourceObject.ObjectName + " - " + message+Environment.NewLine;
            textBox1.Select(textBox1.Text.Length - 1, 0);
            textBox1.ScrollToCaret();
            if (this.Visible == false)
            {
                this.Visible = true;
            }
        }

    }
}
