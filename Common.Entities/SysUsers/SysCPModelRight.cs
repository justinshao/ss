using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类SysCPModelRight。
	/// </summary>
	[Serializable]
	public partial class SysCPModelRight
	{
		public SysCPModelRight()
		{}
		#region Model
		private int _id;
		private string _recordid;
		private string _cpid;
		private string _moduleid;
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
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RecordID
		{
			set{ _recordid=value;}
			get{return _recordid;}
		}
        /// <summary>
        /// 公司ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CPID
		{
			set{ _cpid=value;}
			get{return _cpid;}
		}
        /// <summary>
        /// 模块ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ModuleID
		{
			set{ _moduleid=value;}
			get{return _moduleid;}
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

