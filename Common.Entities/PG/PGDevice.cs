using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Common.Entities.PG
{
    /// <summary>
    /// PGDevice:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class PGDevice: INotifyPropertyChanged
    {
        public PGDevice()
        { }

        #region 属性变更事件
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        #endregion
        #region Model
        private int _id;
        private string _deviceid;
        private string _areaid;
        private string _devicename;
        private string _ipaddr;
        private int _ipport;
        private string _devclass;
        private int _channelnum;
        private int _porttype;
        private int _netid;
        private int _baudrate;
        private string _serialport;
        private int _timeouts;
        private string _arealist;
        private string _username;
        private string _userpwd;
        private DateTime _lastupdatetime;
        private int _haveupdate;
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ID
        {
            set { _id = value; Notify("ID"); }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DeviceID
        {
            set { _deviceid = value; Notify("DeviceID"); }
            get { return _deviceid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AreaID
        {
            set { _areaid = value; Notify("AreaID"); }
            get { return _areaid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DeviceName
        {
            set { _devicename = value; Notify("DeviceName"); }
            get { return _devicename; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string IpAddr
        {
            set { _ipaddr = value; Notify("IpAddr"); }
            get { return _ipaddr; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int IpPort
        {
            set { _ipport = value; Notify("IpPort"); }
            get { return _ipport; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DevClass
        {
            set { _devclass = value; Notify("DevClass"); }
            get { return _devclass; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ChannelNum
        {
            set { _channelnum = value; Notify("ChannelNum"); }
            get { return _channelnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PortType
        {
            set { _porttype = value; Notify("PortType"); }
            get { return _porttype; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int NetID
        {
            set { _netid = value; Notify("NetID"); }
            get { return _netid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Baudrate
        {
            set { _baudrate = value; Notify("Baudrate"); }
            get { return _baudrate; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SerialPort
        {
            set { _serialport = value; Notify("SerialPort"); }
            get { return _serialport; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Timeouts
        {
            set { _timeouts = value; Notify("Timeouts"); }
            get { return _timeouts; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AreaList
        {
            set { _arealist = value; Notify("AreaList"); }
            get { return _arealist; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserName
        {
            set { _username = value; Notify("UserName"); }
            get { return _username; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserPwd
        {
            set { _userpwd = value; Notify("UserPwd"); }
            get { return _userpwd; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastUpdateTime
        {
            set { _lastupdatetime = value; Notify("LastUpdateTime"); }
            get { return _lastupdatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int HaveUpdate
        {
            set { _haveupdate = value; Notify("HaveUpdate"); }
            get { return _haveupdate; }
        }
        #endregion Model

    }
}
