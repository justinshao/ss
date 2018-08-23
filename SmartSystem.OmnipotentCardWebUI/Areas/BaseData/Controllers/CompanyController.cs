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

namespace SmartSystem.OmnipotentCardWebUI.Areas.BaseData.Controllers
{
    [CheckPurview(Roles = "PK010101")]
    public class CompanyController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.CurrUserCompayId = GetCurrentUserCompanyId;
            return View();
        }
        public string GetCompanyData() {
           
            List<BaseCompany> companys = CompanyServices.QueryCompanyAndSubordinateCompany(GetCurrentUserCompanyId);

            BaseCompany currCompany = companys.FirstOrDefault(p => p.CPID == GetCurrentUserCompanyId);
            if (currCompany == null) return string.Empty;

            List<BaseCity> citys = CityServices.QueryAllCitys();

            StringBuilder strTree = new StringBuilder();
            strTree.Append("{\"rows\":[{");
            strTree.AppendFormat("\"CPID\":\"{0}\"", currCompany.CPID);
            strTree.AppendFormat(",\"CPName\":\"{0}\"", currCompany.CPName);
            strTree.AppendFormat(",\"Address\":\"{0}\"", currCompany.Address);
            strTree.AppendFormat(",\"LinkMan\":\"{0}\"", currCompany.LinkMan);
            strTree.AppendFormat(",\"Mobile\":\"{0}\"", currCompany.Mobile);
            strTree.AppendFormat(",\"MasterID\":\"{0}\"", currCompany.MasterID);
            strTree.AppendFormat(",\"CityID\":\"{0}\"", currCompany.CityID);
            strTree.AppendFormat(",\"ProvinceID\":\"{0}\"", "0");
            strTree.Append(",\"iconCls\":\"my-company-icon\"}");
            GetSubordinateCompany(companys, citys, currCompany.CPID, strTree);
            strTree.Append("]}");
            return strTree.ToString();
        }
        public string Search_Company()
        {
            String str = Request.Params["str"];
            // List<BaseCompany> companys = CompanyServices.QueryCompanyAndSubordinateCompany(GetCurrentUserCompanyId); 
            List<BaseCompany> companys = CompanyServices.QueryAllCompanyByName(GetCurrentUserCompanyId, str);
            BaseCompany currCompany = companys.FirstOrDefault(p => p.CPID == GetCurrentUserCompanyId);
            if (currCompany == null) return string.Empty;

            List<BaseCity> citys = CityServices.QueryAllCitys();

            StringBuilder strTree = new StringBuilder();
            strTree.Append("{\"rows\":[{");
            strTree.AppendFormat("\"CPID\":\"{0}\"", currCompany.CPID);
            strTree.AppendFormat(",\"CPName\":\"{0}\"", currCompany.CPName);
            strTree.AppendFormat(",\"Address\":\"{0}\"", currCompany.Address);
            strTree.AppendFormat(",\"LinkMan\":\"{0}\"", currCompany.LinkMan);
            strTree.AppendFormat(",\"Mobile\":\"{0}\"", currCompany.Mobile);
            strTree.AppendFormat(",\"MasterID\":\"{0}\"", currCompany.MasterID);
            strTree.AppendFormat(",\"CityID\":\"{0}\"", currCompany.CityID);
            strTree.AppendFormat(",\"ProvinceID\":\"{0}\"", "0");
            strTree.Append(",\"iconCls\":\"my-company-icon\"}");
            GetSubordinateCompany(companys, citys, currCompany.CPID, strTree);
            strTree.Append("]}");
            return strTree.ToString();
        }
        private void GetSubordinateCompany(List<BaseCompany> companys,List<BaseCity> citys,string masterId, StringBuilder strTree) {
            List<BaseCompany> subordinateCompanys = companys.Where(p => p.MasterID == masterId).ToList();
            foreach (var item in subordinateCompanys) {
                strTree.Append(",{");
                strTree.AppendFormat("\"CPID\":\"{0}\"", item.CPID);
                strTree.AppendFormat(",\"CPName\":\"{0}\"", item.CPName);
                strTree.AppendFormat(",\"Address\":\"{0}\"", item.Address);
                strTree.AppendFormat(",\"LinkMan\":\"{0}\"", item.LinkMan);
                strTree.AppendFormat(",\"Mobile\":\"{0}\"", item.Mobile);
                strTree.AppendFormat(",\"MasterID\":\"{0}\"", item.MasterID);
                strTree.AppendFormat(",\"CityID\":\"{0}\"", item.CityID);
                strTree.AppendFormat(",\"ProvinceID\":\"{0}\"", "0");
                strTree.AppendFormat(",\"_parentId\":\"{0}\"", masterId);
                strTree.Append(",\"iconCls\":\"my-company-icon\"}");
                GetSubordinateCompany(companys,citys,item.CPID,strTree);
            }
        }

        [HttpPost]
        [CheckPurview(Roles = "PK01010101,PK01010102")]
        public JsonResult EditCompany(BaseCompany model)
        {
            try
            {
                bool result = false;
                if (string.IsNullOrWhiteSpace(model.CPID))
                {
                    result = CompanyServices.Add(model);
                }
                else {
                    BaseCompany company = CompanyServices.QueryCompanyByRecordId(model.CPID);
                    if (string.IsNullOrWhiteSpace(company.MasterID) && !string.IsNullOrWhiteSpace(model.MasterID)) {
                        throw new MyException("当前修改单位为顶级单位，不能属于任何单位");
                    }
                    result = CompanyServices.Update(model);
                }
                if (!result) throw new MyException("保存单位信息失败");
                UpdateCompanyCacheData(model);
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "单位管理保存单位信息失败");
                return Json(MyResult.Error("保存单位信息失败"));
            }
        }
        private void UpdateCompanyCacheData(BaseCompany model)
        {
            if (model.CPID == GetCurrentUserCompanyId) {
                Session["SmartCity_CurrLoginUser_Role_Company"] = model;
            }

            if (GetLoginUserRoleCompany.FirstOrDefault(p => p.CPID == model.CPID) != null) {
                GetLoginUserRoleCompany.Remove(GetLoginUserRoleCompany.FirstOrDefault(p => p.CPID == model.CPID));
                GetLoginUserRoleCompany.Add(model);
            }
        }
        /// <summary>
        /// 删除公司信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPurview(Roles = "PK01010103")]
        public JsonResult Delete(string companyId)
        {
            try
            {
                List<BaseCompany> companys = CompanyServices.QueryCompanysByMasterID(companyId);
                if (companys.Count > 0) throw new MyException("请先删除下属单位");

                bool result = CompanyServices.Delete(companyId);
                if (!result) throw new MyException("删除下属单位失败");

                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除单位失败");
                return Json(MyResult.Error("删除失败"));
            }
        }
        /// <summary>
        /// 获取单位操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCompanyOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010101").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01010101":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "添加";
                            option.handler = "Add";
                            option.sort = 1;
                            options.Add(option);
                            break;
                        }
                    case "PK01010102":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "Update";
                            option.sort = 2;
                            options.Add(option);
                            break;
                        }
                    case "PK01010103":
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
  
            SystemOperatePurview rtoolbar = new SystemOperatePurview();
            rtoolbar.text = "刷新";
            rtoolbar.handler = "Refresh";
            rtoolbar.sort = 4;
            options.Add(rtoolbar);

            result.Data = options.OrderBy(p => p.sort);
            return result;
        }
    }
}
