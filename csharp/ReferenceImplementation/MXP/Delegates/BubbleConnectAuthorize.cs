using System;
using System.Collections.Generic;

using System.Text;
using MXP.Messages;

namespace MXP.Delegates
{
    public delegate bool BubbleConnectAuthorize(Session session,AttachRequestMessage message);
}
