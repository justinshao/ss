using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataAccess;

namespace Common.IRepository.Park
{
    public interface IParkSeller
    {
        List<ParkCarDerate> GetCanUseCarderatesByIORecordid(string IORecordid, out string errorMsg);
        List<ParkCarDerate> GetCanUseCarderatesByPlatenumber(string cardNo, out string errorMsg);
        ParkDerate GetDerate(string derateid, out string errorMsg);
        ParkSeller GetSeller(string sellerid, out string errorMsg);
        bool ModifyCarderate(ParkCarDerate mode, out string errorMsg);
        bool ModifySeller(ParkSeller mode, out string errorMsg);

        bool Add(ParkSeller model);

        bool AddCarderate(ParkCarDerate mode);

        bool UpdatePassword(string sellerId, string password);
        /// <summary>
        /// 商家扣款
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="balance"></param>
        /// <param name="dbOperator"></param>
        /// <returns></returns>
        bool SellerDebit(string sellerId, decimal balance, DbOperator dbOperator);

        /// <summary>
        /// 商家充值
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="balance"></param>
        /// <param name="dbOperator"></param>
        /// <returns></returns>
        bool SellerRecharge(string sellerId, decimal balance, DbOperator dbOperator);

        bool Delete(string sellerId);

        List<ParkSeller> QueryByVillageId(string villageId);

        List<ParkSeller> QueryByVillageIds(List<string> villageIds);

        List<ParkSeller> QueryPage(string villageId, string sellerName, int pagesize, int pageindex, out int total);

        ParkSeller QueryBySellerId(string sellerId);

        ParkSeller QueryBySellerNo(string sellerNo);

        bool UpdateParkCarDerateStatus(List<string> derateIds, int status, DbOperator dbOperator);
    }
}
