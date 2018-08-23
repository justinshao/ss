using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Entities.WX;
using Common.Services.WeiXin;
using Common.Services;
using Common.Entities;
using SmartSystem.OmnipotentCardWebUI.Models;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using Common.Entities.Enum;
using Common.Utilities.Helpers;

namespace SmartSystem.OmnipotentCardWebUI.Areas.NWeiXin.Controllers
{
    /// <summary>
    /// 微信关键字管理
    /// </summary>
    [CheckPurview(Roles = "PK010904")]
    public class WXKeywordController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetKeywordData()
        {
            JsonResult json = new JsonResult();
            try
            {
                if (string.IsNullOrWhiteSpace(Request["companyId"])) {
                    return json;
                }

                List<WX_Keyword> models = WXKeywordServices.QueryALL(Request["companyId"].ToString());
                var result = from p in models
                             select new
                             {
                                 Id = p.ID,
                                 Keyword = p.Keyword,
                                 KeywordType = p.KeywordType,
                                 KeywordTypeDes = p.KeywordType.GetDescription(),
                                 MatchType = p.MatchType,
                                 MatchTypeDes = p.MatchType.GetDescription(),
                                 ReplyType = p.ReplyType,
                                 ReplyTypeDes = p.ReplyType.GetDescription(),
                                 Text = p.Text,
                                 ArticleGroupID = p.ArticleGroupID,
                                 CompanyID=p.CompanyID
                             };
                json.Data = result;
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "查询关键字信息失败");
            }

            return json;
        }
        public JsonResult GetMatchTypeData()
        {
            JsonResult json = new JsonResult();
            json.Data = EnumHelper.GetEnumContextList(typeof(MatchType));
            return json;
        }
        public JsonResult GetKeywordTypeData()
        {
            JsonResult json = new JsonResult();
            json.Data = EnumHelper.GetEnumContextList(typeof(KeywordType));
            return json;
        }
        public JsonResult GetArticleData(string companyId)
        {
            JsonResult json = new JsonResult();
            List<WX_Article> models = WXArticleServices.QueryAll(companyId);
            if (models.Count > 0)
            {
                List<string> groupIds = models.Select(p => p.GroupID).Distinct().ToList();
                var result = from p in groupIds
                             select new
                             {
                                 id = p,
                                 text = models.OrderBy(o => o.Sort).First(o => o.GroupID == p).Title
                             };
                json.Data = result;
            }
            return json;
        }
        [HttpPost]
        [ValidateInput(false)]
        [CheckPurview(Roles = "PK01090401,PK01090402")]
        public JsonResult AddOrUpdate(WX_Keyword model)
        {
            try
            {
                if (model.KeywordType == KeywordType.Article)
                {
                    if (string.IsNullOrWhiteSpace(model.ArticleGroupID))
                    {
                        throw new MyException("请选择图文");
                    }
                    model.Text = string.Empty;
                }
                if (model.KeywordType == KeywordType.Text)
                {
                    if (string.IsNullOrWhiteSpace(model.Text))
                    {
                        throw new MyException("请填写回复内容");
                    }
                    model.ArticleGroupID = string.Empty;
                }
                model.ReplyType = ReplyType.AutoReplay;
                if (model.ID < 1)
                {
                    bool result = WXKeywordServices.Create(model);
                    if (!result) throw new MyException("添加失败");
                }
                else
                {
                    bool result = WXKeywordServices.Update(model);
                    if (!result) throw new MyException("修改失败");
                }
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存关键字信息失败");
                return Json(MyResult.Error("保存失败"));
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01090403")]
        public JsonResult Delete(string companyId,int id)
        {
            try
            {
                bool result = WXKeywordServices.Delete(companyId,id);
                if (!result) throw new MyException("删除关键字失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除关键字失败");
                return Json(MyResult.Error("删除关键字失败"));
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01090401,PK01090402")]
        public JsonResult UpdateForDefault(string companyId,int id)
        {
            try
            {
                bool result = WXKeywordServices.UpdateForDefault(companyId,id);
                if (!result) throw new MyException("设置默认回复失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "设置默认回复失败");
                return Json(MyResult.Error("设置默认回复失败"));
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01090401,PK01090402")]
        public JsonResult UpdateForSubscribe(string companyId,int id)
        {
            try
            {
                bool result = WXKeywordServices.UpdateForSubscribe(companyId,id);
                if (!result) throw new MyException("设置关注回复失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "设置关注回复失败");
                return Json(MyResult.Error("设置关注回复失败"));
            }
        }
        /// <summary>
        /// 获取关键字操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetKeyWordOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010904").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01090401":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "添加";
                            option.handler = "Add";
                            option.sort = 1;
                            options.Add(option);
                            break;
                        }
                    case "PK01090402":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "Update";
                            option.sort = 2;
                            options.Add(option);

                            SystemOperatePurview option1 = new SystemOperatePurview();
                            option1.text = "设为默认回复";
                            option1.handler = "UpdateForDefault";
                            option1.iconCls = "icon-edit";
                            option1.id = "updatefordefault";
                            option1.sort = 3;
                            options.Add(option1);

                            SystemOperatePurview option2 = new SystemOperatePurview();
                            option2.text = "设为关注回复";
                            option2.handler = "UpdateForSubscribe";
                            option2.iconCls = "icon-edit";
                            option2.id = "updateforsubscribe";
                            option2.sort = 4;
                            options.Add(option2);
                            break;
                        }
                    case "PK01090403":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "删除";
                            option.handler = "Delete";
                            option.sort = 5;
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
