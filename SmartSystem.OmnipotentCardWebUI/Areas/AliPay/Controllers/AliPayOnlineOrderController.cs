using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using SmartSystem.WeiXinServices;
using Common.Entities.Order;
using Common.Entities.Enum;
using Common.Utilities.Helpers;
using Common.Services;
using Common.Entities.Condition;
using Common.Utilities;
using Common.Entities;
using SmartSystem.OmnipotentCardWebUI.Models;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using Common.Entities.BWY;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;

namespace SmartSystem.OmnipotentCardWebUI.Areas.AliPay.Controllers
{
    [CheckPurview(Roles = "PK011002")]
    public class AliPayOnlineOrderController : BaseController
    {
        public ActionResult Index()
        {

            ViewBag.DefaultStartDate = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.DefaultEndDate = DateTime.Now.AddDays(1).Date.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.NeedShowBWYParking = !string.IsNullOrWhiteSpace(SystemDefaultConfig.BWPKID);
            return View();
        }
        public string GetOnlineOrderData()
        {
            StringBuilder strData = new StringBuilder();
            try
            {
                if (string.IsNullOrWhiteSpace(Request.Params["Query"]))
                {
                    return strData.ToString();
                }
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                List<BWYGateMapping> gates = BWYGateMappingServices.QueryAll();
                int total = 0;
                List<OnlineOrder> result = OnlineOrderServices.QueryPage(GetOnlineOrderCondition(), page, rows, out total);
                var obj = from p in result
                          select new
                          {
                              OrderID = p.OrderID.ToString(),
                              PKName = p.PKName,
                              PlateNo = p.PlateNo,
                              Amount = p.Amount,
                              MonthNum = p.OrderType == OnlineOrderType.MonthCardRecharge ? p.MonthNum.ToString() : string.Empty,
                              PayerNickName = p.PayerNickName,
                              SyncResultTimes = p.SyncResultTimes,
                              LastSyncResultTime = p.LastSyncResultTime != DateTime.MinValue ? p.LastSyncResultTime.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                              RefundOrderId = p.RefundOrderId,
                              Remark = p.Remark,
                              OrderType = (int)p.OrderType,
                              OrderTypeDes = p.OrderType.GetDescription(),
                              Status = (int)p.Status,
                              StatusDes = p.Status.GetDescription(),
                              OrderTime = p.OrderTime.ToString("yyyy-MM-dd HH:mm:ss"),
                              RealPayTime = p.RealPayTime != DateTime.MinValue ? p.RealPayTime.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                              BWYParkingName = GetBWYParkingName(p.ExternalPKID, gates)
                          };
                strData.Append("{");
                strData.Append("\"total\":" + total + ",");
                strData.Append("\"rows\":" + JsonHelper.GetJsonString(obj) + ",");
                strData.Append("\"index\":" + page);
                strData.Append("}");
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "查询微信订单信息失败");
            }

