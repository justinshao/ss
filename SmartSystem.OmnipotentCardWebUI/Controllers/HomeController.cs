using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Common.Services.Statistics;
using Common.Entities;
using Common.Services;
using Common.Core;
namespace SmartSystem.OmnipotentCardWebUI.Controllers
{
    /// <summary>
    /// 首页
    /// </summary>
    public class HomeController : BaseController
    {
        [CheckPurview(Roles = "")]
        public ActionResult Index()
        {
            if (Session["SmartSystem_SystemLoginUser"] == null) {
                string response_js = "<script>window.parent.location.href='/RedirectPage/LoginTimeOut';</script>";
                return Content(response_js);
            }
            ViewBag.CurrLoginUserName = GetLoginUser.UserAccount;

            string roleDes = string.Empty;
            List<SysRoles> sysRoles = SysRolesServies.QuerySysRolesByUserId(GetLoginUser.RecordID);
            if (sysRoles.Count > 0) {
                roleDes = sysRoles.First().RoleName;
                if (!string.IsNullOrWhiteSpace(roleDes) && roleDes.Length > 10) {
                    roleDes = roleDes.Substring(0, 10) + "...";
                }
                if (sysRoles.Count > 1) {
                    roleDes = roleDes + "、...";
                }
            }
            ViewBag.RoleDes = roleDes;
            return View(GetLoginUserRoleAuthorize);
        }
        public ActionResult Main() {
            //车场首页权限
           SysRoleAuthorize homeAuthorize = GetLoginUserRoleAuthorize.FirstOrDefault(p => p.ModuleID == "PK010507");
           if (homeAuthorize == null) {
               return RedirectToAction("Empty", "Home");
           }
            return View();
        }
        public ActionResult Empty() {
            return View();
        }
        /// <summary>
        /// 查询车场收入数据
        /// </summary>
        /// <returns></returns>
        public JsonResult Get_HomeData()
        {
            JsonResult json = new JsonResult();
            json.Data = StatisticsServices.GetHomeData(GetLoginUserVillages.Select(p => p.VID).ToList());
            return json;
        }
        [HttpPost]
        public JsonResult UpdateCurrLoginPwd(string oldPwd,string newPwd1,string newPwd2)
        {
            try
            {
                if (newPwd1 != newPwd2) throw new MyException("两次输入密码不匹配");

                SysUser user = SysUserServices.QuerySysUserByUserAccount(GetLoginUser.UserAccount);
                if (user == null) throw new MyException("用户不存在");

                if (!user.Password.Equals(MD5.Encrypt(oldPwd)))
                {
                    throw new MyException("原始密码不正确");
                }
                bool result = SysUserServices.ResetPassword(user.UserAccount, MD5.Encrypt(newPwd1));
                if (!result) throw new MyException("修改密码失败");

                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "修改登录密码失败");
                return Json(MyResult.Error("修改密码失败"));
            }
        }
    }
}
