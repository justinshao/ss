using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.DataAccess;

namespace Common.IRepository.Park
{
    public interface  IParkCarTypeSingle
    {
        bool Add(ParkCarTypeSingle model);
        bool Add(List<ParkCarTypeSingle> models, DbOperator dbOperator);
        bool Update(ParkCarTypeSingle model);
        List<ParkCarTypeSingle> QueryParkCarTypeByCarTypeID(string CarTypeID);
    }
}
