using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Helper
{
    /// <summary>
    /// Task的帮助类库
    /// </summary>
    public class TaskHelper
    {
        /// <summary>
        /// 启动指定数量的线程完成所有任务
        /// </summary>
        /// <param name="actions">操作列表</param>
        /// <param name="maxTaskCount">最大线程数</param>
        /// <param name="cancellationToken">任务取消标记</param>
        public static void RunParallel(List<Action> actions, int maxTaskCount, CancellationToken? cancellationToken = null)
        {
            RunParallel<Action>(actions, x => x(), maxTaskCount, cancellationToken);
            //CancellationToken cancelToken = cancellationToken ?? CancellationToken.None;
            //List<Task> tasks = new List<Task>();
            //foreach (var item in actions)
            //{
            //    if (cancelToken.IsCancellationRequested) break;
            //    if (tasks.Count > maxTaskCount)
            //    {
            //        int index = Task.WaitAny(tasks.ToArray(), cancelToken);
            //        tasks[index] = Task.Run(item, cancelToken);
            //        continue;
            //    }
            //    tasks.Add(Task.Run(item, cancelToken));
            //}
            //Task.WaitAll(tasks.ToArray());
        }

        /// <summary>
        /// 启动指定数量的线程完成所有任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">数据源</param>
        /// <param name="action">要对数据源执行的操作</param>
        /// <param name="maxTaskCount">最大线程数</param>
        /// <param name="cancellationToken">任务取消标记</param>
        public static void RunParallel<T>(List<T> source, Action<T> action, int maxTaskCount, CancellationToken? cancellationToken = null)
        {
            CancellationToken cancelToken = cancellationToken ?? CancellationToken.None;
            List<Task> tasks = new List<Task>();
            foreach (var item in source)
            {
                if (cancelToken.IsCancellationRequested) break;
                if (tasks.Count > maxTaskCount)
                {
                    int index = Task.WaitAny(tasks.ToArray(), cancelToken);
                    tasks[index] = Task.Run(() => action(item), cancelToken);
                    continue;
                }
                tasks.Add(Task.Run(() => action(item), cancelToken));
            }
            Task.WaitAll(tasks.ToArray());
        }
    }
}
