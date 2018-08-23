using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
    /// <summary>
    /// 类ParkCarDerate。
    /// </summary>
    [Serializable]
    public partial class ParkCarDerate
    {
        public ParkCarDerate()
        { }
        #region Model
        private int _id;
        private string _carderateid;
        private string _carderateno;
        private string _derateid;
        private int _freetime = 0;
        private decimal _freemoney = 0;
        private string _platenumber;
        private string _cardno;
        private string _iorecordid;
        private CarDerateStatus _status;
        private DateTime _createtime;
        private DateTime _expirytime;
        private string _pkid;
        private string _areaid;
        private int _haveupdate;
        private DataStatus _datastatus;
        private DateTime _lastupdatetime;
        private int _enforce;
        private string _reason;
        private string _identity;
        public long SumMin { set; get; }
        public string Identity
        {
            get { return _identity; }
            set { _identity = value; }
        }
        public string Reason
        {
            get { return _reason; }
            set { _reason = value; }
        }
        public int Enforce
        {
            get { return _enforce; }
            set { _enforce = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 车辆优免ID(可做二维码生成)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarDerateID
        {
            set { _carderateid = value; }
            get { return _carderateid; }
        }
        /// <summary>
        /// 优免卷编号(待定)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarDerateNo
        {
            set { _carderateno = value; }
            get { return _carderateno; }
        }
        /// <summary>
        /// 商家优免ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DerateID
        {
            set { _derateid = value; }
            get { return _derateid; }
        }
        /// <summary>
        /// 免费时长
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int FreeTime
        {
            set { _freetime = value; }
            get { return _freetime; }
        }
        /// <summary>
        /// 免费金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal FreeMoney
        {
            set { _freemoney = value; }
            get { return _freemoney; }
        }
        /// <summary>
        /// 车牌号码
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PlateNumber
        {
            set { _platenumber = value; }
            get { return _platenumber; }
        }
        /// <summary>
        /// 卡片编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CardNo
        {
            set { _cardno = value; }
            get { return _cardno; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string IORecordID
        {
            set { _iorecordid = value; }
            get { return _iorecordid; }
        }
        ///<summary>
        /// 状态 0正常 1已经使用 2 已结算 3 作废
        ///</summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CarDerateStatus Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 发放时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CreateTimeToString
        {
            get
            {
                return CreateTime.ToyyyyMMddHHmmss();
            }
        }
        /// <summary>
        /// 有效截止时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ExpiryTime
        {
            set { _expirytime = value; }
            get { return _expirytime; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExpiryTimeToString
        {
            get
            {
                return ExpiryTime.ToyyyyMMddHHmmss();
            }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 车场ID
        /// </summary>
        public string PKID
        {
            set { _pkid = value; }
            get { return _pkid; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 区域ID
        /// </summary>
        public string AreaID
        {
            set { _areaid = value; }
            get { return _areaid; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public int HaveUpdate
        {
            set { _haveupdate = value; }
            get { return _haveupdate; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public DataStatus DataStatus
        {
            set { _datastatus = value; }
            get { return _datastatus; }
        }
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastUpdateTime
        {
            set { _lastupdatetime = value; }
            get { return _lastupdatetime; }
        }
        /// <summary>
        /// 优免券二维码编号
        /// </summary>
        public string DerateQRCodeID { get; set; }
        #endregion Model

        #region 优免规则
        private ParkDerate _derate;
        /// <summary>
        /// 优免规则
        /// </summary>
        public ParkDerate Derate
        {
            get { return _derate; }
            set
            {

                if (_derate != value)
                {
                    _derate = value;
                }
            }
        }
        #endregion

        #region 扩展方法
        /// <summary>
        /// 商家名称
        /// </summary>
        public string SellerName
        {
            get;
            set;
        }
        /// <summary>
        /// 规则名称
        /// </summary>
        public string RuleName
        {
            get;
            set;
        }
        public string StatusName
        {
            get;
            set;
        }
        /// <summary>
        /// 车场名称
        /// </summary>
        public string ParkingName
        {
            get;
            set;
        }
        /// <summary>
        /// 优免规则名称
        /// </summary>
        public string DerateName { get; set; }
        /// <summary>
        /// 优免规则类型
        /// </summary>
        public string DerateType { get; set; }
        public string QRCodeImageBase64String { get; set; }
        #endregion
    }
}

