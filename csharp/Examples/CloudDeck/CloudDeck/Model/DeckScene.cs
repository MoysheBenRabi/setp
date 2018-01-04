using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MXP.Cloud;
using CloudMath;

namespace CloudDeck.Model
{
    public delegate void BubbleAdd(DeckBubble deckBubble);
    public delegate void BubbleRemove(DeckBubble deckBubble);
    public delegate void ObjectAdd(DeckObject deckObject);
    public delegate void ObjectUpdate(DeckObject deckObject);
    public delegate void ObjectRemove(DeckObject deckObject);

    public delegate void ChatMessage(DeckObject deckObject, string message);

    public class DeckScene
    {
        public ObjectAdd ObjectAdd;
        public ObjectUpdate ObjectUpdate;
        public ObjectRemove ObjectRemove;
        public BubbleAdd BubbleAdd;
        public BubbleRemove BubbleRemove;
        public ChatMessage ChatMessage;


        private IDictionary<Guid, DeckBubble> m_bubbles = new Dictionary<Guid, DeckBubble>();

        private IDictionary<Guid, DeckObject> m_objects = new Dictionary<Guid, DeckObject>();
        private HashSet<Guid> m_activeObjects = new HashSet<Guid>();

        public DeckScene()
        {
            ObjectAdd = delegate(DeckObject deckObject) { };
            ObjectUpdate = delegate(DeckObject deckObject) { };
            ObjectRemove = delegate(DeckObject deckObject) { };
            BubbleAdd = delegate(DeckBubble deckObject) { };
            BubbleRemove = delegate(DeckBubble deckObject) { };
        }

        public void Process()
        {
            List<Guid> objectIdsToDeactivate = new List<Guid>();
            foreach (Guid objectId in m_activeObjects)
            {
                DeckObject deckObject = m_objects[objectId];
                deckObject.Process();
                ObjectUpdate(deckObject);
                
                Quaternion networkOrientation=deckObject.NetworkOrientation;
                Quaternion renderOrientation=deckObject.RenderOrientation;
                if ((deckObject.NetworkLocation - deckObject.RenderLocation).Length<0.01
                    && (deckObject.NetworkScale - deckObject.RenderScale).Length < 0.01
                    && Common.FastDistance(ref networkOrientation, ref renderOrientation) < 0.01)
                {
                    objectIdsToDeactivate.Add(objectId);
                }
            }
            foreach (Guid objectId in objectIdsToDeactivate)
            {
                DeactivateObject(objectId);
            }

            DeckProgram.DeckRudder.UpdateCameraTransformation();

        }

        public DeckBubble GetBubble(Guid bubbleId)
        {
            return m_bubbles[bubbleId];
        }

        public void AddBubble(DeckBubble bubble)
        {
            m_bubbles.Add(bubble.BubbleId,bubble);
            BubbleAdd(bubble);
        }

        public void RemoveBubble(Guid bubbleId)
        {
            BubbleRemove(m_bubbles[bubbleId]);
            m_bubbles.Remove(bubbleId);
        }

        public bool ContainsObject(Guid objectId)
        {
            return m_objects.ContainsKey(objectId);
        }

        public DeckObject GetObject(Guid objectId)
        {
            return m_objects[objectId];
        }

        public void AddObject(DeckObject deckObject)
        {
            m_objects.Add(deckObject.ObjectId, deckObject);
            ObjectAdd(deckObject);
        }

        public void UpdateObject(DeckObject deckObject)
        {
            ObjectUpdate(deckObject);
        }

        public void RemoveObject(Guid objectId)
        {
            DeckProgram.DeckSelection.DeselectObject(GetObject(objectId));

            ObjectRemove(m_objects[objectId]);
            if (IsActive(objectId))
            {
                DeactivateObject(objectId);
            }
            m_objects.Remove(objectId);
        }

        public void ActivateObject(Guid objectId)
        {
            if (!m_objects.ContainsKey(objectId))
            {
                throw new ArgumentException("Not known object type.");
            }
            m_activeObjects.Add(objectId);
            m_objects[objectId].IsActive = true;
        }

        public void DeactivateObject(Guid objectId)
        {
            m_objects[objectId].IsActive = false;
            m_activeObjects.Remove(objectId);
        }

        public bool IsActive(Guid objectId)
        {
            return m_activeObjects.Contains(objectId);
        }

        public void Clear()
        {
            foreach (DeckObject deckObject in m_objects.Values)
            {
                ObjectRemove(deckObject);
            }
            m_objects.Clear();
            m_activeObjects.Clear();
            foreach (DeckBubble bubble in m_bubbles.Values)
            {
                BubbleRemove(bubble);
            }
            m_bubbles.Clear();
            DeckProgram.DeckSelection.ClearSelection();
        }

        public void AddChatMessage(Guid objectId, string message)
        {
            if (!m_objects.ContainsKey(objectId))
            {
                throw new ArgumentException("Not known object type.");
            }
            ChatMessage(m_objects[objectId], message);
        }
    }
}
