using System;
using System.Collections.Generic;

using System.Text;
using MXP;

namespace MXTank
{
    /// <summary>
    /// RemoteBubble is class representing remote bubbles connected to tank.
    /// </summary>
    public class RemoteBubble : Bubble
    {
        public string BubbleAssetCacheUrl; // 55
        public string BubbleServerAddress; // 40
        public uint BubbleServerPort; // 4
        public float[] BubbleCenter = new float[3]; // 12
        public ulong BubbleRealTime;

        public Session Session;

        public RemoteBubble(Guid bubbleId,
            string bubbleName,
            string cloudUrl,
            Guid ownerId,
            string bubbleServerAddress,
            uint bubbleServerPort,
            float bubbleCenterX,
            float bubbleCenterY,
            float bubbleCenterZ,
            float bubbleRange,
            float bubblePerceptionRange,
            ulong bubbleRealTime,
            Session session) : base(bubbleId,bubbleName,ownerId,bubbleRange,bubblePerceptionRange)
        {
            this.BubbleAssetCacheUrl = cloudUrl;
            this.BubbleServerAddress = bubbleServerAddress;
            this.BubbleServerPort = bubbleServerPort;
            this.BubbleCenter[0] = bubbleCenterX;
            this.BubbleCenter[1] = bubbleCenterY;
            this.BubbleCenter[2] = bubbleCenterZ;
            this.BubbleRealTime=bubbleRealTime;
            this.Session = session;
        }

    }
}
