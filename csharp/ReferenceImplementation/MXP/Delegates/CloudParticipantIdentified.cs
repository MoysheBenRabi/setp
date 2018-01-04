using System;
using System.Collections.Generic;
using System.Text;
using MXP.Cloud;
using MXP.Messages;

namespace MXP.Delegates
{
    public delegate byte CloudParticipantIdentified(CloudBubble bubble, IdentifyRequestMessage message);
}
