using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Entities.Parking;

using Common.Entities.Statistics;
using Common.IRepository.Park;
using Common.Factory.Park;
using Common.IRepository.Statistics;
using Common.Factory.Statistics;
using Common.DataAccess;
namespace Common.Services.Statistics
{
    /// <summary>
    /// 通道统计
    /// </summary>
    internal class GateGatherServices
    {
        /// <summary>
        /// 按通道统计
        /// </summary>
        /// <param name="gatelist">通道集合</param>
        /// <param name="park">车场</param>
        /// <param name="dt">日期</param>
        /// <param name="loop">次数</param>
        public void Statistics_DailyByGate(List<ParkGate> gatelist,BaseParkinfo park, DateTime dt, int loop)
        {
            #region 统计通道的实收应收  按小时
            if (gatelist == null || gatelist.Count == 0)
                return;
            IParkIORecord iorecordfactory = ParkIORecordFactory.GetFactory();
            IParkOrder iorder = ParkOrderFactory.GetFactory();
            IStatistics_GatherGate igathergate = Statistics_GatherGateFactory.GetFactory();
            foreach (var g in gatelist)
            {
                DateTime startdate = DateTime.Parse(dt.AddDays(-loop).ToString("yyyy-MM-dd 00:00:00"));
                DateTime enddate = DateTime.Parse(dt.AddDays(-loop).ToString("yyyy-MM-dd 00:59:59"));
                int hours = 0;
                int maxhours = 23;
                List<Statistics_GatherGate> gathergates = new List<Statistics_GatherGate>();
                List<DateTime> DeleteGather = new List<DateTime>();

                if (startdate.ToString("yyyyMMdd") == dt.ToString("yyyyMMdd"))
                {
                    maxhours = dt.Hour;
                }
                while (hours <= maxhours)
                {
                    System.Threading.Thread.Sleep(50);
                    Statistics_GatherGate gather = new Statistics_GatherGate();
                    DateTime tempstartdate = startdate.AddHours(hours);
                    DateTime tempenddate = enddate.AddHours(hours);
                    gather.StatisticsGatherID = System.Guid.NewGuid().ToString();
                    gather.ParkingID = park.PKID;
                    gather.ParkingName = park.PKName;
                    gather.GatherTime = tempstartdate;
                    gather.BoxID = g.BoxID;
                    gather.BoxName = g.BoxName;
                    gather.GateID = g.GateID;
                    gather.GateName = g.GateName;

                    if (startdate.ToString("yyyyMMdd") == dt.ToString("yyyyMMdd"))
                    {
                        if (hours == maxhours || hours == maxhours - 1)
                        {
                            DeleteGather.Add(tempstartdate);
                        }
                        else
                        {
                            if (igathergate.IsExists(g.GateID, tempstartdate))
                            {
                                hours++;
                                continue;
                            }
                        }
                    }
                    else
                    {
                        if (igathergate.IsExists(g.GateID, tempstartdate))
                        {
                            hours++;
                            continue;
                        }
                    }

                    //查询进场数
                    gather.Entrance_Count = iorecordfactory.EntranceCountByGate(g.GateID, tempstartdate, tempenddate);
                    //查询出场数
                    gather.Exit_Count = iorecordfactory.ExitCountByGate(g.GateID, tempstartdate, tempenddate);

                    #region 进场卡片类型
                    List<KeyValue> _InCardType = iorecordfactory.GetInCardTypeByGateID(g.GateID, tempstartdate, tempenddate);
                    if (_InCardType != null && _InCardType.Count > 0)
                    {
                        foreach (var kv in _InCardType)
                        {
                            switch (kv.KeyName)
                            {
                                case "0":
                                    gather.VIPCard = kv.Key_Value;
                                    break;
                                case "1":
                                    gather.StordCard = kv.Key_Value;
                                    break;
                                case "2":
                                    gather.MonthCard = kv.Key_Value;
                                    break;
                                case "3":
                                    gather.TempCard = kv.Key_Value;
                                    break;
                                case "4":
                                    gather.JobCard = kv.Key_Value;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    #endregion

                    #region 统计放行类型
                    List<KeyValue> _ReleaseType = iorecordfactory.GetReleaseTypeByGate(g.GateID, tempstartdate, tempenddate);
                    if (_ReleaseType != null && _ReleaseType.Count > 0)
                    {
                        foreach (var kv in _ReleaseType)
                        {
                            switch (kv.KeyName)
                            {
                                case "0":
                                    gather.ReleaseType_Normal = kv.Key_Value;
                                    break;
                                case "1":
                                    gather.ReleaseType_Charge = kv.Key_Value;
                                    break;
                                case "2":
                                    gather.ReleaseType_Free = kv.Key_Value;
                                    break;
                                case "3":
                                    gather.ReleaseType_Catch = kv.Key_Value;
                                    break;
                            }
                        }
                    }
                    #endregion

                    #region 统计费用相关
                    List<ParkOrder> _orderfee = iorder.GetOrdersByGateID(park.PKID,g.GateID,tempstartdate,tempenddate);
                    if (_orderfee != null && _orderfee.Count > 0)
                    {
                        foreach (var o in _orderfee)
                        {
                            gather.Receivable_Amount += o.Amount;
                            gather.Real_Amount += o.PayAmount;
                            gather.Diff_Amount +=  o.UnPayAmount;
                            //支付方式(1.现金、2.微信、3.支付宝、4.网银、5.电子钱包、6.优免卷7.余额
                            switch (o.PayWay)
                            {
                                case OrderPayWay.Cash:
                                    gather.Cash_Amount +=  o.PayAmount;
                                    gather.Cash_Count++;

                                    if (o.DiscountAmount > 0)
                                    {
                                        gather.CashDiscount_Amount +=  o.DiscountAmount;
                                        gather.CashDiscount_Count++;

                                        gather.Discount_Amount += o.DiscountAmount;
                                        gather.Discount_Count++;
                                    }

                                    break;
                                case OrderPayWay.WeiXin:
                                case OrderPayWay.Alipay:
                                case OrderPayWay.OnlineBanking:
                                case OrderPayWay.Wallet:
                                    gather.OnLine_Amount += o.PayAmount;
                                    gather.OnLine_Count++;

                                    if (o.DiscountAmount > 0)
                                    {
                                        gather.OnLineDiscount_Amount +=  o.DiscountAmount;
                                        gather.OnLineDiscount_Count++;

                                        gather.Discount_Amount += o.DiscountAmount;
                                        gather.Discount_Count++;
                                    }
                                    break;
                                case OrderPayWay.PreferentialTicket:
                                    gather.Discount_Amount += o.DiscountAmount;
                                    gather.Discount_Count++;
                                    break;
                                case OrderPayWay.ValueCard:
                                    gather.StordCard_Amount += o.PayAmount;
                                    gather.StordCard_Count++;
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
                                            gather.OnLineTemp_Amount +=  o.PayAmount;
                                            gather.OnLineTemp_Count++;
                                            break;
                                        default:
                                            gather.Temp_Amount += o.PayAmount;
                                            gather.Temp_Count++;
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
                                            gather.OnLineMonthCardExtend_Amount += o.PayAmount;
                                            gather.OnLineMonthCardExtend_Count++;
                                            break;
                                        default:
                                            gather.MonthCardExtend_Amount +=  o.PayAmount;
                                            gather.MonthCardExtend_Count++;
                                            break;
                                    }
                                    break;
                                //VIP卡续期
                                case OrderType.VIPCardRenewal:
                                    gather.VIPExtend_Count++;
                                    break;
                                //储值卡充值
                                case OrderType.ValueCardRecharge:
                                    switch (o.PayWay)
                                    {
                                        case OrderPayWay.WeiXin:
                                        case OrderPayWay.Alipay:
                                        case OrderPayWay.OnlineBanking:
                                        case OrderPayWay.Wallet:
                                            gather.OnLineStordCard_Amount += o.PayAmount;
                                            gather.OnLineStordCard_Count++;
                                            break;
                                        default:
                                            gather.StordCardRecharge_Amount +=  o.PayAmount;
                                            gather.StordCardRecharge_Count++;
                                            break;
                                    }
                                    break;
                            }
                        }
                    }
                    #endregion

                    gathergates.Add(gather);
                    hours++;
                }
                if (gathergates != null)
                {
                    using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
                    {
                        try
                        {
                            dboperator.BeginTransaction();
                            foreach (DateTime d in DeleteGather)
                            {
                                igathergate.Delete(park.PKID, dt, dboperator);
                            }
                            foreach (var gate in gathergates)
                            {
                                if (!igathergate.Insert(gate, dboperator))
                                    throw new Exception("插入通道统计数据失败");
                            }
                            dboperator.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            Common.Services.TxtLogServices.WriteTxtLog("添加通道统计信息异常 异常信息:{0}", ex.Message);
                            dboperator.RollbackTransaction();
                        }
                    }
                }
            }
            #endregion

        }

        public void Statistics_DailyByGate(List<ParkGate> gatelist, BaseParkinfo park, DateTime startdate, DateTime enddate)
        {
            #region 统计通道的实收应收  按小时
            if (gatelist == null || gatelist.Count == 0)
                return;
            IParkIORecord iorecordfactory = ParkIORecordFactory.GetFactory();
            IParkOrder iorder = ParkOrderFactory.GetFactory();
            IStatistics_GatherGate igathergate = Statistics_GatherGateFactory.GetFactory();
            foreach (var g in gatelist)
            { 
                int hours = 0;
                int maxhours = 23;
                List<Statistics_GatherGate> gathergates = new List<Statistics_GatherGate>();
                List<DateTime> DeleteGather = new List<DateTime>();

                if (startdate.ToString("yyyyMMdd") == startdate.ToString("yyyyMMdd"))
                {
                    maxhours = startdate.Hour;
                }
                while (hours <= maxhours)
                {
                    System.Threading.Thread.Sleep(50);
                    Statistics_GatherGate gather = new Statistics_GatherGate();
                    DateTime tempstartdate = startdate.AddHours(hours);
                    DateTime tempenddate = enddate.AddHours(hours);
                    gather.StatisticsGatherID = System.Guid.NewGuid().ToString();
                    gather.ParkingID = park.PKID;
                    gather.ParkingName = park.PKName;
                    gather.GatherTime = tempstartdate;
                    gather.BoxID = g.BoxID;
                    gather.BoxName = g.BoxName;
                    gather.GateID = g.GateID;
                    gather.GateName = g.GateName;

                    if (startdate.ToString("yyyyMMdd") == startdate.ToString("yyyyMMdd"))
                    {
                        if (hours == maxhours || hours == maxhours - 1)
                        {
                            DeleteGather.Add(tempstartdate);
                        }
                        else
                        {
                            if (igathergate.IsExists(g.GateID, tempstartdate))
                            {
                                hours++;
                                continue;
                            }
                        }
                    }
                    else
                    {
                        if (igathergate.IsExists(g.GateID, tempstartdate))
                        {
                            hours++;
                            continue;
                        }
                    }

                    //查询进场数
                    gather.Entrance_Count = iorecordfactory.EntranceCountByGate(g.GateID, tempstartdate, tempenddate);
                    //查询出场数
                    gather.Exit_Count = iorecordfactory.ExitCountByGate(g.GateID, tempstartdate, tempenddate);

                    #region 进场卡片类型
                    List<KeyValue> _InCardType = iorecordfactory.GetInCardTypeByGateID(g.GateID, tempstartdate, tempenddate);
                    if (_InCardType != null && _InCardType.Count > 0)
                    {
                        foreach (var kv in _InCardType)
                        {
                            switch (kv.KeyName)
                            {
                                case "0":
                                    gather.VIPCard = kv.Key_Value;
                                    break;
                                case "1":
                                    gather.StordCard = kv.Key_Value;
                                    break;
                                case "2":
                                    gather.MonthCard = kv.Key_Value;
                                    break;
                                case "3":
                                    gather.TempCard = kv.Key_Value;
                                    break;
                                case "4":
                                    gather.JobCard = kv.Key_Value;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    #endregion

                    #region 统计放行类型
                    List<KeyValue> _ReleaseType = iorecordfactory.GetReleaseTypeByGate(g.GateID, tempstartdate, tempenddate);
                    if (_ReleaseType != null && _ReleaseType.Count > 0)
                    {
                        foreach (var kv in _ReleaseType)
                        {
                            switch (kv.KeyName)
                            {
                                case "0":
                                    gather.ReleaseType_Normal = kv.Key_Value;
                                    break;
                                case "1":
                                    gather.ReleaseType_Charge = kv.Key_Value;
                                    break;
                                case "2":
                                    gather.ReleaseType_Free = kv.Key_Value;
                                    break;
                                case "3":
                                    gather.ReleaseType_Catch = kv.Key_Value;
                                    break;
                            }
                        }
                    }
                    #endregion

                    #region 统计费用相关
                    List<ParkOrder> _orderfee = iorder.GetOrdersByGateID(park.PKID, g.GateID, tempstartdate, tempenddate);
                    if (_orderfee != null && _orderfee.Count > 0)
                    {
                        foreach (var o in _orderfee)
                        {
                            gather.Receivable_Amount += o.Amount;
                            gather.Real_Amount += o.PayAmount;
                            gather.Diff_Amount += o.UnPayAmount;
                            //支付方式(1.现金、2.微信、3.支付宝、4.网银、5.电子钱包、6.优免卷7.余额
                            switch (o.PayWay)
                            {
                                case OrderPayWay.Cash:
                                    gather.Cash_Amount += o.PayAmount;
                                    gather.Cash_Count++;

                                    if (o.DiscountAmount > 0)
                                    {
                                        gather.CashDiscount_Amount += o.DiscountAmount;
                                        gather.CashDiscount_Count++;

                                        gather.Discount_Amount += o.DiscountAmount;
                                        gather.Discount_Count++;
                                    }

                                    break;
                                case OrderPayWay.WeiXin:
                                case OrderPayWay.Alipay:
                                case OrderPayWay.OnlineBanking:
                                case OrderPayWay.Wallet:
                                    gather.OnLine_Amount += o.PayAmount;
                                    gather.OnLine_Count++;

                                    if (o.DiscountAmount > 0)
                                    {
                                        gather.OnLineDiscount_Amount += o.DiscountAmount;
                                        gather.OnLineDiscount_Count++;

                                        gather.Discount_Amount += o.DiscountAmount;
                                        gather.Discount_Count++;
                                    }
                                    break;
                                case OrderPayWay.PreferentialTicket:
                                    gather.Discount_Amount += o.DiscountAmount;
                                    gather.Discount_Count++;
                                    break;
                                case OrderPayWay.ValueCard:
                                    gather.StordCard_Amount += o.PayAmount;
                                    gather.StordCard_Count++;
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
                                            gather.OnLineTemp_Amount += o.PayAmount;
                                            gather.OnLineTemp_Count++;
                                            break;
                                        default:
                                            gather.Temp_Amount += o.PayAmount;
                                            gather.Temp_Count++;
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
                                            gather.OnLineMonthCardExtend_Amount += o.PayAmount;
                                            gather.OnLineMonthCardExtend_Count++;
                                            break;
                                        default:
                                            gather.MonthCardExtend_Amount += o.PayAmount;
                                            gather.MonthCardExtend_Count++;
                                            break;
                                    }
                                    break;
                                //VIP卡续期
                                case OrderType.VIPCardRenewal:
                                    gather.VIPExtend_Count++;
                                    break;
                                //储值卡充值
                                case OrderType.ValueCardRecharge:
                                    switch (o.PayWay)
                                    {
                                        case OrderPayWay.WeiXin:
                                        case OrderPayWay.Alipay:
                                        case OrderPayWay.OnlineBanking:
                                        case OrderPayWay.Wallet:
                                            gather.OnLineStordCard_Amount += o.PayAmount;
                                            gather.OnLineStordCard_Count++;
                                            break;
                                        default:
                                            gather.StordCardRecharge_Amount += o.PayAmount;
                                            gather.StordCardRecharge_Count++;
                                            break;
                                    }
                                    break;
                            }
                        }
                    }
                    #endregion

                    gathergates.Add(gather);
                    hours++;
                }
                if (gathergates != null)
                {
                    using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
                    {
                        try
                        {
                            dboperator.BeginTransaction();
                            foreach (DateTime d in DeleteGather)
                            {
                                igathergate.Delete(park.PKID, startdate, dboperator);
                            }
                            foreach (var gate in gathergates)
                            {
                                if (!igathergate.Insert(gate, dboperator))
                                    throw new Exception("插入通道统计数据失败");
                            }
                            dboperator.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            Common.Services.TxtLogServices.WriteTxtLog("添加通道统计信息异常 异常信息:{0}", ex.Message);
                            dboperator.RollbackTransaction();
                        }
                    }
                }
            }
            #endregion

        }
    }
}
