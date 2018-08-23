using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Statistics
{
    public class ReportParkOrder
    {
        #region Model
        private int _id;
        private string _recordid;
        private string _orderno;
        private string _tagid;
        private OrderType _ordertype;
        private OrderPayWay _payway;
        private decimal _amount;
        private decimal _unpayamount;
        private decimal _payamount;
        private decimal _discountamount;
        private string _carderateid;
        private int _status;
        private OrderSource _ordersource;
        private DateTime _ordertime;
        private DateTime _paytime;
        private DateTime _olduseruldate;
        private DateTime _newusefuldate;
        private decimal _oldmoney;
        private decimal _newmoney;
        private string _pKID;
        private string _userid;
        private string _onlineuserid;
        private string _onlineorderno;
        private string _remark;
        private DateTime _lastupdatetime;
        private int _haveupdate;
        private DataStatus _datastatus;
        private DateTime _cashTime;
        private decimal _cashMoney;
        private DateTime _oldUserBegin;
        private DateTime _newUserBegin;
        public string FeeRuleID { set; get; }
        private string _zZWeight;
        private string _netWeight;
        private string _tare;

        public string Tare
        {
            get { return _tare; }
            set { _tare = value; }
        }
        public string NetWeight
        {
            get { return _netWeight; }
            set { _netWeight = value; }
        }
        public string ZZWeight
        {
            get { return _zZWeight; }
            set { _zZWeight = value; }
        }
        public DateTime NewUserBegin
        {
            get { return _newUserBegin; }
            set { _newUserBegin = value; }
        }
        public DateTime OldUserBegin
        {
            get { return _oldUserBegin; }
            set { _oldUserBegin = value; }
        }
        public decimal CashMoney
        {
            get
            {
                return _cashMoney;
            }
            set
            {
                _cashMoney = value;
            }
        }

        public DateTime CashTime
        {
            get { return _cashTime; }
            set { _cashTime = value; }
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
        public string OrderNo
        {
            set { _orderno = value; }
            get { return _orderno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TagID
        {
            set { _tagid = value; }
            get { return _tagid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public OrderType OrderType
        {
            set { _ordertype = value; }
            get { return _ordertype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public OrderPayWay PayWay
        {
            set { _payway = value; }
            get { return _payway; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal Amount
        {
            set { _amount = value; }
            get { return _amount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal UnPayAmount
        {
            set { _unpayamount = value; }
            get { return _unpayamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal PayAmount
        {
            set { _payamount = value; }
            get { return _payamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal DiscountAmount
        {
            set { _discountamount = value; }
            get { return _discountamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CarderateID
        {
            set { _carderateid = value; }
            get { return _carderateid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public OrderSource OrderSource
        {
            set { _ordersource = value; }
            get { return _ordersource; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime OrderTime
        {
            set { _ordertime = value; }
            get { return _ordertime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime PayTime
        {
            set { _paytime = value; }
            get { return _paytime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime OldUserulDate
        {
            set { _olduseruldate = value; }
            get { return _olduseruldate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime NewUsefulDate
        {
            set { _newusefuldate = value; }
            get { return _newusefuldate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal OldMoney
        {
            set { _oldmoney = value; }
            get { return _oldmoney; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal NewMoney
        {
            set { _newmoney = value; }
            get { return _newmoney; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PKID
        {
            set { _pKID = value; }
            get { return _pKID; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OnlineUserID
        {
            set { _onlineuserid = value; }
            get { return _onlineuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OnlineOrderNo
        {
            set { _onlineorderno = value; }
            get { return _onlineorderno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
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



        #region 扩展字段
        /// <summary>
        /// 手机号码
        /// </summary>
        public string MobilePhone
        {
            get;
            set;
        }
        /// <summary>
        /// 车场名称
        /// </summary>
        public string PKName
        {
            get;
            set;
        }
        /// <summary>
        /// 车主
        /// </summary>
        public string EmployeeName
        {
            get;
            set;
        }
        /// <summary>
        /// 卡片编号
        /// </summary>
        public string CardNo
        {
            get;
            set;
        }
        /// <summary>
        /// 订单类型名称
        /// </summary>
        public string OrderTypeName
        {
            get;
            set;
        }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator
        {
            get;
            set;
        }
        /// <summary>
        /// 订单来源
        /// </summary>
        public string OrderSourceName
        {
            get;
            set;
        }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayWayName
        {
            get;
            set;
        }
        /// <summary>
        /// 车牌号码
        /// </summary>
        public string PlateNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 进场时间
        /// </summary>
        public DateTime EntranceTime
        {
            get;
            set;
        }
        /// <summary>
        /// 出场时间
        /// </summary>
        public DateTime ExitTime
        {
            get;
            set;
        }
        /// <summary>
        /// 停车时长
        /// </summary>
        public string LongTime
        {
            get;
            set;
        }

       

        #endregion
    }
}
