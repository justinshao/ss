using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataAccess;

namespace Common.IRepository.Park
{
    public interface IEmployeePlate
    {
        bool Add(EmployeePlate model);

        bool Add(EmployeePlate model, DbOperator dbOperator);

        bool Update(EmployeePlate model);

        bool Update(EmployeePlate model, DbOperator dbOperator);

        bool Update(string plateId, string plateNumber, PlateColor color, DbOperator dbOperator);

        bool Delete(string plateId, DbOperator dbOperator);

        bool DeleteByEmployeeId(string employeeId, DbOperator dbOperator);

        List<EmployeePlate> QueryByEmployeeId(string employeeId);

        EmployeePlate Query(string plateId);

        EmployeePlate QueryByEmployeeIdAndPlateNumber(string employeeId, string plateNumber);

        EmployeePlate GetEmployeePlateNumberByPlateNumber(string villageid, string plateNumber, out string ErrorMessage);

        EmployeePlate GetPlateNumber(string villageid, string plateId, out string ErrorMessage);

        EmployeePlate GetGrantPlateNumberByLike(string villageid, string plateNumber, out string ErrorMessage);
         
        List<EmployeePlate> GetGrantPlateNumberListByLike(string villageid, string plateNumber, out string ErrorMessage);

    }
}
