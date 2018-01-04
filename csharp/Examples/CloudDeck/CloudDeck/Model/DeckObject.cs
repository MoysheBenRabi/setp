using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudMath;
using System.Windows.Forms;
using Horde3DNET;
using MXP.Util;

namespace CloudDeck.Model
{
    public class DeckObject
    {
        public Guid ObjectId;
        public string ObjectName;
        public string ModelUrl;
        public string TypeName;
        public float Radius;
        public float ModelScale;
        public int RenderId;

        public bool IsActive = false;
        public bool IsAvatar;

        public DateTime m_LastProcessTime = DateTime.MinValue;

        private Vector3 m_networkLocation;
        private Quaternion m_networkOrientation;
        private Vector3 m_networkScale;

        public Vector3 RudderLocation;
        public Quaternion RudderOrientation;
        public Vector3 RudderScale;

        public Vector3 NetworkLocation
        {
            set
            {
                m_networkLocation = value;
            }
            get
            {
                return m_networkLocation;
            }
        }
        public Quaternion NetworkOrientation
        {
            set
            {
                m_networkOrientation = value;
            }
            get
            {
                return m_networkOrientation;
            }
        }
        public Vector3 NetworkScale
        {
            set
            {
                m_networkScale = value;
            }
            get
            {
                return m_networkScale;
            }
        }

        private Vector3 m_renderLocation;
        private Quaternion m_renderOrientation;
        private Vector3 m_renderScale;

        public Vector3 RenderLocation
        {
            get
            {
                if (IsActive)
                {
                    return m_renderLocation;
                }
                else
                {
                    return m_networkLocation;
                }
            }
        }
        public Quaternion RenderOrientation
        {
            get
            {
                if (IsActive)
                {
                    return m_renderOrientation;
                }
                else
                {
                    return m_networkOrientation;
                }
            }
        }
        public Vector3 RenderScale
        {
            get
            {
                if (IsActive)
                {
                    return m_renderScale;
                }
                else
                {
                    return m_networkScale;
                }
            }
        }

        public DeckObject(Guid objectId, String objectName,String typeName,String modelUrl, float modelScale, Vector3 location, Quaternion orientation, float radius, Vector3 scale)
        {
            ObjectId = objectId;
            ObjectName = objectName;
            ModelUrl = modelUrl;
            TypeName = typeName;
            Radius = radius;
            ModelScale = modelScale;
            m_networkLocation = location;
            m_renderLocation = location;
            m_networkOrientation = orientation;
            m_renderOrientation = orientation;
            m_networkScale = scale;
            m_renderScale = scale;
        }

        public void Process()
        {
            DateTime now = DateTime.Now;
            
            double secondsSinceLastUpdate = now.Subtract(m_LastProcessTime).TotalSeconds;
            if (secondsSinceLastUpdate > 0.1)
            {
                secondsSinceLastUpdate = 0.1;
            }

            float synchronizationTimeSeconds = 0.2f;
            float synchronizationTimeWindowRatio = (float)(secondsSinceLastUpdate / synchronizationTimeSeconds);
            if (synchronizationTimeWindowRatio > 1)
            {
                synchronizationTimeWindowRatio = 1;
            }

            m_LastProcessTime = now;

            m_renderLocation = (m_networkLocation - m_renderLocation) * synchronizationTimeWindowRatio + m_renderLocation;

            m_renderScale = (m_networkScale - m_renderScale) * synchronizationTimeWindowRatio + m_renderScale;

            Quaternion result;
            Common.Slerp(out result, ref m_renderOrientation, ref m_networkOrientation, synchronizationTimeWindowRatio);
            m_renderOrientation = result;


            if (TypeName.Equals("Avatar"))
            {
                // Rudder object walking for own avatar
                if(DeckProgram.DeckEngine.AvatarId==ObjectId){
                    Matrix orientationMatrix;
                    Common.Rotate(out orientationMatrix, ref RudderOrientation);
                    Vector3 delta = 2 * Vector3.TransformNormal(Vector3.Down, orientationMatrix);
                    Vector3 origin = RudderLocation;
                    origin += Vector3.TransformNormal(Vector3.Up, orientationMatrix);
                    Vector3 intersection;

                    float distance;
                    int intersectingNodeRenderId;
                    if (DeckProgram.DeckRenderer.GetIntersectionPoint(RenderId, ref origin, ref delta, out intersectingNodeRenderId, out intersection, out distance))
                    {
                        //LogUtil.Debug("Intersection: " + intersection + " distance: " + distance + " own id: " + RenderId + " intersecting id: " + intersectingNodeRenderId);
                        RudderLocation = intersection;
                    }
                }

                // Render object walking for all avatars
                {
                    Matrix orientationMatrix;
                    Common.Rotate(out orientationMatrix, ref m_renderOrientation);
                    Vector3 delta = 2 * Vector3.TransformNormal(Vector3.Down, orientationMatrix);
                    Vector3 origin = m_renderLocation;
                    origin += Vector3.TransformNormal(Vector3.Up, orientationMatrix);
                    Vector3 intersection;

                    float distance;
                    int intersectingNodeRenderId;
                    if (DeckProgram.DeckRenderer.GetIntersectionPoint(RenderId, ref origin, ref delta, out intersectingNodeRenderId, out intersection, out distance))
                    {
                        //LogUtil.Debug("Intersection: " + intersection + " distance: " + distance + " own id: " + RenderId + " intersecting id: " + intersectingNodeRenderId);
                        m_renderLocation = intersection;
                    }
                }
            }
            
        }

    }
}
