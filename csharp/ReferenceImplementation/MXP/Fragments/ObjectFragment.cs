using System;
using System.Collections.Generic;

using System.Text;
using MXP.Util;
using MXP.Common.Proto;

namespace MXP.Fragments
{
    /*
16 : uuid     : object_id 
4  : uint     : object_index 
16 : uuid     : type_id 
16 : uuid     : parent_object_id 
20 : string   : object_name
20 : string   : type_name
16 : uuid     : owner_id 
12 : vector3f : location
12 : vector3f : velocity
12 : vector3f : acceleration
16 : vector4f : orientation
16 : vector4f : angular_velocity
16 : vector4f : angular_acceleration
4  : float    : bounding_sphere_radius
4  : float    : mass
4  : string   : extension_dialect
1  : byte     : extension_dialect_major_version
1  : byte     : extension_dialect_minor_version
4  : uint     : extension_length
X  : data     : extension_data
*/

    public class ObjectFragment : SplittableFragment
    {
        public Guid ObjectId=Guid.Empty; // 16
        public uint ObjectIndex; // 4
        public Guid TypeId = Guid.Empty; // 16
        public Guid ParentObjectId = Guid.Empty; // 16
        public string ObjectName; // 20
        public string TypeName; // 20
        public Guid OwnerId = Guid.Empty; // 16
        public MsdVector3f Location = new  MsdVector3f(); // 12
        public MsdVector3f Velocity = new MsdVector3f(); // 12
        public MsdVector3f Acceleration = new MsdVector3f(); // 12
        public MsdQuaternion4f Orientation = new MsdQuaternion4f(); // 16
        public MsdQuaternion4f AngularVelocity = new MsdQuaternion4f(); // 16
        public MsdQuaternion4f AngularAcceleration = new MsdQuaternion4f(); // 16
        public float BoundingSphereRadius; // 4
        public float Mass; // 4
        public string ExtensionDialect=""; // 4
        public byte ExtensionDialectMajorVersion; // 1
        public byte ExtensionDialectMinorVersion; // 1
        private uint extensionLength; // 4
        private byte[] ExtensionData = new byte[0]; //X

        public uint ExtensionLength
        {
            get
            {
                return extensionLength;
            }
        }

        public override string ToString()
        {
            String str = "ObjectFragmet [" +
            "ObjectId: " + ObjectId +
            ",ParentObjectId: " + ParentObjectId +
            ",ObjectIndex: " + ObjectIndex +
            ",TypeId: " + TypeId +
            ",ObjectName: " + ObjectName +
            ",TypeName: " + TypeName +
            ",OwnerId: " + OwnerId +
            ",Location: " + Location.X+"," +Location.Y+","+Location.Z+
            ",Velocity: " + Velocity.X + "," + Velocity.Y + "," + Velocity.Z +
            ",Acceleration: " + Acceleration.X + "," + Acceleration.Y + "," + Acceleration.Z +
            ",Orientation: " + Orientation.X + "," +Orientation.Y + "," +Orientation.Z + "," +Orientation.W +
            ",AngularVelocity: " + AngularVelocity.X + "," + AngularVelocity.Y + "," + AngularVelocity.Z + "," + AngularVelocity.W +
            ",AngularAcceleration: " + AngularAcceleration.X + "," + AngularAcceleration.Y + "," + AngularAcceleration.Z + "," + AngularAcceleration.W +
            ",BoundingSphereRadius: " + BoundingSphereRadius +
            ",Mass: " + Mass +
            ",ExtensionDialect: " + ExtensionDialect +
            ",ExtensionDialectMajorVersion: " + ExtensionDialectMajorVersion +
            ",ExtensionDialectMinorVersion: " + ExtensionDialectMinorVersion +
            ",ExtensionLength: " + extensionLength +
            ",ExtensionData: ";

            if (ExtensionData != null)
            {
                if (extensionLength < 512)
                {
                    for (int i = 0; i < extensionLength; i++)
                    {
                        if (i > 0)
                        {
                            str += ",";
                        }
                        str += ExtensionData[i];
                    }
                }
                else
                {
                    str += "binary";
                }
            }

            str+="]";

            return str;
        }

        private int InternalDataPrefixSize = 210;
        private int TotalDataPrefixSize = 0;
        private int FrameDataPrefixSize = 0;
        
