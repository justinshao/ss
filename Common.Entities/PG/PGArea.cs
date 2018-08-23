using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Common.Entities.PG
{
    /// <summary>
    /// PGArea:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class PGArea: INotifyPropertyChanged
    {
        public PGArea()
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
        private string _areaid;
        private string _areaname;
        private string _pkid;
        private string _areaimgpath;
        private DateTime _lastupdatetime;
        private int _haveupdate;
        private byte[] _areaimgdata;
        private DataStatus _datastatus;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DataStatus Datastatus
        {
            get { return _datastatus; }
            set { _datastatus = value; }
        }
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
        public string AreaID
        {
            set { _areaid = value; Notify("AreaID"); }
            get { return _areaid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AreaName
        {
            set { _areaname = value; Notify("AreaName"); }
            get { return _areaname; }
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
        public string AreaImgPath
        {
            set { _areaimgpath = value; Notify("AreaImgPath"); }
            get { return _areaimgpath; }
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

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public byte[] Areaimgdata
        {
            get
            {
                return _areaimgdata;
            }

            set
            {
                _areaimgdata = value;
            }
        }
        #endregion Model

    }
}
