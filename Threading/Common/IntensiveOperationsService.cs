using System.Linq;
using System.Threading;

namespace Threading.Common
{
    public class IntensiveOperationsService
    {
        public static void OneSecondWork(object param = null)
        {
            ThreadLogger.Log("worker start");
            Thread.Sleep(1000);
            ThreadLogger.Log("worker end");
        }

        public static void CancellableLoop(CancellationToken token)
        {
            foreach (var num in Enumerable.Range(0, 10))
            {
                if (token.IsCancellationRequested)
                {
                    ThreadLogger.Log($"worker cancelling on iteration {num}");
                    break;
                }
                ThreadLogger.Log($"worker iteration {num}");
                Thread.Sleep(1000);
            }
        }
    }
}
