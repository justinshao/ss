using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.Utilities;
using Common.Factory;
using Common.IRepository;
using Common.Entities;
using PKFee;

namespace Common.Services.Park
{
    public class ParkFeeRuleServices
    {
        public static bool Add(ParkFeeRule model)
        {
            if (model == null) throw new ArgumentNullException("model");

            model.FeeRuleID = GuidGenerator.GetGuid().ToString();
            model.ParkFeeRuleDetails.ForEach(p => { p.RuleDetailID = GuidGenerator.GetGuid().ToString(); p.RuleID = model.FeeRuleID; });
            IParkFeeRule factory = ParkFeeRuleFactory.GetFactory();
            ParkArea parkArea = ParkAreaServices.QueryByRecordId(model.AreaID);
            if(parkArea==null)throw new MyException("获取区域信息失败");
            if (model.IsOffline)
            {
                List<ParkFeeRule> models = factory.QueryParkFeeRuleByParkingId(parkArea.PKID);
                if (models.Exists(p => p.IsOffline==true)) throw new MyException("该车场已存在脱机收费规则了");
            }
            bool result = factory.Add(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkFeeRule>(model, OperateType.Add);
            }
            return result;
        }

        public static bool Update(ParkFeeRule model)
        {
            if (model == null) throw new ArgumentNullException("model");

            ParkArea parkArea = ParkAreaServices.QueryByRecordId(model.AreaID);
            if (parkArea == null) throw new MyException("获取区域信息失败");

            IParkFeeRule factory = ParkFeeRuleFactory.GetFactory();
            if (model.IsOffline)
            {
                List<ParkFeeRule> models = factory.QueryParkFeeRuleByParkingId(parkArea.PKID);
                if (models.Exists(p => p.IsOffline == true && p.FeeRuleID!=model.FeeRuleID)) throw new MyException("该车场已存在脱机收费规则了");
            }
            model.ParkFeeRuleDetails.ForEach(p => { p.RuleDetailID = GuidGenerator.GetGuid().ToString(); p.RuleID = model.FeeRuleID; });
            bool result = factory.Update(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkFeeRule>(model, OperateType.Update);
            }
            return result;
        }

        public static bool Delete(string feeRuleId)
        {
            if (string.IsNullOrWhiteSpace(feeRuleId)) throw new ArgumentNullException("feeRuleId");

            IParkFeeRule factory = ParkFeeRuleFactory.GetFactory();
            bool result = factory.Delete(feeRuleId);
            if (result)
            {
                OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("feeRuleId:{0}", feeRuleId));
            }
            return result;
        }

        public static List<ParkFeeRule> QueryFeeRuleByCarModelAndCarType(string areaId, string carModelId, string carTypeId)
        {
            if (string.IsNullOrWhiteSpace(areaId)) throw new ArgumentNullException("areaId");
            if (string.IsNullOrWhiteSpace(carModelId)) throw new ArgumentNullException("carModelId");
            if (string.IsNullOrWhiteSpace(carTypeId)) throw new ArgumentNullException("carTypeId");

            IParkFeeRule factory = ParkFeeRuleFactory.GetFactory();
            return factory.QueryFeeRuleByCarModelAndCarType(areaId, carModelId, carTypeId);
        }

        public static ParkFeeRule QueryParkFeeRuleByFeeRuleId(string feeRuleId)
        {
            if (string.IsNullOrWhiteSpace(feeRuleId)) throw new ArgumentNullException("feeRuleId");

            IParkFeeRule factory = ParkFeeRuleFactory.GetFactory();
            return factory.QueryParkFeeRuleByFeeRuleId(feeRuleId);
        }

        public static ParkFeeRule QueryParkFeeRuleByFeeIsOffline(string PKID)
        {
            if (string.IsNullOrWhiteSpace(PKID)) throw new ArgumentNullException("PKID");

            IParkFeeRule factory = ParkFeeRuleFactory.GetFactory();
            return factory.QueryParkFeeRuleByFeeIsOffline(PKID);

        }

        public static List<ParkFeeRule> QueryParkFeeRuleByParkingId(string parkingId)
        {
            if (string.IsNullOrWhiteSpace(parkingId)) throw new ArgumentNullException("parkingId");

            IParkFeeRule factory = ParkFeeRuleFactory.GetFactory();
            return factory.QueryParkFeeRuleByParkingId(parkingId);
        }

        public static List<ParkFeeRule> QueryFeeRules(string parkingId, string carTypeId, string carModelId)
        {
            if (string.IsNullOrWhiteSpace(parkingId)) throw new ArgumentNullException("parkingId");
            if (string.IsNullOrWhiteSpace(carTypeId)) throw new ArgumentNullException("carTypeId");
            if (string.IsNullOrWhiteSpace(carModelId)) throw new ArgumentNullException("carModelId");

            IParkFeeRule factory = ParkFeeRuleFactory.GetFactory();
            return factory.QueryFeeRules(parkingId, carTypeId, carModelId);
        }
        public static List<ParkFeeRuleDetail> QueryFeeRuleDetailByFeeRuleId(string ruleID)
        {
            if (string.IsNullOrWhiteSpace(ruleID)) throw new ArgumentNullException("ruleID");

            IParkFeeRule factory = ParkFeeRuleFactory.GetFactory();
            return factory.QueryFeeRuleDetailByFeeRuleId(ruleID);
        }
        public static decimal TestCalculateFee(DateTime startTime, DateTime endTime, string feeRuleId)
        {
            IParkFeeRule factory = ParkFeeRuleFactory.GetFactory();
            ParkFeeRule rule = factory.QueryParkFeeRuleByFeeRuleId(feeRuleId);
            if (rule == null) throw new MyException("获取规则失败");

            switch (rule.FeeType) {
                case FeeType.DayAndNight: {
                    IFeeRule IFee = new DayNight();
                    IFee.ParkingBeginTime = startTime;
                    IFee.ParkingEndTime = endTime;
                    IFee.FeeRule = rule;
                    IFee.listRuleDetail = rule.ParkFeeRuleDetails;
                    return IFee.CalcFee();
                }
                case FeeType.Hour12:
                    {
                        IFeeRule IFee = new Hours12();
                        IFee.ParkingBeginTime = startTime;
                        IFee.ParkingEndTime = endTime;
                        IFee.FeeRule = rule;
                        IFee.listRuleDetail = rule.ParkFeeRuleDetails;
                        return IFee.CalcFee();
                    }
                case FeeType.Hour24:
                    {
                        IFeeRule IFee = new Hours24();
                        IFee.ParkingBeginTime = startTime;
                        IFee.ParkingEndTime = endTime;
                        IFee.FeeRule = rule;
                        IFee.listRuleDetail = rule.ParkFeeRuleDetails;
                        return IFee.CalcFee();
                    }
                case FeeType.NaturalDay:
                    {
                        IFeeRule IFee = new NaturalDay();
                        IFee.ParkingBeginTime = startTime;
                        IFee.ParkingEndTime = endTime;
                        IFee.FeeRule = rule;
                        IFee.listRuleDetail = rule.ParkFeeRuleDetails;
                        return IFee.CalcFee();
                    }
                case FeeType.Custom:
                    {
                        IFeeRule IFee = new Userdefined2();
                        IFee.ParkingBeginTime = startTime;
                        IFee.ParkingEndTime = endTime;
                        IFee.FeeRule = rule;
                        IFee.FeeText = rule.RuleText;
                        return IFee.CalcFee();
                    }
                default: throw new MyException("算费规则不存在");
            }
        }
    }
}
