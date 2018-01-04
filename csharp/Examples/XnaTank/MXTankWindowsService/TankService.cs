using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;

using System.ServiceProcess;
using System.Text;
using MXTank;
using MXP;

namespace MXTankWindowsService
{
    public partial class TankService : ServiceBase
    {
        private Tank tank;
        //private TestParticipant testParticipant;
        private BoxKickerDaemonParticipant testParticipant;

        public TankService()
        {
            InitializeComponent();

        }

        protected override void OnStart(string[] args)
        {
            tank = new Tank("http://test.cloud.org", "tank1.test.org", MxpConstants.DefaultServerPort, MxpConstants.DefaultHubPort);
            TankBubble bubble = new TankBubble(new Guid("c9e95415-f2dd-4f97-b1b8-2736a75ea764"), "Test Bubble 1", Guid.Empty, 100, 1000);
            tank.AddBubble(bubble);

            testParticipant = new BoxKickerDaemonParticipant("127.0.0.1", MxpConstants.DefaultServerPort, new Guid("c9e95415-f2dd-4f97-b1b8-2736a75ea764"), "testlocation", "http://test.provider.org", "TestParticipantName", "TestParticipantPassphrase");

            tank.Startup();
            testParticipant.Startup();
        }

        protected override void OnStop()
        {
            tank.Shutdown();
            testParticipant.Shutdown();
        }
    }
}
