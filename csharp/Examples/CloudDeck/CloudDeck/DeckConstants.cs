using System;
using System.Collections.Generic;
using System.Text;

namespace CloudDeck
{
    public static class DeckConstants
    {

        public static string ProgramName
        {
            get
            {
                return "Cloud Deck";
            }
        }

        public static byte ProgramMajorVersion
        {
            get
            {
                return (byte) typeof(DeckConstants).Assembly.GetName().Version.Major;
            }
        }

        public static byte ProgramMinorVersion
        {
            get
            {
                return (byte) typeof(DeckConstants).Assembly.GetName().Version.Minor;
            }
        }

        public static int ProgramSourceRevision
        {
            get
            {
                return Convert.ToInt32("$Revision: 253 $".Split(' ')[1]);
            }
        }

    }
}
