using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类BaseProvince。
	/// </summary>
	[Serializable]
	public partial class BaseProvince
	{
		public BaseProvince()
		{}
		#region Model
		private int _provinceid;
		private string _provincename;
		private int _orderindex;
		private decimal _longitude;
		private decimal _lititute;
		private string _remark;
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
        public string ProvinceName
		{
			set{ _provincename=value;}
			get{return _provincename;}
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

