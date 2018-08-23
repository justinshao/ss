using System;
using System.Diagnostics; 
using Common.Entities;
using Common.Entities.Validation;
using Common.Utilities.Helpers;

namespace Common.CarInOutValidation
{
    /// <summary>
    /// 验证模块
    /// </summary>
    public class ValidationTemplate
    {
        public ResultAgs Process(InputAgs args)
        {
            var watch = new Stopwatch();
            watch.Start();

            try
            {
                ValidationFactory contextFactory = new ValidationFactory();
                //
                var rst = contextFactory.Process(args);
                return rst;
            }
            catch (Exception ex)
            { 
                LogerHelper.Loger.Error(string.Format("进出场流程处理的验证模块异常({0})", this.ToString()), ex);
                return new ResultAgs { ResCode = ResultCode.LocalError };
            }
            finally
            {
                watch.Stop();
                LogerHelper.Loger.Debug(string.Format("车辆({0})的验证步骤运行时间：{1}ms", args.Plateinfo.LicenseNum, watch.ElapsedMilliseconds));
            }
        }
    }
}
