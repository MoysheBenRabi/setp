using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ionic.Zip;
using System.IO;
using MXP;
using IOT.Encoding;

namespace IOT
{
    public partial class reference_messages_zip : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AddHeader("Content-Type", "application/zip");
            Response.AddHeader("Content-Disposition", "attachment; filename=\"mxp_" + MxpConstants.ProtocolMajorVersion + "_" + MxpConstants.ProtocolMinorVersion + "_reference_messages.zip\"");
            using (MemoryStream stream = new MemoryStream())
            {
                using (ZipFile zip = new ZipFile())
                {
                    foreach (ReferenceMessage item in ReferenceMessageLoader.Current.ReferenceMessages.Values)
                    {
                        ZipEntry entry = zip.AddEntry(item.MessageFileName, "messages", item.ByteValue);
                    }
                    zip.Save(stream);
                }
                Response.OutputStream.Write(stream.GetBuffer(), 0, (int)stream.Length);
            }
        }
    }
}
