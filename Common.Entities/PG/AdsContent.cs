using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Common.Entities.PG
{
    /// <summary>
    /// AdsContent:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class AdsContent: INotifyPropertyChanged
    {
        public AdsContent()
        { }

        #region 属性变更事件
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        #endregion
        #region Model
        private int _id;
        private string _recordid;
        private string _pkid;
        private string _adsname;
        private string _adspath;
        private int _adstype;
        private int _adsstate;
        private DateTime _lastupdatetime;
        private int _haveupdate;
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ID
        {
            set { _id = value; Notify("ID"); }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RecordID
        {
            set { _recordid = value; Notify("RecordID"); }
            get { return _recordid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKID
        {
            set { _pkid = value; Notify("PKID"); }
            get { return _pkid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AdsName
        {
            set { _adsname = value; Notify("AdsName"); }
            get { return _adsname; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AdsPath
        {
            set { _adspath = value; Notify("AdsPath"); }
            get { return _adspath; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int AdsType
        {
            set { _adstype = value; Notify("AdsType"); }
            get { return _adstype; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int AdsState
        {
            set { _adsstate = value; Notify("AdsState"); }
            get { return _adsstate; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastUpdateTime
        {
            set { _lastupdatetime = value; Notify("LastUpdateTime"); }
            get { return _lastupdatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int HaveUpdate
        {
            set { _haveupdate = value; Notify("HaveUpdate"); }
            get { return _haveupdate; }
        }
        #endregion Model

    }
}
