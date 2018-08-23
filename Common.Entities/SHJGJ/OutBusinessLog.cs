using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.SHJGJ
{
    public class OutBusinessLog
    {
        /// <summary>
        /// 批次号
        /// </summary>
        public string batchCode { set; get; }

        /// <summary>
        /// 业务流水号
        /// </summary>
        public int bizSn { set; get; }

        /// <summary>
        /// 停车点编号：
        /// </summary>
        public string parkingSpotId { set; get; }

        /// <summary>
        /// 平台编号：
        /// </summary>
        public string platformId { set; get; }

        /// <summary>
        /// 泊位编号：
        /// </summary>
        public string berthId { set; get; }

        /// <summary>
        /// 附加泊位：
        /// </summary>
        public string addBerth { set; get; }

        /// <summary>
        /// 业务类型：
        /// </summary>
        public int businessType { set; get; }

        /// <summary>
        /// 操作类型：
        /// </summary>
        public int actType { set; get; }

        /// <summary>
        /// 操作时间：
        /// </summary>
        public string actTime { set; get; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string carNumber { set; get; }

        /// <summary>
        /// 包月证号：
        /// </summary>
        public string monthlyCertNumber { set; get; }

        /// <summary>
        /// 车辆类型
        /// </summary>
        public int carType { set; get; }

        /// <summary>
        /// 总剩余车位
        /// </summary>
        public int totRemainNum { set; get; }

        /// <summary>
        /// 月租剩余车位：
        /// </summary>
        public int monthlyRemainNum { set; get; }

        /// <summary>
        /// 访客剩余车位：
        /// </summary>
        public int guestRemainNum { set; get; }

        /// <summary>
        /// 停车时长：
        /// </summary>
        public int parkingTimeLength { set; get; }

        /// <summary>
        /// 收费金额：
        /// </summary>
        public int payMoney { set; get; }

        /// <summary>
        /// 支付类型：
        /// </summary>
        public int paymentType { set; get; }

        /// <summary>
        /// 停车凭证类型：
        /// </summary>
        public int voucherType { set; get; }

        /// <summary>
        /// 停车凭证号
        /// </summary>
        public string voucherNo { set; get; }
    }
}
