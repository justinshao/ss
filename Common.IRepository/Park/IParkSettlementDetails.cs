using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Entities.Parking;
using Common.DataAccess;

namespace Common.IRepository.Park
{
    public interface IParkSettlementDetails
    {
        bool Add(ParkSettlementDetailsModel model, DbOperator dbOperator);
    }
}
