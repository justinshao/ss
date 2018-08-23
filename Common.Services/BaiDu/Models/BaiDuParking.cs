using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Services.BaiDu
{
    public class BaiDuParking
    {
        public int status { get; set; }

        public string message { get; set; }

        public List<BaiDuCarParkResult> results { get; set; }

        public bool IsSuccess
        {
            get
            {
                return status == 0;
            }
        }
    }
    /// <summary>
    /// 停车场信息
    /// </summary>
    public class BaiDuCarParkResult
    {
        /// <summary>
        /// 停车场名称
        /// </summary>
        public string name { get; set; }
        public BaiDuCarParkLocation location { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        public string street_id { get; set; }
        public int detail { get; set; }
        public string uid { get; set; }
    }
    /// <summary>
    /// 位置信息
    /// </summary>
    public class BaiDuCarParkLocation
    {
        /// <summary>
        /// 纬度
        /// </summary>
        public string lat { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string lng { get; set; }
    }
}