        public ObjectFragment(int frameDataPrefixSize)
        {
            this.FrameDataPrefixSize = frameDataPrefixSize;
            this.TotalDataPrefixSize = frameDataPrefixSize + InternalDataPrefixSize;
            this.FrameCount = (ushort)Math.Ceiling(((double)TotalDataPrefixSize) / MxpConstants.MaxFrameDataSize);
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
            ObjectId = Guid.Empty;
            ParentObjectId = Guid.Empty;
            ObjectIndex = 0;
            TypeId = Guid.Empty;
            ObjectName = null;
            TypeName = null;
            OwnerId = Guid.Empty;
            Location.X = 0; Location.Y = 0; Location.Z = 0;
            Velocity.X = 0; Velocity.Y = 0; Velocity.Z = 0;
            Acceleration.X = 0; Acceleration.Y = 0; Acceleration.Z = 0;
            Orientation.X = 0; Orientation.Y = 0; Orientation.Z = 0; Orientation.W = 0;
            AngularVelocity.X = 0; AngularVelocity.Y = 0; AngularVelocity.Z = 0; AngularVelocity.W = 0;
            AngularAcceleration.X = 0; AngularAcceleration.Y = 0; AngularAcceleration.Z = 0; AngularAcceleration.W = 0;
            BoundingSphereRadius = 0;
            Mass = 0;
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
                currentIndex = EncodeUtil.Encode(ref ObjectId, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(ref ObjectIndex, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(ref TypeId, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(ref ParentObjectId, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(ref ObjectName, packetBytes, currentIndex, 20);
                currentIndex = EncodeUtil.Encode(ref TypeName, packetBytes, currentIndex, 20);
                currentIndex = EncodeUtil.Encode(ref OwnerId, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(Location.X, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(Location.Y, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(Location.Z, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(Velocity.X, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(Velocity.Y, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(Velocity.Z, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(Acceleration.X, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(Acceleration.Y, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(Acceleration.Z, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(Orientation.X, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(Orientation.Y, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(Orientation.Z, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(Orientation.W, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(AngularVelocity.X, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(AngularVelocity.Y, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(AngularVelocity.Z, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(AngularVelocity.W, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(AngularAcceleration.X, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(AngularAcceleration.Y, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(AngularAcceleration.Z, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(AngularAcceleration.W, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(ref BoundingSphereRadius, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Encode(ref Mass, packetBytes, currentIndex);
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
            float tempFloat = 0;
            if (frameIndex == 0)
            {
                currentIndex = EncodeUtil.Decode(ref ObjectId, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Decode(ref ObjectIndex, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Decode(ref TypeId, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Decode(ref ParentObjectId, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Decode(ref ObjectName, packetBytes, currentIndex, 20);
                currentIndex = EncodeUtil.Decode(ref TypeName, packetBytes, currentIndex, 20);
                currentIndex = EncodeUtil.Decode(ref OwnerId, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
                Location.X = tempFloat;
                currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
                Location.Y = tempFloat;
                currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
                Location.Z = tempFloat;
                currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
                Velocity.X = tempFloat;
                currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
                Velocity.Y = tempFloat;
                currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
                Velocity.Z = tempFloat;
                currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
                Acceleration.X = tempFloat;
                currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
                Acceleration.Y = tempFloat;
                currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
                Acceleration.Z = tempFloat;
                currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
                Orientation.X = tempFloat;
                currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
                Orientation.Y = tempFloat;
                currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
                Orientation.Z = tempFloat;
                currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
                Orientation.W = tempFloat;
                currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
                AngularVelocity.X = tempFloat;
                currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
                AngularVelocity.Y = tempFloat;
                currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
                AngularVelocity.Z = tempFloat;
                currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
                AngularVelocity.W = tempFloat;
                currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
                AngularAcceleration.X = tempFloat;
                currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
                AngularAcceleration.Y = tempFloat;
                currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
                AngularAcceleration.Z = tempFloat;
                currentIndex = EncodeUtil.Decode(ref tempFloat, packetBytes, currentIndex);
                AngularAcceleration.W = tempFloat;
                currentIndex = EncodeUtil.Decode(ref BoundingSphereRadius, packetBytes, currentIndex);
                currentIndex = EncodeUtil.Decode(ref Mass, packetBytes, currentIndex);
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
