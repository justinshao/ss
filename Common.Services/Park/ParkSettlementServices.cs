using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Entities.Parking;
using Common.Factory.Park;
using Common.IRepository.Park;
using Common.Entities;
using Common.Entities.Other;
using Common.Entities.Statistics;
namespace Common.Services.Park
{
    public class ParkSettlementServices
    {
        /// <summary>
        /// 获得结算单
        /// </summary>
        /// <param name="PKID">公司编号</param>
        /// <param name="SettleStatus">结算状态</param>
        /// <param name="Priod">帐期</param>
        /// <returns></returns>
        public static List<ParkSettlementModel> GetSettlements(string PKID,int SettleStatus,string Priod,string userid)
        {
            IParkSettlement factory = ParkSettlementFactory.GetFactory();

            return factory.GetSettlements(PKID, SettleStatus, Priod, userid);
            
                //return factory.GetSettlements(GetLoginUserVillages(), SettleStatus, Priod, userid);
        }


        public static List<ParkSettlementModel> GetSettlements(IList<string > villIDs, int SettleStatus, string Priod, string userid)
        {
            IParkSettlement factory = ParkSettlementFactory.GetFactory();
            return factory.GetSettlements(villIDs, SettleStatus, Priod, userid);
        }
        /// <summary>
        /// 计算金额
        /// </summary>
        /// <param name="PKID">车场编号</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public static CalSettleAmountModel CalSettleAmount(string PKID, DateTime StartTime, DateTime EndTime)
        {
            CalSettleAmountModel settleamount = new CalSettleAmountModel();
            try
            {
                IParkSettlement factory = ParkSettlementFactory.GetFactory();
                //获取结算金额
                decimal totalamount = 0;
                decimal handlingfeeamount = 0;
                decimal receivableamount = 0;
                //List<ParkOrder> orderlist = factory.GetSettlementPayAmount(PKID, StartTime, EndTime);
                List<Statistics_Gather> orderlist = factory.GetSettlementPayAmount(PKID, StartTime, EndTime);
                if (orderlist != null && orderlist.Count > 0)
                {
                    var park = ParkingServices.QueryParkingByParkingID(PKID);
                    if (park == null || park.HandlingFee <= 0)
                    {
                        settleamount.Message = "未配置车场结算费率 不能生成结算单";
                        settleamount.IsSuccess = false;
                        return settleamount;
                    }
                    //费率在车场配置 
                    decimal handlingfee = park.HandlingFee / 1000;
                    //foreach (var order in orderlist)
                    //{
                    //    //totalamount += order.PayAmount;
                    //    //handlingfeeamount += Math.Round(order.PayAmount * handlingfee, 2);
                    //    
                    //}
                    //receivableamount = totalamount - handlingfeeamount;
                    foreach (var order in orderlist)
                    {
                        
                        totalamount += order.OnLine_Amount;
                       
                    }
                    handlingfeeamount = Math.Round(totalamount * handlingfee, 2);
                    receivableamount = totalamount - handlingfeeamount;
                }

                settleamount.IsSuccess = true;
                settleamount.RateFeeAmount = handlingfeeamount;
                settleamount.ReceiveAmount = receivableamount;
                settleamount.TotalAmount = totalamount;
            }
            catch (Exception ex)
            {
                settleamount.IsSuccess = false;
                settleamount.Message = "计算费用异常";
            }
            return settleamount;
        }

        public static MyResult IsApprovalSettlement(string PKID)
        {
            MyResult result = new MyResult();
            result.result = true;
            try
            {
                IParkSettlement factory = ParkSettlementFactory.GetFactory();
                ParkSettlementModel settlelist = factory.GetMaxPriodSettlement(PKID);
                if (settlelist != null)
                {
                    if (settlelist.SettleStatus != 2 && settlelist.SettleStatus != -1)
                    {
                        //结算单还在审批中 不能建立新的结算单
                        result.msg = "帐期:" + settlelist.Priod + " 的结算单正在审批中 不能创建新的结算单";
                        result.result = false;
                        return result;
                    }
                }
            }
            catch
            {
                result.msg = "其它错误";
                result.result = false;
                return result;
            }
            return result;
        }


