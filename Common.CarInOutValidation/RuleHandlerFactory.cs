using Common.CarInOutValidation.Validation;
using Common.CarInOutValidation.Validation.In;
using Common.CarInOutValidation.Validation.Out;
namespace Common.CarInOutValidation
{
    /// <summary>
    /// 验证规则工厂
    /// </summary>
    class RuleHandlerFactory
    {
        #region 系统停车
        RuleHandler monthRuleHandler = new MonthInRuleHandler();

        /// <summary>
        /// 验证车位数
        /// </summary>
        RuleHandler valiCarbitRuleHandler = new ValiCarbitRuleHandler();

        /// <summary>
        /// 验证卡加车牌
        /// </summary>
        RuleHandler valiLpDistinguishRuleHandler = new ValiLpDistinguishRuleHandler();

        /// <summary>
        /// 重复入场判断
        /// </summary>
        RuleHandler valiRepeatInRuleHandler = new ValiRepeatInRuleHandler();

        /// <summary>
        /// 验证进出最小间隔
        /// </summary>
        RuleHandler valiInOutTimeRuleHandler = new ValiInOutTimeRuleHandler();

        /// <summary>
        /// 入场验证有效期
        /// </summary>
        RuleHandler valiInPeriodRuleHandler = new ValiInPeriodRuleHandler();


        /// <summary>
        /// 出场验证有效期
        /// </summary>
        RuleHandler valiOutPeriodRuleHandler = new ValiOutPeriodRuleHandler();

        /// <summary>
        /// 临时车验证入口
        /// </summary>
        RuleHandler tempRuleHandler = new TempInRuleHandler();

        /// <summary>
        /// 储值卡验证入口
        /// </summary>
        RuleHandler valueInRuleHandler = new ValueInRuleHandler();

        /// <summary>
        /// Vip卡验证入口
        /// </summary>
        RuleHandler vIPInRuleHandler = new VIPInRuleHandler();

        /// <summary>
        /// 长期卡出
        /// </summary>
        RuleHandler monthOutRuleHandler = new MonthOutRuleHandler();

        /// <summary>
        /// 月卡出场验证进出信息
        /// </summary>
        RuleHandler valiMonthInRecordRuleHandler = new ValiMonthInRecordRuleHandler();

        /// <summary>
        /// 正常进
        /// </summary>
        RuleHandler valiInOkRuleHandler = new ValiInOkRuleHandler();

        /// <summary>
        /// 临时卡出入口
        /// </summary>
        RuleHandler tempOutRuleHandler = new TempOutRuleHandler();

        /// <summary>
        /// 临时卡出场验证进信息
        /// </summary>
        RuleHandler valiTempOutRecordRuleHandler = new ValiTempOutRecordRuleHandler();

        /// <summary>
        ///VIP卡出验证入口
        /// </summary>
        RuleHandler vIPOutRuleHandler = new VIPOutRuleHandler();

        /// <summary>
        /// 储值卡出场验证入口
        /// </summary>
        RuleHandler valueOutRuleHandler = new ValueOutRuleHandler();

        /// <summary>
        /// 储值卡出场验证信息
        /// </summary>
        RuleHandler valiValueInRecordRuleHandler = new ValiValueInRecordRuleHandler();

        /// <summary>
        /// VIP卡出场验证信息
        /// </summary>
        RuleHandler valiVIPInRecordRuleHandler = new ValiVIPInRecordRuleHandler();

        /// <summary>
        /// 车辆锁定出场验证
        /// </summary>
        RuleHandler valiLockCarRuleHander = new ValiLockCarRuleHander();

        /// <summary>
        /// 重复出场判断
        /// </summary>
        RuleHandler valiRepeatOutRuleHandler = new ValiRepeatOutRuleHandler();

        /// <summary>
        /// 小车场入场判断
        /// </summary>
        RuleHandler nestAreaInRuleHandler = new NestAreaInRuleHandler();

        /// <summary>
        /// 小车场出场判断
        /// </summary>
        RuleHandler nestAreaOutRuleHandler = new NestAreaOutRuleHandler();

        /// <summary>
        /// 黑名单进出验证
        /// </summary>
        VailBlacklistRuleHandler vailBlacklistRuleHandler = new VailBlacklistRuleHandler();
        #endregion


        private static RuleHandlerFactory _Instance = new RuleHandlerFactory();
        public static RuleHandlerFactory Instance { get { return _Instance; } }

        private RuleHandlerFactory() { }

