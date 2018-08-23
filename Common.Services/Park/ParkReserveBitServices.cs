using Common.Entities.Parking;
using Common.Factory.Park;
using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Services.Park
{
    public class ParkReserveBitServices
    {
        public static ParkReserveBit GetCanUseParkReserveBit(string pkid,string platenumber, out string errorMsg)
        {
            if (pkid.IsEmpty()) throw new ArgumentNullException("pkid");

            IParkReserveBit factory = ParkReserveBitFactory.GetFactory();
            return factory.GetCanUseParkReserveBit(pkid, platenumber, out errorMsg);
        }

        public static bool ModifyReserveBit(string ReserveBitID, int status, out string errorMsg)
        {
            if (ReserveBitID.IsEmpty()) throw new ArgumentNullException("ReserveBitID");

            IParkReserveBit factory = ParkReserveBitFactory.GetFactory();
            return factory.ModifyReserveBit(ReserveBitID, status, out errorMsg);
        }
    }
}
