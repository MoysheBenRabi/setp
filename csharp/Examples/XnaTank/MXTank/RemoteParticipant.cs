using System;
using System.Collections.Generic;

using System.Text;
using MXP;

namespace MXTank
{
    public class RemoteParticipant
    {
        public Guid ParticipantId;
        public string ParticipantName;
        public Session Session;

        public RemoteParticipant(Guid participantId, string participantName, Session session)
        {
            this.ParticipantId = participantId;
            this.ParticipantName = participantName;
            this.Session = session;
        }

    }
}
