using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.Entities.Statistics;

namespace Common.IRepository.Park
{
    public interface IParkLpPlan
    {
        /// <summary>
        /// 根据条件获取派车信息总数
        /// </summary>
        /// <param name="paras"></param>
        /// <returns></returns>
        int Search_ParkLpPlansCount(InParams paras);

        /// <summary>
        /// 根据条件获取派车信息
        /// </summary>
        /// <param name="paras"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        List<ParkLpPlan> Search_ParkLpPlans(InParams paras, int PageSize, int PageIndex);

        /// <summary>
        /// 根据派车单ID获取派遣单
        /// </summary>
        /// <param name="PlanID"></param>
        /// <returns></returns>
        ParkLpPlan Search_ParkLpPlanByID(string PlanID);

        bool Add(ParkLpPlan model);

        bool Update(ParkLpPlan model);

        bool Delete(string PlanID);
    }
}
