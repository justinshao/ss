using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.Entities.Statistics;
using Common.IRepository.Park;
using Common.Factory.Park;
using Common.Utilities;
using Common.Entities;

namespace Common.Services.Park
{
    public class ParkLpPlanServicescs
    {
         public static List<ParkLpPlan> Search_ParkLpPlan(InParams paras, int PageSize, int PageIndex,ref int pagecount)
        {
            IParkLpPlan factory = ParkLpPlanFactory.GetFactory();
            pagecount = factory.Search_ParkLpPlansCount(paras);
            List<ParkLpPlan> list = factory.Search_ParkLpPlans(paras, PageSize, PageIndex);

            return list;
        }

         /// <summary>
        /// 根据派车单ID获取派遣单
        /// </summary>
        /// <param name="PlanID"></param>
        /// <returns></returns>
         public static ParkLpPlan Search_ParkLpPlanByID(string PlanID)
         {
             if (string.IsNullOrWhiteSpace(PlanID)) throw new ArgumentNullException("PlanID");
             IParkLpPlan factory = ParkLpPlanFactory.GetFactory();
             ParkLpPlan model = factory.Search_ParkLpPlanByID(PlanID);
             return model;
         }

         public static bool Add(ParkLpPlan model)
         {
             if (model == null) throw new ArgumentNullException("model");

             model.PlanID = GuidGenerator.GetGuid().ToString();
             IParkLpPlan factory = ParkLpPlanFactory.GetFactory();
             bool result = factory.Add(model);
             if (result)
             {
                 OperateLogServices.AddOperateLog<ParkLpPlan>(model, OperateType.Add);
             }
             return result;
         }

         public static bool Update(ParkLpPlan model)
         {
             if (model == null) throw new ArgumentNullException("model");

             IParkLpPlan factory = ParkLpPlanFactory.GetFactory();
             bool result = factory.Update(model);
             if (result)
             {
                 OperateLogServices.AddOperateLog<ParkLpPlan>(model, OperateType.Update);
             }
             return result;
         }

         public static bool Delete(string PlanID)
         {
             if (string.IsNullOrWhiteSpace(PlanID)) throw new ArgumentNullException("PlanID");

             IParkLpPlan factory = ParkLpPlanFactory.GetFactory();
             bool result = factory.Delete(PlanID);
             if (result)
             {
                 OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("recordId:{0}", PlanID));
             }
             return result;
         }
    }
}
