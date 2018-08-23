using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Common.Entities.WX
{
	/// <summary>
	/// WX_UserLocation:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class WX_UserLocation
	{
		public WX_UserLocation()
		{}
		#region Model
		private int _id;
		private string _openid;
		private string _latitude;
		private string _longitude;
		private string _precision;
		private DateTime _lastreportedtime;
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
        /// 微信用户编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OpenId
		{
			set{ _openid=value;}
			get{return _openid;}
		}
        /// <summary>
        /// 纬度
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Latitude
		{
			set{ _latitude=value;}
			get{return _latitude;}
		}
        /// <summary>
        /// 经度
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Longitude
		{
			set{ _longitude=value;}
			get{return _longitude;}
		}
        /// <summary>
        /// 地理位置精度
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Precision
		{
			set{ _precision=value;}
			get{return _precision;}
		}
        /// <summary>
        /// 最后位置时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastReportedTime
		{
			set{ _lastreportedtime=value;}
			get{return _lastreportedtime;}
		}
		#endregion Model

	}
}

