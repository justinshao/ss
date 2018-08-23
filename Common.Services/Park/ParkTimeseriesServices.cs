using Common.Entities;
using Common.Entities.Parking;
using Common.Factory.Park;
using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Services.Park
{
    public class ParkTimeseriesServices
    {
        public static ParkTimeseries GetTimeseriesesByIORecordID(string parkid, string iorecordID, out string ErrorMessage)
        {
            if (string.IsNullOrWhiteSpace(parkid)) throw new ArgumentNullException("parkid");

            IParkTimeseries factory = ParkTimeseriesFactory.GetFactory();
            return factory.GetTimeseriesesByIORecordID(parkid, iorecordID, out ErrorMessage);
        }

        public static bool RemoveTimeseries(string timeseriesid)
        {
            if (string.IsNullOrWhiteSpace(timeseriesid)) throw new ArgumentNullException("timeseriesid");

            IParkTimeseries factory = ParkTimeseriesFactory.GetFactory();
            return factory.RemoveTimeseries(timeseriesid);
        }

        public static int GetAreaCarNum(string areaID, BaseCarType carType, out string ErrorMessage)
        {
            if (string.IsNullOrWhiteSpace(areaID)) throw new ArgumentNullException("areaID");

            IParkTimeseries factory = ParkTimeseriesFactory.GetFactory();
            return factory.GetAreaCarNum(areaID, carType, out ErrorMessage);
        }

        public static int GetIsEditCarNum(string areaID, out string ErrorMessage)
        {
            if (string.IsNullOrWhiteSpace(areaID)) throw new ArgumentNullException("areaID");

            IParkTimeseries factory = ParkTimeseriesFactory.GetFactory();
            return factory.GetIsEditCarNum(areaID, out ErrorMessage);
        }

         
        public static int GetAreaCarNum(string areaID, out string ErrorMessage)
        {
            if (string.IsNullOrWhiteSpace(areaID)) throw new ArgumentNullException("areaID");

            IParkTimeseries factory = ParkTimeseriesFactory.GetFactory();
            return factory.GetAreaCarNum(areaID, out ErrorMessage);
        }
        public static DateTime GetLastRecordExitDate(string parkingID, string iorecordid, out string ErrorMessage)
        {
            if (string.IsNullOrWhiteSpace(parkingID)) throw new ArgumentNullException("parkingID");
            if (string.IsNullOrWhiteSpace(iorecordid)) throw new ArgumentNullException("iorecordid");

            IParkTimeseries factory = ParkTimeseriesFactory.GetFactory();
            return factory.GetLastRecordExitDate(parkingID, iorecordid, out ErrorMessage);
        }
        public static DateTime GetLastRecordEnterTime(string parkingID, string iorecordid, out string ErrorMessage)
        {
            if (string.IsNullOrWhiteSpace(parkingID)) throw new ArgumentNullException("parkingID");
            if (string.IsNullOrWhiteSpace(iorecordid)) throw new ArgumentNullException("iorecordid");

            IParkTimeseries factory = ParkTimeseriesFactory.GetFactory();
            return factory.GetLastRecordEnterTime(parkingID, iorecordid, out ErrorMessage);
        }

        public static bool ModifyTimeseries(ParkTimeseries mode, out string ErrorMessage)
        {
            if (mode == null) throw new ArgumentNullException("mode");

            IParkTimeseries factory = ParkTimeseriesFactory.GetFactory();
            return factory.ModifyTimeseries(mode, out ErrorMessage);
        }
        public static ParkTimeseries AddTimeseries(ParkTimeseries mode, out string ErrorMessage)
        {
            if (mode == null) throw new ArgumentNullException("mode");

            IParkTimeseries factory = ParkTimeseriesFactory.GetFactory();
            return factory.AddTimeseries(mode, out ErrorMessage);
        }
        public static List<ParkTimeseries> GetAllExitsTimeseriesesByIORecordID(string parkid, string iorecordID, out string ErrorMessage)
        {
            if (string.IsNullOrWhiteSpace(parkid)) throw new ArgumentNullException("parkid");

            IParkTimeseries factory = ParkTimeseriesFactory.GetFactory();
            return factory.GetAllExitsTimeseriesesByIORecordID(parkid, iorecordID, out ErrorMessage);
        }
        public static List<ParkTimeseries> GetTimeseriesIORecordWhitPageTab(string parkingID, int pageSize, int pageIndex, List<string> cardTypeIDs, string carTypeID, string likePlateNumber, string ingateid, string outgateid, DateTime startTime, DateTime endTime, out int pageCount, out string ErrorMessage, int isExit = 0, int stayDay = -1)
        {
            if (string.IsNullOrWhiteSpace(parkingID)) throw new ArgumentNullException("parkingID");

            IParkTimeseries factory = ParkTimeseriesFactory.GetFactory();
            return factory.GetTimeseriesIORecordWhitPageTab(parkingID, pageSize, pageIndex, cardTypeIDs, carTypeID, likePlateNumber, ingateid, outgateid, startTime, endTime,out pageCount,out ErrorMessage,isExit,stayDay);
        }
        public static ParkTimeseries GetTimeseries(string timeseriesID, out string ErrorMessage)
        {
            if (string.IsNullOrWhiteSpace(timeseriesID)) throw new ArgumentNullException("timeseriesID");

            IParkTimeseries factory = ParkTimeseriesFactory.GetFactory();
            return factory.GetTimeseries(timeseriesID, out ErrorMessage);
        }
        public static List<ParkTimeseries> GetTimeseriesIORecordByLikeStrWhitPageTab(string parkingID, int pageSize, int pageIndex, List<string> cardTypeIDs, string likePlateNumberStr, out int pageCount, out string ErrorMessage, int isExit = 0)
        {
            if (parkingID.IsEmpty()) throw new ArgumentNullException("parkingID");

            IParkTimeseries factory = ParkTimeseriesFactory.GetFactory();
            return factory.GetTimeseriesIORecordByLikeStrWhitPageTab(parkingID, pageSize, pageIndex, cardTypeIDs, likePlateNumberStr, out pageCount, out ErrorMessage, isExit);
        }
    }
}
