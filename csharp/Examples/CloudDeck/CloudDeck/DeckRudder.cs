using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Horde3DNET;
using Horde3DNET.Utils;
using System.Drawing;
using CloudMath;
using MXP.Util;

namespace CloudDeck
{
    /// <summary>
    /// Deck rudder handles in 3d steering.
    /// </summary>
    public class DeckRudder
    {

        #region Fields

        private Vector3 m_avatarTargetLocation = Vector3.Zero;
        public Vector3 AvatarTargetLocation
        {
            get
            {
                if (DeckProgram.DeckScene.ContainsObject(DeckProgram.DeckEngine.AvatarId))
                {
                    m_avatarTargetLocation = DeckProgram.DeckScene.GetObject(DeckProgram.DeckEngine.AvatarId).RudderLocation;
                    return DeckProgram.DeckScene.GetObject(DeckProgram.DeckEngine.AvatarId).RudderLocation;
                }
                else
                {
                    return m_avatarTargetLocation;
                }
            }
            set
            {
                if (DeckProgram.DeckScene.ContainsObject(DeckProgram.DeckEngine.AvatarId))
                {
                    m_avatarTargetLocation = value;
                    DeckProgram.DeckScene.GetObject(DeckProgram.DeckEngine.AvatarId).RudderLocation=value;
                }
                else
                {
                    m_avatarTargetLocation=value;
                }
            }
        }
        public Matrix m_avatarTargetOrientationMatrix = Matrix.Identity;
        public Quaternion m_avatarTargetOrientation = Quaternion.Identity;
        public Matrix AvatarTargetOrientationMatrix
        {
            get
            {
                if (DeckProgram.DeckScene.ContainsObject(DeckProgram.DeckEngine.AvatarId))
                {
                    // Orientation is stored in rudder as matrix because it is needed in matrix format for transforming vectors.
                    return m_avatarTargetOrientationMatrix;
                }
                else
                {
                    return m_avatarTargetOrientationMatrix;
                }
            }
            set
            {
                if (DeckProgram.DeckScene.ContainsObject(DeckProgram.DeckEngine.AvatarId))
                {
                    m_avatarTargetOrientationMatrix = value;
                    Quaternion quaternion;
                    Common.Rotate(out quaternion, ref m_avatarTargetOrientationMatrix);
                    m_avatarTargetOrientation = quaternion;
                    DeckProgram.DeckScene.GetObject(DeckProgram.DeckEngine.AvatarId).RudderOrientation = quaternion;
                }
                else
                {
                    m_avatarTargetOrientationMatrix = value;
                }
            }
        }

        public Vector3 AvatarCurrentLocation = Vector3.Zero;
        public Matrix AvatarCurrentOrientationMatrix = Matrix.Identity;

        public Vector3 CameraLocation = Vector3.Zero;
        private Quaternion m_cameraOrientation = Quaternion.Identity;
        public Matrix CameraOrientationMatrix = Matrix.Identity;

        public Vector3 FlashlightLocation = Vector3.Zero;
        public Matrix FlashlightOrientationMatrix = Matrix.Identity;


        private float m_cameraDistance = 10f;
        private float m_observerModeVelocity = 50.0f;
        private HashSet<Keys> m_pressedKeys = new HashSet<Keys>();
        private bool m_isSteered = false;
        private DateTime m_LastCameraUpdateTime;
        
        #endregion

        #region Constructor

        public DeckRudder()
        {
        }

        #endregion

        #region Processing

        public void Process()
        {
            ProcessKeyboard();
        }

        public void UpdateCameraTransformation()
        {
            DateTime now = DateTime.Now;

            double secondsSinceLastUpdate = now.Subtract(m_LastCameraUpdateTime).TotalSeconds;
            if (secondsSinceLastUpdate > 0.05)
            {
                secondsSinceLastUpdate = 0.05;
            }

            float synchronizationTimeSeconds = 0.1f;
            float synchronizationTimeWindowRatio = (float)(secondsSinceLastUpdate / synchronizationTimeSeconds);
            if (synchronizationTimeWindowRatio > 1)
            {
                synchronizationTimeWindowRatio = 1;
            }

            m_LastCameraUpdateTime = now;

            Quaternion resultOrientation;
            Common.Slerp(out resultOrientation, ref m_cameraOrientation, ref m_avatarTargetOrientation, synchronizationTimeWindowRatio);
            m_cameraOrientation = resultOrientation;

            Common.Rotate(out CameraOrientationMatrix, ref m_cameraOrientation);

            Vector3 cameraTargetLocation = AvatarTargetLocation;
            cameraTargetLocation -= Vector3.TransformNormal(Vector3.Backward, CameraOrientationMatrix) * m_cameraDistance;
            cameraTargetLocation += Vector3.TransformNormal(Vector3.Up, CameraOrientationMatrix) * 0.3f * m_cameraDistance;
            CameraLocation = (cameraTargetLocation - CameraLocation) * synchronizationTimeWindowRatio + CameraLocation;

            FlashlightOrientationMatrix = AvatarCurrentOrientationMatrix;
            FlashlightLocation = AvatarCurrentLocation;
            FlashlightLocation -= Vector3.TransformNormal(Vector3.Forward, FlashlightOrientationMatrix) * 1;
            FlashlightLocation += Vector3.TransformNormal(Vector3.Up, FlashlightOrientationMatrix) * 1;
        }

