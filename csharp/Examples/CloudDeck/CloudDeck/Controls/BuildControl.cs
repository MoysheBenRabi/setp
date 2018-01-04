using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CloudDeck.Model;
using CloudMath;

namespace CloudDeck.Controls
{
    public partial class BuildControl : UserControl
    {
        public BuildControl()
        {
            InitializeComponent();
            if (DeckProgram.DeckDaemon != null)
            {
                DeckProgram.DeckDaemon.ObjectTypesChanged += OnObjectTypesChanged;
            }

            if (DeckProgram.DeckSelection != null)
            {
                DeckProgram.DeckSelection.ObjectFocused += ObjectFocused;
                DeckProgram.DeckSelection.ObjectDefocused += ObjectDefocused;
            }

        }

        public void ObjectFocused(DeckObject deckObject)
        {
            deleteObjectButton.Enabled = true;
            this.Visible = true;
        }

        public void ObjectDefocused(DeckObject deckObject)
        {
            deleteObjectButton.Enabled = false;
        }

        private void loadTypesButton_Click(object sender, EventArgs e)
        {
            DeckProgram.DeckEngine.RequestObjectTypes();
        }

        private void OnObjectTypesChanged(List<DeckObjectType> objectTypes)
        {
            typeComboBox.Items.Clear();
            foreach (DeckObjectType objectType in objectTypes)
            {
                typeComboBox.Items.Add(objectType);
            }
            if (objectTypes.Count > 0)
            {
                typeComboBox.SelectedIndex = 0;
            }
        }

        private void insertObjectButton_Click(object sender, EventArgs e)
        {
            DeckObjectType objectType=(DeckObjectType)typeComboBox.SelectedItem;
            if(objectType==null) 
            {
                return;
            }
            DeckObject avatarObject = DeckProgram.DeckScene.GetObject(DeckProgram.DeckEngine.AvatarId);
            DeckProgram.DeckEngine.RequestObjectInsert(objectType.TypeId, avatarObject.RenderLocation, Quaternion.Identity);
        }

        private void deleteObjectButton_Click(object sender, EventArgs e)
        {
            DeckObject deckObject = DeckProgram.DeckSelection.GetFocus();
            if (deckObject != null)
            {
                DeckProgram.DeckEngine.RequestObjectDelete(deckObject.ObjectId);
            }
        }

    }
}
