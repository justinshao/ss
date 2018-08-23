using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Common.Services;
using Common.Entities;

namespace SmartSystem.OmnipotentCardWebUI.Controllers
{
    public class ParkingDataController : BaseController
    {
        public string GetParkingTreeData()
        {
            try
            {
                StringBuilder strParkingTree = new StringBuilder();
                if (GetLoginUserVillages.Count == 0)
                    return strParkingTree.ToString();

                List<BaseParkinfo> parkings = ParkingServices.QueryParkingByVillageIds(GetLoginUserVillages.Select(p => p.VID).ToList());
                strParkingTree.Append("[");
                int index = 1;
                foreach (var obj in GetLoginUserRoleCompany)
                {

                    //List<BaseVillage> childVillages = GetLoginUserVillages.Where(p => p.CPID == obj.CPID).ToList();
                    //if (childVillages.Count == 0) continue;

                    //List<BaseParkinfo> childParkings = parkings.Where(p => childVillages.Select(o => o.VID).Contains(p.VID)).ToList();
                    //if (childParkings.Count == 0) continue;
                    //if (index != 1)
                    //{
                    //    strParkingTree.Append(",");
                    //}
                    if (index != 1)
                    {
                        strParkingTree.Append(",");
                    }
                    strParkingTree.Append("{\"id\":\"" + obj.CPID + "\",");
                    strParkingTree.Append("\"iconCls\":\"my-company-icon\",");
                    strParkingTree.Append("\"attributes\":{\"type\":0},");
                    strParkingTree.Append("\"text\":\"" + obj.CPName + "\"");
                    GetVillageTreeData(obj.CPID, parkings, strParkingTree);
                    strParkingTree.Append("}");
                    index++;
                }
                strParkingTree.Append("]");
                return strParkingTree.ToString();
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "构建车场树异常");
                return string.Empty;
            }
        }
        private void GetVillageTreeData(string companyId, List<BaseParkinfo> parkings, StringBuilder strParkingTree)
        {
            List<BaseVillage> villages = GetLoginUserVillages.Where(p => p.CPID == companyId).ToList();
            if (villages.Count == 0) return;

            strParkingTree.Append(",\"children\":[");
            int i = 1;
            foreach (var item in villages)
            {
                strParkingTree.Append("{");
                strParkingTree.AppendFormat("\"id\":\"{0}\"", item.VID);
                strParkingTree.AppendFormat(",\"text\":\"{0}\"", item.VName);
                strParkingTree.Append(",\"iconCls\":\"my-village-icon\"");
                strParkingTree.Append(",\"attributes\":{\"type\":1}");
                GetParkingTreeData(companyId, item.VID, parkings, strParkingTree);
                strParkingTree.Append("}");
                if (i != villages.Count())
                {
                    strParkingTree.Append(",");
                }
                i++;
            }
            strParkingTree.Append("]");
        }
        private void GetParkingTreeData(string companyId, string villageId, List<BaseParkinfo> parkings, StringBuilder strParkingTree)
        {
            List<BaseParkinfo> childParkings = parkings.Where(p => p.VID == villageId).ToList();
            if (childParkings.Count == 0) return;

            strParkingTree.Append(",\"children\":[");
            int i = 1;
            foreach (var item in childParkings)
            {
                strParkingTree.Append("{");
                strParkingTree.AppendFormat("\"id\":\"{0}\"", item.PKID);
                strParkingTree.Append(",\"iconCls\":\"my-parking-icon\"");
                strParkingTree.AppendFormat(",\"text\":\"{0}\"", item.PKName);
                strParkingTree.Append(",\"attributes\":{\"type\":2,\"companyId\":\"" + companyId + "\"}");
                strParkingTree.Append("}");
                if (i != childParkings.Count())
                {
                    strParkingTree.Append(",");
                }
                i++;
            }
            strParkingTree.Append("]");
        }
        public string GetOnlyParkingTreeData()
        {
            try
            {
                StringBuilder strParkingTree = new StringBuilder();
                if (GetLoginUserVillages.Count == 0)
                    return strParkingTree.ToString();

                List<BaseParkinfo> parkings = ParkingServices.QueryParkingByVillageIds(GetLoginUserVillages.Select(p => p.VID).ToList());
                strParkingTree.Append("[");
                int index = 1;
                foreach (var item in parkings)
                {

                    strParkingTree.Append("{");
                    strParkingTree.AppendFormat("\"id\":\"{0}\"", item.PKID);
                    strParkingTree.Append(",\"iconCls\":\"my-parking-icon\"");
                    strParkingTree.AppendFormat(",\"text\":\"{0}\"", item.PKName);
                    strParkingTree.Append(",\"attributes\":{\"type\":1}");
                    strParkingTree.Append("}");
                    if (index != parkings.Count())
                    {
                        strParkingTree.Append(",");
                    }
                    index++;
                }
                strParkingTree.Append("]");
                return strParkingTree.ToString();
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "构建车场树异常");
                return string.Empty;
            }
        }
    }
}
