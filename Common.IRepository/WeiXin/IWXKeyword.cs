using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.Entities.Enum;
using Common.DataAccess;

namespace Common.IRepository.WeiXin
{
    public interface IWXKeyword
    {
        bool Create(WX_Keyword model);

        bool Update(WX_Keyword model);

        bool UpdateReplyType(int id, ReplyType type);

        bool UpdateReplyType(int id, ReplyType type, DbOperator dbOperator);

        bool Delete(int id, DbOperator dbOperator);

        WX_Keyword QueryById(int id);

        WX_Keyword QueryByReplyType(string companyId, ReplyType type, string keyValue);

        bool CheckKeyWord(string companyId, string keyValue, ReplyType rType, int? notContainsId = null);

        List<WX_Keyword> QueryALL(string companyId);

        List<WX_Keyword> QueryByReplyType(string companyId, ReplyType type);

        List<WX_Keyword> QueryKeyWordByIds(List<int> ids);

        List<WX_Keyword> QueryList(string companyId, ReplyType type, string keyValue);
    }
}
