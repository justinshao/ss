using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Parking
{
    /// <summary>
    /// ParkFeeRule:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class ParkFeeRule
    {
        public ParkFeeRule()
        { }
        #region Model
        private int _id;
        private string _feeruleid;
        private string _rulename;
        private FeeType _feetype;
        private string _cartypeid;
        private string _carmodelid;
        private string _areaid;
        private DateTime _lastupdatetime;
        private int _haveupdate;
        private DataStatus _datastatus;
        private string _ruletext;
        private bool _isOffline;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsOffline
        {
            get { return _isOffline; }
            set { _isOffline = value; }
        } 
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
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FeeRuleID
        {
            set { _feeruleid = value; }
            get { return _feeruleid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RuleName
        {
            set { _rulename = value; }
            get { return _rulename; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FeeType FeeType
        {
            set { _feetype = value; }
            get { return _feetype; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarTypeID
        {
            set { _cartypeid = value; }
            get { return _cartypeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarModelID
        {
            set { _carmodelid = value; }
            get { return _carmodelid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AreaID
        {
            set { _areaid = value; }
            get { return _areaid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastUpdateTime
        {
            set { _lastupdatetime = value; }
            get { return _lastupdatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int HaveUpdate
        {
            set { _haveupdate = value; }
            get { return _haveupdate; }
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
        public string RuleText
        {
            set { _ruletext = value; }
            get { return _ruletext; }
        }

        #endregion Model
        public List<ParkFeeRuleDetail> ParkFeeRuleDetails { get; set; }
    }
}
