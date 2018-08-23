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
    public class TgOrderService
    {
        /// <summary>
        /// 添加推广人员成功订单记录
        /// </summary>
        /// <param name="modle"></param>
        /// <returns></returns>
        public static bool Addperson(TgOrder modle)
        {
            ITgOrder factory = TgOrderFactory.GetFactory();
            bool result = factory.addTgOrder(modle);
            if (result)
            {
                OperateLogServices.AddOperateLog(OperateType.Add, string.Format("orderId:{0},personName:{1},parkingname:{2}", modle.ID, modle.PersonName, modle.PKName));
            }
            return result;
        }
        /// <summary>
        /// 查询所有推广记录次数 按人员id分类
        /// </summary>
        /// <param name="paras"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public static Pagination CountTgPersonOrder(InParams paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            ITgOrder factory = TgOrderFactory.GetFactory();
            _pagination.Total = factory.CountTgPersonOrderCount(paras);
            _pagination.StatisticsTgOrderList = factory.CountTgPersonOrder(paras,PageSize,PageIndex); 
            return _pagination;
        }
        public static List<TgOrder> QueryAllCountTgPersonOrder(InParams paras)
        {
            ITgOrder factory = TgOrderFactory.GetFactory();
            List<TgOrder> result = factory.QueryAllCountTgPersonOrder(paras);
            return result;
        }
    }
}
