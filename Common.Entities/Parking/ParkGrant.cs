using Newtonsoft.Json;
using System;

namespace Common.Entities
{
	/// <summary>
	/// 类ParkGrant。
	/// </summary>
	[Serializable]
	public partial class ParkGrant
	{
		public ParkGrant()
		{}
		#region Model
		private int _id;
		private string _gid;
		private string _cardid;
		private string _pkid;
		private DateTime _begindate;
		private DateTime _enddate;
		private string _cartypeid;
		private string _carmodelid;
		private string _pklot;
		private string _plateid;
		private DateTime _lastupdatetime;
        private int _haveupdate;
        private ComdState _comdstate;
		private string _areaids;
		private string _gateid;
        private ParkGrantState _state;
		private DataStatus _datastatus;

        private int _pKLotNum=1;
          [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PKLotNum
        {
            get { return _pKLotNum; }
            set { _pKLotNum = value; }
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
        /// 授权ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string GID
		{
			set{ _gid=value;}
			get{return _gid;}
		}
        /// <summary>
        /// 卡ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CardID
		{
			set{ _cardid=value;}
			get{return _cardid;}
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
        /// 开始时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime BeginDate
		{
			set{ _begindate=value;}
			get{return _begindate;}
		}
        /// <summary>
        /// 结束时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EndDate
		{
			set{ _enddate=value;}
			get{
                if (_enddate != DateTime.MinValue)
                {
                    return _enddate.Date.AddDays(1).AddSeconds(-1);
                }
                return _enddate;
            }
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
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarModelID
		{
			set{ _carmodelid=value;}
			get{return _carmodelid;}
		}
        /// <summary>
        /// 车位号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKLot
		{
			set{ _pklot=value;}
			get{return _pklot;}
		}
        /// <summary>
        /// 车牌ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PlateID
		{
			set{ _plateid=value;}
			get{return _plateid;}
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
        /// 下发状态(0正常,1新增,2修改,3删除)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ComdState ComdState
		{
			set{ _comdstate=value;}
			get{return _comdstate;}
		}
        /// <summary>
        /// 区域权限(空为所有)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AreaIDS
		{
			set{ _areaids=value;}
			get{return _areaids;}
		}
        /// <summary>
        /// 通道权限(空为所有)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string GateID
		{
			set{ _gateid=value;}
			get{return _gateid;}
		}
        /// <summary>
        /// 状态(0正常，1停止，2暂停)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ParkGrantState State
		{
			set{ _state=value;}
			get{return _state;}
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
        #endregion Model

        #region wpf中使用

        private BaseCard _usercard;
         [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BaseCard Usercard
        {
            get { return _usercard; }
            set
            {
                if (_usercard != value)
                {
                    _usercard = value; 
                }
            }
        }

        private EmployeePlate _ownerPlateNumber;
         [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public EmployeePlate OwnerPlateNumber
        {
            get { return _ownerPlateNumber; }
            set
            {
                if (_ownerPlateNumber != value)
                {
                    _ownerPlateNumber = value; 
                }
            }
        }

        private ParkCarType _cardType;
         [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ParkCarType CardType
        {
            get { return _cardType; }
            set
            {
                if (_cardType != value)
                {
                    _cardType = value; 
                }
            }
        }

        /// <summary>
        /// TelPhone
        /// </summary>		
        private string _homePhone;
         [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HomePhone
        {
            get { return _homePhone; }
            set
            {
                if (_homePhone != value)
                {
                    _homePhone = value; 
                }
            }
        }

        /// <summary>
        /// PlateNo
        /// </summary>		
        private string _plateNo;
         [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PlateNo
        {
            get { return _plateNo; }
            set
            {
                if (_plateNo != value)
                {
                    _plateNo = value;
                }
            }
        }

        #endregion
    }
}

