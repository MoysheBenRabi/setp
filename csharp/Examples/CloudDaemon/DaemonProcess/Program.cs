using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MXP.Util;
using System.Diagnostics;

namespace DaemonProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2 && args[1].Equals("--db-log"))
            {
                log4net.Config.XmlConfigurator.Configure();
            }
            else
            {
                log4net.Config.BasicConfigurator.Configure();
            }


            DaemonProcess daemonProcess = new DaemonProcess
            {
                LocalProcessId = new Guid(args[0])
            };

            try
            {
                daemonProcess.Startup();
            }
            catch (Exception e)
            {
                LogUtil.Error("Unexpected error in startup phase: " + e.ToString());
                return;
            }

            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e) { 
                daemonProcess.RequestShutdown();
                while (daemonProcess.IsRunning)
                {
                    Thread.Sleep(10);
                }
                System.Environment.Exit(0);
            };

            try
            {
                while (daemonProcess.KeepRunning)
                {
                    DateTime lastTime = DateTime.Now;
                    daemonProcess.Process();

                    while (DateTime.Now.Subtract(lastTime).TotalMilliseconds < 50)
                    {
                        Thread.Sleep(10);
                    }
                }
            }
            catch (Exception e)
            {
                LogUtil.Error("Unexpected error in process cycle: " + e.ToString());
            }
            finally
            {
                try
                {
                    daemonProcess.Shutdown();
                }
                catch (Exception e)
                {
                    LogUtil.Error("Unexpected error in shutdown phase: " + e.ToString());
                }
            }

            System.Environment.Exit(0);
        }
    }
}
