using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MXP.Fragments;
using Microsoft.Xna.Framework;
using MXP.Messages;

namespace MXDeck.Engine
{
    public class DeckObject
    {
        public Guid ObjectId = Guid.Empty; // 16
        public uint ObjectIndex; // 4
        public Guid TypeId = Guid.Empty; // 16
        public string ObjectName; // 20
        public string TypeName; // 20
        public Guid OwnerId = Guid.Empty; // 16
        public Vector3 InterpolatedLocation = new Vector3(); // 12
        public Vector3 Velocity = new Vector3(); // 12
        public Vector3 Acceleration = new Vector3(); // 12
        public Quaternion Orientation = new Quaternion(); // 16
        public Quaternion AngularVelocity = new Quaternion(); // 16
        public Quaternion AngularAcceleration = new Quaternion(); // 16
        public float BoundingSphereRadius; // 4
        public float Mass; // 4
        public string StatePayloadDialect = ""; // 4
        public uint StatePayloadLength; // 4
        private byte[] StatePayloadData = new byte[0]; //X

        bool firstInterpolation = true;
        public Vector3 SmoothedLocation = new Vector3();
        public Vector3 InterpolatedVelocity = new Vector3();
        public Quaternion SmoothedOrientation = new Quaternion();

        public override string ToString()
        {
            String str = "TankObject [" +
            "ObjectId: " + ObjectId +
            ",ObjectIndex: " + ObjectIndex +
            ",TypeId: " + TypeId +
            ",ObjectName: " + ObjectName +
            ",TypeName: " + TypeName +
            ",OwnerId: " + OwnerId +
            ",Location: " + InterpolatedLocation.X + "," + InterpolatedLocation.Y + "," + InterpolatedLocation.Z + "," +
            ",Velocity: " + Velocity.X + "," + Velocity.Y + "," + Velocity.Z + "," +
            ",Acceleration: " + Acceleration.X + "," + Acceleration.Y + "," + Acceleration.Z + "," +
            ",Orientation: " + Orientation.X + "," + Orientation.Y + "," + Orientation.Z + "," + Orientation.W + "," +
            ",AngularVelocity: " + AngularVelocity.X + "," + AngularVelocity.Y + "," + AngularVelocity.Z + "," + AngularVelocity.W + "," +
            ",AngularAcceleration: " + AngularAcceleration.X + "," + AngularAcceleration.Y + "," + AngularAcceleration.Z + "," + AngularAcceleration.W + "," +
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
            DeckObject target = this;
            target.ObjectId = source.ObjectId;
            target.ObjectIndex = source.ObjectIndex;
            target.TypeId = source.TypeId;
            target.ObjectName = source.ObjectName;
            target.TypeName = source.TypeName;
            target.OwnerId = source.OwnerId;
            target.InterpolatedLocation.X = source.Location.X; target.InterpolatedLocation.Y = source.Location.Y; target.InterpolatedLocation.Z = source.Location.Z;
            target.Velocity.X = source.Velocity.X; target.Velocity.Y = source.Velocity.Y; target.Velocity.Z = source.Velocity.Z;
            target.Acceleration.X = source.Acceleration.X; target.Acceleration.Y = source.Acceleration.Y; target.Acceleration.Z = source.Acceleration.Z;
            target.Orientation.X = source.Orientation.X; target.Orientation.Y = source.Orientation.Y; target.Orientation.Z = source.Orientation.Z; target.Orientation.W = source.Orientation.W;
            target.AngularVelocity.X = source.AngularVelocity.X; target.AngularVelocity.Y = source.AngularVelocity.Y; target.AngularVelocity.Z = source.AngularVelocity.Z; target.AngularVelocity.W = source.AngularVelocity.W;
            target.AngularAcceleration.X = source.AngularAcceleration.X; target.AngularAcceleration.Y = source.AngularAcceleration.Y; target.AngularAcceleration.Z = source.AngularAcceleration.Z; target.AngularAcceleration.W = source.AngularAcceleration.W;
            target.BoundingSphereRadius = source.BoundingSphereRadius;
            target.Mass = source.Mass;
            target.StatePayloadDialect = source.ExtensionDialect;
            target.StatePayloadLength = source.ExtensionLength;
            target.StatePayloadData = source.GetExtensionData();

            this.SmoothedLocation.X = source.Location.X;
            this.SmoothedLocation.Y = source.Location.Y;
            this.SmoothedLocation.Z = source.Location.Z;

            lastUpdateTime = DateTime.Now;
        }

