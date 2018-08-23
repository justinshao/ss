using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Statistics
{
    /// <summary>
    /// 停车时长统计模型
    /// </summary>
    public class Statistics_GatherLongTime
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
        /// 统计时间
        /// </summary>
        public DateTime GatherTime
        {
            get;
            set;
        }
        /// <summary>
        /// 停车时长
        /// </summary>
        public int LTime
        {
            get;
            set;
        }
        /// <summary>
        /// 次数
        /// </summary>
        public int Times
        {
            get;
            set;
        }
        /// <summary>
        /// 是否更新
        /// </summary>
        public int HaveUpdate
        {
            get;
            set;
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime LastUpdateTime
        {
            get;
            set;
        }

        public string RecordID { set; get; }
    }
}