        /// <summary>
        /// 生成结算单
        /// </summary>
        /// <param name="PKID">车场编号</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="UserID">用户编号</param>
        /// <returns></returns>
        public static MyResult BuildSettlement(string PKID, DateTime StartTime, DateTime EndTime,string Remark,string UserID)
        {
            MyResult result = new MyResult();
            result.result = false;
            try
            {
                string priod = "F00001";
                IParkSettlement factory = ParkSettlementFactory.GetFactory();
                ParkSettlementModel settlelist = factory.GetMaxPriodSettlement(PKID);
                if (settlelist != null)
                {
                    if (settlelist.SettleStatus != 2 && settlelist.SettleStatus!=-1)
                    {
                        //结算单还在审批中 不能建立新的结算单
                        result.msg = "帐期:" + settlelist.Priod + " 的结算单正在审批中 不能创建新的结算单";
                        return result;
                    }
                    else
                    {
                        string maxpriod = settlelist.Priod;
                        priod = maxpriod.Substring(0, 1) + int.Parse(maxpriod.Substring(1)).ToString().PadLeft(5, '0');
                    }

                    if (StartTime < settlelist.EndTime)
                    {
                        result.msg = "当前结算单的开始时间不能小于前一个结算单结算时间";
                        return result;
                    }

                    if (settlelist.EndTime.AddSeconds(1) != StartTime)
                    {
                        result.msg = "提现需要日期连续";
                        return result;
                    }


                }
                //获取结算金额
                decimal totalamount = 0;
                decimal handlingfeeamount = 0;
                decimal receivableamount = 0;
                //List<ParkOrder> orderlist = factory.GetSettlementPayAmount(PKID, StartTime, EndTime);
                List<Statistics_Gather> orderlist = factory.GetSettlementPayAmount(PKID, StartTime, EndTime);
                if (orderlist != null && orderlist.Count > 0)
                {
                    var park = ParkingServices.QueryParkingByParkingID(PKID);
                    if (park == null || park.HandlingFee <= 0)
                    {
                        result.msg = "未配置车场结算费率 不能生成结算单";
                        return result;
                    }
                    //费率在车场配置 
                    decimal handlingfee = park.HandlingFee / 1000;
                    //foreach (var order in orderlist)
                    //{
                    //    //totalamount += order.PayAmount;
                    //    //handlingfeeamount += Math.Round(order.PayAmount * handlingfee, 2);
                    //    
                    //}
                    //receivableamount = totalamount - handlingfeeamount;
                    foreach (var order in orderlist)
                    {

                        totalamount += order.OnLine_Amount;

                    }
                    handlingfeeamount = Math.Round(totalamount * handlingfee, 2);
                    receivableamount = totalamount - handlingfeeamount;
                }
                ParkSettlementModel settlemodel = new ParkSettlementModel()
                {
                    PKID = PKID,
                    TotalAmount = totalamount,
                    ReceivableAmount = receivableamount,
                    HandlingFeeAmount = handlingfeeamount,
                    EndTime = EndTime,
                    StartTime = StartTime,
                    Priod = priod,
                    Remark = Remark,
                    CreateUser = UserID
                };
                result.result = factory.BuildSettlement(settlemodel);
            }
            catch
            {
                result.msg = "生成结算单异常";
            }
            return result;
        }
        /// <summary>
        /// 撤销结算单
        /// </summary>
        /// <param name="RecordID"></param>
        /// <param name="SettleStatus"></param>
        /// <returns></returns>
        public static bool CancelSettleDocument(string RecordID, int SettleStatus)
        {
            SettleStatus--;
            IParkSettlement factory = ParkSettlementFactory.GetFactory();
            return factory.UpdateSettlementStatus(RecordID, SettleStatus);
        }
        public static bool ApplySettleDocument(string RecordID, int SettleStatus)
        {
            SettleStatus++;
            IParkSettlement factory = ParkSettlementFactory.GetFactory();
            return factory.UpdateSettlementStatus(RecordID, SettleStatus);
        }
        /// <summary>
        /// 根据车场编号获得所有有效帐期
        /// </summary>
        /// <param name="PKID">车场编号</param>
        /// <returns></returns>
        public static List<string> GetPriods(string PKID)
        {
            IParkSettlement factory = ParkSettlementFactory.GetFactory();
            return factory.GetPriods(PKID);
        }
        /// <summary>
        /// 根据车场编号获取下一个帐期
        /// </summary>
        /// <param name="PKID">车场编号</param>
        /// <returns></returns>
        public static string GetNextPriod(string PKID)
        {
            string priod = "F00001";
            IParkSettlement factory = ParkSettlementFactory.GetFactory();
            ParkSettlementModel settlement = factory.GetMaxPriodSettlement(PKID);
            if (settlement != null)
            {
                int current = int.Parse(settlement.Priod.Substring(1));
                current++;
                priod = "F" + current.ToString().PadLeft(5, '0');
            }
            return priod;
        }
        /// <summary>
        /// 获得最后的结算单
        /// </summary>
        /// <param name="PKID">车场编号</param>
        /// <returns></returns>
        public static ParkSettlementModel GetMaxSettlement(string PKID)
        {
            IParkSettlement factory = ParkSettlementFactory.GetFactory();
            ParkSettlementModel settlement = factory.GetMaxPriodSettlement(PKID);
            if (settlement != null)
            {
                if (settlement.EndTime > DateTime.MinValue)
                {
                    settlement.EndTimeName = settlement.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                    settlement.EndTimeName1 = settlement.EndTime.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    settlement.EndTimeName = "";
                    settlement.EndTimeName1 = "";
                }
                
            }
            else
            {
                settlement = new ParkSettlementModel();
                settlement.EndTimeName = "";
                settlement.EndTimeName1 = "";
            }
            return settlement;
        }

    }
}
