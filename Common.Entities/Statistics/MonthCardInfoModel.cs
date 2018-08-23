using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Statistics
{
    /// <summary>
    /// 月卡信息
    /// </summary>
    public class MonthCardInfoModel
    {
        /// <summary>
        /// 车场名称
        /// </summary>
        public string PKName
        {
            get;set;
        }
        /// <summary>
        /// 车牌号码
        /// </summary>
        public string PlateNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 卡片类型名称
        /// </summary>
        public string CarTypeName
        {
            get;
            set;
        }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Mobile
        {
            get;
            set;
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get;
            set;
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string strStartTime
        {
            get;
            set;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get;
            set;
        }
        /// <summary>
        /// 有效结束时间
        /// </summary>
        public string strEndTime
        {
            get;
            set;
        }
        /// <summary>
        /// 家庭地址
        /// </summary>
        public string Addr
        {
            get;
            set;
        }
        /// <summary>
        /// 业主姓名
        /// </summary>
        public string EmployeeName
        {
            get;
            set;
        }
    }
}
