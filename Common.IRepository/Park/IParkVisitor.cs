using Common.Entities.Parking;
using Common.Entities.WX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataAccess;
using Common.Entities.Condition;

namespace Common.IRepository.Park
{
    public interface IParkVisitor
    {
        bool Add(VisitorInfo model);

        List<VisitorInfo> GetVisitorInfoPage(string operatorId, int pageIndex, int pageSize, out int total);

        bool CancelVisitor(string visitorId);

        VisitorInfo GetVisitor(string parkingID, string platenumber, out string ErrorMessage);

        VisitorInfo GetNormalVisitor(string parkingID, string platenumber, out string ErrorMessage);
        
        ParkVisitor GetVisitorCar(string parkingID, string visitorID, out string ErrorMessage);

        bool ModifyVisitorCar(ParkVisitor mode, out string ErrorMessage);

        bool EditVisitorInfo(VisitorInfo model);

        List<VisitorInfo> QueryPage(VisitorInfoCondition condition, int pagesize, int pageindex, out int total);
        List<VisitorInfo> QueryPageIn(Entities.Condition.VisitorInfoCondition condition, int pagesize, int pageindex, out int total);
        List<VisitorInfo> QueryPageInOut(Entities.Condition.VisitorInfoCondition condition, int pagesize, int pageindex, out int total);
    }
}
