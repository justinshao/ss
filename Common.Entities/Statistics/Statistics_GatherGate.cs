using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Statistics
{
    /// <summary>
    /// 通道时统计
    /// </summary>
    public class Statistics_GatherGate:Statistics_Base
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
        /// 通道编号
        /// </summary>
        public string GateID
        {
            get;
            set;
        }
        /// <summary>
        /// 通道名称
        /// </summary>
        public string GateName
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
    }
}
