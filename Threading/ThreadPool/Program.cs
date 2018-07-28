using System;
using System.Threading;

namespace Threading.ThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Example: Queueing a method on thread pool");
            ThreadPoolThreadCreation.QueueUserWorkItem();
            Thread.Sleep(3000);
            Console.WriteLine("Example: Queueing a cancellable method on thread pool and cancelling it");
            ThreadPoolThreadCreation.QueueUserWorkItemAndCancelIt();
            Thread.Sleep(3000);
            Console.WriteLine("Example: Register() callback throwing exception, not using aggregation");
            ThreadPoolThreadCreation.CancellationTokenCallbackThrowingExceptionsWithoutAggregation();
            Thread.Sleep(3000);
            Console.WriteLine("Example: Register() callback throwing exception, using aggregation");
            ThreadPoolThreadCreation.CancellationTokenCallbackThrowingExceptionsWithAggregation();
            Console.ReadKey();
        }
    }
}