        private DateTime lastUpdateTime = DateTime.Now;

        public void SetValues(MovementEventMessage movement)
        {         
            this.Orientation.X = movement.Orientation.X;
            this.Orientation.Y = movement.Orientation.Y;
            this.Orientation.Z = movement.Orientation.Z;
            this.Orientation.W = movement.Orientation.W;

            float timeDelta = (float)DateTime.Now.Subtract(lastUpdateTime).TotalSeconds;
            lastUpdateTime = DateTime.Now;

            this.InterpolatedVelocity.X = (movement.Location.X - this.InterpolatedLocation.X) / 0.1f; // server tick time
            this.InterpolatedVelocity.Y = (movement.Location.Y - this.InterpolatedLocation.Y) / 0.1f;
            this.InterpolatedVelocity.Z = (movement.Location.Z - this.InterpolatedLocation.Z) / 0.1f;
        }

        public void GetValues(ObjectFragment target)
        {
            DeckObject source = this;
            target.ObjectId = source.ObjectId;
            target.ObjectIndex = source.ObjectIndex;
            target.TypeId = source.TypeId;
            target.ObjectName = source.ObjectName;
            target.TypeName = source.TypeName;
            target.OwnerId = source.OwnerId;
            target.Location.X = source.InterpolatedLocation.X; target.Location.Y = source.InterpolatedLocation.Y; target.Location.Z = source.InterpolatedLocation.Z;
            target.Velocity.X = source.Velocity.X; target.Velocity.Y = source.Velocity.Y; target.Velocity.Z = source.Velocity.Z;
            target.Acceleration.X = source.Acceleration.X; target.Acceleration.Y = source.Acceleration.Y; target.Acceleration.Z = source.Acceleration.Z;
            target.Orientation.X = source.Orientation.X; target.Orientation.Y = source.Orientation.Y; target.Orientation.Z = source.Orientation.Z; target.Orientation.W = source.Orientation.W;
            target.AngularVelocity.X = source.AngularVelocity.X; target.AngularVelocity.Y = source.AngularVelocity.Y; target.AngularVelocity.Z = source.AngularVelocity.Z; target.AngularVelocity.W = source.AngularVelocity.W;
            target.AngularAcceleration.X = source.AngularAcceleration.X; target.AngularAcceleration.Y = source.AngularAcceleration.Y; target.AngularAcceleration.Z = source.AngularAcceleration.Z; target.AngularAcceleration.W = source.AngularAcceleration.W;
            target.BoundingSphereRadius = source.BoundingSphereRadius;
            target.Mass = source.Mass;
            target.ExtensionDialect = source.StatePayloadDialect;
            target.SetExtensionData(source.StatePayloadData);
        }

        public void InterpolateLocation(float timeDelta)
        {
            
            // Doing double interpolation

            // Making sure lag does not completely through interpolation off the track
            if (timeDelta > 0.05f)
            {
                timeDelta = 0.05f;
            }

            // First level: Linear interpolation based on velocity calculated on the time of last movement event.
            InterpolatedLocation += InterpolatedVelocity * timeDelta * 0.05f;


            // Second level: Simple smoothing by adding another location which tracks the Linearly Interpolated Location
            //SmoothedLocation += (InterpolatedLocation - SmoothedLocation) * 0.02f;
            SmoothedLocation = InterpolatedLocation; // hack to avoid second level smoothing

            SmoothedOrientation = Quaternion.Lerp(SmoothedOrientation, Orientation, timeDelta);

            firstInterpolation = false;

        }

    }
}
