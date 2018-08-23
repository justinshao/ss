using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Entities;
using Common.Entities.Parking;
using Common.IRepository.Park;
using Common.IRepository.Statistics;
using Common.Factory.Statistics;
using Common.Factory.Park;
using Common.Entities.Statistics;
using Common.IRepository;
using Common.Factory;

using Common.DataAccess;
using Common.Services;
namespace Common.Services.Statistics
{
    public class OnDutyGatherServices
    {
        /// <summary>
        /// 当班统计
        /// </summary>
        public void Statistics_OnDuty()
        {
            try
            {
                IParking iparking = ParkingFactory.GetFactory();
                IParkIORecord iorecordfactory = ParkIORecordFactory.GetFactory();
                IStatistics_Gather gatherfactory = Statistics_GatherFactory.GetFactory();
                IParkChangeshiftrecord ichangeshift = ParkChangeshiftrecordFactory.GetFactory();
                IParkOrder iorder = ParkOrderFactory.GetFactory();
                IParkBox iparkbox = ParkBoxFactory.GetFactory();
                IStatistics_ChangeShift istatisticschangeshift = Statistics_ChangeShiftFactory.GetFactory();
                List<BaseParkinfo> parks = iparking.QueryParkingAll();
                if (parks == null || parks.Count == 0)
                    return;
                DateTime dtNow = DateTime.Now;
                foreach (BaseParkinfo parking in parks)
                {
                    if (parking.IsOnLineGathe == YesOrNo.Yes)
                    {
                        continue;
                    }
                    List<ParkBox> boxlist = iparkbox.QueryByParkingID(parking.PKID);
                    if (boxlist != null && boxlist.Count > 0)
                    {
                        foreach (ParkBox box in boxlist)
                        {
                            List<ParkChangeshiftrecord> ondutys = ichangeshift.GetChangeShiftRecord(box.BoxID);
                            if (ondutys == null || ondutys.Count == 0)
                                continue;
                            foreach (ParkChangeshiftrecord onduty in ondutys)
                            {
                                Statistics_ChangeShift changeshift = new Statistics_ChangeShift
                                {
                                    BoxID = onduty.BoxID,
                                    ParkingName = parking.PKName,
                                    StartWorkTime = onduty.StartWorkTime,
                                    EndWorkTime = onduty.EndWorkTime,
                                    ParkingID = parking.PKID,
                                    AdminID = onduty.UserID,
                                    ChangeShiftID = onduty.RecordID
                                };
                                DateTime starttime = onduty.StartWorkTime;
                                DateTime endtime = DateTime.Now;
                                if (onduty.EndWorkTime > DateTime.MinValue)
                                {
                                    endtime = onduty.EndWorkTime;
                                }
                                //统计相关信息
                                //查询进场数
                                changeshift.Entrance_Count = iorecordfactory.EntranceCountByBox(box.BoxID, starttime, endtime);
                                //查询出场数
                                changeshift.Exit_Count = iorecordfactory.ExitCountByBox(box.BoxID, starttime, endtime);
                                #region 进场卡片类型
                                List<KeyValue> _InCardType = iorecordfactory.GetInCardTypeByBoxID(box.BoxID, starttime, endtime);
                                if (_InCardType != null && _InCardType.Count > 0)
                                {
                                    foreach (var k in _InCardType)
                                    {
                                        switch (k.KeyName)
                                        {
                                            case "0":
                                                changeshift.VIPCard = k.Key_Value;
                                                break;
                                            case "1":
                                                changeshift.StordCard = k.Key_Value;
                                                break;
                                            case "2":
                                                changeshift.MonthCard = k.Key_Value;
                                                break;
                                            case "3":
                                                changeshift.TempCard = k.Key_Value;
                                                break;
                                            case "4":
                                                changeshift.JobCard = k.Key_Value;
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                                #endregion

                                #region 统计放行类型
                                List<KeyValue> _ReleaseType = iorecordfactory.GetReleaseTypeByBox(box.BoxID, starttime, endtime);
                                if (_ReleaseType != null && _ReleaseType.Count > 0)
                                {
                                    foreach (KeyValue k in _ReleaseType)
                                    {
                                        switch (k.KeyName)
                                        {
                                            case "0":
                                                changeshift.ReleaseType_Normal = k.Key_Value;
                                                break;
                                            case "1":
                                                changeshift.ReleaseType_Charge = k.Key_Value;
                                                break;
                                            case "2":
                                                changeshift.ReleaseType_Free = k.Key_Value;
                                                break;
                                            case "3":
                                                changeshift.ReleaseType_Catch = k.Key_Value;
                                                break;
                                        }
                                    }
                                }
                                #endregion

                                #region 统计费用相关
                                List<ParkOrder> _orderfee = iorder.GetOrdersByBoxID(parking.PKID, onduty.BoxID, starttime, endtime);
                                if (_orderfee != null && _orderfee.Count > 0)
                                {
                                    foreach (ParkOrder o in _orderfee)
                                    {
                                        changeshift.Receivable_Amount += o.Amount;
                                        changeshift.Diff_Amount += o.UnPayAmount;
                                        changeshift.Real_Amount = (changeshift.Receivable_Amount - changeshift.Diff_Amount);
                                        //支付方式(1.现金、2.微信、3.支付宝、4.网银、5.电子钱包、6.优免卷7.余额
                                        switch (o.PayWay)
                                        {
                                            case OrderPayWay.Cash:
                                                changeshift.Cash_Amount += o.PayAmount;
                                                changeshift.Cash_Count++;

                                                if (o.DiscountAmount > 0)
                                                {
                                                    changeshift.CashDiscount_Amount += o.DiscountAmount;
                                                    changeshift.CashDiscount_Count++;

                                                    changeshift.Discount_Amount += o.DiscountAmount;
                                                    changeshift.Discount_Count++;
                                                }
                                                break;
                                            case OrderPayWay.WeiXin:
                                            case OrderPayWay.Alipay:
                                            case OrderPayWay.OnlineBanking:
                                            case OrderPayWay.Wallet:
                                                changeshift.OnLine_Amount += o.PayAmount;
                                                changeshift.OnLine_Count++;

                                                if (o.DiscountAmount > 0)
                                                {
                                                    changeshift.OnLineDiscount_Amount += o.DiscountAmount;
                                                    changeshift.OnLineDiscount_Count++;

                                                    changeshift.Discount_Amount += o.DiscountAmount;
                                                    changeshift.Discount_Count++;
                                                }
                                                break;
                                            case OrderPayWay.PreferentialTicket:
                                                changeshift.Discount_Amount += o.DiscountAmount;
                                                changeshift.Discount_Count++;
                                                break;
                                            case OrderPayWay.ValueCard:
                                                changeshift.StordCard_Amount += o.PayAmount;
                                                changeshift.StordCard_Count++;
                                                break;
                                        }
                                        switch (o.OrderType)
                                        {
                                            //临时卡缴费
                                            case OrderType.TempCardPayment:
                                            case OrderType.AreaTempCardPayment:
                                                switch (o.PayWay)
                                                {
                                                    case OrderPayWay.WeiXin:
                                                    case OrderPayWay.Alipay:
                                                    case OrderPayWay.OnlineBanking:
                                                    case OrderPayWay.Wallet:
                                                        changeshift.OnLineTemp_Amount += o.PayAmount;
                                                        changeshift.OnLineTemp_Count++;
                                                        break;
                                                    default:
                                                        changeshift.Temp_Amount += o.PayAmount;
                                                        changeshift.Temp_Count++;
                                                        break;
                                                }
                                                break;
                                            //月卡续期
                                            case OrderType.MonthCardPayment:
                                                switch (o.PayWay)
                                                {
                                                    case OrderPayWay.WeiXin:
                                                    case OrderPayWay.Alipay:
                                                    case OrderPayWay.OnlineBanking:
                                                    case OrderPayWay.Wallet:
                                                        changeshift.OnLineMonthCardExtend_Amount += o.PayAmount;
                                                        changeshift.OnLineMonthCardExtend_Count++;
                                                        break;
                                                    default:
                                                        changeshift.MonthCardExtend_Amount += o.PayAmount;
                                                        changeshift.MonthCardExtend_Count++;
                                                        break;
                                                }
                                                break;
                                            //VIP卡续期
                                            case OrderType.VIPCardRenewal:
                                                changeshift.VIPExtend_Count++;
                                                break;
                                            //储值卡充值
                                            case OrderType.ValueCardRecharge:
                                                switch (o.PayWay)
                                                {
                                                    case OrderPayWay.WeiXin:
                                                    case OrderPayWay.Alipay:
                                                    case OrderPayWay.OnlineBanking:
                                                    case OrderPayWay.Wallet:
                                                        changeshift.OnLineStordCard_Amount += o.PayAmount;
                                                        changeshift.OnLineStordCard_Count++;
                                                        break;
                                                    default:
                                                        changeshift.StordCardRecharge_Amount +=  o.PayAmount;
                                                        changeshift.StordCardRecharge_Count++;
                                                        break;
                                                }
                                                break;
                                        }
                                    }
                                }
                                #endregion
                                using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
                                {
                                    try
                                    {
                                        dboperator.BeginTransaction();
                                        istatisticschangeshift.Delete(onduty.RecordID, dboperator);
                                        istatisticschangeshift.Insert(changeshift, dboperator);
                                        dboperator.CommitTransaction();
                                    }
                                    catch (Exception ex)
                                    {
                                        TxtLogServices.WriteTxtLog("添加当班信息异常 异常信息:{0}", ex.Message);
                                        dboperator.RollbackTransaction();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLog("当班统计异常 异常信息:{0}", ex.Message);
            }
        }

        /// <summary>
        /// 当班统计
        /// </summary>
        public void Statistics_OnDuty(string ParkingID)
        {
            try
            {
                IParking iparking = ParkingFactory.GetFactory();
                IParkIORecord iorecordfactory = ParkIORecordFactory.GetFactory();
                IStatistics_Gather gatherfactory = Statistics_GatherFactory.GetFactory();
                IParkChangeshiftrecord ichangeshift = ParkChangeshiftrecordFactory.GetFactory();
                IParkOrder iorder = ParkOrderFactory.GetFactory();
                IParkBox iparkbox = ParkBoxFactory.GetFactory();
                IStatistics_ChangeShift istatisticschangeshift = Statistics_ChangeShiftFactory.GetFactory();
                BaseParkinfo parking = iparking.QueryParkingByParkingID(ParkingID);
                List<ParkBox> boxlist = iparkbox.QueryByParkingID(parking.PKID);
                if (boxlist != null && boxlist.Count > 0)
                {
                    foreach (ParkBox box in boxlist)
                    {
                        List<ParkChangeshiftrecord> ondutys = ichangeshift.GetChangeShiftRecord(box.BoxID);
                        if (ondutys == null || ondutys.Count == 0)
                            continue;
                        foreach (ParkChangeshiftrecord onduty in ondutys)
                        {
                            Statistics_ChangeShift changeshift = new Statistics_ChangeShift
                            {
                                BoxID = onduty.BoxID,
                                ParkingName = parking.PKName,
                                StartWorkTime = onduty.StartWorkTime,
                                EndWorkTime = onduty.EndWorkTime,
                                ParkingID = parking.PKID,
                                AdminID = onduty.UserID,
                                ChangeShiftID = onduty.RecordID
                            };
                            DateTime starttime = onduty.StartWorkTime;
                            DateTime endtime = DateTime.Now;
                            if (onduty.EndWorkTime > DateTime.MinValue)
                            {
                                endtime = onduty.EndWorkTime;
                            }
                            //统计相关信息
                            //查询进场数
                            changeshift.Entrance_Count = iorecordfactory.EntranceCountByBox(box.BoxID, starttime, endtime);
                            //查询出场数
                            changeshift.Exit_Count = iorecordfactory.ExitCountByBox(box.BoxID, starttime, endtime);
                            #region 进场卡片类型
                            List<KeyValue> _InCardType = iorecordfactory.GetInCardTypeByBoxID(box.BoxID, starttime, endtime);
                            if (_InCardType != null && _InCardType.Count > 0)
                            {
                                foreach (var k in _InCardType)
                                {
                                    switch (k.KeyName)
                                    {
                                        case "0":
                                            changeshift.VIPCard = k.Key_Value;
                                            break;
                                        case "1":
                                            changeshift.StordCard = k.Key_Value;
                                            break;
                                        case "2":
                                            changeshift.MonthCard = k.Key_Value;
                                            break;
                                        case "3":
                                            changeshift.TempCard = k.Key_Value;
                                            break;
                                        case "4":
                                            changeshift.JobCard = k.Key_Value;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            #endregion

                            #region 统计放行类型
                            List<KeyValue> _ReleaseType = iorecordfactory.GetReleaseTypeByBox(box.BoxID, starttime, endtime);
                            if (_ReleaseType != null && _ReleaseType.Count > 0)
                            {
                                foreach (KeyValue k in _ReleaseType)
                                {
                                    switch (k.KeyName)
                                    {
                                        case "0":
                                            changeshift.ReleaseType_Normal = k.Key_Value;
                                            break;
                                        case "1":
                                            changeshift.ReleaseType_Charge = k.Key_Value;
                                            break;
                                        case "2":
                                            changeshift.ReleaseType_Free = k.Key_Value;
                                            break;
                                        case "3":
                                            changeshift.ReleaseType_Catch = k.Key_Value;
                                            break;
                                    }
                                }
                            }
                            #endregion

                            #region 统计费用相关
                            List<ParkOrder> _orderfee = iorder.GetOrdersByBoxID(parking.PKID, onduty.BoxID, starttime, endtime);
                            if (_orderfee != null && _orderfee.Count > 0)
                            {
                                foreach (ParkOrder o in _orderfee)
                                {
                                    changeshift.Receivable_Amount += o.Amount;
                                    changeshift.Diff_Amount += o.UnPayAmount;
                                    changeshift.Real_Amount = (changeshift.Receivable_Amount - changeshift.Diff_Amount);
                                    //支付方式(1.现金、2.微信、3.支付宝、4.网银、5.电子钱包、6.优免卷7.余额
                                    switch (o.PayWay)
                                    {
                                        case OrderPayWay.Cash:
                                            changeshift.Cash_Amount += o.PayAmount;
                                            changeshift.Cash_Count++;

                                            if (o.DiscountAmount > 0)
                                            {
                                                changeshift.CashDiscount_Amount += o.DiscountAmount;
                                                changeshift.CashDiscount_Count++;

                                                changeshift.Discount_Amount += o.DiscountAmount;
                                                changeshift.Discount_Count++;
                                            }
                                            break;
                                        case OrderPayWay.WeiXin:
                                        case OrderPayWay.Alipay:
                                        case OrderPayWay.OnlineBanking:
                                        case OrderPayWay.Wallet:
                                            changeshift.OnLine_Amount += o.PayAmount;
                                            changeshift.OnLine_Count++;

                                            if (o.DiscountAmount > 0)
                                            {
                                                changeshift.OnLineDiscount_Amount += o.DiscountAmount;
                                                changeshift.OnLineDiscount_Count++;

                                                changeshift.Discount_Amount += o.DiscountAmount;
                                                changeshift.Discount_Count++;
                                            }
                                            break;
                                        case OrderPayWay.PreferentialTicket:
                                            changeshift.Discount_Amount += o.DiscountAmount;
                                            changeshift.Discount_Count++;
                                            break;
                                        case OrderPayWay.ValueCard:
                                            changeshift.StordCard_Amount += o.PayAmount;
                                            changeshift.StordCard_Count++;
                                            break;
                                    }
                                    switch (o.OrderType)
                                    {
                                        //临时卡缴费
                                        case OrderType.TempCardPayment:
                                        case OrderType.AreaTempCardPayment:
                                            switch (o.PayWay)
                                            {
                                                case OrderPayWay.WeiXin:
                                                case OrderPayWay.Alipay:
                                                case OrderPayWay.OnlineBanking:
                                                case OrderPayWay.Wallet:
                                                    changeshift.OnLineTemp_Amount += o.PayAmount;
                                                    changeshift.OnLineTemp_Count++;
                                                    break;
                                                default:
                                                    changeshift.Temp_Amount += o.PayAmount;
                                                    changeshift.Temp_Count++;
                                                    break;
                                            }
                                            break;
                                        //月卡续期
                                        case OrderType.MonthCardPayment:
                                            switch (o.PayWay)
                                            {
                                                case OrderPayWay.WeiXin:
                                                case OrderPayWay.Alipay:
                                                case OrderPayWay.OnlineBanking:
                                                case OrderPayWay.Wallet:
                                                    changeshift.OnLineMonthCardExtend_Amount += o.PayAmount;
                                                    changeshift.OnLineMonthCardExtend_Count++;
                                                    break;
                                                default:
                                                    changeshift.MonthCardExtend_Amount += o.PayAmount;
                                                    changeshift.MonthCardExtend_Count++;
                                                    break;
                                            }
                                            break;
                                        //VIP卡续期
                                        case OrderType.VIPCardRenewal:
                                            changeshift.VIPExtend_Count++;
                                            break;
                                        //储值卡充值
                                        case OrderType.ValueCardRecharge:
                                            switch (o.PayWay)
                                            {
                                                case OrderPayWay.WeiXin:
                                                case OrderPayWay.Alipay:
                                                case OrderPayWay.OnlineBanking:
                                                case OrderPayWay.Wallet:
                                                    changeshift.OnLineStordCard_Amount += o.PayAmount;
                                                    changeshift.OnLineStordCard_Count++;
                                                    break;
                                                default:
                                                    changeshift.StordCardRecharge_Amount += o.PayAmount;
                                                    changeshift.StordCardRecharge_Count++;
                                                    break;
                                            }
                                            break;
                                    }
                                }
                            }
                            #endregion
                            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
                            {
                                try
                                {
                                    dboperator.BeginTransaction();
                                    istatisticschangeshift.Delete(onduty.RecordID, dboperator);
                                    istatisticschangeshift.Insert(changeshift, dboperator);
                                    dboperator.CommitTransaction();
                                }
                                catch (Exception ex)
                                {
                                    TxtLogServices.WriteTxtLog("添加当班信息异常 异常信息:{0}", ex.Message);
                                    dboperator.RollbackTransaction();
                                }
                            }
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLog("当班统计异常 异常信息:{0}", ex.Message);
            }
        }


    }
}
