using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Common.Entities.WX
{
	/// <summary>
	/// WX_ApiConfig:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class WX_ApiConfig
	{
		public WX_ApiConfig()
		{}
		#region Model
		private int _id;
		private string _domain;
		private string _serverip;
		private string _systemname;
		private string _systemlogo;
		private string _appid;
		private string _appsecret;
		private string _token;
		private string _partnerkey;
		private string _partnerid;
		private string _certpath;
		private string _certpwd;
        private string _companyid;
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
        /// 单位信息
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CompanyID
        {
            set { _companyid = value; }
            get { return _companyid; }
        }
        /// <summary>
        /// 微信域名
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Domain
		{
			set{ _domain=value;}
			get{
                return SystemDefaultConfig.SystemDomain;
            }
		}
        /// <summary>
        /// 微信服务器IP
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ServerIP
		{
			set{ _serverip=value;}
			get{return _serverip;}
		}
        /// <summary>
        /// 微信公众号名称
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SystemName
		{
			set{ _systemname=value;}
			get{return _systemname;}
		}
        /// <summary>
        /// 微信公众号LOGO
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SystemLogo
		{
			set{ _systemlogo=value;}
			get{return _systemlogo;}
		}
        /// <summary>
        /// 微信公众号AppId
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AppId
		{
			set{ _appid=value;}
			get{return _appid;}
		}
        /// <summary>
        /// 微信公众号AppSecret
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AppSecret
		{
			set{ _appsecret=value;}
			get{return _appsecret;}
		}
        /// <summary>
        /// 微信公众号Token
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Token
		{
			set{ _token=value;}
			get{return _token;}
		}
        /// <summary>
        /// 微信公众号支付PartnerKey
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PartnerKey
		{
			set{ _partnerkey=value;}
			get{return _partnerkey;}
		}
        /// <summary>
        /// 微信公众号支付PartnerId
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PartnerId
		{
			set{ _partnerid=value;}
			get{return _partnerid;}
		}
        /// <summary>
        /// 微信公众号支付证书路径
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CertPath
		{
			set{ _certpath=value;}
			get{return _certpath;}
		}
        /// <summary>
        /// 微信公众号支付证书密码
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CertPwd
		{
			set{ _certpwd=value;}
			get{return _certpwd;}
		}
        /// <summary>
        /// 是否有效
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Status { get; set; }
        /// <summary>
        /// 支持上级账号支付
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool SupportSuperiorPay { get; set; }
		#endregion Model

	}
}

