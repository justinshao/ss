using Common.Entities;
using Common.Entities.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.IRepository.Park
{
    public interface IParkTimeseries
    {
        ParkTimeseries AddTimeseries(ParkTimeseries model, out string errorMsg);

        bool ModifyTimeseries(ParkTimeseries model, out string errorMsg);

        ParkTimeseries GetTimeseriesesByIORecordID(string parkid, string iorecordID, out string ErrorMessage);
        bool RemoveTimeseries(string timeseriesid);
        int GetAreaCarNum(string areaID, BaseCarType carType, out string ErrorMessage);
        int GetAreaCarNum(string areaID, out string ErrorMessage);
        DateTime GetLastRecordExitDate(string parkingID, string iorecordid, out string ErrorMessage);
        DateTime GetLastRecordEnterTime(string parkingID, string iorecordid, out string ErrorMessage);
        /// <summary>
        /// 获取所有已经完成的 时序记录
        /// </summary>
        /// <param name="parkid"></param>
        /// <param name="iorecordID"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        List<ParkTimeseries> GetAllExitsTimeseriesesByIORecordID(string parkid, string iorecordID, out string ErrorMessage);
        List<ParkTimeseries> GetTimeseriesIORecordWhitPageTab(string parkingID, int pageSize, int pageIndex, List<string> cardTypeIDs, string carTypeID, string likePlateNumber, string ingateid, string outgateid, DateTime startTime, DateTime endTime, out int pageCount, out string ErrorMessage, int isExit = 0, int stayDay = -1);
        ParkTimeseries GetTimeseries(string timeseriesID, out string ErrorMessage);
        List<ParkTimeseries> GetTimeseriesIORecordByLikeStrWhitPageTab(string parkingID, int pageSize, int pageIndex, List<string> cardTypeIDs, string likePlateNumberStr, out int pageCount, out string ErrorMessage, int isExit = 0);
        int GetIsEditCarNum(string areaID, out string ErrorMessage); 
    }
}
