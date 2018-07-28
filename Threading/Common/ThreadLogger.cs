using System;
using System.Threading;

namespace Threading.Common
{
    public class ThreadLogger
    {
        public static void Log(string message)
        {
             Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId:00} " + 
                                $"({(Thread.CurrentThread.IsBackground ? "B" : "F")})] " + 
                                $"@ {DateTime.UtcNow:mm:ss.fff}: {message}");
        }
    }
}