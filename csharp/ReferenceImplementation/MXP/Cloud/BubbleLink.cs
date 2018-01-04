using System;
using System.Collections.Generic;
using System.Text;
using MXP.Common.Proto;

namespace MXP.Cloud
{
    /// <summary>
    /// BubbleLink reflects a link between two bubbles.
    /// BubbleLink may either be connected or not.
    /// BubbleLink may be permanent link or created on the fly.
    /// BubbleLink may be initiator meaning this end will initiate the connection.
    /// BubbleLink may be disabled.
    /// </summary>
    public class BubbleLink
    {
        public Guid RemoteBubbleId = Guid.Empty;
        public Guid RemoteBubbleOwnerId = Guid.Empty;
        public string RemoteBubbleName;
        public float RemoteBubbleRange;
        public float RemoteBubblePerceptionRange;
        public string RemoteBubbleAssetCacheUrl;
        public string RemoteHubAddress;
        public int RemoteHubPort;
        public string RemoteHubProgram;
        public byte RemoteHubProgramMajorVersion;
        public byte RemoteHubProgramMinorVersion;
        public byte RemoteProtocolMajorVersion;
        public byte RemoteProtocolMinorVersion;
        public uint RemoteProtocolSourceRevision;
        public MsdVector3f RemoteBubbleCenter=new MsdVector3f();
        public bool IsInitiator;
        public bool IsConnected;
        public bool IsConnecting;
        public bool IsPermanent;
        public bool IsEnabled;
        public DateTime LastConnect = DateTime.MinValue;
        public DateTime LastDisconnect = DateTime.MinValue;
        public DateTime ScheduledConnect = DateTime.MinValue;
        public DateTime ScheduledDisconnect = DateTime.MinValue;

        public IDictionary<Guid, DateTime> ObservedObjects = new Dictionary<Guid, DateTime>();
        public IList<Guid> ObjectSendList = new List<Guid>();
    }
}
