using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DaemonLogic;
using RelyingPartyLogic;

namespace CloudDaemonWeb.Members
{
    public partial class GetLoginSecret : System.Web.UI.Page
    {

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (DaemonHttpContext.LoggedInParticipant == null)
            {
                RelyingPartyLogic.User user = Database.LoggedInUser;
                if (user != null)
                {
                    foreach (AuthenticationToken token in from t in Database.DataContext.AuthenticationTokens where t.User.UserId == user.UserId select t)
                    {
                        if (token.ClaimedIdentifier != null)
                        {
                            Participant participant = ParticipantLogic.AttachParticipantProfileToOpenIdIdentity(user.UserId, token.ClaimedIdentifier);
                            DaemonHttpContext.LoggedInParticipant = participant;
                        }
                    }
                }
            }
            if (DaemonHttpContext.LoggedInParticipant != null)
            {
                Participant participant = DaemonHttpContext.LoggedInParticipant;
                ParticipantLogic.GetLoginSecret(participant);
                Page.Items["goto"] = Request["goto"];
                Page.Items["participantIdentifier"] = participant.ParticipantId.ToString("N");
                Page.Items["loginSecret"] = participant.LoginSecret;
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }
    }
}
