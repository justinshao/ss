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
    public class ParkBoxDataController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public string GetParkBoxTreeData()
        {
            try
            {
                StringBuilder strBoxTree = new StringBuilder();
                List<BaseParkinfo> parkings = ParkingServices.QueryParkingByVillageIds(GetLoginUserVillages.Select(p => p.VID).ToList());
                if (parkings.Count == 0) return string.Empty;
                List<ParkArea> parkAreas = ParkAreaServices.GetParkAreaByParkingIds(parkings.Select(p => p.PKID).ToList());
                List<ParkBox> parkBoxs = ParkBoxServices.QueryByParkAreaIds(parkAreas.Select(p => p.AreaID).ToList());
                strBoxTree.Append("[");
                int index = 1;

                foreach (var item in GetLoginUserVillages)
                {
                    BaseCompany company = GetLoginUserRoleCompany.FirstOrDefault(p => p.CPID == item.CPID);
                    if (company == null) continue;

                    string text = string.Format("{0}【{1}】", item.VName, company.CPName);
                    strBoxTree.Append("{\"id\":\"" + item.VID + "\",");
                    strBoxTree.Append("\"attributes\":{\"type\":0},");
                    strBoxTree.Append("\"iconCls\":\"my-village-icon\",");
                    strBoxTree.Append("\"text\":\"" + text + "\"");
                    GetParkingTreeData(parkings, parkAreas, item.VID, parkBoxs, strBoxTree);
                    strBoxTree.Append("}");
                    if (index != GetLoginUserVillages.Count)
                    {
                        strBoxTree.Append(",");
                    }
                    index++;
                }

                strBoxTree.Append("]");
                return strBoxTree.ToString();
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "构建岗亭树结构失败");
                return string.Empty;
            }
        }
        private void GetParkingTreeData(List<BaseParkinfo> parkings, List<ParkArea> parkAreas, string villageId, List<ParkBox> parkBoxs, StringBuilder strBoxTree)
        {
            List<BaseParkinfo> villageParkings = parkings.Where(p => p.VID == villageId).ToList();
            if (villageParkings.Count == 0) return;
        
            int index = 1;
            strBoxTree.Append(",\"children\":[");
            foreach (var item in villageParkings)
            {
                strBoxTree.Append("{\"id\":\"" + item.PKID + "\",");
                strBoxTree.Append("\"iconCls\":\"my-parking-icon\",");
                strBoxTree.Append("\"attributes\":{\"type\":0},");
                strBoxTree.Append("\"text\":\"" + item.PKName + "\"");

                List<ParkArea> topParkAreas = parkAreas.Where(p => p.PKID == item.PKID && string.IsNullOrWhiteSpace(p.MasterID)).ToList();
                if (topParkAreas.Count == 0)
                {
                    strBoxTree.Append("}");
                    if (index != villageParkings.Count)
                    {
                        strBoxTree.Append(",");
                    }
                    index++;
                    continue;
                }

                int i = 1;
                strBoxTree.Append(",\"children\":[");
                foreach (var area in topParkAreas)
                {
                    strBoxTree.Append("{\"id\":\"" + area.AreaID + "\",");
                    strBoxTree.Append("\"iconCls\":\"my-pkarea-icon\",");
                    strBoxTree.Append("\"attributes\":{\"type\":0,\"parkName\":\"" + item.PKName + "\",\"parkingId\":\"" + item.PKID + "\"},");
                    strBoxTree.Append("\"text\":\"" + area.AreaName + "\"");

                    List<ParkArea> childParkAreas = parkAreas.Where(p => p.MasterID == area.AreaID).ToList();
                    List<ParkBox> areaParkBoxs = parkBoxs.Where(p => p.AreaID == area.AreaID).ToList();
                    if (areaParkBoxs.Count > 0 || childParkAreas.Count > 0)
                    {
                        strBoxTree.Append(",\"children\":[");
                        int j = 1;
                        foreach (var box in areaParkBoxs)
                        {
                            strBoxTree.Append("{\"id\":\"" + box.BoxID + "\",");
                            strBoxTree.Append("\"iconCls\":\"my-box-icon\",");
                            strBoxTree.Append("\"attributes\":{\"type\":1,\"parkingId\":\"" + item.PKID + "\"},");
                            strBoxTree.Append("\"text\":\"" + box.BoxName + "\"");
                            strBoxTree.Append("}");
                            if (j != areaParkBoxs.Count)
                            {
                                strBoxTree.Append(",");
                            }
                            j++;
                        }
                        if (areaParkBoxs.Count > 0 && childParkAreas.Count > 0)
                        {
                            strBoxTree.Append(",");
                        }
                        int areaIndex = 1;
                        foreach (var child in childParkAreas)
                        {
                            strBoxTree.Append("{\"id\":\"" + child.AreaID + "\",");
                            strBoxTree.Append("\"iconCls\":\"my-pkarea-icon\",");
                            strBoxTree.Append("\"attributes\":{\"type\":0,\"parkName\":\"" + item.PKName + "\",\"parkingId\":\"" + item.PKID + "\"},");
                            strBoxTree.Append("\"text\":\"" + child.AreaName + "\"");

                            List<ParkBox> areaBoxs = parkBoxs.Where(p => p.AreaID == child.AreaID).ToList();
                            if (areaBoxs.Count > 0)
                            {
                                strBoxTree.Append(",\"children\":[");
                                int g = 1;
                                foreach (var box in areaBoxs)
                                {
                                    strBoxTree.Append("{\"id\":\"" + box.BoxID + "\",");
                                    strBoxTree.Append("\"iconCls\":\"my-box-icon\",");
                                    strBoxTree.Append("\"attributes\":{\"type\":1,\"parkingId\":\"" + item.PKID + "\"},");
                                    strBoxTree.Append("\"text\":\"" + box.BoxName + "\"");
                                    strBoxTree.Append("}");
                                    if (g != areaBoxs.Count)
                                    {
                                        strBoxTree.Append(",");
                                    }
                                    g++;
                                }
                                strBoxTree.Append("]");
                            }
                            strBoxTree.Append("}");
                            if (areaIndex != childParkAreas.Count)
                            {
                                strBoxTree.Append(",");
                            }
                            areaIndex++;
                        }
                        strBoxTree.Append("]");
                    }
                    strBoxTree.Append("}");
                    if (i != topParkAreas.Count)
                    {
                        strBoxTree.Append(",");
                    }
                    i++;
                }
                strBoxTree.Append("]}");
                if (index != villageParkings.Count)
                {
                    strBoxTree.Append(",");
                }
                index++;
            }
            strBoxTree.Append("]");
        }
       
    }
}
