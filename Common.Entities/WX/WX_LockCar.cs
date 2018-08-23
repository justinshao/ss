using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WX
{
    /// <summary>
    /// WX_LockCar:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class WX_LockCar
    {
        public WX_LockCar()
        { }
        #region Model
        private int _id;
        private string _recordid;
        private string _accountid;
        private string _pkid;
        private string _platenumber;
        private int _status;
        private DateTime _lockdate;
        private DateTime _unlockdate;
        private DateTime _lastupdatetime;
        private int _haveupdate;
        private int _datastatus;
        private string _vID;
        private string _pKName;
        private string _entranceTime;
        private string _companyid;

        /// <summary>
        /// 单位编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CompanyID
        {
            set { _companyid = value; }
            get { return _companyid; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EntranceTime
        {
            get { return _entranceTime; }
            set { _entranceTime = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKName
        {
            get { return _pKName; }
            set { _pKName = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string VID
        {
            get { return _vID; }
            set { _vID = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RecordID
        {
            set { _recordid = value; }
            get { return _recordid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AccountID
        {
            set { _accountid = value; }
            get { return _accountid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKID
        {
            set { _pkid = value; }
            get { return _pkid; }
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
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LockDate
        {
            set { _lockdate = value; }
            get { return _lockdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime UnLockDate
        {
            set { _unlockdate = value; }
            get { return _unlockdate; }
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
        public int DataStatus
        {
            set { _datastatus = value; }
            get { return _datastatus; }
        }
        #endregion Model

    }
}
