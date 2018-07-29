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

        public static int CancellableSleepingSum(int id, int n, CancellationToken? token = null)
        {
            var sum = 0;
            for (int i = 1; i <= n; i++)
            {
                if(token.HasValue && token.Value.IsCancellationRequested)
                {
                    ThreadLogger.Log($"worker {id} cancelling on iteration {i}");
                    token?.ThrowIfCancellationRequested();
                }

                sum += i;
                ThreadLogger.Log($"worker {id} iteration {i}");
                Thread.Sleep(1000);
            }
            return sum;
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
