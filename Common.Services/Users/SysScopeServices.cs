using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Factory;
using Common.IRepository;
using Common.Utilities;

namespace Common.Services
{
    public class SysScopeServices
    {
        public static bool Add(SysScope model) {
            if (model == null) throw new ArgumentNullException("model");

            model.ASID = GuidGenerator.GetGuid().ToString();
            ISysScope factory = SysScopeFactory.GetFactory();
            bool result = factory.Add(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<SysScope>(model, OperateType.Add);
            }
            return result;
        }

        public static bool Update(SysScope model)
        {
            if (model == null) throw new ArgumentNullException("model");

            ISysScope factory = SysScopeFactory.GetFactory();
            bool result = factory.Update(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<SysScope>(model, OperateType.Update);
            }
            return result;
        }

        public static bool DeleteByRecordId(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            ISysScope factory = SysScopeFactory.GetFactory();
            SysScope scope = factory.QuerySysScopeByRecordId(recordId);
            if (scope != null && scope.IsDefaultScope == YesOrNo.Yes)
                throw new MyException("默认作用域不能删除");

            bool result = factory.DeleteByRecordId(recordId);
            if (result)
            {
                OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("RecordId:{0}", recordId));
            }
            return result;
        }

        public static SysScope QuerySysScopeByRecordId(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            ISysScope factory = SysScopeFactory.GetFactory();
            return factory.QuerySysScopeByRecordId(recordId);
        }

        public static List<SysScope> QuerySysScopeByCompanyId(string companyId)
        {
            if (string.IsNullOrWhiteSpace(companyId)) throw new ArgumentNullException("companyId");

            ISysScope factory = SysScopeFactory.GetFactory();
            return factory.QuerySysScopeByCompanyId(companyId);
        }
        public static List<SysScope> QuerySysScopeByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException("userId");

            ISysScope factory = SysScopeFactory.GetFactory();
            return factory.QuerySysScopeByUserId(userId);
        }
    }
}
