using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类BaseCard。
	/// </summary>
	[Serializable] 
    public partial class BaseCard
	{
		public BaseCard()
		{}
		#region Model
		private int _id;
		private string _cardid;
		private string _cardno;
		private string _cardnumb;
        private CardType _cardtype;
		private decimal _balance=0;
        private CardStatus _state = CardStatus.Normal;
		private DateTime _lastupdatetime;
        private int _haveupdate;
        private DataStatus _datastatus;
		private decimal _deposit =0;
		private DateTime _registertime;
		private string _operatorid;
        private CardSystem _cardsystem;
		private string _vid;
        private string _employeeID;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public int ID
		{
			set{ _id=value;}
			get{return _id;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 卡片ID
        /// </summary>
        public string CardID
		{
			set{ _cardid=value;}
			get{return _cardid;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 卡片编号
        /// </summary>
        public string CardNo
		{
			set{ _cardno=value;}
			get{return _cardno;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNumb
		{
			set{ _cardnumb=value;}
			get{return _cardnumb;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 卡类型(1车牌,2卡片)
        /// </summary>
        public CardType CardType
		{
			set{ _cardtype=value;}
			get{return _cardtype;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance
		{
			set{ _balance=value;}
			get{return _balance;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 状态1正常2挂失,3停用4注销
        /// </summary>
        public CardStatus State
		{
			set{ _state=value;}
			get{return _state;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public DateTime LastUpdateTime
		{
			set{ _lastupdatetime=value;}
			get{return _lastupdatetime;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public int HaveUpdate
		{
			set{ _haveupdate=value;}
			get{return _haveupdate;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public DataStatus DataStatus
		{
			set{ _datastatus=value;}
			get{return _datastatus;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 押金
        /// </summary>
        public decimal Deposit
		{
			set{ _deposit=value;}
			get{return _deposit;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public DateTime RegisterTime
		{
			set{ _registertime=value;}
			get{return _registertime;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public string OperatorID
		{
			set{ _operatorid=value;}
			get{return _operatorid;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 卡片所属系统 1.停车场,2.门禁 （按位与运算）
        /// </summary>
        public CardSystem CardSystem
		{
			set{ _cardsystem=value;}
			get{return _cardsystem;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 所属小区
        /// </summary>
        public string VID
		{
			set{ _vid=value;}
			get{return _vid;}
		}
		#endregion Model

        public string EmployeeName { get; set; }
        public string EmployeeMobile { get; set; }
	}
}

