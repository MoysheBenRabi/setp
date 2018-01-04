using System;
using System.Text;
using System.Collections.Generic;

using NUnit.Framework;
using MXP;
using System.Threading;
using MXP.Messages;
using MXP.Common.Proto;
using MXP.Util;
using MXP.Cloud;

namespace MXPTests.Cloud
{

    [TestFixture]
    public class BubbleCacheTest
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void TestBubbleCache()
        {

            Guid localBubbleId = Guid.NewGuid();
            Guid objectId = Guid.NewGuid();
            Guid ownerId = Guid.NewGuid();
            CloudCache m_cloudCache = new CloudCache(localBubbleId, 1000, new TimeSpan(0,10,0,0));

            CloudObject cloudObject=new CloudObject();
            cloudObject.ObjectId = objectId;
            cloudObject.OwnerId = ownerId;
            cloudObject.BubbleId = localBubbleId;
            cloudObject.Location.X = 1;
            cloudObject.Location.Y = 2;
            cloudObject.Location.Z = 3;
            cloudObject.BoundingSphereRadius = 10;

            m_cloudCache.PutObject(cloudObject,true);

            Assert.IsNotNull(m_cloudCache.GetObject(objectId));
            Assert.IsNotNull(m_cloudCache.GetObject(localBubbleId, cloudObject.RemoteObjectIndex));
            Assert.AreEqual(1, m_cloudCache.GetObjectIds(0, 0, 0, 1, 1, 1, new HashSet<Guid>()).Count);
            Assert.AreEqual(1, m_cloudCache.GetBubbleObjectIds(localBubbleId).Count);
            Assert.AreEqual(1, m_cloudCache.GetParticipantObjectIds(ownerId).Count);


            m_cloudCache.RemoveObject(objectId);

            Assert.IsNull(m_cloudCache.GetObject(objectId));
            Assert.IsNull(m_cloudCache.GetObject(localBubbleId, cloudObject.RemoteObjectIndex));
            Assert.AreEqual(0, m_cloudCache.GetObjectIds(0, 0, 0, 1, 1, 1, new HashSet<Guid>()).Count);
            Assert.AreEqual(0, m_cloudCache.GetBubbleObjectIds(localBubbleId).Count);
            Assert.AreEqual(0, m_cloudCache.GetParticipantObjectIds(ownerId).Count);

        }


        [Test]
        public void TestBubbleCacheGarbageCollection()
        {

            Guid localBubbleId = Guid.NewGuid();
            Guid remoteBubbleId = Guid.NewGuid();
            Guid localObjectId = Guid.NewGuid();
            Guid remoteObjectId = Guid.NewGuid();
            CloudCache m_cloudCache = new CloudCache(localBubbleId, 1000, new TimeSpan(0, 0, 0, 0, 10));

            CloudObject localObject = new CloudObject();
            localObject.ObjectId = localObjectId;
            localObject.BubbleId = localBubbleId;
            localObject.Location.X = 1;
            localObject.Location.Y = 2;
            localObject.Location.Z = 3;
            localObject.BoundingSphereRadius = 10;
            m_cloudCache.PutObject(localObject, true);

            CloudObject remoteObject = new CloudObject();
            remoteObject.ObjectId = remoteObjectId;
            remoteObject.BubbleId = remoteBubbleId;
            remoteObject.Location.X = 1;
            remoteObject.Location.Y = 2;
            remoteObject.Location.Z = 3;
            remoteObject.BoundingSphereRadius = 10;
            m_cloudCache.PutObject(remoteObject, true);

            Assert.IsNotNull(m_cloudCache.GetObject(localObjectId));
            Assert.IsNotNull(m_cloudCache.GetObject(remoteObjectId));
            Assert.IsNotNull(m_cloudCache.GetObject(localBubbleId, localObject.RemoteObjectIndex));
            Assert.IsNotNull(m_cloudCache.GetObject(remoteBubbleId, remoteObject.RemoteObjectIndex));

            m_cloudCache.Process();

            Assert.IsNotNull(m_cloudCache.GetObject(localObjectId));
            Assert.IsNotNull(m_cloudCache.GetObject(remoteObjectId));
            Assert.IsNotNull(m_cloudCache.GetObject(localBubbleId, localObject.RemoteObjectIndex));
            Assert.IsNotNull(m_cloudCache.GetObject(remoteBubbleId, remoteObject.RemoteObjectIndex));

            Thread.Sleep(20);
            m_cloudCache.Process();

            Assert.IsNotNull(m_cloudCache.GetObject(localObjectId));
            Assert.IsNull(m_cloudCache.GetObject(remoteObjectId));
            Assert.IsNotNull(m_cloudCache.GetObject(localBubbleId, localObject.RemoteObjectIndex));
            Assert.IsNull(m_cloudCache.GetObject(remoteBubbleId, remoteObject.RemoteObjectIndex));

        }

    }
}
