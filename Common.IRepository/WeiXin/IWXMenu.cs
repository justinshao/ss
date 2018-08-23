using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.DataAccess;
using Common.Entities.Enum;

namespace Common.IRepository.WeiXin
{
    public interface IWXMenu
    {
        List<WX_Menu> GetMenus(string companyId);

        bool Create(WX_Menu model);

        bool Update(WX_Menu model);

        bool UpdateMenuName(int menuId, string menuName, int seq);

        bool UpdateMenuKeyId(int menuId, int? keyId);

        bool UpdateMenuKeyId(int menuId, int? keyId, DbOperator dbOperator);

        bool UpdateMenuKeyIdToNull(string companyId, int keyId, DbOperator dbOperator);

        bool Delete(int menuId);

        List<WX_Menu> GetMenuByKeyId(string companyId, MenuType type, int keyId);
    }
}
