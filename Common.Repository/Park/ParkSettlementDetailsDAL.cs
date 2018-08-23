using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.IRepository.Park;
using Common.Entities.Parking;
using Common.DataAccess;
namespace Common.SqlRepository.Park
{
    public class ParkSettlementDetailsDAL : BaseDAL, IParkSettlementDetails
    {
        /// <summary>
        /// 增加结算明细
        /// </summary>
        /// <param name="model">结算明细模型</param>
        /// <param name="dboperator">数据库操作对象</param>
        /// <returns></returns>
        public bool Add(ParkSettlementDetailsModel model,DbOperator dboperator)
        {
            return true;
        }
    }
}
