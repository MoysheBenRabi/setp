using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CloudDeck.Model;

namespace CloudDeck.Controls
{
    public partial class SelectionControl : UserControl
    {
        private DataTable m_selectionTable=new DataTable();

        public SelectionControl()
        {
            InitializeComponent();
            if (DeckProgram.DeckSelection != null)
            {
                DeckProgram.DeckSelection.ObjectSelected += ObjectSelected;
                DeckProgram.DeckSelection.ObjectDeselected += ObjectDeselected;
                DeckProgram.DeckSelection.ObjectFocused += ObjectFocused;
                DeckProgram.DeckSelection.ObjectDefocused += ObjectDefocused;
            }

            m_selectionTable.Columns.Add("Id", typeof(Guid));
            m_selectionTable.Columns.Add("Name", typeof(string));
            m_selectionTable.Columns.Add("Type", typeof(string));
            m_selectionTable.Columns.Add("Object", typeof(DeckObject));
            m_selectionTable.PrimaryKey = new DataColumn[] { m_selectionTable.Columns["Id"] };
            selectionGrid.DataSource = m_selectionTable;
        }

        public void ObjectSelected(DeckObject deckObject)
        {
            m_selectionTable.Rows.Add(new object[]{deckObject.ObjectId,deckObject.ObjectName,deckObject.TypeName,deckObject});
        }

        public void ObjectDeselected(DeckObject deckObject)
        {
            DataRow row=m_selectionTable.Rows.Find(deckObject.ObjectId);
            if (row != null)
            {
                m_selectionTable.Rows.Remove(row);
            }
        }

        public void ObjectFocused(DeckObject deckObject)
        {
            if (this.Visible == false)
            {
                this.Visible = true;
            }
            DataRow row = m_selectionTable.Rows.Find(deckObject.ObjectId);
            int rowIndex=m_selectionTable.Rows.IndexOf(row);
            if (selectionGrid.Rows.Count > rowIndex)
            {
                selectionGrid.Rows[rowIndex].Selected = true;
            }
        }

        public void ObjectDefocused(DeckObject deckObject)
        {
            if (selectionGrid.SelectedRows.Count != 0)
            {
                selectionGrid.ClearSelection();
            }
        }

        private void selectionGrid_SelectionChanged(object sender, EventArgs e)
        {
        }

        private void selectionGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataRow row = m_selectionTable.Rows[e.RowIndex];
            DeckObject deckObject = (DeckObject)row["Object"];
            if (Control.ModifierKeys == Keys.Control)
            {
                if (DeckProgram.DeckSelection.GetSelection().Contains(deckObject))
                {
                    DeckProgram.DeckSelection.DeselectObject(deckObject);
                }
            }
            else
            {
                if (DeckProgram.DeckSelection.GetFocus() != deckObject)
                {
                    DeckProgram.DeckSelection.FocusObject(deckObject);
                }
            }
        }

    }
}
