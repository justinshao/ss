using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类ParkDerateIntervar。
	/// </summary>
	[Serializable]
	public partial class ParkDerateIntervar
	{
		public ParkDerateIntervar()
		{}
		#region Model
		private int _id;
		private string _derateintervarid;
		private string _derateid;
		private int _freetime;
		private decimal _monetry;
        private int _haveupdate;
		private DateTime _lastupdatetime;
		private DataStatus _datastatus;
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
        /// 时段ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DerateIntervarID
		{
			set{ _derateintervarid=value;}
			get{return _derateintervarid;}
		}
        /// <summary>
        /// 优免ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DerateID
		{
			set{ _derateid=value;}
			get{return _derateid;}
		}
        /// <summary>
        /// 免费时间 （分钟）
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int FreeTime
		{
			set{ _freetime=value;}
			get{return _freetime;}
		}
        /// <summary>
        /// 消费金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Monetry
		{
			set{ _monetry=value;}
			get{return _monetry;}
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
        public DataStatus DataStatus
		{
			set{ _datastatus=value;}
			get{return _datastatus;}
		}
		#endregion Model
	}
}

