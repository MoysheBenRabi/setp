using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IOT.Model;

namespace IOT.Controls
{
    public partial class TestState : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            TestSuiteState suiteState = IotContext.TestSuiteState;
            writer.WriteLine("<table class=\"grid\">");

            writer.WriteLine("<tr><th>Test Category</th>");
            writer.WriteLine("<th><div title=\"Passed \"><u>P</u></div></th>");
            writer.WriteLine("<th><div title=\"Failed\"><u>F</u></div></th>");
            writer.WriteLine("<th><div title=\"Missing\"><u>M</u></div></th>");
            writer.WriteLine("<th><div title=\"Total\"><u>T</u></div></th>");
            writer.WriteLine("<th></th></tr>");

            WriteResultRow(writer, suiteState, TestCategory.MessageSerialization);
            WriteResultRow(writer, suiteState, TestCategory.CandidateClientToReferenceServer);
            WriteResultRow(writer, suiteState, TestCategory.ReferenceClientToCandidateServer);
            WriteResultRow(writer, suiteState, TestCategory.CandidateServerToReferenceServer);
            WriteResultRow(writer, suiteState, TestCategory.ReferenceServerToCandidateServer);            

            writer.WriteLine("</table>");
        }

        private void WriteResultRow(HtmlTextWriter writer, TestSuiteState suiteState, TestCategory category)
        {
            int passedCount = suiteState.GetCategoryPassedCount(category);
            int failedCount = suiteState.GetCategoryFailedCount(category);
            int testCount = suiteState.GetCategoryTestCount(category);
            int missingCount = testCount - passedCount - failedCount;

            writer.WriteLine("<tr>");
            writer.WriteLine("<td>" + category + "</td>");

            writer.WriteLine("<td>");
            writer.WriteLine(passedCount);
            writer.WriteLine("</td>");

            writer.WriteLine("<td>");
            writer.WriteLine(failedCount);
            writer.WriteLine("</td>");

            writer.WriteLine("<td>");
            writer.WriteLine(missingCount);
            writer.WriteLine("</td>");

            writer.WriteLine("<td>");
            writer.WriteLine(testCount);
            writer.WriteLine("</td>");

            
            writer.WriteLine("<td>");
            if (testCount == missingCount)
            {
                writer.WriteLine("<img src=\"Images/missing.png\" />");
            }
            else if (passedCount == testCount)
            {
                writer.WriteLine("<img src=\"Images/passed.png\" />");
            }
            else
            {
                writer.WriteLine("<img src=\"Images/failed.png\" />");
            }
            writer.WriteLine("</td>");
            writer.WriteLine("</tr>");

        }
    }
}