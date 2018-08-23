using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

public class MyWatch : IDisposable
{
    private string name;

    private Stopwatch watch;

    public MyWatch(string name)
    {
        this.name = name;
        watch = new Stopwatch();
        watch.Start();
    }



    #region IDisposable 成员

    public void Dispose()
    {
        watch.Stop();
        Console.WriteLine(name + "花费了" + watch.ElapsedMilliseconds.ToString() + "ms");
    }

    #endregion
}
