using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.Utilities;
using Common.Factory;
using Common.IRepository.Park;
using Common.DataAccess;
using Common.Entities.Enum;
using Common.Factory.Park;
using Common.Entities;
using Common.Utilities.Helpers;
using System.Web;
using System.IO;

namespace Common.Services.Park
{
    public class ParkDerateQRcodeServices
    {
        public static bool Add(ParkDerateQRcode model) {
            if (model == null) throw new ArgumentNullException("model");
            model.RecordID = GuidGenerator.GetGuidString();

            IParkDerateQRcode factory = ParkDerateQRcodeFactory.GetFactory();
            return factory.Add(model);
        }

        public static bool Update(ParkDerateQRcode model)
        {
            if (model == null) throw new ArgumentNullException("model");
            if (string.IsNullOrWhiteSpace(model.RecordID)) throw new ArgumentNullException("RecordID");

            IParkDerateQRcode factory = ParkDerateQRcodeFactory.GetFactory();
            return factory.Update(model);
        }

        public static bool Delete(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("RecordID");

            IParkDerateQRcode factory = ParkDerateQRcodeFactory.GetFactory();
            ParkDerateQRcode qrCode = factory.QueryByRecordId(recordId);
            if (qrCode == null) throw new MyException("待删除优免券不存在");

            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = factory.Delete(recordId, dbOperator);
                    if (!result) throw new MyException("删除失败");
                    if (qrCode.DerateQRcodeType == 1)
                    {
                        IParkCarDerate factoryCarDerate = ParkCarDerateFactory.GetFactory();
                        factoryCarDerate.DeleteNotUseByDerateQRCodeID(recordId,dbOperator);
                    }
                    dbOperator.CommitTransaction();
                    return true;
                }
                catch {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
            
        }

        public static List<ParkDerateQRcode> QueryByDerateID(string derateId)
        {
            if (string.IsNullOrWhiteSpace(derateId)) throw new ArgumentNullException("derateId");

            IParkDerateQRcode factory = ParkDerateQRcodeFactory.GetFactory();
            return factory.QueryByDerateID(derateId);
        }

        public static ParkDerateQRcode QueryByRecordId(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            IParkDerateQRcode factory = ParkDerateQRcodeFactory.GetFactory();
            return factory.QueryByRecordId(recordId);
        }
        public static bool Discount(string recordId, int alreadyUseTimes)
        {
            IParkDerateQRcode factory = ParkDerateQRcodeFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
               
                return factory.UpdateAlreadyUseTimes(recordId, alreadyUseTimes, dbOperator);
            }
        }
        public static List<ParkDerateQRcode> QueryPage(string sellerId, string derateId, int derateQRcodeType, int? status, DerateQRCodeSource? source, int pagesize, int pageindex, out int total)
        {
            IParkDerateQRcode factory = ParkDerateQRcodeFactory.GetFactory();
            return factory.QueryPage(sellerId, derateId, derateQRcodeType,status, source, pagesize, pageindex, out total);
        }
        /// <summary>
        /// 下发有免费券
        /// </summary>
        /// <param name="carDerateIds"></param>
        /// <param name="qrCodeZipFilePath">压缩二维码的文件路径</param>
        /// <param name="qrCodeId"></param>
        /// <returns></returns>
        public static bool GrantCarDerate(List<string> carDerateIds, string qrCodeZipFilePath, string qrCodeId)
        {
            if (carDerateIds == null || carDerateIds.Count == 0) throw new ArgumentNullException("carDerateIds");
            if (string.IsNullOrWhiteSpace(qrCodeZipFilePath)) throw new ArgumentNullException("qrCodeFilePath");

            string absolutePath = System.Web.HttpContext.Current.Server.MapPath("~/");
            string s = string.Format("{0}{1}", absolutePath, qrCodeZipFilePath);
            if (!File.Exists(s)) throw new MyException("压缩二维码的文件不存在");

            IParkDerateQRcode factory = ParkDerateQRcodeFactory.GetFactory();
            ParkDerateQRcode qrCode = factory.QueryByRecordId(qrCodeId);
            if (qrCode == null) throw new MyException("优免券规则不存在");

            if (qrCode.EndTime < DateTime.Now) throw new MyException("该优免券规则已过期");
            
            ParkDerate derate = ParkDerateServices.Query(qrCode.DerateID);
            if (derate == null) throw new MyException("获取优免规则失败");

            decimal totalAmount = 0;
            if (derate.DerateType == DerateType.SpecialTimePeriodPayment)
            {
                string errorMsg = string.Empty;
                ParkSeller seller = ParkSellerServices.GetSeller(derate.SellerID, out errorMsg);
                if (derate == null) throw new MyException("获取优免规则失败");

                totalAmount = qrCode.DerateValue * carDerateIds.Count;
                if ((seller.Creditline + seller.Balance) < totalAmount) throw new MyException("商家余额不足");
            }

            List<ParkCarDerate> carDerates = new List<ParkCarDerate>();
            foreach (var item in carDerateIds) {
                ParkCarDerate model = new ParkCarDerate();
                model.CarDerateID = item;
                model.CarDerateNo = IdGenerator.Instance.GetId().ToString();
                model.DerateID = qrCode.DerateID;
                model.PlateNumber = "";
                model.AreaID = "";
                model.PKID = qrCode.PKID;
                if (derate.DerateType != DerateType.ReliefTime && derate.DerateType != DerateType.VotePayment && derate.DerateType != DerateType.DayFree)
                {
                    model.FreeMoney = qrCode.DerateValue;
                }
                else if (derate.DerateType == DerateType.DayFree)
                {
                    model.FreeMoney = qrCode.DerateValue * derate.DerateMoney;
                }
                else
                {
                    model.FreeTime = (int)qrCode.DerateValue;
                }
                model.Status = CarDerateStatus.Used;
                model.CreateTime = DateTime.Now;
                model.ExpiryTime = qrCode.EndTime;
                model.DerateQRCodeID = qrCode.RecordID;
                carDerates.Add(model);
            }
            IParkCarDerate factoryCarDerate = ParkCarDerateFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    //添加券
                    foreach (var item in carDerates)
                    {
                        bool result = factoryCarDerate.Add(item, dbOperator);
                        if (!result) throw new MyException("发券失败");
                    }
                    //修改发放总张数据,修改最后压缩包路径
                    int totalNumbers = qrCode.CanUseTimes+carDerateIds.Count;
                    bool updateResult = factory.Update(qrCodeId, qrCodeZipFilePath, totalNumbers, dbOperator);
                    if (!updateResult) throw new MyException("修改券总数失败");

                    //商家扣款
                    if (totalAmount > 0) {
                        IParkSeller factorySeller = ParkSellerFactory.GetFactory();
                        updateResult = factorySeller.SellerDebit(derate.SellerID, totalAmount, dbOperator);
                        if (!updateResult) throw new MyException("商家扣款失败");
                    }
                    dbOperator.CommitTransaction();
                    return true;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }
    }
}
