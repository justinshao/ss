using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Parking
{
    /// <summary>
    /// ParkDeviceParam:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class ParkDeviceParam
    {
        public ParkDeviceParam()
        { }
        #region Model
        private int _id;
        private string _recordid;
        private string _deviceid;
        private int _vipmode;
        private int _tempmode;
        private int _netoffmode;
        private int _vipdevmultin;
        private int _ploicfree;
        private int _vipdutyday;
        private int _overdutyyorn;
        private int _overdutyday;
        private int _sysid;
        private int _devid;
        private int _sysindev;
        private int _sysoutdev;
        private int _sysparknumber;
        private int _devinorout;
        private int _swipeinterval;
        private int _unkonwcardtype;
        private int _lednumber;
        private int _datastatus;
        private DateTime _lastupdatetime;
        private int _haveupdate;
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
        public string DeviceID
        {
            set { _deviceid = value; }
            get { return _deviceid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int VipMode
        {
            set { _vipmode = value; }
            get { return _vipmode; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int TempMode
        {
            set { _tempmode = value; }
            get { return _tempmode; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int NetOffMode
        {
            set { _netoffmode = value; }
            get { return _netoffmode; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int VipDevMultIn
        {
            set { _vipdevmultin = value; }
            get { return _vipdevmultin; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PloicFree
        {
            set { _ploicfree = value; }
            get { return _ploicfree; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int VipDutyDay
        {
            set { _vipdutyday = value; }
            get { return _vipdutyday; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int OverDutyYorN
        {
            set { _overdutyyorn = value; }
            get { return _overdutyyorn; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int OverDutyDay
        {
            set { _overdutyday = value; }
            get { return _overdutyday; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SysID
        {
            set { _sysid = value; }
            get { return _sysid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int DevID
        {
            set { _devid = value; }
            get { return _devid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SysInDev
        {
            set { _sysindev = value; }
            get { return _sysindev; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SysOutDev
        {
            set { _sysoutdev = value; }
            get { return _sysoutdev; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SysParkNumber
        {
            set { _sysparknumber = value; }
            get { return _sysparknumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int DevInorOut
        {
            set { _devinorout = value; }
            get { return _devinorout; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SwipeInterval
        {
            set { _swipeinterval = value; }
            get { return _swipeinterval; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int UnKonwCardType
        {
            set { _unkonwcardtype = value; }
            get { return _unkonwcardtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int LEDNumber
        {
            set { _lednumber = value; }
            get { return _lednumber; }
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
        #endregion Model

    }
}
