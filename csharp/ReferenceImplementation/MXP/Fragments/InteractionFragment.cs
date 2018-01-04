using System;
using System.Collections.Generic;

using System.Text;
using MXP.Util;

namespace MXP.Fragments
{
    /*
20 : string   : interaction_name
16 : uuid     : source_participant_id
16 : uuid     : source_object_id
16 : uuid     : target_participant_id
16 : uuid     : target_object_id
4  : string   : interaction_payload_dialect
4  : uint     : interaction_payload_length
X  : data     : interaction_payload_data
*/

    public class InteractionFragment : SplittableFragment
    {
        public string InteractionName; // 20
        public Guid SourceParticipantId = Guid.Empty; // 16
        public Guid SourceObjectId = Guid.Empty; // 16
        public Guid TargetParticipantId = Guid.Empty; // 16
        public Guid TargetObjectId = Guid.Empty; // 16
        public string ExtensionDialect=""; // 4
        public byte ExtensionDialectMajorVersion; // 1
        public byte ExtensionDialectMinorVersion; // 1
        private uint extensionLength; // 4
        public uint ExtensionLength
        {
            get
            {
                return extensionLength;
            }
        }
        private byte[] ExtensionData=new byte[0]; //X

        public override string ToString()
        {
            String str = "ObjectFragmet [" +
            "InteractionName: " + InteractionName +
            "SourceParticipantId: " + SourceParticipantId +
            "SourceObjectId: " + SourceObjectId +
            "TargetParticipantId: " + TargetParticipantId +
            "TargetObjectId: " + TargetObjectId +
            ",ExtensionDialect: " + ExtensionDialect +
            ",ExtensionDialectMajorVersion: " + ExtensionDialectMajorVersion +
            ",ExtensionDialectMinorVersion: " + ExtensionDialectMinorVersion +
            ",ExtensionLength: " + extensionLength +
            ",ExtensionData: ";

            if (ExtensionData != null)
            {
                for (int i = 0; i < ExtensionData.Length; i++)
                {
                    if (i > 0)
                    {
                        str += ",";
                    }
                    str += ExtensionData[i];
                }
            }

            str+="]";

            return str;
        }


        private int InternalDataPrefixSize = 94;
        private int TotalDataPrefixSize = 0;
        private int FrameDataPrefixSize = 0;

        public InteractionFragment(int frameDataPrefixSize)
        {
            this.FrameDataPrefixSize = frameDataPrefixSize;
            this.TotalDataPrefixSize = frameDataPrefixSize + InternalDataPrefixSize;
        }

        public void SetExtensionData(byte[] data)
        {
            ExtensionData = data;
            extensionLength = (uint)data.Length;
            FrameCount = (ushort)Math.Ceiling(((double)TotalDataPrefixSize + (double)ExtensionData.Length) / MxpConstants.MaxFrameDataSize);
        }

        public byte[] GetExtensionData()
        {
            return ExtensionData;
        }

        public override byte FragmentDataSize(int frameIndex)
        {
            if (frameIndex == 0)
            {
                return (byte)Math.Min(InternalDataPrefixSize + ExtensionData.Length, MxpConstants.MaxFrameDataSize - FrameDataPrefixSize);
            }
            else
            {
                return (byte)Math.Min(TotalDataPrefixSize + ExtensionData.Length - frameIndex * MxpConstants.MaxFrameDataSize, MxpConstants.MaxFrameDataSize);
            }
        }

        public override void Clear()
        {
            InteractionName = null;
            SourceParticipantId = Guid.Empty;
            SourceObjectId = Guid.Empty;
            TargetParticipantId = Guid.Empty;
            TargetObjectId = Guid.Empty;
            ExtensionDialect = "";
            ExtensionDialectMajorVersion = 0;
            ExtensionDialectMinorVersion = 0;
            extensionLength = 0;
            ExtensionData = new byte[0];
            FrameCount=1;
        }

        public override int EncodeFragmentData(int frameIndex, byte[] packetBytes, int startIndex)
        {
            int currentIndex = startIndex;

            if (frameIndex == 0)
            {
                currentIndex = EncodeUtil.Encode(ref InteractionName, packetBytes, currentIndex, 20);
                currentIndex = EncodeUtil.Encode(ref SourceParticipantId, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(ref SourceObjectId, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(ref TargetParticipantId, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(ref TargetObjectId, packetBytes, currentIndex);

                currentIndex = EncodeUtil.Encode(ref ExtensionDialect, packetBytes, currentIndex, 4);
                currentIndex = EncodeUtil.Encode(ref ExtensionDialectMajorVersion, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(ref ExtensionDialectMinorVersion, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(ref extensionLength, packetBytes, currentIndex);

                currentIndex = EncodeUtil.Encode(ref ExtensionData, 0, packetBytes, currentIndex, FragmentDataSize(frameIndex) - InternalDataPrefixSize);
            }
            else
            {
                currentIndex = EncodeUtil.Encode(ref ExtensionData, frameIndex * MxpConstants.MaxFrameDataSize - TotalDataPrefixSize, packetBytes, currentIndex, FragmentDataSize(frameIndex));
            }
             
            return currentIndex;
        }

        public override int DecodeFragmentData(int frameIndex, byte[] packetBytes, int startIndex)
        {
            int currentIndex = startIndex;

            if (frameIndex == 0)
            {
                currentIndex = EncodeUtil.Decode(ref InteractionName, packetBytes, currentIndex, 20);
                currentIndex = EncodeUtil.Decode(ref SourceParticipantId, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Decode(ref SourceObjectId, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Decode(ref TargetParticipantId, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Decode(ref TargetObjectId, packetBytes, currentIndex);

                currentIndex = EncodeUtil.Decode(ref ExtensionDialect, packetBytes, currentIndex, 4);
                currentIndex = EncodeUtil.Decode(ref ExtensionDialectMajorVersion, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Decode(ref ExtensionDialectMinorVersion, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Decode(ref extensionLength, packetBytes, currentIndex);
                
                SetExtensionData(new byte[extensionLength]);

                currentIndex = EncodeUtil.Decode(ref ExtensionData, 0, packetBytes, currentIndex, FragmentDataSize(frameIndex) - InternalDataPrefixSize);
            }
            else
            {
                currentIndex = EncodeUtil.Decode(ref ExtensionData, frameIndex * MxpConstants.MaxFrameDataSize - TotalDataPrefixSize, packetBytes, currentIndex, FragmentDataSize(frameIndex));
            }

            return currentIndex;
        }

    }
}
