using System;
using System.Collections.Generic;
using System.Text;
using MXP;

namespace IOT.Encoding
{
    public class MessageTestState
    {
        public string MessageName;
        public string MessageFileName;

        public Message ReferenceMessage;
        public string ReferenceString;
        public byte[] ReferenceBytes;

        public byte[] CandidateBytes;
        public Nullable<bool> Result;
        public List<int> DifferenceIndexes;
    }
}
