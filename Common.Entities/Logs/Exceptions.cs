using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类Exceptions。
	/// </summary>
	[Serializable]
	public partial class Exceptions
	{
		public Exceptions()
		{}
		#region Model
		private int _id;
		private DateTime _datetime;
		private string _source;
		private string _server;
		private string _description;
		private string _detail;
		private string _track;
        private LogFrom _logfrom;
        /// <summary>
        /// 
        /// </summary>
        public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime DateTime
		{
			set{ _datetime=value;}
			get{return _datetime;}
		}
        /// <summary>
        /// 源
        /// </summary>
        public string Source
		{
			set{ _source=value;}
			get{return _source;}
		}
        /// <summary>
        /// 服务器
        /// </summary>
        public string Server
		{
			set{ _server=value;}
			get{return _server;}
		}
        /// <summary>
        /// 描述
        /// </summary>
        public string Description
		{
			set{ _description=value;}
			get{return _description;}
		}
        /// <summary>
        /// 详细
        /// </summary>
        public string Detail
		{
			set{ _detail=value;}
			get{return _detail;}
		}
        /// <summary>
        /// 跟踪
        /// </summary>
        public string Track
		{
			set{ _track=value;}
			get{return _track;}
		}
        /// <summary>
        /// 日志来源（平台）
        /// </summary>
        public LogFrom LogFrom
		{
			set{ _logfrom=value;}
			get{return _logfrom;}
		}
		#endregion Model

	}
}

