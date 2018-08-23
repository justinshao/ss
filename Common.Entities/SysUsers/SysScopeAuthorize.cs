using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类SysActionScopeData。
	/// </summary>
	[Serializable]
	public partial class SysScopeAuthorize
	{
        public SysScopeAuthorize()
		{}
		#region Model
		private int _id;
		private string _asid;
		private string _asdid;
		private string _tagid;
        private ASType _astype;
		private string _cpid;
		private DateTime _lastupdatetime;
        private int _haveupdate;
		private DataStatus _datastatus;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ProxyNo { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int HaveUpdate2 { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastUpdateTime2 { set; get; }
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
        /// 作用域ID(主表编号)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ASID
		{
			set{ _asid=value;}
			get{return _asid;}
		}
        /// <summary>
        /// 作用域对应数据ID（子表编号）
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ASDID
		{
			set{ _asdid=value;}
			get{return _asdid;}
		}
        /// <summary>
        /// 作用域目标ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TagID
		{
			set{ _tagid=value;}
			get{return _tagid;}
		}
        /// <summary>
        /// 目标类型作用域类型（1小区，2其他)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ASType ASType
		{
			set{ _astype=value;}
			get{return _astype;}
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