        #region 月卡入场
        /// <summary>
        /// 创建通用系统停车入场验证规则
        /// </summary>
        /// <returns></returns>
        public RuleHandler CreateMonthMainInRuleHandler()
        {
            monthRuleHandler.SetNextRuleHandler(nestAreaInRuleHandler);//小车场入场 
            nestAreaInRuleHandler.SetNextRuleHandler(vailBlacklistRuleHandler);//黑名单验证 
            vailBlacklistRuleHandler.SetNextRuleHandler(valiCarbitRuleHandler);//验证车位数
            valiCarbitRuleHandler.SetNextRuleHandler(valiLpDistinguishRuleHandler);//验证卡加车牌
            valiLpDistinguishRuleHandler.SetNextRuleHandler(valiInPeriodRuleHandler);//验证有效期
            valiInPeriodRuleHandler.SetNextRuleHandler(valiRepeatInRuleHandler);//重复入场判断
            valiRepeatInRuleHandler.SetNextRuleHandler(valiInOutTimeRuleHandler);//验证最小时间间隔
            valiInOutTimeRuleHandler.SetNextRuleHandler(valiInOkRuleHandler);//正常进

            return monthRuleHandler;
        }
        #endregion

        #region 月卡出场
        /// <summary>
        /// 创建通用系统停车出场验证规则
        /// </summary>
        /// <returns></returns>
        public RuleHandler CreateMonthMainOutRuleHandler()
        { 
            monthOutRuleHandler.SetNextRuleHandler(nestAreaOutRuleHandler);//验证车辆锁定  
            nestAreaOutRuleHandler.SetNextRuleHandler(vailBlacklistRuleHandler);//黑名单验证  
            vailBlacklistRuleHandler.SetNextRuleHandler(valiLockCarRuleHander);//验证车辆锁定  
            valiLockCarRuleHander.SetNextRuleHandler(valiLpDistinguishRuleHandler);//验证卡加车牌
            valiLpDistinguishRuleHandler.SetNextRuleHandler(valiOutPeriodRuleHandler);//验证有效期 
            valiOutPeriodRuleHandler.SetNextRuleHandler(valiCarbitRuleHandler);//验证车位 
            valiCarbitRuleHandler.SetNextRuleHandler(valiInOutTimeRuleHandler);//验证最小时间间隔
            valiInOutTimeRuleHandler.SetNextRuleHandler(valiMonthInRecordRuleHandler);//月卡出场验证进场信息
            return monthOutRuleHandler;
        }
        #endregion

        #region 临时车入场
        /// <summary>
        ///临时车入场验证规则
        /// </summary>
        /// <returns></returns>
        public RuleHandler CreateTempMainInRuleHandler()
        {
            tempRuleHandler.SetNextRuleHandler(nestAreaInRuleHandler);//小车场入场
            nestAreaInRuleHandler.SetNextRuleHandler(vailBlacklistRuleHandler);//黑名单验证
            vailBlacklistRuleHandler.SetNextRuleHandler(valiCarbitRuleHandler);//验证车位数 
            valiCarbitRuleHandler.SetNextRuleHandler(valiLpDistinguishRuleHandler);//验证卡加车牌
            valiLpDistinguishRuleHandler.SetNextRuleHandler(valiRepeatInRuleHandler);//重复入场判断
            valiRepeatInRuleHandler.SetNextRuleHandler(valiInOutTimeRuleHandler);//验证最小时间间隔
            return tempRuleHandler;
        }
         
        #endregion

        #region 临时车出场
        /// <summary>
        /// 创建通用非系统停车出场验证规则
        /// </summary>
        /// <returns></returns>
        public RuleHandler CreateTempMainOutRuleHandler()
        {
            tempOutRuleHandler.SetNextRuleHandler(nestAreaOutRuleHandler);//小车场出场  
            nestAreaOutRuleHandler.SetNextRuleHandler(vailBlacklistRuleHandler);   //黑名单验证
            vailBlacklistRuleHandler.SetNextRuleHandler(valiLockCarRuleHander);   //验证车辆锁定
            valiLockCarRuleHander.SetNextRuleHandler(valiLpDistinguishRuleHandler);//验证卡加车牌  
            valiLpDistinguishRuleHandler.SetNextRuleHandler(valiCarbitRuleHandler);//验证车位  
            valiCarbitRuleHandler.SetNextRuleHandler(valiInOutTimeRuleHandler);    //验证最小时间间隔  
            valiInOutTimeRuleHandler.SetNextRuleHandler(valiTempOutRecordRuleHandler);    // 临时卡出场验证进信息   
            return tempOutRuleHandler;
        }
        #endregion
          
