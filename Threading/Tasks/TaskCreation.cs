using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Threading.Common;

namespace Tasks
{
    public class TaskCreation
    {
        public static void SimpleTaskCreation()
        {
            var task1 = Task.Run(() => IntensiveOperationsService.CancellableSleepingSum(1, 2));
            var task2 = new Task<int>(n => IntensiveOperationsService.CancellableSleepingSum(2, (int)n), 2);
            task2.Start();

            ThreadLogger.Log($"Task 1: {task1.Result}");
            ThreadLogger.Log($"Task 2: {task2.Result}");
        }

        public static void WaitAnyAndWaitAll()
        {
            var task1 = Task.Run(() => IntensiveOperationsService.CancellableSleepingSum(1, 1));
            var task2 = Task.Run(() => IntensiveOperationsService.CancellableSleepingSum(2, 2));
            var task3 = Task.Run(() => IntensiveOperationsService.CancellableSleepingSum(3, 3));

            var index = Task.WaitAny(task1, task2, task3);
            ThreadLogger.Log($"Task {index + 1} finished first");

            Task.WaitAll(task1, task2, task3);
            ThreadLogger.Log("All tasks finished");
        }

        public static void CancellingTheTask()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var task1 = Task.Run(() => IntensiveOperationsService.CancellableSleepingSum(1, 10, token), token);

            Thread.Sleep(2000);
            ThreadLogger.Log("Cancelling the cts");
            cts.Cancel();
        }

        public static void TaskContinuations()
        {
            var task1 = Task.Run(() => IntensiveOperationsService.CancellableSleepingSum(1, 3));
            var task2 = task1.ContinueWith(t => ThreadLogger.Log($"The sum is {t.Result}"));
            var task3 = task2.ContinueWith(t => ThreadLogger.Log("Just a simple message from third task"));

            ThreadLogger.Log("Waiting for simple message from third task");
            //Needed to avoid mixing with other examples
            task3.Wait();
        }

        public static void TaskThrowsExceptionWhichIsNotObserved()
        {
            //Adding an event handler for unobserved exceptions
            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                if (sender is Task task && task.Exception is AggregateException agg)
                {
                    var messages = agg.InnerExceptions.Select(ie => ie.Message).Aggregate((a, b) => a + ", " + b);
                    ThreadLogger.Log($"Within event handler - {messages}");
                }
            };

            //scheduling a task and not calling .Wait(), or accessing .Result or .Exception properties
            Task.Run(() => throw new Exception("Something's wrong!"));

            //ensure that the task will be garbage collected
            Thread.Sleep(1000);
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
