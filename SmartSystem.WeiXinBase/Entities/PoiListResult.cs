using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSystem.WeiXinBase
{
    public class PoiListResult
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public int total_count { get; set; }

        public List<PoiInfo> business_list { get; set; }
    }

    public class PoiInfo
    {
        public BaseInfo base_info { get; set; }

        public class BaseInfo
        {
            public class PhotoUrl
            {
                public string photo_url { get; set; }

            }
            public string sid { get; set; }
            public string business_name { get; set; }
            public string poi_id { get; set; }
            /// <summary>
            ///省会
            /// </summary>
            public string province { get; set; }
            /// <summary>
            ///地市
            /// </summary>
            public string city { get; set; }
            /// <summary>
            ///地址
            /// </summary>
            public string address { get; set; }
            /// <summary>
            /// 电话
            /// </summary>
            public string telephone { get; set; }
            /// <summary>
            /// 类别
            /// </summary>
            public string[] categories { get; set; }
            /// <summary>
            /// 偏移类型
            /// </summary>
            public string offset_type { get; set; }
            /// <summary>
            /// 经度
            /// </summary>
            public float? longitude { get; set; }
            /// <summary>
            /// 纬度
            /// </summary>
            public float? latitude { get; set; }
            /// <summary>
            /// 图片url列表
            /// </summary>
            public List<PhotoUrl> photo_list { get; set; }
            /// <summary>
            /// 商户简介
            /// </summary>
            public string introduction { get; set; }
            /// <summary>
            /// 推荐品
            /// </summary>
            public string recommend { get; set; }
            /// <summary>
            /// 特色服务
            /// </summary>
            public string special { get; set; }
            /// <summary>
            /// 营业时间，24 小时制表示，用“-”连接，如 8:00-20:00
            /// </summary>
            public string open_time { get; set; }
            /// <summary>
            /// 人均价格，大于0 的整数
            /// </summary>
            public int? avg_price { get; set; }
            /// <summary>
            /// 门店是否可用状态。1 表示系统错误、2 表示审核中、3 审核通过、4 审核驳回。当该字段为1、2、4 状态时，poi_id 为空
            /// </summary>
            public int? available_state { get; set; }
            /// <summary>
            /// 地址
            /// </summary>
            public string district { get; set; }
            /// <summary>
            /// 扩展字段是否正在更新中。1 表示扩展字段正在更新中，尚未生效，不允许再次更新； 0 表示扩展字段没有在更新中或更新已生效，可以再次更新
            /// </summary>
            public string update_status { get; set; }
        }
    }
}
