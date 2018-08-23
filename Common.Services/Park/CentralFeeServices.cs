using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.Entities;
using Common.Entities.Validation;
using Common.DataAccess;
using Common.Factory.Park;
using Common.IRepository.Park;

namespace Common.Services.Park
{
    public class CentralFeeServices
    {
        public static bool Payment(string ioRecordId, string parkingId, ResultAgs billResult, string operatorId)
        {
            if (billResult == null) throw new MyException("获取缴费信息失败");
            if (billResult.FeeRule == null) throw new MyException("获取收费规则失败");
            if (billResult.Rate == null) throw new MyException("获取缴费金额失败");
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    List<string> carderateIds = new List<string>();
                    Dictionary<string, decimal> sellers = new Dictionary<string, decimal>();
                    if (billResult.Carderates != null && billResult.Carderates.Count > 0)
                    {
                        carderateIds = billResult.Carderates.Select(p => p.CarDerateID).ToList();
                        foreach (var item in billResult.Carderates)
                        {
                            if (sellers.ContainsKey(item.Derate.SellerID))
                            {
                                sellers[item.Derate.SellerID] = sellers[item.Derate.SellerID] + item.FreeMoney;
                                continue;
                            }
                            sellers.Add(item.Derate.SellerID, item.FreeMoney);
                        }
                    }
                    RateInfo rate = billResult.Rate;
                    ParkOrder order = ParkOrderServices.AddCentralFeeOrder(parkingId, ioRecordId, rate.Amount, rate.UnPayAmount, rate.DiscountAmount, carderateIds, billResult.FeeRule.FeeRuleID, rate.CashTime, rate.CashMoney, operatorId, dbOperator);
                    if (order == null) throw new MyException("创建缴费订单失败");
                    if (carderateIds.Count > 0)
                    {
                        IParkSeller factory = ParkSellerFactory.GetFactory();
                        factory.UpdateParkCarDerateStatus(carderateIds, 2, dbOperator); 

                        foreach (var item in sellers)
                        {
                            bool result = factory.SellerDebit(item.Key, item.Value, dbOperator);
                            if (!result) throw new MyException("商家扣款失败");
                        }

                    }
                    dbOperator.CommitTransaction();
                    
                    OperateLogServices.AddOperateLog(OperateType.Other, string.Format("中央收费，ioRecordId:{0},parkingId:{1},operatorId:{2},收费金额：{3},优惠金额:{4}", ioRecordId, parkingId, operatorId, rate.UnPayAmount, rate.DiscountAmount));
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
            return true;
        }
    }
}
