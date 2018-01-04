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
    public partial class ModifyControl : UserControl
    {
        private Guid objectId = Guid.Empty;

        public ModifyControl()
        {
            InitializeComponent();

            if (DeckProgram.DeckSelection != null)
            {
                DeckProgram.DeckSelection.ObjectFocused += ObjectFocused;
                DeckProgram.DeckSelection.ObjectDefocused += ObjectDefocused;
                DeckProgram.DeckScene.ObjectUpdate += OnObjectUpdate;
            }
        }

        public void ObjectFocused(DeckObject deckObject)
        {
            RefreshUserInterfaceValues(deckObject);
            this.Visible = true;
        }

        public void ObjectDefocused(DeckObject deckObject)
        {
            this.Visible = false;
        }

        public void OnObjectUpdate(DeckObject deckObject)
        {
            if (objectId == deckObject.ObjectId)
            {
                RefreshUserInterfaceValues(deckObject);
            }
        }

        private void RefreshUserInterfaceValues(DeckObject deckObject)
        {
            objectId = deckObject.ObjectId;
            objectNameTextBox.Text = deckObject.ObjectName;
            typeNameTextBox.Text = deckObject.TypeName;
            xTextBox.Text = deckObject.NetworkLocation.X.ToString();
            yTextBox.Text = deckObject.NetworkLocation.Y.ToString();
            zTextBox.Text = deckObject.NetworkLocation.Z.ToString();
            scaleTextBox.Text = deckObject.Radius.ToString();
        }

        private void objectNameTextBox_Leave(object sender, EventArgs e)
        {
            DeckObject deckObject = DeckProgram.DeckScene.GetObject(objectId);
            DeckProgram.DeckEngine.RequestObjectUpdate(objectId, objectNameTextBox.Text, deckObject.NetworkLocation, deckObject.NetworkOrientation, deckObject.Radius);
        }

        private void resetOrientationButton_Click(object sender, EventArgs e)
        {
            DeckObject deckObject = DeckProgram.DeckScene.GetObject(objectId);
            DeckProgram.DeckEngine.RequestObjectUpdate(objectId, deckObject.ObjectName, deckObject.NetworkLocation, Quaternion.Identity, deckObject.Radius);            
        }

        private void xTextBox_Leave(object sender, EventArgs e)
        {
            DeckObject deckObject = DeckProgram.DeckScene.GetObject(objectId);
            Vector3 location = new Vector3();
            location.X = Convert.ToSingle(xTextBox.Text);
            location.Y = deckObject.NetworkLocation.Y;
            location.Z = deckObject.NetworkLocation.Z;
            DeckProgram.DeckEngine.RequestObjectUpdate(objectId, deckObject.ObjectName, location, deckObject.NetworkOrientation, deckObject.Radius);
        }

        private void yTextBox_Leave(object sender, EventArgs e)
        {
            DeckObject deckObject = DeckProgram.DeckScene.GetObject(objectId);
            Vector3 location = new Vector3();
            location.X = deckObject.NetworkLocation.X;
            location.Y = Convert.ToSingle(yTextBox.Text);
            location.Z = deckObject.NetworkLocation.Z;
            DeckProgram.DeckEngine.RequestObjectUpdate(objectId, deckObject.ObjectName, location, deckObject.NetworkOrientation, deckObject.Radius);
        }

        private void zTextBox_Leave(object sender, EventArgs e)
        {
            DeckObject deckObject = DeckProgram.DeckScene.GetObject(objectId);
            Vector3 location = new Vector3();
            location.X = deckObject.NetworkLocation.X;
            location.Y = deckObject.NetworkLocation.Y;
            location.Z = Convert.ToSingle(zTextBox.Text);
            DeckProgram.DeckEngine.RequestObjectUpdate(objectId, deckObject.ObjectName, location, deckObject.NetworkOrientation, deckObject.Radius);
        }

        private void scaleTextBox_Leave(object sender, EventArgs e)
        {
            DeckObject deckObject = DeckProgram.DeckScene.GetObject(objectId);
            DeckProgram.DeckEngine.RequestObjectUpdate(objectId, deckObject.ObjectName, deckObject.NetworkLocation, deckObject.NetworkOrientation, Convert.ToSingle(scaleTextBox.Text));
        }

        private void xAddButton_Click(object sender, EventArgs e)
        {
            DeckObject deckObject = DeckProgram.DeckScene.GetObject(objectId);
            Vector3 location = new Vector3();
            location.X = deckObject.NetworkLocation.X+Convert.ToSingle(locationStepTextBox.Text);
            location.Y = deckObject.NetworkLocation.Y;
            location.Z = deckObject.NetworkLocation.Z;
            DeckProgram.DeckEngine.RequestObjectUpdate(objectId, deckObject.ObjectName, location, deckObject.NetworkOrientation, deckObject.Radius);
        }

        private void xSubButton_Click(object sender, EventArgs e)
        {
            DeckObject deckObject = DeckProgram.DeckScene.GetObject(objectId);
            Vector3 location = new Vector3();
            location.X = deckObject.NetworkLocation.X - Convert.ToSingle(locationStepTextBox.Text);
            location.Y = deckObject.NetworkLocation.Y;
            location.Z = deckObject.NetworkLocation.Z;
            DeckProgram.DeckEngine.RequestObjectUpdate(objectId, deckObject.ObjectName, location, deckObject.NetworkOrientation, deckObject.Radius);
        }

        private void yAddButton_Click(object sender, EventArgs e)
        {
            DeckObject deckObject = DeckProgram.DeckScene.GetObject(objectId);
            Vector3 location = new Vector3();
            location.X = deckObject.NetworkLocation.X;
            location.Y = deckObject.NetworkLocation.Y + Convert.ToSingle(locationStepTextBox.Text);
            location.Z = deckObject.NetworkLocation.Z;
            DeckProgram.DeckEngine.RequestObjectUpdate(objectId, deckObject.ObjectName, location, deckObject.NetworkOrientation, deckObject.Radius);
        }

        private void ySubButton_Click(object sender, EventArgs e)
        {
            DeckObject deckObject = DeckProgram.DeckScene.GetObject(objectId);
            Vector3 location = new Vector3();
            location.X = deckObject.NetworkLocation.X;
            location.Y = deckObject.NetworkLocation.Y - Convert.ToSingle(locationStepTextBox.Text);
            location.Z = deckObject.NetworkLocation.Z;
            DeckProgram.DeckEngine.RequestObjectUpdate(objectId, deckObject.ObjectName, location, deckObject.NetworkOrientation, deckObject.Radius);
        }

        private void zAddButton_Click(object sender, EventArgs e)
        {
            DeckObject deckObject = DeckProgram.DeckScene.GetObject(objectId);
            Vector3 location = new Vector3();
            location.X = deckObject.NetworkLocation.X;
            location.Y = deckObject.NetworkLocation.Y;
            location.Z = deckObject.NetworkLocation.Z + Convert.ToSingle(locationStepTextBox.Text);
            DeckProgram.DeckEngine.RequestObjectUpdate(objectId, deckObject.ObjectName, location, deckObject.NetworkOrientation, deckObject.Radius);
        }

        private void zSubButton_Click(object sender, EventArgs e)
        {
            DeckObject deckObject = DeckProgram.DeckScene.GetObject(objectId);
            Vector3 location = new Vector3();
            location.X = deckObject.NetworkLocation.X;
            location.Y = deckObject.NetworkLocation.Y;
            location.Z = deckObject.NetworkLocation.Z - Convert.ToSingle(locationStepTextBox.Text);
            DeckProgram.DeckEngine.RequestObjectUpdate(objectId, deckObject.ObjectName, location, deckObject.NetworkOrientation, deckObject.Radius);
        }

        private void scaleAddbutton_Click(object sender, EventArgs e)
        {
            DeckObject deckObject = DeckProgram.DeckScene.GetObject(objectId);
            DeckProgram.DeckEngine.RequestObjectUpdate(objectId, deckObject.ObjectName, deckObject.NetworkLocation, deckObject.NetworkOrientation, deckObject.Radius * (1+Convert.ToSingle(scaleStepTextBox.Text)/100.0f) );
        }

        private void scaleSubButton_Click(object sender, EventArgs e)
        {
            DeckObject deckObject = DeckProgram.DeckScene.GetObject(objectId);
            DeckProgram.DeckEngine.RequestObjectUpdate(objectId, deckObject.ObjectName, deckObject.NetworkLocation, deckObject.NetworkOrientation, deckObject.Radius * (1 - Convert.ToSingle(scaleStepTextBox.Text) / 100.0f));
        }

        private void yawAddButton_Click(object sender, EventArgs e)
        {
            DeckObject deckObject = DeckProgram.DeckScene.GetObject(objectId);
            Quaternion originalOrientation=deckObject.NetworkOrientation;
            Matrix originalOrientationMatrix;
            Common.Rotate(out originalOrientationMatrix, ref originalOrientation);

            Matrix resultOrientationMatrix;
            Common.RotateY(out resultOrientationMatrix, ref originalOrientationMatrix, Common.DegreesToRadians(Convert.ToSingle(orientationStepTextBox.Text)));

            Quaternion resultOrientation;
            Common.Rotate(out resultOrientation, ref resultOrientationMatrix);
            DeckProgram.DeckEngine.RequestObjectUpdate(objectId, deckObject.ObjectName, deckObject.NetworkLocation, resultOrientation, deckObject.Radius);
        }

        private void yawSubButton_Click(object sender, EventArgs e)
        {
            DeckObject deckObject = DeckProgram.DeckScene.GetObject(objectId);
            Quaternion originalOrientation = deckObject.NetworkOrientation;
            Matrix originalOrientationMatrix;
            Common.Rotate(out originalOrientationMatrix, ref originalOrientation);

            Matrix resultOrientationMatrix;
            Common.RotateY(out resultOrientationMatrix, ref originalOrientationMatrix, -Common.DegreesToRadians(Convert.ToSingle(orientationStepTextBox.Text)));

            Quaternion resultOrientation;
            Common.Rotate(out resultOrientation, ref resultOrientationMatrix);
            DeckProgram.DeckEngine.RequestObjectUpdate(objectId, deckObject.ObjectName, deckObject.NetworkLocation, resultOrientation, deckObject.Radius);
        }

        private void pitchAddButton_Click(object sender, EventArgs e)
        {
            DeckObject deckObject = DeckProgram.DeckScene.GetObject(objectId);
            Quaternion originalOrientation = deckObject.NetworkOrientation;
            Matrix originalOrientationMatrix;
            Common.Rotate(out originalOrientationMatrix, ref originalOrientation);

            Matrix resultOrientationMatrix;
            Common.RotateX(out resultOrientationMatrix, ref originalOrientationMatrix, Common.DegreesToRadians(Convert.ToSingle(orientationStepTextBox.Text)));

            Quaternion resultOrientation;
            Common.Rotate(out resultOrientation, ref resultOrientationMatrix);
            DeckProgram.DeckEngine.RequestObjectUpdate(objectId, deckObject.ObjectName, deckObject.NetworkLocation, resultOrientation, deckObject.Radius);
        }

        private void pitchSubButton_Click(object sender, EventArgs e)
        {
            DeckObject deckObject = DeckProgram.DeckScene.GetObject(objectId);
            Quaternion originalOrientation = deckObject.NetworkOrientation;
            Matrix originalOrientationMatrix;
            Common.Rotate(out originalOrientationMatrix, ref originalOrientation);

            Matrix resultOrientationMatrix;
            Common.RotateX(out resultOrientationMatrix, ref originalOrientationMatrix, -Common.DegreesToRadians(Convert.ToSingle(orientationStepTextBox.Text)));

            Quaternion resultOrientation;
            Common.Rotate(out resultOrientation, ref resultOrientationMatrix);
            DeckProgram.DeckEngine.RequestObjectUpdate(objectId, deckObject.ObjectName, deckObject.NetworkLocation, resultOrientation, deckObject.Radius);

        }

        private void rollAddButton_Click(object sender, EventArgs e)
        {
            DeckObject deckObject = DeckProgram.DeckScene.GetObject(objectId);
            Quaternion originalOrientation = deckObject.NetworkOrientation;
            Matrix originalOrientationMatrix;
            Common.Rotate(out originalOrientationMatrix, ref originalOrientation);

            Matrix resultOrientationMatrix;
            Common.RotateZ(out resultOrientationMatrix, ref originalOrientationMatrix, Common.DegreesToRadians(Convert.ToSingle(orientationStepTextBox.Text)));

            Quaternion resultOrientation;
            Common.Rotate(out resultOrientation, ref resultOrientationMatrix);
            DeckProgram.DeckEngine.RequestObjectUpdate(objectId, deckObject.ObjectName, deckObject.NetworkLocation, resultOrientation, deckObject.Radius);

        }

        private void rollSubButton_Click(object sender, EventArgs e)
        {
            DeckObject deckObject = DeckProgram.DeckScene.GetObject(objectId);
            Quaternion originalOrientation = deckObject.NetworkOrientation;
            Matrix originalOrientationMatrix;
            Common.Rotate(out originalOrientationMatrix, ref originalOrientation);

            Matrix resultOrientationMatrix;
            Common.RotateZ(out resultOrientationMatrix, ref originalOrientationMatrix, -Common.DegreesToRadians(Convert.ToSingle(orientationStepTextBox.Text)));

            Quaternion resultOrientation;
            Common.Rotate(out resultOrientation, ref resultOrientationMatrix);
            DeckProgram.DeckEngine.RequestObjectUpdate(objectId, deckObject.ObjectName, deckObject.NetworkLocation, resultOrientation, deckObject.Radius);

        }

    }
}
