using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


/// <summary>
/// 并发控制器，实现多线程访问同一资源时，只允许一个线程访问，其他线程等待该线程通知（需实例化为静态变量）
/// </summary>
public class ConcurrentControl
{
    private Dictionary<string, ManualResetEvent> dicKeys = new Dictionary<string, ManualResetEvent>();
    private object queryLock = new object();
    int waitTimeOut = 0;

    public ConcurrentControl(int timeOutMiniseconds)
    {
        waitTimeOut = timeOutMiniseconds;
    }

    /// <summary>
    /// 是否有其他线程正在查询该键，若有，则等待
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns></returns>
    public bool IsQuerying(string identifier)
    {
        ManualResetEvent manualEvent;
        lock (queryLock)
        {
            if (!dicKeys.ContainsKey(identifier))
            {
                dicKeys.Add(identifier, null);
                return false;
            }
            manualEvent = dicKeys[identifier];
            if (manualEvent == null)
            {
                manualEvent = new ManualResetEvent(false);
                dicKeys[identifier] = manualEvent;
            }
        }
        var notTimeOut = manualEvent.WaitOne(waitTimeOut, false);
        return true;
    }

    /// <summary>
    /// 通知其他线程已查询完毕
    /// </summary>
    /// <param name="identifier"></param>
    public void Notify(string identifier)
    {
        ManualResetEvent manualEvent;
        lock (queryLock)
        {
            if (dicKeys.TryGetValue(identifier, out manualEvent))
                dicKeys.Remove(identifier);
        }
        if (manualEvent != null)
            manualEvent.Set();
    }
}

