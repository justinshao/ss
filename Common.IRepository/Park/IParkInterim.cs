using Common.Entities.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.IRepository.Park
{
    public interface IParkInterim
    {
        List<ParkInterim> GetInterimByIOrecord(string recordID, out string ErrorMessage);
        bool ModifyInterim(ParkInterim mode, out string ErrorMessage);
        ParkInterim AddInterim(ParkInterim mode, out string ErrorMessage);
        bool RemoveByIORecordId(string recordid);
    }
}
