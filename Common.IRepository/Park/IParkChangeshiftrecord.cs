using Common.Entities.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Statistics;

namespace Common.IRepository.Park
{
    public interface IParkChangeshiftrecord
    {
        ParkChangeshiftrecord GetUnChangeshiftrecord(string recordid, out string errorMsg);

        ParkChangeshiftrecord AddChangeshiftrecord(ParkChangeshiftrecord mode, out string errorMsg);

        bool ModifyChangeshiftrecord(ParkChangeshiftrecord mode, out string errorMsg);

        List<ParkChangeshiftrecord> GetChangeShiftRecord(string boxid);

        bool EditChangeshiftrecord(string RecordID, DateTime EndWorkTime, out string errorMsg);

       List<ParkChangeshiftrecord> GetUnChangeshiftrecordALL();
       List<ParkChangeshiftrecord> GetChangeShiftByUserID(string UserID, DateTime begin, DateTime end);

       List<OnDutyFL> GetOnDutyByUserID(string UserID, DateTime begin, DateTime end);
    }
}
