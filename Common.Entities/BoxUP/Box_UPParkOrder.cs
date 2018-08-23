using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.BoxUP
{
    /// <summary>
    /// Box_UPParkOrder:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Box_UPParkOrder
    {
        public Box_UPParkOrder()
        { }
        #region Model
        private int _id;
        private string _recordid;
        private string _boxid;
        private string _dataID;
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
        public string DataID
        {
            set { _dataID = value; }
            get { return _dataID; }
        }
        #endregion Model

    }
}
