using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类SysUser。
	/// </summary>
	[Serializable]
	public partial class SysUser
	{
		public SysUser()
		{}
		#region Model
		private int _id;
		private string _recordid;
		private string _useraccount;
		private string _username;
		private string _password;
		private DateTime _pwderrortime;
		private int _pwderrorcount=0;
		private DateTime _lastupdatetime;
        private int _haveupdate;
		private string _cpid;
		private DataStatus _datastatus;
		private YesOrNo _isdefaultuser;
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
        /// 纪录ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RecordID
		{
			set{ _recordid=value;}
			get{return _recordid;}
		}
        /// <summary>
        /// 登陆账号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserAccount
		{
			set{ _useraccount=value;}
			get{return _useraccount;}
		}
        /// <summary>
        /// 账号姓名
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserName
		{
			set{ _username=value;}
			get{return _username;}
		}
        /// <summary>
        /// 密码
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Password
		{
			set{ _password=value;}
			get{return _password;}
		}
        /// <summary>
        /// 最后错误时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime PwdErrorTime
		{
			set{ _pwderrortime=value;}
			get{return _pwderrortime;}
		}
        /// <summary>
        /// 错误次数
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PwdErrorCount
		{
			set{ _pwderrorcount=value;}
			get{return _pwderrorcount;}
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
        public DataStatus DataStatus
		{
			set{ _datastatus=value;}
			get{return _datastatus;}
		}
        /// <summary>
        /// 默认用户
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public YesOrNo IsDefaultUser
		{
			set{ _isdefaultuser=value;}
			get{return _isdefaultuser;}
		}
		#endregion Model
	}
}

