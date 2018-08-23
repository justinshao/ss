using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Servers
{
    public class GetMoneyModel
    {
        /// <summary>
        /// 返回的错误码, 0: 成功, ⾮非0: 有错误发⽣生 
        /// </summary>
        public string resp_code { set; get; }

        /// <summary>
        /// resp_code=0时, 返回"成功", ⾮非0时, 返回错误描述信 息 
        /// </summary>
        public string resp_desc { set; get; }

        /// <summary>
        /// 平台交易易流⽔水号 
        /// </summary>
        public string tran_no { set; get; }

        /// <summary>
        /// 订单号,纯数字, 唯⼀一, 头8个字⺟母为"YYYYMMDD"的 ⽇日期 
        /// </summary>
        public string order_no { set; get; }

        /// <summary>
        ///  订单⽇日期, 格式: "YYYY-MM-DD hh:mm:ss"
        /// </summary>
        public string order_time { set; get; }

        /// <summary>
        /// 合作商户ID, 由平台提供
        /// </summary>
        public string mer_id { set; get; }

        /// <summary>
        /// 业务类型, 此处给固定值: ParkingFee 
        /// </summary>
        public string biz_type { set; get; }

        /// <summary>
        /// ⻋车牌号码,
        /// </summary>
        public string car_no { set; get; }

        /// <summary>
        /// ⻋车辆进⼊入停⻋车场时间, 格式: YYYY-MM-DD hh:mm:ss 
        /// </summary>
        public string in_time { set; get; }

        /// <summary>
        /// ⻋车辆离开停⻋车场时间, 格式: YYYY-MM-DD hh:mm:ss 
        /// </summary>
        public string out_time { set; get; }

        /// <summary>
        /// 消费⾦金金额, 单位:元, 保留留2位⼩小数, 精确到分
        /// </summary>
        public string amount { set; get; }

        /// <summary>
        /// 停⻋车场名称
        /// </summary>
        public string parking_name { set; get; }

        /// <summary>
        /// 停⻋车场唯⼀一标识字符串串 
        /// </summary>
        public string parking_id { set; get; }

        /// <summary>
        /// 订单交易易结果的异步通知地址,为空则后台不不做异步 通知 
        /// </summary>
        public string asyncnotifyurl { set; get; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string remark { set; get; }

        /// <summary>
        /// 按照⽂文档描述,⽣生成的参数签名(注:sign参数本身是不不 参与签名计算的)
        /// </summary>
        public string sign { set; get; }
    }
}
