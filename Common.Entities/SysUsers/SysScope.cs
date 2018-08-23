using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
    /// 类SysScope。
	/// </summary>
	[Serializable]
	public partial class SysScope
	{
        public SysScope()
		{}
		#region Model
		private int _id;
		private string _asid;
		private string _cpid;
		private string _asname;
		private DateTime _lastupdatetime;
        private int _haveupdate;
		private DataStatus _datastatus;
        private YesOrNo _isdefaultscope = 0;

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
        /// 作用域ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ASID
		{
			set{ _asid=value;}
			get{return _asid;}
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
        /// 作用域名称
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ASName
		{
			set{ _asname=value;}
			get{return _asname;}
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
        /// <summary>
        /// 默认作用域
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public YesOrNo IsDefaultScope
		{
			set{ _isdefaultscope=value;}
			get{return _isdefaultscope;}
		}
		#endregion Model
	}
}