            return strData.ToString();
        }
        private string GetBWYParkingName(string parkingId, List<BWYGateMapping> gates)
        {
            if (string.IsNullOrWhiteSpace(parkingId))
            {
                return string.Empty;
            }
            BWYGateMapping model = gates.FirstOrDefault(p => p.ParkingID == parkingId);
            if (model != null)
            {
                return model.ParkingName;
            }
            return string.Empty;
        }
        private OnlineOrderCondition GetOnlineOrderCondition()
        {
            DateTime start = DateTime.Now.Date;
            DateTime sTime;
            if (!string.IsNullOrWhiteSpace(Request.Params["StartTime"]) && DateTime.TryParse(Request.Params["StartTime"], out sTime))
            {
                start = sTime;
            }
            DateTime end = DateTime.Now;
            DateTime eTime;
            if (!string.IsNullOrWhiteSpace(Request.Params["EndTime"]) && DateTime.TryParse(Request.Params["EndTime"], out eTime))
            {
                end = eTime;
            }
            OnlineOrderCondition condition = new OnlineOrderCondition();
            if (string.IsNullOrWhiteSpace(Request.Params["CompanyId"]))
                throw new MyException("获取单位编号失败");

            condition.CompanyId = Request.Params["CompanyId"].ToString();
            if (!string.IsNullOrWhiteSpace(Request.Params["ParkingId"]) && Request.Params["ParkingId"].ToString() != "-1")
            {
                condition.ParkingId = Request.Params["ParkingId"].ToString();
            }
            if (!string.IsNullOrWhiteSpace(Request.Params["ExternalPKID"]) && Request.Params["ExternalPKID"].ToString() != "-1")
            {
                condition.ExternalPKID = Request.Params["ExternalPKID"].ToString();
            }
            condition.PaymentChannel = PaymentChannel.AliPay;
            condition.Start = start;
            condition.End = end;
            string messgage = string.Empty;
            if (!string.IsNullOrWhiteSpace(Request.Params["OrderId"]))
            {
                condition.OrderId = Request.Params["OrderId"].ToString();
            }
            if (!string.IsNullOrWhiteSpace(Request.Params["PlateNo"]))
            {
                condition.PlateNo = Request.Params["PlateNo"].ToString();
            }
            if (!string.IsNullOrWhiteSpace(Request.Params["Status"]) && Request.Params["Status"].ToString() != "-1")
            {
                condition.Status = (OnlineOrderStatus)int.Parse(Request.Params["Status"].ToString());
            }
            if (!string.IsNullOrWhiteSpace(Request.Params["OrderType"]) && Request.Params["OrderType"].ToString() != "-1")
            {
                condition.Ordertype = (OnlineOrderType)int.Parse(Request.Params["OrderType"].ToString());
            }
            return condition;
        }
        public string GetOrderStatus()
        {
            StringBuilder strTree = new StringBuilder();
            strTree.Append("[");
            strTree.Append("{\"id\":\"-1\",");
            strTree.Append("\"text\":\"所有\",\"selected\":true");
            strTree.Append("}");
            List<EnumContext> enums = EnumHelper.GetEnumContextList(typeof(OnlineOrderStatus));
            foreach (var item in enums)
            {
                strTree.Append(",{\"id\":\"" + item.EnumValue + "\",");
                strTree.Append("\"text\":\"" + item.Description + "\"");
                strTree.Append("}");
            }
            strTree.Append("]");
            return strTree.ToString();
        }
        public string GetOrderType()
        {
            StringBuilder strTree = new StringBuilder();
            strTree.Append("[");
            strTree.Append("{\"id\":\"-1\",");
            strTree.Append("\"text\":\"所有\",\"selected\":true");
            strTree.Append("}");
            List<EnumContext> enums = EnumHelper.GetEnumContextList(typeof(OnlineOrderType));
            foreach (var item in enums)
            {
                strTree.Append(",{\"id\":\"" + item.EnumValue + "\",");
                strTree.Append("\"text\":\"" + item.Description + "\"");
                strTree.Append("}");
            }
            strTree.Append("]");
            return strTree.ToString();
        }
        public string GetParkingData(string companyId)
        {
            List<BaseParkinfo> parkings = new List<BaseParkinfo>();
            List<BaseCompany> companys = CompanyServices.GetCurrLoginUserRoleCompany(companyId, GetLoginUser.RecordID);
            if (companys.Count > 0)
            {
                parkings = ParkingServices.QueryParkingByCompanyIds(companys.Select(p => p.CPID).ToList());
            }
            StringBuilder strTree = new StringBuilder();
            strTree.Append("[");
            strTree.Append("{\"id\":\"-1\",");
            strTree.Append("\"text\":\"所有\",\"selected\":true");
            strTree.Append("}");
            foreach (var item in parkings)
            {
                strTree.Append(",{\"id\":\"" + item.PKID + "\",");
                strTree.Append("\"text\":\"" + item.PKName + "\"");
                strTree.Append("}");
            }
            strTree.Append("]");
            return strTree.ToString();
        }
        public string GetExternalParkingData()
        {
            Dictionary<string, string> parkings = new Dictionary<string, string>();
            List<BWYGateMapping> gates = BWYGateMappingServices.QueryAll();
            foreach (var item in gates)
            {
                if (!parkings.ContainsKey(item.ParkingID))
                {
                    parkings.Add(item.ParkingID, item.ParkingName);
                }
            }
            StringBuilder strTree = new StringBuilder();
            strTree.Append("[");
            strTree.Append("{\"id\":\"-1\",");
            strTree.Append("\"text\":\"所有\",\"selected\":true");
            strTree.Append("}");
            foreach (var item in parkings)
            {
                strTree.Append(",{\"id\":\"" + item.Key + "\",");
                strTree.Append("\"text\":\"" + item.Value + "\"");
                strTree.Append("}");
            }
            strTree.Append("]");
            return strTree.ToString();
        }
        /// <summary>
        /// 同步支付结果
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [CheckPurview(Roles = "PK01100201")]
        public JsonResult SynchAliPayPaymentResult(decimal orderId)
        {
            try
            {
                bool result = OnlineOrderServices.SyncPaymentResult(orderId);
                if (!result) throw new MyException("同步失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "同步支付结果失败");
                return Json(MyResult.Error("同步支付结果失败"));
            }
        }
        /// <summary>
        /// 手动退款
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [CheckPurview(Roles = "PK01100202")]
        public JsonResult ManualRefund(decimal orderId)
        {
            try
            {
                bool result = OnlineOrderServices.ManualRefund(orderId, Server.MapPath("~"));
                if (!result) throw new MyException("退款失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "手动退款失败");
                return Json(MyResult.Error("手动退款失败"));
            }
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        public ActionResult Export()
        {
            List<BWYGateMapping> gates = BWYGateMappingServices.QueryAll();
            List<OnlineOrder> onlineOrder = OnlineOrderServices.ExportQueryPage(GetOnlineOrderCondition());
            var result = from p in onlineOrder
                         select new
                         {
                             订单编号 = p.OrderID.ToString(),
                             车场名称 = p.PKName,
                             车牌号 = p.PlateNo,
                             支付金额 = p.Amount,
                             续期月数 = p.OrderType == OnlineOrderType.MonthCardRecharge ? p.MonthNum.ToString() : string.Empty,
                             支付人 = p.PayerNickName,
                             同步支付次数 = p.SyncResultTimes,
                             最后同步时间 = p.LastSyncResultTime != DateTime.MinValue ? p.LastSyncResultTime.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                             退款订单号 = p.RefundOrderId,
                             订单类型 = p.OrderType.GetDescription(),
                             订单状态 = p.Status.GetDescription(),
                             订单时间 = p.OrderTime.ToString("yyyy-MM-dd HH:mm:ss"),
                             支付时间 = p.RealPayTime != DateTime.MinValue ? p.RealPayTime.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                             外部车场名称 = GetBWYParkingName(p.ExternalPKID, gates),
                             备注 = p.Remark
                         };
            StringBuilder sb = new StringBuilder();
            sb.Append(JsonHelper.GetJsonString(result));
            var dt = JsonToDataTable(sb.ToString());
            var dl = DownLoadExcel(dt);
            return dl;
        }
        public static DataTable JsonToDataTable(string strJson)
        {
            ////取出表名  
            //Regex rg = new Regex(@"(?<={)[^:]+(?=:/[)", RegexOptions.IgnoreCase);
            //string strName = rg.Match(strJson).Value;
            DataTable tb = null;
            ////去除表名  
            //strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            //strJson = strJson.Substring(0, strJson.IndexOf("]"));

            //获取数据  
            Regex rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split(',');

                //创建表  
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = "";
                    foreach (string str in strRows)
                    {
                        DataColumn dc = new DataColumn();
                        string[] strCell = str.Split(':');

                        dc.ColumnName = strCell[0].ToString().Replace("\"", "").Trim();
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }

                //增加内容  
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    if (r == 7 || r == 11 || r == 12)
                    {
                        //时间列
                        if (strRows[r].Split(':')[1].Trim().Replace("\"", "").Trim() != "")
                        {
                            dr[r] = strRows[r].Split(':')[1].Trim().Replace("\"", "").Trim() + ":" + strRows[r].Split(':')[2].Trim().Replace("\"", "").Trim() + ":" + strRows[r].Split(':')[3].Trim().Replace("\"", "").Trim();
                        }
                        else
                        {
                            dr[r] = strRows[r].Split(':')[1].Trim().Replace("\"", "").Trim();
                        }
                    }
                    else
                    {
                        //非时间列
                        if (strRows[r].Split(':')[1].Trim().Replace("\"", "").Trim() == "null")
                        {
                            dr[r] = "";
                        }
                        else
                        {
                            dr[r] = strRows[r].Split(':')[1].Trim().Replace("\"", "").Trim();
                        }
                    }
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }

            return tb;
        }
        public ActionResult DownLoadExcel(DataTable dt)
        {
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("支付宝订单信息");
            NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                row.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                NPOI.SS.UserModel.IRow row2 = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                    row2.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
            }
            string pathToFiles = Server.MapPath("/");
            string filename = DateTime.Now.ToString("yyyyMMddhhmmss") + "支付宝订单信息.xls";
            string path = @"" + pathToFiles + "\\" + filename;

            //写入到客户端
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                book.Write(ms);

                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
                book = null;

            }
            return Content("/" + filename);//返回文件名提供下载  
        }

        /// <summary>
        /// 获取线上订单操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOnlieOrderOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK011002").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01100201":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.id = "btnpayresult";
                            option.iconCls = "icon-edit";
                            option.text = "同步支付结果";
                            option.handler = "SynchPaymentResult";
                            option.sort = 0;
                            options.Add(option);
                            break;
                        }
                    case "PK01100202":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.id = "btnmanualrefund";
                            option.iconCls = "icon-edit";
                            option.text = "手动退款";
                            option.handler = "ManualRefund";
                            option.sort = 1;
                            options.Add(option);
                            break;
                        }
                }
            }
            SystemOperatePurview roption = new SystemOperatePurview();
            roption.text = "刷新";
            roption.handler = "Refresh";
            roption.sort = 3;
            options.Add(roption);

            SystemOperatePurview roption2 = new SystemOperatePurview();
            roption2.id = "btnexport";
            roption2.text = "导出";
            roption2.handler = "Export";
            roption2.iconCls = "icon-print";
            roption2.sort = 12;
            options.Add(roption2);

            result.Data = options.OrderBy(p => p.sort);
            return result;
        }
    }
}
