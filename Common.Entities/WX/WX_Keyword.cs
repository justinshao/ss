using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Enum;
using Newtonsoft.Json;

namespace Common.Entities.WX
{
    /// <summary>
    /// WX_Keyword:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class WX_Keyword
    {
        public WX_Keyword()
        { }
        #region Model
        private int _id;
        private string _keyword;
        private KeywordType _keywordtype;
        private MatchType _matchtype;
        private ReplyType _replytype;
        private string _text;
        private string _articlegroupid;
        private string _companyid;
        private DataStatus _datastatus;
        private DateTime _createtime;
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 关键字
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Keyword
        {
            set { _keyword = value; }
            get { return _keyword; }
        }
        /// <summary>
        /// 关键字类型 0-回复文字 1-回复图文
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public KeywordType KeywordType
        {
            set { _keywordtype = value; }
            get { return _keywordtype; }
        }
        /// <summary>
        /// 匹配类型 0-完全匹配 1-模糊匹配
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public MatchType MatchType
        {
            set { _matchtype = value; }
            get { return _matchtype; }
        }
        /// <summary>
        /// 回复类型 0-关注回复，1-自动回复，2-菜单，3-浏览，4-位置，255-默认回复
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ReplyType ReplyType
        {
            set { _replytype = value; }
            get { return _replytype; }
        }
        /// <summary>
        /// 文本信息
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Text
        {
            set { _text = value; }
            get { return _text; }
        }
        /// <summary>
        /// 图文分组编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ArticleGroupID
        {
            set { _articlegroupid = value; }
            get { return _articlegroupid; }
        }
        /// <summary>
        /// 单位编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CompanyID
        {
            set { _companyid = value; }
            get { return _companyid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DataStatus DataStatus
        {
            set { _datastatus = value; }
            get { return _datastatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        #endregion Model

    }
}
