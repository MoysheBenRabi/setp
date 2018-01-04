using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DaemonLogic;

namespace CloudDaemonWeb.Controls.Objects
{
    public partial class MyObjects : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void AddObjectButton_Click(object sender, EventArgs e)
        {
            if(ObjectTypeDropDownList.SelectedValue==null||ObjectTypeDropDownList.SelectedValue.Length==0)
            {
                return;
            }
            if (BubbleDropDownList.SelectedValue == null || BubbleDropDownList.SelectedValue.Length == 0)
            {
                return;
            }
            ObjectType objectType = ObjectLogic.GetObjectType(new Guid(ObjectTypeDropDownList.SelectedValue));
            Bubble bubble = BubbleLogic.GetBubble(new Guid(BubbleDropDownList.SelectedValue));
            CloudObject cloudObject = ObjectLogic.AddObject(DaemonHttpContext.LoggedInParticipant, objectType, bubble);
            Response.Redirect(Request.Url.OriginalString);
        }
    }
}