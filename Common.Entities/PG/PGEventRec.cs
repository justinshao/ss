using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Common.Entities.PG
{
    /// <summary>
    /// PGEventRec:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class PGEventRec: INotifyPropertyChanged
    {
        public PGEventRec()
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
        private string _pointid;
        private DateTime _eventdate;
        private int _eventid;
        private string _eventname;
        private DateTime _eventenddate;
        private int _timecount;
        private string _platenumber;
        private string _imgpath;
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
        public string PointID
        {
            set { _pointid = value; Notify("PointID"); }
            get { return _pointid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EventDate
        {
            set { _eventdate = value; Notify("EventDate"); }
            get { return _eventdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int EventID
        {
            set { _eventid = value; Notify("EventID"); }
            get { return _eventid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EventName
        {
            set { _eventname = value; Notify("EventName"); }
            get { return _eventname; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EventEndDate
        {
            set { _eventenddate = value; Notify("EventEndDate"); }
            get { return _eventenddate; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int TimeCount
        {
            set { _timecount = value; Notify("TimeCount"); }
            get { return _timecount; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PlateNumber
        {
            set { _platenumber = value; Notify("PlateNumber"); }
            get { return _platenumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ImgPath
        {
            set { _imgpath = value; Notify("ImgPath"); }
            get { return _imgpath; }
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
