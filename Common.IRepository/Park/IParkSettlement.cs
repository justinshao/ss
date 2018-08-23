using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Entities.Parking;
using Common.DataAccess;
using Common.Entities.Statistics;
namespace Common.IRepository.Park
{
    public interface IParkSettlement
    {
        List<string> GetPriods(string PKID);
        ParkSettlementModel GetMaxPriodSettlement(string PKID);
        List<ParkSettlementModel> GetSettlements(string PKID, int SettleStatus, string Priod,string UserID);
        bool UpdateSettlementStatus(string RecordID, int SettleStatus);
        //List<ParkOrder> GetSettlementPayAmount(string PKID, DateTime StartTime, DateTime EndTime);
        List<Statistics_Gather> GetSettlementPayAmount(string PKID, DateTime StartTime, DateTime EndTime);
        bool BuildSettlement(ParkSettlementModel settlement);

        List<ParkSettlementModel> GetSettlements(IList<string> villIDs, int SettleStatus, string Priod, string userid);
    }
}
