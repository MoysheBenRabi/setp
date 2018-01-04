using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using MXP.Util;
using MXP.Fragments;
using System.IO;
using ProtoBuf;

namespace MXP.Messages
{
    public class InjectRequestMessage : Message
    {

        public InjectRequestMessage()
        {
            TypeCode = 14;
            FrameCount = 1;
            Quaranteed = true;
        }

        public void SetExtension<ExtensionFragment>(ExtensionFragment extensionFragment)
        {
            using (MemoryStream bufferStream = new MemoryStream(0))
            {
                Serializer.Serialize(bufferStream, extensionFragment);
                byte[] bufferBytes = new byte[bufferStream.Length];
                Array.Copy(bufferStream.GetBuffer(), bufferBytes, bufferStream.Length);
                ObjectFragment.SetExtensionData(bufferBytes);
                ObjectFragment.ExtensionDialect = "GPB";
            }
        }

        public ExtensionFragment GetExtension<ExtensionFragment>()
        {
            if (ObjectFragment.ExtensionDialect != "GPB")
            {
                throw new Exception("State dialect not Google Protocol Buffers (GPB): " + ObjectFragment.ExtensionDialect);
            }
            using (MemoryStream memoryStream = new MemoryStream(ObjectFragment.GetExtensionData(), 0, (int)ObjectFragment.ExtensionLength))
            {
                ExtensionFragment extensionFragment = Serializer.Deserialize<ExtensionFragment>(memoryStream);
                return extensionFragment;
            }
        }

        public bool HasExtension
        {
            get
            {
                return ObjectFragment.ExtensionDialect == "GPB";
            }
        }

        private const byte DataPrefixSize = 0;

        public override byte FrameDataSize(int frameIndex)
        {
            if (frameIndex == 0)
            {
                return (byte)(DataPrefixSize + ObjectFragment.FragmentDataSize(frameIndex));
            }
            else
            {
                return ObjectFragment.FragmentDataSize(frameIndex);
            }
        }

        public ObjectFragment ObjectFragment = new ObjectFragment(DataPrefixSize);

        #region IMessage Members

        public override void Clear()
        {
            ObjectFragment.Clear();
            FrameCount = ObjectFragment.FrameCount;
            base.Clear();
        }

        public override void PrepareEncoding()
        {
            FrameCount = ObjectFragment.FrameCount;
        }

        public override int EncodeFrameData(int frameIndex, byte[] packetBytes, int startIndex)
        {
            int currentIndex = startIndex;
            // If ObjectFragment frame count has been set directly update the message frame count.
            /*if (FrameCount != ObjectFragment.FrameCount)
            {
                FrameCount = ObjectFragment.FrameCount;
            }*/
            currentIndex = ObjectFragment.EncodeFragmentData(frameIndex, packetBytes, currentIndex);

            //FramesEncoded++;
            return currentIndex;
        }

        public override int DecodeFrameData(int frameIndex, byte[] packetBytes, int startIndex, int length)
        {
            int currentIndex = startIndex;

            currentIndex = ObjectFragment.DecodeFragmentData(frameIndex, packetBytes, currentIndex);
            FrameCount = ObjectFragment.FrameCount; // refreshing message frame count from object fragment

            //FramesDecoded++;
            return currentIndex;
        }

        #endregion

    }
}
