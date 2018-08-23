using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities
{
    /// <summary>
    /// ParkCarBitGroup:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class ParkCarBitGroup
    {
        public ParkCarBitGroup()
        { }
        #region Model
        private int _id;
        private string _recordid;
        private string _carbitname;
        private int _carbitnum = 1;
        private DateTime _lastupdatetime;
        private int _haveupdate;
        private DataStatus _datastatus;
        private string _pKID;

        public string PKID
        {
            get { return _pKID; }
            set { _pKID = value; }
        }
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
        public string CarBitName
        {
            set { _carbitname = value; }
            get { return _carbitname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CarBitNum
        {
            set { _carbitnum = value; }
            get { return _carbitnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LastUpdateTime
        {
            set { _lastupdatetime = value; }
            get { return _lastupdatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int HaveUpdate
        {
            set { _haveupdate = value; }
            get { return _haveupdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DataStatus DataStatus
        {
            set { _datastatus = value; }
            get { return _datastatus; }
        }
        #endregion Model

    }
}
