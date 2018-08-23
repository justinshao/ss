using Common.Entities.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Core.Expands;
using Common.IRepository.Park;
using Common.Factory.Park;

namespace Common.Services.Park
{
    public class ParkInterimServices
    {
        public static List<ParkInterim> GetInterimByIOrecord(string recordID, out string ErrorMessage)
        {
            if (recordID.IsEmpty()) throw new ArgumentNullException("recordID");

            IParkInterim factory = ParkInterimFactory.GetFactory();
            return factory.GetInterimByIOrecord(recordID, out ErrorMessage);
        }
        public static bool ModifyInterim(ParkInterim mode, out string ErrorMessage)
        {
            if (mode == null) throw new ArgumentNullException("mode");

            IParkInterim factory = ParkInterimFactory.GetFactory();
            return factory.ModifyInterim(mode, out ErrorMessage);
        }
        public static ParkInterim AddInterim(ParkInterim mode, out string ErrorMessage)
        {
            if (mode == null) throw new ArgumentNullException("mode");

            IParkInterim factory = ParkInterimFactory.GetFactory();
            return factory.AddInterim(mode, out ErrorMessage);
        }
    }
}
