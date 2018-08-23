using System;
using Common.Entities;
using Common.Entities.Validation;
using Common.Utilities.Helpers;

namespace Common.CarInOutValidation
{
    /// <summary>
    /// 验证规则基类(职责链模式)
    /// </summary>
    abstract class RuleHandler
    {
        private string[] FreeCarStartContains = new string[] { "WJ", "军", "空", "海", "北", "沈", "兰", "济", "南", "广", "成", "甲", "V", "Z", "B", "N", "G", "S", "C", "L", "J", "K", "H" };
        private string[] FreeCarEndContains = new string[] { "警" };
        private RuleHandler _NextRuleHandler { get; set; }

        /// <summary>
        /// 设置下一步要应用的验证规则
        /// </summary>
        /// <param name="ruleHandler"></param>
        public void SetNextRuleHandler(RuleHandler ruleHandler)
        {
            _NextRuleHandler = ruleHandler;
        }

        /// <summary>
        /// 应用验证规则处理数据
        /// </summary>
        /// <param name="args"></param>
        /// <param name="rst"></param>
        protected abstract void ProcessRule(InputAgs args, ResultAgs rst);

        /// <summary>
        /// 应用校验规则
        /// </summary>
        /// <param name="args"></param>
        /// <param name="rst"></param>
        public void ApplyRule(InputAgs args, ResultAgs rst)
        {
            try
            {
                ProcessRule(args, rst);
            }
            catch (Exception ex)
            {
                rst.ResCode = ResultCode.ErrorMSG;
                LogerHelper.Loger.Error(ex);
            }

            //执行下一个职责链
            if (_NextRuleHandler != null && rst.ResCode == ResultCode.Defaut)
            {
                _NextRuleHandler.ApplyRule(args, rst);
            }
            else if (rst.ResCode == ResultCode.OverdueToTemp)
            {
                //职责链中断后的辅助处理
                if (args.GateInfo.IoState == IoState.GoIn)
                {
                    var assistHandler = RuleHandlerFactory.Instance.CreateTempMainInRuleHandler();
                    rst.ResCode = ResultCode.Defaut;
                    assistHandler.ApplyRule(args, rst);
                }
                else
                {
                    var assistHandler = RuleHandlerFactory.Instance.CreateTempMainOutRuleHandler();
                    rst.ResCode = ResultCode.Defaut;
                    assistHandler.ApplyRule(args, rst);
                }
            }
        }
         
        /// <summary>
        /// 是否军警免费车辆
        /// </summary>
        /// <param name="args"></param>
        /// <param name="rst"></param>
        public void IsPoliceFree(InputAgs args, ResultAgs rst)
        {
            if (args.Plateinfo.LicenseNum.Length > 1 && args.AreadInfo.Parkinfo.PoliceFree)
            {
                foreach (var item in FreeCarStartContains)
                {
                    if (args.Plateinfo.LicenseNum.StartsWith(item))
                    {
                        rst.IsPoliceFree = true;
                        rst.ValidMsg = "军警免费车辆";
                        break;
                    }
                }
                foreach (var item in FreeCarEndContains)
                {
                    if (args.Plateinfo.LicenseNum.Contains(item))
                    {
                        rst.IsPoliceFree = true;
                        rst.ValidMsg = "军警免费车辆";
                        break;
                    }
                }
            }
        }
    }
}
