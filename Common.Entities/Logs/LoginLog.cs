using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类LoginLog。
	/// </summary>
	[Serializable]
	public partial class LoginLog
	{
		public LoginLog()
		{}
		#region Model
		private int _id;
		private string _useraccount;
		private string _loginip;
		private DateTime _logintime;
		private DateTime _logouttime;
        private LogFrom _logfrom;
		private string _remark;
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
        /// 登录账户
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserAccount
		{
			set{ _useraccount=value;}
			get{return _useraccount;}
		}
        /// <summary>
        /// 登录IP
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LoginIP
		{
			set{ _loginip=value;}
			get{return _loginip;}
		}
        /// <summary>
        /// 登录时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LoginTime
		{
			set{ _logintime=value;}
			get{return _logintime;}
		}
        /// <summary>
        /// 注销登录时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LogoutTime
		{
			set{ _logouttime=value;}
			get{return _logouttime;}
		}
        /// <summary>
        /// 日志来源（平台）
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public LogFrom LogFrom
		{
			set{ _logfrom=value;}
			get{return _logfrom;}
		}
        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		#endregion Model
	}
}

