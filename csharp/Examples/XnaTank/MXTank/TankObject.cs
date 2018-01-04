using System;
using System.Collections.Generic;

using System.Text;
using MXP.Fragments;

namespace MXTank
{

    /// <summary>
    /// TankObject represents a virtual object injected to the bubble by a participant.
    /// </summary>
    public class TankObject
    {
        private static uint IndexCounter = 0;
        public static uint GetNewIndex()
        {
            IndexCounter++;
            return IndexCounter;
        }

        public Guid ObjectId = Guid.Empty; // 16
        public uint ObjectIndex; // 4
        public Guid TypeId = Guid.Empty; // 16
        public string ObjectName; // 20
        public string TypeName; // 20
        public Guid OwnerId = Guid.Empty; // 16
        public float[] Location = new float[3]; // 12
        public float[] Velocity = new float[3]; // 12
        public float[] Acceleration = new float[3]; // 12
        public float[] Orientation = new float[4]; // 16
        public float[] AngularVelocity = new float[4]; // 16
        public float[] AngularAcceleration = new float[4]; // 16
        public float BoundingSphereRadius; // 4
        public float Mass; // 4
        public string StatePayloadDialect=""; // 4
        public uint StatePayloadLength; // 4
        private byte[] StatePayloadData=new byte[0]; //X

        public override string ToString()
        {
            String str = "TankObject [" +
            "ObjectId: " + ObjectId +
            ",ObjectIndex: " + ObjectIndex +
            ",TypeId: " + TypeId +
            ",ObjectName: " + ObjectName +
            ",TypeName: " + TypeName +
            ",OwnerId: " + OwnerId +
            ",Location: " + Location[0] + "," + Location[1] + "," + Location[2] + "," +
            ",Velocity: " + Velocity[0] + "," + Velocity[1] + "," + Velocity[2] + "," +
            ",Acceleration: " + Acceleration[0] + "," + Acceleration[1] + "," + Acceleration[2] + "," +
            ",Orientation: " + Orientation[0] + "," + Orientation[1] + "," + Orientation[2] + "," + Orientation[3] + "," +
            ",AngularVelocity: " + AngularVelocity[0] + "," + AngularVelocity[1] + "," + AngularVelocity[2] + "," + AngularVelocity[3] + "," +
            ",AngularAcceleration: " + AngularAcceleration[0] + "," + AngularAcceleration[1] + "," + AngularAcceleration[2] + "," + AngularAcceleration[3] + "," +
            ",BoundingSphereRadius: " + BoundingSphereRadius +
            ",Mass: " + Mass +
            ",StatePayloadDialect: " + StatePayloadDialect +
            ",StatePayloadLength: " + StatePayloadLength +
            ",StatePayloadData: ";

            if (StatePayloadData != null)
            {
                for (int i = 0; i < StatePayloadData.Length; i++)
                {
                    if (i > 0)
                    {
                        str += ",";
                    }
                    str += StatePayloadData[i];
                }
            }

            str += "]";

            return str;
        }

        public void SetValues(ObjectFragment source)
        {
            TankObject target = this;
            target.ObjectId = source.ObjectId;
            //target.ObjectIndex = source.ObjectIndex;
            target.TypeId = source.TypeId;
            target.ObjectName = source.ObjectName;
            target.TypeName = source.TypeName;
            //target.OwnerId = source.OwnerId;
            target.Location[0] = source.Location.X; target.Location[1] = source.Location.Y; target.Location[2] = source.Location.Z;
            target.Velocity[0] = source.Velocity.X; target.Velocity[1] = source.Velocity.Y; target.Velocity[2] = source.Velocity.Z;
            target.Acceleration[0] = source.Acceleration.X; target.Acceleration[1] = source.Acceleration.Y; target.Acceleration[2] = source.Acceleration.Z;
            target.Orientation[0] = source.Orientation.X; target.Orientation[1] = source.Orientation.Y; target.Orientation[2] = source.Orientation.Z; target.Orientation[3] = source.Orientation.W;
            target.AngularVelocity[0] = source.AngularVelocity.X; target.AngularVelocity[1] = source.AngularVelocity.Y; target.AngularVelocity[2] = source.AngularVelocity.Z; target.AngularVelocity[3] = source.AngularVelocity.W;
            target.AngularAcceleration[0] = source.AngularAcceleration.X; target.AngularAcceleration[1] = source.AngularAcceleration.Y; target.AngularAcceleration[2] = source.AngularAcceleration.Z; target.AngularAcceleration[3] = source.AngularAcceleration.W;
            target.BoundingSphereRadius = source.BoundingSphereRadius;
            target.Mass = source.Mass;
            target.StatePayloadDialect = source.ExtensionDialect;
            target.StatePayloadLength = source.ExtensionLength;
            target.StatePayloadData = source.GetExtensionData();
        }

        public void GetValues(ObjectFragment target)
        {
            TankObject source = this;
            target.ObjectId = source.ObjectId;
            target.ObjectIndex = source.ObjectIndex;
            target.TypeId = source.TypeId;
            target.ObjectName = source.ObjectName;
            target.TypeName = source.TypeName;
            target.OwnerId = source.OwnerId;
            target.Location.X = source.Location[0]; target.Location.Y = source.Location[1]; target.Location.Z = source.Location[2];
            target.Velocity.X = source.Velocity[0]; target.Velocity.Y = source.Velocity[1]; target.Velocity.Z = source.Velocity[2];
            target.Acceleration.X = source.Acceleration[0]; target.Acceleration.Y = source.Acceleration[1]; target.Acceleration.Z = source.Acceleration[2];
            target.Orientation.X = source.Orientation[0]; target.Orientation.Y = source.Orientation[1]; target.Orientation.Z = source.Orientation[2]; target.Orientation.W = source.Orientation[3];
            target.AngularVelocity.X = source.AngularVelocity[0]; target.AngularVelocity.Y = source.AngularVelocity[1]; target.AngularVelocity.Z = source.AngularVelocity[2]; target.AngularVelocity.W = source.AngularVelocity[3];
            target.AngularAcceleration.X = source.AngularAcceleration[0]; target.AngularAcceleration.Y = source.AngularAcceleration[1]; target.AngularAcceleration.Z = source.AngularAcceleration[2]; target.AngularAcceleration.W = source.AngularAcceleration[3];
            target.BoundingSphereRadius = source.BoundingSphereRadius;
            target.Mass = source.Mass;
            target.ExtensionDialect = source.StatePayloadDialect;
            target.SetExtensionData(source.StatePayloadData);
        }

    }
}
