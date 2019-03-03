using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Helper
{
    /// <summary>
    /// 循环任务代理
    /// </summary>
    public class WhileTaskProxy
    {
        /// <summary>
        /// 创建一个WhileTaskProxy的实例
        /// </summary>
        public WhileTaskProxy() => complateTask.Start();

        /// <summary>
        /// 代表已结束的任务
        /// </summary>
        Task complateTask = new Task(() => { });

        /// <summary>
        /// 用于原子替换
        /// </summary>
        WhileTask whileTask;

        /// <summary>
        /// 是否暂停任务
        /// </summary>
        bool isPause = false;

        /// <summary>
        /// 启动下一个任务，这会取消上一个任务，利用延续任务防止同时执行两个任务
        /// </summary>
        /// <param name="action">具体需要执行的方法</param>
        /// <param name="milliseconds">执行间隔</param>
        public void Next(Action action, int milliseconds)
        {
            Interlocked.Exchange(ref whileTask, new WhileTask(action, milliseconds))?.Dispose();
            Interlocked.Exchange(ref complateTask, complateTask.ContinueWith(t => whileTask.Start(ref isPause)));
        }

        /// <summary>
        /// 暂停当前任务
        /// </summary>
        public void Pause() => isPause = true;

        /// <summary>
        /// 继续当前任务
        /// </summary>
        public void Reume() => isPause = false;

        /// <summary>
        /// 循环任务的包装类
        /// </summary>
        public class WhileTask : IDisposable
        {
            /// <summary>
            /// 创建循环任务的包装类的实例
            /// </summary>
            /// <param name="action"></param>
            /// <param name="milliseconds"></param>
            public WhileTask(Action action, int milliseconds)
            {
                Action = action;
                Milliseconds = milliseconds;
            }

            /// <summary>
            /// 被执行的任务
            /// </summary>
            public Action Action { get; }

            /// <summary>
            /// 执行间隔
            /// </summary>
            public int Milliseconds { get; }

            /// <summary>
            /// 用于标记是否被释放
            /// </summary>
            public bool IsDispose { get; private set; }

            /// <summary>
            /// 开始重复执行任务
            /// </summary>
            /// <param name="isPause">检测是否暂停</param>
            public void Start(ref bool isPause)
            {
                if (Action != null)
                {
                    while (!IsDispose)
                    {
                        while (isPause) {/*.*/}
                        Action.Invoke();
                        Thread.Sleep(Milliseconds);
                    }
                }
            }

            /// <summary>
            /// 释放对象
            /// </summary>
            public void Dispose() => IsDispose = true;
        }
    }
}
