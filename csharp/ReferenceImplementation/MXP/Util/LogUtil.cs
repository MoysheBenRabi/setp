using System;
using System.Collections.Generic;
using System.Text;
using log4net.Repository.Hierarchy;
using log4net;
using System.Reflection;

namespace MXP.Util
{
    public class LogUtil
    {
        private static readonly ILog logger = LogManager.GetLogger("MXP");
        public static bool LogToTrace = true;
        public static bool LogToLog4Net = true;
        public static bool LogToConsole = false;
        public static bool LogDebug = true;

        private static object MemoryLogLock = new object();
        private static StringBuilder MemoryLog = null;

        public static void EnableMemoryLog()
        {
            lock (MemoryLogLock)
            {
                MemoryLog = new StringBuilder();
            }
        }

        public static void DisableMemoryLog()
        {
            lock (MemoryLogLock)
            {
                MemoryLog = null;
            }
        }

        public static string GetMemoryLog()
        {
            lock (MemoryLogLock)
            {
                if (MemoryLog != null)
                {
                    return MemoryLog.ToString();
                }
                else
                {
                    return null;
                }
            }
        }

        public static void Debug(string message)
        {
            if (LogDebug)
            {
                if (LogToTrace)
                    System.Diagnostics.Debug.WriteLine(message);
                if (LogToLog4Net)
                    logger.Debug(message);
                if (LogToConsole)
                    Console.WriteLine("debug: " + message);
                lock (MemoryLogLock)
                {
                    if (MemoryLog != null)
                    {
                        MemoryLog.Append("debug: " + message + "\n");
                    }
                }
            }
        }

        public static void Info(string message)
        {
            if (LogToTrace)
                System.Diagnostics.Trace.TraceInformation(message);
            if (LogToLog4Net)
                logger.Info(message);
            if (LogToConsole)
                Console.WriteLine("info: " + message);
            lock (MemoryLogLock)
            {
                if (MemoryLog != null)
                {
                    MemoryLog.Append("info: " + message + "\n");
                }
            }
        }

        public static void Warn(string message)
        {
            if (LogToTrace)
                System.Diagnostics.Trace.TraceWarning("Warning: " + message);
            if (LogToLog4Net)
                logger.Warn(message);
            if (LogToConsole)
                Console.WriteLine("warn: " + message);
            lock (MemoryLogLock)
            {
                if (MemoryLog != null)
                {
                    MemoryLog.Append("warn: " + message + "\n");
                }
            }
        }

        public static void Error(string message)
        {
            if (LogToTrace)
                System.Diagnostics.Trace.TraceError(message);
            if (LogToLog4Net)
                logger.Error(message);
            if (LogToConsole)
                Console.Error.WriteLine("error: " + message);
            lock (MemoryLogLock)
            {
                if (MemoryLog != null)
                {
                    MemoryLog.Append("error: " + message + "\n");
                }
            }
        }
    }
}