        #region 储值卡
        /// <summary>
        /// 储值卡入
        /// </summary>
        /// <returns></returns>
        public RuleHandler CreateValueMainInRuleHandler()
        {
            valueInRuleHandler.SetNextRuleHandler(nestAreaInRuleHandler);//小车场入场 
            nestAreaInRuleHandler.SetNextRuleHandler(vailBlacklistRuleHandler);   //黑名单验证
            vailBlacklistRuleHandler.SetNextRuleHandler(valiCarbitRuleHandler);//验证车位数
            valiCarbitRuleHandler.SetNextRuleHandler(valiLpDistinguishRuleHandler);//验证卡加车牌
            //valiLpDistinguishRuleHandler.SetNextRuleHandler(valiPeriodRuleHandler);//验证有效期
            valiLpDistinguishRuleHandler.SetNextRuleHandler(valiRepeatInRuleHandler);//重复入场判断
            valiRepeatInRuleHandler.SetNextRuleHandler(valiInOutTimeRuleHandler);//验证最小时间间隔
            valiInOutTimeRuleHandler.SetNextRuleHandler(valiInOkRuleHandler);//正常进
            return valueInRuleHandler;
        }

        /// <summary>
        /// 储值卡出
        /// </summary>
        /// <returns></returns>
        public RuleHandler CreateValueMainOutRuleHandler()
        {
            valueOutRuleHandler.SetNextRuleHandler(nestAreaOutRuleHandler);//小车场出场  
            nestAreaOutRuleHandler.SetNextRuleHandler(vailBlacklistRuleHandler);   //黑名单验证
            vailBlacklistRuleHandler.SetNextRuleHandler(valiLockCarRuleHander);   //验证车辆锁定  
            valiLockCarRuleHander.SetNextRuleHandler(valiCarbitRuleHandler);//验证车位  
            valiCarbitRuleHandler.SetNextRuleHandler(valiLpDistinguishRuleHandler);   //验证卡加车牌 
            valiLpDistinguishRuleHandler.SetNextRuleHandler(valiInOutTimeRuleHandler);    //验证最小时间间隔  
            valiInOutTimeRuleHandler.SetNextRuleHandler(valiValueInRecordRuleHandler);    // 临时卡出场验证进信息   
            return valueOutRuleHandler;
        }

        #endregion

        #region VIP卡
        /// <summary>
        /// VIP卡入
        /// </summary>
        /// <returns></returns>
        public RuleHandler CreateVIPMainInRuleHandler()
        {
            vIPInRuleHandler.SetNextRuleHandler(nestAreaInRuleHandler);//小车场入场
            nestAreaInRuleHandler.SetNextRuleHandler(vailBlacklistRuleHandler);   //黑名单验证
            vailBlacklistRuleHandler.SetNextRuleHandler(valiCarbitRuleHandler);//验证车位数
            valiCarbitRuleHandler.SetNextRuleHandler(valiLpDistinguishRuleHandler);//验证卡加车牌
            valiLpDistinguishRuleHandler.SetNextRuleHandler(valiRepeatInRuleHandler);//重复入场判断 
            valiRepeatInRuleHandler.SetNextRuleHandler(valiInOutTimeRuleHandler);//验证最小时间间隔
            valiInOutTimeRuleHandler.SetNextRuleHandler(valiInOkRuleHandler);//正常进
            return vIPInRuleHandler;
        }

        /// <summary>
        /// VIP卡出
        /// </summary>
        /// <returns></returns>
        public RuleHandler CreateVIPMainOutRuleHandler()
        {
            vIPOutRuleHandler.SetNextRuleHandler(nestAreaOutRuleHandler);//小车场出场 
            nestAreaOutRuleHandler.SetNextRuleHandler(vailBlacklistRuleHandler);   //黑名单验证
            vailBlacklistRuleHandler.SetNextRuleHandler(valiLockCarRuleHander);   //验证车辆锁定 
            valiLockCarRuleHander.SetNextRuleHandler(valiLpDistinguishRuleHandler);//验证卡加车牌 
            valiLpDistinguishRuleHandler.SetNextRuleHandler(valiCarbitRuleHandler);//验证车位  
            valiCarbitRuleHandler.SetNextRuleHandler(valiInOutTimeRuleHandler);//验证最小时间间隔 
            valiInOutTimeRuleHandler.SetNextRuleHandler(valiVIPInRecordRuleHandler);//月卡出场验证进场信息
            return vIPOutRuleHandler;
        }
        #endregion

    }
}