        public void SynchronizeCameraCurrentLocationWithTargetLocation()
        {
            m_cameraOrientation = m_avatarTargetOrientation;
            Vector3 cameraTargetLocation = AvatarTargetLocation;
            cameraTargetLocation -= Vector3.TransformNormal(Vector3.Backward, CameraOrientationMatrix) * m_cameraDistance;
            cameraTargetLocation += Vector3.TransformNormal(Vector3.Up, CameraOrientationMatrix) * 0.3f * m_cameraDistance;
            CameraLocation = cameraTargetLocation;
        }

        public void ProcessKeyboard()
        {
            if (DeckProgram.MainForm.IsKeyboardSteeringEnabled)
            {
                float curVel = m_observerModeVelocity/DeckProgram.DeckRenderer.CurrentFramesPerSecond;

                if (IsKeyDown(Keys.W))
                {
                    AvatarTargetLocation -= Vector3.TransformNormal(Vector3.Forward,AvatarTargetOrientationMatrix);
                    m_isSteered = true;
                }

                if (IsKeyDown(Keys.S))
                {
                    AvatarTargetLocation -= Vector3.TransformNormal(Vector3.Backward, AvatarTargetOrientationMatrix);
                    m_isSteered = true;
                }

                if (IsKeyDown(Keys.E))
                {
                    AvatarTargetLocation += 1.5f*Vector3.TransformNormal(Vector3.Up, AvatarTargetOrientationMatrix);
                    m_isSteered = true;
                }

                if (IsKeyDown(Keys.C))
                {
                    AvatarTargetLocation += 1.5f*Vector3.TransformNormal(Vector3.Down, AvatarTargetOrientationMatrix);
                    m_isSteered = true;
                }

                if (IsKeyDown(Keys.A))
                {
                    AvatarTargetLocation += Vector3.TransformNormal(Vector3.Left, AvatarTargetOrientationMatrix);
                    m_isSteered = true;
                }

                if (IsKeyDown(Keys.D))
                {
                    AvatarTargetLocation += Vector3.TransformNormal(Vector3.Right, AvatarTargetOrientationMatrix);
                    m_isSteered = true;
                }

                if (IsKeyDown(Keys.D1)) // 1
                {
                    DeckProgram.DeckRenderer.AnimationBlendingWeight += 2/
                                                                        DeckProgram.DeckRenderer.CurrentFramesPerSecond;
                    if (DeckProgram.DeckRenderer.AnimationBlendingWeight > 1)
                        DeckProgram.DeckRenderer.AnimationBlendingWeight = 1;
                }
                if (IsKeyDown(Keys.D2)) // 2
                {
                    DeckProgram.DeckRenderer.AnimationBlendingWeight -= 2/
                                                                        DeckProgram.DeckRenderer.CurrentFramesPerSecond;
                    if (DeckProgram.DeckRenderer.AnimationBlendingWeight < 0)
                        DeckProgram.DeckRenderer.AnimationBlendingWeight = 0;
                }

            }
        }

        #endregion

        #region Public Interface

        public bool IsSteered
        {
            get
            {
                return m_isSteered;
            }
            set
            {
                m_isSteered = value;
            }
        }

        public void KeyDown(Keys key)
        {
            if (!m_pressedKeys.Contains(key))
            {
                m_pressedKeys.Add(key);
            }
            switch (key)
            {
                case Keys.Space:
                    DeckProgram.DeckRenderer.IsFreeze = !DeckProgram.DeckRenderer.IsFreeze;
                    break;
                case Keys.F3:
                    if (h3d.getNodeParamI(DeckProgram.DeckRenderer.CameraHid, (int)h3d.H3DCamera.PipeResI) == DeckProgram.DeckRenderer.HdrPipeHid)
                        h3d.setNodeParamI(DeckProgram.DeckRenderer.CameraHid, (int)h3d.H3DCamera.PipeResI, DeckProgram.DeckRenderer.ForwardPipeHid);
                    else
                        h3d.setNodeParamI(DeckProgram.DeckRenderer.CameraHid, (int)h3d.H3DCamera.PipeResI, DeckProgram.DeckRenderer.HdrPipeHid);
                    break;
                case Keys.F7:
                    DeckProgram.DeckRenderer.IsDebugViewMode = !DeckProgram.DeckRenderer.IsDebugViewMode;
                    break;
                case Keys.F8:
                    DeckProgram.DeckRenderer.IsWireframeMode = !DeckProgram.DeckRenderer.IsWireframeMode;
                    break;
                case Keys.F9:
                    DeckProgram.DeckRenderer.StatisticsMode += 1;
                    if (DeckProgram.DeckRenderer.StatisticsMode > Horde3DUtils.MaxStatMode) DeckProgram.DeckRenderer.StatisticsMode = 0;
                    break;
            }
        }

        public void KeyUp(Keys key)
        {
            if (m_pressedKeys.Contains(key))
            {
                m_pressedKeys.Remove(key);
            }
        }

        public bool IsKeyDown(Keys key)
        {
            return m_pressedKeys.Contains(key);
        }

        public void ProcessMouse(float dX, float dY)
        {
            AvatarTargetOrientationMatrix = Matrix.Multiply(AvatarTargetOrientationMatrix,Matrix.RotationY(Common.DegreesToRadians(-dX / 100 * 30)));
            AvatarTargetOrientationMatrix = Matrix.Multiply(Matrix.RotationX(Common.DegreesToRadians(-dY / 100 * 30)), AvatarTargetOrientationMatrix);            

            m_isSteered = true;
        }

        internal void MouseWheel(int p)
        {
            m_cameraDistance -= p*0.03f;
            UpdateCameraTransformation();
        }

        #endregion

    }
}
