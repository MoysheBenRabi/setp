using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ionic.Zip;
using System.IO;
using IOT.Encoding;
using IOT.Util;

namespace IOT.Controls
{
    public partial class MessageSerializationVerification : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void VerifyButton_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                using (ZipFile zipFile = ZipFile.Read(FileUpload1.FileBytes))
                {
                    foreach (ZipEntry entry in zipFile.Entries)
                    {
                        string fileName = entry.FileName;
                        if (fileName.IndexOf('/')>-1)
                        {
                            fileName = fileName.Substring(fileName.LastIndexOf('/') + 1);
                        }

                        if (!IotContext.TestSuiteState.MessageTestStates.ContainsKey(fileName))
                        {
                            continue;
                        }

                        MessageTestState messageTestState = IotContext.TestSuiteState.MessageTestStates[fileName];

                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            entry.Extract(memoryStream);
                            byte[] sourceBytes = memoryStream.GetBuffer();
                            byte[] targetBytes = new byte[memoryStream.Length];
                            for (int i = 0; i < targetBytes.Length; i++)
                            {
                                targetBytes[i] = sourceBytes[i];
                            }
                            messageTestState.CandidateBytes = targetBytes;
                            messageTestState.DifferenceIndexes = CompareUtil.FindFirstDifference(messageTestState.ReferenceBytes, messageTestState.CandidateBytes);
                            messageTestState.Result = messageTestState.DifferenceIndexes.Count == 0;
                        }
                    }
                }

            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            if (IotContext.TestSuiteState.MessageTestStates == null)
            {
                return;
            }

            writer.WriteLine("<h2>Verification Results</h2>");
            writer.WriteLine("<table class=\"grid\">");
            writer.WriteLine("<tr><th>State</th><th>Message Name</th><th>Bytes</th><th>Candidate Bytes</th></tr>");

            int totalItems = 0;
            int passedItems = 0;
            foreach (MessageTestState item in IotContext.TestSuiteState.MessageTestStates.Values)
            {
                totalItems++;
                writer.WriteLine("<tr>");
                if (item.Result.HasValue)
                {
                    if (item.Result == true)
                    {
                        writer.WriteLine("<td><img src=\"Images\\passed.png\"></td>");
                    }
                    else
                    {
                        writer.WriteLine("<td><img src=\"Images\\failed.png\"></td>");
                    }
                }
                else
                {
                    writer.WriteLine("<td><img src=\"Images\\missing.png\"></td>");
                }
                writer.WriteLine("<td>" + item.MessageName + "</td>");
                if (item.Result.HasValue)
                {
                    if (item.Result==true)
                    {
                        writer.WriteLine("<td colspan=\"2\"></td>");
                        passedItems++;
                    }
                    else
                    {
                        writer.WriteLine("<td class=\"bytes\">" + RenderUtil.RenderByteArray(item.ReferenceBytes,item.DifferenceIndexes,10)+ "</td>");
                        writer.WriteLine("<td class=\"bytes\">" + RenderUtil.RenderByteArray(item.CandidateBytes, item.DifferenceIndexes, 10) + "</td>");
                    }
                }
                else
                {
                        writer.WriteLine("<td colspan=\"2\"></td>");
                }

            }
            writer.WriteLine("</table>");
        }
    }
}