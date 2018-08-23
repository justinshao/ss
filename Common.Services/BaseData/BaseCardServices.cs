using Common.Entities;
using Common.Factory.BaseData;
using Common.IRepository.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Core.Expands;
using Common.DataAccess;
using Common.Utilities;

namespace Common.Services.BaseData
{
    public class BaseCardServices
    {
        public static BaseCard GetBaseCard(string cardID, out string ErrorMessage)
        {
            if (cardID.IsEmpty()) throw new ArgumentNullException("cardID");

            IBaseCard factory = BaseCardFactory.GetFactory();
            return factory.GetBaseCard(cardID, out ErrorMessage);
        }

        public static bool ModifyBaseCard(BaseCard mode, out string ErrorMessage)
        {
            if (mode==null) throw new ArgumentNullException("mode");

            IBaseCard factory = BaseCardFactory.GetFactory();
            return factory.ModifyBaseCard(mode, out ErrorMessage);
        }
        public static List<BaseCard> QueryBaseCardByVillageId(string villageId)
        {
            if (villageId.IsEmpty()) throw new ArgumentNullException("villageId");

            IBaseCard factory = BaseCardFactory.GetFactory();
            return factory.QueryBaseCardByVillageId(villageId);
        }

        public static bool Add(BaseCard model) {
            if (model == null) throw new ArgumentNullException("model");

            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    IBaseCard factory = BaseCardFactory.GetFactory();
                    model.CardID = GuidGenerator.GetGuidString();
                    bool result = factory.Add(model, dbOperator);
                    if (!result) throw new MyException("添加卡信息失败");
                    dbOperator.CommitTransaction();
                    return result;
                }
                catch {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }

        public static bool Update(BaseCard model)
        {
            if (model == null) throw new ArgumentNullException("model");

            IBaseCard factory = BaseCardFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = factory.Update(model, dbOperator);
                    if (!result) throw new MyException("修改卡信息失败");
                    dbOperator.CommitTransaction();
                    return result;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }

        public static bool CancelEmployeeIdByCardId(string cardId)
        {
            if (cardId.IsEmpty()) throw new ArgumentNullException("cardId");

            IBaseCard factory = BaseCardFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = factory.CancelEmployeeIdByCardId(cardId, dbOperator);
                    if (!result) throw new MyException("取消卡绑定用户信息失败");
                    dbOperator.CommitTransaction();
                    return result;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }

        public static bool CancelEmployeeIdByEmployeeId(string employeeId)
        {
            if (employeeId.IsEmpty()) throw new ArgumentNullException("employeeId");

            IBaseCard factory = BaseCardFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = factory.CancelEmployeeIdByEmployeeId(employeeId, dbOperator);
                    if (!result) throw new MyException("取消卡绑定用户信息失败");
                    dbOperator.CommitTransaction();
                    return result;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }
        public static bool LogOut(string cardId)
        {
            if (cardId.IsEmpty()) throw new ArgumentNullException("cardId");

            IBaseCard factory = BaseCardFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = factory.LogOut(cardId, dbOperator);
                    if (!result) throw new MyException("注销卡失败");
                    dbOperator.CommitTransaction();
                    return result;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }

        public static bool IssueCard(string employeeId, string cardId, string carNo)
        {
            if (employeeId.IsEmpty()) throw new ArgumentNullException("employeeId");
            if (cardId.IsEmpty()) throw new ArgumentNullException("cardId");
            if (carNo.IsEmpty()) throw new ArgumentNullException("carNo");

            IBaseCard factory = BaseCardFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = factory.IssueCard(employeeId,cardId,carNo,dbOperator);
                    if (!result) throw new MyException("发卡失败");
                    dbOperator.CommitTransaction();
                    return result;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }

        public static bool Recharge(string cardId, decimal balance)
        {
            if (cardId.IsEmpty()) throw new ArgumentNullException("cardId");
            if (balance <= 0) throw new ArgumentNullException("balance");

            IBaseCard factory = BaseCardFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = factory.Recharge(cardId, balance, dbOperator);
                    if (!result) throw new MyException("卡片充值失败");
                    dbOperator.CommitTransaction();
                    return result;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }

        public static bool Delete(string cardId)
        {
            if (cardId.IsEmpty()) throw new ArgumentNullException("cardId");

            IBaseCard factory = BaseCardFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = factory.Delete(cardId,dbOperator);
                    if (!result) throw new MyException("删除卡失败");
                    dbOperator.CommitTransaction();
                    return result;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }

