using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;

namespace Common.IRepository
{
    public interface IBaseEmployee
    {
        bool Add(BaseEmployee model);

        bool Add(BaseEmployee model, DbOperator dbOperator);

        bool Update(BaseEmployee model);

        bool Update(BaseEmployee model, DbOperator dbOperator);

        List<BaseEmployee> QueryEmployeeByVillageId(string villageId);

        BaseEmployee QueryBaseEmployeeByVillageIdAndMobile(string villageId, string mobile);

        BaseEmployee QueryByEmployeeId(string employeeId);

        bool LogoutEmployee(string employeeId, string logoutreason);

        bool Delete(string employeeId);

        bool Delete(string employeeId, DbOperator dbOperator);

        List<BaseEmployee> QueryEmployeeByHrdeptId(string hrdeptId);

        List<BaseEmployee> QueryEmployeePage(EmployeeCondition condition, int pagesize, int pageindex, out int total);
    }
}
