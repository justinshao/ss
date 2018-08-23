using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Factory.BaseData;
using Common.IRepository;
using Common.Utilities;
using Common.DataAccess;
using Common.Factory.Park;
using Common.IRepository.Park;

namespace Common.Services
{
    public class BaseEmployeeServices
    {
        public static bool Add(BaseEmployee model) {
            if (model == null) throw new ArgumentNullException("model");

            IBaseEmployee factory = BaseEmployeeFactory.GetFactory();
            model.EmployeeID = GuidGenerator.GetGuidString();
            return factory.Add(model);
        }

        public static bool Update(BaseEmployee model) {
            if (model == null) throw new ArgumentNullException("model");
            if (string.IsNullOrWhiteSpace(model.EmployeeID)) throw new ArgumentNullException("EmployeeID");

            IBaseEmployee factory = BaseEmployeeFactory.GetFactory();
            return factory.Update(model);
        }

   
       public static List<BaseEmployee> QueryEmployeeByVillageId(string villageId)
        {
            if (string.IsNullOrWhiteSpace(villageId)) throw new ArgumentNullException("villageId");

            IBaseEmployee factory = BaseEmployeeFactory.GetFactory();
            return factory.QueryEmployeeByVillageId(villageId);
        }

       public static BaseEmployee QueryBaseEmployeeByVillageIdAndMobile(string villageId, string mobile)
       {
           if (string.IsNullOrWhiteSpace(villageId)) throw new ArgumentNullException("villageId");
           if (string.IsNullOrWhiteSpace(mobile)) throw new ArgumentNullException("mobile");

           IBaseEmployee factory = BaseEmployeeFactory.GetFactory();
           return factory.QueryBaseEmployeeByVillageIdAndMobile(villageId, mobile);
       }

       public static BaseEmployee QueryByEmployeeId(string employeeId)
       {
           if (string.IsNullOrWhiteSpace(employeeId)) throw new ArgumentNullException("employeeId");

           IBaseEmployee factory = BaseEmployeeFactory.GetFactory();
           return factory.QueryByEmployeeId(employeeId);
       }

       public static bool Delete(string employeeId)
       {
           if (string.IsNullOrWhiteSpace(employeeId)) throw new ArgumentNullException("employeeId");
           IBaseEmployee factory = BaseEmployeeFactory.GetFactory();
           IEmployeePlate platefactory = EmployeePlateFactory.GetFactory();
           using (DbOperator dbOperator = ConnectionManager.CreateConnection())
           {
               try
               {
                   dbOperator.BeginTransaction();
                   //删除车牌
                  bool result = platefactory.DeleteByEmployeeId(employeeId, dbOperator);
                  if (!result) throw new MyException("删除车牌失败");
                   //删授权表
                   result = factory.Delete(employeeId, dbOperator);
                   if (!result) throw new MyException("删除人员信息失败");
                   dbOperator.CommitTransaction();
                   return result;
               }
               catch {
                   dbOperator.RollbackTransaction();
                   throw;
               }
           }
       }

       public static List<BaseEmployee> QueryEmployeeByHrdeptId(string hrdeptId)
       {
           if (string.IsNullOrWhiteSpace(hrdeptId)) throw new ArgumentNullException("hrdeptId");

           IBaseEmployee factory = BaseEmployeeFactory.GetFactory();
           return factory.QueryEmployeeByHrdeptId(hrdeptId);
       }

       public static List<BaseEmployee> QueryEmployeePage(EmployeeCondition condition, int pagesize, int pageindex, out int total)
       {
           if (condition == null) throw new ArgumentNullException("condition");
           if (string.IsNullOrWhiteSpace(condition.VillageId)) throw new ArgumentNullException("VillageId");

           IBaseEmployee factory = BaseEmployeeFactory.GetFactory();
           return factory.QueryEmployeePage(condition,pagesize,pageindex,out total);
       }
       public static bool AddOrUpdateBaseEmployee(BaseEmployee employee, DbOperator dbOperator)
       {
           IBaseEmployee employeeFactory = BaseEmployeeFactory.GetFactory();
           BaseEmployee oldEmployee = BaseEmployeeServices.QueryByEmployeeId(employee.EmployeeID);
           if (oldEmployee != null)
           {
               return employeeFactory.Update(employee, dbOperator);
           }
           else
           {
               return employeeFactory.Add(employee, dbOperator);
           }
       }
    }
}
