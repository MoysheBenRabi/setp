using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DaemonLogic
{
    public class ObjectLogic
    {

        public static ObjectType GetObjectType(Guid objectTypeId)
        {
            using (DaemonEntities entities = new DaemonEntities())
            {
                ObjectType objectType = QueryUtil.First<ObjectType>((from o in entities.ObjectType where o.ObjectTypeId == objectTypeId select o));
                entities.Detach(objectType);
                return objectType;
            }
        }

        public static ObjectType AddObjectType(Participant participant)
        {
            using (DaemonEntities entities = new DaemonEntities())
            {
                try
                {
                    entities.Attach(participant);
                    ObjectType objectType = new ObjectType
                    {
                        ObjectTypeId = Guid.NewGuid(),
                        Participant = participant,                        
                        Name = "New Object Type",
                        Radius = 11,
                        Mass = 10,
                        ModelUrl = "http://",
                        ModelScale = 10,
                        Published = false
                    };
                    entities.AddToObjectType(objectType);
                    entities.SaveChanges();
                    entities.Detach(objectType);
                    return objectType;
                }
                finally
                {
                    entities.Detach(participant);
                }
            }
        }

        public static CloudObject AddObject(Participant participant,ObjectType objectType, Bubble bubble)
        {
            using (DaemonEntities entities = new DaemonEntities())
            {
                try
                {
                    entities.Attach(participant);
                    entities.Attach(objectType);
                    entities.Attach(bubble);
                    CloudObject cloudObject = new CloudObject
                    {
                        CloudObjectId = Guid.NewGuid(),
                        Participant = participant,
                        ObjectType=objectType,
                        Bubble=bubble,
                        Name = "New "+objectType.Name,
                        Radius = objectType.Radius,
                        Mass = objectType.Mass,
                        ModelUrl = objectType.ModelUrl,
                        ModelScale = objectType.ModelScale,
                        X=0,
                        Y=0,
                        Z=0,
                        OX = 0,
                        OY = 0,
                        OZ = 0,
                        OW = 0,
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                        Enabled = false
                    };
                    entities.AddToCloudObject(cloudObject);
                    entities.SaveChanges();
                    entities.Detach(cloudObject);
                    return cloudObject;
                }
                finally
                {
                    entities.Detach(participant);
                    entities.Detach(objectType);
                    entities.Detach(bubble);
                }
            }
        }

        public static void UpdateObject(CloudObject cloudObject)
        {
            using (DaemonEntities entities = new DaemonEntities())
            {
                try
                {
                    entities.Attach(cloudObject);
                    ObjectType objectType = QueryUtil.First<ObjectType>(from o in entities.CloudObject where o.CloudObjectId == cloudObject.CloudObjectId select o.ObjectType);
                    Participant participant = QueryUtil.First<Participant>(from o in entities.CloudObject where o.CloudObjectId == cloudObject.CloudObjectId select o.Participant);
                    cloudObject.Modified = DateTime.Now;
                    entities.SaveChanges();
                }
                finally
                {
                    entities.Detach(cloudObject);
                }
            }
        }

    }
}
