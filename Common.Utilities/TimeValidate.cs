using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utilities
{
    public class TimeValidate
    {
        public static void Validate(DateTime? timeLower, DateTime? timeUpper, int maxDate, string errMsg)
        {
            if (timeLower.HasValue && timeUpper.HasValue && timeUpper.Value.Subtract(timeLower.Value).TotalDays > maxDate - 1) {
                throw new Exception(errMsg + "时间大于" + maxDate + "天");
            }
            if (timeLower.HasValue&&!timeUpper.HasValue) {
                throw new Exception(errMsg + "时间输入开始时间必须输入结束时间");
            }
            if (!timeLower.HasValue && timeUpper.HasValue)
            {
                throw new Exception(errMsg + "时间输入结束时间必须输入开始时间");
            }
        }
        public static void ValidateTotalNull(string msg,params DateTime?[] times){

            if (times.Any(t => t.HasValue)) return;
            throw new Exception(msg) ;
        }
    }
}
