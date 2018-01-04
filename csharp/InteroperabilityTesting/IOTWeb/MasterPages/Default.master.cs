using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IOT.MasterPages
{
    public partial class Default : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*Response.AddHeader("Pragma", "no-cache");
            Response.CacheControl = "no-cache";
            Response.Expires=-1;
            Response.Cache.SetMaxAge(new TimeSpan(0, 0, 0));
            Response.ExpiresAbsolute = new DateTime(2000, 1, 1);*/
        }
    }
}
