using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类ParkCarType。
	/// </summary>
	[Serializable]
	public partial class ParkCarType
	{
		public ParkCarType()
		{}
		#region Model
		private int _id;
		private string _cartypeid;
		private string _cartypename;
		private string _pkid;
        private BaseCarType _basetypeid;
		private YesOrNo _repeatin;
        private YesOrNo _repeatout;
        private YesOrNo _affirmin;
        private YesOrNo _affirmout;
		private DateTime _inbegintime;
		private DateTime _inedntime;
		private decimal _maxusemoney =0;
        private YesOrNo _allowlose;
        private LpDistinguish _lpdistinguish;
        private YesOrNo _inouteditcar;
		private int _inouttime;
        private YesOrNo _carnolike;
		private DateTime _lastupdatetime;
        private int _haveupdate;
        private YesOrNo _isallowonline;
		private decimal _amount;
        private decimal _seasonamount;
        private decimal _yearamount;
		private int _maxmonth;
        private int _maxseason;
        private int _maxyear;
        private int _avemonth;
		private decimal _maxvalue;
		private DataStatus _datastatus;
        private OverdueToTemp _overduetotemp;
        private LotOccupy _lotoccupy;
		private decimal _deposit;
		private int _monthcardexpiredenterday;
		private string _affirmbegin;
		private string _affirmend;
        private bool _isNeedCapturePaper;
        private bool _isNeedAuthentication;
        private bool _isDispatch;
        private int _onlineUnit=1;
        private bool _isIgnoreHZ = true;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsIgnoreHZ
        {
            get { return _isIgnoreHZ; }
            set { _isIgnoreHZ = value; }
        }
        /// <summary>
        /// 自定义续期月数
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int AveMonth
        {
            get { return _avemonth; }
            set { _avemonth = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int OnlineUnit
        {
            get { return _onlineUnit; }
            set { _onlineUnit = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsDispatch
        {
            get { return _isDispatch; }
            set { _isDispatch = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsNeedAuthentication
        {
            get { return _isNeedAuthentication; }
            set { _isNeedAuthentication = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 是否开启证件抓拍
        /// </summary>
        public bool IsNeedCapturePaper
        {
            get { return _isNeedCapturePaper; }
            set { _isNeedCapturePaper = value; }
        }
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
        /// 车类型ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarTypeID
		{
			set{ _cartypeid=value;}
			get{return _cartypeid;}
		}
        /// <summary>
        /// 车类型描述
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarTypeName
		{
			set{ _cartypename=value;}
			get{return _cartypename;}
		}
        /// <summary>
        /// 车场ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKID
		{
			set{ _pkid=value;}
			get{return _pkid;}
		}
        /// <summary>
        /// 基础类型(0贵宾卡、1储值卡、2月卡、3临时卡、4工作卡、5季卡、6年卡、7自定义月卡)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BaseCarType BaseTypeID
		{
			set{ _basetypeid=value;}
			get{return _basetypeid;}
		}
        /// <summary>
        /// 重入
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public YesOrNo RepeatIn
		{
			set{ _repeatin=value;}
			get{return _repeatin;}
		}
        /// <summary>
        /// 重出
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public YesOrNo RepeatOut
		{
			set{ _repeatout=value;}
			get{return _repeatout;}
		}
        /// <summary>
        /// 入场确认
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public YesOrNo AffirmIn
		{
			set{ _affirmin=value;}
			get{return _affirmin;}
		}
        /// <summary>
        /// 出场确认
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public YesOrNo AffirmOut
		{
			set{ _affirmout=value;}
			get{return _affirmout;}
		}
        /// <summary>
        /// 允许进时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime InBeginTime
		{
			set{ _inbegintime=value;}
			get{return _inbegintime;}
		}
        /// <summary>
        /// 允许出时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime InEdnTime
		{
			set{ _inedntime=value;}
			get{return _inedntime;}
		}
        /// <summary>
        /// 24小时最大金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal MaxUseMoney
		{
			set{ _maxusemoney=value;}
			get{return _maxusemoney;}
		}
        /// <summary>
        /// 满位是否能进
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public YesOrNo AllowLose
		{
			set{ _allowlose=value;}
			get{return _allowlose;}
		}
        /// <summary>
        /// 进出方式(1刷卡、2车牌+刷卡、3车牌）
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public LpDistinguish LpDistinguish
		{
			set{ _lpdistinguish=value;}
			get{return _lpdistinguish;}
		}
		/// <summary>
		/// 进出修改车位数
		/// </summary>
        public YesOrNo InOutEditCar
		{
			set{ _inouteditcar=value;}
			get{return _inouteditcar;}
		}
        /// <summary>
        /// 进出最小间隔
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int InOutTime
		{
			set{ _inouttime=value;}
			get{return _inouttime;}
		}
        /// <summary>
        /// 支持模糊识别
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public YesOrNo CarNoLike
		{
			set{ _carnolike=value;}
			get{return _carnolike;}
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
        public int HaveUpdate
		{
			set{ _haveupdate=value;}
			get{return _haveupdate;}
		}
        /// <summary>
        /// 允许线上缴费
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public YesOrNo IsAllowOnlIne
		{
			set{ _isallowonline=value;}
			get{return _isallowonline;}
		}
        /// <summary>
        /// 续费月金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Amount
		{
			set{ _amount=value;}
			get{return _amount;}
		}
        /// <summary>
        /// 续费季金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal SeasonAmount
        {
            set { _seasonamount = value; }
            get { return _seasonamount; }
        }
        /// <summary>
        /// 续费年金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal YearAmount
        {
            set { _yearamount = value; }
            get { return _yearamount; }
        }
        /// <summary>
        /// 线上续费最大月数
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int MaxMonth
		{
			set{ _maxmonth=value;}
			get{return _maxmonth;}
		}
        /// <summary>
        /// 线上续费最大季数
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int MaxSeason
        {
            set { _maxseason = value; }
            get { return _maxseason; }
        }
        /// <summary>
        /// 线上续费最大年数
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int MaxYear
        {
            set { _maxyear = value; }
            get { return _maxyear; }
        }
        /// <summary>
        /// 线上充值最大金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal MaxValue
		{
			set{ _maxvalue=value;}
			get{return _maxvalue;}
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
        /// <summary>
        /// 过期转临停0过期转临停1禁止进出
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OverdueToTemp OverdueToTemp
		{
			set{ _overduetotemp=value;}
			get{return _overduetotemp;}
		}
        /// <summary>
        /// 车位占用0转临停1禁止进出
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public LotOccupy LotOccupy
		{
			set{ _lotoccupy=value;}
			get{return _lotoccupy;}
		}
        /// <summary>
        /// 押金
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Deposit
		{
			set{ _deposit=value;}
			get{return _deposit;}
		}
        /// <summary>
        /// 过期允许进出天数
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int MonthCardExpiredEnterDay
		{
			set{ _monthcardexpiredenterday=value;}
			get{return _monthcardexpiredenterday;}
		}
        /// <summary>
        /// 确认进出开始时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AffirmBegin
		{
			set{ _affirmbegin=value;}
			get{return _affirmbegin;}
		}
        /// <summary>
        /// 确认进出结束时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AffirmEnd
		{
			set{ _affirmend=value;}
			get{return _affirmend;}
		}
		#endregion Model
	}
}

