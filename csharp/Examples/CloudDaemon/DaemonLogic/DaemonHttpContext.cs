using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DaemonLogic
{
    public class DaemonHttpContext
    {
        public static Participant LoggedInParticipant
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return (Participant)HttpContext.Current.Session["Participant"];
                }
                else
                {
                    HttpContext.Current.Session["Participant"] = null;
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["Participant"] = value;
                HttpContext.Current.Session["ParticipantId"] = value.ParticipantId;
            }
        }
   }
}
