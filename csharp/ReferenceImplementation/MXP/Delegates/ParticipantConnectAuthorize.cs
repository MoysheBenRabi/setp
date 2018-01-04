using System;
using System.Collections.Generic;

using System.Text;
using MXP.Messages;

namespace MXP.Delegates
{
    public delegate bool ParticipantConnectAuthorize(Session session,JoinRequestMessage message, out Guid participantId, out Guid avatarId);
}
