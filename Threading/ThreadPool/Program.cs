using System;

namespace Threading.ThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
           if (args.Length == 0) 
            {
                Console.WriteLine("Manual thread creation:");
                Console.WriteLine("1. Background worker thread doesn't finish work.");
                Console.WriteLine("2. Foreground worker thread finishes after main thread (no join).");
                Console.WriteLine("3. Foreground worker thread finishes before main thread (join).");
                return;
            }
            switch(args[0])
            {
                case "1":
                    ThreadPoolThreadCreation.BackgroundThreadWithoutJoin();
                    break;
                case "2":
                    ThreadPoolThreadCreation.ForegroundThreadWithoutJoin();
                    break;
                case "3":
                    ThreadPoolThreadCreation.ForegroundThreadWithJoin();
                    break;
                
            }
        }
    }
}
