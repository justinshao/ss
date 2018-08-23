using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类BaseCity。
	/// </summary>
	[Serializable]
	public partial class BaseCity
	{
		public BaseCity()
		{}
		#region Model
		private int _cityid;
		private int _provinceid;
		private string _cityname;
		private int _orderindex;
		private decimal _longitude;
		private decimal _lititute;
		private string _remark;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public int CityID
		{
			set{ _cityid=value;}
			get{return _cityid;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public int ProvinceID
		{
			set{ _provinceid=value;}
			get{return _provinceid;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public string CityName
		{
			set{ _cityname=value;}
			get{return _cityname;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public int OrderIndex
		{
			set{ _orderindex=value;}
			get{return _orderindex;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public decimal Longitude
		{
			set{ _longitude=value;}
			get{return _longitude;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public decimal Lititute
		{
			set{ _lititute=value;}
			get{return _lititute;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		#endregion Model

	}
}

