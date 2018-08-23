using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.Factory.WeiXin;
using Common.IRepository.WeiXin;
using Common.Entities.Enum;
using Common.Entities;

namespace Common.Services.WeiXin
{
    public class WXMenuServices
    {
        public static List<WX_Menu> GetMenus(string companyId)
        {
            IWXMenu factory = WXMenuFactory.GetFactory();
            return factory.GetMenus(companyId);
        }
        public static List<WX_Menu> GetMenuByKeyId(string companyId, MenuType type, int keyId)
        {
            IWXMenu factory = WXMenuFactory.GetFactory();
            return factory.GetMenuByKeyId(companyId, type, keyId);
        }
        public static bool Create(WX_Menu model)
        {
            IWXMenu factory = WXMenuFactory.GetFactory();
            bool result = factory.Create(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<WX_Menu>(model, OperateType.Add);
            }
            return result;
        }

        public static bool Update(WX_Menu model)
        {
            IWXMenu factory = WXMenuFactory.GetFactory();
            bool result = factory.Update(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<WX_Menu>(model, OperateType.Update);
            }
            return result;
        }

        public static bool UpdateMenuName(int menuId, string menuName, int seq)
        {
            if (menuId < 1) throw new ArgumentNullException("menuId");
            if (menuName.IsEmpty()) throw new ArgumentNullException("menuName");

            IWXMenu factory = WXMenuFactory.GetFactory();
            bool result = factory.UpdateMenuName(menuId, menuName, seq);
            if (result)
            {
                OperateLogServices.AddOperateLog(string.Format("编号：{0}，修改菜单名称为：{1}，序号为：{2}", menuId, menuName, seq), OperateType.Update);
            }
            return result;
        }

        public static bool UpdateMenuKeyId(int menuId, int? keyId)
        {
            if (menuId < 1) throw new ArgumentNullException("menuId");

            IWXMenu factory = WXMenuFactory.GetFactory();
            bool result = factory.UpdateMenuKeyId(menuId, keyId);
            if (result)
            {
                OperateLogServices.AddOperateLog(string.Format("编号：{0}，关键字为：{1}", menuId, keyId.HasValue ? keyId.Value.ToString() : string.Empty), OperateType.Update);
            }
            return result;
        }
        public static bool Delete(string companyId, int menuId)
        {
            if (menuId < 1) throw new ArgumentNullException("menuId");

            IWXMenu factory = WXMenuFactory.GetFactory();
            List<WX_Menu> models = factory.GetMenus(companyId);
            WX_Menu model = models.FirstOrDefault(p => p.ID == menuId);
            if (model != null && models.Count(p => p.MasterID == menuId) > 0)
            {
                throw new MyException("请先删除该菜单下的二级菜单");
            }
            bool result = factory.Delete(menuId);
            if (result)
            {
                OperateLogServices.AddOperateLog(string.Format("删除菜单编号为：{0}菜单", menuId), OperateType.Update);
            }
            return result;
        }
    }
}
