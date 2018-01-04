using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using DaemonLogic;
using System.Threading;
using System.IO;

namespace ProcessGuardService
{
    public partial class ProcessGuard : ServiceBase
    {
        private Timer timer;

        public ProcessGuard()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Trace.TraceInformation("Cloud daemon process guard started.");
            timer = new Timer(timer_Tick, null,10000, 15000);
        }

        protected override void OnStop()
        {
            Trace.TraceInformation("Cloud daemon process guard stopped.");
            timer.Dispose();
        }

        private void timer_Tick(object state)
        {
            try
            {

                DaemonEntities entityContext = new DaemonEntities();
                List<LocalProcess> localProcessEntities = (
                    from lp in entityContext.LocalProcess select lp).ToList<LocalProcess>();

                foreach (LocalProcess localProcessEntity in localProcessEntities)
                {
                    LocalProcessState localProcessState = QueryUtil.First<LocalProcessState>(
                        from lps in entityContext.LocalProcessState where lps.LocalProcess.LocalProcessId == localProcessEntity.LocalProcessId select lps);

                    if (localProcessEntity.Enabled)
                    {
                        if (localProcessState == null)
                        {
                            Process process = Process.Start(Directory.GetParent(this.GetType().Assembly.Location).FullName + "\\DaemonProcess.exe", localProcessEntity.LocalProcessId.ToString() + " --db-log");
                        }
                    }
                    if (localProcessState != null)
                    {
                        if (DateTime.Now - localProcessState.Modified > new TimeSpan(0, 0, 60))
                        {
                            entityContext.DeleteObject(localProcessState);
                            entityContext.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Trace.TraceError("Error in process guard loop: " + e.ToString() + " : " + e.StackTrace);
                Thread.Sleep(1000);
            }


        }

        private void CloudDaemonProcessGuard_Exited(object sender, EventArgs e)
        {

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {

        }
    }
}
