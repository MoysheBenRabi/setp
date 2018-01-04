using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DaemonLogic;
using System.Net;

namespace CloudDaemonWeb.Controls.Processes
{
    public partial class MyLocalProcesses : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ReserveProcessButton_Click(object sender, EventArgs e)
        {
            LocalProcessLogic.ReserveLocalProcess(DaemonHttpContext.LoggedInParticipant,Dns.GetHostAddresses(Request.Url.Host)[0].ToString());
            Response.Redirect(Request.Url.OriginalString);
        }
    }
}