﻿using System;
using System.Collections.Generic;

using System.Text;
using MXP.Messages;
using System.Runtime.Remoting.Messaging;

namespace MXP.Delegates
{
    public delegate void ParticipantConnected(Session session, JoinRequestMessage message, Guid participantId, Guid avatarId);
}
