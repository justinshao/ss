using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Common.Utilities.Helpers
{
    public class SystemHelper
    {

        public static void SetLocalTime(DateTime dt)
        {
            SystemTime MySystemTime = new SystemTime();
            SetSystemDateTime.GetLocalTime(MySystemTime);
            MySystemTime.vYear = (ushort)dt.Year;

            MySystemTime.vMonth = (ushort)dt.Month;

            MySystemTime.vDay = (ushort)dt.Day;

            MySystemTime.vHour = (ushort)dt.Hour;

            MySystemTime.vMinute = (ushort)dt.Minute;

            MySystemTime.vSecond = (ushort)dt.Second;

            SetSystemDateTime.SetLocalTime(MySystemTime);
        }
    }

    public class SetSystemDateTime
    {

        [DllImportAttribute("Kernel32.dll")]

        public static extern void GetLocalTime(SystemTime st);

        [DllImportAttribute("Kernel32.dll")]

        public static extern void SetLocalTime(SystemTime st);

    }
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class SystemTime 
    { 
        public ushort vYear;

        public ushort vMonth;

        public ushort vDayOfWeek;

        public ushort vDay;

        public ushort vHour;

        public ushort vMinute;

        public ushort vSecond;

    }
}
