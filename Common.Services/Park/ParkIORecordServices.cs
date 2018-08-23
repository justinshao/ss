using Common.Entities;
using Common.Entities.Parking;
using Common.Factory.Park;
using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Core.Expands;

namespace Common.Services.Park
{
    public class ParkIORecordServices
    {

        /// <summary>
        /// 查询车牌时段内的记录
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="platenumber"></param>
        /// <param name="parkingId"></param>
        /// <param name="IsExit">false时 end无效</param>
        /// <returns></returns>
        public static List<ParkIORecord> QueryPlatenumberIORecordByTime(DateTime start, DateTime end, string platenumber, string parkingId, bool IsExit)
        {
            if (start == null) throw new ArgumentNullException("start");
            if (end == null) throw new ArgumentNullException("end");
            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.QueryPlatenumberIORecordByTime(start, end, platenumber, parkingId, IsExit);
        }

        public static ParkIORecord AddIORecord(ParkIORecord mode, out string ErrorMessage)
        {
            if (mode == null) throw new ArgumentNullException("mode");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.AddIORecord(mode, out ErrorMessage);
        }
        public static bool ModifyIORecord(ParkIORecord mode, out string ErrorMessage)
        {
            if (mode == null) throw new ArgumentNullException("mode");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.ModifyIORecord(mode, out ErrorMessage);
        }
        public static ParkIORecord GetNoExitIORecordByCardNo(string parkid, string cardNo, out string ErrorMessage)
        {
            if (parkid.IsEmpty()) throw new ArgumentNullException("mode");
            if (cardNo.IsEmpty()) throw new ArgumentNullException("cardNo");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.GetNoExitIORecordByCardNo(parkid, cardNo, out ErrorMessage);
        }

        public static List<ParkIORecord> QueryIORecordIds(DateTime start, DateTime end, string parkingId, bool IsExit)
        {
            if (start == null) throw new ArgumentNullException("start");
            if (end == null) throw new ArgumentNullException("end");
            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.QueryMonthExpiredIORecordIds(start, end, parkingId, IsExit);
        }
        public static List<ParkIORecord> QueryInIORecordIds(string parkingId, bool IsExit)
        {
            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.QueryInIORecordIds(parkingId, IsExit);
        }

        public static ParkIORecord GetNoExitIORecordByPlateNumber(string parkid, string plateNumber, out string ErrorMessage)
        {
            if (parkid.IsEmpty()) throw new ArgumentNullException("mode");
            if (plateNumber.IsEmpty()) throw new ArgumentNullException("plateNumber");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.GetNoExitIORecordByPlateNumber(parkid, plateNumber, out ErrorMessage);
        }
        public static bool RemoveRepeatInIORecordByPlateNumber(string parkingID, string plateNumber, out string ErrorMessage)
        {
            if (parkingID.IsEmpty()) throw new ArgumentNullException("parkingID");
            if (plateNumber.IsEmpty()) throw new ArgumentNullException("plateNumber");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.RemoveRepeatInIORecordByPlateNumber(parkingID, plateNumber, out ErrorMessage);
        }
        public static bool RemoveRepeatInIORecordByCardNo(string parkingID, string cardNo, out string ErrorMessage)
        {
            if (parkingID.IsEmpty()) throw new ArgumentNullException("parkingID");
            if (cardNo.IsEmpty()) throw new ArgumentNullException("cardNo");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.RemoveRepeatInIORecordByCardNo(parkingID, cardNo, out ErrorMessage);
        }
        public static int GetAreaCarNum(string areaID, BaseCarType cartype, out string ErrorMessage)
        {
            if (areaID.IsEmpty()) throw new ArgumentNullException("areaID");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.GetAreaCarNum(areaID, cartype, out ErrorMessage);
        }
        public static int GetIsEditCarNum(string areaID, out string ErrorMessage)
        {
            if (areaID.IsEmpty()) throw new ArgumentNullException("areaID");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.GetIsEditCarNum(areaID, out ErrorMessage);
        }

