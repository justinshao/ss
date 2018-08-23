using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Entities;
using Common.Entities.Statistics;
using Common.Entities.Parking;
using Common.IRepository.Park;
using Common.Factory.Park;
using Common.Factory.Statistics;
using Common.IRepository.Statistics;
using Common.DataAccess;
using Common.Factory;
using Common.IRepository;
using Common.Services;
namespace Common.Services.Statistics
{
    /// <summary>
    /// 数据汇总
    /// </summary>
    public class GatherServices
    {
        public void Statistics_DailyGather()
        {
            try
            {
                IParking iparking = ParkingFactory.GetFactory();
                string strerror = string.Empty;
                List<BaseParkinfo> parks = iparking.QueryParkingAll();
                if (parks == null || parks.Count == 0)
                    return;
                DateTime dtNow = DateTime.Now;
                //循环车场
                foreach (BaseParkinfo p in parks)
                {
                    //如果在线上统计,则本地就不统计
                    if (p.IsOnLineGathe == YesOrNo.Yes)
                    {
                        continue;
                    }
                    List<ParkGate> gatelist = ParkGateServices.QueryByParkingId(p.PKID);
                    //统计近8天的数据 
                    for (int i = 30; i >= 0; i--)
                    {
                        try
                        {
                            new DailyGatherServices().Statistics_DailyByHour(p, dtNow, i);
                        }
                        catch (Exception ex)
                        {
                            Common.Services.TxtLogServices.WriteTxtLog("按时统计异常 异常信息:{0}", ex.Message);
                        }
                        //按通道统计时数据
                        try
                        {
                            new GateGatherServices().Statistics_DailyByGate(gatelist,p, dtNow, i);
                        }
                        catch (Exception ex)
                        {
                            Common.Services.TxtLogServices.WriteTxtLog("按通道统计异常 异常信息:{0}", ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Services.TxtLogServices.WriteTxtLog("按日统计异常 异常信息:{0}", ex.Message);
            }
        }

        public void Statistics_DailyGather(DateTime start,DateTime end)
        {
            try
            {
                IParking iparking = ParkingFactory.GetFactory();
                string strerror = string.Empty;
                List<BaseParkinfo> parks = iparking.QueryParkingAll();
                if (parks == null || parks.Count == 0)
                    return;
                end = end.Date.AddHours(23).AddMinutes(59);
                //循环车场
                foreach (BaseParkinfo p in parks)
                {
                    //如果在线上统计,则本地就不统计
                    if (p.IsOnLineGathe == YesOrNo.Yes)
                    {
                        continue;
                    }
                    List<ParkGate> gatelist = ParkGateServices.QueryByParkingId(p.PKID);
                    TimeSpan ts = end - start;
                    //统计近8天的数据 
                    for (int i = ts.Days; i >= 0; i--)
                    {
                        try
                        {
                            new DailyGatherServices().Statistics_DailyByHour(p, end, i);
                        }
                        catch (Exception ex)
                        {
                            Common.Services.TxtLogServices.WriteTxtLog("按时统计异常 异常信息:{0}", ex.Message);
                        }
                        //按通道统计时数据
                        try
                        {
                            new GateGatherServices().Statistics_DailyByGate(gatelist, p, end, i);
                        }
                        catch (Exception ex)
                        {
                            Common.Services.TxtLogServices.WriteTxtLog("按通道统计异常 异常信息:{0}", ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Services.TxtLogServices.WriteTxtLog("按日统计异常 异常信息:{0}", ex.Message);
            }
        }

        public void Statistics_DailyGather(string PKID,DateTime start, DateTime end)
        {
            try
            {
                IParking iparking = ParkingFactory.GetFactory();
                string strerror = string.Empty;
                var parking = iparking.QueryParkingByParkingID(PKID);
                List<ParkGate> gatelist = ParkGateServices.QueryByParkingId(PKID);
                try
                {
                    var p = iparking.QueryParkingByParkingID(PKID);
                    new DailyGatherServices().Statistics_DailyByHour(p, start, end);
                }
                catch (Exception ex)
                {
                    Common.Services.TxtLogServices.WriteTxtLog("按时统计异常 异常信息:{0}", ex.Message);
                }
                //按通道统计时数据
                try
                {
                    new GateGatherServices().Statistics_DailyByGate(gatelist, parking, start, end);
                }
                catch (Exception ex)
                {
                    Common.Services.TxtLogServices.WriteTxtLog("按通道统计异常 异常信息:{0}", ex.Message);
                }
            }
            catch (Exception ex)
            {
                Common.Services.TxtLogServices.WriteTxtLog("按日统计异常 异常信息:{0}", ex.Message);
            }
        }

        public bool DeleteGatherTime(string parkingid, DateTime start, DateTime end)
        {
            IStatistics_Gather gatherfactory = Statistics_GatherFactory.GetFactory();
            return gatherfactory.DeleteGatherTime(parkingid, start, end);
        }
    }
}
