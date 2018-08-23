using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Parking
{
    /// <summary>
    /// ParkFeeRuleDetail:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class ParkFeeRuleDetail
    {
        public ParkFeeRuleDetail()
        { }
        #region Model
        private int _id;
        private string _ruledetailid;
        private string _ruleid;
        private string _starttime;
        private string _endtime;
        private bool _supplement;
        private LoopType _looptype;
        private decimal _limit;
        private int _freetime;
        private int _firsttime;
        private decimal _firstfee;
        private int _loop1pertime;
        private decimal _loop1perfee;
        private int _loop2start;
        private int _loop2pertime;
        private decimal _loop2perfee;
        private DateTime _lastupdatetime;
        private int _haveupdate;
        private DataStatus _datastatus;
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
        public string RuleDetailID
        {
            set { _ruledetailid = value; }
            get { return _ruledetailid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RuleID
        {
            set { _ruleid = value; }
            get { return _ruleid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string StartTime
        {
            set { _starttime = value; }
            get { return _starttime; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Supplement
        {
            set { _supplement = value; }
            get { return _supplement; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public LoopType LoopType
        {
            set { _looptype = value; }
            get { return _looptype; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Limit
        {
            set { _limit = value; }
            get { return _limit; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int FreeTime
        {
            set { _freetime = value; }
            get { return _freetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int FirstTime
        {
            set { _firsttime = value; }
            get { return _firsttime; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal FirstFee
        {
            set { _firstfee = value; }
            get { return _firstfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Loop1PerTime
        {
            set { _loop1pertime = value; }
            get { return _loop1pertime; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Loop1PerFee
        {
            set { _loop1perfee = value; }
            get { return _loop1perfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Loop2Start
        {
            set { _loop2start = value; }
            get { return _loop2start; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Loop2PerTime
        {
            set { _loop2pertime = value; }
            get { return _loop2pertime; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Loop2PerFee
        {
            set { _loop2perfee = value; }
            get { return _loop2perfee; }
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
        #endregion Model

    }
}
