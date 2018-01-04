using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MXP;

namespace DaemonLogic
{
    public class BubbleLogic
    {
        public static Bubble GetBubble(Guid bubbleId)
        {
            using (DaemonEntities entities = new DaemonEntities())
            {
                Bubble bubble = QueryUtil.First<Bubble>((from b in entities.Bubble where b.BubbleId == bubbleId select b));
                entities.Detach(bubble);
                return bubble;
            }
        }

        public static Bubble AddBuble(Participant participant, LocalProcess localProcess)
        {
            using (DaemonEntities entities = new DaemonEntities())
            {
                try
                {
                    entities.Attach(participant);
                    entities.Attach(localProcess);
                    Bubble bubble = new Bubble
                    {
                        BubbleId = Guid.NewGuid(),
                        Participant = participant,
                        LocalProcess = localProcess,
                        Name = "New Bubble",
                        Range = 100,
                        PerceptionRange = 150,
                        Published = false
                    };
                    entities.AddToBubble(bubble);
                    entities.SaveChanges();
                    entities.Detach(bubble);
                    return bubble;
                }
                finally
                {
                    entities.Detach(participant);
                    entities.Detach(localProcess);
                }
            }
        }

        public static BubbleLink AddBubleLink(Participant participant, Bubble bubble)
        {
            using (DaemonEntities entities = new DaemonEntities())
            {
                try
                {
                    entities.Attach(participant);
                    entities.Attach(bubble);
                    if (bubble.Participant != participant)
                    {
                        throw new UnauthorizedAccessException("You are not owner of this bubble.");
                    }
                    BubbleLink bubbleLink = new BubbleLink
                    {
                        BubbleLinkId = Guid.NewGuid(),
                        RemoteBubbleId = Guid.Empty,
                        Bubble = bubble,
                        Name = "New Bubble Link",
                        Address = "127.0.0.1",
                        Port = MxpConstants.DefaultHubPort,
                        X = 50,
                        Y = 50,
                        Z = 0,
                        Enabled = false
                    };
                    entities.AddToBubbleLink(bubbleLink);
                    entities.SaveChanges();
                    entities.Detach(bubbleLink);
                    return bubbleLink;
                }
                finally
                {
                    entities.Detach(bubble);
                    entities.Detach(participant);
                }
            }
        }


    }
}
