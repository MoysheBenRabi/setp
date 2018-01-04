using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using IOT.Model;
using MXP.Cloud;
using MXP;
using MXP.Util;

namespace IOT.Service
{
    /// <summary>
    /// This class controls launching the cloud service which is available for one test at a time.
    /// </summary>
    public class IotServiceController
    {
        /// <summary>
        /// Time which service controller instance will be reserved for each test.
        /// </summary>
        private static TimeSpan IotServiceLifeSpan = new TimeSpan(0, 0, 60);
        /// <summary>
        /// Contains the currently reserved service controller.
        /// </summary>
        private static IotServiceController serviceController;
        /// <summary>
        /// Used to synchronize thread access to serviceController variable.
        /// </summary>
        private static object threadLock=new object();

        /// <summary>
        /// Returns true if service controller is reserved.
        /// </summary>
        /// <returns></returns>
        public static bool IsReserved
        {
            get
            {
                lock (threadLock)
                {
                    return serviceController != null;
                }
            }
        }
        /// <summary>
        /// Returns the time of reservation left.
        /// </summary>
        public static TimeSpan ReservedTimeLeft
        {
            get
            {
                lock (threadLock)
                {
                    if (serviceController != null)
                    {
                        return serviceController.startTime.Add(IotServiceLifeSpan).Subtract(DateTime.Now);
                    }
                    else
                    {
                        return new TimeSpan(0, 0, 0);
                    }
                }
            }
        }
        /// <summary>
        /// Returns service controller or null if it is already reserved.
        /// Service controller is automatically released when test time ends.
        /// </summary>
        /// <param name="testSuiteState">Test suite state of the session reserving the controller.</param>
        /// <returns></returns>
        public static IotServiceController ReserveServiceController(TestSuiteState testSuiteState)
        {
            lock (threadLock)
            {
                if (serviceController != null)
                {
                    // Service controller is already existing and reserved by someone else.
                    return null;
                }
                else
                {
                    serviceController = new IotServiceController(testSuiteState);
                    return serviceController;
                }
            }
        }
        
        /// <summary>
        /// Thread which controls the IOT service life time.
        /// </summary>
        private Thread lifetimeControlThread;
        /// <summary>
        /// Test asser which follows the service events and updates test states to test suite report.
        /// </summary>
        private ServiceTestAssessor testAssessor;
        /// <summary>
        /// Start time of this IOT service controller instance.
        /// </summary>
        private DateTime startTime;
        /// <summary>
        /// Test suite state where test states are updated to.
        /// </summary>
        private TestSuiteState testSuiteState;
        /// <summary>
        /// True if test is completed and controller can exit.
        /// </summary>
        private bool isCompleted;
        /// <summary>
        /// Whether this service controller instance is alive.
        /// </summary>
        /// <returns></returns>
        public bool IsAlive
        {
            get
            {
                return lifetimeControlThread.IsAlive;
            }
        }
        /// <summary>
        /// Time span this service controller has left to live.
        /// </summary>
        public TimeSpan LifeTimeLeft
        {
            get
            {
                return startTime.Add(IotServiceLifeSpan).Subtract(DateTime.Now);
            }
        }
        
        /// <summary>
        /// Default constructor which constructs and starts the life time thread.
        /// </summary>
        public IotServiceController(TestSuiteState testSuiteState)
        {
            this.startTime = DateTime.Now;
            this.testSuiteState = testSuiteState;
            this.testAssessor = new ServiceTestAssessor(testSuiteState);
            this.lifetimeControlThread = new Thread(new ThreadStart(Process));
            this.lifetimeControlThread.Start();
        }
        /// <summary>
        /// The main processing loop.
        /// </summary>
        public void Process()
        {
            LogUtil.EnableMemoryLog();
            try
            {
                string serviceProgram = "Inter Operability Testing Service";
                byte serviceProgramMajorVersion = 0;
                byte serviceProgramMinorVersion = 1;
                string serviceAddress = "0.0.0.0";
                int serviceHubPort = MxpConstants.DefaultHubPort+2;
                int serviceServerPort = MxpConstants.DefaultServerPort+2;
                string serviceAssetCacheUrl = "http://test.assetcache.one";

                Guid bubbleOneId = new Guid("539DFA16-5B52-4c9f-9A09-AD746520873E");
                string bubbleOneName = "IOT Bubble 1";
                float bubbleOneRange = 100;
                float bubbleOnePerceptionRange = 110;

                CloudService cloudService = new CloudService(serviceAssetCacheUrl, serviceAddress, serviceHubPort, serviceServerPort, serviceProgram, serviceProgramMajorVersion, serviceProgramMinorVersion);
 
                CloudBubble bubbleOne = new CloudBubble(bubbleOneId, bubbleOneName, bubbleOneRange, bubbleOnePerceptionRange);

                this.testAssessor.RegisterEventHandlers(bubbleOne);

                bubbleOne.ParticipantDisconnected += OnParticipantDisconnected;

                cloudService.AddBubble(bubbleOne);

                cloudService.Startup(true);

                while (DateTime.Now.Subtract(startTime) < IotServiceLifeSpan && !isCompleted)
                {
                    cloudService.Process();
                    testSuiteState.ReferenceServerLog = LogUtil.GetMemoryLog();
                    Thread.Sleep(10);
                }

                cloudService.Shutdown();

            }
            finally
            {
                lock (threadLock)
                {
                    serviceController = null;
                }
                testSuiteState.ReferenceServerLog = LogUtil.GetMemoryLog();
                LogUtil.DisableMemoryLog();
            }
        }

        public void OnParticipantDisconnected(Session session)
        {
            isCompleted = true;
        }
    }
}
