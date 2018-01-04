using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace MXP
{
    /// <summary>
    /// SessionKey uniquely identifies session.
    /// </summary>
    public class SessionKey
    {
        /// <summary>
        /// Remote IP end point of the session.
        /// </summary>
        public IPEndPoint RemoteEndPoint;
        /// <summary>
        /// Session id used in incoming packets of this session.
        /// </summary>
        public uint IncomingSessionId;

        /// <summary>
        /// Constructs the SessionKey and populates end point and incoming session id fields.
        /// </summary>
        /// <param name="remoteEndPoint">Remote ip endpoint of this session. In other words remote peer.</param>
        /// <param name="incomingSessionId">The session id used in incoming packets. In other words downstream session id.</param>
        public SessionKey(IPEndPoint remoteEndPoint, uint incomingSessionId)
        {
            this.RemoteEndPoint = remoteEndPoint;
            this.IncomingSessionId = incomingSessionId;
        }

        /// <summary>
        /// Constructs clone of given sesssion key.
        /// </summary>
        /// <param name="key">Session key to clone from.</param>
        public SessionKey(SessionKey key)
        {
            this.RemoteEndPoint = key.RemoteEndPoint;
            this.IncomingSessionId = key.IncomingSessionId;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(SessionKey))
            {
                return false;
            }
            SessionKey key = (SessionKey)obj;
            return RemoteEndPoint.Equals(key.RemoteEndPoint) && IncomingSessionId == key.IncomingSessionId;
        }

        public override int GetHashCode()
        {
            return RemoteEndPoint.GetHashCode()+IncomingSessionId.GetHashCode();
        }

        public override string ToString()
        {
            return "SessionKey RemoteEndPoint: " + RemoteEndPoint + " IncomingSessionId: " + IncomingSessionId;
        }
    }
}
