using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Common.Entities.Parking
{

    /// <summary>
    /// ParkTimeseries。
    /// </summary>
    [Serializable]
    public partial class ParkTimeseries
    {
        public ParkTimeseries()
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
        /// <summary>
        /// auto_increment
        /// </summary>		
        private int _id;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ID
        {
            get { return _id; }
            set
            {

                if (_id != value)
                {
                    _id = value;
                    Notify("ID");
                }
            }
        }
        /// <summary>
        /// auto_increment
        /// </summary>		
        private string _timeseriesID;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TimeseriesID
        {
            get { return _timeseriesID; }
            set
            {

                if (_timeseriesID != value)
                {
                    _timeseriesID = value;
                    Notify("TimeseriesID");
                }
            }
        }

        private string _ioRecordID;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string IORecordID
        {
            get { return _ioRecordID; }
            set
            {

                if (_ioRecordID != value)
                {
                    _ioRecordID = value;
                    Notify("IORecordID");
                }
            }
        }


        /// <summary>
        /// CardNo
        /// </summary>		
        private string _enterImage;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EnterImage
        {
            get { return _enterImage; }
            set
            {

                if (_enterImage != value)
                {
                    _enterImage = value;
                    Notify("EnterImage");
                }
            }
        }
        /// <summary>
        /// CarLp
        /// </summary>		
        private string _exitImage;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExitImage
        {
            get { return _exitImage; }
            set
            {

                if (_exitImage != value)
                {
                    _exitImage = value;
                    Notify("ExitImage");
                }
            }
        }
        /// <summary>
        /// ParkingID
        /// </summary>		
        private string _parkingid;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ParkingID
        {
            get { return _parkingid; }
            set
            {

                if (_parkingid != value)
                {
                    _parkingid = value;
                    Notify("ParkingID");
                }
            }
        }
        /// <summary>
        /// RecTime
        /// </summary>		
        private DateTime _enterTime;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EnterTime
        {
            get { return _enterTime; }
            set
            {

                if (_enterTime != value)
                {
                    _enterTime = value;
                    Notify("EnterTime");
                }
            }
        }
        /// <summary>
        /// RecTime
        /// </summary>		
        private DateTime _exitTime;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ExitTime
        {
            get { return _exitTime; }
            set
            {

                if (_exitTime != value)
                {
                    _exitTime = value;
                    Notify("ExitTime");
                }
            }
        }
        /// <summary>
        /// State
        /// </summary>		
        private string _exitGateID;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExitGateID
        {
            get { return _exitGateID; }
            set
            {

                if (_exitGateID != value)
                {
                    _exitGateID = value;
                    Notify("ExitGateID");
                }
            }
        }

        /// <summary>
        /// State
        /// </summary>		
        private string _enterGateID;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EnterGateID
        {
            get { return _enterGateID; }
            set
            {

                if (_enterGateID != value)
                {
                    _enterGateID = value;
                    Notify("EnterGateID");
                }
            }
        }
        /// <summary>
        /// State
        /// </summary>		
        private bool _isExit;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsExit
        {
            get { return _isExit; }
            set
            {

                if (_isExit != value)
                {
                    _isExit = value;
                    Notify("IsExit");
                }
            }
        }
        /// <summary>
        /// LastUpdateTime
        /// </summary>		
        private DateTime _lastUpdateTime;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastUpdateTime
        {
            get { return _lastUpdateTime; }
            set
            {

                if (_lastUpdateTime != value)
                {
                    _lastUpdateTime = value;
                    Notify("LastUpdateTime");
                }
            }
        }
        /// <summary>
        /// HaveUpdate
        /// </summary>		
        private int _haveUpdate;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int HaveUpdate
        {
            get { return _haveUpdate; }
            set
            {

                if (_haveUpdate != value)
                {
                    _haveUpdate = value;
                    Notify("HaveUpdate");
                }
            }
        }

        private DataStatus _datastatus;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DataStatus DataStatus
        {
            set { _datastatus = value; }
            get { return _datastatus; }
        } 

        private int _releaseType;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ReleaseType
        {
            get { return _releaseType; }
            set
            {

                if (_releaseType != value)
                {
                    _releaseType = value;
                    Notify("ReleaseType");
                }
            }
        }

        private ParkIORecord _iorecord;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ParkIORecord IORecord
        {
            get { return _iorecord; }
            set
            {

                if (_iorecord != value)
                {
                    _iorecord = value;
                    Notify("IORecord");
                }
            }
        }


        private string _inGateName = string.Empty;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        /// <summary>
        /// 进通道
        /// </summary>
        public string InGateName
        {
            get { return _inGateName; }
            set
            {

                if (_inGateName != value)
                {
                    _inGateName = value;
                    Notify("InGateName");
                }
            }
        }

        private string _outGateName = string.Empty;

        /// <summary>
        /// 出通道
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OutGateName
        {
            get { return _outGateName; }
            set
            {

                if (_outGateName != value)
                {
                    _outGateName = value;
                    Notify("OutGateName");
                }
            }
        } 
    }
}
