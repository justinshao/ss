using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;

namespace Common.IRepository
{
    public interface IVillage
    {
        bool Add(BaseVillage model);
        bool Add(BaseVillage model, DbOperator dbOperator);
        bool Update(BaseVillage model);

        bool Delete(string recordId);

        BaseVillage QueryVillageByProxyNo(string proxyNo);

        BaseVillage QueryVillageByVillageNo(string villageNo, string companyId);

        List<BaseVillage> QueryVillageByUserId(string userId);
        List<BaseVillage> QueryVillageAll();
        List<BaseVillage> QueryPage(string companyId, int pageIndex, int pageSize, out int totalRecord);

        List<BaseVillage> QueryPage(List<string> villageIds,string companyId, int pageIndex, int pageSize, out int totalRecord);

        List<BaseVillage> QueryVillageByCompanyId(string companyId);

        List<BaseVillage> QueryVillageByCompanyIds(List<string> companyIds);

        BaseVillage QueryVillageByRecordId(string recordId);

        List<BaseVillage> QueryVillageByRecordIds(List<string> recordIds);

        List<BaseVillage> QueryVillageByEmployeeMobilePhone(string mobilePhone);
    }
}
