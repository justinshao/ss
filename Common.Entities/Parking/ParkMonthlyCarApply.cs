using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Parking
{
    public class ParkMonthlyCarApply
    {
        public int ID { get; set; }

        public string RecordID { get; set; }
        /// <summary>
        /// 申请人微信账号
        /// </summary>
        public string AccountID { get; set; }
        /// <summary>
        /// 所申请的车场
        /// </summary>
        public string PKID { get; set; }
        /// <summary>
        /// 申请车类
        /// </summary>
        public string CarTypeID { get; set; }
        /// <summary>
        /// 申请车型
        /// </summary>
        public string CarModelID { get; set; }
        /// <summary>
        /// 申请人姓名
        /// </summary>
        public string ApplyName { get; set; }
        /// <summary>
        /// 申请人手机号
        /// </summary>
        public string ApplyMoblie { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNo { get; set; }
        /// <summary>
        /// 车位号
        /// </summary>
        public string PKLot { get; set; }
        /// <summary>
        /// 家庭地址
        /// </summary>
        public string FamilyAddress { get; set; }
        /// <summary>
        /// 申请备注
        /// </summary>
        public string ApplyRemark { get; set; }
        /// <summary>
        /// 申请状态
        /// </summary>
        public MonthlyCarApplyStatus ApplyStatus { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
        public string AuditRemark { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyDateTime { get; set; }
        /// <summary>
        /// 车场名称
        /// </summary>
        public string PKName { get; set; }
        /// <summary>
        /// 车类名称
        /// </summary>
        public string CarTypeName { get; set; }
        /// <summary>
        /// 车型名称
        /// </summary>
        public string CarModelName { get; set; }
    }
}
