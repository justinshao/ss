using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Common.Entities;
using Common.Services;

namespace SmartSystem.OmnipotentCardWebUI.Controllers
{
    public class VillageDataController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public string CreateVillageTreeOnLineStatus()
        {
            try
            {
                StringBuilder str = new StringBuilder();

                if (GetLoginUserVillages.Count == 0)
                    return str.ToString();

                List<BaseCompany> companys = GetLoginUserRoleCompany;
                BaseCompany topCompany = companys.FirstOrDefault(p => p.CPID == GetCurrentUserCompanyId);
                if (topCompany == null) return str.ToString();

                str.Append("[");
                str.Append("{\"id\":\"" + topCompany.CPID + "\",");
                str.Append("\"iconCls\":\"my-company-icon\",");
                str.Append("\"attributes\":{\"type\":0},");
                str.Append("\"text\":\"" + topCompany.CPName + "\"");
                CreateSubordinateVillageTreeDataOnlineStatus(companys, topCompany.CPID, str);
                str.Append("}");
                str.Append("]");
                return str.ToString();
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "构建小区树结构失败");
                return string.Empty;
            }
        }
        private void CreateSubordinateVillageTreeDataOnlineStatus(List<BaseCompany> companys, string masterId, StringBuilder str)
        {
            List<BaseVillage> villages = GetLoginUserVillages.Where(p => p.CPID == masterId).ToList();
            List<BaseCompany> subCompanys = companys.Where(p => p.MasterID == masterId).ToList();

            str.Append(",\"children\":[");
            int i = 1;
            foreach (var item in subCompanys)
            {
                str.Append("{");
                str.AppendFormat("\"id\":\"{0}\"", item.CPID);
                
                str.Append(",\"iconCls\":\"my-company-icon\"");
                str.AppendFormat(",\"text\":\"{0}\"", item.CPName);
                str.Append(",\"attributes\":{\"type\":0}");
                CreateSubordinateVillageTreeDataOnlineStatus(companys, item.CPID, str);
                str.Append("}");
                if (i != subCompanys.Count())
                {
                    str.Append(",");
                }
                i++;
            }
            if (subCompanys.Count > 0 && villages.Count > 0)
            {
                str.Append(",");
            }
            int index = 1;
            foreach (var item in villages)
            {
                str.Append("{");
                str.AppendFormat("\"id\":\"{0}\"", item.VID);
                if (item.IsOnLine == 1)
                {
                    str.Append(",\"iconCls\":\"icon-status-yes\"");
                }
                else
                {
                    str.Append(",\"iconCls\":\"icon-status-no\"");
                }
                str.AppendFormat(",\"text\":\"{0}\"", item.VName);
                str.Append(",\"attributes\":{\"type\":1,\"CompanyID\":\"" + item.CPID + "\"}");
                str.Append("}");
                if (index != villages.Count())
                {
                    str.Append(",");
                }
                index++;
            }
            str.Append("]");
        }
        public string CreateVillageTreeData()
        {
            try
            {
                StringBuilder str = new StringBuilder();

                if (GetLoginUserVillages.Count == 0)
                    return str.ToString();

                List<BaseCompany> companys = GetLoginUserRoleCompany;
                BaseCompany topCompany = companys.FirstOrDefault(p => p.CPID == GetCurrentUserCompanyId);
                if (topCompany == null) return str.ToString();

                str.Append("[");
                str.Append("{\"id\":\"" + topCompany.CPID + "\",");
                str.Append("\"iconCls\":\"my-company-icon\",");
                str.Append("\"attributes\":{\"type\":0},");
                str.Append("\"text\":\"" + topCompany.CPName + "\"");
                CreateSubordinateVillageTreeData(companys, topCompany.CPID, str);
                str.Append("}");
                str.Append("]");
                return str.ToString();
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "构建小区树结构失败");
                return string.Empty;
            }
        }
        public string CreateLoginUserSubVillageTreeData()
        {
            try
            {
                StringBuilder str = new StringBuilder();

                List<BaseCompany> companys =CompanyServices.QueryCompanyAndSubordinateCompany(GetCurrentUserCompanyId);
                if (companys.Count == 0) return str.ToString();

               
                BaseCompany topCompany = companys.FirstOrDefault(p => p.CPID == GetCurrentUserCompanyId);
                 List<BaseVillage> villages = VillageServices.QueryVillageByCompanyIds(companys.Select(p => p.CPID).ToList());
                 villages = villages.Where(p => p.CPID != topCompany.CPID).ToList();

                if (topCompany == null) return str.ToString();

                str.Append("[");
                str.Append("{\"id\":\"" + topCompany.CPID + "\",");
                str.Append("\"iconCls\":\"my-company-icon\",");
                str.Append("\"attributes\":{\"type\":0},");
                str.Append("\"text\":\"" + topCompany.CPName + "\"");
                CreateSubordinateVillageTreeData(companys,villages,topCompany.CPID, str);
                str.Append("}");
                str.Append("]");
                return str.ToString();
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "构建小区树结构失败");
                return string.Empty;
            }
        }
        private void CreateSubordinateVillageTreeData(List<BaseCompany> companys,List<BaseVillage> villages,string masterId, StringBuilder str)
        {
            List<BaseVillage> companyVillages = villages.Where(p => p.CPID == masterId).ToList();
            List<BaseCompany> subCompanys = companys.Where(p => p.MasterID == masterId).ToList();

            str.Append(",\"children\":[");
            int i = 1;
            foreach (var item in subCompanys)
            {
                str.Append("{");
                str.AppendFormat("\"id\":\"{0}\"", item.CPID);
                str.Append(",\"iconCls\":\"my-company-icon\"");
                str.AppendFormat(",\"text\":\"{0}\"", item.CPName);
                str.Append(",\"attributes\":{\"type\":0}");
                CreateSubordinateVillageTreeData(companys, villages,item.CPID, str);
                str.Append("}");
                if (i != subCompanys.Count())
                {
                    str.Append(",");
                }
                i++;
            }
            if (companyVillages.Count > 0 && subCompanys.Count > 0)
            {
                str.Append(",");
            }
            int index = 1;
            foreach (var item in companyVillages)
            {
                str.Append("{");
                str.AppendFormat("\"id\":\"{0}\"", item.VID);
                str.Append(",\"iconCls\":\"my-village-icon\"");
                str.AppendFormat(",\"text\":\"{0}\"", item.VName);
                str.Append(",\"attributes\":{\"type\":1,\"CompanyID\":\"" + item.CPID + "\"}");
                str.Append("}");
                if (index != companyVillages.Count())
                {
                    str.Append(",");
                }
                index++;
            }
            str.Append("]");
        }
        private void CreateSubordinateVillageTreeData(List<BaseCompany> companys, string masterId, StringBuilder str)
        {
            List<BaseVillage> villages = GetLoginUserVillages.Where(p => p.CPID == masterId).ToList();
            List<BaseCompany> subCompanys = companys.Where(p => p.MasterID == masterId).ToList();

            str.Append(",\"children\":[");
            int i = 1;
            foreach (var item in subCompanys)
            {
                str.Append("{");
                str.AppendFormat("\"id\":\"{0}\"", item.CPID);
                str.Append(",\"iconCls\":\"my-company-icon\"");
                str.AppendFormat(",\"text\":\"{0}\"", item.CPName);
                str.Append(",\"attributes\":{\"type\":0}");
                CreateSubordinateVillageTreeData(companys, item.CPID, str);
                str.Append("}");
                if (i != subCompanys.Count())
                {
                    str.Append(",");
                }
                i++;
            }
            if (villages.Count > 0 && subCompanys.Count > 0)
            {
                str.Append(",");
            }
            int index = 1;
            foreach (var item in villages)
            {
                str.Append("{");
                str.AppendFormat("\"id\":\"{0}\"", item.VID);
                str.Append(",\"iconCls\":\"my-village-icon\"");
                str.AppendFormat(",\"text\":\"{0}\"", item.VName);
                str.Append(",\"attributes\":{\"type\":1,\"CompanyID\":\"" + item.CPID + "\"}");
                str.Append("}");
                if (index != villages.Count())
                {
                    str.Append(",");
                }
                index++;
            }
            str.Append("]");
        }

        public string CreateSelectVillageTreeData(bool needShowDefault = false, string defaultText = "不限")
        {
            StringBuilder str = new StringBuilder();
            try
            {
                str.Append("[");
                if (needShowDefault)
                {
                    str.Append("{\"id\":\"\",");
                    str.Append("\"attributes\":{\"type\":1},");
                    str.Append("\"text\":\"" + defaultText + "\"");
                    str.Append("}");
                    if (GetLoginUserVillages.Count > 0)
                    {
                        str.Append(",");
                    }
                }
                int index = 1;
                foreach (var item in GetLoginUserVillages)
                {
                    BaseCompany company = GetLoginUserRoleCompany.FirstOrDefault(p => p.CPID == item.CPID);
                    if (company == null)
                    {
                        index++;
                        continue;
                    }
                    str.Append("{\"id\":\"" + item.VID + "\",");
                    str.Append("\"attributes\":{\"type\":1},");
                    str.Append("\"iconCls\":\"my-village-icon\",");
                    str.AppendFormat("\"text\":\"{0}\"", string.Format("{0}【{1}】", item.VName, company.CPName));
                    str.Append("}");
                    if (index != GetLoginUserVillages.Count())
                    {
                        str.Append(",");
                    }
                    index++;
                }
                str.Append("]");
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "构建小区下拉结构树失败");
            }
            return str.ToString();
        }
    }
}
