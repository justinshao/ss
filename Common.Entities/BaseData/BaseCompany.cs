using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类BaseCompany。
	/// </summary>
	[Serializable]
	public partial class BaseCompany
	{
		public BaseCompany()
		{}
		#region Model
		private int _id;
		private string _cpid;
		private string _cpname;
		private int _cityid;
		private string _address;
		private string _linkman;
		private string _mobile;
		private string _masterid;
		private DateTime _lastupdatetime;
        private int _haveupdate;
        private DataStatus _datastatus = 0;
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
        /// 公司ID
        /// </summary>
        public string CPID
		{
			set{ _cpid=value;}
			get{return _cpid;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CPName
		{
			set{ _cpname=value;}
			get{return _cpname;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 所属城市ID
        /// </summary>
        public int CityID
		{
			set{ _cityid=value;}
			get{return _cityid;}
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
        /// 上级ID
        /// </summary>
        public string MasterID
		{
			set{ _masterid=value;}
			get{return _masterid;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime LastUpdateTime
		{
			set{ _lastupdatetime=value;}
			get{return _lastupdatetime;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 是否修改
        /// </summary>
        public int HaveUpdate
		{
			set{ _haveupdate=value;}
			get{return _haveupdate;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 纪录状态(0正常，2删除)
        /// </summary>
        public DataStatus DataStatus
		{
			set{ _datastatus=value;}
			get{return _datastatus;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 用户账号(仅用于单位添加默认用户)
        /// </summary>
        public string UserAccount { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 用户密码(仅用于单位添加默认用户)
        /// </summary>
        public string UserPassword { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public int HaveUpdate2 { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastUpdateTime2 { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //签名密钥
        public string Secretkey { get; set; }
        public BaseVillage ListVillage { set; get; }
		#endregion Model
	}
}

