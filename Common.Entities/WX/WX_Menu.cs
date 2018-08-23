using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Enum;
using Newtonsoft.Json;

namespace Common.Entities.WX
{
	/// <summary>
	/// WX_Meun:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class WX_Menu
	{
		public WX_Menu()
		{}
		#region Model
		private int _id;
		private string _menuname;
		private string _url;
		private int _keywordid;
        private MenuType _menutype;
		private int _sort;
		private int _masterid;
        private string _companyid;
		private int _datastatus;
		private DateTime _createtime;
        private string _miniprogramappid;
        private string _miniprogrampagepath;
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
        /// 菜单名称
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MenuName
		{
			set{ _menuname=value;}
			get{return _menuname;}
		}
        /// <summary>
        /// 菜单链接地址
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Url
		{
			set{ _url=value;}
			get{return _url;}
		}
        /// <summary>
        /// 关键字编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int KeywordId
		{
			set{ _keywordid=value;}
			get{return _keywordid;}
		}
        /// <summary>
        /// 菜单类型 0-跳转链接，1-匹配关键字
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public MenuType MenuType
		{
			set{ _menutype=value;}
			get{return _menutype;}
		}
        /// <summary>
        /// 排序（低到高）
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Sort
		{
			set{ _sort=value;}
			get{return _sort;}
		}
        /// <summary>
        /// 父级编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int MasterID
		{
			set{ _masterid=value;}
			get{return _masterid;}
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
        /// 有效状态
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int DataStatus
		{
			set{ _datastatus=value;}
			get{return _datastatus;}
		}
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CreateTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
        /// <summary>
        /// 小程序APPID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MinIprogramAppId {
            set { _miniprogramappid = value; }
            get { return _miniprogramappid; }
        }
        /// <summary>
        /// 小程序页面地址
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MinIprogramPagePath {
            set { _miniprogrampagepath = value; }
            get { return _miniprogrampagepath; }
        }
		#endregion Model

	}
}

