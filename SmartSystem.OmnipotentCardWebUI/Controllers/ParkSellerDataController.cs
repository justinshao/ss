using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Common.Entities;
using Common.Services;
using Common.Services.Park;

namespace SmartSystem.OmnipotentCardWebUI.Controllers
{
    public class ParkSellerDataController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public string GetSellerTreeData()
        {
          
            try
            {
                StringBuilder strTree = new StringBuilder();

                if (GetLoginUserVillages.Count == 0)
                    return strTree.ToString();

                List<ParkSeller> sellers = ParkSellerServices.QueryByVillageIds(GetLoginUserVillages.Select(p => p.VID).ToList());
                BaseCompany currCompany = GetLoginUserRoleCompany.FirstOrDefault(p => p.CPID == GetCurrentUserCompanyId);
                if (currCompany == null) return string.Empty;
                strTree.Append("[{\"id\":\"" + currCompany.CPID + "\",");
                strTree.Append("\"iconCls\":\"my-company-icon\",");
                strTree.Append("\"attributes\":{\"type\":0},");
                strTree.Append("\"text\":\"" + currCompany.CPName + "\"");
                GetVillageTree(sellers,currCompany.CPID, strTree);
                strTree.Append("}]");
                return strTree.ToString();
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "构建商家树结构失败");
                return string.Empty;
            }
        }
        private void GetVillageTree(List<ParkSeller> sellers, string companyId, StringBuilder strTree)
        {
            List<BaseVillage> villages = GetLoginUserVillages.Where(p => p.CPID == companyId).ToList();
            List<BaseCompany> companys = GetLoginUserRoleCompany.Where(p => p.MasterID == companyId).ToList();

            if (villages.Count == 0 && companys.Count == 0) return;

            strTree.Append(",\"children\":[");
            int i = 1;
            foreach (var item in companys)
            {
                strTree.Append("{");
                strTree.AppendFormat("\"id\":\"{0}\"", item.CPID);
                strTree.Append(",\"iconCls\":\"my-company-icon\"");
                strTree.AppendFormat(",\"text\":\"{0}\"", item.CPName);
                strTree.Append(",\"attributes\":{\"type\":0}");
                GetVillageTree(sellers, item.CPID, strTree);
                strTree.Append("}");
                if (i != companys.Count())
                {
                    strTree.Append(",");
                }
                i++;
            }
            if (companys.Count > 0 && villages.Count > 0)
            {
                strTree.Append(",");
            }
            int index = 1;
            foreach (var item in villages)
            {
                strTree.Append("{");
                strTree.AppendFormat("\"id\":\"{0}\"", item.VID);
                strTree.Append(",\"iconCls\":\"my-village-icon\"");
                strTree.AppendFormat(",\"text\":\"{0}\"", item.VName);
                strTree.Append(",\"attributes\":{\"type\":0,\"CompanyID\":\"" + item.CPID + "\"}");
                GetParkSellerTree(item.VID, sellers, strTree);
                strTree.Append("}");
                if (index != villages.Count())
                {
                    strTree.Append(",");
                }
                index++;
            }
            strTree.Append("]");
        }
        private void GetParkSellerTree(string villageId, List<ParkSeller> sellers, StringBuilder strTree)
        {
            List<ParkSeller> villageSellers = sellers.Where(p => p.VID == villageId).ToList();
            if (villageSellers.Count == 0) return;

            strTree.Append(",\"children\":[");
            int index = 1;
            foreach (var item in villageSellers)
            {
                strTree.Append("{");
                strTree.AppendFormat("\"id\":\"{0}\"", item.SellerID);
                strTree.Append(",\"iconCls\":\"my-seller-icon\"");
                strTree.AppendFormat(",\"text\":\"{0}\"", item.SellerName);
                strTree.Append(",\"attributes\":{\"type\":1,\"VillageId\":\"" + item.VID + "\"}");
                strTree.Append("}");
                if (index != villageSellers.Count())
                {
                    strTree.Append(",");
                }
                index++;
            }
            strTree.Append("]");
        }
    }
}
