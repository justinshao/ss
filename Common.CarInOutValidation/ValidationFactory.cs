
using System.Linq;
using System;
using Common.Entities;
using Common.Entities.Parking;
using Common.Services.Park;
using Common.Services;
using Common.Core.Expands;
using Common.Services.BaseData;
using Common.Entities.Validation;
using Common.Utilities.Helpers;

namespace Common.CarInOutValidation
{
    public class ValidationFactory
    {
        /// <summary>
        /// 接口的工厂实现
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ResultAgs Process(InputAgs args)
        {
            var rst = new ResultAgs();
            var ruleHandler = CreateRuleHandler(args, rst);
            if (ruleHandler != null)
            {
                ruleHandler.ApplyRule(args, rst);
            }
            return rst;
        }

        /// <summary>
        /// Context工厂
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private RuleHandler CreateRuleHandler(InputAgs args, ResultAgs rst)
        {
            string errorMsg = "";
            if (args.AreadInfo.IsNestArea && args.AreadInfo.Parent == null)//内部车场时 且上级区域为空时
            {
                args.AreadInfo.Parent = ParkAreaServices.QueryByRecordId(args.AreadInfo.MasterID);
                if (args.AreadInfo.Parent == null)//还是找不到上级区域时
                {
                    LogerHelper.Loger.Error(string.Format("找不到车场[{0}]的上级车场，上级车场ID为[{1}].", args.AreadInfo.AreaName, args.AreadInfo.MasterID));
                    args.AreadInfo.MasterID = "";//找不到就至为空
                }
            }
            //判断系统停车或非系统停车
            var cartype = BaseCarType.TempCar;
            if (!args.Plateinfo.LicenseNum.Contains("无") && args.Plateinfo.LicenseNum.Length > 3)//不是“无车牌”
            {
                EmployeePlate platenumber = null;
                if (args.GateInfo.OpenPlateBlurryMatch == YesOrNo.Yes)
                {
                    string likeplate = args.Plateinfo.LicenseNum;
                    platenumber = EmployeePlateServices.GetEmployeePlateNumberByPlateNumber(args.AreadInfo.Parkinfo.VID, likeplate, out errorMsg);

                    if (platenumber == null && args.Plateinfo.LicenseNum.Length >= 3)//模糊识别 前一位后一位模糊识别
                    {
                        likeplate = args.Plateinfo.LicenseNum.Substring(2, args.Plateinfo.LicenseNum.Length - 2);
                        platenumber = EmployeePlateServices.GetGrantPlateNumberByLike(args.AreadInfo.Parkinfo.VID, likeplate, out errorMsg);
                        if (platenumber == null)
                        {
                            likeplate = args.Plateinfo.LicenseNum.Substring(0, args.Plateinfo.LicenseNum.Length - 2);
                            platenumber = EmployeePlateServices.GetGrantPlateNumberByLike(args.AreadInfo.Parkinfo.VID, likeplate, out errorMsg);
                        }
                        if (platenumber != null)
                        {
                            args.Plateinfo.LicenseNum = platenumber.PlateNo;
                        }
                    }
                }
                else
                {
                    platenumber = EmployeePlateServices.GetEmployeePlateNumberByPlateNumber(args.AreadInfo.Parkinfo.VID, args.Plateinfo.LicenseNum, out errorMsg);
                }
                if (platenumber != null)
                {
                    //根据车牌获取车辆用户信息
                    var pkcard = ParkGrantServices.GetParkGrantByPlateNumberID(args.AreadInfo.PKID, platenumber.PlateID, out errorMsg);

                    if (pkcard.Count() > 0)
                    {
                        //判断是否有区域 通道限制
                        var cardinfo = pkcard.First(); 
                        args.LastCardInfo = cardinfo;
                        if (cardinfo.AreaIDS.IsEmpty())//没有限制
                        {
                            if ((cardinfo.GateID.IsEmpty())
                                  || cardinfo.GateID.Contains(args.GateInfo.GateID))//区域包含 且通道未控制授权 或 通道已经授权
                            {
                                args.CardInfo = cardinfo;
                                args.CardInfo.OwnerPlateNumber = platenumber;
                                var usercard = BaseCardServices.GetBaseCard(args.CardInfo.CardID, out errorMsg);
                                if (usercard != null)
                                {
                                    args.CardInfo.Usercard = usercard;
                                }
                                else
                                {
                                    args.CardInfo = null;
                                }
                            }
                        }
                        else
                        {
                            if (cardinfo.AreaIDS.Contains(args.AreadInfo.AreaID) && (cardinfo.GateID.IsEmpty())
                                || cardinfo.GateID.Contains(args.GateInfo.GateID))//区域包含 且通道未控制授权 或 通道已经授权
                            {
                                args.CardInfo = cardinfo;
                                args.CardInfo.OwnerPlateNumber = platenumber;
                                var usercard = BaseCardServices.GetBaseCard(args.CardInfo.CardID, out errorMsg);
                                if (usercard != null)
                                {
                                    args.CardInfo.Usercard = usercard;
                                }
                                else
                                {
                                    args.CardInfo = null;
                                }

                            }
                        }
                        if (args.CardInfo == null)
                        {
                            rst.ValidMsg = "非本区域月卡";
                        }
                    }
                }

                if (args.CardInfo != null)
                {
                    var pkcardtype = ParkCarTypeServices.QueryParkCarTypeByRecordId(args.CardInfo.CarTypeID);
                    if (pkcardtype != null)
                    {
                        args.CarTypeInfo = pkcardtype;
                    }
                    else
                    {
                        //获取对应的卡类型
                        var pkcardtypes = ParkCarTypeServices.QueryCardTypesByBaseCardType(args.AreadInfo.PKID, BaseCarType.TempCar);
                        if (pkcardtypes != null && pkcardtypes.Count() > 0)
                        {
                            args.CarTypeInfo = pkcardtypes.First();
                        }
                    }

                    if (!args.CardInfo.CarModelID.IsEmpty())
                    {
                        var model = ParkCarModelServices.QueryByRecordId(args.CardInfo.CarModelID);
                        args.CarModel = model;
                    }
                    if (args.CarModel == null)
                    {
                        //获取卡对应的车类型
                        var listcarmodel = ParkCarModelServices.GetDefaultCarModel(args.AreadInfo.PKID, out errorMsg);
                        if (listcarmodel != null)
                        {
                            args.CarModel = listcarmodel;
                        }
                    }

                }
                else
                {
                    //默认为临时车
                    var pkcardtypes = ParkCarTypeServices.QueryCardTypesByBaseCardType(args.AreadInfo.PKID, BaseCarType.TempCar);
                    if (pkcardtypes != null && pkcardtypes.Count() > 0)
                    {
                        args.CarTypeInfo = pkcardtypes.First();
                    }

                    ////获取默认车类型
                    //var listcarmodel = ParkCarModelServices.GetDefaultCarModel(args.AreadInfo.PKID, out errorMsg);
                    //if (listcarmodel != null)
                    //{
                    //    args.CarModel = listcarmodel;
                    //}

                    if (args.Plateinfo.PlateColor != null && args.Plateinfo.PlateColor.Contains("黄"))
                    {
                        var list = ParkCarModelServices.QueryByParkingId(args.AreadInfo.PKID);
                        foreach (var item in list)
                        {
                            if (item.PlateColor != null && item.PlateColor.Contains("黄"))
                            {
                                args.CarModel = item;
                                break;
                            }
                        }
                    }
                    if (args.CarModel == null)
                    {
                        //获取卡对应的车类型 
                        var carmode = ParkCarModelServices.GetDefaultCarModel(args.AreadInfo.PKID, out errorMsg);
                        if (carmode != null)
                        {
                            args.CarModel = carmode;
                        }
                    }

                }
            }
            else
            {
                //默认为临时车
                var pkcardtypes = ParkCarTypeServices.QueryCardTypesByBaseCardType(args.AreadInfo.PKID, BaseCarType.TempCar);
                if (pkcardtypes != null && pkcardtypes.Count() > 0)
                {
                    args.CarTypeInfo = pkcardtypes.First();
                }
                if (args.Plateinfo.PlateColor!=null && args.Plateinfo.PlateColor.Contains("黄"))
                {
                    var list = ParkCarModelServices.QueryByParkingId(args.AreadInfo.PKID);
                    foreach (var item in list)
                    {
                        if (item.PlateColor != null && item.PlateColor.Contains("黄"))
                        {
                            args.CarModel = item;
                            break;
                        }
                    }
                }
                if (args.CarModel == null)
                {
                    //获取卡对应的车类型 
                    var carmode = ParkCarModelServices.GetDefaultCarModel(args.AreadInfo.PKID, out errorMsg);
                    if (carmode != null)
                    {
                        args.CarModel = carmode;
                    }
                }
            }
            if (args.AreadInfo.IsNestArea)//小车场 先查询对应的通行记录
            {
                ParkIORecord ioRecord = ParkIORecordServices.GetNoExitIORecordByPlateNumber(args.AreadInfo.PKID, args.Plateinfo.LicenseNum, out errorMsg);

                args.NestAreaIORecord = ioRecord;
            }
            if (args.CarTypeInfo == null)
            {
                rst.ResCode = ResultCode.LocalError;
                throw new Exception("参数错误：默认卡片类型为空");
            }
            if (args.CarTypeInfo == null)
            {
                rst.ResCode = ResultCode.LocalError;
                throw new Exception("参数错误：默认车辆类型为空");
            }

            cartype = args.CarTypeInfo.BaseTypeID;
            #region 生产对应的验证规则
            //此处可根据配置文件利用反射生成配置的验证规则
            RuleHandler ruleHandler = null;
            switch (cartype)
            {
                case BaseCarType.MonthlyRent:
                    #region 月卡
                    if (args.GateInfo.IoState == IoState.GoIn)//进
                    {
                        ruleHandler = RuleHandlerFactory.Instance.CreateMonthMainInRuleHandler();
                    }
                    else//出
                    {
                        ruleHandler = RuleHandlerFactory.Instance.CreateMonthMainOutRuleHandler();
                    }
                    #endregion
                    break;
                case BaseCarType.TempCar:
                    #region 临时卡
                    if (args.GateInfo.IoState == IoState.GoIn)//进
                    {
                        ruleHandler = RuleHandlerFactory.Instance.CreateTempMainInRuleHandler();
                    }
                    else//出
                    {
                        ruleHandler = RuleHandlerFactory.Instance.CreateTempMainOutRuleHandler();
                    }
                    #endregion
                    break;
                case BaseCarType.StoredValueCar:
                    #region 储值卡
                    if (args.GateInfo.IoState == IoState.GoIn)//进
                    {
                        ruleHandler = RuleHandlerFactory.Instance.CreateValueMainInRuleHandler();
                    }
                    else//出
                    {
                        ruleHandler = RuleHandlerFactory.Instance.CreateValueMainOutRuleHandler();
                    }
                    #endregion
                    break;
                case BaseCarType.VIPCar:
                    #region VIP卡
                    if (args.GateInfo.IoState == IoState.GoIn)//进
                    {
                        ruleHandler = RuleHandlerFactory.Instance.CreateVIPMainInRuleHandler();
                    }
                    else//出
                    {
                        ruleHandler = RuleHandlerFactory.Instance.CreateVIPMainOutRuleHandler();
                    }
                    #endregion
                    break;
            }
            #endregion
            return ruleHandler;
        }

    }
}
