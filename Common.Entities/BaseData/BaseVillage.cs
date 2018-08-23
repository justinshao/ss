using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类BaseVillage。
	/// </summary>
	[Serializable]
	public partial class BaseVillage
	{
		public BaseVillage()
		{}
		#region Model
		private int _id;
		private string _vid;
		private string _vno;
		private string _vname;
		private string _cpid;
		private string _linkman;
		private string _mobile;
		private string _address;
		private string _coordinate;
		private string _proxyno;
		private DateTime _lastupdatetime;
        private int _haveupdate;
        private DataStatus _datastatus;
        private int _isonline;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 小区ID
        /// </summary>
        public string VID
		{
			set{ _vid=value;}
			get{return _vid;}
		}
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 小区编号
        /// </summary>
        public string VNo
		{
			set{ _vno=value;}
			get{return _vno;}
		}
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 小区名称
        /// </summary>
        public string VName
		{
			set{ _vname=value;}
			get{return _vname;}
		}
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 公司ID
        /// </summary>
        public string CPID
		{
			set{ _cpid=value;}
			get{return _cpid;}
		}
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 联系人
        /// </summary>
        public string LinkMan
		{
			set{ _linkman=value;}
			get{return _linkman;}
		}
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Mobile
		{
			set{ _mobile=value;}
			get{return _mobile;}
		}
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address
		{
			set{ _address=value;}
			get{return _address;}
		}
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 坐标
        /// </summary>
        public string Coordinate
		{
			set{ _coordinate=value;}
			get{return _coordinate;}
		}
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 代理编号
        /// </summary>
        public string ProxyNo
		{
			set{ _proxyno=value;}
			get{return _proxyno;}
		}
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public DateTime LastUpdateTime
		{
			set{ _lastupdatetime=value;}
			get{return _lastupdatetime;}
		}
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public int HaveUpdate
		{
			set{ _haveupdate=value;}
			get{return _haveupdate;}
		}
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public DataStatus DataStatus
		{
			set{ _datastatus=value;}
			get{return _datastatus;}
		}
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int IsOnLine
        {
            get { return _isonline; }
            set { _isonline = value; }
        }
		#endregion Model
	}
}

