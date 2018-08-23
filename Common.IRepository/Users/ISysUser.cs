using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;

namespace Common.IRepository
{
    public interface ISysUser
    {
   
        SysUser QuerySysUserByUserAccount(string userAccount);

        SysUser QuerySysUserByRecordId(string recordId);

        bool Update(SysUser model, DbOperator dbOperator);

        bool Add(SysUser model, DbOperator dbOperator);

        bool Add(SysUser model);

        bool Update(SysUser model);

        bool LoginError(string recordId);

        bool LoginErrorByUserId(string userAccount);

        bool LoginSuccess(string recordId);

        List<SysUser> QuerySysUserByParkingId(string parkingId);

        List<SysUser> QuerySysUserPage(string companyId, string username, int pagesize, int pageindex, out int total);

        bool Delete(string recordId);

        bool Delete(string recordId, DbOperator dbOperator);

        bool DeleteByCompanyId(string companyId, DbOperator dbOperator);

        bool ResetPassword(string userAccount, string newPassword);
        List<SysUser> QuerySysUserAll();
        List<SysUser> QuerySysUserByCompanys(List<string> companys);
    }
}
