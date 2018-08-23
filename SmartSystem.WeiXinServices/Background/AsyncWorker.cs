using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Common.Services;
using Common.Entities;

namespace SmartSystem.WeiXinServices.Background
{
    /// <summary>
    /// 异步工作
    /// </summary>
    public class AsyncWorker
    {
        /// <summary>
        /// 同步访问锁对象
        /// </summary>
        public object SyncRoot { get; private set; }

        /// <summary>
        /// 是否正在工作
        /// </summary>
        public bool IsRunning { get; private set; }

        private Action _action;

        /// <summary>
        /// 获取当前正在执行的线程
        /// </summary>
        protected Thread CurrentThread { get; private set; }

        public AsyncWorker(Action action)
        {
            SyncRoot = new object();
            _action = action;
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            lock (SyncRoot)
            {
                if (IsRunning)
                {
                    return;
                }
                else
                {
                    //线程池入列
                    IsRunning = ThreadPool.QueueUserWorkItem(ThreadMethod, _action);
                }
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public virtual void Stop()
        {
            if (
                IsRunning &&
                CurrentThread != null &&
                CurrentThread.ThreadState == ThreadState.Running)
            {
                CurrentThread.Abort();
            }
        }

        private void ThreadMethod(object state)
        {
            CurrentThread = Thread.CurrentThread;

            if (_action != null)
            {
                try
                {
                    _action();
                }
                catch (Exception e)
                {
                    ExceptionsServices.AddExceptionToDbAndTxt("Background", "后台工作线程错", e, LogFrom.WeiXin);
                }
            }
            lock (SyncRoot)
            {
                IsRunning = false;
            }
        }
    }
}
