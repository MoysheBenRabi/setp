using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IOT.Encoding;
using IOT.Util;
using Ionic.Zip;

namespace IOT.Controls
{
    public partial class ReferenceMessageList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            writer.WriteLine("<h2>Reference Messages</h2>");
            writer.WriteLine("<div class=\"code\">");
            writer.WriteLine("<table class=\"grid\">");
            writer.WriteLine("<tr><th>Message Name</th><th>File Name</th><th width=\"300\">Content</th><th width=\"300\">Bytes</th></tr>");
            foreach (ReferenceMessage item in ReferenceMessageLoader.Current.ReferenceMessages.Values)
            {
                writer.WriteLine("<tr>");
                writer.WriteLine("<td>"+item.MessageName+"</td>");
                writer.WriteLine("<td>"+item.MessageFileName+"</td>");
                writer.WriteLine("<td nowrap>" + RenderUtil.FixedWrapString(item.StringValue, 60) + "</td>");
                writer.WriteLine("<td nowrap>" + RenderUtil.RenderByteArray(item.ByteValue,new List<int>(), 20) + "</td>");
                writer.WriteLine("</tr>");
            }
            writer.WriteLine("</table>");
            writer.WriteLine("</div>");

        }

        protected void DownloadReferenceMessagesButton_Click(object sender, EventArgs e)
        {
            Server.Transfer("reference_messages_zip.aspx");
        }
    }
}