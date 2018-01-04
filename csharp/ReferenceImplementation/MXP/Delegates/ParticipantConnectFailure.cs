using System;
using System.Collections.Generic;

using System.Text;
using MXP.Messages;
using System.Runtime.Remoting.Messaging;

namespace MXP.Delegates
{
    public delegate void ParticipantConnectFailure(Session session, Message message);
}