        public static int GetAreaCarNumWhenTimeseriesUnExit(string areaID, out string ErrorMessage)
        {
            if (areaID.IsEmpty()) throw new ArgumentNullException("areaID");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.GetAreaCarNumWhenTimeseriesUnExit(areaID, out ErrorMessage);
        }
        public static DateTime? GetLastRecordExitDateByPlateNumber(string parkingID, string platenumber, out string ErrorMessage)
        {
            if (parkingID.IsEmpty()) throw new ArgumentNullException("parkingID");
            if (platenumber.IsEmpty()) throw new ArgumentNullException("platenumber");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.GetLastRecordExitDateByPlateNumber(parkingID, platenumber, out ErrorMessage);
        }
        public static DateTime? GetLastRecordExitDateByCarNo(string parkingID, string cardNo, out string ErrorMessage)
        {
            if (parkingID.IsEmpty()) throw new ArgumentNullException("parkingID");
            if (cardNo.IsEmpty()) throw new ArgumentNullException("cardNo");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.GetLastRecordExitDateByCarNo(parkingID, cardNo, out ErrorMessage);
        }

        public static DateTime? GetLastRecordEnterTimeByPlateNumber(string parkingID, string platenumber, out string ErrorMessage)
        {
            if (parkingID.IsEmpty()) throw new ArgumentNullException("parkingID");
            if (platenumber.IsEmpty()) throw new ArgumentNullException("platenumber");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.GetLastRecordEnterTimeByPlateNumber(parkingID, platenumber, out ErrorMessage);
        }
        public static DateTime? GetLastRecordEnterTimeByCarNo(string parkingID, string cardNo, out string ErrorMessage)
        {
            if (parkingID.IsEmpty()) throw new ArgumentNullException("parkingID");
            if (cardNo.IsEmpty()) throw new ArgumentNullException("cardNo");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.GetLastRecordEnterTimeByCarNo(parkingID, cardNo, out ErrorMessage);
        }
        public static ParkIORecord GetIORecord(string recordID, out string ErrorMessage)
        {
            if (recordID.IsEmpty()) throw new ArgumentNullException("recordID");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.GetIORecord(recordID, out ErrorMessage);
        }
        public static ParkIORecord GetIORecordContainsDelete(string recordID, out string ErrorMessage)
        {
            if (recordID.IsEmpty()) throw new ArgumentNullException("recordID");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.GetIORecordContainsDelete(recordID, out ErrorMessage);
        }
        public static int GetMonthIORecordCountByPlateNumber(string parkingID, string plateNumber, DateTime datetime, out string ErrorMessage)
        {
            if (parkingID.IsEmpty()) throw new ArgumentNullException("parkingID");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.GetMonthIORecordCountByPlateNumber(parkingID, plateNumber, datetime, out ErrorMessage);
        }
        public static List<ParkIORecord> GetIORecordWhitPageTab(string parkingID, int pageSize, int pageIndex, List<string> cardTypeIDs, string likePlateNumber, out int pageCount, out string ErrorMessage, int isExit = 0)
        {
            if (parkingID.IsEmpty()) throw new ArgumentNullException("parkingID");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.GetIORecordWhitPageTab(parkingID, pageSize, pageIndex, cardTypeIDs, likePlateNumber, out pageCount, out ErrorMessage, isExit);
        }
        public static List<ParkIORecord> GetIORecordWhitPageTab(string parkingID, int pageSize, int pageIndex, List<string> cardTypeIDs, string carModelID, string likePlateNumber, string ingateid, string outgateid, DateTime startTime, DateTime endTime, out int pageCount, out string ErrorMessage, int isExit = 0, int stayDay = -1)
        {
            if (parkingID.IsEmpty()) throw new ArgumentNullException("parkingID");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.GetIORecordWhitPageTab(parkingID, pageSize, pageIndex, cardTypeIDs, carModelID, likePlateNumber, ingateid, outgateid, startTime, endTime, out pageCount, out ErrorMessage, isExit, stayDay);

        }
        public static ParkIORecord QueryCarLastNotExitIORecord(string parkingId, string plateNumber)
        {
            if (parkingId.IsEmpty()) throw new ArgumentNullException("parkingId");
            if (plateNumber.IsEmpty()) throw new ArgumentNullException("plateNumber");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.QueryCarLastNotExitIORecord(parkingId, plateNumber);
        }

