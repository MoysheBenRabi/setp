using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IOT.Service;

namespace IOT.Controls
{
    public partial class ServiceController : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Refresh();
        }

        protected void ReserveServiceButton_Click(object sender, EventArgs e)
        {
            IotServiceController serviceController=IotContext.ServiceController;
            Refresh();
        }

        public void Refresh()
        {
            if (IotContext.HasServiceController)
            {
                ServiceReservedSelfCheckBox.Checked = true;
                ServiceReservedOtherCheckBox.Checked = false;
                Response.AddHeader("Refresh", "1");
            }
            else
            {
                ServiceReservedSelfCheckBox.Checked = false;
                ServiceReservedOtherCheckBox.Checked = IotServiceController.IsReserved;
            }
            ReservedTimeLeftTextBox.Text = IotServiceController.ReservedTimeLeft.ToString();
            ReserveServiceButton.Enabled = !IotServiceController.IsReserved;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            writer.WriteLine("<div class=\"code\">");
            writer.WriteLine("<pre>");
            writer.WriteLine(IotContext.TestSuiteState.ReferenceServerLog);
            writer.WriteLine("</pre>");
            writer.WriteLine("</div>");
        }
    }
}