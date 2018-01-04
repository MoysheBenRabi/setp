using System;
using System.Collections.Generic;

using System.Web;
using MXTank;
using MXP;

namespace MXTankWeb
{
    public class TankManager
    {
        private static Tank tank;
        public static Tank Tank
        {
            get
            {
                lock (typeof(TankManager))
                {
                    if (tank == null)
                    {
                        tank = new Tank("http://test.cloud.org", "tank1.test.org",MxpConstants.DefaultServerPort, MxpConstants.DefaultHubPort);
                        TankBubble bubble = new TankBubble(new Guid("c9e95415-f2dd-4f97-b1b8-2736a75ea764"), "Test Bubble 1", Guid.Empty,100,1000);
                        tank.AddBubble(bubble);
                    }
                    return tank;
                }
            }
        }

        private static TestParticipant testParticipant;
        public static TestParticipant TestParticipant
        {
            get
            {
                lock (typeof(TankManager))
                {
                    if (testParticipant == null)
                    {
                        testParticipant = new TestParticipant("127.0.0.1", MxpConstants.DefaultServerPort, new Guid("c9e95415-f2dd-4f97-b1b8-2736a75ea764"), "testlocation", "http://test.provider.org", "TestParticipantName", "TestParticipantPassphrase");
                    }
                    return testParticipant;
                }
            }
        }

        private static BoxKickerDaemonParticipant boxKickerDaemonParticipant;
        public static BoxKickerDaemonParticipant BoxKickerDaemonParticipant
        {
            get
            {
                lock (typeof(TankManager))
                {
                    if (boxKickerDaemonParticipant == null)
                    {
                        boxKickerDaemonParticipant = new BoxKickerDaemonParticipant("127.0.0.1", MxpConstants.DefaultServerPort, new Guid("c9e95415-f2dd-4f97-b1b8-2736a75ea764"), "testlocation", "http://test.provider.org", "TestParticipantName", "TestParticipantPassphrase");
                    }
                    return boxKickerDaemonParticipant;
                }
            }
        }


    }
}
