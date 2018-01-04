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
    public class ActionEventMessage : Message
    {

        public ActionEventMessage()
        {
            TypeCode = 60;
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
                SetPayloadData(bufferBytes);
                ActionFragment.ExtensionDialect = "GPB";
            }
        }

        public ExtensionFragment GetExtension<ExtensionFragment>()
        {
            if (ActionFragment.ExtensionDialect != "GPB")
            {
                throw new Exception("State dialect not Google Protocol Buffers (GPB): " + ActionFragment.ExtensionDialect);
            }
            using (MemoryStream memoryStream = new MemoryStream(ActionFragment.GetExtensionData(), 0, (int)ActionFragment.ExtensionLength))
            {
                ExtensionFragment extensionFragment = Serializer.Deserialize<ExtensionFragment>(memoryStream);
                return extensionFragment;
            }
        }

        public bool HasExtension
        {
            get
            {
                return ActionFragment.ExtensionDialect == "GPB";
            }
        }

        public void SetPayloadData(byte[] statePayloadData)
        {
            ActionFragment.SetExtensionData(statePayloadData);
            FrameCount = ActionFragment.FrameCount;
        }

        public byte[] GetPayloadData()
        {
            return ActionFragment.GetExtensionData();
        }

        private const byte DataPrefixSize = 0;

        public override byte FrameDataSize(int frameIndex)
        {
            if (frameIndex == 0)
            {
                return (byte)(DataPrefixSize + ActionFragment.FragmentDataSize(frameIndex));
            }
            else
            {
                return ActionFragment.FragmentDataSize(frameIndex);
            }
        }

        public ActionFragment ActionFragment = new ActionFragment(DataPrefixSize);

        #region IMessage Members

        public override void Clear()
        {
            ActionFragment.Clear();
            FrameCount = ActionFragment.FrameCount;
            base.Clear();
        }

        public override int EncodeFrameData(int frameIndex, byte[] packetBytes, int startIndex)
        {
            int currentIndex = startIndex;

            currentIndex = ActionFragment.EncodeFragmentData(frameIndex, packetBytes, currentIndex);

            //FramesEncoded++;
            return currentIndex;
        }

        public override int DecodeFrameData(int frameIndex, byte[] packetBytes, int startIndex, int length)
        {
            int currentIndex = startIndex;

            currentIndex = ActionFragment.DecodeFragmentData(frameIndex, packetBytes, currentIndex);
            FrameCount = ActionFragment.FrameCount; // refreshing message frame count from object fragment

            //FramesDecoded++;
            return currentIndex;
        }

        #endregion

    }
}
