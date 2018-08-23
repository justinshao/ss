using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类ParkSeller。
	/// </summary>
	[Serializable]
	public partial class ParkCarlineInfo
    {
		public ParkCarlineInfo()
		{}
		#region Model
		private int _id;
		private string _gateid;
		private string _PKID;
		private string _plateNumber;
        private DateTime _targetTime;
        private string _remark; 
		private DateTime _lastupdatetime;
        private int _haveupdate;
		private DataStatus _datastatus;

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
        /// <summary>
        /// 通道ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Gateid
        {
			set{ _gateid = value;}
			get{return _gateid; }
		}
        /// <summary>
        /// PKID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKID
        {
			set{ _PKID = value;}
			get{return _PKID; }
		}
        /// <summary>
        /// PlateNumber
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PlateNumber
        {
			set{ _plateNumber = value;}
			get{return _plateNumber; }
		}
        /// <summary>
        /// TargetTime
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime TargetTime
        {
			set{ _targetTime = value;}
			get{return _targetTime; }
		}
        /// <summary>
        /// Remark
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Remark
        {
			set{ _remark = value;}
			get{return _remark; }
		}
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastUpdateTime
		{
			set{ _lastupdatetime=value;}
			get{return _lastupdatetime;}
		}
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int HaveUpdate
		{
			set{ _haveupdate=value;}
			get{return _haveupdate;}
		}
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DataStatus DataStatus
		{
			set{ _datastatus=value;}
			get{return _datastatus;}
		} 
		#endregion Model
	}
}

