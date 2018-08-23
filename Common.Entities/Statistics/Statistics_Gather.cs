using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Statistics
{
    /// <summary>
    /// 日统计报表
    /// </summary>
    public class Statistics_Gather:Statistics_Base
    {
        /// <summary>
        /// 自增编号
        /// </summary>
        public int ID
        {
            get;
            set;
        }
        /// <summary>
        /// 统计编号
        /// </summary>
        public string StatisticsGatherID
        {
            get;
            set;
        }
        /// <summary>
        /// 车场编号
        /// </summary>
        public string ParkingID
        {
            get;
            set;
        }
        /// <summary>
        /// 车场名称
        /// </summary>
        public string ParkingName
        {
            get;
            set;
        }
        /// <summary>
        /// 汇总时间. 以小时为单位进行汇总
        /// </summary>
        public DateTime GatherTime
        {
            get;
            set;
        }
        public string GatherTimeToString
        {
            get
            {
                return GatherTime.ToyyyyMMddHHmmss();
            }
        }
        public string KeyName
        {
            get;
            set;
        }
    }
}
