using System;
using System.Collections.Generic;

using System.Text;

namespace MXP
{
    /// <summary>
    /// SessionState enumeration contains the session states.
    /// </summary>
    public enum SessionState
    {
        /// <summary>
        /// Session is forming connection.
        /// </summary>
        Connecting,
        /// <summary>
        /// Session is connected.
        /// </summary>
        Connected,
        /// <summary>
        /// Session has been disconnected.
        /// </summary>
        Disconnected
    }
}
