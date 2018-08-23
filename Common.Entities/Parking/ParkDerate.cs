using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类ParkDerate。
	/// </summary>
	[Serializable]
	public partial class ParkDerate
	{
		public ParkDerate()
		{}
		#region Model
		private int _id;
		private string _derateid;
		private string _sellerid;
		private string _name;
        private DerateSwparate _derateswparate;
        private DerateType _deratetype;
		private decimal _deratemoney;
		private int _freetime;
		private DateTime _starttime;
		private DateTime _endtime;
		private string _feeruleid;
        private int _haveupdate;
		private DateTime _lastupdatetime;
		private DataStatus _datastatus;
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
        /// <summary>
        /// 优免ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DerateID
		{
			set{ _derateid=value;}
			get{return _derateid;}
		}
        /// <summary>
        /// 商家ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SellerID
		{
			set{ _sellerid=value;}
			get{return _sellerid;}
		}
        /// <summary>
        /// 优免名称
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
        /// <summary>
        /// 优免时间切分方式 （0前面时间，1后面时间）
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DerateSwparate DerateSwparate
		{
			set{ _derateswparate=value;}
			get{return _derateswparate;}
		}
        /// <summary>
        /// 优免类型 0按时缴费 1不收取费用 2按次收费3按票收费4分时段收费5按收费标准6特定时段按次收费7按票收费（特殊规则）8减免金额9减免时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DerateType DerateType
		{
			set{ _deratetype=value;}
			get{return _deratetype;}
		}
        /// <summary>
        /// 优免金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal DerateMoney
		{
			set{ _deratemoney=value;}
			get{return _deratemoney;}
		}
        /// <summary>
        /// 免费时间(分钟)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int FreeTime
		{
			set{ _freetime=value;}
			get{return _freetime;}
		}
		/// <summary>
		/// 开始时间
		/// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public DateTime StartTime
		{
			set{ _starttime=value;}
			get{return _starttime;}
		}
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
        /// <summary>
        /// 费率ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FeeRuleID
		{
			set{ _feeruleid=value;}
			get{return _feeruleid;}
		}
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int HaveUpdate
		{
			set{ _haveupdate=value;}
			get{return _haveupdate;}
		}
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastUpdateTime
		{
			set{ _lastupdatetime=value;}
			get{return _lastupdatetime;}
		}
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DataStatus DataStatus
		{
			set{ _datastatus=value;}
			get{return _datastatus;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal DayMoney { set; get; }
        /// <summary>
        /// 最大优免次数周期 0表示不限制，-1表示按自然月,-2表示按周,大于0表示多少天内
        /// </summary>
        public int MaxTimes { get; set; }
        /// <summary>
        /// 最大优免次数 0 不限制
        /// </summary>
        public int MaxTimesCycle { get; set; }
        /// <summary>
        /// 优惠券有效期（分钟） 0为7天
        /// </summary>
        public int ValidityTime { get; set; }
        #endregion Model

        #region ParkSeller
        private ParkSeller _seller;
        /// <summary>
        /// 商家
        /// </summary>
        public ParkSeller Seller
        {
            get { return _seller; }
            set
            {

                if (_seller != value)
                {
                    _seller = value; 
                }
            }
        }
        #endregion
        /// <summary>
        /// 优免时间段
        /// </summary>
        public List<ParkDerateIntervar> DerateIntervar { get; set; }
    }
}

