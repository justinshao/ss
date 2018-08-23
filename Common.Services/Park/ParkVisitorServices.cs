using Common.Entities.Parking;
using Common.Entities.WX;
using Common.Factory.Park;
using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Core.Expands;

namespace Common.Services.Park
{
    public class ParkVisitorServices
    {
        public static bool Add(VisitorInfo model) {
            if (model == null) throw new ArgumentNullException("model");
            try
            {
                IParkVisitor factory = ParkVisitorFactory.GetFactory();
                return factory.Add(model);
            }
            catch
            {
                throw;
            }
        }

        public static bool Edit(VisitorInfo model)
        {
            if (model == null) throw new ArgumentNullException("model");

            IParkVisitor factory = ParkVisitorFactory.GetFactory();
            return factory.EditVisitorInfo(model);
        }

        public static List<VisitorInfo> GetVisitorInfoPage(string operatorId, int pageIndex, int pageSize, out int total)
        {
            if (operatorId.IsEmpty()) throw new ArgumentNullException("operatorId");

            IParkVisitor factory = ParkVisitorFactory.GetFactory();
            return factory.GetVisitorInfoPage(operatorId, pageIndex,pageSize,out total);
        }

        public static List<VisitorInfo> QueryPage(Entities.Condition.VisitorInfoCondition condition, int pagesize, int pageindex, out int total)
        {
            if (condition == null) throw new ArgumentNullException("condition");

            IParkVisitor factory = ParkVisitorFactory.GetFactory();
            return factory.QueryPage(condition, pagesize, pageindex, out total);
        }

        public static List<VisitorInfo> QueryPageIn(Entities.Condition.VisitorInfoCondition condition, int pagesize, int pageindex, out int total)
        {
            if (condition == null) throw new ArgumentNullException("condition");

            IParkVisitor factory = ParkVisitorFactory.GetFactory();
            return factory.QueryPageIn(condition, pagesize, pageindex, out total);
        }

        public static List<VisitorInfo> QueryPageInOut(Entities.Condition.VisitorInfoCondition condition, int pagesize, int pageindex, out int total)
        {
            if (condition == null) throw new ArgumentNullException("condition");

            IParkVisitor factory = ParkVisitorFactory.GetFactory();
            return factory.QueryPageInOut(condition, pagesize, pageindex, out total);
        }

        public static VisitorInfo GetVisitor(string vid, string platenumber, out string ErrorMessage)
        {
            if (vid.IsEmpty()) throw new ArgumentNullException("vid");
            if (platenumber.IsEmpty()) throw new ArgumentNullException("platenumber");

            IParkVisitor factory = ParkVisitorFactory.GetFactory();
            return factory.GetVisitor(vid, platenumber, out ErrorMessage);
        }
        public static bool CancelVisitor(string visitorId) {
            if (visitorId.IsEmpty()) throw new ArgumentNullException("visitorId");

            IParkVisitor factory = ParkVisitorFactory.GetFactory();
            return factory.CancelVisitor(visitorId);
        }
        public static ParkVisitor GetVisitorCar(string parkingID, string visitorID, out string ErrorMessage)
        {
            if (parkingID.IsEmpty()) throw new ArgumentNullException("parkingID");
            if (visitorID.IsEmpty()) throw new ArgumentNullException("visitorID");

            IParkVisitor factory = ParkVisitorFactory.GetFactory();
            return factory.GetVisitorCar(parkingID, visitorID, out ErrorMessage);
        }
        public static bool ModifyVisitorCar(ParkVisitor mode, out string ErrorMessage)
        {
            if (mode == null) throw new ArgumentNullException("mode");

            IParkVisitor factory = ParkVisitorFactory.GetFactory();
            return factory.ModifyVisitorCar(mode, out ErrorMessage);
        }
        public static VisitorInfo GetNormalVisitor(string vid, string platenumber, out string ErrorMessage)
        {
            if (vid.IsEmpty()) throw new ArgumentNullException("vid");
            if (platenumber.IsEmpty()) throw new ArgumentNullException("platenumber");

            IParkVisitor factory = ParkVisitorFactory.GetFactory();
            return factory.GetNormalVisitor(vid, platenumber, out ErrorMessage);
        }
    }
}
