using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Common.Entities.Parking;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类ParkGate。
	/// </summary>
	[Serializable]
	public partial class ParkGate
	{
		public ParkGate()
		{}
		#region Model
		private int _id;
		private string _gateid;
		private string _gateno;
		private string _gatename;
		private string _boxid;
        private IoState _iostate;
        private YesOrNo _istempinout;
        private YesOrNo _isenterconfirm;
        private YesOrNo _openplateblurrymatch;
		private string _remark;
		private DataStatus _datastatus;
		private DateTime _lastupdatetime;
        private int _haveupdate;
        private bool _isNeedCapturePaper;
        private bool _plateNumberAndCard;
        private bool _isWeight;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsWeight
        {
            get { return _isWeight; }
            set { _isWeight = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool PlateNumberAndCard
        {
            get { return _plateNumberAndCard; }
            set { _plateNumberAndCard = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsNeedCapturePaper
        {
            get { return _isNeedCapturePaper; }
            set { _isNeedCapturePaper = value; }
        }
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
        /// 通道ID
        /// </summary>
        public string GateID
		{
			set{ _gateid=value;}
			get{return _gateid;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 通道编号
        /// </summary>
        public string GateNo
		{
			set{ _gateno=value;}
			get{return _gateno;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 通道名称
        /// </summary>
        public string GateName
		{
			set{ _gatename=value;}
			get{return _gatename;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 岗亭Id
        /// </summary>
        public string BoxID
		{
			set{ _boxid=value;}
			get{return _boxid;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 进出（1进，2出
        /// </summary>
        public IoState IoState
		{
			set{ _iostate=value;}
			get{return _iostate;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 临停是否允许进
        /// </summary>
        public YesOrNo IsTempInOut
		{
			set{ _istempinout=value;}
			get{return _istempinout;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 确认以卡片类型为主 不确认以该字段为准
        /// </summary>
        public YesOrNo IsEnterConfirm
		{
			set{ _isenterconfirm=value;}
			get{return _isenterconfirm;}
		}
        /// <summary>
        /// 打开车牌模糊匹配
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public YesOrNo OpenPlateBlurryMatch
		{
			set{ _openplateblurrymatch=value;}
			get{return _openplateblurrymatch;}
		}


        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
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
        #endregion Model

        #region WPF中使用
        private List<ParkDevice> _devices = null;
        public List<ParkDevice> Devices
        {
            get
            {
                return _devices;
            }
            set
            {
                _devices = value;
            }
        }

        /// <summary>
        /// 所在岗亭
        /// </summary>		
        private ParkBox _box;
        public ParkBox Box
        {
            get { return _box; }
            set
            {

                if (_box != value)
                {
                    _box = value;
                }
            }
        }
        #endregion

        #region
        /// <summary>
        /// 岗亭名称
        /// </summary>
        public string BoxName
        {
            get;
            set;
        }
        #endregion
    }
}

