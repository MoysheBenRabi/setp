using System;
using System.Collections.Generic;
using System.Text;
using MXP.Common.Proto;
using MXP.Fragments;
using System.IO;
using ProtoBuf;

namespace MXP.Cloud
{
    /// <summary>
    /// Cache object contains object information and they are stored to the object cache.
    /// </summary>
    public class CloudObject
    {
        public Guid BubbleId = Guid.Empty;

        public DateTime LastUpdated = DateTime.MinValue; // Set by cache
        public uint LocalObjectIndex; // Set by cache

        public Guid ObjectId = Guid.Empty;
        public Guid ParentObjectId = Guid.Empty;
        public uint RemoteObjectIndex;
        public Guid TypeId = Guid.Empty;
        public string ObjectName;
        public string TypeName;
        public Guid OwnerId = Guid.Empty;
        public MsdVector3f Location = new MsdVector3f();
        public MsdVector3f Velocity = new MsdVector3f();
        public MsdVector3f Acceleration = new MsdVector3f();
        public MsdQuaternion4f Orientation = new MsdQuaternion4f();
        public MsdQuaternion4f AngularVelocity = new MsdQuaternion4f();
        public MsdQuaternion4f AngularAcceleration = new MsdQuaternion4f();
        public float BoundingSphereRadius;
        public float Mass;
        public string ExtensionDialect = "";
        public byte ExtensionDialectMajorVersion;
        public byte ExtensionDialectMinorVersion;
        public byte[] ExtensionData = new byte[0];

        public void SetExtension<ExtensionFragment>(ExtensionFragment extensionFragment)
        {
            using (MemoryStream bufferStream = new MemoryStream(0))
            {
                Serializer.Serialize(bufferStream, extensionFragment);
                ExtensionData = bufferStream.GetBuffer();
                ExtensionDialect = "GPB";
            }
        }

        public ExtensionFragment GetExtension<ExtensionFragment>()
        {
            if (ExtensionDialect != "GPB")
            {
                throw new Exception("State dialect not Google Protocol Buffers (GPB): " + ExtensionDialect);
            }
            using (MemoryStream memoryStream = new MemoryStream(ExtensionData, 0, ExtensionData.Length))
            {
                ExtensionFragment extensionFragment = Serializer.Deserialize<ExtensionFragment>(memoryStream);
                return extensionFragment;
            }
        }

        public bool HasExtension
        {
            get
            {
                return ExtensionDialect == "GPB";
            }
        }

        public void FromObjectFragment(Guid bubbleId,ObjectFragment objectFragment)
        {
            this.BubbleId = bubbleId;
            RemoteObjectIndex = objectFragment.ObjectIndex;
            ObjectId = objectFragment.ObjectId;
            ObjectName = objectFragment.ObjectName;
            ParentObjectId = objectFragment.ParentObjectId;
            TypeId = objectFragment.TypeId;
            TypeName = objectFragment.TypeName;
            OwnerId = objectFragment.OwnerId;
            Location.X = objectFragment.Location.X;
            Location.Y = objectFragment.Location.Y;
            Location.Z = objectFragment.Location.Z;
            Velocity.X = objectFragment.Velocity.X;
            Velocity.Y = objectFragment.Velocity.Y;
            Velocity.Z = objectFragment.Velocity.Z;
            Acceleration.X = objectFragment.Acceleration.X;
            Acceleration.Y = objectFragment.Acceleration.Y;
            Acceleration.Z = objectFragment.Acceleration.Z;
            Orientation.X = objectFragment.Orientation.X;
            Orientation.Y = objectFragment.Orientation.Y;
            Orientation.Z = objectFragment.Orientation.Z;
            Orientation.W = objectFragment.Orientation.W;
            AngularVelocity.X = objectFragment.AngularVelocity.X;
            AngularVelocity.Y = objectFragment.AngularVelocity.Y;
            AngularVelocity.Z = objectFragment.AngularVelocity.Z;
            AngularVelocity.W = objectFragment.AngularVelocity.W;
            AngularAcceleration.X = objectFragment.AngularAcceleration.X;
            AngularAcceleration.Y = objectFragment.AngularAcceleration.Y;
            AngularAcceleration.Z = objectFragment.AngularAcceleration.Z;
            AngularAcceleration.W = objectFragment.AngularAcceleration.W;
            BoundingSphereRadius = objectFragment.BoundingSphereRadius;
            Mass = objectFragment.Mass;
            ExtensionDialect = objectFragment.ExtensionDialect;
            ExtensionDialectMajorVersion = objectFragment.ExtensionDialectMajorVersion;
            ExtensionDialectMinorVersion = objectFragment.ExtensionDialectMinorVersion;
            ExtensionData = new byte[objectFragment.ExtensionLength];
            Array.Copy(objectFragment.GetExtensionData(), ExtensionData, objectFragment.ExtensionLength);
        }

        public void ToObjectFragment(ObjectFragment objectFragment)
        {
            objectFragment.ObjectId = ObjectId;
            objectFragment.ObjectIndex = LocalObjectIndex; // Remote bubbles have different index.
            objectFragment.ObjectName = ObjectName;
            objectFragment.ParentObjectId = ParentObjectId;
            objectFragment.TypeId = TypeId;
            objectFragment.TypeName = TypeName;
            objectFragment.OwnerId = OwnerId;
            objectFragment.Location.X = Location.X;
            objectFragment.Location.Y = Location.Y;
            objectFragment.Location.Z = Location.Z;
            objectFragment.Velocity.X = Velocity.X;
            objectFragment.Velocity.Y = Velocity.Y;
            objectFragment.Velocity.Z = Velocity.Z;
            objectFragment.Acceleration.X = Acceleration.X;
            objectFragment.Acceleration.Y = Acceleration.Y;
            objectFragment.Acceleration.Z = Acceleration.Z;
            objectFragment.Orientation.X = Orientation.X;
            objectFragment.Orientation.Y = Orientation.Y;
            objectFragment.Orientation.Z = Orientation.Z;
            objectFragment.Orientation.W = Orientation.W;
            objectFragment.AngularVelocity.X = AngularVelocity.X;
            objectFragment.AngularVelocity.Y = AngularVelocity.Y;
            objectFragment.AngularVelocity.Z = AngularVelocity.Z;
            objectFragment.AngularVelocity.W = AngularVelocity.W;
            objectFragment.AngularAcceleration.X = AngularAcceleration.X;
            objectFragment.AngularAcceleration.Y = AngularAcceleration.Y;
            objectFragment.AngularAcceleration.Z = AngularAcceleration.Z;
            objectFragment.AngularAcceleration.W = AngularAcceleration.W;
            objectFragment.BoundingSphereRadius = BoundingSphereRadius;
            objectFragment.Mass = Mass;
            objectFragment.ExtensionDialect = ExtensionDialect;
            objectFragment.ExtensionDialectMajorVersion = ExtensionDialectMajorVersion;
            objectFragment.ExtensionDialectMinorVersion = ExtensionDialectMinorVersion;
            byte[] buffer = new byte[ExtensionData.Length];
            Array.Copy(ExtensionData, buffer, ExtensionData.Length);
            objectFragment.SetExtensionData(buffer);
        }

    }
}
