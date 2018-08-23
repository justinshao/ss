using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Core.Expands;
using Common.Factory.Park;
using Common.IRepository.Park;
using Common.Utilities;
using Common.DataAccess;
using Common.Entities.Parking;

namespace Common.Services.Park
{
    public class ParkSellerServices
    {
        public static List<ParkCarDerate> GetCanUseCarderatesByIORecordid(string Iorecordid, out string errorMsg)
        {
            if (Iorecordid.IsEmpty()) throw new ArgumentNullException("Iorecordid");

            IParkSeller factory = ParkSellerFactory.GetFactory();
            return factory.GetCanUseCarderatesByIORecordid(Iorecordid, out errorMsg);
        }
        public static List<ParkCarDerate> GetCanUseCarderatesByPlatenumber(string platenumber, out string errorMsg)
        {
            if (platenumber.IsEmpty()) throw new ArgumentNullException("platenumber");

            IParkSeller factory = ParkSellerFactory.GetFactory();
            return factory.GetCanUseCarderatesByPlatenumber(platenumber, out errorMsg);
        }
        public static ParkDerate GetDerate(string derateid, out string errorMsg)
        {
            if (derateid.IsEmpty()) throw new ArgumentNullException("derateid");

            IParkSeller factory = ParkSellerFactory.GetFactory();
            return factory.GetDerate(derateid, out errorMsg);
        }

        public static ParkSeller GetSeller(string sellerid, out string errorMsg)
        {
            if (sellerid.IsEmpty()) throw new ArgumentNullException("sellerid");

            IParkSeller factory = ParkSellerFactory.GetFactory();
            return factory.GetSeller(sellerid, out errorMsg);
        }

        public static bool ModifyCarderate(ParkCarDerate mode, out string errorMsg)
        {
            if (mode == null) throw new ArgumentNullException("mode");

            IParkSeller factory = ParkSellerFactory.GetFactory();
            return factory.ModifyCarderate(mode, out errorMsg);
        }

        public static bool AddCarderate(ParkCarDerate mode)
        {
            if (mode == null) throw new ArgumentNullException("mode");

            IParkSeller factory = ParkSellerFactory.GetFactory();
            return factory.AddCarderate(mode);
        }
        public static bool ModifySeller(ParkSeller mode, out string errorMsg)
        {
            if (mode == null) throw new ArgumentNullException("mode");

            IParkSeller factory = ParkSellerFactory.GetFactory();
            ParkSeller dbModel = factory.QueryBySellerNo(mode.SellerNo);
            if (dbModel != null && dbModel.SellerID!=mode.SellerID) throw new MyException("商家编号重复");

            return factory.ModifySeller(mode, out errorMsg);
        }
        public static bool Add(ParkSeller model)
        {
            if (model == null) throw new ArgumentNullException("model");

            IParkSeller factory = ParkSellerFactory.GetFactory();
            model.SellerID = GuidGenerator.GetGuid().ToString();
            ParkSeller dbModel = factory.QueryBySellerNo(model.SellerNo);
            if (dbModel != null) throw new MyException("商家编号重复");
            bool result = factory.Add(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkSeller>(model, OperateType.Add);
            }
            return result;
        }

        public static bool UpdatePassword(string sellerId, string password) {
            if (sellerId.IsEmpty()) throw new ArgumentNullException("sellerId");
            if (password.IsEmpty()) throw new ArgumentNullException("password");

            IParkSeller factory = ParkSellerFactory.GetFactory();
            bool result = factory.UpdatePassword(sellerId,password);
            if (result)
            {
                OperateLogServices.AddOperateLog(OperateType.Update, string.Format("sellerId:{0},修改新密码为：{1}",sellerId,password));
            }
            return result;
        }

        public static bool SellerCharge(string sellerId, decimal balance,string operatorId){
            if (sellerId.IsEmpty()) throw new ArgumentNullException("sellerId");
            if (operatorId.IsEmpty()) throw new ArgumentNullException("operatorId");
            if (balance <= 0) throw new ArgumentNullException("balance");

             IParkSeller factory = ParkSellerFactory.GetFactory();
             ParkSeller model = factory.QueryBySellerId(sellerId);
            if(model==null)throw new MyException("商家不存在");

            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    ParkOrder order = ParkOrderServices.MarkSellerChargeOrder(model, balance, operatorId, OrderSource.ManageOffice, OrderPayWay.Cash, dbOperator);
                    if (order == null) throw new MyException("创建充值订单失败");

                    bool result = factory.SellerRecharge(sellerId, balance, dbOperator);
                    if (!result) throw new MyException("取消暂停计划失败");
                    dbOperator.CommitTransaction();

                    ParkSeller newSeller = factory.QueryBySellerId(sellerId);
                    if (newSeller != null)
                    {
                        OperateLogServices.AddOperateLog(OperateType.Update, string.Format("sellerId:{0},充值:{1},余额为:{2}", sellerId, balance, newSeller.Balance));
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    dbOperator.RollbackTransaction();
                    throw ex;
                }
            }
        }

        public static bool Delete(string sellerId)
        {
            if (sellerId.IsEmpty()) throw new ArgumentNullException("sellerId");

            IParkSeller factory = ParkSellerFactory.GetFactory();
            bool result = factory.Delete(sellerId);
            if (result)
            {
                OperateLogServices.AddOperateLog(OperateType.Update, string.Format("sellerId:{0}", sellerId));
            }
            return result;
        }

        public static List<ParkSeller> QueryByVillageId(string villageId)
        {
            if (villageId.IsEmpty()) throw new ArgumentNullException("villageId");

            IParkSeller factory = ParkSellerFactory.GetFactory();
            return factory.QueryByVillageId(villageId);
        }

        public static List<ParkSeller> QueryByVillageIds(List<string> villageIds)
        {
            if (villageIds == null || villageIds.Count == 0) throw new ArgumentNullException("villageIds");

            IParkSeller factory = ParkSellerFactory.GetFactory();
            return factory.QueryByVillageIds(villageIds);
        }

        public static List<ParkSeller> QueryPage(string villageId, string sellerName, int pagesize, int pageindex, out int total)
        {
            if (villageId.IsEmpty()) throw new ArgumentNullException("villageId");

            IParkSeller factory = ParkSellerFactory.GetFactory();
            return factory.QueryPage(villageId,sellerName,pagesize,pageindex,out total);
        }

        public static ParkSeller QueryBySellerId(string sellerId)
        {
            if (sellerId.IsEmpty()) throw new ArgumentNullException("sellerId");

            IParkSeller factory = ParkSellerFactory.GetFactory();
            return factory.QueryBySellerId(sellerId);
        }

        public static ParkSeller QueryBySellerNo(string sellerNo)
        {
            if (sellerNo.IsEmpty()) throw new ArgumentNullException("sellerNo");

            IParkSeller factory = ParkSellerFactory.GetFactory();
            return factory.QueryBySellerNo(sellerNo);
        }
    }
}
