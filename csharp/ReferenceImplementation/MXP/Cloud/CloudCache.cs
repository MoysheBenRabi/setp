using System;
using System.Collections.Generic;
using MXP.Delegates;
using MXP.Util;

namespace MXP.Cloud
{
    /// <summary>
    /// CloudCache contains bubble objects.
    /// </summary>
    public class CloudCache
    {
        #region Fields

        private Guid localBubbleId;

        private IDictionary<Guid, CloudObject> objects = new Dictionary<Guid, CloudObject>();
        private IList<Guid> objectList = new List<Guid>();
        private IDictionary<Guid, IDictionary<Guid, Guid>> bubbleObjects = new Dictionary<Guid, IDictionary<Guid, Guid>>();
        private IDictionary<Guid, HashSet<Guid>> participantObjects = new Dictionary<Guid, HashSet<Guid>>();
        private IDictionary<Guid, Guid> objectParticipantDictionary=new Dictionary<Guid, Guid>();
        private IDictionary<Guid, Guid> objectBubbleDictionary=new Dictionary<Guid, Guid>();
        private IDictionary<Guid, IDictionary<uint, Guid>> bubbleObjectIndexGuidDictionary = new Dictionary<Guid, IDictionary<uint, Guid>>();
        private IDictionary<Guid, IDictionary<Guid, uint>> bubbleObjectGuidIndexDictionary = new Dictionary<Guid, IDictionary<Guid, uint>>();
        private IntervalKDTree<Guid> objectKDTree;

        private uint localObjectIndexCounter = 0;
        private TimeSpan cacheObjectLifeSpan;
        private int cacheObjectsToCrawlPerCycle = 20;
        private int lastCrawledObjectListIndex = 0;

        public CacheObjectPut CacheObjectPut = delegate(CloudObject cacheObject) { };
        public CacheObjectRemoved CacheObjectRemoved = delegate(CloudObject cacheObject) { };

        #endregion

        #region Constructors

        public CloudCache(Guid localBubbleId, double bubblePerceptionRange, TimeSpan cacheObjectLifeSpan)
        {
            this.localBubbleId = localBubbleId;
            this.objectKDTree = new IntervalKDTree<Guid>(bubblePerceptionRange,10);
            this.cacheObjectLifeSpan = cacheObjectLifeSpan;
        }

        #endregion

        #region Processing

        public void Process()
        {
            // Crawling cache to remove timed out remote objects.
            for (int i = 0; i < cacheObjectsToCrawlPerCycle&&objectList.Count>0; i++)
            {
                lastCrawledObjectListIndex++;
                if (lastCrawledObjectListIndex >= objectList.Count)
                {
                    lastCrawledObjectListIndex = 0;
                }
                CloudObject cloudObject = objects[objectList[lastCrawledObjectListIndex]];
                if (cloudObject.LastUpdated.Add(cacheObjectLifeSpan) < DateTime.Now
                    && cloudObject.BubbleId!=localBubbleId
                    && localBubbleId!=Guid.Empty)
                {
                    LogUtil.Debug("Carbage collecting object: " + cloudObject.ObjectId+" frm bubble "+localBubbleId);
                    RemoveObject(cloudObject.ObjectId);
                }
            }
        }

        #endregion 

        #region Object Management

        public void Clear()
        {
            objects.Clear();
            objectList.Clear();
            bubbleObjects.Clear();
            objectBubbleDictionary.Clear();
            bubbleObjectIndexGuidDictionary.Clear();
            bubbleObjectGuidIndexDictionary.Clear();
            objectKDTree.Clear();
            participantObjects.Clear();
            objectParticipantDictionary.Clear();
            localObjectIndexCounter = 0;
            lastCrawledObjectListIndex = 0;
        }

