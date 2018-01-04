using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DaemonLogic;

namespace CloudDaemonWeb.Controls.Objects
{
    public partial class MyObjectTypes : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void AddObjectTypeButton_Click(object sender, EventArgs e)
        {
            ObjectType objectType = ObjectLogic.AddObjectType(DaemonHttpContext.LoggedInParticipant);
            Response.Redirect(Request.Url.OriginalString);
        }
    }
}