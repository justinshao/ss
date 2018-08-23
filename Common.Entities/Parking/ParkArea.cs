using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
    /// <summary>
    /// 类ParkArea。
    /// </summary>
    [Serializable]
    public partial class ParkArea
    {
        public ParkArea()
        { }
        #region Model
        private int _id;
        private string _areaid;
        private string _areaname;
        private string _masterid;
        private string _pkid;
        private int _carbitnum = 0;
        private DateTime _lastupdatetime;
        private int _haveupdate;
        private YesOrNo _needtoll = 0;
        private int _camerawaittime;
        private YesOrNo _twocamerawait = 0;
        private string _remark;
        private DataStatus _datastatus;
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
        /// 区域Id
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AreaID
        {
            set { _areaid = value; }
            get { return _areaid; }
        }
        /// <summary>
        /// 区域描述
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AreaName
        {
            set { _areaname = value; }
            get { return _areaname; }
        }
        /// <summary>
        /// 上级区域
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MasterID
        {
            set { _masterid = value; }
            get { return _masterid; }
        }
        /// <summary>
        /// 车场ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKID
        {
            set { _pkid = value; }
            get { return _pkid; }
        }
        /// <summary>
        /// 车位数量
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int CarbitNum
        {
            set { _carbitnum = value; }
            get { return _carbitnum; }
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
        /// 是否收费
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public YesOrNo NeedToll
        {
            set { _needtoll = value; }
            get { return _needtoll; }
        }
        /// <summary>
        /// 等待时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int CameraWaitTime
        {
            set { _camerawaittime = value; }
            get { return _camerawaittime; }
        }
        /// <summary>
        /// 双摄像头是否等待
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public YesOrNo TwoCameraWait
        {
            set { _twocamerawait = value; }
            get { return _twocamerawait; }
        }
        /// <summary>
        /// 备注
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
        public DataStatus DataStatus
        {
            set { _datastatus = value; }
            get { return _datastatus; }
        }
        private ParkArea _parent;
        public ParkArea Parent
        {
            set { _parent = value; }
            get { return _parent; }
        }

        /// <summary>
        /// 是否内部车场
        /// </summary>	
        public bool IsNestArea
        {
            get
            {
                if(MasterID==AreaID)
                {
                    return false;
                }
                return string.IsNullOrEmpty(MasterID) ? false : true;
            }
        }
        #endregion Model

        #region WPF
         
        private   BaseParkinfo _parkinfo = null;
        /// <summary>
        ///  (监控端缓存)岗亭所在的车场
        /// </summary>
        public   BaseParkinfo Parkinfo
        {
            get
            {
                return _parkinfo;
            }
            set
            {
                _parkinfo = value;
            }
        }
        #endregion
    }
}

