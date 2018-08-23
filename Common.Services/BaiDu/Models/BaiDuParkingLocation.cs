using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Services.BaiDu
{
    public class BaiDuParkingLocation
    {
        /// <summary>
        /// 停车场编号
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 街景编号
        /// </summary>
        public string street_id { get; set; }
        /// <summary>
        /// 百度编号
        /// </summary>
        public string uid { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string lat { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string lng { get; set; }
        /// <summary>
        /// 停车场名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 类型 0-自己的 1-百度的
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 距离
        /// </summary>
        public double distance { get; set; }
        /// <summary>
        /// 需要花费的时间
        /// </summary>
        public int time { get; set; }
        /// <summary>
        /// 车位数量
        /// </summary>
        public int quantity { get; set; }
    }
}
