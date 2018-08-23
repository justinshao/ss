using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Parking
{
    /// <summary>
    /// ParkBoxDetection:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class ParkBoxDetection
    {
        public ParkBoxDetection()
        { }
        #region Model
        private int _id;
        private string _recordid;
        private string _boxid;
        private string _pkid;
        private int? _connectionstate;
        private DateTime? _disconnecttime;
        private int? _datastatus;
        private DateTime? _lastupdatetime;
        private int? _haveupdate;
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
        public string RecordID
        {
            set { _recordid = value; }
            get { return _recordid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BoxID
        {
            set { _boxid = value; }
            get { return _boxid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PKID
        {
            set { _pkid = value; }
            get { return _pkid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ConnectionState
        {
            set { _connectionstate = value; }
            get { return _connectionstate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DisconnectTime
        {
            set { _disconnecttime = value; }
            get { return _disconnecttime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DataStatus
        {
            set { _datastatus = value; }
            get { return _datastatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastUpdateTime
        {
            set { _lastupdatetime = value; }
            get { return _lastupdatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? HaveUpdate
        {
            set { _haveupdate = value; }
            get { return _haveupdate; }
        }
        #endregion Model

    }
}
