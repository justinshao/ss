using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Core.Expands;
using Common.IRepository.Park;
using Common.Factory.Park;
using Common.Utilities;
using Common.DataAccess;

namespace Common.Services.Park
{
    public class EmployeePlateServices
    {
        public static bool Add(EmployeePlate model) {
            if (model == null) throw new ArgumentNullException("model");

            model.PlateID = GuidGenerator.GetGuidString();
            IEmployeePlate factory = EmployeePlateFactory.GetFactory();
            return factory.Add(model);
        }

        public static bool Update(EmployeePlate model)
        {
            if (model == null) throw new ArgumentNullException("model");

            IEmployeePlate factory = EmployeePlateFactory.GetFactory();
            return factory.Update(model);
        }

        public static bool Update(string plateId, string plateNumber, PlateColor color) {
            if (string.IsNullOrWhiteSpace(plateId)) throw new ArgumentNullException("plateId");
            if (string.IsNullOrWhiteSpace(plateNumber)) throw new ArgumentNullException("plateNumber");

            IEmployeePlate factory = EmployeePlateFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
               return factory.Update(plateId, plateNumber, color, dbOperator);
            }
        }

        public static bool Delete(string plateId) {
            if (string.IsNullOrWhiteSpace(plateId)) throw new ArgumentNullException("plateId");

            IEmployeePlate factory = EmployeePlateFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                return factory.Delete(plateId, dbOperator);
            }
        }

        public static List<EmployeePlate> QueryByEmployeeId(string employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId)) throw new ArgumentNullException("employeeId");

            IEmployeePlate factory = EmployeePlateFactory.GetFactory();
            return factory.QueryByEmployeeId(employeeId);
        }

        public static EmployeePlate Query(string plateId)
        {
            if (string.IsNullOrWhiteSpace(plateId)) throw new ArgumentNullException("plateId");

            IEmployeePlate factory = EmployeePlateFactory.GetFactory();
            return factory.Query(plateId);
        }

        public static EmployeePlate QueryByEmployeeIdAndPlateNumber(string employeeId, string plateNumber)
        {
            if (string.IsNullOrWhiteSpace(employeeId)) throw new ArgumentNullException("employeeId");
            if (string.IsNullOrWhiteSpace(plateNumber)) throw new ArgumentNullException("plateNumber");

            IEmployeePlate factory = EmployeePlateFactory.GetFactory();
            return factory.QueryByEmployeeIdAndPlateNumber(employeeId, plateNumber);
        }

        public static EmployeePlate GetEmployeePlateNumberByPlateNumber(string vid, string plateNumber, out string ErrorMessage)
        {
            if (plateNumber.IsEmpty()) throw new ArgumentNullException("plateNumber");

            IEmployeePlate factory = EmployeePlateFactory.GetFactory();
            return factory.GetEmployeePlateNumberByPlateNumber(vid, plateNumber, out ErrorMessage);
        }
        public static EmployeePlate GetPlateNumber(string vid, string plateID, out string ErrorMessage)
        {
            if (plateID.IsEmpty()) throw new ArgumentNullException("plateID");

            IEmployeePlate factory = EmployeePlateFactory.GetFactory();
            return factory.GetPlateNumber(vid,plateID, out ErrorMessage);
        }
        public static EmployeePlate GetGrantPlateNumberByLike(string vid, string plateNumber, out string ErrorMessage)
        {
            if (plateNumber.IsEmpty()) throw new ArgumentNullException("plateNumber");

            IEmployeePlate factory = EmployeePlateFactory.GetFactory();
            return factory.GetGrantPlateNumberByLike(vid,plateNumber, out ErrorMessage);
        }

        public static List<EmployeePlate> GetGrantPlateNumberListByLike(string vid, string plateNumber, out string ErrorMessage)
        {
            if (plateNumber.IsEmpty()) throw new ArgumentNullException("plateNumber");

            IEmployeePlate factory = EmployeePlateFactory.GetFactory();
            return factory.GetGrantPlateNumberListByLike(vid, plateNumber, out ErrorMessage);
        }
        public static bool AddOrUpdateEmployeePlate(EmployeePlate plate,DbOperator dbOperator)
        {
            IEmployeePlate plateFactory = EmployeePlateFactory.GetFactory();
            EmployeePlate oldPlate = plateFactory.Query(plate.PlateID);
            if (oldPlate == null)
            {
                return plateFactory.Add(plate, dbOperator);
            }
            else
            {
                oldPlate.Color = plate.Color;
                oldPlate.PlateNo = plate.PlateNo;
                oldPlate.EmployeeID = plate.EmployeeID;
                plate = oldPlate;
                return plateFactory.Update(plate, dbOperator);
            }

        }
    }
}
