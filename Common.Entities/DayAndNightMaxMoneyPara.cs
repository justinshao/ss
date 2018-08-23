using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities
{
    public class DayAndNightMaxMoneyPara
    {
        #region 白天夜间最高收费
        /// <summary>
        ///  是否开启白天夜间最高收费
        /// </summary>
        public bool UseDayAndNightMaxMoney = false;
        public TimeSpan DayStartTime;
        public TimeSpan DayEndTime;
        /// <summary>
        ///  
        /// </summary>
        public int DayMaxMoney = 5;
        public int NightMaxMoney = 10;
        #endregion 
    }
}
