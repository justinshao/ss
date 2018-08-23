using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Entities;
using Common.Entities.Parking;
using Common.IRepository.Statistics;
using Common.IRepository.Park;
using Common.Factory.Park;
using Common.Factory.Statistics;
using Common.IRepository;
using Common.Factory;

namespace Common.Services.Statistics
{
    public class LongTimeGatherServices
    {
        /// <summary>
        /// 停车时长统计
        /// </summary>
        public void Statistics_LongTime()
        {
            try
            {
                IParking iparking = ParkingFactory.GetFactory();
                IParkIORecord iorecordfactory = ParkIORecordFactory.GetFactory();
                IStatistics_GatherLongTime gatherlongtimefactory = Statistics_GatherLongTimeFactory.GetFactory();
                string strerror = string.Empty;
                List<BaseParkinfo> parks = iparking.QueryParkingAll();
                //取得所有有效车场
                if (parks != null && parks.Count > 0)
                {
                    foreach (BaseParkinfo park in parks)
                    {
                        if (park.IsOnLineGathe == YesOrNo.Yes)
                        {
                            continue;
                        }
                        //近7天的统计数据. 如果没有则补上.
                        int loop = 7;
                        while (loop >= 0)
                        {
                            #region 变量
                            DateTime dtNow = DateTime.Now;
                            DateTime GatherTime = DateTime.Parse(dtNow.AddDays(-1 - loop).ToString("yyyy-MM-dd 00:00:00"));
                            DateTime EndTime = DateTime.Parse(dtNow.AddDays(-1 - loop).ToString("yyyy-MM-dd 23:59:59"));
                            int longtime1 = 0;
                            int longtime2 = 0;
                            int longtime3 = 0;
                            int longtime4 = 0;
                            int longtime5 = 0;
                            int longtime6 = 0;
                            int longtime7 = 0;
                            int longtime8 = 0;
                            int longtime9 = 0;
                            int longtime10 = 0;
                            int longtime11 = 0;
                            int longtime12 = 0;
                            int longtime13 = 0;
                            int longtime14 = 0;
                            int longtime15 = 0;
                            int longtime16 = 0;
                            int longtime17 = 0;
                            int longtime18 = 0;
                            int longtime19 = 0;
                            int longtime20 = 0;
                            int longtime21 = 0;
                            int longtime22 = 0;
                            int longtime23 = 0;
                            int longtime24 = 0;
                            #endregion

                            #region 判断是否已统计过
                            //判断统计数据是否存在.
                            if (gatherlongtimefactory.IsExists(park.PKID, GatherTime))
                            {
                                loop--;
                                continue;
                            }
                            #endregion

                            #region 获取记录并生成统计数据
                            List<ParkIORecord> iorecordlist = iorecordfactory.GetCarEntranceTimeAndExitTime(park.PKID, GatherTime, EndTime);
                            if (iorecordlist != null && iorecordlist.Count > 0)
                            {
                                foreach (ParkIORecord record in iorecordlist)
                                {
                                    int CarInHour = 0;
                                    if (record.EntranceTime == DateTime.MinValue)
                                    {
                                        CarInHour = 24;
                                    }
                                    else
                                    {
                                        if (record.ExitTime == DateTime.MinValue)
                                        {
                                            record.ExitTime = EndTime;
                                        }
                                        TimeSpan ts = EndTime - record.EntranceTime;
                                        CarInHour = System.Convert.ToInt32(Math.Ceiling(ts.Seconds / 3600.0));
                                    }
                                    switch (CarInHour)
                                    {
                                        case 1:
                                            longtime1++;
                                            break;
                                        case 2:
                                            longtime2++;
                                            break;
                                        case 3:
                                            longtime3++;
                                            break;
                                        case 4:
                                            longtime4++;
                                            break;
                                        case 5:
                                            longtime5++;
                                            break;
                                        case 6:
                                            longtime6++;
                                            break;
                                        case 7:
                                            longtime7++;
                                            break;
                                        case 8:
                                            longtime8++;
                                            break;
                                        case 9:
                                            longtime9++;
                                            break;
                                        case 10:
                                            longtime10++;
                                            break;
                                        case 11:
                                            longtime11++;
                                            break;
                                        case 12:
                                            longtime12++;
                                            break;
                                        case 13:
                                            longtime13++;
                                            break;
                                        case 14:
                                            longtime14++;
                                            break;
                                        case 15:
                                            longtime15++;
                                            break;
                                        case 16:
                                            longtime16++;
                                            break;
                                        case 17:
                                            longtime17++;
                                            break;
                                        case 18:
                                            longtime18++;
                                            break;
                                        case 19:
                                            longtime19++;
                                            break;
                                        case 20:
                                            longtime20++;
                                            break;
                                        case 21:
                                            longtime21++;
                                            break;
                                        case 22:
                                            longtime22++;
                                            break;
                                        case 23:
                                            longtime23++;
                                            break;
                                        case 24:
                                            longtime24++;
                                            break;
                                    }
                                }

                                #region 插入统计数据
                                string strSql = "insert into statistics_gatherlongtime(ParkingID,RecordID,GatherTime,LTime,Times,HaveUpdate,LastUpdateTime)"
                                          + " select '" + park.PKID + "','"+System.Guid.NewGuid().ToString()+"','" + GatherTime + "',1," + longtime1 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',2," + longtime2 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',3," + longtime3 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',4," + longtime4 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',5," + longtime5 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',6," + longtime6 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',7," + longtime7 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',8," + longtime8 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',9," + longtime9 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',10," + longtime10 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',11," + longtime11 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',12," + longtime12 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',13," + longtime13 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',14," + longtime14 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',15," + longtime15 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',16," + longtime16 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',17," + longtime17 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',18," + longtime18 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',19," + longtime19 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',20," + longtime20 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',21," + longtime21 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',22," + longtime22 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',23," + longtime23 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',24," + longtime24 + ",1,getdate()";
                                if (!gatherlongtimefactory.Insert(strSql))
                                {
                                    TxtLogServices.WriteTxtLog("插入停车时长统计信息异常 Sql:" + strSql);
                                }
                                #endregion
                            }
                            #endregion
                            loop--;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLog("统计停车时长信息异常 异常信息:" + ex.Message);
            }
        }

        /// <summary>
        /// 停车时长统计
        /// </summary>
        public void Statistics_LongTime(DateTime start,DateTime end)
        {
            try
            {
                IParking iparking = ParkingFactory.GetFactory();
                IParkIORecord iorecordfactory = ParkIORecordFactory.GetFactory();
                IStatistics_GatherLongTime gatherlongtimefactory = Statistics_GatherLongTimeFactory.GetFactory();
                string strerror = string.Empty;
                List<BaseParkinfo> parks = iparking.QueryParkingAll();
                //取得所有有效车场
                if (parks != null && parks.Count > 0)
                {
                    foreach (BaseParkinfo park in parks)
                    {
                        if (park.IsOnLineGathe == YesOrNo.Yes)
                        {
                            continue;
                        }
                       
                        //近7天的统计数据. 如果没有则补上.
                        int loop = (end-start).Days;
                        while (loop >= 0)
                        {
                            #region 变量
                            DateTime dtNow = end;
                            DateTime GatherTime = DateTime.Parse(dtNow.AddDays(-1 - loop).ToString("yyyy-MM-dd 00:00:00"));
                            DateTime EndTime = DateTime.Parse(dtNow.AddDays(-1 - loop).ToString("yyyy-MM-dd 23:59:59"));
                            int longtime1 = 0;
                            int longtime2 = 0;
                            int longtime3 = 0;
                            int longtime4 = 0;
                            int longtime5 = 0;
                            int longtime6 = 0;
                            int longtime7 = 0;
                            int longtime8 = 0;
                            int longtime9 = 0;
                            int longtime10 = 0;
                            int longtime11 = 0;
                            int longtime12 = 0;
                            int longtime13 = 0;
                            int longtime14 = 0;
                            int longtime15 = 0;
                            int longtime16 = 0;
                            int longtime17 = 0;
                            int longtime18 = 0;
                            int longtime19 = 0;
                            int longtime20 = 0;
                            int longtime21 = 0;
                            int longtime22 = 0;
                            int longtime23 = 0;
                            int longtime24 = 0;
                            #endregion

                            #region 判断是否已统计过
                            //判断统计数据是否存在.
                            if (gatherlongtimefactory.IsExists(park.PKID, GatherTime))
                            {
                                loop--;
                                continue;
                            }
                            #endregion

                            #region 获取记录并生成统计数据
                            List<ParkIORecord> iorecordlist = iorecordfactory.GetCarEntranceTimeAndExitTime(park.PKID, GatherTime, EndTime);
                            if (iorecordlist != null && iorecordlist.Count > 0)
                            {
                                foreach (ParkIORecord record in iorecordlist)
                                {
                                    int CarInHour = 0;
                                    if (record.EntranceTime == DateTime.MinValue)
                                    {
                                        CarInHour = 24;
                                    }
                                    else
                                    {
                                        if (record.ExitTime == DateTime.MinValue)
                                        {
                                            record.ExitTime = EndTime;
                                        }
                                        TimeSpan ts = EndTime - record.EntranceTime;
                                        CarInHour = System.Convert.ToInt32(Math.Ceiling(ts.Seconds / 3600.0));
                                    }
                                    switch (CarInHour)
                                    {
                                        case 1:
                                            longtime1++;
                                            break;
                                        case 2:
                                            longtime2++;
                                            break;
                                        case 3:
                                            longtime3++;
                                            break;
                                        case 4:
                                            longtime4++;
                                            break;
                                        case 5:
                                            longtime5++;
                                            break;
                                        case 6:
                                            longtime6++;
                                            break;
                                        case 7:
                                            longtime7++;
                                            break;
                                        case 8:
                                            longtime8++;
                                            break;
                                        case 9:
                                            longtime9++;
                                            break;
                                        case 10:
                                            longtime10++;
                                            break;
                                        case 11:
                                            longtime11++;
                                            break;
                                        case 12:
                                            longtime12++;
                                            break;
                                        case 13:
                                            longtime13++;
                                            break;
                                        case 14:
                                            longtime14++;
                                            break;
                                        case 15:
                                            longtime15++;
                                            break;
                                        case 16:
                                            longtime16++;
                                            break;
                                        case 17:
                                            longtime17++;
                                            break;
                                        case 18:
                                            longtime18++;
                                            break;
                                        case 19:
                                            longtime19++;
                                            break;
                                        case 20:
                                            longtime20++;
                                            break;
                                        case 21:
                                            longtime21++;
                                            break;
                                        case 22:
                                            longtime22++;
                                            break;
                                        case 23:
                                            longtime23++;
                                            break;
                                        case 24:
                                            longtime24++;
                                            break;
                                    }
                                }

                                #region 插入统计数据
                                string strSql = "insert into statistics_gatherlongtime(ParkingID,RecordID,GatherTime,LTime,Times,HaveUpdate,LastUpdateTime)"
                                          + " select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',1," + longtime1 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',2," + longtime2 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',3," + longtime3 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',4," + longtime4 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',5," + longtime5 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',6," + longtime6 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',7," + longtime7 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',8," + longtime8 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',9," + longtime9 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',10," + longtime10 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',11," + longtime11 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',12," + longtime12 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',13," + longtime13 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',14," + longtime14 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',15," + longtime15 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',16," + longtime16 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',17," + longtime17 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',18," + longtime18 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',19," + longtime19 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',20," + longtime20 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',21," + longtime21 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',22," + longtime22 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',23," + longtime23 + ",1,getdate()"
                                          + " union select '" + park.PKID + "','" + System.Guid.NewGuid().ToString() + "','" + GatherTime + "',24," + longtime24 + ",1,getdate()";
                                if (!gatherlongtimefactory.Insert(strSql))
                                {
                                    TxtLogServices.WriteTxtLog("插入停车时长统计信息异常 Sql:" + strSql);
                                }
                                #endregion
                            }
                            #endregion
                            loop--;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLog("统计停车时长信息异常 异常信息:" + ex.Message);
            }
        }
    }
}
