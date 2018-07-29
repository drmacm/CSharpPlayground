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
            //Create and schedule a task via static method
            var task1 = Task.Run(() => IntensiveOperationsService.CancellableSleepingSum(1, 2));

            //Create a new task instance and schedule it via .Start() method
            var task2 = new Task<int>(n => IntensiveOperationsService.CancellableSleepingSum(2, (int)n), 2);
            task2.Start();

            //Create a task factory and use .StartNew() method to create and schedule a task
            TaskFactory tf = new TaskFactory(/*Various options for all tasks created via this factory*/);
            var task3 = tf.StartNew(() => IntensiveOperationsService.CancellableSleepingSum(3, 2));


            ThreadLogger.Log($"Task 1: {task1.Result}");
            ThreadLogger.Log($"Task 2: {task2.Result}");
            ThreadLogger.Log($"Task 3: {task3.Result}");
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

        public static void TaskAndChildTasks()
        {
            var parent = new Task<int[]>(() =>
            {
                var results = new int[3];

                new Task(() => results[0] = IntensiveOperationsService.CancellableSleepingSum(1, 1), TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[1] = IntensiveOperationsService.CancellableSleepingSum(2, 2), TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[2] = IntensiveOperationsService.CancellableSleepingSum(3, 3), TaskCreationOptions.AttachedToParent).Start();

                return results;
            });

            parent.ContinueWith(x => ThreadLogger.Log($"Sum of all results: {x.Result.Sum()}"));

            ThreadLogger.Log("Starting parrent task");
            parent.Start();
            //Needed to avoid mixing with other examples
            parent.Wait();

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
