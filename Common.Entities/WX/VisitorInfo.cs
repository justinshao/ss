using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Newtonsoft.Json;

namespace Common.Entities.WX
{
    /// <summary>
    /// VisitorInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class VisitorInfo
    {
        public VisitorInfo()
        { }
        #region Model
        private int _id;
        private string _recordID;
        private string _platenumber;
        private string _visitorMobilePhone; 
        private int _visitorcount;
        private DateTime _begindate;
        private DateTime _enddate;
        private string _accountid;
        private DateTime _createtime;
        private int _visitorstate;
        private DateTime _lastUpdateTime;
        private int _haveUpdate;
        private int _dataStatus; 
        private string _VID; 
        private int _lookSate; 
        private int _isExamine;
        private string _visitorName;
        private string _visitorSex;
        private DateTime _birthday;
        private int _certifType;
        private string _certifNo;
        private string _certifAddr;
        private string _employeeID;
        private string _cardNo;
        private string _operatorID;
        private string _visitorCompany;
        private string _vistorPhoto;
        private string _certifPhoto;
        private string _remark;
        private string _userName="";
        private string _LongTime;
        private string _gateName;
        private string _entranceImage;
        private string _entranceCertificateImage;
        private string _exitImage;
        private string _exitcertificateImage;
        private string _exitGateName;
        private DateTime _exitTime=DateTime.Now;
        private string _carModelID;

        public string CarModelID
        {
            get { return _carModelID; }
            set { _carModelID = value; }
        }
        public DateTime ExitTime
        {
            get { return _exitTime; }
            set { _exitTime = value; }
        }
        public decimal Amount { set; get; }
        public decimal PayAmount { set; get; }
        public decimal UnPayAmount { set; get; }
        public string ExitGateName
        {
            get { return _exitGateName; }
            set { _exitGateName = value; }
        }
        public string ExitcertificateImage
        {
            get { return _exitcertificateImage; }
            set { _exitcertificateImage = value; }
        }

        public string ExitImage
        {
            get { return _exitImage; }
            set { _exitImage = value; }
        }
        public string EntranceCertificateImage
        {
            get { return _entranceCertificateImage; }
            set { _entranceCertificateImage = value; }
        }
        public string EntranceImage
        {
            get { return _entranceImage; }
            set { _entranceImage = value; }
        }
        public string GateName
        {
            get { return _gateName; }
            set { _gateName = value; }
        }

        public string LongTime
        {
            get
            {
                TimeSpan aa = ExitTime - EntranceTime;
               return _LongTime = aa.Days + "天" + aa.Hours + "时" + aa.Minutes + "分";
            }
            set { _LongTime = value; }
        }

        private DateTime _entranceTime;

        public DateTime EntranceTime
        {
            get { return _entranceTime; }
            set { 
                _entranceTime = value;
              
            }
        }
        
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ID
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string RecordID
        {
            get
            {
                return _recordID;
            }

            set
            {
                _recordID = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PlateNumber
        {
            get
            {
                return _platenumber;
            }

            set
            {
                _platenumber = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string VisitorMobilePhone
        {
            get
            {
                return _visitorMobilePhone;
            }

            set
            {
                _visitorMobilePhone = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int VisitorCount
        {
            get
            {
                return _visitorcount;
            }

            set
            {
                _visitorcount = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime BeginDate
        {
            get
            {
                return _begindate;
            }

            set
            {
                _begindate = value;
            }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public DateTime EndDate
        {
            get
            {
                return _enddate;
            }

            set
            {
                _enddate = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AccountID
        {
            get
            {
                return _accountid;
            }

            set
            {
                _accountid = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CreateTime
        {
            get
            {
                return _createtime;
            }

            set
            {
                _createtime = value;
            }
        }
        /// <summary>
        /// 访客状态 1-正常 2-取消 3-已完结
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int VisitorState
        {
            get
            {
                return _visitorstate;
            }

            set
            {
                _visitorstate = value;
            }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public DateTime LastUpdateTime
        {
            get
            {
                return _lastUpdateTime;
            }

            set
            {
                _lastUpdateTime = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int HaveUpdate
        {
            get
            {
                return _haveUpdate;
            }

            set
            {
                _haveUpdate = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int DataStatus
        {
            get
            {
                return _dataStatus;
            }

            set
            {
                _dataStatus = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string VID
        {
            get
            {
                return _VID;
            }

            set
            {
                _VID = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int LookSate
        {
            get
            {
                return _lookSate;
            }

            set
            {
                _lookSate = value;
            }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 审核状态0未审核，1已审核，2拒绝
        /// </summary>
        public int IsExamine
        {
            get
            {
                return _isExamine;
            }

            set
            {
                _isExamine = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string VisitorName
        {
            get
            {
                return _visitorName;
            }

            set
            {
                _visitorName = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string VisitorSex
        {
            get
            {
                return _visitorSex;
            }

            set
            {
                _visitorSex = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Birthday
        {
            get
            {
                return _birthday;
            }

            set
            {
                _birthday = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int CertifType
        {
            get
            {
                return _certifType;
            }

            set
            {
                _certifType = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CertifNo
        {
            get
            {
                return _certifNo;
            }

            set
            {
                _certifNo = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CertifAddr
        {
            get
            {
                return _certifAddr;
            }

            set
            {
                _certifAddr = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EmployeeID
        {
            get
            {
                return _employeeID;
            }

            set
            {
                _employeeID = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CardNo
        {
            get
            {
                return _cardNo;
            }

            set
            {
                _cardNo = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OperatorID
        {
            get
            {
                return _operatorID;
            }

            set
            {
                _operatorID = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string VisitorCompany
        {
            get
            {
                return _visitorCompany;
            }

            set
            {
                _visitorCompany = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string VistorPhoto
        {
            get
            {
                return _vistorPhoto;
            }

            set
            {
                _vistorPhoto = value;
            }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string CertifPhoto
        {
            get
            {
                return _certifPhoto;
            }

            set
            {
                _certifPhoto = value;
            }
        }
        private int _visitorsource;
        /// <summary>
        /// 访客来源 0-监控端 1-微信
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int VisitorSource
        {
            set { _visitorsource = value; }
            get { return _visitorsource; }
        }

        /// <summary>
        /// 访客车辆信息
        /// </summary>
        public List<ParkVisitor> ParkVisitors { get; set; }
        /// <summary>
        /// 小区名称
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string VName { get; set; }
        #endregion Model
        public int AlreadyVisitorCount { set; get; }
    }
}
