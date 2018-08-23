using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类ParkBox。
	/// </summary>
	[Serializable]
	public partial class ParkBox
	{
		public ParkBox()
		{}
		#region Model
		private int _id;
		private string _boxid;
		private string _boxno;
		private string _boxname;
		private string _computerip;
		private string _areaid;
		private string _remark;
        private YesOrNo _iscenterpayment;
		private DataStatus _datastatus;
        private int _haveupdate;
		private DateTime _lastupdatetime;
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
        /// 岗亭ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BoxID
		{
			set{ _boxid=value;}
			get{return _boxid;}
		}
        /// <summary>
        /// 岗亭编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BoxNo
		{
			set{ _boxno=value;}
			get{return _boxno;}
		}
        /// <summary>
        /// 岗亭名称
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BoxName
		{
			set{ _boxname=value;}
			get{return _boxname;}
		}
        /// <summary>
        /// 计算机IP
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ComputerIP
		{
			set{ _computerip=value;}
			get{return _computerip;}
		}
        /// <summary>
        /// 区域ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AreaID
		{
			set{ _areaid=value;}
			get{return _areaid;}
		}
        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
        /// <summary>
        /// 是中心缴费岗亭
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public YesOrNo IsCenterPayment {
            set { _iscenterpayment = value; }
            get { return _iscenterpayment; }
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
        #endregion Model
        #region WPF
        private ParkArea _area;
        public ParkArea Area
        {
            set { _area = value; }
            get { return _area; }
        }
        #endregion
    }
}

