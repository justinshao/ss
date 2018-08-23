using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.DataAccess;

namespace Common.IRepository.WeiXin
{
    public interface IWXArticle
    {
        bool Create(WX_Article model, DbOperator dbOperator);

        bool Update(WX_Article model, DbOperator dbOperator);

        bool Delete(int id);

        bool Delete(int id, DbOperator dbOperator);

        bool DeleteByGroupID(string groupId);

        bool DeleteByGroupID(string groupId, DbOperator dbOperator);

        List<WX_Article> QueryAll(string companyId);

        List<WX_Article> QueryByGroupID(string groupId);

        WX_Article QueryById(int id);
    }
}
