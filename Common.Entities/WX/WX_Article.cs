using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Enum;
using Newtonsoft.Json;

namespace Common.Entities.WX
{
	/// <summary>
	/// WX_Article:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class WX_Article
	{
		public WX_Article()
		{}
		#region Model
		private int _id;
		private string _title;
		private string _imagepath;
		private string _description;
		private string _url;
		private string _text;
        private ArticleType _articletype;
		private int _sort;
		private string _groupid;
        private DataStatus _datastatus;
		private DateTime _createtime;
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
        /// 图文标题
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
        /// <summary>
        /// 图片地址
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ImagePath
		{
			set{ _imagepath=value;}
			get{return _imagepath;}
		}
        /// <summary>
        /// 描述信息
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description
		{
			set{ _description=value;}
			get{return _description;}
		}
        /// <summary>
        /// 图文链接地址
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Url
		{
			set{ _url=value;}
			get{return _url;}
		}
        /// <summary>
        /// 文本信息
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Text
		{
			set{ _text=value;}
			get{return _text;}
		}
        /// <summary>
        /// 图文类型 0-文字  1-链接
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ArticleType ArticleType
		{
			set{ _articletype=value;}
			get{return _articletype;}
		}
        /// <summary>
        /// 排序（由低到高）
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Sort
		{
			set{ _sort=value;}
			get{return _sort;}
		}
        /// <summary>
        /// 分组号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string GroupID
		{
			set{ _groupid=value;}
			get{return _groupid;}
		}
        /// <summary>
        /// 有效状态
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DataStatus DataStatus
		{
			set{ _datastatus=value;}
			get{return _datastatus;}
		}
        /// <summary>
        /// 添加时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CreateTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

