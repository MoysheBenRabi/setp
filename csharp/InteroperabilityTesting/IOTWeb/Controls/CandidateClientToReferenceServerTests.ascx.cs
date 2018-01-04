using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IOT.Model;

namespace IOT.Controls
{
    public partial class CandidateClientToReferenceServerTests : System.Web.UI.UserControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            TestSuiteState suiteState = IotContext.TestSuiteState;
            writer.WriteLine("<table class=\"grid\">");

            writer.WriteLine("<tr><th></th><th>Test Name</th><th>Error Message</th></tr>");

            foreach (IOT.Model.TestState testState in suiteState.GetTestStates(TestCategory.CandidateClientToReferenceServer))
            {
                WriteResultRow(writer, testState);
            }

            writer.WriteLine("</table>");
        }

        private void WriteResultRow(HtmlTextWriter writer, IOT.Model.TestState testState)
        {
            writer.WriteLine("<tr>");
            writer.WriteLine("<td>");
            if (testState.Result.HasValue&&testState.Result.Value)
            {
                writer.WriteLine("<img src=\"Images/passed.png\" />");
            }
            else if (testState.Result.HasValue && !testState.Result.Value)
            {
                writer.WriteLine("<img src=\"Images/failed.png\" />");
            }
            else
            {
                writer.WriteLine("<img src=\"Images/missing.png\" />");
            }
            writer.WriteLine("</td>");
            writer.WriteLine("<td>"+testState.Key+"</td>");
            writer.WriteLine("<td>");
            writer.WriteLine(testState.ErrorMessage!=null?testState.ErrorMessage:"");
            writer.WriteLine("</td>");
            writer.WriteLine("</tr>");
        }
    }
}