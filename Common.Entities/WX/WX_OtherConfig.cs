using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Enum;
using Newtonsoft.Json;

namespace Common.Entities.WX
{
	/// <summary>
	/// WX_OtherConfig:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class WX_OtherConfig
	{
		public WX_OtherConfig()
		{}
		#region Model
		private int _id;
        private ConfigType _configtype;
		private string _configvalue;
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
        /// 配置类型（对应程序枚举值）
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ConfigType ConfigType
		{
			set{ _configtype=value;}
			get{return _configtype;}
		}
        /// <summary>
        /// 配置值
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ConfigValue
		{
			set{ _configvalue=value;}
			get{return _configvalue;}
		}
        /// <summary>
        /// 描述信息（仅用于界面显示）
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        /// <summary>
        /// 格式信息（仅用于界面显示）
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DefaultDescription { get; set; }
		#endregion Model

	}
}

