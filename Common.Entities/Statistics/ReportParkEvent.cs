using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Statistics
{
    public class ReportParkEvent
    {
        #region Model
        private int _id;
        private string _recordid;
        private string _cardno;
        private string _cardnum;
        private string _employeename;
        private string _platenumber;
        private PlateColor _platecolor;
        private string _cartypeid;
        private string _carmodelid;
        private DateTime _rectime;
        private string _gateid;
        private string _operatorid;
        private string _picturename;
        private int _eventid;
        private int _iostate;
        private string _iorecordid;
        private string _parkingid;
        private DateTime _lastupdatetime;
        private int _haveupdate;
        private int _datastatus;
        private string _remark;
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
        public string CardNo
        {
            set { _cardno = value; }
            get { return _cardno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CardNum
        {
            set { _cardnum = value; }
            get { return _cardnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EmployeeName
        {
            set { _employeename = value; }
            get { return _employeename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PlateNumber
        {
            set { _platenumber = value; }
            get { return _platenumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        public PlateColor PlateColor
        {
            set { _platecolor = value; }
            get { return _platecolor; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CarTypeID
        {
            set { _cartypeid = value; }
            get { return _cartypeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CarModelID
        {
            set { _carmodelid = value; }
            get { return _carmodelid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime RecTime
        {
            set { _rectime = value; }
            get { return _rectime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GateID
        {
            set { _gateid = value; }
            get { return _gateid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OperatorID
        {
            set { _operatorid = value; }
            get { return _operatorid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PictureName
        {
            set { _picturename = value; }
            get { return _picturename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int EventID
        {
            set { _eventid = value; }
            get { return _eventid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IOState
        {
            set { _iostate = value; }
            get { return _iostate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IORecordID
        {
            set { _iorecordid = value; }
            get { return _iorecordid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ParkingID
        {
            set { _parkingid = value; }
            get { return _parkingid; }
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
        public int DataStatus
        {
            set { _datastatus = value; }
            get { return _datastatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        #endregion Model

        #region 扩展字段
        /// <summary>
        /// 进场卡片类型名称
        /// </summary>
        public string CarTypeName
        {
            get;
            set;
        }
        /// <summary>
        /// 车型
        /// </summary>
        public string CarModelName
        {
            get;
            set;
        }
        /// <summary>
        /// 通道名称
        /// </summary>
        public string GateName
        {
            get;
            set;
        }
        /// <summary>
        /// 事件类型名称
        /// </summary>
        public string EventName
        {
            get;
            set;
        }
        /// <summary>
        /// 进出方向名称
        /// </summary>
        public string IOStateName
        {
            get;
            set;
        }
        /// <summary>
        /// 操作员
        /// </summary>
        public string Operator
        {
            get;
            set;
        }
        /// <summary>
        /// 车场名称
        /// </summary>
        public string ParkingName
        {
            get;
            set;
        }
        #endregion
    }
}
