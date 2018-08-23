using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.IRepository.Park;
using Common.Entities.Parking;
using Common.DataAccess;
using Common.Factory.Park;
namespace Common.Services.Park
{
    public class ParkSettlementDetailsService
    {
        public static bool Add(ParkSettlementDetailsModel mode, DbOperator dbOperator)
        {
            if (mode == null) throw new ArgumentNullException("mode");
            IParkSettlementDetails factory = ParkSettlementDetailsFactory.GetFactory();
            return factory.Add(mode, dbOperator);
        }
    }
}
