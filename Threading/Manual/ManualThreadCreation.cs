using System;
using System.Threading;
using Threading.Common;

namespace Threading.Manual
{
    public class ManualThreadCreation
    {
        public static void ForegroundThreadWithoutJoin()
        {
            ThreadLogger.Log("main start");
            var thread = new Thread(IntensiveOperationsService.OneSecondWork);
            thread.Start();
            ThreadLogger.Log("main end");
        }
        public static void ForegroundThreadWithJoin()
        {
            ThreadLogger.Log("main start");
            var thread = new Thread(IntensiveOperationsService.OneSecondWork);
            thread.Start();
            thread.Join();
            ThreadLogger.Log("main end");
        }

    }

   
}