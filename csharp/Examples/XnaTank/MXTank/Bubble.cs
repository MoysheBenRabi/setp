using System;
using System.Collections.Generic;

using System.Text;
using MXP;

namespace MXTank
{
    /// <summary>
    /// Bubble is shared base class containing bubble information shared by tank and remote bubbles.
    /// </summary>
    public class Bubble
    {
        public Guid BubbleId; // 16
        public string BubbleName; // 40
        public Guid OwnerId; // 16
        public float BubbleRange; // 4
        public float BubblePerceptionRange; // 4

        public Bubble(
            Guid bubbleId,
            string bubbleName,
            Guid ownerId,
            float bubbleRange,
            float bubblePerceptionRange
            )
        {
            this.BubbleId = bubbleId;
            this.BubbleName = bubbleName;
            this.OwnerId = ownerId;
            this.BubbleRange = bubbleRange;
            this.BubblePerceptionRange = bubblePerceptionRange;
        }

    }
}
