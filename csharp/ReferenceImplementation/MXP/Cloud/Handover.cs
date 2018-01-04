using System;
using System.Collections.Generic;
using System.Text;

namespace MXP.Cloud
{
    public class Handover
    {
        public Guid RemoteBubbleId;
        public Guid ObjectId;
        public Guid ParticipantId;
        public CloudObject CloudObject;
        public DateTime Started;
        public DateTime Completed;
        public uint IdentityRequestMessageId;
        public uint HandoverRequestMessageId;
    }
}
