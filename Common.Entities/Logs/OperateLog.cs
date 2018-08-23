using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类OperateLog。
	/// </summary>
	[Serializable]
	public partial class OperateLog
	{
		public OperateLog()
		{}
		#region Model
		private int _id;
		private string _operator;
		private string _operatorip;
		private DateTime _operatetime;
		private string _modulename;
		private string _methodname;
		private LogFrom _logfrom;
        private OperateType _operatetype;
		private string _operatorcontent;
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
        /// 操作人
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Operator
		{
			set{ _operator=value;}
			get{return _operator;}
		}
        /// <summary>
        /// 操作者IP
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OperatorIP
		{
			set{ _operatorip=value;}
			get{return _operatorip;}
		}
        /// <summary>
        /// 操作时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime OperateTime
		{
			set{ _operatetime=value;}
			get{return _operatetime;}
		}
        /// <summary>
        /// 模块名称
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ModuleName
		{
			set{ _modulename=value;}
			get{return _modulename;}
		}
        /// <summary>
        /// 方法名称
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MethodName
		{
			set{ _methodname=value;}
			get{return _methodname;}
		}
        /// <summary>
        /// 日志来源
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public LogFrom LogFrom
		{
			set{ _logfrom=value;}
			get{return _logfrom;}
		}
        /// <summary>
        /// 操作类型
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OperateType OperateType
		{
            set { _operatetype = value; }
            get { return _operatetype; }
		}
        /// <summary>
        /// 操作内容
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OperatorContent
		{
			set{ _operatorcontent=value;}
			get{return _operatorcontent;}
		}
		#endregion Model
	}
}

