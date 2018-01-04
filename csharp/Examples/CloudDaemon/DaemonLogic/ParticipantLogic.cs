using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jug;
using MXP;

namespace DaemonLogic
{
    public class ParticipantLogic
    {
        public static Participant AttachParticipantProfileToOpenIdIdentity(int userId, string openIdUrl)
        {
            using (DaemonEntities entities = new DaemonEntities())
            {
                OpenIdUser user = (from u in entities.OpenIdUser where u.UserId == userId select u).First<OpenIdUser>();
                Participant participant = QueryUtil.First<Participant>(from p in entities.Participant where p.User.UserId == user.UserId select p);
                
                // If participant is not attached try to find existing participant with openIdUrl
                if (participant == null)
                {
                    participant = QueryUtil.First<Participant>(from p in entities.Participant where p.OpenIdUrl == openIdUrl select p);
                    if (participant != null)
                    {
                        // If participant is not attached to user then attach it.
                        if (participant.User == null)
                        {
                            participant.User = user;
                            entities.SaveChanges();
                        }
                        else
                        {
                            throw new ArgumentException("Participant exists with the same OpenId but already connected to different user.");
                        }
                    }
                }

                // If participant is still null then create new participant and attach to user.
                if (participant == null)
                {
                    participant = new Participant
                    {                        
                        ParticipantId = new Guid(UUIDGenerator.Current.GenerateNameBasedUUID(new UUID(MxpConstants.MxpNamespaceId.ToString()),openIdUrl).ToString()),
                        OpenIdUrl = openIdUrl,
                        User = user
                    };
                    entities.AddToParticipant(participant);
                    entities.SaveChanges();
                }

                entities.Detach(participant);
                return participant;
            }
        }

        public static void GetLoginSecret(Participant participant)
        {
            using (DaemonEntities entities = new DaemonEntities())
            {
                entities.Attach(participant);
                participant.LoginSecret = Guid.NewGuid().ToString("N");
                participant.LoginSecretExpires = DateTime.Now.Add(new TimeSpan(0, 0, 0, 10));
                entities.SaveChanges();
                entities.Detach(participant);
            }
        }

        public static void ClearLoginSecret(Participant participant)
        {
            using (DaemonEntities entities = new DaemonEntities())
            {
                entities.Attach(participant);
                participant.LoginSecret = null;
                participant.LoginSecretExpires = null;
                entities.SaveChanges();
                entities.Detach(participant);
            }
        }

        public static Participant GetParticipant(Guid participantId)
        {
            using (DaemonEntities entities = new DaemonEntities())
            {
                Participant participant = QueryUtil.First<Participant>(from p in entities.Participant where p.ParticipantId == participantId select p);
                entities.Detach(participant);
                return participant;
            }
        }
    }
}
