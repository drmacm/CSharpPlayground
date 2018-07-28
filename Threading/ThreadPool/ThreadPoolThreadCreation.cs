using System;
using System.Threading;

namespace Threading.ThreadPool
{
    public class ThreadPoolThreadCreation
    {
        public static void BackgroundThreadWithoutJoin()
        {
            ThreadLogger.Log("main start");
            ThreadPool.QueueUserWorkItem(FancyService.FancyWork);
            ThreadLogger.Log("main end");
        }
    }

    public class FancyService
    {
        public static void FancyWork(object param = null) 
        {
            ThreadLogger.Log("worker start");
            Thread.Sleep(1000);
            ThreadLogger.Log("worker end");
        }
    }
}