        public void PutObject(CloudObject cloudObject, bool publicUpdate)
        {
            if(!objects.ContainsKey(cloudObject.ObjectId))
            {
                // Assigning local cache index to the object. This will be used when object is sent to clients.
                cloudObject.LastUpdated = DateTime.Now;
                localObjectIndexCounter++;
                cloudObject.LocalObjectIndex = localObjectIndexCounter;
                if(cloudObject.RemoteObjectIndex==0)
                {
                    // For participant lets set remote object index.
                    cloudObject.RemoteObjectIndex = localObjectIndexCounter;
                }

                objects.Add(cloudObject.ObjectId,cloudObject);
                objectList.Add(cloudObject.ObjectId);
                if(!bubbleObjects.ContainsKey(cloudObject.BubbleId))
                {
                    bubbleObjects.Add(cloudObject.BubbleId,new Dictionary<Guid, Guid>());
                    bubbleObjectGuidIndexDictionary.Add(cloudObject.BubbleId, new Dictionary<Guid, uint>());
                    bubbleObjectIndexGuidDictionary.Add(cloudObject.BubbleId, new Dictionary<uint, Guid>());
                }
                bubbleObjects[cloudObject.BubbleId].Add(cloudObject.ObjectId, cloudObject.ObjectId);
                bubbleObjectGuidIndexDictionary[cloudObject.BubbleId].Add(cloudObject.ObjectId, cloudObject.RemoteObjectIndex);
                bubbleObjectIndexGuidDictionary[cloudObject.BubbleId].Add(cloudObject.RemoteObjectIndex,cloudObject.ObjectId);

                objectBubbleDictionary.Add(cloudObject.ObjectId, cloudObject.BubbleId);
                objectKDTree.Put(cloudObject.Location.X - cloudObject.BoundingSphereRadius,
                    cloudObject.Location.Y-cloudObject.BoundingSphereRadius,
                    cloudObject.Location.Z-cloudObject.BoundingSphereRadius,
                    cloudObject.Location.X+cloudObject.BoundingSphereRadius,
                    cloudObject.Location.Y+cloudObject.BoundingSphereRadius,
                    cloudObject.Location.Z+cloudObject.BoundingSphereRadius,
                    cloudObject.ObjectId
                    );

                if(!participantObjects.ContainsKey(cloudObject.OwnerId))
                {
                    participantObjects.Add(cloudObject.OwnerId,new HashSet<Guid>());
                }
                participantObjects[cloudObject.OwnerId].Add(cloudObject.ObjectId);
                objectParticipantDictionary.Add(cloudObject.ObjectId,cloudObject.OwnerId);
            }
            else
            {
                if (cloudObject.RemoteObjectIndex == 0)
                {
                    // For participant lets set remote object index.
                    cloudObject.RemoteObjectIndex = cloudObject.LocalObjectIndex;
                }

                if(publicUpdate)
                {
                    cloudObject.LastUpdated = DateTime.Now;
                }

                if (objects[cloudObject.ObjectId] != cloudObject)
                {
                    throw new Exception("Cache must be updated with same cache object.");
                }

                // Updating changed bubble id.
                if (objectBubbleDictionary[cloudObject.ObjectId] != cloudObject.BubbleId)
                {
                    Guid oldBubbleId = objectBubbleDictionary[cloudObject.ObjectId];
                    bubbleObjects[oldBubbleId].Remove(cloudObject.ObjectId);
                    uint oldIndex = bubbleObjectGuidIndexDictionary[oldBubbleId][cloudObject.ObjectId];
                    bubbleObjectGuidIndexDictionary[oldBubbleId].Remove(cloudObject.ObjectId);
                    bubbleObjectIndexGuidDictionary[oldBubbleId].Remove(oldIndex);
                    if (bubbleObjects[objectBubbleDictionary[cloudObject.ObjectId]].Count == 0)
                    {
                        bubbleObjects.Remove(oldBubbleId);
                        bubbleObjectGuidIndexDictionary.Remove(oldBubbleId);
                        bubbleObjectIndexGuidDictionary.Remove(oldBubbleId);
                    }

                    if (!bubbleObjects.ContainsKey(cloudObject.BubbleId))
                    {
                        bubbleObjects.Add(cloudObject.BubbleId, new Dictionary<Guid, Guid>());
                        bubbleObjectGuidIndexDictionary.Add(cloudObject.BubbleId, new Dictionary<Guid, uint>());
                        bubbleObjectIndexGuidDictionary.Add(cloudObject.BubbleId, new Dictionary<uint, Guid>());
                    }
                    bubbleObjects[cloudObject.BubbleId].Add(cloudObject.ObjectId, cloudObject.ObjectId);
                    bubbleObjectGuidIndexDictionary[cloudObject.BubbleId].Add(cloudObject.ObjectId, cloudObject.RemoteObjectIndex);
                    bubbleObjectIndexGuidDictionary[cloudObject.BubbleId].Add(cloudObject.RemoteObjectIndex, cloudObject.ObjectId);
                    objectBubbleDictionary[cloudObject.ObjectId] = cloudObject.BubbleId;
                }

                // Updating changed object index.
                if (bubbleObjectGuidIndexDictionary[cloudObject.BubbleId][cloudObject.ObjectId]!=cloudObject.RemoteObjectIndex)
                {
                    uint oldRemoteIndex = bubbleObjectGuidIndexDictionary[cloudObject.BubbleId][cloudObject.ObjectId];
                    bubbleObjectIndexGuidDictionary[cloudObject.BubbleId].Remove(oldRemoteIndex);
                    bubbleObjectGuidIndexDictionary[cloudObject.BubbleId][cloudObject.ObjectId]=cloudObject.RemoteObjectIndex;
                    bubbleObjectIndexGuidDictionary[cloudObject.BubbleId].Add(cloudObject.RemoteObjectIndex, cloudObject.ObjectId);
                }

                objectKDTree.Put(cloudObject.Location.X - cloudObject.BoundingSphereRadius,
                    cloudObject.Location.Y - cloudObject.BoundingSphereRadius,
                    cloudObject.Location.Z - cloudObject.BoundingSphereRadius,
                    cloudObject.Location.X + cloudObject.BoundingSphereRadius,
                    cloudObject.Location.Y + cloudObject.BoundingSphereRadius,
                    cloudObject.Location.Z + cloudObject.BoundingSphereRadius,
                    cloudObject.ObjectId
                    );

                // Updating changed object participantId
                if(cloudObject.OwnerId!=objectParticipantDictionary[cloudObject.ObjectId])
                {
                    participantObjects[objectParticipantDictionary[cloudObject.ObjectId]].Remove(cloudObject.ObjectId);
                    if(participantObjects[objectParticipantDictionary[cloudObject.ObjectId]].Count==0)
                    {
                        participantObjects.Remove(objectParticipantDictionary[cloudObject.ObjectId]);
                    }
                    if(!participantObjects.ContainsKey(cloudObject.OwnerId))
                    {
                        participantObjects.Add(cloudObject.OwnerId,new HashSet<Guid>());
                    }
                    participantObjects[cloudObject.OwnerId].Add(cloudObject.ObjectId);
                    objectParticipantDictionary[cloudObject.ObjectId]=cloudObject.OwnerId;
                }
            }
            if (publicUpdate)
            {
                CacheObjectPut(cloudObject);
            }
        }        

