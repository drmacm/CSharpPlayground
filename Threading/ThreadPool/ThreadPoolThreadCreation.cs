using System;
using System.Linq;
using System.Threading;
using Threading.Common;

namespace Threading.ThreadPool
{
    public class ThreadPoolThreadCreation
    {
        public static void QueueUserWorkItem()
        {
            ThreadLogger.Log("main start");
            System.Threading.ThreadPool.QueueUserWorkItem(IntensiveOperationsService.OneSecondWork);
            ThreadLogger.Log("main end");
        }

        public static void QueueUserWorkItemAndCancelIt()
        {
            ThreadLogger.Log("main start");

            var cts = new CancellationTokenSource();
            var token = cts.Token;
            
            System.Threading.ThreadPool.QueueUserWorkItem(x => IntensiveOperationsService.CancellableLoop(token));

            Thread.Sleep(3000);
            cts.Cancel();
            ThreadLogger.Log("main end");
        }

        public static void CancellationTokenWithCallback()
        {
            ThreadLogger.Log("main start");

            var cts = new CancellationTokenSource();
            var token = cts.Token;
            token.Register(() => ThreadLogger.Log("token has been cancelled"));

            System.Threading.ThreadPool.QueueUserWorkItem(x => IntensiveOperationsService.CancellableLoop(token));

            Thread.Sleep(3000);
            cts.Cancel();
            ThreadLogger.Log("main end");
        }

        public static void LinkedCancellationTokens()
        {
            ThreadLogger.Log("main start");

            var cts1 = new CancellationTokenSource();
            cts1.Token.Register(() => ThreadLogger.Log("cts1 cancelled"));
            var cts2 = new CancellationTokenSource();
            cts2.Token.Register(() => ThreadLogger.Log("cts2 cancelled"));
            var cts = CancellationTokenSource.CreateLinkedTokenSource(cts1.Token, cts2.Token);
            cts.Token.Register(() => ThreadLogger.Log("linked cts cancelled"));

            System.Threading.ThreadPool.QueueUserWorkItem(x => IntensiveOperationsService.CancellableLoop(cts.Token));

            Thread.Sleep(3000);
            cts2.Cancel();
            ThreadLogger.Log("main end");
        }

        public static void CancellationTokenCallbackThrowingExceptionsWithoutAggregation()
        {
            ThreadLogger.Log("main start");

            var cts = new CancellationTokenSource();
            var token = cts.Token;
            token.Register(() => throw new Exception("foo"));
            token.Register(() => throw new Exception("bar"));

            System.Threading.ThreadPool.QueueUserWorkItem(x => IntensiveOperationsService.CancellableLoop(token));

            Thread.Sleep(3000);
            try
            {
                cts.Cancel(true);
            }
            catch (Exception ex)
            {
                ThreadLogger.Log($"main exception: {ex.GetType()} - {ex.Message}");
            }

            ThreadLogger.Log("main end");
        }

        public static void CancellationTokenCallbackThrowingExceptionsWithAggregation()
        {
            ThreadLogger.Log("main start");

            var cts = new CancellationTokenSource();
            var token = cts.Token;
            token.Register(() => throw new Exception("foo"));
            token.Register(() => throw new Exception("bar"));

            System.Threading.ThreadPool.QueueUserWorkItem(x => IntensiveOperationsService.CancellableLoop(token));

            Thread.Sleep(3000);
            try
            {
                cts.Cancel(false);
            }
            catch (AggregateException ex)
            {
                ThreadLogger.Log($"main exception: {ex.GetType()} - {ex.InnerExceptions.Select(e => e.Message).Aggregate((a, b) => a + ", " + b)}");
            }

            ThreadLogger.Log("main end");
        }
    }
}