        public static BaseCard QueryBaseCard(string villageId, string cardNo)
        {
            if (villageId.IsEmpty()) throw new ArgumentNullException("villageId");
            if (cardNo.IsEmpty()) throw new ArgumentNullException("cardNo");

            IBaseCard factory = BaseCardFactory.GetFactory();
            return factory.QueryBaseCard(villageId, cardNo);
        }
        public static BaseCard QueryBaseCardByParkingId(string parkingId, string cardNo)
        {
            if (parkingId.IsEmpty()) throw new ArgumentNullException("parkingId");
            if (cardNo.IsEmpty()) throw new ArgumentNullException("cardNo");

            IBaseCard factory = BaseCardFactory.GetFactory();
            return factory.QueryBaseCardByParkingId(parkingId, cardNo);
        }
        public static BaseCard QueryBaseCard(string villageId, string cardNo, CardType cardType)
        {
            if (villageId.IsEmpty()) throw new ArgumentNullException("villageId");
            if (cardNo.IsEmpty()) throw new ArgumentNullException("cardNo");

            IBaseCard factory = BaseCardFactory.GetFactory();
            return factory.QueryBaseCard(villageId, cardNo, cardType);
        }

        public static BaseCard QueryBaseCard(string villageId, CardType cardType, string cardNumber)
        {
            if (villageId.IsEmpty()) throw new ArgumentNullException("villageId");
            if (cardNumber.IsEmpty()) throw new ArgumentNullException("cardNo");

            IBaseCard factory = BaseCardFactory.GetFactory();
            return factory.QueryBaseCard(villageId, cardType, cardNumber);
        }

        public static BaseCard QueryBaseCardByCardNumber(string villageId, string cardNumber)
        {
            if (villageId.IsEmpty()) throw new ArgumentNullException("villageId");
            if (cardNumber.IsEmpty()) throw new ArgumentNullException("cardNo");

            IBaseCard factory = BaseCardFactory.GetFactory();
            return factory.QueryBaseCardByCardNumber(villageId,cardNumber);
        }

        public static List<BaseCard> QueryBaseCardByEmployeeId(string employeeId)
        {
            if (employeeId.IsEmpty()) throw new ArgumentNullException("employeeId");

            IBaseCard factory = BaseCardFactory.GetFactory();
            return factory.QueryBaseCardByEmployeeId(employeeId);
        }

        public static List<BaseCard> QueryPage(string villageId, string employeeId, string cardNo, CardType? cardType, int pagesize, int pageindex, out int total)
        {
            if (villageId.IsEmpty()) throw new ArgumentNullException("villageId");

            IBaseCard factory = BaseCardFactory.GetFactory();
            return factory.QueryPage(villageId,employeeId,cardNo,cardType,pagesize,pageindex,out total);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="villageId"></param>
        /// <param name="condition">人员姓名、电话</param>
        /// <returns></returns>
        public static List<BaseCard> QueryBaseCardByEmployeeInfo(string villageId, string condition)
        {
            if (villageId.IsEmpty()) throw new ArgumentNullException("villageId");
            if (condition.IsEmpty()) throw new ArgumentNullException("condition");

            IBaseCard factory = BaseCardFactory.GetFactory();
            return factory.QueryBaseCardByEmployeeInfo(villageId, condition);
        }

        public static List<BaseCard> QueryBaseCardByEmployeeMobile(string villageId, string mobile)
        {
            if (villageId.IsEmpty()) throw new ArgumentNullException("villageId");
            if (mobile.IsEmpty()) throw new ArgumentNullException("mobile");

            IBaseCard factory = BaseCardFactory.GetFactory();
            return factory.QueryBaseCardByEmployeeMobile(villageId, mobile);
        }
        public static DateTime CalculateNewEndDate(DateTime startTime, DateTime? oldEndDate, int month)
        {
            DateTime newEndDate = DateTime.Now.Date;
            if (!oldEndDate.HasValue)
            {
                newEndDate = startTime;
            }
            else
            {
                if (oldEndDate.Value.Date > startTime.Date)
                {
                    if (oldEndDate.Value.Date > DateTime.Now.Date)
                    {
                        newEndDate = oldEndDate.Value.Date.AddDays(1);
                    }
                    else
                    {
                        newEndDate = oldEndDate.Value.Date;
                    }

                }
                else
                {
                    newEndDate = startTime.Date;
                }
            }
            return newEndDate.Date.AddMonths(month).AddDays(-1);
        }

        public static bool AddOrUpdateCard(BaseCard card,DbOperator dbOperator)
        {
            IBaseCard cardFactory = BaseCardFactory.GetFactory();
            string errorMsg = string.Empty;
            BaseCard oldCard = cardFactory.GetBaseCard(card.CardID, out errorMsg);
            if (oldCard == null)
            {
                return cardFactory.Add(card, dbOperator);
            }
            else
            {
                return cardFactory.Update(card, dbOperator);
            }
        }
    }
}
