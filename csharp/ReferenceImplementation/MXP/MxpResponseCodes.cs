using System;
using System.Collections.Generic;

using System.Text;

namespace MXP
{
    public class MxpResponseCodes
    {
        public const byte SUCCESS = 0;
        public const byte UNAUTHORIZED_OPERATION = 1;
        public const byte UNKNOWN_ID = 2;
        public const byte RESERVED_ID = 3;
        public const byte BANNED_ID = 4;
        public const byte UNSUPPORTED_OPERATION = 5;
    }
}
