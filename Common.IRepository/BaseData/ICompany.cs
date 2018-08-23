using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;

namespace Common.IRepository
{
    public interface ICompany
    {
        bool Add(BaseCompany model);

        bool Add(BaseCompany model, DbOperator dbOperator);

        bool Update(BaseCompany model);

        bool Update(BaseCompany model, DbOperator dbOperator);

        bool Delete(string recordId, DbOperator dbOperator);

        List<BaseCompany> QueryCompanyByRecordIds(List<string> recordIds);

        List<BaseCompany> QueryCompanysByMasterID(string masterId);

        BaseCompany QueryCompanyByRecordId(string recordId);

        BaseCompany QueryCompanyByCompanyName(string companyName);

        List<BaseCompany> QueryCompanyAndSubordinateCompany(string recordId);

        BaseCompany QueryTopCompanyByRecordId(string recordId);

        bool SystemExistCompany();

        BaseCompany QueryByParkingId(string parkingId);

        BaseCompany QueryByBoxID(string boxId);
        List<BaseCompany> QueryAllCompanyByName(string str);
        List<BaseCompany> QuerymasterCompanyBymodels(List<BaseCompany> models);
        List<BaseCompany> QuerySubordinateCompanyBymodels(List<BaseCompany> models);

    }
}
