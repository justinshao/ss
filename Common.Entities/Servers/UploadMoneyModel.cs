using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Servers
{
    public class UploadMoneyModel
    {
        /// <summary>
        /// 车牌号码
        /// </summary>
        public string PlateNumber { set; get; }

        /// <summary>
        /// 进场时间
        /// </summary>
        public string EntranceTime { set; get; }

        /// <summary>
        /// 出场时间
        /// </summary>
        public string ExitTime { set; get; }

        /// <summary>
        /// 应缴金额
        /// </summary>
        public string Amount { set; get; }

        /// <summary>
        /// 车场描述
        /// </summary>
        public string PKName { set; get; }

        /// <summary>
        /// 车场ID
        /// </summary>
        public string PKID { set; get; }

        public string PKNO { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { set; get; }

        /// <summary>
        /// 进出记录ID
        /// </summary>
        public string IORecordID { set; get; }

        public string CarModelName { set; get; }
    }
}
