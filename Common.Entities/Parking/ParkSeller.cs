using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类ParkSeller。
	/// </summary>
	[Serializable]
	public partial class ParkSeller
	{
		public ParkSeller()
		{}
		#region Model
		private int _id;
		private string _sellerid;
		private string _sellerno;
		private string _sellername;
		private string _pwd;
		private string _vid;
		private string _addr;
		private DateTime _lastupdatetime;
        private int _haveupdate;
		private DataStatus _datastatus;
		private int _creditline;
		private decimal _balance;
        private string _pPSellerID;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PPSellerID
        {
            get { return _pPSellerID; }
            set { _pPSellerID = value; }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal EnableBalance { set; get; }
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
        /// 商户ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SellerID
		{
			set{ _sellerid=value;}
			get{return _sellerid;}
		}
        /// <summary>
        /// 商户编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SellerNo
		{
			set{ _sellerno=value;}
			get{return _sellerno;}
		}
        /// <summary>
        /// 商户名称
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SellerName
		{
			set{ _sellername=value;}
			get{return _sellername;}
		}
        /// <summary>
        /// 密码
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PWD
		{
			set{ _pwd=value;}
			get{return _pwd;}
		}
        /// <summary>
        /// 小区ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string VID
		{
			set{ _vid=value;}
			get{return _vid;}
		}
        /// <summary>
        /// 地址
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Addr
		{
			set{ _addr=value;}
			get{return _addr;}
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
        public int HaveUpdate
		{
			set{ _haveupdate=value;}
			get{return _haveupdate;}
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
        /// 透支金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Creditline
		{
			set{ _creditline=value;}
			get{return _creditline;}
		}
        /// <summary>
        /// 余额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Balance
		{
			set{ _balance=value;}
			get{return _balance;}
		}
		#endregion Model
	}
}

