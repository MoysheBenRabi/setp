using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MXP;

namespace CloudDeck
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        public void SetBubbleLoginAddress(string address)
        {
            webLoginUrlTextBox.Text = address;
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            DeckProgram.MainForm.Close();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            Uri uri = new Uri(bubbleAddressTextBox.Text);
            string remoteAddress = uri.Host;
            int remoteHubPort = uri.IsDefaultPort ? MxpConstants.DefaultServerPort : uri.Port;
            string remoteBubbleString = uri.AbsolutePath.Substring(1);
            Guid bubbleId = Guid.Empty;
            string bubbleName = "";
            try
            {
                bubbleId=new Guid(remoteBubbleString);
            }
            catch (Exception)
            {
                bubbleName = remoteBubbleString;
            }
            DeckProgram.CloudView.Connect(remoteAddress, remoteHubPort, bubbleId, bubbleName, "", "", userIdentifierTextBox.Text, userPassphraseTextBox.Text, DeckProgram.DeckEngine.AvatarId,false);
            DeckProgram.HudForm.SetUrlLabel(uri.AbsoluteUri.Replace("http","mxp"));
        }

        public void ReloadLoginPage()
        {
            if (webLoginUrlTextBox.Text.StartsWith("http"))
            {
                webLoginBrowser.Url = new Uri(webLoginUrlTextBox.Text);
            }
            else
            {
                webLoginBrowser.Url = new Uri("http://"+webLoginUrlTextBox.Text);
            }
        }

        private void webLoginBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            if ("Login Secret".Equals(webLoginBrowser.Document.Title))
            {

                HtmlElement gotoElement = webLoginBrowser.Document.GetElementById("goto");
                HtmlElement participantIdentifierElement = webLoginBrowser.Document.GetElementById("participant-identifier");
                HtmlElement loginSecretElement = webLoginBrowser.Document.GetElementById("login-secret");
                string gotoUrl = gotoElement.GetAttribute("value");
                string participantIdentifier = participantIdentifierElement.GetAttribute("value");
                string loginSecret = loginSecretElement.GetAttribute("value");
                bubbleAddressTextBox.Text = gotoUrl;
                userIdentifierTextBox.Text = participantIdentifier;
                userPassphraseTextBox.Text = loginSecret;
                this.loginButton_Click(null, null);
                webLoginBrowser.Url = null;
            }

        }

        private void bubbleAddressTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void webLoginUrlTextBox_Leave(object sender, EventArgs e)
        {
        }

        private void webLoginUrlTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                ReloadLoginPage();
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
        }

        private void LoginForm_Shown(object sender, EventArgs e)
        {
        }

        private void LoginForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                ReloadLoginPage();
            }
        }

        private void nickTextBox_TextChanged(object sender, EventArgs e)
        {
            DeckProgram.DeckEngine.ParticipantNickName = nickTextBox.Text;
        }

    }
}
