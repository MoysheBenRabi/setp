using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Text;
using log4net;
using log4net.Config;
using Nini.Config;
using MXP.Cloud;
using MXP;
using System.Diagnostics;

namespace CloudTank
{

    // Main program class which is first executed when CloudTank.exe is started.
    public class Program 
    {

        static void Main(string[] args)
        {
            log4net.Config.BasicConfigurator.Configure();
                       
            CloudTank cloudTank=new CloudTank();

            cloudTank.Startup();

            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e) { cloudTank.Shutdown(); };

            try
            {
                while (cloudTank.IsStarted)
                {
                    DateTime lastTime = DateTime.Now;
                    cloudTank.Process();

                    while (DateTime.Now.Subtract(lastTime).TotalMilliseconds < 100)
                    {
                        Thread.Sleep(1);
                    }
                }
            }
            finally
            {
                cloudTank.Shutdown();
            }
        }

    }
}
