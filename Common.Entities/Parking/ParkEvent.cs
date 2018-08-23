using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Parking
{
    /// <summary>
    /// ParkEvent:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class ParkEvent
    {
        public ParkEvent()
        { }
        #region Model
        private int _id;
        private string _recordid;
        private string _cardno;
        private string _cardnum;
        private string _employeename;
        private string _platenumber;
        private PlateColor _platecolor;
        private string _cartypeid;
        private string _carmodelid;
        private DateTime _rectime;
        private string _gateid;
        private string _operatorid;
        private string _picturename;
        private int _eventid;
        private int _iostate;
        private string _iorecordid;
        private string _parkingid;
        private DateTime _lastupdatetime;
        private int _haveupdate;
        private int _datastatus;
        private string _remark;
        private bool _isScanCode;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int IsOffline { set; get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 证件姓名
        /// </summary>
        public string CertName { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 证件号码
        /// </summary>
        public string CertNo { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 证件性别
        /// </summary>
        public string Sex { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 证件名族
        /// </summary>
        public string Nation { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 证件生日
        /// </summary>
        public DateTime BirthDate { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BirthDateToString
        {
            get
            {
                return BirthDate.ToPlatString();
            }
        }
        /// <summary>
        /// 证件住址
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Address { set; get; }

        /// <summary>
        /// 证件图片
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CertificateImage { set; get; }
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
        public string CardNo
        {
            set { _cardno = value; }
            get { return _cardno; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CardNum
        {
            set { _cardnum = value; }
            get { return _cardnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EmployeeName
        {
            set { _employeename = value; }
            get { return _employeename; }
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
        public PlateColor PlateColor
        {
            set { _platecolor = value; }
            get { return _platecolor; }
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
        public DateTime RecTime
        {
            set { _rectime = value; }
            get { return _rectime; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RecTimeToString
        {
            get
            {
                return RecTime.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string GateID
        {
            set { _gateid = value; }
            get { return _gateid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OperatorID
        {
            set { _operatorid = value; }
            get { return _operatorid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PictureName
        {
            set { _picturename = value; }
            get { return _picturename; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int EventID
        {
            set { _eventid = value; }
            get { return _eventid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int IOState
        {
            set { _iostate = value; }
            get { return _iostate; }
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
        public int DataStatus
        {
            set { _datastatus = value; }
            get { return _datastatus; }
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
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsScanCode
        {
            set { _isScanCode = value; }
            get { return _isScanCode; }
        }
        #endregion Model

        #region 扩展字段
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

        public string GateNo { set; get; }
        /// <summary>
        /// 通道名称
        /// </summary>
        public string GateName
        {
            get;
            set;
        }
        /// <summary>
        /// 事件类型名称
        /// </summary>
        public string EventName
        {
            get;
            set;
        }
        /// <summary>
        /// 进出方向名称
        /// </summary>
        public string IOStateName
        {
            get;
            set;
        }
        /// <summary>
        /// 操作员
        /// </summary>
        public string Operator
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
        #endregion

    }
}
