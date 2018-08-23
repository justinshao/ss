using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Entities;
using Common.Services;

namespace SmartSystem.OmnipotentCardWebUI.Models
{
    public class CacheData
    {
        /// <summary>
        /// 缓存登录信息
        /// </summary>
        public static void CacheUserLoginData(SysUser user)
        {

            HttpContext.Current.Session["SmartSystem_SystemLoginUser"] = user;

            List<BaseVillage> villages = VillageServices.QueryVillageByUserId(user.RecordID);
            HttpContext.Current.Session["SmartSystem_LoginUser_ValidVillage"] = villages;

            HttpContext.Current.Session["SmartSystem_LoginUser_ValidCompany"] = CompanyServices.GetCurrLoginUserRoleCompany(user.CPID, user.RecordID);

            List<SysRoles> sysRoles = SysRolesServies.QuerySysRolesByUserId(user.RecordID);
            if (sysRoles != null)
            {
                HttpContext.Current.Session["SmartSystem_SystemLoginUser_Role"] = sysRoles;
                if (sysRoles.Count > 0)
                {
                    List<SysRoleAuthorize> roleAuthorizes = SysRoleAuthorizeServices.QuerySysRoleAuthorizeByRoleIds(sysRoles.Select(p => p.RecordID).ToList());
                    if (roleAuthorizes != null)
                    {
                      
                        HttpContext.Current.Session["SmartSystem_LoginUser_SysRoleAuthorize"] = roleAuthorizes;

                    }
                }
            }
        }
        public static void UpdateCacheUserLoginData(string userId) {
            SysUser user = SysUserServices.QuerySysUserByRecordId(userId);
            CacheUserLoginData(user);
        }
    }
}