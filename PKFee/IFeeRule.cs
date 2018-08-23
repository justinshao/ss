using Common.Entities.Parking;
using System;
using System.Collections.Generic;



namespace PKFee
{
    public abstract class IFeeRule
    {
        /// <summary>
        /// 入场时间
        /// </summary>
        public DateTime ParkingBeginTime { get; set; }
        public DateTime _parkingEndTime;
        /// <summary>
        /// 出场时间
        /// </summary>
        public DateTime ParkingEndTime
        {
            get
            {

                return _parkingEndTime;
            }
            set
            {
                DateTime ret = value;
                if (!DateTime.TryParse(value.ToString("yyyy-MM-dd HH:mm:ss"), out ret))
                {
                    ret = DateTime.MinValue;
                } 
                //去掉毫秒部分
                _parkingEndTime = ret;
            }

        }
        /// <summary>
        /// 收费规则
        /// </summary>
        public ParkFeeRule FeeRule { get; set; }
        /// <summary>
        /// 收费规则详情，白天/夜晚模式有两段
        /// </summary>
        public List<ParkFeeRuleDetail> listRuleDetail { get; set; }

        public string FeeText{set;get;}
        /// <summary>
        /// 第一次是否计算免费1计算，0不计算
        /// </summary>
        public int IsFirstFree = 1;
        public abstract decimal CalcFee();
         
    }
}
