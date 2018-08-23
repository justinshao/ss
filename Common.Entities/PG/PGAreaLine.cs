using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Common.Entities.PG
{
    /// <summary>
	/// PGAreaLine:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
    public partial class PGAreaLine: INotifyPropertyChanged
    {
        public PGAreaLine()
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
        private string _lineid;
        private string _areaid;
        private string _pointstart;
        private int _pointstartid;
        private decimal _pointstartx;
        private decimal _pointstarty;
        private string _pointend;
        private int _pointendid;
        private decimal _pointendx;
        private decimal _pointendy;
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
        public string LineID
        {
            set { _lineid = value; Notify("LineID"); }
            get { return _lineid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AreaID
        {
            set { _areaid = value; Notify("AreaID"); }
            get { return _areaid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PointStart
        {
            set { _pointstart = value; Notify("PointStart"); }
            get { return _pointstart; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PointStartID
        {
            set { _pointstartid = value; Notify("PointStartID"); }
            get { return _pointstartid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal PointStartX
        {
            set { _pointstartx = value; Notify("PointStartX"); }
            get { return _pointstartx; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal PointStartY
        {
            set { _pointstarty = value; Notify("PointStartY"); }
            get { return _pointstarty; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PointEnd
        {
            set { _pointend = value; Notify("PointEnd"); }
            get { return _pointend; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PointEndID
        {
            set { _pointendid = value; Notify("PointEndID"); }
            get { return _pointendid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal PointEndX
        {
            set { _pointendx = value; Notify("PointEndX"); }
            get { return _pointendx; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal PointEndY
        {
            set { _pointendy = value; Notify("PointEndY"); }
            get { return _pointendy; }
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
