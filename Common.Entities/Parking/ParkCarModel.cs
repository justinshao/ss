using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类ParkCarModel。
	/// </summary>
	[Serializable]
	public partial class ParkCarModel
	{
		public ParkCarModel()
		{}
		#region Model
		private int _id;
		private string _carmodelid;
		private string _carmodelname;
		private string _pkid;
        private YesOrNo _isdefault;
		private DateTime _lastupdatetime;
        private int _haveupdate;
		private DataStatus _datastatus;
        private decimal _maxUseMoney = 0;
        private bool _isNaturalDay=false;
        private string _plateColor;
        private decimal _dayMaxMoney;
        private decimal _nightMaxMoney;
        private DateTime _dayStartTime;
        private DateTime _dayEndTime;
        private DateTime _nightStartTime;
        private DateTime _nightEndTime;
        private int _naturalTime;

        public int NaturalTime
        {
            get { return _naturalTime; }
            set { _naturalTime = value; }
        }
        public DateTime NightEndTime
        {
            get { return _nightEndTime; }
            set { _nightEndTime = value; }
        }
        public DateTime NightStartTime
        {
            get { return _nightStartTime; }
            set { _nightStartTime = value; }
        }
        public DateTime DayEndTime
        {
            get { return _dayEndTime; }
            set { _dayEndTime = value; }
        }
        public DateTime DayStartTime
        {
            get { return _dayStartTime; }
            set { _dayStartTime = value; }
        }
        public decimal NightMaxMoney
        {
            get { return _nightMaxMoney; }
            set { _nightMaxMoney = value; }
        }
        public decimal DayMaxMoney
        {
            get { return _dayMaxMoney; }
            set { _dayMaxMoney = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PlateColor
        {
            get { return _plateColor; }
            set { _plateColor = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsNaturalDay
        {
            get { return _isNaturalDay; }
            set { _isNaturalDay = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal MaxUseMoney
        {
            get { return _maxUseMoney; }
            set { _maxUseMoney = value; }
        }
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
        /// 车型ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarModelID
		{
			set{ _carmodelid=value;}
			get{return _carmodelid;}
		}
        /// <summary>
        /// 车型描述
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarModelName
		{
			set{ _carmodelname=value;}
			get{return _carmodelname;}
		}
        /// <summary>
        /// 车场ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKID
		{
			set{ _pkid=value;}
			get{return _pkid;}
		}
        /// <summary>
        /// 是否默认
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public YesOrNo IsDefault
		{
			set{ _isdefault=value;}
			get{return _isdefault;}
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
		#endregion Model
	}
}

