using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudDeck.Model
{
    public class DeckObjectType
    {
        public Guid TypeId;
        public String TypeName;

        public override string ToString()
        {
            return TypeName;
        }
    }
}