        public static Dictionary<string, DateTime> QueryLastNotExitIORecord(string parkingId, List<string> plateNumbers)
        {
            if (parkingId.IsEmpty()) throw new ArgumentNullException("parkingId");
            if (plateNumbers.Count == 0) throw new ArgumentNullException("plateNumbers");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.QueryLastNotExitIORecord(parkingId, plateNumbers);
        }

        public static List<string> QueryMonthExpiredNotPayAmountIORecordIds(DateTime start, DateTime end, string parkingId, List<string> plateNumbers)
        {
            if (parkingId.IsEmpty()) throw new ArgumentNullException("parkingId");
            if (plateNumbers.Count == 0) throw new ArgumentNullException("plateNumbers");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.QueryMonthExpiredNotPayAmountIORecordIds(start, end, parkingId, plateNumbers);
        }

        public static List<ParkIORecord> GetInParkingIORecords(string pkid)
        {
            if (pkid.IsEmpty()) throw new ArgumentNullException("pkid");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.GetInParkingIORecords(pkid);
        }

        public static List<ParkIORecord> GetIORecordUseLikeStrWhitPageTab(string parkingID, int pageSize, int pageIndex, List<string> cardTypeIDs, string likePlateNumberStr, out int pageCount, out string ErrorMessage, int isExit = 0)
        {
            if (parkingID.IsEmpty()) throw new ArgumentNullException("parkingID");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.GetIORecordUseLikeStrWhitPageTab(parkingID, pageSize, pageIndex, cardTypeIDs, likePlateNumberStr, out pageCount, out ErrorMessage, isExit);
        }
        /// <summary>
        /// 查询没有出场的临停记录
        /// </summary>
        /// <param name="parkingIds"></param>
        /// <param name="plateNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordTotalCount"></param>
        /// <returns></returns>
        public static List<ParkIORecord> QueryPageNotExit(string parkingId, string plateNumber, int pageSize, int pageIndex, out int recordTotalCount)
        {
            if (string.IsNullOrWhiteSpace(parkingId)) throw new ArgumentNullException("parkingId");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.QueryPageNotExit(parkingId, plateNumber, pageSize, pageIndex, out recordTotalCount);
        }

        public static List<ParkIORecord> GetInParkingNoPlatenumberRecords(string pkid)
        {
            if (pkid.IsEmpty()) throw new ArgumentNullException("pkid");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.GetInParkingNoPlatenumberRecords(pkid);
        }

        public static List<ParkIORecord> GetInParkingNoPlatenumberRecords(string pkid, DateTime datetime)
        {
            if (pkid.IsEmpty()) throw new ArgumentNullException("pkid");

            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.GetInParkingNoPlatenumberRecords(pkid, datetime);
        }
        public static ParkIORecord QueryLastExitIORecordByPlateNumber(string plateNumber)
        {
            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.QueryLastExitIORecordByPlateNumber(plateNumber);
        }

        public static bool DelParkIORecord(string RecordID)
        {
            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.DelParkIORecord(RecordID);
        }

        /// <summary>
        /// 修改车类型
        /// </summary>
        /// <param name="RecordID"></param>
        /// <param name="CarType"></param>
        /// <returns></returns>
        public static bool EditParkIORecord(string RecordID, string CarModelID)
        {
            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.EditParkIORecord(RecordID, CarModelID);
        }

        public static ParkIORecord QueryInCarTempIORecordByLikePlateNumber(string parkid, string plateNumber, out string errorMsg)
        {
            IParkIORecord factory = ParkIORecordFactory.GetFactory();
            return factory.QueryInCarTempIORecordByLikePlateNumber(parkid, plateNumber, out errorMsg);
        }
    }
}
