using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Services.WeiXin;
using Common.Entities.WX;
using Common.Entities;
using Common.Services;
using Common.Entities.Enum;
using Common.Utilities.Helpers;
using Common.Utilities;
using System.Text;
using SmartSystem.OmnipotentCardWebUI.Models;
using SmartSystem.OmnipotentCardWebUI.Controllers;

namespace SmartSystem.OmnipotentCardWebUI.Areas.NWeiXin.Controllers
{
    /// <summary>
    /// 微信图片管理
    /// </summary>
    [CheckPurview(Roles = "PK010903")]
    public class WXArticleController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetWXArticleData(string companyId)
        {
            JsonResult json = new JsonResult();
            try
            {

                List<WX_Article> models = WXArticleServices.QueryAll(companyId);
                List<string> groupIds = models.Select(p => p.GroupID).Distinct().ToList();
                var result = from p in groupIds
                             select new
                             {
                                 GroupId = p,
                                 Count = models.Count(o => o.GroupID == p),
                                 Title = models.OrderBy(o => o.Sort).First(o => o.GroupID == p).Title
                             };
                json.Data = result;
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "查询图文信息失败");
            }

            return json;
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01090303")]
        public JsonResult Delete(string groupId)
        {
            try
            {
                bool result = WXArticleServices.DeleteByGroupID(groupId);
                if (!result) throw new MyException("删除图文失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除图文失败");
                return Json(MyResult.Error("删除图文失败"));
            }
        }
        [CheckPurview(Roles = "PK01090301,PK01090302")]
        public ActionResult Edit(string groupId, string companyId)
        {
            ViewBag.GroupId = groupId;
            ViewBag.CompanyID = companyId;
            return View();
        }
        public JsonResult GetArticle(string groupId,string companyI)
        {
            try
            {
                List<WX_Article> models = new List<WX_Article>();
                if (!string.IsNullOrWhiteSpace(groupId))
                {
                    models = WXArticleServices.QueryByGroupID(groupId).OrderBy(p => p.Sort).ToList();
                }
                return Json(MyResult.Success(string.Empty, models));
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "根据分组编号查询图文信息失败");
                return Json(MyResult.Error("查询图文信息失败"));
            }
        }
        public JsonResult GetArticleTypeData()
        {
            JsonResult json = new JsonResult();
            json.Data = EnumHelper.GetEnumContextList(typeof(ArticleType));
            return json;
        }
        public JsonResult GetWeiXinModuleData()
        {
            JsonResult json = new JsonResult();
            json.Data = EnumHelper.GetEnumContextList(typeof(WeiXinModule));
            return json;
        }
        [HttpPost]
        [ValidateInput(false)]
        [CheckPurview(Roles = "PK01090301,PK01090302")]
        public JsonResult SaveArticle(string groupId, int itemnum, string jsonData)
        {
            try
            {
                string newGroupId = string.Empty;
                if (string.IsNullOrWhiteSpace(groupId))
                {
                    newGroupId = GuidGenerator.GetGuidString();
                }
                var dyArticles = JsonHelper.Parse(jsonData);
                List<WX_Article> models = new List<WX_Article>();
                for (var i = 0; i < itemnum; i++)
                {
                    WX_Article model = new WX_Article();
                    model.ID = int.Parse(dyArticles[i].id);
                    if (string.IsNullOrWhiteSpace(dyArticles[i].title))
                    {
                        throw new MyException(string.Format("获取第{0}个图文的标题失败", i + 1));
                    }
                    model.Title = dyArticles[i].title;
                    model.ImagePath = dyArticles[i].pic;
                    if (string.IsNullOrWhiteSpace(dyArticles[i].pic))
                    {
                        throw new MyException(string.Format("获取第{0}个图文的封面图片失败", i + 1));
                    }
                    model.Description = dyArticles[i].desc;
                    if (!string.IsNullOrWhiteSpace(groupId))
                    {
                        List<WX_Article> oldArticles = WXArticleServices.QueryByGroupID(groupId);
                        if (oldArticles.Count == 0) throw new MyException("获取单位编号失败");

                        model.CompanyID = oldArticles.First().CompanyID;
                    }
                    else {
                        model.CompanyID = dyArticles[i].companyid;
                    }
                   
                    model.Text = dyArticles[i].text;
                    model.ArticleType = (ArticleType)int.Parse(dyArticles[i].type);
                    if (model.ArticleType == ArticleType.Url && string.IsNullOrWhiteSpace(dyArticles[i].url))
                    {
                        throw new MyException(string.Format("获取第{0}个图文的链接地址失败", i + 1));
                    }
                    if (model.ArticleType == ArticleType.Module && string.IsNullOrWhiteSpace(dyArticles[i].moduleid))
                    {
                        throw new MyException(string.Format("获取第{0}个图文的模块失败", i + 1));
                    }
                    switch (model.ArticleType)
                    {
                        case ArticleType.Url:
                            {
                                model.Url = dyArticles[i].url;
                                break;
                            }
                        case ArticleType.Module:
                            {
                                model.Url = dyArticles[i].moduleid;
                                break;
                            }
                        default:
                            {
                                model.Url = string.Empty;
                                break;
                            }
                    }
                    model.Sort = (int)dyArticles[i].sort;
                    model.GroupID = string.IsNullOrWhiteSpace(groupId)?newGroupId:groupId;
                    model.CreateTime = DateTime.Now;
                    models.Add(model);
                }
                bool result = WXArticleServices.Create(models);
                if (!result) throw new MyException("保存图文失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存图文失败");
                return Json(MyResult.Error("保存图文失败"));
            }
        }

        /// <summary>
        /// 获取卡片操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetArticleOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010903").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01090301":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "添加";
                            option.handler = "Add";
                            option.sort = 1;
                            options.Add(option);
                            break;
                        }
                    case "PK01090302":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "Update";
                            option.sort = 2;
                            options.Add(option);
                            break;
                        }
                    case "PK01090303":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "删除";
                            option.handler = "Delete";
                            option.sort = 3;
                            options.Add(option);
                            break;
                        }
                }
            }
            SystemOperatePurview roption = new SystemOperatePurview();
            roption.text = "刷新";
            roption.handler = "Refresh";
            roption.sort = 6;
            options.Add(roption);

            result.Data = options.OrderBy(p => p.sort);
            return result;
        }
    }
}
