using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using Common.Services.WeiXin;
using Common.Entities.WX;
using Common.Services;
using Common.Entities;
using Common.Entities.Enum;
using Common.Utilities.Helpers;
using SmartSystem.OmnipotentCardWebUI.Models;
using SmartSystem.WeiXinBase;
using System.IO;

namespace SmartSystem.OmnipotentCardWebUI.Areas.NWeiXin.Controllers
{
    /// <summary>
    /// 微信菜单管理
    /// </summary>
    [CheckPurview(Roles = "PK010905")]
    public class WXMenuController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public string GetMenuData(string companyId)
        {
            try
            {

                List<WX_Menu> menus = WXMenuServices.GetMenus(companyId);

                string json = "{\"rows\":[";
                List<WX_Menu> topMenus = menus.Where(p => p.MasterID == 0).OrderBy(p => p.Sort).ToList();
                foreach (WX_Menu dr in topMenus)
                {
                    var childs = menus.Where(p => p.MasterID == dr.ID).OrderBy(o => o.Sort);
                    string masterId = dr.MasterID!=0 ? dr.MasterID.ToString() : string.Empty;
                    string hasChildMenu = childs.Count() > 0 ? "1" : "0";
                    json += "{\"id\":\"" + dr.ID + "\"";
                    json += ",\"MenuName\":\"" + dr.MenuName + "\"";
                    json += ",\"Url\":\"" + dr.Url + "\"";
                    json += ",\"KeywordId\":\"" + GetKeywordId(dr) + "\"";
                    json += ",\"ModuleId\":\"" + GetModuleId(dr) + "\"";
                    json += ",\"CompanyID\":\"" + dr.CompanyID + "\"";
                    json += ",\"MenuType\":" + (int)dr.MenuType + "";
                    json += ",\"MenuTypeDes\":\"" + GetMenuTypeDescription(menus,dr.ID) + "\"";
                    json += ",\"MenuTypeValue\":\"" + GetMenuTypeValue(menus, dr.ID) + "\"";
                    json += ",\"Sort\":\"" + dr.Sort + "\"";
                    json += ",\"MasterID\":\"" + masterId + "\"";
                    json += ",\"MinIprogramAppId\":\"" + dr.MinIprogramAppId + "\"";
                    json += ",\"MinIprogramPagePath\":\"" + dr.MinIprogramPagePath + "\"";
                    json += ",\"HasChildMenu\":\"" + hasChildMenu + "\"";
                    json += ",\"iconCls\":\"my-menu-icon\"},";
                   
                    foreach (var obj in childs)
                    {
                        string childmasterId = obj.MasterID==0 ? obj.MasterID.ToString() : string.Empty;

                        json += "{\"id\":\"" + obj.ID + "\"";
                        json += ",\"MenuName\":\"" + obj.MenuName + "\"";
                        json += ",\"Url\":\"" + obj.Url + "\"";
                        json += ",\"KeywordId\":\"" + GetKeywordId(obj) + "\"";
                        json += ",\"ModuleId\":\"" + GetModuleId(obj) + "\"";
                        json += ",\"CompanyID\":\"" + dr.CompanyID + "\"";
                        json += ",\"MenuType\":" + (int)obj.MenuType + "";
                        json += ",\"MenuTypeDes\":\"" + GetMenuTypeDescription(menus, obj.ID) + "\"";
                        json += ",\"MenuTypeValue\":\"" + GetMenuTypeValue(menus, obj.ID) + "\"";
                        json += ",\"Sort\":\"" + obj.Sort + "\"";
                        json += ",\"MasterID\":\"" + obj.MasterID + "\"";
                        json += ",\"MinIprogramAppId\":\"" + obj.MinIprogramAppId + "\"";
                        json += ",\"MinIprogramPagePath\":\"" + obj.MinIprogramPagePath + "\"";
                        json += ",\"HasChildMenu\":\"0\"";
                        json += ",\"_parentId\":\"" + dr.ID + "\"";
                        json += ",\"iconCls\":\"my-menu-icon\"},";
                    }
                }
                if (topMenus.Count() > 0)
                {
                    json = json.Substring(0, json.Length - 1);
                }
                json += "]}";
                return json;
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "微信菜单管理获取菜单失败");
                return "{\"rows\":[]}";
            }
        }
        private string GetMenuTypeDescription(List<WX_Menu> menus,int id)
        {
            WX_Menu menu = menus.FirstOrDefault(p => p.ID == id);
            var childs = menus.Where(p => p.MasterID == id);
            if (menu.MasterID==0 && childs.Count() > 0)
            {
                return string.Empty;
            }
            return menu.MenuType.GetDescription();
        }
        private string GetMenuTypeValue(List<WX_Menu> menus, int id)
        {
            WX_Menu menu = menus.FirstOrDefault(p => p.ID == id);
            var childs = menus.Where(p => p.MasterID == id);
            if (menu.MasterID==0 && childs.Count() > 0)
            {
                return string.Empty;
            }
            if (menu.MenuType == MenuType.GKeyValue && menu.KeywordId!=0)
            {
               WX_Keyword keyword = WXKeywordServices.QueryById(menu.KeywordId);
               if (keyword != null) {
                   return keyword.Keyword;
               }
            }
            if (menu.MenuType == MenuType.Url)
            {
                return menu.Url;
            }
            if (menu.MenuType == MenuType.WeiXinModule && menu.KeywordId!=0)
            {
                return ((WeiXinModule)menu.KeywordId).GetDescription();
            }
            if (menu.MenuType == MenuType.MinIprogram) {
                return menu.MinIprogramPagePath;
            }
            return string.Empty;
        }
        private string GetKeywordId(WX_Menu menu) {
            if (menu.MenuType == MenuType.GKeyValue) {
                return menu.KeywordId.ToString();
            }
            return string.Empty;
        }
        private string GetModuleId(WX_Menu menu)
        {
            if (menu.MenuType == MenuType.WeiXinModule)
            {
                return menu.KeywordId.ToString();
            }
            return string.Empty;
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01090501,PK01090502")]
        public JsonResult AddOrUpdate(WX_Menu model)
        {
            try
            {
                model.MasterID = model.MasterID == -1 ? 0 : model.MasterID;
                CheckMenu(model.MasterID, model.ID,model.CompanyID);
                if (model.MenuType == Common.Entities.Enum.MenuType.Url)
                {
                    if (string.IsNullOrWhiteSpace(model.Url))
                    {
                        throw new MyException("请填写链接地址");
                    }
                    model.KeywordId=0;
                }
                if (model.MenuType == Common.Entities.Enum.MenuType.GKeyValue)
                {
                    if (model.KeywordId==0){
                        throw new MyException("请选择关键字");
                    }
                    model.Url = string.Empty;
                }
                if (model.MenuType == Common.Entities.Enum.MenuType.WeiXinModule)
                {
                    if (string.IsNullOrWhiteSpace(Request["Module"]))
                    {
                        throw new MyException("请选择系统模块");
                    }
                    model.KeywordId = int.Parse(Request["Module"].ToString());
                    model.Url = string.Empty;
                }
                if (model.MenuType == Common.Entities.Enum.MenuType.MinIprogram)
                {
                    if (string.IsNullOrWhiteSpace(model.Url))
                    {
                        throw new MyException("请填写链接地址");
                    }
                    if (string.IsNullOrWhiteSpace(model.MinIprogramAppId))
                    {
                        throw new MyException("请填写小程序APPID");
                    }
                    if (string.IsNullOrWhiteSpace(model.MinIprogramPagePath))
                    {
                        throw new MyException("请填写小程序的页面路径");
                    }
                }
                if (model.ID < 1)
                {
                    bool result = WXMenuServices.Create(model);
                    if (!result) throw new MyException("添加失败");
                }
                else
                {
                    bool result = WXMenuServices.Update(model);
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
                ExceptionsServices.AddExceptions(ex, "保存菜单信息失败");
                return Json(MyResult.Error("保存失败"));
            }
        }
        private void CheckMenu(int? masterId, int id, string companyId)
        {
            if (id > 1) return;

            List<WX_Menu> menus = WXMenuServices.GetMenus(companyId);
            if (masterId.HasValue)
            {
                if (menus.Count(p => p.MasterID == masterId) >= 5)
                {
                    throw new MyException("最多只能添加5个二级菜单");
                }
            }
            else {
                if (menus.Count(p => p.MasterID == null || p.MasterID==0) >= 3)
                {
                    throw new MyException("最多只能添加3个一级级菜单");
                }
            }
            
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01090503")]
        public JsonResult Delete(int id, string companyId)
        {
            try
            {
                bool result = WXMenuServices.Delete(companyId,id);
                if (!result) throw new MyException("删除菜单失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除菜单失败");
                return Json(MyResult.Error("删除菜单失败"));
            }
        }

        public JsonResult GetMenuTypeData()
        {
            JsonResult json = new JsonResult();
            json.Data = EnumHelper.GetEnumContextList(typeof(MenuType));
            return json;
        }
        public JsonResult GetWeiXinModuleData()
        {
            JsonResult json = new JsonResult();
            json.Data = EnumHelper.GetEnumContextList(typeof(WeiXinModule));
            return json;
        }
        public JsonResult GetMasterMenu(string companyId)
        {
            Dictionary<string, string> dicMenu = new Dictionary<string, string>();
            dicMenu.Add("-1", "一级菜单");
            JsonResult json = new JsonResult();
            List<WX_Menu> menus = WXMenuServices.GetMenus(companyId).Where(p => p.MasterID == null || p.MasterID==0).ToList();
            foreach (var item in menus) {
                dicMenu.Add(item.ID.ToString(), item.MenuName);
            }
            var result = from p in dicMenu
                         select new
                         {
                             id = p.Key,
                             text = p.Value
                         };
            json.Data = result;
            return json;
        }
        public JsonResult GetKeywordData(string companyId)
        {
            JsonResult json = new JsonResult();
            List<WX_Keyword> models = WXKeywordServices.QueryALL(companyId);
            var result = from p in models
                         select new
                         {
                             id = p.ID,
                             text = p.Keyword
                         };
            json.Data = result;
            return json;
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01090504")]
        public JsonResult PublishMenu(string companyId)
        {
            try
            {
                WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(companyId);
                if (config == null || string.IsNullOrWhiteSpace(config.AppId) || string.IsNullOrWhiteSpace(config.AppSecret)
                || string.IsNullOrWhiteSpace(config.SystemName)) {
                    throw new MyException("获取微信基础信息失败，请确认微信基础信息已配置");
                }
                var accessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret, false);
                var buttonGroup = ToButtonGroup(WXMenuServices.GetMenus(companyId));
                TxtLogServices.WriteTxtLogEx("PublishMenu", JsonHelper.GetJsonString(buttonGroup));
                var result = WxApi.CreateMenu(companyId,accessToken, buttonGroup);
                if (!result) throw new MyException("发布菜单失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "发布菜单失败");
                return Json(MyResult.Error("发布菜单失败"));
            }
        }
        private MainButton ToButtonGroup(List<WX_Menu> menuList)
        {
            var buttonGroup = new MainButton();
            if (menuList != null && menuList.Count>0)
            {
                WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(menuList.First().CompanyID);
                foreach (var pmenu in menuList.Where(o => o.MasterID == null || o.MasterID==0).OrderBy(o => o.Sort))
                {
                    var cmenu = menuList.Where(o => o.MasterID == pmenu.ID).ToList();
                    if (cmenu.Any())
                    {
                        var subButton = new SubButton { Name = pmenu.MenuName };
                        foreach (var smenu in cmenu.OrderBy(o => o.Sort))
                        {
                            if (smenu.MenuType == MenuType.Url || smenu.MenuType == MenuType.WeiXinModule)
                            {
                                subButton.SubButtons.Add(new SingleViewButton
                                {
                                    Name = smenu.MenuName,
                                    Url = GetWeiXinUrl(smenu.Url,smenu.KeywordId, smenu.ID, smenu.MenuType, config)
                                });
                            }
                            if (smenu.MenuType == MenuType.GKeyValue)
                            {
                                string keyvalue = GetKeyValue(smenu.KeywordId);
                                if (string.IsNullOrWhiteSpace(keyvalue))
                                {
                                    throw new MyException("获取关键字失败");
                                }
                                subButton.SubButtons.Add(new SingleClickButton
                                {
                                    Name = smenu.MenuName,
                                    Key = keyvalue
                                });
                            }
                            if (smenu.MenuType == MenuType.MinIprogram)
                            {
                                subButton.SubButtons.Add(new MinIprogramButton
                                {
                                    Name = smenu.MenuName,
                                    Url = smenu.Url,
                                    AppId = smenu.MinIprogramAppId,
                                    PagePath = smenu.MinIprogramPagePath
                                });
                            }
                        }
                        buttonGroup.Button.Add(subButton);
                    }
                    else
                    {

                        if (pmenu.MenuType == MenuType.Url || pmenu.MenuType == MenuType.WeiXinModule)
                        {
                            buttonGroup.Button.Add(new SingleViewButton
                            {
                                Name = pmenu.MenuName,
                                Url = GetWeiXinUrl(pmenu.Url,pmenu.KeywordId,pmenu.ID, pmenu.MenuType,config)
                            });
                        }
                        if (pmenu.MenuType == MenuType.GKeyValue)
                        {
                            string keyvalue = GetKeyValue(pmenu.KeywordId);
                            if (string.IsNullOrWhiteSpace(keyvalue))
                            {
                                throw new MyException("获取关键字失败");
                            }
                            buttonGroup.Button.Add(new SingleClickButton
                            {
                                Name = pmenu.MenuName,
                                Key = keyvalue
                            });
                        }
                        if (pmenu.MenuType == MenuType.MinIprogram)
                        {
                            buttonGroup.Button.Add(new MinIprogramButton
                            {
                                Name = pmenu.MenuName,
                                AppId = pmenu.MinIprogramAppId,
                                Url = pmenu.Url,
                                PagePath=pmenu.MinIprogramPagePath
                            });
                        }
                    }
                }
            }

            return buttonGroup;
        }
        private string GetKeyValue(int? keyId)
        {
            if (!keyId.HasValue)
                return string.Empty;

            WX_Keyword model = WXKeywordServices.QueryById(keyId.Value);
            return model == null ? string.Empty : model.Keyword;
        }
        private string GetWeiXinUrl(string url,int? moduleId, int menuId, MenuType type, WX_ApiConfig config)
        {
            if (type == MenuType.Url) {
                return url;
            }
            if (!moduleId.HasValue) return string.Empty;

            if (type != MenuType.WeiXinModule) return string.Empty;

            string link = ((WeiXinModule)moduleId.Value).GetEnumDefaultValue();
            if (!string.IsNullOrWhiteSpace(link)) {
                if (link.Contains("="))
                {
                    link = string.Format("{0}^cid={1}", link, config.CompanyID);
                }
                else {
                    link = string.Format("{0}_cid={1}", link, config.CompanyID);
                }
            }
            return string.Format("{0}{1}", config.Domain, link);
        }
        /// <summary>
        /// 获取关键字操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMenuOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010905").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01090501":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "添加";
                            option.handler = "Add";
                            option.sort = 1;
                            options.Add(option);
                            break;
                        }
                    case "PK01090502":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "Update";
                            option.sort = 2;
                            options.Add(option);
                            break;
                        }
                    case "PK01090503":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "删除";
                            option.handler = "Delete";
                            option.sort = 3;
                            options.Add(option);
                            break;
                        }
                    case "PK01090504":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "发布菜单";
                            option.handler = "PublishMenu";
                            option.iconCls = "icon-edit";
                            option.id = "btnpublishmenu";
                            option.sort = 4;
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
