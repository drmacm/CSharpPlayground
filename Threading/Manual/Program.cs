using System;
using System.Threading;

namespace Threading.Manual
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Example: Foreground worker thread finishes before main thread (join).");
            ManualThreadCreation.ForegroundThreadWithJoin();
            Thread.Sleep(3000);
            Console.WriteLine("Example: Foreground worker thread finishes after main thread (no join).");
            ManualThreadCreation.ForegroundThreadWithoutJoin();
            Console.ReadKey();
        }
    }
}
