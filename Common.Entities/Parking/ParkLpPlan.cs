using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Parking
{
    /// <summary>
    /// ParkLpPlan:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class ParkLpPlan
    {
        public ParkLpPlan()
        { }
        #region Model
        private int _id;
        private string _planid;
        private string _platenumber;
        private DateTime _planouttime;
        private DateTime _planrtntime;
        private DateTime _factouttime;
        private DateTime _factrtntime;
        private string _empname;
        private string _reason;
        private string _remark;
        private int _planstate;
        private string _zb_user;
        private DateTime _zb_time;
        private string _sh_user;
        private DateTime _sh_time;
        private int _sh_state;
        private DataStatus _datastatus;
        private DateTime _lastupdatetime;
        private int _haveupdate;
        private string _pKID;
        private string _userName;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKID
        {
            get { return _pKID; }
            set { _pKID = value; }
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
        public string PlanID
        {
            set { _planid = value; }
            get { return _planid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PlateNumber
        {
            set { _platenumber = value; }
            get { return _platenumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime PlanOutTime
        {
            set { _planouttime = value; }
            get { return _planouttime; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime PlanRtnTime
        {
            set { _planrtntime = value; }
            get { return _planrtntime; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime FactOutTime
        {
            set { _factouttime = value; }
            get { return _factouttime; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime FactRtnTime
        {
            set { _factrtntime = value; }
            get { return _factrtntime; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EmpName
        {
            set { _empname = value; }
            get { return _empname; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Reason
        {
            set { _reason = value; }
            get { return _reason; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PlanState
        {
            set { _planstate = value; }
            get { return _planstate; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ZB_User
        {
            set { _zb_user = value; }
            get { return _zb_user; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ZB_Time
        {
            set { _zb_time = value; }
            get { return _zb_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SH_User
        {
            set { _sh_user = value; }
            get { return _sh_user; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime SH_Time
        {
            set { _sh_time = value; }
            get { return _sh_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SH_State
        {
            set { _sh_state = value; }
            get { return _sh_state; }
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
        #endregion Model

    }
}
