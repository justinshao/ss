using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Entities.Other;
using Common.Entities.Statistics;
using Common.Factory.Statistics;
using Common.IRepository.Statistics;

namespace Common.Services.Statistics
{
    public class tgPersonServices
    {
        
        public static bool Addperson(tgPerson modle) {
            ItgPerson factory = StatisticstgPerson.GetFactory();
            bool result =factory.Add(modle);
            if (result)
            {
                OperateLogServices.AddOperateLog(OperateType.Add, string.Format("personId:{0},personName:{1},parkingname:{2}", modle.id,modle.name, modle.PKName));
            }
            return result;
        }

        public static string infotgPerson() {
            ItgPerson factory = StatisticstgPerson.GetFactory();
            string result = factory.SearchTg();

            return result;
        }
        /// <summary>
        /// 获得推广人员记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public static Pagination Search_DailyStatistics(InParams paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            ItgPerson factory = StatisticstgPerson.GetFactory();
            _pagination.Total = factory.Search_tgPersonStatisticsCount(paras);
            _pagination.StatisticsPersonList = factory.Search_tgPersonStatistics(paras, PageSize, PageIndex);
            if (!string.IsNullOrEmpty(paras.ParkingID) && _pagination.StatisticsGatherList != null && _pagination.StatisticsGatherList.Count > 0)
            {
                BaseParkinfo parkinfo = ParkingServices.QueryParkingByParkingID(paras.ParkingID);
                if (parkinfo != null)
                {
                    foreach (var v in _pagination.StatisticsGatherList)
                    {
                        v.ParkingName = parkinfo.PKName;
                    }
                }
            }
            return _pagination;
        }
        public static bool DeletePerson(string tbName,string fild,string value){
            ItgPerson factory=StatisticstgPerson.GetFactory();
            bool result=factory.Delete(tbName,fild,value);
            if (result)
            {
                OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("tablename:{0},personid:{1}", tbName, value));
            }
            return result;
        }

        public static bool UpdatePerson(tgPerson modle) {
            ItgPerson factory = StatisticstgPerson.GetFactory();
            bool result = factory.Update(modle);
            if (result)
            {
                OperateLogServices.AddOperateLog(OperateType.Update, string.Format("personId:{0},personName:{1},parkingname:{2}", modle.id, modle.name, modle.PKName));
            }
            return result;
        }
        public static tgPerson QueryPersonByID(int id)
        {
            ItgPerson factory = StatisticstgPerson.GetFactory();
            tgPerson Person = factory.QueryPersonByID(id);
            
            return Person;
        }
    }
}
