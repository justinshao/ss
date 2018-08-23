using Common.Entities.Parking;
using Common.Factory.Park;
using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Statistics;

namespace Common.Services.Park
{
    public class ParkChangeshiftrecordServices
    {
        public static ParkChangeshiftrecord GetUnChangeshiftrecord(string recordid,out string errorMsg)
        {
            if (string.IsNullOrWhiteSpace(recordid)) throw new ArgumentNullException("recordid");

            IParkChangeshiftrecord factory = ParkChangeshiftrecordFactory.GetFactory();
            return factory.GetUnChangeshiftrecord(recordid, out errorMsg);
        }

        public static ParkChangeshiftrecord AddChangeshiftrecord(ParkChangeshiftrecord mode, out string errorMsg)
        {
            if (mode == null) throw new ArgumentNullException("mode");

            IParkChangeshiftrecord factory = ParkChangeshiftrecordFactory.GetFactory();
            return factory.AddChangeshiftrecord(mode, out errorMsg);
        }

        public static bool ModifyChangeshiftrecord(ParkChangeshiftrecord mode, out string errorMsg)
        {
            if (mode == null) throw new ArgumentNullException("mode");

            IParkChangeshiftrecord factory = ParkChangeshiftrecordFactory.GetFactory();
            return factory.ModifyChangeshiftrecord(mode, out errorMsg);
        }

        public static bool EditChangeshiftrecord(string RecordID, DateTime EndWorkTime, out string errorMsg)
        {
            if (RecordID == null) throw new ArgumentNullException("mode");

            IParkChangeshiftrecord factory = ParkChangeshiftrecordFactory.GetFactory();
            return factory.EditChangeshiftrecord(RecordID,EndWorkTime, out errorMsg);
        }

        public static List<ParkChangeshiftrecord> GetUnChangeshiftrecordALL()
        {
            IParkChangeshiftrecord factory = ParkChangeshiftrecordFactory.GetFactory();
            return factory.GetUnChangeshiftrecordALL();
        }

        public static List<ParkChangeshiftrecord> GetChangeShiftByUserID(string UserID, DateTime begin, DateTime end)
        {
            IParkChangeshiftrecord factory = ParkChangeshiftrecordFactory.GetFactory();
            return factory.GetChangeShiftByUserID( UserID,  begin,  end);
        }


        public static List<OnDutyFL> GetOnDutyByUserID(string UserID, DateTime begin, DateTime end)
        {
            IParkChangeshiftrecord factory = ParkChangeshiftrecordFactory.GetFactory();
            return factory.GetOnDutyByUserID(UserID, begin, end);
        }
    }
}
