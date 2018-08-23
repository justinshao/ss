using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utilities
{
    public class GuidGenerator
    {
        /// <summary>
        /// 生成有序GUID 
        /// </summary>
        /// <returns></returns>
        public static Guid GetGuid()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();

            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;

            //使用天和毫秒作为种子生成GUID  
            //字节字符串     
            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = now.TimeOfDay;

            //转换成字节        
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            //反转bytes使其符合在数据库中默认排序    
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            //字节拷贝到GUID      
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new Guid(guidArray);
        }
        public static string GetGuidString() {
            return GetGuid().ToString();
        }
    }
}
