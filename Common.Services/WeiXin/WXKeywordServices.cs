using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.IRepository.WeiXin;
using Common.Factory.WeiXin;
using Common.Entities.Enum;
using Common.DataAccess;
using Common.Entities;

namespace Common.Services.WeiXin
{
    public class WXKeywordServices
    {
        public static bool Create(WX_Keyword model)
        {
            IWXKeyword factory = WXKeywordFactory.GetFactory();
            WX_Keyword dbModel = factory.QueryByReplyType(model.CompanyID, model.ReplyType, model.Keyword);
            if (dbModel != null) throw new MyException("关键字已存在");
            bool result = factory.Create(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<WX_Keyword>(model, OperateType.Add);
            }
            return result;
        }

        public static bool Update(WX_Keyword model)
        {
            IWXKeyword factory = WXKeywordFactory.GetFactory();
            WX_Keyword dbModel = factory.QueryByReplyType(model.CompanyID, model.ReplyType, model.Keyword);
            if (dbModel != null && dbModel.ID != model.ID) throw new MyException("关键字已存在");
            bool result = factory.Update(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<WX_Keyword>(model, OperateType.Update);
            }
            return result;
        }

        public static bool UpdateReplyType(int id, ReplyType type)
        {
            if (id < 1) throw new ArgumentNullException("id");

            IWXKeyword factory = WXKeywordFactory.GetFactory();
            bool result = factory.UpdateReplyType(id, type);
            if (result)
            {
                OperateLogServices.AddOperateLog(string.Format("编号：{0}，修改为：{1}", id, type.GetDescription()), OperateType.Update);
            }
            return result;
        }
        public static bool UpdateForDefault(string companyId, int id)
        {
            if (id < 1) throw new ArgumentNullException("id");

            IWXKeyword factory = WXKeywordFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    List<WX_Keyword> keywords = factory.QueryByReplyType(companyId, ReplyType.Default).Where(p => p.ID != id).ToList();
                    foreach (var item in keywords)
                    {
                        bool result = factory.UpdateReplyType(item.ID, ReplyType.AutoReplay);
                        if (!result) throw new MyException("修改原始默认回复失败");
                    }
                    bool r = factory.UpdateReplyType(id, ReplyType.Default);
                    if (!r) throw new MyException("修改为默认回复失败");
                    dbOperator.CommitTransaction();
                    if (r)
                    {
                        OperateLogServices.AddOperateLog(string.Format("编号：{0}，修改为：{1}", id, ReplyType.AutoReplay.GetDescription()), OperateType.Update);
                    }
                    return r;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }
        public static bool UpdateForSubscribe(string companyId, int id)
        {
            if (id < 1) throw new ArgumentNullException("id");

            IWXKeyword factory = WXKeywordFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    List<WX_Keyword> keywords = factory.QueryByReplyType(companyId, ReplyType.Subscribe).Where(p => p.ID != id).ToList();
                    foreach (var item in keywords)
                    {
                        bool result = factory.UpdateReplyType(item.ID, ReplyType.AutoReplay);
                        if (!result) throw new MyException("修改原始关注回复失败");
                    }
                    bool r = factory.UpdateReplyType(id, ReplyType.Subscribe);
                    if (!r) throw new MyException("修改为关注回复失败");
                    dbOperator.CommitTransaction();
                    if (r)
                    {
                        OperateLogServices.AddOperateLog(string.Format("编号：{0}，修改为：{1}", id, ReplyType.AutoReplay.GetDescription()), OperateType.Update);
                    }
                    return r;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }
        public static bool Delete(string companyId, int id)
        {
            if (id < 1) throw new ArgumentNullException("Id");

            IWXKeyword factory = WXKeywordFactory.GetFactory();
            IWXMenu menufactory = WXMenuFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    List<WX_Menu> menus = menufactory.GetMenuByKeyId(companyId, MenuType.GKeyValue, id);
                    if (menus.Count > 0) throw new MyException("微信菜单上已绑定该关键字，请解除绑定后再删除");

                    bool result = factory.Delete(id, dbOperator);
                    if (!result) throw new MyException("删除失败");
                    dbOperator.CommitTransaction();
                    if (result)
                    {
                        OperateLogServices.AddOperateLog(string.Format("删除编号：{0}", id), OperateType.Delete);
                    }
                    return result;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }

        public static WX_Keyword QueryById(int id)
        {
            if (id < 1) throw new ArgumentNullException("id");

            IWXKeyword factory = WXKeywordFactory.GetFactory();
            return factory.QueryById(id);
        }

        public static WX_Keyword QueryByReplyType(string companyId, ReplyType type, string keyValue)
        {
            IWXKeyword factory = WXKeywordFactory.GetFactory();
            return factory.QueryByReplyType(companyId, type, keyValue);
        }

        public static bool CheckKeyWord(string companyId, string keyValue, ReplyType rType, int? notContainsId = null)
        {
            if (keyValue.IsEmpty()) throw new ArgumentNullException("keyValue");

            IWXKeyword factory = WXKeywordFactory.GetFactory();
            return factory.CheckKeyWord(companyId, keyValue, rType, notContainsId);
        }

        public static List<WX_Keyword> QueryALL(string companyId)
        {
            IWXKeyword factory = WXKeywordFactory.GetFactory();
            return factory.QueryALL(companyId);
        }

        public static List<WX_Keyword> QueryKeyWordByIds(List<int> ids)
        {
            if (ids == null || ids.Count == 0) throw new ArgumentNullException("ids");

            IWXKeyword factory = WXKeywordFactory.GetFactory();
            return factory.QueryKeyWordByIds(ids);
        }

        public static List<WX_Keyword> QueryList(string companyId, ReplyType type, string keyValue)
        {
            IWXKeyword factory = WXKeywordFactory.GetFactory();
            return factory.QueryList(companyId, type, keyValue);
        }
    }
}
