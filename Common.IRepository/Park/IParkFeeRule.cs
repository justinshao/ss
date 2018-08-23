 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.DataAccess;

namespace Common.IRepository
{
    public interface IParkFeeRule
    {

        bool Add(ParkFeeRule model);

        bool Update(ParkFeeRule model);

        bool Delete(string feeRuleId);

        List<ParkFeeRule> QueryFeeRuleByCarModelAndCarType(string areaId, string carModelId, string carTypeId);

        ParkFeeRule QueryParkFeeRuleByFeeRuleId(string feeRuleId);

        List<ParkFeeRule> QueryParkFeeRuleByParkingId(string parkingId);

        List<ParkFeeRule> QueryFeeRules(string parkingId, string carTypeId, string carModelId);

        List<ParkFeeRuleDetail> QueryFeeRuleDetailByFeeRuleId(string feeRuleId);
        ParkFeeRule QueryParkFeeRuleByFeeIsOffline(string PKID); 
    }
}
