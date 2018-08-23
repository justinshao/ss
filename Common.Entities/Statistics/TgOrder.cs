using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Statistics
{
   
    public class TgOrder
    {
         
        #region Model
        private int _id;
        private decimal _orderid; 
        private string _pkid;
        private string _pkname;  
        private string _plateno;
        private decimal _amount; 
        private DateTime _realpaytime;
        private int _personid;
        private string _personname;
        private int _count;
        /// <summary>
        /// 主键编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 线上订单编号 
        /// </summary>
        public decimal OrderID
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        /// 车场编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKID
        {
            set { _pkid = value; }
            get { return _pkid; }
        }  
        /// <summary>
        /// 车场名称
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKName
        {
            set { _pkname = value; }
            get { return _pkname; }
        }
        
        /// <summary>
        /// 车牌号码
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        
        public string PlateNo
        {
            set { _plateno = value; }
            get { return _plateno; }
        }
        
       
        /// <summary>
        /// 付款金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Amount
        {
            set { _amount = value; }
            get { return _amount; }
        }
        
        /// <summary>
        /// 实际支付时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime RealPayTime
        {
            set { _realpaytime = value; }
            get { return _realpaytime; }
        }
        public string RealPayTimeToString
        {
            get
            {
                return RealPayTime.ToTimeString();
            }
        }

        /// <summary>
        /// 推广人ID
        /// </summary> 
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PersonId
        {
            set { _personid = value; }
            get { return _personid; }
        }
        /// <summary>
        /// 推广人姓名
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PersonName
        {
            set { _personname = value; }
            get { return _personname; }
        }
        /// <summary>
        /// 推广人ID
        /// </summary>  
        public int Count
        {
            set { _count = value; }
            get { return _count; }
        }
        #endregion Model

    }
}
