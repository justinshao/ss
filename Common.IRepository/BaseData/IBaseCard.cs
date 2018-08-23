using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataAccess;

namespace Common.IRepository.BaseData
{
    public interface IBaseCard
    {
        BaseCard GetBaseCard(string cardID, out string ErrorMessage);

        bool ModifyBaseCard(BaseCard mode, out string ErrorMessage);

        List<BaseCard> QueryBaseCardByVillageId(string villageId);

        bool Add(BaseCard model);

        bool Add(BaseCard model, DbOperator dbOperator);

        bool Update(BaseCard model, DbOperator dbOperator);

        bool CancelEmployeeIdByCardId(string cardId, DbOperator dbOperator);

        bool CancelEmployeeIdByEmployeeId(string employeeId, DbOperator dbOperator);

        bool LogOut(string cardId, DbOperator dbOperator);

        bool IssueCard(string employeeId, string cardId, string carNo, DbOperator dbOperator);

       
        bool Recharge(string cardId, decimal balance,  DbOperator dbOperator);

        bool SetEndDate(string cardId, DateTime enddate, DbOperator dbOperator);
        bool Delete(string cardId);

        bool Delete(string cardId, DbOperator dbOperator);

        BaseCard QueryBaseCard(string villageId, string cardNo);

        BaseCard QueryBaseCardByParkingId(string parkingId, string cardNo);

        BaseCard QueryBaseCard(string villageId, string cardNo, CardType cardType);

        BaseCard QueryBaseCard(string villageId, CardType cardType, string cardNumber);

        BaseCard QueryBaseCardByCardNumber(string villageId, string cardNumber);

        List<BaseCard> QueryBaseCardByEmployeeId(string employeeId);

        List<BaseCard> QueryPage(string villageId, string employeeId, string cardNo, CardType? cardType, int pagesize, int pageindex, out int total);

        List<BaseCard> QueryBaseCardByEmployeeInfo(string villageId, string condition);

        List<BaseCard> QueryBaseCardByEmployeeMobile(string villageId,string mobile);
         
    }
}
