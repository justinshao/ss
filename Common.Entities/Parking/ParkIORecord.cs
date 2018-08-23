using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Common.Entities.Parking
{
    /// <summary>
    /// ParkIORecord:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class ParkIORecord
    {
        public ParkIORecord()
        { }
        #region Model
        private int _id;
        private string _recordid;
        private string _cardid;
        private string _platenumber;
        private string _cardno;
        private string _cardnumb;
        private DateTime _entrancetime;
        private string _entranceimage;
        private string _entrancegateid;
        private string _entranceoperatorid;
        private DateTime _exittime;
        private string _exitimage;
        private string _exitgateid;
        private string _exitoperatorid;
        private string _cartypeid;
        private string _carmodelid;
        private bool _isexit;
        private string _areaid;
        private string _parkingid;
        private int _releasetype;
        private int _entertype;
        private int _datastatus;
        private bool _mhout;
        private DateTime _lastupdatetime;
        private int _haveupdate;
        private string _remark;
        private bool _isScanCodeIn;
        private bool _isScanCodeOut;
        private string _gateNo;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string GateNo
        {
            get { return _gateNo; }
            set { _gateNo = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int IsOffline { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int OfflineID { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public bool IsDiscount { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DiscountTime { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DerateID { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string InimgData { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PlateColor PlateColor { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string IsexitName { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExittimeName { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 进证件姓名
        /// </summary>
        public string EntranceCertName { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 进证件性别
        /// </summary>
        public string EntranceSex { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 进证件名族
        /// </summary>
        public string EntranceNation { set; get; }
        /// <summary>
        /// 进证件生日
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EntranceBirthDate { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EntranceBirthDateToString {
            get {
                return EntranceBirthDate.ToPlatString();
            }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 进证件住址
        /// </summary>
        public string EntranceAddress { set; get; }
        /// <summary>
        /// 出证件姓名
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExitCertName { set; get; }

        /// <summary>
        /// 出证件性别
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExitSex { set; get; }
        /// <summary>
        /// 出证件名族
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExitNation { set; get; }
        /// <summary>
        /// 出证件生日
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ExitBirthDate { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExitBirthDateToString
        {
            get
            {
                return ExitBirthDate.ToPlatString();
            }
        }
        /// <summary>
        /// 出证件住址
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExitAddress { set; get; }

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
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RecordID
        {
            set { _recordid = value; }
            get { return _recordid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CardID
        {
            set { _cardid = value; }
            get { return _cardid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PlateNumber
        {
            set { _platenumber = value; }
            get { return _platenumber; }
        }
        /// <summary>
        /// 
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
        public string CardNumb
        {
            set { _cardnumb = value; }
            get { return _cardnumb; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EntranceTime
        {
            set { _entrancetime = value; }
            get { return _entrancetime; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EntranceTimeToString
        {
            get
            {
                return EntranceTime.ToyyyyMMddHHmmss();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EntranceImage
        {
            set { _entranceimage = value; }
            get { return _entranceimage; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EntranceGateID
        {
            set { _entrancegateid = value; }
            get { return _entrancegateid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EntranceOperatorID
        {
            set { _entranceoperatorid = value; }
            get { return _entranceoperatorid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ExitTime
        {
            set { 
                _exittime = value;
                if (_exittime.ToString() == "1900-1-0 0:00:00")
                {
                    ExittimeName = "";
                }
                else
                {
                    ExittimeName = _exittime.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            get
            {
                return _exittime;
            }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExitTimeToString
        {
            get
            {
                return ExitTime.ToyyyyMMddHHmmss();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExitImage
        {
            set { _exitimage = value; }
            get { return _exitimage; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExitGateID
        {
            set { _exitgateid = value; }
            get { return _exitgateid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExitOperatorID
        {
            set { _exitoperatorid = value; }
            get { return _exitoperatorid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarTypeID
        {
            set { _cartypeid = value; }
            get { return _cartypeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarModelID
        {
            set { _carmodelid = value; }
            get { return _carmodelid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsExit
        {
            set { 
                _isexit = value;
                if (_isexit)
                {
                    IsexitName = "是";
                }
                else
                {
                    IsexitName = "否";
                }
            }
            get { return _isexit; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AreaID
        {
            set { _areaid = value; }
            get { return _areaid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ParkingID
        {
            set { _parkingid = value; }
            get { return _parkingid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ReleaseType
        {
            set { _releasetype = value; }
            get { return _releasetype; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int EnterType
        {
            set { _entertype = value; }
            get { return _entertype; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int DataStatus
        {
            set { _datastatus = value; }
            get { return _datastatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool MHOut
        {
            set { _mhout = value; }
            get { return _mhout; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastUpdateTime
        {
            set { _lastupdatetime = value; }
            get { return _lastupdatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int HaveUpdate
        {
            set { _haveupdate = value; }
            get { return _haveupdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }


        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsScanCodeIn
        {
            set { _isScanCodeIn = value; }
            get { return _isScanCodeIn; }
        }


        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsScanCodeOut
        {
            set { _isScanCodeOut = value; }
            get { return _isScanCodeOut; }
        }
        private string _entranceCertificateNo;
        /// <summary>
        /// 进场身份证号码
        /// </summary>	
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EntranceCertificateNo
        {
            get { return _entranceCertificateNo; }
            set
            {

                if (_entranceCertificateNo != value)
                {
                    _entranceCertificateNo = value;
                }
            }
        }

        private string _exitCertificateNo;
        /// <summary>
        /// 出场身份证号码
        /// </summary>	
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExitCertificateNo
        {
            get { return _exitCertificateNo; }
            set
            {

                if (_exitCertificateNo != value)
                {
                    _exitCertificateNo = value;
                }
            }
        }

        private string _entranceCertificateImage;
        /// <summary>
        /// 进场身份抓拍
        /// </summary>	
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EntranceCertificateImage
        {
            get { return _entranceCertificateImage; }
            set
            {

                if (_entranceCertificateImage != value)
                {
                    _entranceCertificateImage = value;
                }
            }
        }

        private string _exitcertificateImage;
        /// <summary>
        /// 出场身份抓拍
        /// </summary>	
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExitcertificateImage
        {
            get { return _exitcertificateImage; }
            set
            {

                if (_exitcertificateImage != value)
                {
                    _exitcertificateImage = value;
                }
            }
        }



        private string _entranceIDCardPhoto;
        /// <summary>
        /// 入场证件头像
        /// </summary>	
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EntranceIDCardPhoto
        {
            get { return _entranceIDCardPhoto; }
            set
            {

                if (_entranceIDCardPhoto != value)
                {
                    _entranceIDCardPhoto = value;
                }
            }
        }

        private string _exitIDCardPhoto;
        /// <summary>
        /// 出场证件头像
        /// </summary>	
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExitIDCardPhoto
        {
            get { return _exitIDCardPhoto; }
            set
            {

                if (_exitIDCardPhoto != value)
                {
                    _exitIDCardPhoto = value;
                }
            }
        }

        private byte[] _resFeature;
        /// <summary>
        /// 特征码
        /// </summary>	
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public byte[] ResFeature
        {
            get { return _resFeature; }
            set
            {

                if (_resFeature != value)
                {
                    _resFeature = value;
                }
            }
        }
         
        private string _logName;
        /// <summary>
        /// 车标
        /// </summary>	
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LogName
        {
            get { return _logName; }
            set
            {

                if (_logName != value)
                {
                    _logName = value;
                }
            }
        }
        private string _enterDistinguish;
        /// <summary>
        /// 入场识别车牌
        /// </summary>	
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EnterDistinguish
        {
            get { return _enterDistinguish; }
            set
            {

                if (_enterDistinguish != value)
                {
                    _enterDistinguish = value;
                }
            }
        }

        private string _exitDistinguish;
        /// <summary>
        /// 出场识别车牌
        /// </summary>	
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExitDistinguish
        {
            get { return _exitDistinguish; }
            set
            { 
                if (_exitDistinguish != value)
                {
                    _exitDistinguish = value;
                }
            }
        }

        private decimal _netWeighth;
        /// <summary>
        ///  重量
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal NetWeight
        {
            get { return _netWeighth; }
            set
            {
                if (_netWeighth != value)
                {
                    _netWeighth = value;
                }
            }
        }

        private decimal _tare;
        /// <summary>
        ///  皮重量
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Tare
        {
            get { return _tare; }
            set
            {
                if (_tare != value)
                {
                    _tare = value;
                }
            }
        }

        private string _goods;
        /// <summary>
        /// 物品
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Goods
        {
            get { return _goods; }
            set
            {
                if (_goods != value)
                {
                    _goods = value;
                }
            }
        }



        private string _shipper;
        /// <summary>
        /// 货主
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Shipper
        {
            get { return _shipper; }
            set
            {
                if (_shipper != value)
                {
                    _shipper = value;
                }
            }
        }


        private string _Shippingspace;
        /// <summary>
        /// 仓位号 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Shippingspace
        {
            get { return _Shippingspace; }
            set
            {
                if (_Shippingspace != value)
                {
                    _Shippingspace = value;
                }
            }
        }

        private string _documentsNo;
        /// <summary>
        /// 单据编号 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DocumentsNo
        {
            get { return _documentsNo; }
            set
            {
                if (_documentsNo != value)
                {
                    _documentsNo = value;
                }
            }
        }
        private string _visitorID;
        /// <summary>
        /// 单据编号 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string VisitorID
        {
            get { return _visitorID; }
            set
            {
                if (_visitorID != value)
                {
                    _visitorID = value;
                }
            }
        }
        int _sensorlessuser;
        /// <summary>
        /// 是否签约用户 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SensorlessUser
        {
            get
            {
                return _sensorlessuser;
            }
            set
            {
                if (_sensorlessuser != value)
                {
                    _sensorlessuser = value;
                }
            }
        }

        #endregion Model

        #region 扩展字段
        /// <summary>
        /// 停车时长
        /// </summary>
        public string LongTime
        {
            get;
            set;
        }

        public decimal JZWeight { set; get; }

        public decimal ZZWeight { set; get; }
        /// <summary>
        /// 车场名称
        /// </summary>
        public string PKName
        {
            get;
            set;
        }
        /// <summary>
        /// 进道口名称
        /// </summary>
        public string InGateName
        {
            get;
            set;
        }
        /// <summary>
        /// 出道口名称
        /// </summary>
        public string OutGateName
        {
            get;
            set;
        }
        /// <summary>
        /// 进操作员
        /// </summary>
        public string InOperatorName
        {
            get;
            set;
        }
        /// <summary>
        /// 出操作员
        /// </summary>
        public string OutOperatorName
        {
            get;
            set;
        }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName
        {
            get;
            set;
        }
        /// <summary>
        /// 放行类型名
        /// </summary>
        public string ReleaseTypeName
        {
            get;
            set;
        }
        /// <summary>
        /// 手机编号
        /// </summary>
        public string MobilePhone
        {
            get;
            set;
        }
        /// <summary>
        /// 卡主
        /// </summary>
        public string EmployeeName
        {
            get;
            set;
        }
        /// <summary>
        /// 进场卡片类型名称
        /// </summary>
        public string CarTypeName
        {
            get;
            set;
        }
        /// <summary>
        /// 车型
        /// </summary>
        public string CarModelName
        {
            get;
            set;
        }

        public int BaseTypeID { set; get; }
        public byte[] InImage { set; get; }


        private float _likeMatch = 0;
        /// <summary>
        /// 匹配度 
        /// </summary>
        public float LikeMatch
        {
            get { return _likeMatch; }
            set
            {

                if (_likeMatch != value)
                {
                    _likeMatch = value; 
                }
            }
        }

        private string _likeMatchString = "";
        /// <summary>
        /// 匹配度 
        /// </summary>
        public string LikeMatchString
        {
            get
            {
                if (ResFeature == null)
                {
                    return "无数据";
                }
                return (_likeMatch * 100).ToString("f2") + "%";
            }
        }
        public decimal Amount { set; get; }
        private string _reserveBitID = "";

        public string ReserveBitID
        {
            get { return _reserveBitID; }
            set { _reserveBitID = value; }
        }

        public string OrderNo { set; get; }


        /// <summary>
        /// 现金收费
        /// </summary>
        public decimal Money
        {
            get; set;
        }
        /// <summary>
        /// 折扣金额
        /// </summary>
        public decimal DiscountAmount
        {
            get; set;
        }
        /// <summary>
        ///  
        /// </summary>
        public decimal PayAmount
        {
            get; set;
        }
        public decimal JinWeight
        {
            get; set;
        }

        public decimal FBWeight
        {
            get; set;
        }

        public int SpaceBitNum { set; get; }

        public string CarderateID { set; get; }
        #endregion
    }
}
