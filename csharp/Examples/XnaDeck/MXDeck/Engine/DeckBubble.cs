using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MXDeck.Engine
{
    public class DeckBubble
    {
        public Guid BubbleId=Guid.Empty;
        public string BubbleName="";
        public string CloudUrl = "";
        public Guid OwnerId=Guid.Empty;
        public float BubbleRange=0;
        public float BubblePerceptionRange=0;
        public String ServerProgramName = "";
        public int ServerProgramMajorVersion = -1;
        public int ServerProgramMinorVersion = -1;
        public int ServerProtocolMajorVersion = -1;
        public int ServerProtocolMinorVersion = -1;
        public int ServerProtocolSourceRevision = -1;

        public Dictionary<uint, DeckObject> Objects = new Dictionary<uint, DeckObject>();
        public Dictionary<Guid, DeckObject> IdObjectDictionary = new Dictionary<Guid, DeckObject>();

        public DeckBubble()
        {
        }

        public void Process(float timeDelta)
        {
            foreach(DeckObject obj in Objects.Values)
            {
                obj.InterpolateLocation(timeDelta);
            }
        }

    }
}
