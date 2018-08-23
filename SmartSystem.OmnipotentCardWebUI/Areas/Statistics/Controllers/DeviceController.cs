using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Common.Entities.Parking;
using Common.Utilities;
using Common.Utilities.Helpers;
namespace SmartSystem.OmnipotentCardWebUI.Areas.Statistics.Controllers
{
    public class DeviceController : Controller
    {
        [CheckPurview(Roles = "PK010412")]
        public ActionResult Index()
        {
            return View();
        }

        public string GetDeviceDetectionData()
        {
            try
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                string villageid = Request.Params["villageid"];
                if (string.IsNullOrEmpty(villageid))
                    return string.Empty;
                List<ParkDevice> parkdevicelist = Common.Services.ParkDeviceServices.QueryParkDeviceDetectionByParkingID(villageid);

                List<string> parkinglist = parkdevicelist.Select(u => u.PKID ).Distinct().ToList();
                string json = "{\"rows\":[";
                if (parkinglist != null && parkinglist.Count > 0)
                {
                    foreach (string parkingid in parkinglist)
                    {
                        string parkingname = parkdevicelist.Find(u => u.PKID == parkingid).ParkingName;
                        List<ParkDevice> devicelist = parkdevicelist.FindAll(u => u.PKID == parkingid);
                        if (devicelist != null && devicelist.Count > 0)
                        {
                            json += "{\"DeviceName\":\"" + parkingname + "\",\"DeviceType\":\"\",\"IsOnLine\":\"\",\"HasChildMenu\":\"1\",\"iconCls\":\"my-parking-icon\",\"id\":\"" + parkingid + "\",\"MasterID\":\"\"}";
                            foreach (var parkdevice in devicelist)
                            {
                                string tempdevicetypename = string.Format("[{0}][{1}][{2}({3})]", parkdevice.BoxName, parkdevice.GateName, parkdevice.DeviceTypeName, parkdevice.IpAddr);
                                if (parkdevice.ConnectionStateName=="连接")
                                {
                                    json += ",{\"DeviceName\":\"" + tempdevicetypename + "\",\"IsOnLine\":\"连接正常\",\"HasChildMenu\":\"0\",\"iconCls\":\"icon-status-yes\",\"id\":\"" + parkdevice.ID + "\",\"_parentId\":\"" + parkingid + "\"}";
                                }
                                else
                                {
                                    json += ",{\"DeviceName\":\"" + tempdevicetypename + "\",\"IsOnLine\":\"连接异常\",\"HasChildMenu\":\"0\",\"iconCls\":\"icon-status-no\",\"id\":\"" + parkdevice.ID + "\",\"_parentId\":\"" + parkingid + "\"}";
                                }
                            }
                        }
                    }
                }
                json += "]}";
                return json;
            }
            catch
            {
                return "{\"rows\":[]}";
            }
        }

        /// <summary>
        /// 获取在场车辆
        /// </summary>
        /// <returns></returns>
        public string Search_DeviceDetection()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string parkingid = Request.Params["parkingid"];
            if (string.IsNullOrEmpty(parkingid))
                return string.Empty;
            List<ParkDevice> parkdevicelist = Common.Services.ParkDeviceServices.QueryParkDeviceDetectionByParkingID(parkingid);
            string str = JsonHelper.GetJsonString(parkdevicelist);
            sb.Append("{");
            sb.Append("\"total\":0,");
            sb.Append("\"rows\":" + str + ",");
            sb.Append("\"index\":0");
            sb.Append("}");
            return sb.ToString();
        }
    }
}
