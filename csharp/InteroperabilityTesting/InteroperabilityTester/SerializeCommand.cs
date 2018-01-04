using System;
using System.Collections.Generic;
using System.Text;
using IOT.Encoding;
using MXP;
using Ionic.Zip;
using System.IO;

namespace InteroperabilityTester
{
    public class SerializeCommand
    {
        public static void Execute()
        {
            if(!Directory.Exists("messages"))
            {
                Directory.CreateDirectory("messages");
            }

            using (ZipFile zip = new ZipFile())
            {
                foreach (ReferenceMessage item in ReferenceMessageLoader.Current.ReferenceMessages.Values)
                {
                    ZipEntry entry = zip.AddEntry(item.MessageFileName, "messages", item.ByteValue);
                }
                zip.Save("mxp_" + MxpConstants.ProtocolMajorVersion + "_" + MxpConstants.ProtocolMinorVersion + "_reference_messages.zip");
            }

        }
    }
}
