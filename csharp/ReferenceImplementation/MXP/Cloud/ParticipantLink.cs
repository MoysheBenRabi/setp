using System;
using System.Collections.Generic;
using System.Text;

namespace MXP.Cloud
{
    public class ParticipantLink
    {
        public Guid ParticipantId;
        public Guid AvatarId;
        public string ParticipantIdentifier;
        public ulong ParticipantRealTime;

        public string ProgramName;
        public byte ProgramMajorVersion;
        public byte ProgramMinorVersion;
        public byte ProtocolMajorVersion;
        public byte ProtocolMinorVersion;
        public uint ProtocolSourceRevision;

        public bool InitialObjectSendListFillDone = false;
        public IDictionary<Guid, DateTime> ObservedObjects=new Dictionary<Guid, DateTime>();
        public IList<Guid> ObjectSendList = new List<Guid>();
    }
}
