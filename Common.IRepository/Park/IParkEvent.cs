using Common.Entities.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.IRepository.Park
{
    public interface IParkEvent
    {
        ParkEvent AddEventRec(ParkEvent mode, out string ErrorMessage);
        List<ParkEvent> GetEventRecYC(DateTime startTime, DateTime endtime);

        List<ParkEvent> GetEventRecWhitPageTab(string parkingID, int pageSize, int pageIndex, string carTypeID, string carModelid, string operatorID, string gateID, int eventID, DateTime startTime, DateTime endTime, int iostate, string likePlateNumber, out int pageCount);
    }
}
