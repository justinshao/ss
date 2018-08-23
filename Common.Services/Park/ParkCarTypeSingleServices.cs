using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.IRepository.Park;
using Common.Factory.Park;
using Common.Utilities;
using Common.Entities;
using Common.DataAccess;

namespace Common.Services.Park
{
    public class ParkCarTypeSingleServices
    {
        public static bool Add(ParkCarTypeSingle model)
        {
            if (model == null) throw new ArgumentNullException("model");


            IParkCarTypeSingle factory = ParkCarTypeSingleFactory.GetFactory();

            model.SingleID = GuidGenerator.GetGuidString();

            bool result = factory.Add(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkCarTypeSingle>(model, OperateType.Add);
            }
            return result;
        }

        public static bool AddDefault(string CarTypeID, DbOperator dbOperator)
        {
            if (string.IsNullOrWhiteSpace(CarTypeID)) throw new ArgumentNullException("CarTypeID");

            IParkCarTypeSingle factory = ParkCarTypeSingleFactory.GetFactory();
            return factory.Add(GetDefaultParkCarTypeSingles(CarTypeID), dbOperator);
        }
        public static bool AddDefault(string CarTypeID)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                if (string.IsNullOrWhiteSpace(CarTypeID)) throw new ArgumentNullException("CarTypeID");

                IParkCarTypeSingle factory = ParkCarTypeSingleFactory.GetFactory();
                return factory.Add(GetDefaultParkCarTypeSingles(CarTypeID), dbOperator);
            }
        }
        private static List<ParkCarTypeSingle> GetDefaultParkCarTypeSingles(string CarTypeID)
        {
            List<ParkCarTypeSingle> models = new List<ParkCarTypeSingle>();
            models.Add(new ParkCarTypeSingle()
            {
                SingleID = GuidGenerator.GetGuidString(),
                CarTypeID = CarTypeID,
                Week = 1,
                SingleType = 0
            });
            models.Add(new ParkCarTypeSingle()
            {
                SingleID = GuidGenerator.GetGuidString(),
                CarTypeID = CarTypeID,
                Week = 2,
                SingleType = 0
            });
            models.Add(new ParkCarTypeSingle()
            {
                SingleID = GuidGenerator.GetGuidString(),
                CarTypeID = CarTypeID,
                Week = 3,
                SingleType = 0
            });
            models.Add(new ParkCarTypeSingle()
            {
                SingleID = GuidGenerator.GetGuidString(),
                CarTypeID = CarTypeID,
                Week = 4,
                SingleType = 0
            });
            models.Add(new ParkCarTypeSingle()
            {
                SingleID = GuidGenerator.GetGuidString(),
                CarTypeID = CarTypeID,
                Week = 5,
                SingleType = 0
            });
            models.Add(new ParkCarTypeSingle()
            {
                SingleID = GuidGenerator.GetGuidString(),
                CarTypeID = CarTypeID,
                Week = 6,
                SingleType = 0
            });
            models.Add(new ParkCarTypeSingle()
            {
                SingleID = GuidGenerator.GetGuidString(),
                CarTypeID = CarTypeID,
                Week = 7,
                SingleType = 0
            });
     
            return models;
        }

        public static bool Update(ParkCarTypeSingle model)
        {
            if (model == null) throw new ArgumentNullException("model");

            IParkCarTypeSingle factory = ParkCarTypeSingleFactory.GetFactory();
            bool result = factory.Update(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkCarTypeSingle>(model, OperateType.Update);
            }
            return result;
        }

        public static List<ParkCarTypeSingle> QueryParkCarTypeByCarTypeID(string cardtypeID)
        {
            if (string.IsNullOrWhiteSpace(cardtypeID)) throw new ArgumentNullException("recordId");

            IParkCarTypeSingle factory = ParkCarTypeSingleFactory.GetFactory();
            return factory.QueryParkCarTypeByCarTypeID(cardtypeID);
        }
    }
}
