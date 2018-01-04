using System;
using System.Collections.Generic;

using System.Text;

namespace MXP
{
    public static class MxpConstants
    {

        public const int MaxPacketSize = 1500;
        public const int MaxFrameDataSize = 255;
        public const byte ProtocolMajorVersion = 0;
        public const byte ProtocolMinorVersion = 5;
        public static readonly uint ProtocolSourceRevision = Convert.ToUInt32("$Revision: 529 $".Split(' ')[1]);
        public const int TimeOutSeconds = 5;
        public const int MaxIdleTimeSecods = 3;
        public const int MaxAcknowledgeWaitTimeMilliSeconds = 500;
        public const int MaxResendCount = 10;
        public const int MaxPacketsWaitingAcknowledge = 10000;
        public const int BubbleToBubbleConnectDelaySeconds = 3;
        public const int BubbleToBubbleDisconnectDelaySeconds = 3;
        public const int DefaultServerPort = 1253;
        public const int DefaultHubPort = 1254;
        public static readonly Guid MxpNamespaceId = new Guid("C46991DC-2C29-46c9-9A45-BB8DABF2898C");

    }
}
