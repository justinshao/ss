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
    public class PrakAreaDataController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public string GetPrakAreaTreeData()
        {

            try
            {
                StringBuilder strAreaTree = new StringBuilder();
                List<BaseParkinfo> parkings = ParkingServices.QueryParkingByVillageIds(GetLoginUserVillages.Select(p => p.VID).ToList());
                if (parkings.Count == 0) return string.Empty;

                List<ParkArea> parkAreas = ParkAreaServices.GetParkAreaByParkingIds(parkings.Select(p => p.PKID).ToList());

                strAreaTree.Append("[");
                int index = 1;
                foreach (var item in GetLoginUserVillages)
                {
                    BaseCompany company = GetLoginUserRoleCompany.FirstOrDefault(p => p.CPID == item.CPID);
                    if (company == null) continue;

                    string text = string.Format("{0}【{1}】", item.VName, company.CPName);
                    strAreaTree.Append("{\"id\":\"" + item.VID + "\",");
                    strAreaTree.Append("\"iconCls\":\"my-village-icon\",");
                    strAreaTree.Append("\"attributes\":{\"type\":0},");
                    strAreaTree.Append("\"text\":\"" + text + "\"");
                    GetParkingTreeData(parkings, parkAreas, item.VID, strAreaTree);
                    strAreaTree.Append("}");
                    if (index != GetLoginUserVillages.Count)
                    {
                        strAreaTree.Append(",");
                    }
                    index++;
                }

                strAreaTree.Append("]");
                return strAreaTree.ToString();
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "构建车场区域树失败");
                return string.Empty;
            }
        }
        private void GetParkingTreeData(List<BaseParkinfo> parkings, List<ParkArea> parkAreas, string villageId, StringBuilder strAreaTree)
        {
            List<BaseParkinfo> villageParkings = parkings.Where(p => p.VID == villageId).ToList();
            if (villageParkings.Count == 0) return;

            int index = 1;
            strAreaTree.Append(",\"children\":[");
            foreach (var item in villageParkings)
            {
                strAreaTree.Append("{\"id\":\"" + item.PKID + "\",");
                strAreaTree.Append("\"iconCls\":\"my-parking-icon\",");
                strAreaTree.Append("\"attributes\":{\"type\":0},");
                strAreaTree.Append("\"text\":\"" + item.PKName + "\"");

                List<ParkArea> topParkAreas = parkAreas.Where(p => p.PKID == item.PKID && string.IsNullOrWhiteSpace(p.MasterID)).ToList();
                if (topParkAreas.Count == 0)
                {
                    strAreaTree.Append("}");
                    if (index != villageParkings.Count)
                    {
                        strAreaTree.Append(",");
                    }
                    index++;
                    continue;
                }

                int i = 1;
                strAreaTree.Append(",\"children\":[");
                foreach (var area in topParkAreas)
                {
                    int type = parkAreas.Count(p => p.MasterID == area.AreaID) > 0 ? 0 : 1;
                    strAreaTree.Append("{\"id\":\"" + area.AreaID + "\",");
                    strAreaTree.Append("\"iconCls\":\"my-pkarea-icon\",");
                    strAreaTree.Append("\"attributes\":{\"type\":1,\"parkname\":\"" + item.PKName + "\",\"parkingid\":\"" + item.PKID + "\"},");
                    strAreaTree.Append("\"text\":\"" + area.AreaName + "\"");
                    GetParkAreaTreeData(parkAreas, area.AreaID, item.PKName, item.PKID, strAreaTree);
                    strAreaTree.Append("}");
                    if (i != topParkAreas.Count)
                    {
                        strAreaTree.Append(",");
                    }
                    i++;
                }
                strAreaTree.Append("]}");
                if (index != villageParkings.Count)
                {
                    strAreaTree.Append(",");
                }
                index++;
            }
            strAreaTree.Append("]");
        }
        private void GetParkAreaTreeData(List<ParkArea> parkAreas, string masterId, string parkName, string parkingid, StringBuilder strAreaTree)
        {
            List<ParkArea> childPKAreas = parkAreas.Where(p => p.MasterID == masterId).ToList();
            if (childPKAreas.Count == 0) return;
           
            strAreaTree.Append(",\"children\":[");
            int index = 1;
            foreach (var item in childPKAreas)
            {
                strAreaTree.Append("{\"id\":\"" + item.AreaID + "\",");
                strAreaTree.Append("\"iconCls\":\"my-pkarea-icon\",");
                strAreaTree.Append("\"attributes\":{\"type\":1,\"parkname\":\"" + parkName + "\",\"parkingid\":\"" + parkingid + "\"},");
                strAreaTree.Append("\"text\":\"" + item.AreaName + "\"");
                GetParkAreaTreeData(parkAreas, item.AreaID, parkName, parkingid, strAreaTree);
                strAreaTree.Append("}");
                if (index != childPKAreas.Count)
                {
                    strAreaTree.Append(",");
                }
                index++;
            }
            strAreaTree.Append("]");
        }
    }
}
