using System.ComponentModel;

namespace Common.Entities.Validation
{
    public enum ProcessValidInfo
    {
        /// <summary>
        /// 进验证通过
        /// </summary>
        [Description("进验证通过")]
        InYes = 1,
        /// <summary>
        /// 进验证失败
        /// </summary>
        [Description("进验证失败")]
        InNo = 2,
        /// <summary>
        /// 出验证通过
        /// </summary>
        [Description("出验证通过")]
        OutYes = 3,
        /// <summary>
        ///出验证失败
        /// </summary>
        [Description("出验证失败")]
        OutNo = 4,
        /// <summary>
        /// 需要模糊识别
        /// </summary>
        [Description("需要模糊识别")]
        FuzzyRecognition = 5,
        /// <summary>
        /// 主出出场时间小于入场时间
        /// </summary>
        [Description("主出出场时间小于入场时间")]
        MainExitUnTime = 6,
        /// <summary>
        /// 确认出场
        /// </summary>
        [Description("确认出场")]
        MainExitFreeOut = 7,
        /// <summary>
        /// 主出待支付现金
        /// </summary>
        [Description("主出待支付现金")]
        MainExitNeedPay = 8,
        /// <summary>
        /// 确认入场
        /// </summary>
        [Description("确认入场")]
        MainEnterConfirm = 9,
    }
}
