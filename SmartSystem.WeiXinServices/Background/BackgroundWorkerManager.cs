using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace SmartSystem.WeiXinServices.Background
{
    /// <summary>
    /// 后台线程管理器
    /// </summary>
    public static class BackgroundWorkerManager
    {
        /// <summary>
        /// 环境的路径
        /// </summary>
        public static string EnvironmentPath { get; set; }

        static private Timer _timer;

        static private List<Schedule> _schedules;

        static BackgroundWorkerManager()
        {
            _schedules = new List<Schedule>();
            _timer = new Timer(1000 * 1) { AutoReset = false };
            _timer.Elapsed += timer_Elapsed;

            //处理同步支付结果失败的订单
            RegisterSchedule(new Schedule
            {
                Worker = new AsyncWorker(SyncPayResultFailProcess.Instance.Process),
                TimeInterval = 1000 * 2,
                LastAction = DateTime.Now
            });
            //退款处理
            RegisterSchedule(new Schedule
            {
                Worker = new AsyncWorker(OrderRefundProcess.Instance.Process),
                TimeInterval = 1000 * 5,
                LastAction = DateTime.Now
            });
        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                lock (_schedules)
                {
                    foreach (Schedule schedule in _schedules)
                    {
                        if ((e.SignalTime - schedule.LastAction).TotalMilliseconds >= schedule.TimeInterval)
                        {
                            if (!schedule.Worker.IsRunning)
                            {
                                schedule.Worker.Start();
                                schedule.LastAction = DateTime.Now;
                            }
                        }
                    }
                }
            }
            finally
            {
                _timer.Start();
            }
        }

        /// <summary>
        /// 注册计划任务
        /// </summary>
        public static void RegisterSchedule(Schedule schedule)
        {
            if (schedule == null)
            {
                return;
            }
            lock (_schedules)
            {
                if (
                    !_schedules.Contains(schedule))
                {
                    schedule.LastAction = DateTime.Now;
                    _schedules.Add(schedule);
                }
            }
        }

        public static void Start()
        {
            _timer.Start();
        }

        public static void Stop()
        {
            _timer.Stop();
            lock (_schedules)
            {
                foreach (var s in _schedules)
                {
                    if (s.Worker.IsRunning)
                    {
                        s.Worker.Stop();
                    }
                }
            }
        }
    }

    /// <summary>
    /// 计划任务
    /// </summary>
    public class Schedule
    {
        /// <summary>
        /// 任务执行者
        /// </summary>
        public AsyncWorker Worker { get; set; }

        /// <summary>
        /// 任务间隔时间
        /// <para>单位:ms</para>
        /// </summary>
        public double TimeInterval { get; set; }

        /// <summary>
        /// 上次任务执行时间
        /// </summary>
        public DateTime LastAction { get; set; }

        public Schedule()
        {
            LastAction = DateTime.Now;
        }
    }
}
