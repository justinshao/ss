using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.DataAccess;
using Common.Entities;

namespace Common.IRepository.Park
{
    public interface IParkMonthlyCarApply
    {
        /// <summary>
        /// 提交申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Add(ParkMonthlyCarApply model);
        /// <summary>
        /// 重新提交申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AgainApply(ParkMonthlyCarApply model);
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="recrdId"></param>
        /// <returns></returns>
        bool Cancel(string recordId);
        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="dbOperator"></param>
        /// <returns></returns>
        bool Passed(ParkMonthlyCarApply monthlyCarApply, DbOperator dbOperator);
        /// <summary>
        /// 拒绝
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="remark">拒绝原因</param>
        /// <returns></returns>
        bool Refused(string recordId,string remark);

        ParkMonthlyCarApply QueryByRecordID(string recordId);

        List<ParkMonthlyCarApply> QueryByAccountID(string accountId);

        List<ParkMonthlyCarApply> QueryPage(List<string> parkingIds, string applyInfo, MonthlyCarApplyStatus? Status, DateTime start, DateTime end, int pagesize, int pageindex, out int total);
    }
}