        public void RemoveObject(Guid objectId)
        {
            if (objects.ContainsKey(objectId))
            {
                CloudObject cloudObject = objects[objectId];
                objects.Remove(objectId);
                objectList.Remove(objectId);
                bubbleObjects[cloudObject.BubbleId].Remove(objectId);
                bubbleObjectGuidIndexDictionary[cloudObject.BubbleId].Remove(objectId);
                bubbleObjectIndexGuidDictionary[cloudObject.BubbleId].Remove(cloudObject.RemoteObjectIndex);
                if(bubbleObjects[cloudObject.BubbleId].Count==0)
                {
                    bubbleObjects.Remove(cloudObject.BubbleId);
                    bubbleObjectGuidIndexDictionary.Remove(cloudObject.BubbleId);
                    bubbleObjectIndexGuidDictionary.Remove(cloudObject.BubbleId);
                }
                objectBubbleDictionary.Remove(cloudObject.ObjectId);
                objectKDTree.Remove(objectId);
                participantObjects[objectParticipantDictionary[objectId]].Remove(objectId);
                if (participantObjects[objectParticipantDictionary[objectId]].Count == 0)
                {
                    participantObjects.Remove(objectParticipantDictionary[objectId]);
                }
                objectParticipantDictionary.Remove(objectId);
                CacheObjectRemoved(cloudObject);
            }
        }

        public CloudObject GetObject(Guid objectId)
        {
            if(objects.ContainsKey(objectId))
            {
                return objects[objectId];
            }
            else
            {
                return null;
            }
        }

        public HashSet<Guid> GetObjectIds(double minX, double minY, double minZ, double maxX, double maxY, double maxZ, HashSet<Guid> objectIds)
        {
            return objectKDTree.GetValues(minX, minY, minZ, maxX, maxY, maxZ, objectIds);
        }

        public IList<Guid> GetObjectIds(double minX, double minY, double minZ, double maxX, double maxY, double maxZ, IList<Guid> objectIds)
        {
            return objectKDTree.GetValues(minX, minY, minZ, maxX, maxY, maxZ, objectIds);
        }

        public IDictionary<Guid, Guid> GetBubbleObjectIds(Guid bubbleId)
        {
            if(bubbleObjects.ContainsKey(bubbleId))
            {
                return bubbleObjects[bubbleId];
            }
            else
            {
                return new Dictionary<Guid, Guid>();
            }
        }

        public HashSet<Guid> GetParticipantObjectIds(Guid bubbleId)
        {
            if (participantObjects.ContainsKey(bubbleId))
            {
                return participantObjects[bubbleId];
            }
            else
            {
                return new HashSet<Guid>();
            }
        }

        public CloudObject GetObject(Guid bubbleId, uint objectIndex)
        {
            if (bubbleObjectIndexGuidDictionary.ContainsKey(bubbleId) &&
                bubbleObjectIndexGuidDictionary[bubbleId].ContainsKey(objectIndex))
            {
                return objects[bubbleObjectIndexGuidDictionary[bubbleId][objectIndex]];
            }
            else
            {
                return null;
            }
        }

        #endregion

    }
}
