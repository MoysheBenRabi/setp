using System;
using System.Collections.Generic;
using System.Web;
using IOT.Encoding;
using IOT.Model;
using IOT.Service;

namespace IOT
{
    public class IotContext
    {
        private static object ThreadLock = new object();

        public static TestSuiteState TestSuiteState
        {
            get
            {
                lock (ThreadLock)
                {
                    TestSuiteState testState = (TestSuiteState)HttpContext.Current.Session["TestReport"];
                    if (testState == null)
                    {
                        testState = new TestSuiteState();
                        HttpContext.Current.Session["TestReport"] = testState;
                    }
                    return testState;
                }
            }            
        }

        public static bool HasServiceController
        {
            get
            {
                return HttpContext.Current.Session["IotServiceController"] != null &&
                    ((IotServiceController)HttpContext.Current.Session["IotServiceController"]).IsAlive;
            }
        }

        public static IotServiceController ServiceController
        {
            get
            {
                IotServiceController iotServiceController = (IotServiceController)HttpContext.Current.Session["IotServiceController"];
                if (iotServiceController!=null&&!iotServiceController.IsAlive)
                {
                    iotServiceController = null;
                }
                if (iotServiceController==null&&!IotServiceController.IsReserved)
                {
                    iotServiceController = IotServiceController.ReserveServiceController(TestSuiteState);
                }
                HttpContext.Current.Session["IotServiceController"] = iotServiceController;
                return iotServiceController;
            }
        }

    }
}
