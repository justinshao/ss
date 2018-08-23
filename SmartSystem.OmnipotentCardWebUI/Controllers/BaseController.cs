using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Utilities;
using Common.Entities;
using Common.Services;

namespace SmartSystem.OmnipotentCardWebUI.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 当前登录用户单位
        /// </summary>
        public string GetCurrentUserCompanyId {
            get {
                return GetLoginUser.CPID;
            }
        }
        /// <summary>
        /// 当前登录用户
        /// </summary>
        public SysUser GetLoginUser {
            get {
                if (Session["SmartSystem_SystemLoginUser"] != null)
                {
                    return Session["SmartSystem_SystemLoginUser"] as SysUser;
                }
                return null;
            }
            set {
                Session["SmartSystem_SystemLoginUser"] = value;
            }
        }
        /// <summary>
        /// 当前登录用户所属单位
        /// </summary>
        public BaseCompany GetLoginUserCompany {
            get {
                if (Session["SmartSystem_LoginUserCompany"] != null)
                {
                    return Session["SmartSystem_LoginUserCompany"] as BaseCompany;
                }
               BaseCompany company = CompanyServices.QueryCompanyByRecordId(GetLoginUser.CPID);
               Session["SmartSystem_LoginUserCompany"] = company;
               return company;
            }
        }
        /// <summary>
        /// 当前登录用户拥有的小区
        /// </summary>
        public List<BaseVillage> GetLoginUserVillages {
            get {
                if (Session["SmartSystem_LoginUser_ValidVillage"] != null)
                {
                    return Session["SmartSystem_LoginUser_ValidVillage"] as List<BaseVillage>;
                }
                return new List<BaseVillage>();
            }
        }
        /// <summary>
        /// 当前登录用户拥有的单位
        /// </summary>
        public List<BaseCompany> GetLoginUserRoleCompany
        {
            get
            {
                if (Session["SmartSystem_LoginUser_ValidCompany"] != null)
                {
                    return Session["SmartSystem_LoginUser_ValidCompany"] as List<BaseCompany>;
                }
                return new List<BaseCompany>();
            }
        }
        /// <summary>
        /// 当前登录用户的模块权限
        /// </summary>
        public List<SysRoleAuthorize> GetLoginUserRoleAuthorize
        {
            get
            {
                if (Session["SmartSystem_LoginUser_SysRoleAuthorize"] != null)
                {
                    return Session["SmartSystem_LoginUser_SysRoleAuthorize"] as List<SysRoleAuthorize>;
                }
                return new List<SysRoleAuthorize>();
            }
        } 
    }
}
