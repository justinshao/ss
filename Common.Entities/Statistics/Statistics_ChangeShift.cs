using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Statistics
{
    /// <summary>
    /// 当班统计模型
    /// </summary>
    public class Statistics_ChangeShift : Statistics_Base
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
        /// 岗亭编号
        /// </summary>
        public string BoxID
        {
            get;
            set;
        }
        /// <summary>
        /// 岗亭名称
        /// </summary>
        public string BoxName
        {
            get;
            set;
        }
        /// <summary>
        /// 班次编号
        /// </summary>
        public string ChangeShiftID
        {
            get;
            set;
        }
        /// <summary>
        /// 当班人
        /// </summary>
        public string AdminID
        {
            get;
            set;
        }
        /// <summary>
        /// 当班人名称
        /// </summary>
        public string AdminName
        {
            get;
            set;
        }
        /// <summary>
        /// 上班时间
        /// </summary>
        public DateTime StartWorkTime
        {
            get;
            set;
        }
        public string StartWorkTimeToString
        {
            get
            {
                return StartWorkTime.ToyyyyMMddHHmmss();
            }
        }
        /// <summary>
        /// 下班时间
        /// </summary>
        public DateTime EndWorkTime
        {
            get;
            set;
        }
        public string EndWorkTimeToString
        {
            get
            {
                return EndWorkTime.ToyyyyMMddHHmmss();
            }
        }
    }
}
