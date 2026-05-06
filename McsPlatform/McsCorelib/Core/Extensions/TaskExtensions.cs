namespace McsCoreLib.Core.Extensions
{
    public static class TaskExtensions
    {
        /// <summary>
        ///  Void Task 异常处理
        /// </summary>
        public static async void FireAndForget<Texception>(this Task task, Action<Texception> recovery, Func<Exception, bool> onError) where Texception : Exception
        {
            try
            {
                await task;
            }
            catch (Texception ex)
            {
                recovery(ex);
            }
            catch (Exception ex) when (onError(ex))
            {
                throw;
            }
        }

        /// <summary>
        /// 获取多个任务的执行结果，并按完成顺序返回
        /// </summary>

        public static Task<T>[] OrderByComplention<T>(this IEnumerable<Task<T>> tasks)
        {
            var taskList = tasks.ToList();
            var taskCount = taskList.Count;
            //容纳任务执行结果
            var compentionSources = new TaskCompletionSource<T>[taskCount];
            var result = new Task<T>[taskCount];

            for (int i = 0; i < taskCount; i++)
            {
                compentionSources[i] = new TaskCompletionSource<T>();
                result[i] = compentionSources[i].Task;
            }

            // 任务完成时，设置对应的 TaskCompletionSource 的结果
            int nextIndex = -1;
            void action(Task<T> completed)
            {
                var bucket = compentionSources[Interlocked.Increment(ref nextIndex)];
                if (completed.IsFaulted) bucket.TrySetException(completed.Exception);
                else bucket.TrySetResult(completed.Result);
            }

            foreach (var task in taskList)
            {
                task.ContinueWith(action, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
            }

            return result;
        }
    }
}