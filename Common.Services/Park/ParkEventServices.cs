using Common.Entities.Parking;
using Common.Factory.Park;
using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Utilities;
using Common.Entities;

namespace Common.Services.Park
{
    public class ParkEventServices
    {
        public static ParkEvent AddEventRec(ParkEvent mode, out string ErrorMessage)
        {
            if (mode == null) throw new ArgumentNullException("mode");

            IParkEvent factory = ParkEventFactory.GetFactory();
            return factory.AddEventRec(mode, out ErrorMessage);
        }

        public static List<EnumContext> GetEventType()
        {
            return Common.Utilities.Helpers.EnumHelper.GetEnumContextList(typeof(ResultCode));
        }

        public static List<ParkEvent> GetEventRecYC(DateTime startTime, DateTime endtime)
        {
            IParkEvent factory = ParkEventFactory.GetFactory();
            return factory.GetEventRecYC(startTime, endtime);
        }

        public static List<ParkEvent> GetEventRecWhitPageTab(string parkingID, int pageSize, int pageIndex, string carTypeID, string carModelid, string operatorID, string gateID, int eventID, DateTime startTime, DateTime endTime, int iostate, string likePlateNumber, out int pageCount)
        {
            IParkEvent factory = ParkEventFactory.GetFactory();
            return factory.GetEventRecWhitPageTab(parkingID, pageSize, pageIndex, carTypeID, carModelid, operatorID, gateID, eventID, startTime, endTime, iostate, likePlateNumber, out pageCount);
        }
    }
}
