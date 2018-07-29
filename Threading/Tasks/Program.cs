using System;
using System.Threading;

namespace Tasks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Example: Creating 2 simple tasks and waiting for results");
            TaskCreation.SimpleTaskCreation();
            Thread.Sleep(3000);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Example: WaitAny() and WaitAll()");
            TaskCreation.WaitAnyAndWaitAll();
            Thread.Sleep(3000);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Example: Task cancellation");
            TaskCreation.CancellingTheTask();
            Thread.Sleep(3000);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Example: Task continuations");
            TaskCreation.TaskContinuations();
            Thread.Sleep(3000);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Example: Task and child tasks");
            TaskCreation.TaskAndChildTasks();
            Thread.Sleep(3000);

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Example: Unobserved exceptions caught by event handler");
            TaskCreation.TaskThrowsExceptionWhichIsNotObserved();
            Thread.Sleep(3000);
            Console.ReadKey();
        }
    }
}
