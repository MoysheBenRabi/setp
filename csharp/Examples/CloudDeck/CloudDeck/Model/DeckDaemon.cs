using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace CloudDeck.Model
{
    public delegate void ObjectTypesChanged(List<DeckObjectType> objectTypes);

    public class DeckDaemon
    {
        private List<DeckObjectType> m_objectTypes = new List<DeckObjectType>();
        public ObjectTypesChanged ObjectTypesChanged = delegate(List<DeckObjectType> objectTypes) { };

        public void SetObjectTypes(List<DeckObjectType> objectTypes)
        {
            m_objectTypes = objectTypes;
            ObjectTypesChanged(m_objectTypes);
        }

        public List<DeckObjectType> GetObjectTypes()
        {
            return m_objectTypes;
        }
    }
}
