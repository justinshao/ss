using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Common.Entities.Parking
{
    /// <summary>
    /// ParkDevice:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class ParkDevice : INotifyPropertyChanged
    {
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
        public ParkDevice()
        { }
        #region Model
        private int _id;
        private string _deviceid;
        private string _gateid;
        private DeviceType _devicetype;
        private PortType _porttype;
        private int _baudrate;
        private string _serialport;
        private string _ipaddr;
        private int _ipport;
        private string _username;
        private string _userpwd;
        private int _netid;
        private int _lednum;
        private DataStatus _datastatus;
        private DateTime _lastupdatetime;
        private int _haveupdate;
        private string _deviceNo;
        private int _offlinePort;
        private bool _isCarBit = false;
        private string _mac;
        private bool _isContestDev = false;
        private DeviceTypeBK _controllerType = 0;
        private int _displayMode = 0;
        private bool _isMonitor = false;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsMonitor
        {
            get { return _isMonitor; }
            set { _isMonitor = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int DisplayMode
        {
            get { return _displayMode; }
            set { _displayMode = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DeviceTypeBK ControllerType
        {
            get { return _controllerType; }
            set { _controllerType = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsContestDev
        {
            get { return _isContestDev; }
            set
            {
                _isContestDev = value;
                Notify("IsContestDev");
            }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MAC
        {
            get { return _mac; }
            set { _mac = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsCarBit
        {
            get { return _isCarBit; }
            set { _isCarBit = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int OfflinePort
        {
            get { return _offlinePort; }
            set { _offlinePort = value; }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string DeviceNo
        {
            get { return _deviceNo; }
            set { _deviceNo = value; }
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
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DeviceID
        {
            set
            {
                if (value != _deviceid)
                {
                    _deviceid = value;
                    Notify("DeviceID");
                }
            }
            get { return _deviceid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string GateID
        {
            set
            {
                if (value != _gateid)
                {
                    _gateid = value;
                    Notify("GateID");
                }
            }
            get { return _gateid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DeviceType DeviceType
        {
            set
            {
                if (value != _devicetype)
                {
                    _devicetype = value;
                    Notify("DeviceType");
                }
            }
            get { return _devicetype; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PortType PortType
        {
            set
            {
                if (value != _porttype)
                {
                    _porttype = value;
                    Notify("PortType");
                }
            }
            get { return _porttype; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Baudrate
        {
            set
            {
                if (value != _baudrate)
                {
                    _baudrate = value;
                    Notify("Baudrate");
                }
            }
            get { return _baudrate; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SerialPort
        {
            set
            {
                if (value != _serialport)
                {
                    _serialport = value;
                    Notify("SerialPort");
                }
            }
            get { return _serialport; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string IpAddr
        {
            set
            {
                if (value != _ipaddr)
                {
                    _ipaddr = value;
                    Notify("IpAddr");
                }
            }
            get { return _ipaddr; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int IpPort
        {
            set
            {
                if (value != _ipport)
                {
                    _ipport = value;
                    Notify("IpPort");
                }
            }
            get { return _ipport; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserName
        {
            set
            {
                if (value != _username)
                {
                    _username = value;
                    Notify("UserName");
                }
            }
            get { return _username; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserPwd
        {
            set
            {
                if (value != _userpwd)
                {
                    _userpwd = value;
                    Notify("UserPwd");
                }
            }
            get { return _userpwd; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int NetID
        {
            set
            {
                if (value != _netid)
                {
                    _netid = value;
                    Notify("NetID");
                }
            }
            get { return _netid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int LedNum
        {
            set
            {
                if (value != _lednum)
                {
                    _lednum = value;
                    Notify("LedNum");
                }
            }
            get { return _lednum; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DataStatus DataStatus
        {
            set
            {
                if (value != _datastatus)
                {
                    _datastatus = value;
                    Notify("DataStatus");
                }
            }
            get { return _datastatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastUpdateTime
        {
            set
            {
                if (value != _lastupdatetime)
                {
                    _lastupdatetime = value;
                    Notify("LastUpdateTime");
                }
            }
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
        private bool _isCapture = false;
        /// <summary>
        /// 抓拍相机
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsCapture
        {
            set { _isCapture = value; }
            get { return _isCapture; }
        }

        private bool _isSVoice = false;
        /// <summary>
        /// 智能语音
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsSVoice
        {
            set { _isSVoice = value; }
            get { return _isSVoice; }
        }
        #endregion Model

        #region 扩展属于
        /// <summary>
        /// 车场编号
        /// </summary>
        public string PKID
        {
            get;
            set;
        }
        /// <summary>
        /// 车场编号
        /// </summary>
        public string ParkingName
        {
            get; set;
        }
        /// <summary>
        /// 通道名称
        /// </summary>
        public string GateName
        {
            get;
            set;
        }
        /// <summary>
        /// 岗亭名称
        /// </summary>
        public string BoxName
        {
            get;
            set;
        }
        /// <summary>
        /// 设备状态名称
        /// </summary>
        public string ConnectionStateName
        {
            get;
            set;
        }
        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string DeviceTypeName
        {
            get;
            set;
        }
        /// <summary>
        /// 通信类型名称
        /// </summary>
        public string PortTypeName
        {
            get;
            set;
        }

        #endregion
        #region wpf使用

        private bool _isConnected = false;

        /// <summary>
        /// 是否连接正常
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
            set
            {
                if (value != _isConnected)
                {
                    _isConnected = value;
                    Notify("IsConnected");
                }
            }
        }
        private ParkGate _gate;
        public ParkGate Gate
        {
            set { _gate = value; }
            get { return _gate; }
        }
        private string _devClassString;
        /// <summary>
        /// 设备类型
        /// </summary>	
        public string DevClassString
        {
            get { return _devClassString; }
            set
            {

                if (_devClassString != value)
                {
                    _devClassString = value;
                    Notify("DevClassString");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IntDeviceType
        {
            get { return (int)_devicetype; }
        }

        private bool _is485Connected = false;

        /// <summary>
        /// 是否连接正常
        /// </summary>
        public bool Is485Connected
        {
            get
            {
                return _is485Connected;
            }
            set
            {
                if (value != _is485Connected)
                {
                    _is485Connected = value;
                    Notify("Is485Connected");
                }
            }
        }

        private DateTime _lastConnectTime;
        /// <summary>
        /// 
        /// </summary>
        public DateTime LastConnectTime
        {
            set
            {
                if (value != _lastConnectTime)
                {
                    _lastConnectTime = value;
                    Notify("LastConnectTime");
                }
            }
            get { return _lastConnectTime; }
        }


        private int _voicevalue = 0;

        /// <summary>
        /// 音量大小
        /// </summary>
        public int Voicevalue
        {
            get
            {
                return _voicevalue;
            }
            set
            {
                if (value != _voicevalue)
                {
                    _voicevalue = value;
                    Notify("Voicevalue");
                }
            }
        }
        #endregion
    }
}
