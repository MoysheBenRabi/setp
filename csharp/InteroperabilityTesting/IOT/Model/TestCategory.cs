using System;
using System.Collections.Generic;
using System.Text;

namespace IOT.Model
{
    public enum TestCategory
    {
        MessageSerialization,
        CandidateClientToReferenceServer,
        ReferenceClientToCandidateServer,
        CandidateServerToReferenceServer,
        ReferenceServerToCandidateServer
    }
}
