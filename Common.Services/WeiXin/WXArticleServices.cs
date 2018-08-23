using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.Factory.WeiXin;
using Common.IRepository.WeiXin;
using Common.DataAccess;
using Common.Entities;

namespace Common.Services.WeiXin
{
    public class WXArticleServices
    {
       public static bool Create(List<WX_Article> models) {
            if (models == null || models.Count == 0) throw new ArgumentNullException("models");

            IWXArticle factory = WXArticleFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection()) {
                try
                {
                    dbOperator.BeginTransaction();

                    factory.DeleteByGroupID(models.First().GroupID, dbOperator);
                    foreach (var item in models)
                    {
                       bool result = factory.Create(item, dbOperator);
                       if (!result) throw new MyException("添加图文失败");
                       OperateLogServices.AddOperateLog<WX_Article>(item, OperateType.Add);
                    }
                 
                    dbOperator.CommitTransaction();
                    return true;
                }
                catch {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }

        public static bool Delete(int id) {
            if (id < 1) throw new ArgumentNullException("id");

            IWXArticle factory = WXArticleFactory.GetFactory();
            bool result = factory.Delete(id);
            if (result)
            {
                OperateLogServices.AddOperateLog("删除编号:"+id, OperateType.Delete);
            }
            return result;
        }

        public static bool DeleteByGroupID(string groupId)
        {
            if (groupId.IsEmpty()) throw new ArgumentNullException("groupId");

            IWXArticle factory = WXArticleFactory.GetFactory();
            bool result = factory.DeleteByGroupID(groupId);
            if (result)
            {
                OperateLogServices.AddOperateLog("groupId:" + groupId, OperateType.Delete);
            }
            return result;
        }

        public static List<WX_Article> QueryAll(string companyId)
        {
            IWXArticle factory = WXArticleFactory.GetFactory();
            return factory.QueryAll(companyId);
        }

        public static List<WX_Article> QueryByGroupID(string groupId)
        {
            if (groupId.IsEmpty()) throw new ArgumentNullException("groupId");

            IWXArticle factory = WXArticleFactory.GetFactory();
            return factory.QueryByGroupID(groupId);
        }

        public static WX_Article QueryById(int id)
        {
            if (id < 1) throw new ArgumentNullException("id");

            IWXArticle factory = WXArticleFactory.GetFactory();
            return factory.QueryById(id);
        }
    }
}
