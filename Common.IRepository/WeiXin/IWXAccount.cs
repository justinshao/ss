using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.DataAccess;

namespace Common.IRepository.WeiXin
{
    public interface IWXAccount
    {
        List<WX_Account> Search_WXAccount(string companyId,string accountname, string mobile, DateTime? starttime, DateTime? endtime, int PageSize, int PageIndex);
        int Search_WXAccount_Count(string companyId,string accountname, string mobile, DateTime? starttime, DateTime? endtime);

        bool AddWXAccount(WX_Info model, DbOperator dbOperator);

        bool AddWXInfo(WX_Info model, DbOperator dbOperator);

        WX_Info QueryWXInfoByMobilePhone(string mobilePhone, string companyId);
    }
}
