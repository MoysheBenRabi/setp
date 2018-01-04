using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using MXP.Util;
using MXP.Fragments;

namespace MXP.Messages
{

    public class ListBubblesResponse : Message
    {
        
        public List<BubbleFragment> BubbleFragments = new List<BubbleFragment>();

        public ListBubblesResponse()
        {
            TypeCode = 26;
            FrameCount = 1;
            Quaranteed = true;
        }

        public void AddBubbleFragment(BubbleFragment bubbleEntry)
        {
            BubbleFragments.Add(bubbleEntry);
            FrameCount = (ushort) BubbleFragments.Count;
        }

        public override byte FrameDataSize(int frameIndex)
        {
            // Handling empty list.
            if (BubbleFragments.Count == 0)
            {
                return 0;
            }
            return 255;
        }

        #region IMessage Members

        public override void Clear()
        {
            base.Clear();
            BubbleFragments.Clear();
            FrameCount = 1;
        }

        public override int EncodeFrameData(int frameIndex, byte[] packetBytes, int startIndex)
        {
            int currentIndex = startIndex;

            // Handling empty list.
            if (BubbleFragments.Count == 0)
            {
                return currentIndex;
            }

            BubbleFragment entry = BubbleFragments[frameIndex];
            currentIndex = entry.EncodeFragmentData(packetBytes, currentIndex);
            currentIndex += 60; // No need to read the padding.
            return currentIndex;
        }

        public override int DecodeFrameData(int frameIndex, byte[] packetBytes, int startIndex, int length)
        {
            int currentIndex = startIndex;
            
            // Handling empty list.
            if (length == 0)
            {
                return currentIndex;
            }

            BubbleFragment entry = new BubbleFragment();
            currentIndex = entry.DecodeFragmentData(packetBytes, currentIndex);
            currentIndex += 60; // No need to read the padding.
            BubbleFragments.Add(entry);
            return currentIndex;
        }

        public override string ToString()
        {
            String str = GetType().Name + " {";

            foreach (BubbleFragment bubbleEntry in BubbleFragments)
            {
                str += bubbleEntry + " ";
            }

            str += "}";

            return str;
        }

        #endregion

    }
}
