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
    public class InteractResponseMessage : Message
    {

        public InteractResponseMessage()
        {
            TypeCode = 21;
            FrameCount = 1;
            Quaranteed = true;
        }

        public uint RequestMessageId = 0; // 4
        public byte FailureCode; // 1

        public void SetExtension<ExtensionFragment>(ExtensionFragment extensionFragment)
        {
            using (MemoryStream bufferStream = new MemoryStream(0))
            {
                Serializer.Serialize(bufferStream, extensionFragment);
                byte[] bufferBytes = new byte[bufferStream.Length];
                Array.Copy(bufferStream.GetBuffer(), bufferBytes, bufferStream.Length);
                SetPayloadData(bufferBytes);
                InteractionFragment.ExtensionDialect = "GPB";
            }
        }

        public ExtensionFragment GetExtension<ExtensionFragment>()
        {
            if (InteractionFragment.ExtensionDialect != "GPB")
            {
                throw new Exception("State dialect not Google Protocol Buffers (GPB): " + InteractionFragment.ExtensionDialect);
            }
            using (MemoryStream memoryStream = new MemoryStream(InteractionFragment.GetExtensionData(), 0, (int)InteractionFragment.ExtensionLength))
            {
                ExtensionFragment extensionFragment = Serializer.Deserialize<ExtensionFragment>(memoryStream);
                return extensionFragment;
            }
        }

        public bool HasExtension
        {
            get
            {
                return InteractionFragment.ExtensionDialect == "GPB";
            }
        }

        public void SetPayloadData(byte[] statePayloadData)
        {
            InteractionFragment.SetExtensionData(statePayloadData);
            FrameCount = InteractionFragment.FrameCount;
        }

        public byte[] GetInteractionPayloadData()
        {
            return InteractionFragment.GetExtensionData();
        }

        private const byte DataPrefixSize = 5;

        public override byte FrameDataSize(int frameIndex)
        {
            if (frameIndex == 0)
            {
                return (byte)(DataPrefixSize + InteractionFragment.FragmentDataSize(frameIndex));
            }
            else
            {
                return InteractionFragment.FragmentDataSize(frameIndex);
            }
        }

        public InteractionFragment InteractionFragment = new InteractionFragment(DataPrefixSize);

        #region IMessage Members

        public override void Clear()
        {
            InteractionFragment.Clear();
            FrameCount = InteractionFragment.FrameCount;
            RequestMessageId = 0;
            FailureCode = 0;
            base.Clear();
        }

        public override int EncodeFrameData(int frameIndex, byte[] packetBytes, int startIndex)
        {
            int currentIndex = startIndex;

            if (frameIndex == 0)
            {
                // Response Fragment
                currentIndex = EncodeUtil.Encode(ref RequestMessageId, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(ref FailureCode, packetBytes, currentIndex);
            }

            currentIndex = InteractionFragment.EncodeFragmentData(frameIndex, packetBytes, currentIndex);

            //FramesEncoded++;
            return currentIndex;
        }

        public override int DecodeFrameData(int frameIndex, byte[] packetBytes, int startIndex, int length)
        {
            int currentIndex = startIndex;

            if (frameIndex == 0)
            {
                // Response Fragment
                currentIndex = EncodeUtil.Decode(ref RequestMessageId, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Decode(ref FailureCode, packetBytes, currentIndex);
            }

            currentIndex = InteractionFragment.DecodeFragmentData(frameIndex, packetBytes, currentIndex);
            FrameCount = InteractionFragment.FrameCount; // refreshing message frame count from object fragment

            //FramesDecoded++;
            return currentIndex;
        }

        #endregion

    }
}
