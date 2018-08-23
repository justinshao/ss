using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Parking
{
    // <summary>
    /// 类ParkVisitor。
    /// </summary>
    [Serializable]
    public partial class ParkVisitor
    {
        public ParkVisitor()
        { }
        #region Model

        private int _id;
        /// <summary>
        /// auto_increment
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ID
        {
            set
            {
                if (_id != value)
                {
                    _id = value;
                }
            }
            get { return _id; }
        }

        private string _recordID;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public string RecordID
        {
            set
            {
                if (_recordID != value)
                {
                    _recordID = value;
                }
            }
            get { return _recordID; }
        }

        private string _visitorid;
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string VisitorID
        {
            set
            {
                if (_visitorid != value)
                {
                    _visitorid = value;
                }
            }
            get { return _visitorid; }
        }

        private string _PKID;
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKID
        {
            set
            {
                if (_PKID != value)
                {
                    _PKID = value;
                }
            }
            get { return _PKID; }
        }




        private string _VID;
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string VID
        {
            set
            {
                if (_VID != value)
                {
                    _VID = value;
                }
            }
            get { return _VID; }
        }

        private int _alreadyVisitorCount;
        /// <summary>
        /// 已经进出次数
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int AlreadyVisitorCount
        {
            set
            {
                if (_alreadyVisitorCount != value)
                {
                    _alreadyVisitorCount = value;
                }
            }
            get { return _alreadyVisitorCount; }
        }

        private DateTime _lastupdatetime;
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastUpdateTime
        {
            set { _lastupdatetime = value; }
            get { return _lastupdatetime; }
        }

        private int _haveupdate;
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int HaveUpdate
        {
            set { _haveupdate = value; }
            get { return _haveupdate; }
        }

        private DataStatus _datastatus;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DataStatus DataStatus
        {
            set { _datastatus = value; }
            get { return _datastatus; }
        }
        /// <summary>
        /// 车场名称
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKName { get; set; }

        /// <summary>
        /// 增加人账号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RegAccountID { set; get; }

        /// <summary>
        /// 车牌号码
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PlateNumber { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime BeginTime { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EndTime { set; get; }
        #endregion Model
    }
}
