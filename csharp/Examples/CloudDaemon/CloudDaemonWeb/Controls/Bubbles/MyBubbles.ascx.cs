﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DaemonLogic;

namespace CloudDaemonWeb.Controls.Bubbles
{
    public partial class MyBubbles : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void AddBubbleButton_Click(object sender, EventArgs e)
        {
            if (LocalProcessList.SelectedValue == null || LocalProcessList.SelectedValue == "")
            {
                return;
            }
            Guid localProcessId = new Guid(LocalProcessList.SelectedValue);
            LocalProcess localProcess = LocalProcessLogic.GetLocalProcess(localProcessId);
            Bubble bubble = BubbleLogic.AddBuble(DaemonHttpContext.LoggedInParticipant, localProcess);
            Response.Redirect(Request.Url.OriginalString);

        }
    }
}