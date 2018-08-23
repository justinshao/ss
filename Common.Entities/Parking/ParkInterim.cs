using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Parking
{
    /// <summary>
	/// 类EmployeePlate。
	/// </summary>
	[Serializable]
    public partial class ParkInterim
    {
        public ParkInterim()
        { }

        private int _id;
        private string _RecordID;
        private string _IORecordID;
        private DateTime _StartInterimTime;
        private DateTime _EndInterimTime;
        private int _HaveUpdate;
        private DateTime _LastUpdateTime;
        private DataStatus _DataStatus;
        private string _Remark;
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
            set { _RecordID = value; }
            get { return _RecordID; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string IORecordID
        {
            set { _IORecordID = value; }
            get { return _IORecordID; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartInterimTime
        {
            set { _StartInterimTime = value; }
            get { return _StartInterimTime; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EndInterimTime
        {
            set { _EndInterimTime = value; }
            get { return _EndInterimTime; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int HaveUpdate
        {
            set { _HaveUpdate = value; }
            get { return _HaveUpdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastUpdateTime
        {
            set { _LastUpdateTime = value; }
            get { return _LastUpdateTime; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DataStatus DataStatus
        {
            set { _DataStatus = value; }
            get { return _DataStatus; }
        }
        /// <summary>
        /// 
        /// </summary
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Remark
        {
            set { _Remark = value; }
            get { return _Remark; }
        }
    }
}
