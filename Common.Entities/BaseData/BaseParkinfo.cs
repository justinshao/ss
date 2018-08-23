using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类BaseParkinfo。
	/// </summary>
    [Serializable]
    public partial class BaseParkinfo
    {
        public BaseParkinfo()
        { }
        #region Model
        private int _id;
        private string _pkid;
        private string _pkno;
        private string _pkname;
        private int _carbitnum = 0;
        private int _carbitnumleft = 0;
        private int _carbitnumfixed = 0;
        private int _spacebitnum = 0;
        private int _centertime = 0;
        private YesOrNo _allowlosedisplay = 0;
        private string _linkman;
        private string _mobile;
        private string _address;
        private string _coordinate;
        private YesOrNo _mobilepay = 0;
        private YesOrNo _mobilelock = 0;
        private YesOrNo _isparkingspace = 0;
        private YesOrNo _isreverseseekingvehicle = 0;
        private string _feeremark;
        private YesOrNo _online = 0;
        private string _remark;
        private DateTime _lastupdatetime;
        private int _haveupdate;
        private int _cityid;
        private string _vid;
        private DataStatus _datastatus;
        private int _datasavedays = 30;
        private int _picturesavedays = 30;
        private YesOrNo _isonlinegathe = 0;
        private YesOrNo _isline = 0;
        private YesOrNo _needfee = 0;
        private int _expiredadvanceremindday = 0;
        private string _defaultplate;
        private bool _policeFree;
        private decimal _onlineDiscount = 10;
        private bool _isOnlineDiscount = false;
        private bool _unconfirmedCalculation = false;
        private bool _isNoPlateConfirm = false;
        private bool _outerringCharge = false;

        /// <summary>
        /// 内外圈分开算费
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool OuterringCharge
        {
            get { return _outerringCharge; }
            set { _outerringCharge = value; }
        } 
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsNoPlateConfirm
        {
            get { return _isNoPlateConfirm; }
            set { _isNoPlateConfirm = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool UnconfirmedCalculation
        {
            get { return _unconfirmedCalculation; }
            set { _unconfirmedCalculation = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsOnlineDiscount
        {
            get { return _isOnlineDiscount; }
            set { _isOnlineDiscount = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal OnlineDiscount
        {
            get { return _onlineDiscount; }
            set { _onlineDiscount = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool PoliceFree
        {
            get { return _policeFree; }
            set { _policeFree = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
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
        /// 车场编码
        /// </summary>
        public string PKNo
        {
            set { _pkno = value; }
            get { return _pkno; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 车场名称
        /// </summary>
        public string PKName
        {
            set { _pkname = value; }
            get { return _pkname; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 总车位数
        /// </summary>
        public int CarBitNum
        {
            set { _carbitnum = value; }
            get { return _carbitnum; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 临停在场数量
        /// </summary>
        public int CarBitNumLeft
        {
            set { _carbitnumleft = value; }
            get { return _carbitnumleft; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 固定在场数量
        /// </summary>
        public int CarBitNumFixed
        {
            set { _carbitnumfixed = value; }
            get { return _carbitnumfixed; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 剩余车位
        /// </summary>
        public int SpaceBitNum
        {
            set { _spacebitnum = value; }
            get { return _spacebitnum; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 中心缴费超时
        /// </summary>
        public int CenterTime
        {
            set { _centertime = value; }
            get { return _centertime; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 负数是否显示
        /// </summary>
        public YesOrNo AllowLoseDisplay
        {
            set { _allowlosedisplay = value; }
            get { return _allowlosedisplay; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 联系人
        /// </summary>
        public string LinkMan
        {
            set { _linkman = value; }
            get { return _linkman; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Mobile
        {
            set { _mobile = value; }
            get { return _mobile; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address
        {
            set { _address = value; }
            get { return _address; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 坐标
        /// </summary>
        public string Coordinate
        {
            set { _coordinate = value; }
            get { return _coordinate; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 手机支付
        /// </summary>
        public YesOrNo MobilePay
        {
            set { _mobilepay = value; }
            get { return _mobilepay; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 手机锁车
        /// </summary>
        public YesOrNo MobileLock
        {
            set { _mobilelock = value; }
            get { return _mobilelock; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 车位预定
        /// </summary>
        public YesOrNo IsParkingSpace
        {
            set { _isparkingspace = value; }
            get { return _isparkingspace; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 反向寻车
        /// </summary>
        public YesOrNo IsReverseSeekingVehicle
        {
            set { _isreverseseekingvehicle = value; }
            get { return _isreverseseekingvehicle; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 收费描述
        /// </summary>
        public string FeeRemark
        {
            set { _feeremark = value; }
            get { return _feeremark; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 是否在线
        /// </summary>
        public YesOrNo OnLine
        {
            set { _online = value; }
            get { return _online; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public DateTime LastUpdateTime
        {
            set { _lastupdatetime = value; }
            get { return _lastupdatetime; }
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
        /// 城市ID
        /// </summary>
        public int CityID
        {
            set { _cityid = value; }
            get { return _cityid; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 小区ID
        /// </summary>
        public string VID
        {
            set { _vid = value; }
            get { return _vid; }
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
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 数据保存天数
        /// </summary>
        public int DataSaveDays
        {
            set { _datasavedays = value; }
            get { return _datasavedays; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 图片保存天数
        /// </summary>
        public int PictureSaveDays
        {
            set { _picturesavedays = value; }
            get { return _picturesavedays; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 是否线上统计  0: 线下统计  1:线上统计
        /// </summary>
        public YesOrNo IsOnLineGathe
        {
            set { _isonlinegathe = value; }
            get { return _isonlinegathe; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 车辆进出是否排队
        /// </summary>
        public YesOrNo IsLine
        {
            set { _isline = value; }
            get { return _isline; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 过期提取提醒天数（0表示不提醒）
        /// </summary>
        public int ExpiredAdvanceRemindDay
        {
            set { _expiredadvanceremindday = value; }
            get { return _expiredadvanceremindday; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 是否需要收费
        /// </summary>
        public YesOrNo NeedFee
        {
            set { _needfee = value; }
            get { return _needfee; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 默认车牌
        /// </summary>
        public string DefaultPlate
        {
            set { _defaultplate = value; }
            get { return _defaultplate; }
        }
        private bool _supportAutoRefund;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 线上支付通知线下失败自动退款
        /// </summary>
        public bool SupportAutoRefund
        {
            set { _supportAutoRefund = value; }
            get { return _supportAutoRefund; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 结算标识  0: 不支持结算  1: 自动结算  2:手动结算
        /// </summary>
        public int SupprtSettlement
        {
            get;
            set;
        }
        /// <summary>
        /// 是否支持无感   0: 不支持   1:支持
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SupportNoSense
        {
            get;
            set;
        }


        #endregion Model
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double Lat { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double Lng { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ProxyNo { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 手续费  n/1000
        /// </summary>
        public decimal HandlingFee
        {
            get;
            set;
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 最在提现金额
        /// </summary>
        public decimal MaxAmountOfCash
        {
            get;
            set;
        }
        /// <summary>
        /// 每次提现最小额度
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal MinAmountOfCash
        {
            get;
            set;
        }

    }
}

