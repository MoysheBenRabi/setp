using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DaemonLogic;

namespace CloudDaemonWeb.Controls.Bubbles
{
    public partial class MyBubbleLinks : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void AddBubbleLink_Click(object sender, EventArgs e)
        {
            Guid bubbleId = new Guid(Request["BubbleId"]);
            Bubble bubble = BubbleLogic.GetBubble(bubbleId);
            BubbleLink bubbleLink = BubbleLogic.AddBubleLink(DaemonHttpContext.LoggedInParticipant,bubble);
            Response.Redirect(Request.Url.OriginalString);
        }
    }
}