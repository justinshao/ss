using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Common.Entities.PG
{
    /// <summary>
    /// PGAreaPoint:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class PGAreaPoint: INotifyPropertyChanged
    {
        public PGAreaPoint()
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
        private string _areaid;
        private string _pointname;
        private decimal _pointx;
        private decimal _pointy;
        private decimal _angle;
        private int _pointtype;
        private string _deviceid;
        private int _channelno;
        private string _inareaids;
        private int _pointstate;
        private string _platenumber;
        private string _imgpath;
        private DateTime _indatetime;
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
        public string AreaID
        {
            set { _areaid = value; Notify("AreaID"); }
            get { return _areaid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PointName
        {
            set { _pointname = value; Notify("PointName"); }
            get { return _pointname; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal PointX
        {
            set { _pointx = value; Notify("PointX"); }
            get { return _pointx; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal PointY
        {
            set { _pointy = value; Notify("PointY"); }
            get { return _pointy; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Angle
        {
            set { _angle = value; Notify("Angle"); }
            get { return _angle; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PointType
        {
            set { _pointtype = value; Notify("PointType"); }
            get { return _pointtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DeviceID
        {
            set { _deviceid = value; Notify("DeviceID"); }
            get { return _deviceid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ChannelNo
        {
            set { _channelno = value; Notify("ChannelNo"); }
            get { return _channelno; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string InAreaIDs
        {
            set { _inareaids = value; Notify("InAreaIDs"); }
            get { return _inareaids; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PointState
        {
            set { _pointstate = value; Notify("PointState"); }
            get { return _pointstate; }
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
        public DateTime InDatetime
        {
            set { _indatetime = value; Notify("InDatetime"); }
            get { return _indatetime; }
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
