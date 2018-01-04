using System;
using System.Collections.Generic;

using System.Text;
using MXP.Messages;
using System.Runtime.Remoting.Messaging;

namespace MXP.Delegates
{
    public delegate void BubbleConnected(Session session, Message message);
}
