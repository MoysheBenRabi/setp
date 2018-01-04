using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudDeck.Model
{
    public delegate void ObjectSelected(DeckObject deckObject);
    public delegate void ObjectDeselected(DeckObject deckObject);
    public delegate void ObjectFocused(DeckObject deckObject);
    public delegate void ObjectDefocused(DeckObject deckObject);

    /// <summary>
    /// DeckSelection is the object selection model.Only single object can be focused but multiple objects can be selected.
    /// If object is focused then it is always selected.
    /// </summary>
    public class DeckSelection
    {
        private DeckObject m_focus;
        private List<DeckObject> m_selection=new List<DeckObject>();

        public ObjectSelected ObjectSelected;
        public ObjectDeselected ObjectDeselected;
        public ObjectFocused ObjectFocused;
        public ObjectDefocused ObjectDefocused;

        public DeckSelection()
        {
            ObjectSelected = delegate(DeckObject deckObject) { };
            ObjectDeselected = delegate(DeckObject deckObject) { };
            ObjectFocused = delegate(DeckObject deckObject) { };
            ObjectDefocused = delegate(DeckObject deckObject) { };
        }

        public void SelectObject(DeckObject deckObject)
        {
            if (!m_selection.Contains(deckObject))
            {
                m_selection.Add(deckObject);
                ObjectSelected(deckObject);
            }
            if (m_focus == null)
            {
                FocusObject(deckObject);
            }
        }

        public void DeselectObject(DeckObject deckObject)
        {
            if (m_focus == deckObject)
            {
                DefocusObject(deckObject);
                if (m_selection.Count > 0 && m_selection[0] != deckObject)
                {
                    FocusObject(m_selection[0]);
                }
                else
                {
                    if (m_selection.Count > 1)
                    {
                        FocusObject(m_selection[1]);
                    }
                }
            }
            if (m_selection.Contains(deckObject))
            {
                m_selection.Remove(deckObject);
                ObjectDeselected(deckObject);
            }
        }

        public List<DeckObject> GetSelection()
        {
            return m_selection;
        }

        public DeckObject GetFocus()
        {
            return m_focus;
        }

        public void ClearSelection()
        {
            if (m_focus != null)
            {
                DefocusObject(m_focus);
            }
            foreach (DeckObject deckObject in m_selection)
            {
                ObjectDeselected(deckObject);
            }
            m_selection.Clear();
        }

        public void FocusObject(DeckObject deckObject)
        {
            if (!m_selection.Contains(deckObject))
            {
                SelectObject(deckObject);
            }
            if (m_focus != deckObject)
            {
                m_focus = deckObject;
                ObjectFocused(deckObject);
            }
        }

        public void DefocusObject(DeckObject deckObject)
        {
            if (m_focus != null)
            {
                m_focus = null;
                ObjectDefocused(deckObject);
            }
        }

    }
}
