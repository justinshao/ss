using System;

namespace Common.Utilities.Helpers
{
    public class AsyncHelper
    {
        public static void BeginInvoke(Action callBack)
        {
            try
            {
                AsyncCallback asyn = new AsyncCallback((IAsyncResult result) =>
                {
                    callBack.EndInvoke(result);
                });
                callBack.BeginInvoke(asyn, null);
            }
            catch
            {
                callBack();
            }
        }

        public static void BeginInvoke<T>(Action<T> callBack, T t)
        {
            try
            {
                AsyncCallback asyn = new AsyncCallback((IAsyncResult result) =>
                {
                    callBack.EndInvoke(result);
                });
                callBack.BeginInvoke(t, asyn, null);
            }
            catch
            {
                callBack(t);
            }
        }

        public static void BeginInvoke<T1, T2>(Action<T1, T2> callBack, T1 t1, T2 t2)
        {
            try
            {
                AsyncCallback asyn = new AsyncCallback((IAsyncResult result) =>
                {
                    callBack.EndInvoke(result);
                });
                callBack.BeginInvoke(t1, t2, asyn, null);
            }
            catch
            {
                callBack(t1, t2);
            }
        }

        public static void BeginInvoke<T1, T2, T3>(Action<T1, T2, T3> callBack, T1 t1, T2 t2, T3 t3)
        {
            try
            {
                AsyncCallback asyn = new AsyncCallback((IAsyncResult result) =>
                {
                    callBack.EndInvoke(result);
                });
                callBack.BeginInvoke(t1, t2, t3, asyn, null);
            }
            catch
            {
                callBack(t1, t2, t3);
            }
        }

    }
}
