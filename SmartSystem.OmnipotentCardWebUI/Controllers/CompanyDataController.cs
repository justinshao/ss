using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Entities;
using Common.Services;
using System.Text;

namespace SmartSystem.OmnipotentCardWebUI.Controllers
{
    public class CompanyDataController : BaseController
    {
        public string GetCompanyTree()
        {
            List<BaseCompany> subordinateCompanys = CompanyServices.QueryCompanyAndSubordinateCompany(GetCurrentUserCompanyId);
            BaseCompany loginCompany = subordinateCompanys.FirstOrDefault(p => p.CPID == GetCurrentUserCompanyId);
            List<BaseCompany> validCompanys = new List<BaseCompany>();
            bool needShowSubComapny = true;
            if (!string.IsNullOrWhiteSpace(Request["excludeCompanyId"]))
            {
                if (Request["excludeCompanyId"] == GetCurrentUserCompanyId)
                {
                    needShowSubComapny = false;
                    if (loginCompany != null && !string.IsNullOrWhiteSpace(loginCompany.MasterID))
                    {
                        BaseCompany parntCompany = CompanyServices.QueryCompanyByRecordId(loginCompany.MasterID);
                        if (parntCompany != null) {
                            validCompanys.Add(parntCompany);
                        }
                      
                    }
                }
                else
                {
                    List<BaseCompany> excludeCompanys = CompanyServices.QueryCompanyAndSubordinateCompany(Request["excludeCompanyId"].ToString());
                    if (excludeCompanys != null && excludeCompanys.Count > 0)
                    {
                        subordinateCompanys = subordinateCompanys.Where(p => !excludeCompanys.Select(c => c.CPID).Contains(p.CPID)).ToList();
                    }
                    validCompanys = subordinateCompanys.Where(p => p.CPID == GetCurrentUserCompanyId).ToList();
                }

            }
            else {
                validCompanys = subordinateCompanys.Where(p => p.CPID == GetCurrentUserCompanyId).ToList();
            }
          
            StringBuilder strTree = new StringBuilder();
            strTree.Append("[");
            int index = 1;
            foreach (var item in validCompanys)
            {
                strTree.Append("{");
                strTree.AppendFormat("\"id\":\"{0}\",", item.CPID);
                strTree.Append("\"iconCls\":\"my-company-icon\",");
                strTree.AppendFormat("\"text\":\"{0}\"", item.CPName);
                if (needShowSubComapny) {
                    GetSubordinateCompany(subordinateCompanys, item.CPID, strTree);
                }
                
                strTree.Append("}");
                if (index != validCompanys.Count)
                {
                    strTree.Append(",");
                }
                index++;
            }
            strTree.Append("]");
            return strTree.ToString();
        }
        private void GetSubordinateCompany(List<BaseCompany> subordinateCompanys, string masterId, StringBuilder strTree)
        {
            List<BaseCompany> childs = subordinateCompanys.Where(p => p.MasterID == masterId).ToList();
            if (childs.Count == 0) return;

            strTree.Append(",\"children\":[");
            int index = 1;
            foreach (var item in childs)
            {
                strTree.Append("{");
                strTree.AppendFormat("\"id\":\"{0}\",", item.CPID);
                strTree.Append("\"iconCls\":\"my-company-icon\",");
                strTree.AppendFormat("\"text\":\"{0}\"", item.CPName);
                GetSubordinateCompany(subordinateCompanys, item.CPID, strTree);
                strTree.Append("}");
                if (index != childs.Count)
                {
                    strTree.Append(",");
                }
                index++;
            }
            strTree.Append("]");
        }
    }
}
