using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using System.Text;
using Common.Services;
using Common.Entities;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Entities.Statistics;
using Common.Entities.Other;
using Common.Utilities.Helpers;
using SmartSystem.WeiXinInerface;

namespace SmartSystem.OmnipotentCardWebUI.Areas.BaseData.Controllers
{
    public class MessageController : BaseController
    {
        public ActionResult Index()
        {
             
            return View();
        }
        /// <summary>
        /// 获取通知数据
        /// </summary>
        /// <returns></returns>
        public string GetMessageData(string text,string starttime,string endtime)
        {
            try
            {
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]); 
                Pagination pagination = MessageServices.GetMessageData(text, starttime, endtime, rows, page);
                StringBuilder sb = new StringBuilder();
                string str = JsonHelper.GetJsonString(pagination.MessageList);
                sb.Append("{");
                sb.Append("\"total\":" + pagination.Total + ",");
                sb.Append("\"rows\":" + str + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 获取操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMessageOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010109").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01010901":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "添加";
                            option.handler = "Add";
                            option.sort = 1;
                            options.Add(option);
                            break;
                        }
                    case "PK01010902":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "Update";
                            option.sort = 2;
                            options.Add(option);
                            break;
                        }
                    case "PK01010903":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "删除";
                            option.handler = "Delete";
                            option.sort = 3;
                            options.Add(option);
                            break;
                        }
                    case "PK01010904":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "发送通知";
                            option.handler = "send";
                            option.iconCls = "icon-ok";
                            option.sort = 4;
                            options.Add(option);
                            break;
                        }
                } 
            } 
            SystemOperatePurview rtoolbar = new SystemOperatePurview();
            rtoolbar.text = "刷新";
            rtoolbar.handler = "Refresh";
            rtoolbar.sort = 5;
            options.Add(rtoolbar);

            result.Data = options.OrderBy(p => p.sort);
            return result;
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01010901,PK01010902")]
        public JsonResult EditMessage(Message model)
        {

            try
            {
                model.UserID = GetLoginUser.RecordID;
                model.UserName = GetLoginUser.UserName;
                if (string.IsNullOrWhiteSpace(model.RecordID))
                {
                    model.UserID = GetLoginUser.RecordID;
                    model.UserName = GetLoginUser.UserName;
                    bool result = MessageServices.Add(model);
                    if (!result) throw new MyException("保存失败");
                }
                else
                {
                    bool result = MessageServices.Update(model);
                    if (!result) throw new MyException("保存失败");
                }

               
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存黑名单信息失败");
                return Json(MyResult.Error("保存失败"));
            }
        }

        [HttpPost]
        [CheckPurview(Roles = "PK01010903")]
        public JsonResult Delete(string recordId)
        {
            try
            {
                bool result = MessageServices.Delete(recordId);
                if (!result) throw new MyException("删除失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除信息失败");
                return Json(MyResult.Error("删除失败"));
            }
        }

        [HttpPost]
        [CheckPurview(Roles = "PK01010904")]
        public JsonResult Send(string title,string text)
        {
            try
            { 
                //调接口 发数据
                PostService.SendNotify(title, text); 
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除信息失败");
                return Json(MyResult.Error("删除失败"));
            }
        }
    }
}