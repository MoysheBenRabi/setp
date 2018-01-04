//-----------------------------------------------------------------------
// <copyright file="Default.aspx.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CloudDaemonWeb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using RelyingPartyLogic;
    using DaemonLogic;

    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User user = Database.LoggedInUser;
            this.Label1.Text = user != null ? HttpUtility.HtmlEncode(user.FirstName) : "<not logged in>";


        }
    }
}
