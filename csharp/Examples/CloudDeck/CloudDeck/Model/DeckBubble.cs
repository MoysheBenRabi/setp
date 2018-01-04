using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MXP.Common.Proto;

namespace CloudDeck.Model
{
    public class DeckBubble
    {
        public Guid BubbleId;
        public String Name;
        public String Address;
        public float Range;
        public uint Port;
        public String WebUrl;
        public MsdVector3f Center;
        public int RenderId;
    }
}
