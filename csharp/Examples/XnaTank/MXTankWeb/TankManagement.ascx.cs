using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MXTank;
using MXP;

namespace MXTankWeb
{
    public partial class TankManagement : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TankManager.Tank.Startup();
            TankManager.BoxKickerDaemonParticipant.Startup();
        }

        protected void StartTankButton_Click(object sender, EventArgs e)
        {
            TankManager.Tank.Startup();
        }

        protected void StopTankButton_Click(object sender, EventArgs e)
        {
            TankManager.Tank.Shutdown();
        }

        protected void RefreshButton_Click(object sender, EventArgs e)
        {

        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            writer.WriteLine("<div>");

            Tank tank = TankManager.Tank;
            writer.WriteLine("<h2>Tank is running: " + tank.IsAlive+"</h2>");

            writer.WriteLine("<table>");
            writer.WriteLine("<tr><td>Server Packets Received</td><td>" + tank.ServerPacketsReceived+ "</td></tr>");
            writer.WriteLine("<tr><td>Server Packets Sent</td><td>" + tank.ServerPacketsSent + "</td></tr>");
            writer.WriteLine("<tr><td>Server Bytes Received</td><td>" + tank.ServerBytesReceived + "</td></tr>");
            writer.WriteLine("<tr><td>Server Bytes Sent</td><td>" + tank.ServerBytesSent + "</td></tr>");
            writer.WriteLine("<tr><td>Hub Packets Received</td><td>" + tank.HubPacketsReceived + "</td></tr>");
            writer.WriteLine("<tr><td>Hub Packets Sent</td><td>" + tank.HubPacketsSent + "</td></tr>");
            writer.WriteLine("<tr><td>Hub Bytes Received</td><td>" + tank.HubBytesReceived + "</td></tr>");
            writer.WriteLine("<tr><td>Hub Bytes Sent</td><td>" + tank.HubBytesSent + "</td></tr>");
            writer.WriteLine("</table>");


            List<TankBubble> bubbles = tank.GetBubbles();
            foreach (TankBubble bubble in bubbles)
            {
                writer.WriteLine("<table>");
                writer.WriteLine("<tr><td>Bubble Id</td><td>"+bubble.BubbleId+"</td></tr>");
                writer.WriteLine("<tr><td>Bubble Name</td><td>" + bubble.BubbleName + "</td></tr>");
                writer.WriteLine("<tr><td>Number of Connected Bubbles</td><td>" + bubble.Bubbles.Count + "</td></tr>");
                writer.WriteLine("<tr><td>Number of Connected Participants</td><td>" + bubble.Participants.Count + "</td></tr>");
                writer.WriteLine("<tr><td>Number of Injected Objects</td><td>" + bubble.Objects.Count + "</td></tr>");
                writer.WriteLine("</table>");
            }

            /*
            TestParticipant testParticipant = TankManager.TestParticipant;

            writer.WriteLine("<h2>Test Participant is running:" + testParticipant.IsAlive + "</h2>");
            writer.WriteLine("<table>");
            writer.WriteLine("<tr><td>Test Participant Id</td><td>" + testParticipant.ParticipantId + "</td></tr>");
            writer.WriteLine("<tr><td>Test Participant Session Id</td><td>" + testParticipant.client.SessionId + "</td></tr>");
            writer.WriteLine("<tr><td>Test Participant is connected</td><td>" + testParticipant.IsConnected + "</td></tr>");
            writer.WriteLine("</table>");

            writer.WriteLine("<h2>Message Factory</h2>");
            writer.WriteLine(MessageFactory.Current.ToString());
            */

            BoxKickerDaemonParticipant testParticipant = TankManager.BoxKickerDaemonParticipant;

            writer.WriteLine("<h2>Test Participant is running:" + testParticipant.IsAlive + "</h2>");
            writer.WriteLine("<table>");
            writer.WriteLine("<tr><td>Test Participant Id</td><td>" + testParticipant.ParticipantId + "</td></tr>");
            writer.WriteLine("<tr><td>Test Participant Session Id</td><td>" + testParticipant.client.SessionId + "</td></tr>");
            writer.WriteLine("<tr><td>Test Participant is connected</td><td>" + testParticipant.IsConnected + "</td></tr>");
            writer.WriteLine("</table>");

            writer.WriteLine("<h2>Message Factory</h2>");
            writer.WriteLine(MessageFactory.Current.ToString());

        }

        protected void StartTestParticipantButton_Click(object sender, EventArgs e)
        {
            //TankManager.TestParticipant.Startup();
            TankManager.BoxKickerDaemonParticipant.Startup();
        }

        protected void StopTestParticipantButton_Click(object sender, EventArgs e)
        {
            //TankManager.TestParticipant.Shutdown();
            TankManager.BoxKickerDaemonParticipant.Shutdown();
        }

    }
}