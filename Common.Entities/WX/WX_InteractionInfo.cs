using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Enum;
using Newtonsoft.Json;

namespace Common.Entities.WX
{
	/// <summary>
	/// WX_InteractionInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class WX_InteractionInfo
	{
		public WX_InteractionInfo()
		{}
		#region Model
		private int _id;
		private string _replyid;
		private string _openid;
        private WxMsgType _msgtype;
		private string _interactioncontent;
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
        /// 交互编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ReplyID
		{
			set{ _replyid=value;}
			get{return _replyid;}
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
        /// 消息类型
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public WxMsgType MsgType
		{
			set{ _msgtype=value;}
			get{return _msgtype;}
		}
        /// <summary>
        /// 交互内容
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string InteractionContent
		{
			set{ _interactioncontent=value;}
			get{return _interactioncontent;}
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

