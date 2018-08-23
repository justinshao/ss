using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Common.Entities.WX
{
	/// <summary>
	/// WX_MenuAccessRecord:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class WX_MenuAccessRecord
	{
		public WX_MenuAccessRecord()
		{}
		#region Model
		private int _id;
		private string _menuname;
		private string _openid;
		private DateTime _accesstime;
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
        /// 菜单名称
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MenuName
		{
            set { _menuname = value; }
            get { return _menuname; }
		}
        /// <summary>
        /// 用户编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OpenID
		{
			set{ _openid=value;}
			get{return _openid;}
		}
        /// <summary>
        /// 访问时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime AccessTime
		{
			set{ _accesstime=value;}
			get{return _accesstime;}
		}
		#endregion Model

	}
}

