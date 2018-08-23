using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.SHJGJ
{
    public class DeviceSignInModel
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public int seqno { set; get; }

        /// <summary>
        /// 业务编号：
        /// </summary>
        public string code { set; get; }

        /// <summary>
        ///停车点编号
        /// </summary>
        public string parkingSpotId { set; get; }

        /// <summary>
        /// 平台编号：
        /// </summary>
        public string platformId { set; get; }

        /// <summary>
        /// 工号：
        /// </summary>
        public string uid { set; get; }

        /// <summary>
        /// 密码：
        /// </summary>
        public string pwd { set; get; }

        /// <summary>
        /// 经度：
        /// </summary>
        public string longi { set; get; }

        /// <summary>
        /// 纬度：
        /// </summary>
        public string lati { set; get; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string batchCode { set; get; }

        /// <summary>
        /// 停车点名称
        /// </summary>
        public string name { set; get; }

        /// <summary>
        /// 停车点地址
        /// </summary>
        public string address { set; get; }

        /// <summary>
        /// 服务时段：
        /// </summary>
        public string opentime { set; get; }

        /// <summary>
        /// 收费标准：
        /// </summary>
        public string price { set; get; }

        /// <summary>
        /// 通用请求字段
        /// </summary>
        public CommRequest commRequest { set; get; }
    }
}
