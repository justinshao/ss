using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类EmployeePlate。
	/// </summary>
	[Serializable]
	public partial class EmployeePlate
	{
		public EmployeePlate()
		{}
		#region Model
		private int _id;
		private string _plateid;
		private string _employeeid;
		private string _plateno;
		private DateTime _lastupdatetime;
        private int _haveupdate;
        private DataStatus _datastatus;
        private PlateColor _color;
		private string _carbrand;
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
        /// 车牌ID
        /// </summary>
        public string PlateID
		{
			set{ _plateid=value;}
			get{return _plateid;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 所属人员
        /// </summary>
        public string EmployeeID
		{
			set{ _employeeid=value;}
			get{return _employeeid;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 车牌号码
        /// </summary>
        public string PlateNo
		{
			set{ _plateno=value;}
			get{return _plateno;}
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
        /// <summary>
        /// 车牌颜色
        /// </summary>
        public PlateColor Color
		{
			set{ _color=value;}
			get{return _color;}
		}
        /// <summary>
        /// 车辆品牌
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarBrand
		{
			set{ _carbrand=value;}
			get{return _carbrand;}
		}
		#endregion Model

	}
}

