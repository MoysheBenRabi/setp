using System;
using System.Collections.Generic;

using System.Text;
using MXP.Util;

namespace MXP.Fragments
{

    public class ActionFragment : SplittableFragment
    {
        public string ActionName; // 20
        public Guid SourceObjectId = Guid.Empty; // 16
        public float ObservationRadius; //4
        public string ExtensionDialect; // 4
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
        private byte[] ExtensionData; //X

        public override string ToString()
        {
            String str = "ObjectFragmet [" +
            "ActionName: " + ActionName +
            "SourceObjectId: " + SourceObjectId +
            "ObservationRadius: " + ObservationRadius +
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


        private int InternalDataPrefixSize = 50;
        private int TotalDataPrefixSize = 0;
        private int FrameDataPrefixSize = 0;

        public ActionFragment(int frameDataPrefixSize)
        {
            this.FrameDataPrefixSize = frameDataPrefixSize;
            this.TotalDataPrefixSize = frameDataPrefixSize + InternalDataPrefixSize;
        }

        public void SetExtensionData(byte[] data)
        {
            ExtensionData = data;
            extensionLength = (uint) data.Length;
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
            ActionName = null;
            SourceObjectId = Guid.Empty;
            ObservationRadius = 0;
            ExtensionDialect = null;
            ExtensionDialectMajorVersion = 0;
            ExtensionDialectMinorVersion = 0;
            extensionLength = 0;
            ExtensionData = null;
            FrameCount=1;
        }

        public override int EncodeFragmentData(int frameIndex, byte[] packetBytes, int startIndex)
        {
            int currentIndex = startIndex;

            if (frameIndex == 0)
            {
                currentIndex = EncodeUtil.Encode(ref ActionName, packetBytes, currentIndex, 20);
                currentIndex = EncodeUtil.Encode(ref SourceObjectId, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(ref ObservationRadius, packetBytes, currentIndex);

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
                currentIndex = EncodeUtil.Decode(ref ActionName, packetBytes, currentIndex, 20);
                currentIndex = EncodeUtil.Decode(ref SourceObjectId, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Decode(ref ObservationRadius, packetBytes, currentIndex);


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
