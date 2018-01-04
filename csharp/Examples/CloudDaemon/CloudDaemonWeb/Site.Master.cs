//-----------------------------------------------------------------------
// <copyright file="Site.Master.cs" company="Andrew Arnott">
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

    public partial class Site : System.Web.UI.MasterPage
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (DaemonHttpContext.LoggedInParticipant == null)
            {
                User user = Database.LoggedInUser;
                if (user != null)
                {
                    foreach (AuthenticationToken token in from t in Database.DataContext.AuthenticationTokens where t.User.UserId == user.UserId select t)
                    {
                        if (token.ClaimedIdentifier != null)
                        {
                            Participant participant=ParticipantLogic.AttachParticipantProfileToOpenIdIdentity(user.UserId, token.ClaimedIdentifier);
                            DaemonHttpContext.LoggedInParticipant = participant;
                        }
                    }
                }
            }
        }

    }
}
