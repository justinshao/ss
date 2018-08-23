using Common.Entities;
using Common.Entities.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataAccess;

using Common.Entities.Statistics;
namespace Common.IRepository.Park
{
    public interface IParkIORecord
    {
        ParkIORecord AddIORecord(ParkIORecord mode, out string ErrorMessage);
        bool ModifyIORecord(ParkIORecord mode, out string ErrorMessage);
        ParkIORecord GetNoExitIORecordByCardNo(string parkid, string cardNo, out string ErrorMessage);
        ParkIORecord GetNoExitIORecordByPlateNumber(string parkid, string plateNumber, out string ErrorMessage);
        bool RemoveRepeatInIORecordByPlateNumber(string parkingID, string plateNumber, out string ErrorMessage);
        bool RemoveRepeatInIORecordByCardNo(string parkingID, string cardNo, out string ErrorMessage);
        int GetAreaCarNum(string areaID, BaseCarType cartype, out string ErrorMessage);
        int GetAreaCarNumWhenTimeseriesUnExit(string areaID, out string ErrorMessage);
        DateTime? GetLastRecordExitDateByPlateNumber(string parkingID, string platenumber, out string ErrorMessage);
        DateTime? GetLastRecordExitDateByCarNo(string parkingID, string cardNo, out string ErrorMessage);
        DateTime? GetLastRecordEnterTimeByPlateNumber(string parkingID, string platenumber, out string ErrorMessage);
        DateTime? GetLastRecordEnterTimeByCarNo(string parkingID, string cardNo, out string ErrorMessage);
        ParkIORecord GetIORecord(string recordID, out string ErrorMessage);
        ParkIORecord GetIORecordContainsDelete(string recordID, out string ErrorMessage);
        int GetMonthIORecordCountByPlateNumber(string parkingID, string plateNumber, DateTime datetime, out string ErrorMessage);
        List<ParkIORecord> GetIORecordWhitPageTab(string parkingID, int pageSize, int pageIndex, List<string> cardTypeIDs, string likePlateNumber, out int pageCount, out string ErrorMessage, int isExit = 0);
        List<ParkIORecord> GetIORecordWhitPageTab(string parkingID, int pageSize, int pageIndex, List<string> cardTypeIDs, string carTypeID, string likePlateNumber, string ingateid, string outgateid, DateTime startTime, DateTime endTime, out int pageCount, out string ErrorMessage, int isExit = 0, int stayDay = -1);

        ParkIORecord QueryCarLastNotExitIORecord(string parkingId, string plateNumber);
        List<ParkIORecord> QueryMonthExpiredIORecordIds(DateTime start, DateTime end, string parkingId, bool IsExit);
        Dictionary<string, DateTime> QueryLastNotExitIORecord(string parkingId, List<string> plateNumbers);

        List<string> QueryMonthExpiredNotPayAmountIORecordIds(DateTime start, DateTime end, string parkingId, List<string> plateNumbers);
        List<ParkIORecord> QueryInIORecordIds(string parkingId, bool IsExit);
        bool UpdateIORecordEnterType(List<string> recordIds, int enterType, DbOperator dbOperator);
        List<ParkIORecord> GetInParkingIORecords(string pkid);
        int EntranceCountByGate(string gateid, DateTime starttime, DateTime endtime);
        int EntranceCountByBox(string boxid, DateTime starttime, DateTime endtime);
        int EntranceCountByParkingID(string parkingid, DateTime starttime, DateTime endtime);
        int ExitCountByGate(string gateid, DateTime starttime, DateTime endtime);
        int ExitCountByBox(string boxid, DateTime starttime, DateTime endtime);
        int ExitCountByParkingID(string parkingid, DateTime starttime, DateTime endtime);
        List<KeyValue> GetInCardTypeByParkingID(string parkingid, DateTime starttime, DateTime endtime);
        List<KeyValue> GetInCardTypeByBoxID(string boxid, DateTime starttime, DateTime endtime);
        List<KeyValue> GetInCardTypeByGateID(string gateid, DateTime starttime, DateTime endtime);
        List<KeyValue> GetReleaseTypeByParkingID(string parkingid, DateTime starttime, DateTime endtime);
        List<KeyValue> GetReleaseTypeByGate(string gateid, DateTime starttime, DateTime endtime);
        List<KeyValue> GetReleaseTypeByBox(string boxid, DateTime starttime, DateTime endtime);
        List<ParkIORecord> GetCarEntranceTimeAndExitTime(string parkingid, DateTime starttime, DateTime endtime);
        List<ParkIORecord> GetIORecordUseLikeStrWhitPageTab(string parkingID, int pageSize, int pageIndex, List<string> cardTypeIDs, string likePlateNumberStr, out int pageCount, out string ErrorMessage, int isExit = 0);
        List<ParkIORecord> QueryPageNotExit(string parkingId, string plateNumber, int pageSize, int pageIndex, out int totalCount);
        /// <summary>
        /// 获取车场所有无牌记录
        /// </summary>
        /// <param name="pkid"></param>
        /// <returns></returns>
        List<ParkIORecord> GetInParkingNoPlatenumberRecords(string pkid);
        List<ParkIORecord> GetInParkingNoPlatenumberRecords(string pkid, DateTime datetime);
        ParkIORecord QueryLastExitIORecordByPlateNumber(string plateNumber);
        bool DelParkIORecord(string RecordID);

        /// <summary>
        /// 修改车类型
        /// </summary>
        /// <param name="RecordID"></param>
        /// <param name="CarType"></param>
        /// <returns></returns>
        bool EditParkIORecord(string RecordID, string CarModelID);

        int GetIsEditCarNum(string areaID, out string ErrorMessage);
        /// <summary>
        /// 根据车牌模糊查询在场临时车  模糊方式%__
        /// </summary>
        /// <param name="likeplateNumber"></param>
        /// <returns></returns>
        ParkIORecord QueryInCarTempIORecordByLikePlateNumber(string parkid, string likeplateNumber, out string ErrorMessage);


        List<ParkIORecord> QueryPlatenumberIORecordByTime(DateTime start, DateTime end, string platenumber, string parkingId, bool IsExit);
    }
}
