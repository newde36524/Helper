using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Helper
{
    /*
     *  For example:
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("start");
                var data = new List<string> { };
                for (int i = 0; i < 10; i++)
                {
                    data.Add(i.ToString());
                }
                TaskSchedule<string>.Schedule(data, new List<Action<string>>
                {
                    x => {
                        Console.WriteLine($"A {x}");
                    },
                    x => {
                        Console.WriteLine($"B {x}");
                        Thread.Sleep(1000);
                    },
                    x => {
                        Console.WriteLine($"C {x}");
                    },
                });
                Console.WriteLine("end");
                Console.ReadLine();
            }
        }
     * **/

    /// <summary>
    /// 任务调度器 使用指定的方法列表均衡处理数据源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TaskSchedule<T> where T : class
    {
        class TaskEntry<Value> where Value : class
        {
            long canUse = 0;
            public Action<Value> Action { get; set; }
            public TaskEntry(Action<Value> action) => Action = action;
            public bool CanUse() => Interlocked.Read(ref canUse) == 0;
            public void Invoke(Value v) => Action(v);
            public void UnUse() => Interlocked.Exchange(ref canUse, 0);
            public void Use() => Interlocked.Exchange(ref canUse, 1);
        }

        public static void Schedule(IEnumerable<T> src, IEnumerable<Action<T>> actions)
        {
            var taskEntrys = actions.Select(x => new TaskEntry<T>(x)).ToList();
            var srcTemp = src.ToList();
            var taskList = new List<Task>();
            SemaphoreSlim semaphoreSlim = new SemaphoreSlim(0);
            for (int j = 0; j < srcTemp.Count(); j++)
            {
                var item = srcTemp[j];
                var IsUse = false;
                for (int i = 0; i < taskEntrys.Count(); i++)
                {
                    var taskEntry = taskEntrys[i];
                    if (taskEntry.CanUse())
                    {
                        taskEntry.Use();
                        IsUse = true;
                        taskList.Add(Task.Factory.StartNew(() =>
                        {
                            taskEntry.Invoke(item);
                            taskEntry.UnUse();
                            semaphoreSlim.Release(1);
                        }));
                        break;
                    }
                }
                if (!IsUse)
                {
                    j--;
                    semaphoreSlim.Wait();
                }
            }
            Task.WaitAll(taskList.ToArray());
        }
    }
}
